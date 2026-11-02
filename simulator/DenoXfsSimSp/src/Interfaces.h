#pragma once

#include "XfsPlatform.h"

#include <cstdint>
#include <memory>
#include <string>
#include <utility>
#include <vector>

#include <nlohmann/json.hpp>

namespace sim {

// Connection to the simulator host (one per Service Provider session).
class IHostConnection {
public:
    virtual ~IHostConnection() = default;
    virtual bool Connect(const std::string& url) = 0;
    virtual bool Send(const std::string& text) = 0;
    // Blocks until one text message arrived. False when the connection closed or failed.
    virtual bool Receive(std::string& text) = 0;
    virtual void Close() = 0;
};

class IHostConnectionFactory {
public:
    virtual ~IHostConnectionFactory() = default;
    virtual std::unique_ptr<IHostConnection> Create() = 0;
};

class IHostEndpointProvider {
public:
    virtual ~IHostEndpointProvider() = default;
    virtual std::string GetHostUrl() const = 0;
};

// The WFM* support functions of the XFS Manager.
class IXfsManager {
public:
    virtual ~IXfsManager() = default;
    virtual HRESULT AllocateBuffer(ULONG size, ULONG flags, LPVOID* data) = 0;
    virtual HRESULT AllocateMore(ULONG size, LPVOID original, LPVOID* data) = 0;
    virtual HRESULT FreeBuffer(LPVOID data) = 0;
};

class IMessagePoster {
public:
    virtual ~IMessagePoster() = default;
    virtual bool Post(HWND window, UINT message, LPARAM lParam) = 0;
};

// Flat result block built by the host: data plus pointer fixups (see the host's ResultBuffer).
struct ResultBlock {
    int ptrSize = 4;
    std::vector<uint8_t> data;
    std::vector<std::pair<uint32_t, uint32_t>> fixups; // {pointer field offset, target offset}
};

class IResultMaterializer {
public:
    virtual ~IResultMaterializer() = default;
    // Creates a WFSRESULT (and body) in XFS Manager memory. Null on failure; the caller owns it until posted.
    virtual LPWFSRESULT Create(HSERVICE service, REQUESTID request, HRESULT result, DWORD code, const ResultBlock* block) = 0;
    virtual void Release(LPWFSRESULT result) = 0;
};

// Turns the application's command input (lpCmdData) into JSON for the host.
class ICommandInputDecoder {
public:
    virtual ~ICommandInputDecoder() = default;
    virtual bool CanDecode(DWORD command) const = 0;
    virtual nlohmann::json Decode(LPVOID data) const = 0;
};

} // namespace sim
