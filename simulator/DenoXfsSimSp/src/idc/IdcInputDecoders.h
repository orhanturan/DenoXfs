#pragma once

#include "../Base64.h"
#include "../Interfaces.h"

#include <xfsidc.h>

namespace sim::idc {

// Input that is a single LPWORD (may be NULL): {"<field>": n} or {}.
class WordInputDecoder : public ICommandInputDecoder {
public:
    WordInputDecoder(DWORD code, const char* field) : code_(code), field_(field) {}

    bool CanDecode(DWORD code) const override { return code == code_; }

    nlohmann::json Decode(LPVOID data) const override {
        nlohmann::json input = nlohmann::json::object();
        if (data != nullptr) input[field_] = *static_cast<const WORD*>(data);
        return input;
    }

private:
    DWORD code_;
    const char* field_;
};

// Input that is a single LPSTR (may be NULL): {"<field>": "text"} or {}.
class StringInputDecoder : public ICommandInputDecoder {
public:
    StringInputDecoder(DWORD code, const char* field) : code_(code), field_(field) {}

    bool CanDecode(DWORD code) const override { return code == code_; }

    nlohmann::json Decode(LPVOID data) const override {
        nlohmann::json input = nlohmann::json::object();
        if (data != nullptr) input[field_] = std::string(static_cast<LPCSTR>(data));
        return input;
    }

private:
    DWORD code_;
    const char* field_;
};

// Input that is one structure, decoded by a function.
class StructInputDecoder : public ICommandInputDecoder {
public:
    using DecodeFunction = nlohmann::json (*)(LPVOID data);

    StructInputDecoder(DWORD code, DecodeFunction decode) : code_(code), decode_(decode) {}

    bool CanDecode(DWORD code) const override { return code == code_; }

    nlohmann::json Decode(LPVOID data) const override {
        return data == nullptr ? nlohmann::json::object() : decode_(data);
    }

private:
    DWORD code_;
    DecodeFunction decode_;
};

namespace detail {

inline std::string Text(LPCSTR text) { return text != nullptr ? std::string(text) : std::string(); }

inline std::string Bytes(const BYTE* data, ULONG length) { return data != nullptr ? Base64Encode(data, length) : std::string(); }

// NULL terminated array of LPWFSIDCCARDDATA (as READ_RAW_DATA returns it).
inline nlohmann::json CardData(LPWFSIDCCARDDATA* items) {
    nlohmann::json array = nlohmann::json::array();
    for (; items != nullptr && *items != nullptr; ++items) {
        const WFSIDCCARDDATA& item = **items;
        array.push_back({{"dataSource", item.wDataSource}, {"status", item.wStatus},
                         {"data", Bytes(item.lpbData, item.ulDataLength)}, {"writeMethod", item.fwWriteMethod}});
    }
    return array;
}

inline nlohmann::json EjectCard(LPVOID data) {
    return {{"ejectPosition", static_cast<const WFSIDCEJECTCARD*>(data)->wEjectPosition}};
}

inline nlohmann::json ChipIo(LPVOID data) {
    const auto* io = static_cast<const WFSIDCCHIPIO*>(data);
    return {{"chipProtocol", io->wChipProtocol}, {"chipData", Bytes(io->lpbChipData, io->ulChipDataLength)}};
}

inline nlohmann::json WriteTrack(LPVOID data) {
    const auto* write = static_cast<const WFSIDCWRITETRACK*>(data);
    return {{"formName", Text(write->lpstrFormName)}, {"trackData", Text(write->lpstrTrackData)}, {"writeMethod", write->fwWriteMethod}};
}

inline nlohmann::json Setkey(LPVOID data) {
    const auto* key = static_cast<const WFSIDCSETKEY*>(data);
    return {{"keyLength", key->usKeyLen}, {"key", Bytes(key->lpbKeyValue, key->usKeyLen)}};
}

inline nlohmann::json WriteRawData(LPVOID data) {
    return {{"cardData", CardData(static_cast<LPWFSIDCCARDDATA*>(data))}};
}

inline nlohmann::json ParseData(LPVOID data) {
    const auto* parse = static_cast<const WFSIDCPARSEDATA*>(data);
    return {{"formName", Text(parse->lpstrFormName)}, {"cardData", CardData(parse->lppCardData)}};
}

inline nlohmann::json SetGuidanceLight(LPVOID data) {
    const auto* light = static_cast<const WFSIDCSETGUIDLIGHT*>(data);
    return {{"guidLight", light->wGuidLight}, {"command", light->dwCommand}};
}

inline nlohmann::json PowerSaveControl(LPVOID data) {
    return {{"maxPowerSaveRecoveryTime", static_cast<const WFSIDCPOWERSAVECONTROL*>(data)->usMaxPowerSaveRecoveryTime}};
}

inline nlohmann::json ParkCard(LPVOID data) {
    const auto* park = static_cast<const WFSIDCPARKCARD*>(data);
    return {{"direction", park->wDirection}, {"parkingStation", park->usParkingStation}};
}

inline nlohmann::json SynchronizeCommand(LPVOID data) {
    return {{"command", static_cast<const WFSIDCSYNCHRONIZECOMMAND*>(data)->dwCommand}};
}

} // namespace detail

// Decoders of the WFPExecute input (lpCmdData) of every IDC command that has one.
inline std::vector<std::shared_ptr<ICommandInputDecoder>> CommandInputDecoders() {
    return {
        std::make_shared<WordInputDecoder>(WFS_CMD_IDC_READ_RAW_DATA, "readData"),
        std::make_shared<WordInputDecoder>(WFS_CMD_IDC_RESET, "resetIn"),
        std::make_shared<WordInputDecoder>(WFS_CMD_IDC_CHIP_POWER, "chipPower"),
        std::make_shared<StringInputDecoder>(WFS_CMD_IDC_READ_TRACK, "formName"),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_EJECT_CARD, &detail::EjectCard),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_CHIP_IO, &detail::ChipIo),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_WRITE_TRACK, &detail::WriteTrack),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_SETKEY, &detail::Setkey),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_WRITE_RAW_DATA, &detail::WriteRawData),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_PARSE_DATA, &detail::ParseData),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_SET_GUIDANCE_LIGHT, &detail::SetGuidanceLight),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_POWER_SAVE_CONTROL, &detail::PowerSaveControl),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_PARK_CARD, &detail::ParkCard),
        std::make_shared<StructInputDecoder>(WFS_CMD_IDC_SYNCHRONIZE_COMMAND, &detail::SynchronizeCommand),
    };
}

// Decoders of the WFPGetInfo query details (lpQueryDetails). Separate from the commands: the codes overlap.
inline std::vector<std::shared_ptr<ICommandInputDecoder>> InfoInputDecoders() {
    return {
        std::make_shared<StringInputDecoder>(WFS_INF_IDC_QUERY_FORM, "formName"),
    };
}

} // namespace sim::idc
