#include "SpSession.h"

#include "Base64.h"

#include <cstring>
#include <thread>

namespace sim {

namespace {

constexpr int kProtocolVersion = 1;

UINT MessageFor(const std::string& kind) {
    if (kind == "execute") return WFS_EXECUTE_COMPLETE;
    if (kind == "getinfo") return WFS_GETINFO_COMPLETE;
    if (kind == "close") return WFS_CLOSE_COMPLETE;
    return 0;
}

// Window message and registration class of a non-execute event kind.
bool EventRouteFor(const std::string& kind, UINT& message, DWORD& eventClass) {
    if (kind == "service") { message = WFS_SERVICE_EVENT; eventClass = SERVICE_EVENTS; return true; }
    if (kind == "user") { message = WFS_USER_EVENT; eventClass = USER_EVENTS; return true; }
    if (kind == "system") { message = WFS_SYSTEM_EVENT; eventClass = SYSTEM_EVENTS; return true; }
    return false;
}

void FillVersion(LPWFSVERSION version, WORD current, WORD low, WORD high) {
    if (version == nullptr) return;
    version->wVersion = current;
    version->wLowVersion = low;
    version->wHighVersion = high;
    strcpy_s(version->szDescription, sizeof(version->szDescription), "DenoXfs Simulator");
    version->szSystemStatus[0] = '\0';
}

std::string MachineName() {
    char name[MAX_COMPUTERNAME_LENGTH + 1] = {};
    DWORD size = sizeof(name);
    ::GetComputerNameA(name, &size);
    return name;
}

} // namespace

SpSession::SpSession(HSERVICE service, SessionDependencies dependencies)
    : service_(service), deps_(std::move(dependencies)) {}

HRESULT SpSession::Open(const OpenArguments& args, LPWFSVERSION spiVersion, LPWFSVERSION serviceVersion) {
    if (!deps_.connection->Connect(deps_.hostUrl)) {
        deps_.log->error("open failed hService={} logical={} reason=host-unreachable url={}", service_, args.logicalName, deps_.hostUrl);
        return WFS_ERR_CONNECTION_LOST;
    }

    nlohmann::json hello = {
        {"type", "hello"}, {"protocol", kProtocolVersion}, {"machine", MachineName()},
        {"pid", ::GetCurrentProcessId()}, {"logicalName", args.logicalName}, {"appId", args.appId},
        {"traceLevel", args.traceLevel}, {"spiVersionsRequired", args.spiVersionsRequired},
        {"srvcVersionsRequired", args.srvcVersionsRequired}};
    std::string text;
    if (!SendJson(hello) || !deps_.connection->Receive(text)) {
        deps_.connection->Close();
        deps_.log->error("open failed hService={} logical={} reason=no-hello-ack", service_, args.logicalName);
        return WFS_ERR_CONNECTION_LOST;
    }

    nlohmann::json ack = nlohmann::json::parse(text, nullptr, false);
    if (ack.is_discarded() || ack.value("type", "") != "helloAck") {
        deps_.connection->Close();
        deps_.log->error("open failed hService={} logical={} reason=bad-hello-ack", service_, args.logicalName);
        return WFS_ERR_INTERNAL_ERROR;
    }

    FillVersion(spiVersion, ack.value("spiVersion", 0), ack.value("spiLow", 0), ack.value("spiHigh", 0));
    FillVersion(serviceVersion, ack.value("srvcVersion", 0), ack.value("srvcLow", 0), ack.value("srvcHigh", 0));

    const HRESULT hResult = ack.value("hResult", static_cast<int>(WFS_ERR_INTERNAL_ERROR));
    if (hResult != WFS_SUCCESS) {
        deps_.connection->Close();
        deps_.log->warn("open rejected hService={} logical={} hResult={}", service_, args.logicalName, hResult);
        return hResult;
    }

    connected_ = true;
    deps_.log->info("open hService={} logical={} appId={} spi=0x{:04X} service=0x{:04X}", service_, args.logicalName,
                    args.appId, ack.value("spiVersion", 0), ack.value("srvcVersion", 0));

    std::thread([self = shared_from_this()] { self->ReadLoop(); }).detach();
    CompleteLocally(WFS_OPEN_COMPLETE, args.window, args.requestId);
    return WFS_SUCCESS;
}

HRESULT SpSession::Execute(DWORD command, LPVOID data, DWORD timeoutMs, HWND window, REQUESTID requestId) {
    nlohmann::json input = nlohmann::json::object();
    for (const auto& decoder : *deps_.decoders) {
        if (decoder->CanDecode(command)) { input = decoder->Decode(data); break; }
    }
    return Forward("execute", "command", command, input, timeoutMs, window, requestId);
}

HRESULT SpSession::GetInfo(DWORD category, LPVOID query, DWORD timeoutMs, HWND window, REQUESTID requestId) {
    nlohmann::json input = nlohmann::json::object();
    for (const auto& decoder : *deps_.infoDecoders) {
        if (decoder->CanDecode(category)) { input = decoder->Decode(query); break; }
    }
    return Forward("getinfo", "category", category, input, timeoutMs, window, requestId);
}

HRESULT SpSession::Close(HWND window, REQUESTID requestId) {
    if (!connected_) {
        CompleteLocally(WFS_CLOSE_COMPLETE, window, requestId);
        End();
        return WFS_SUCCESS;
    }
    return Forward("close", "code", 0, nlohmann::json::object(), 0, window, requestId);
}

HRESULT SpSession::Register(DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId) {
    registrations_.Register(registeredWindow, eventClasses);
    deps_.log->info("register hService={} classes={} window={:p}", service_, eventClasses, static_cast<void*>(registeredWindow));
    return CompleteLocally(WFS_REGISTER_COMPLETE, window, requestId);
}

HRESULT SpSession::Deregister(DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId) {
    registrations_.Deregister(registeredWindow, eventClasses);
    deps_.log->info("deregister hService={} classes={} window={:p}", service_, eventClasses, static_cast<void*>(registeredWindow));
    return CompleteLocally(WFS_DEREGISTER_COMPLETE, window, requestId);
}

HRESULT SpSession::Cancel(REQUESTID requestId) {
    if (!connected_) return WFS_ERR_CONNECTION_LOST;
    if (requestId != 0 && !pending_.Find(requestId)) {
        deps_.log->warn("cancel rejected hService={} reqId={} reason=unknown-request", service_, requestId);
        return WFS_ERR_INVALID_REQ_ID;
    }

    deps_.log->info("cancel hService={} reqId={}", service_, requestId);
    if (!SendJson({{"type", "cancel"}, {"reqId", requestId}})) return WFS_ERR_CONNECTION_LOST;
    return WFS_SUCCESS;
}

HRESULT SpSession::CompleteLocally(UINT message, HWND window, REQUESTID requestId) {
    LPWFSRESULT result = deps_.materializer->Create(service_, requestId, WFS_SUCCESS, 0, nullptr);
    if (result == nullptr) return WFS_ERR_INTERNAL_ERROR;
    if (!deps_.poster->Post(window, message, reinterpret_cast<LPARAM>(result))) {
        deps_.log->warn("post failed message=0x{:X} reqId={} reason=window", message, requestId);
        deps_.materializer->Release(result);
        return WFS_ERR_INTERNAL_ERROR;
    }
    return WFS_SUCCESS;
}

bool SpSession::SendJson(const nlohmann::json& message) {
    const std::string text = message.dump();
    std::lock_guard<std::mutex> lock(sendMutex_);
    return deps_.connection->Send(text);
}

HRESULT SpSession::Forward(const std::string& kind, const char* codeField, DWORD code, const nlohmann::json& input,
                           DWORD timeoutMs, HWND window, REQUESTID requestId) {
    if (!connected_) return WFS_ERR_CONNECTION_LOST;

    pending_.Add({requestId, window, kind, code});
    nlohmann::json message = {{"type", kind}, {"reqId", requestId}, {codeField, code}, {"timeoutMs", timeoutMs}, {"input", input}};
    deps_.log->info("{} hService={} reqId={} code={} input={}", kind, service_, requestId, code, input.dump());
    if (!SendJson(message)) {
        pending_.Take(requestId);
        connected_ = false;
        return WFS_ERR_CONNECTION_LOST;
    }
    return WFS_SUCCESS;
}

void SpSession::ReadLoop() {
    std::string text;
    while (deps_.connection->Receive(text)) {
        nlohmann::json message = nlohmann::json::parse(text, nullptr, false);
        if (message.is_discarded()) {
            deps_.log->warn("host message ignored reason=invalid-json hService={}", service_);
            continue;
        }
        HandleMessage(message);
    }

    if (!ended_) {
        connected_ = false;
        deps_.log->warn("host connection lost hService={}", service_);
        FailAllPending(WFS_ERR_CONNECTION_LOST);
    }
}

void SpSession::HandleMessage(const nlohmann::json& message) {
    const std::string type = message.value("type", "");
    if (type == "completion") {
        HandleCompletion(message);
    } else if (type == "event") {
        HandleEvent(message);
    } else {
        deps_.log->warn("host message ignored reason=unknown-type type={}", type);
    }
}

bool SpSession::ReadBlock(const nlohmann::json& message, ResultBlock& block) {
    if (!message.contains("result") || !message["result"].is_object()) return false;

    const auto& result = message["result"];
    block.ptrSize = result.value("ptrSize", 4);
    if (!Base64Decode(result.value("data", ""), block.data)) return false;
    for (const auto& fixup : result.value("fixups", nlohmann::json::array()))
        if (fixup.is_array() && fixup.size() == 2) block.fixups.emplace_back(fixup[0].get<uint32_t>(), fixup[1].get<uint32_t>());
    return true;
}

void SpSession::HandleCompletion(const nlohmann::json& message) {
    const REQUESTID requestId = message.value("reqId", 0u);
    const auto pending = pending_.Take(requestId);
    if (!pending) {
        deps_.log->warn("completion ignored reason=unknown-request reqId={}", requestId);
        return;
    }

    ResultBlock block;
    const bool hasBlock = ReadBlock(message, block);
    Complete(*pending, message.value("hResult", static_cast<int>(WFS_ERR_INTERNAL_ERROR)), hasBlock ? &block : nullptr);
}

void SpSession::HandleEvent(const nlohmann::json& message) {
    const std::string kind = message.value("kind", "");
    const DWORD eventId = message.value("eventId", 0u);
    const REQUESTID requestId = message.value("reqId", 0u);
    const HRESULT hResult = message.value("hResult", static_cast<int>(WFS_SUCCESS));

    // Execute events go to the window of their request; the other classes to every window registered for them.
    UINT windowMessage = WFS_EXECUTE_EVENT;
    std::vector<HWND> windows;
    if (kind == "execute") {
        const auto pending = pending_.Find(requestId);
        if (!pending) {
            deps_.log->warn("event ignored reason=unknown-request kind={} eventId={} reqId={}", kind, eventId, requestId);
            return;
        }
        windows.push_back(pending->window);
    } else {
        DWORD eventClass = 0;
        if (!EventRouteFor(kind, windowMessage, eventClass)) {
            deps_.log->warn("event ignored reason=unknown-kind kind={} eventId={}", kind, eventId);
            return;
        }
        windows = registrations_.WindowsFor(eventClass);
    }

    ResultBlock block;
    const bool hasBlock = ReadBlock(message, block);
    for (HWND window : windows) {
        // One WFSRESULT per recipient window; the application frees it with WFSFreeResult.
        LPWFSRESULT result = deps_.materializer->Create(service_, requestId, hResult, eventId, hasBlock ? &block : nullptr);
        if (result == nullptr) continue;
        if (!deps_.poster->Post(window, windowMessage, reinterpret_cast<LPARAM>(result))) {
            deps_.log->warn("event post failed kind={} eventId={} window={:p}", kind, eventId, static_cast<void*>(window));
            deps_.materializer->Release(result);
        }
    }
    deps_.log->info("event kind={} eventId={} reqId={} windows={} body={}", kind, eventId, requestId, windows.size(), hasBlock);
}

void SpSession::Complete(const PendingRequest& request, HRESULT hResult, const ResultBlock* block) {
    LPWFSRESULT result = deps_.materializer->Create(service_, request.id, hResult, request.code, block);
    if (result == nullptr)
        result = deps_.materializer->Create(service_, request.id, WFS_ERR_INTERNAL_ERROR, request.code, nullptr);

    if (result != nullptr) {
        const UINT message = MessageFor(request.kind);
        if (message == 0 || !deps_.poster->Post(request.window, message, reinterpret_cast<LPARAM>(result))) {
            // Never delivered: nobody else will free it.
            deps_.log->warn("post failed kind={} reqId={} reason=window-or-kind", request.kind, request.id);
            deps_.materializer->Release(result);
        } else {
            deps_.log->info("completion posted kind={} reqId={} hResult={} body={}", request.kind, request.id, hResult, block != nullptr);
        }
    }

    if (request.kind == "close") End();
}

void SpSession::FailAllPending(HRESULT hResult) {
    for (const auto& request : pending_.TakeAll()) Complete(request, hResult, nullptr);
}

void SpSession::End() {
    if (ended_.exchange(true)) return;
    connected_ = false;
    deps_.connection->Close();
    if (deps_.onEnded) deps_.onEnded(service_);
}

} // namespace sim
