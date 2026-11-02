#pragma once

#include "SpSession.h"

#include <map>
#include <mutex>

namespace sim {

// Creates a session with all its dependencies (the object graph below one WFPOpen).
class ISessionFactory {
public:
    virtual ~ISessionFactory() = default;
    virtual std::shared_ptr<SpSession> Create(HSERVICE service, std::function<void(HSERVICE)> onEnded) = 0;
};

// Use cases behind the exported WFP* functions: finds the session of an hService and delegates to it.
class ServiceProvider {
public:
    ServiceProvider(std::shared_ptr<ISessionFactory> factory, std::shared_ptr<spdlog::logger> log);

    HRESULT Open(HSERVICE service, const OpenArguments& arguments, LPWFSVERSION spiVersion, LPWFSVERSION serviceVersion);
    HRESULT Close(HSERVICE service, HWND window, REQUESTID requestId);
    HRESULT Execute(HSERVICE service, DWORD command, LPVOID data, DWORD timeoutMs, HWND window, REQUESTID requestId);
    HRESULT GetInfo(HSERVICE service, DWORD category, LPVOID query, DWORD timeoutMs, HWND window, REQUESTID requestId);
    HRESULT Cancel(HSERVICE service, REQUESTID requestId);
    HRESULT Register(HSERVICE service, DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId);
    HRESULT Deregister(HSERVICE service, DWORD eventClasses, HWND registeredWindow, HWND window, REQUESTID requestId);
    HRESULT CompleteLocally(HSERVICE service, UINT message, HWND window, REQUESTID requestId);

private:
    std::shared_ptr<SpSession> Find(HSERVICE service);
    void Remove(HSERVICE service);

    std::shared_ptr<ISessionFactory> factory_;
    std::shared_ptr<spdlog::logger> log_;
    std::mutex mutex_;
    std::map<HSERVICE, std::shared_ptr<SpSession>> sessions_;
};

} // namespace sim
