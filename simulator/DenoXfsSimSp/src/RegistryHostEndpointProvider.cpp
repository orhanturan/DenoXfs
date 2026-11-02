#include "RegistryHostEndpointProvider.h"

namespace sim {

std::string RegistryHostEndpointProvider::GetHostUrl() const {
    char buffer[512] = {};
    const DWORD fromEnv = ::GetEnvironmentVariableA("DENOXFS_SIM_HOST", buffer, sizeof(buffer));
    if (fromEnv > 0 && fromEnv < sizeof(buffer)) return buffer;

    DWORD size = sizeof(buffer);
    if (::RegGetValueA(HKEY_LOCAL_MACHINE, "SOFTWARE\\DenoXfs\\Simulator", "Host", RRF_RT_REG_SZ, nullptr, buffer, &size) == ERROR_SUCCESS)
        return buffer;

    return "ws://127.0.0.1:5080/ws";
}

} // namespace sim
