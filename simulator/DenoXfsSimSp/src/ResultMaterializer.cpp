#include "ResultMaterializer.h"

#include <cstring>

namespace sim {

ResultMaterializer::ResultMaterializer(std::shared_ptr<IXfsManager> manager, std::shared_ptr<spdlog::logger> log)
    : manager_(std::move(manager)), log_(std::move(log)) {}

LPWFSRESULT ResultMaterializer::Create(HSERVICE service, REQUESTID request, HRESULT hResult, DWORD code, const ResultBlock* block) {
    LPVOID memory = nullptr;
    if (manager_->AllocateBuffer(sizeof(WFSRESULT), WFS_MEM_SHARE | WFS_MEM_ZEROINIT, &memory) != WFS_SUCCESS || memory == nullptr) {
        log_->error("result alloc failed reason=WFMAllocateBuffer reqId={}", request);
        return nullptr;
    }

    auto* result = static_cast<LPWFSRESULT>(memory);
    result->RequestID = request;
    result->hService = service;
    ::GetLocalTime(&result->tsTimestamp);
    result->hResult = hResult;
    result->u.dwCommandCode = code;
    result->lpBuffer = nullptr;

    if (block != nullptr && !block->data.empty() && !CopyBody(result, *block)) {
        manager_->FreeBuffer(result); // frees the linked blocks as well
        return nullptr;
    }

    log_->trace("result created reqId={} address={:p} size={}", request, static_cast<void*>(result), block ? block->data.size() : 0);
    return result;
}

void ResultMaterializer::Release(LPWFSRESULT result) {
    if (result != nullptr) manager_->FreeBuffer(result);
}

bool ResultMaterializer::CopyBody(LPWFSRESULT result, const ResultBlock& block) {
    if (block.ptrSize != static_cast<int>(sizeof(void*))) {
        log_->error("result rejected reason=ptrSize expected={} actual={}", sizeof(void*), block.ptrSize);
        return false;
    }

    const size_t size = block.data.size();
    for (const auto& fixup : block.fixups) {
        if (static_cast<size_t>(fixup.first) + sizeof(void*) > size || fixup.second > size) {
            log_->error("result rejected reason=fixup-out-of-range field={} target={} size={}", fixup.first, fixup.second, size);
            return false;
        }
    }

    LPVOID memory = nullptr;
    if (manager_->AllocateMore(static_cast<ULONG>(size), result, &memory) != WFS_SUCCESS || memory == nullptr) {
        log_->error("result alloc failed reason=WFMAllocateMore size={}", size);
        return false;
    }

    auto* body = static_cast<uint8_t*>(memory);
    std::memcpy(body, block.data.data(), size);
    for (const auto& fixup : block.fixups) {
        const auto address = reinterpret_cast<uintptr_t>(body + fixup.second);
        std::memcpy(body + fixup.first, &address, sizeof(address));
    }
    result->lpBuffer = body;
    return true;
}

} // namespace sim
