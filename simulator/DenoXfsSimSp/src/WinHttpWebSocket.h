#pragma once

#include "Interfaces.h"

#include <winhttp.h>

namespace sim {

// WebSocket client on the WinHTTP WebSocket API (Windows 8 and later), used in synchronous mode.
class WinHttpWebSocket : public IHostConnection {
public:
    WinHttpWebSocket() = default;
    ~WinHttpWebSocket() override;

    bool Connect(const std::string& url) override;
    bool Send(const std::string& text) override;
    bool Receive(std::string& text) override;
    void Close() override;

private:
    HINTERNET session_ = nullptr;
    HINTERNET connection_ = nullptr;
    HINTERNET socket_ = nullptr;
};

class WinHttpWebSocketFactory : public IHostConnectionFactory {
public:
    std::unique_ptr<IHostConnection> Create() override { return std::make_unique<WinHttpWebSocket>(); }
};

} // namespace sim
