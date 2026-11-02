#pragma once

#include "EventRegistrations.h"
#include "Interfaces.h"
#include "PendingRequests.h"

#include <spdlog/spdlog.h>

#include <atomic>
#include <functional>
#include <mutex>

namespace sim {

struct OpenArguments {
    std::string logicalName;
    std::string appId;
    DWORD traceLevel = 0;
    DWORD spiVersionsRequired = 0;
    DWORD srvcVersionsRequired = 0;
    HWND window = nullptr;
    REQUESTID requestId = 0;
};

struct SessionDependencies {
    std::unique_ptr<IHostConnection> connection;
    std::shared_ptr<IResultMaterializer> materializer;
    std::shared_ptr<IMessagePoster> poster;
    std::shared_ptr<const std::vector<std::shared_ptr<ICommandInputDecoder>>> decoders;     // WFPExecute lpCmdData
    std::shared_ptr<const std::vector<std::shared_ptr<ICommandInputDecoder>>> infoDecoders; // WFPGetInfo lpQueryDetails
    std::shared_ptr<spdlog::logger> log;
    std::string hostUrl;
    std::function<void(HSERVICE)> onEnded; // called once the session is over (close completed)
};

// One Service Provider session (one WFPOpen): forwards requests to the host and posts its completions to the
// application's window. A reader thread (detached, owns a reference to the session) receives the host messages.
class SpSession : public std::enable_shared_from_this<SpSession> {
public:
    SpSession(HSERVICE service, SessionDependencies dependencies);

    HRESULT Open(const OpenArguments& arguments, LPWFSVERSION spiVersion, LPWFSVERSION serviceVersion);
    HRESULT Execute(DWORD command, LPVOID data, DWORD timeoutMs, HWND window, REQUESTID requestId);
    HRESULT GetInfo(DWORD category, LPVOID query, DWORD timeoutMs, HWND window, REQUESTID requestId);
    HRESULT Close(HWND window, REQUESTID requestId);
    // WFPCancelAsyncRequest: asks the host to end the request (0 = all) with WFS_ERR_CANCELED.
    HRESULT Cancel(REQUESTID requestId);
    // Register and deregister are kept here (the host sends events, this session routes them to the registered windows).
    HRESULT Register(DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId);
    HRESULT Deregister(DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId);
    // Lock and unlock are completed here without the host (spike behaviour).
    HRESULT CompleteLocally(UINT message, HWND window, REQUESTID requestId);

private:
    bool SendJson(const nlohmann::json& message);
    HRESULT Forward(const std::string& kind, const char* codeField, DWORD code, const nlohmann::json& input, DWORD timeoutMs, HWND window, REQUESTID requestId);
    void ReadLoop();
    void HandleMessage(const nlohmann::json& message);
    void HandleCompletion(const nlohmann::json& message);
    void HandleEvent(const nlohmann::json& message);
    static bool ReadBlock(const nlohmann::json& message, ResultBlock& block);
    void Complete(const PendingRequest& request, HRESULT hResult, const ResultBlock* block);
    void FailAllPending(HRESULT hResult);
    void End();

    HSERVICE service_;
    SessionDependencies deps_;
    PendingRequests pending_;
    EventRegistrations registrations_;
    std::mutex sendMutex_;
    std::atomic<bool> connected_{false};
    std::atomic<bool> ended_{false};
};

} // namespace sim
