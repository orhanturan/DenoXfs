#include "WinHttpWebSocket.h"

namespace sim {

namespace {

std::wstring Widen(const std::string& text) {
    if (text.empty()) return std::wstring();
    const int size = ::MultiByteToWideChar(CP_UTF8, 0, text.data(), static_cast<int>(text.size()), nullptr, 0);
    std::wstring wide(static_cast<size_t>(size), L'\0');
    ::MultiByteToWideChar(CP_UTF8, 0, text.data(), static_cast<int>(text.size()), wide.data(), size);
    return wide;
}

struct ParsedUrl {
    std::wstring host;
    INTERNET_PORT port = 80;
    std::wstring path = L"/";
    bool secure = false;
};

bool ParseUrl(const std::string& url, ParsedUrl& parsed) {
    std::string rest;
    if (url.rfind("ws://", 0) == 0) { rest = url.substr(5); }
    else if (url.rfind("wss://", 0) == 0) { rest = url.substr(6); parsed.secure = true; parsed.port = 443; }
    else return false;

    const auto slash = rest.find('/');
    std::string authority = slash == std::string::npos ? rest : rest.substr(0, slash);
    if (slash != std::string::npos) parsed.path = Widen(rest.substr(slash));

    const auto colon = authority.find(':');
    if (colon != std::string::npos) {
        parsed.port = static_cast<INTERNET_PORT>(std::stoi(authority.substr(colon + 1)));
        authority = authority.substr(0, colon);
    }
    parsed.host = Widen(authority);
    return !parsed.host.empty();
}

} // namespace

WinHttpWebSocket::~WinHttpWebSocket() { Close(); }

bool WinHttpWebSocket::Connect(const std::string& url) {
    ParsedUrl parsed;
    if (!ParseUrl(url, parsed)) return false;

    session_ = ::WinHttpOpen(L"DenoXfsSimSp", WINHTTP_ACCESS_TYPE_NO_PROXY, WINHTTP_NO_PROXY_NAME, WINHTTP_NO_PROXY_BYPASS, 0);
    if (!session_) return false;
    connection_ = ::WinHttpConnect(session_, parsed.host.c_str(), parsed.port, 0);
    if (!connection_) { Close(); return false; }

    HINTERNET request = ::WinHttpOpenRequest(connection_, L"GET", parsed.path.c_str(), nullptr, WINHTTP_NO_REFERER,
                                             WINHTTP_DEFAULT_ACCEPT_TYPES, parsed.secure ? WINHTTP_FLAG_SECURE : 0);
    if (!request) { Close(); return false; }

    const bool upgraded =
        ::WinHttpSetOption(request, WINHTTP_OPTION_UPGRADE_TO_WEB_SOCKET, nullptr, 0) &&
        ::WinHttpSendRequest(request, WINHTTP_NO_ADDITIONAL_HEADERS, 0, nullptr, 0, 0, 0) &&
        ::WinHttpReceiveResponse(request, nullptr);
    if (upgraded) socket_ = ::WinHttpWebSocketCompleteUpgrade(request, 0);
    ::WinHttpCloseHandle(request);

    if (!socket_) { Close(); return false; }
    return true;
}

bool WinHttpWebSocket::Send(const std::string& text) {
    if (!socket_) return false;
    return ::WinHttpWebSocketSend(socket_, WINHTTP_WEB_SOCKET_UTF8_MESSAGE_BUFFER_TYPE,
                                  const_cast<char*>(text.data()), static_cast<DWORD>(text.size())) == ERROR_SUCCESS;
}

bool WinHttpWebSocket::Receive(std::string& text) {
    text.clear();
    if (!socket_) return false;

    char buffer[8192];
    for (;;) {
        DWORD read = 0;
        WINHTTP_WEB_SOCKET_BUFFER_TYPE type;
        if (::WinHttpWebSocketReceive(socket_, buffer, sizeof(buffer), &read, &type) != ERROR_SUCCESS) return false;
        if (type == WINHTTP_WEB_SOCKET_CLOSE_BUFFER_TYPE) return false;
        text.append(buffer, read);
        if (type == WINHTTP_WEB_SOCKET_UTF8_MESSAGE_BUFFER_TYPE) return true;
        // UTF8_FRAGMENT: keep reading until the final fragment.
    }
}

void WinHttpWebSocket::Close() {
    if (socket_) {
        ::WinHttpWebSocketClose(socket_, WINHTTP_WEB_SOCKET_SUCCESS_CLOSE_STATUS, nullptr, 0);
        ::WinHttpCloseHandle(socket_);
        socket_ = nullptr;
    }
    if (connection_) { ::WinHttpCloseHandle(connection_); connection_ = nullptr; }
    if (session_) { ::WinHttpCloseHandle(session_); session_ = nullptr; }
}

} // namespace sim
