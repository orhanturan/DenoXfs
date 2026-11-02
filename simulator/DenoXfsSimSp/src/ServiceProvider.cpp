#include "ServiceProvider.h"

namespace sim {

ServiceProvider::ServiceProvider(std::shared_ptr<ISessionFactory> factory, std::shared_ptr<spdlog::logger> log)
    : factory_(std::move(factory)), log_(std::move(log)) {}

HRESULT ServiceProvider::Open(HSERVICE service, const OpenArguments& arguments, LPWFSVERSION spiVersion, LPWFSVERSION serviceVersion) {
    auto session = factory_->Create(service, [this](HSERVICE ended) { Remove(ended); });
    {
        std::lock_guard<std::mutex> lock(mutex_);
        sessions_[service] = session;
    }

    const HRESULT result = session->Open(arguments, spiVersion, serviceVersion);
    if (result != WFS_SUCCESS) Remove(service);
    return result;
}

HRESULT ServiceProvider::Close(HSERVICE service, HWND window, REQUESTID requestId) {
    auto session = Find(service);
    return session ? session->Close(window, requestId) : WFS_ERR_INVALID_HSERVICE;
}

HRESULT ServiceProvider::Execute(HSERVICE service, DWORD command, LPVOID data, DWORD timeoutMs, HWND window, REQUESTID requestId) {
    auto session = Find(service);
    return session ? session->Execute(command, data, timeoutMs, window, requestId) : WFS_ERR_INVALID_HSERVICE;
}

HRESULT ServiceProvider::GetInfo(HSERVICE service, DWORD category, LPVOID query, DWORD timeoutMs, HWND window, REQUESTID requestId) {
    auto session = Find(service);
    return session ? session->GetInfo(category, query, timeoutMs, window, requestId) : WFS_ERR_INVALID_HSERVICE;
}

HRESULT ServiceProvider::Cancel(HSERVICE service, REQUESTID requestId) {
    auto session = Find(service);
    return session ? session->Cancel(requestId) : WFS_ERR_INVALID_HSERVICE;
}

HRESULT ServiceProvider::Register(HSERVICE service, DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId) {
    auto session = Find(service);
    return session ? session->Register(eventClasses, registeredWindow, window, requestId) : WFS_ERR_INVALID_HSERVICE;
}

HRESULT ServiceProvider::Deregister(HSERVICE service, DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId) {
    auto session = Find(service);
    return session ? session->Deregister(eventClasses, registeredWindow, window, requestId) : WFS_ERR_INVALID_HSERVICE;
}

HRESULT ServiceProvider::CompleteLocally(HSERVICE service, UINT message, HWND window, REQUESTID requestId) {
    auto session = Find(service);
    return session ? session->CompleteLocally(message, window, requestId) : WFS_ERR_INVALID_HSERVICE;
}

std::shared_ptr<SpSession> ServiceProvider::Find(HSERVICE service) {
    std::lock_guard<std::mutex> lock(mutex_);
    const auto found = sessions_.find(service);
    return found == sessions_.end() ? nullptr : found->second;
}

void ServiceProvider::Remove(HSERVICE service) {
    std::lock_guard<std::mutex> lock(mutex_);
    sessions_.erase(service);
}

} // namespace sim
