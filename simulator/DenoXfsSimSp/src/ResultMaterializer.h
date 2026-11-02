#pragma once

#include "Interfaces.h"

#include <spdlog/spdlog.h>

namespace sim {

// Builds WFSRESULT (WFMAllocateBuffer) and its body (WFMAllocateMore, linked to the result) from a flat block.
// The receiving application releases both with WFSFreeResult.
class ResultMaterializer : public IResultMaterializer {
public:
    ResultMaterializer(std::shared_ptr<IXfsManager> manager, std::shared_ptr<spdlog::logger> log);

    LPWFSRESULT Create(HSERVICE service, REQUESTID request, HRESULT result, DWORD code, const ResultBlock* block) override;
    void Release(LPWFSRESULT result) override;

private:
    bool CopyBody(LPWFSRESULT result, const ResultBlock& block);

    std::shared_ptr<IXfsManager> manager_;
    std::shared_ptr<spdlog::logger> log_;
};

} // namespace sim
