#pragma once

#include <cstdint>
#include <string>
#include <vector>

namespace sim {

// Decodes standard base64 (with or without padding). Returns false on invalid input.
bool Base64Decode(const std::string& text, std::vector<uint8_t>& bytes);

// Encodes as standard base64 with padding (binary command input for the host).
std::string Base64Encode(const uint8_t* data, size_t length);

} // namespace sim
