#include "Base64.h"

namespace sim {

namespace {
int Value(char c) {
    if (c >= 'A' && c <= 'Z') return c - 'A';
    if (c >= 'a' && c <= 'z') return c - 'a' + 26;
    if (c >= '0' && c <= '9') return c - '0' + 52;
    if (c == '+') return 62;
    if (c == '/') return 63;
    return -1;
}
} // namespace

bool Base64Decode(const std::string& text, std::vector<uint8_t>& bytes) {
    bytes.clear();
    uint32_t buffer = 0;
    int bits = 0;
    for (char c : text) {
        if (c == '=') break;
        const int v = Value(c);
        if (v < 0) return false;
        buffer = (buffer << 6) | static_cast<uint32_t>(v);
        bits += 6;
        if (bits >= 8) {
            bits -= 8;
            bytes.push_back(static_cast<uint8_t>((buffer >> bits) & 0xFF));
        }
    }
    return true;
}

std::string Base64Encode(const uint8_t* data, size_t length) {
    static const char kAlphabet[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    std::string text;
    text.reserve((length + 2) / 3 * 4);
    for (size_t i = 0; i < length; i += 3) {
        const uint32_t b0 = data[i];
        const uint32_t b1 = i + 1 < length ? data[i + 1] : 0;
        const uint32_t b2 = i + 2 < length ? data[i + 2] : 0;
        const uint32_t triple = (b0 << 16) | (b1 << 8) | b2;
        text.push_back(kAlphabet[(triple >> 18) & 0x3F]);
        text.push_back(kAlphabet[(triple >> 12) & 0x3F]);
        text.push_back(i + 1 < length ? kAlphabet[(triple >> 6) & 0x3F] : '=');
        text.push_back(i + 2 < length ? kAlphabet[triple & 0x3F] : '=');
    }
    return text;
}

} // namespace sim
