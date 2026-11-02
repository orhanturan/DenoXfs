#pragma once

#include "Interfaces.h"

namespace sim {

// Host URL from the environment variable DENOXFS_SIM_HOST, else HKLM\SOFTWARE\DenoXfs\Simulator value "Host"
// (this 32 bit DLL reads the WOW6432Node view), else the local default.
class RegistryHostEndpointProvider : public IHostEndpointProvider {
public:
    std::string GetHostUrl() const override;
};

} // namespace sim
