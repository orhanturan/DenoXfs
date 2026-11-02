#pragma once

#include "Interfaces.h"

namespace sim {

// Calls WFMAllocateBuffer/WFMAllocateMore/WFMFreeBuffer of the loaded msxfs.dll through GetProcAddress,
// so the DLL needs no import library of the XFS SDK.
class DynamicXfsManager : public IXfsManager {
public:
    DynamicXfsManager();

    HRESULT AllocateBuffer(ULONG size, ULONG flags, LPVOID* data) override;
    HRESULT AllocateMore(ULONG size, LPVOID original, LPVOID* data) override;
    HRESULT FreeBuffer(LPVOID data) override;

private:
    using AllocateBufferFn = HRESULT(WINAPI*)(ULONG, ULONG, LPVOID*);
    using AllocateMoreFn = HRESULT(WINAPI*)(ULONG, LPVOID, LPVOID*);
    using FreeBufferFn = HRESULT(WINAPI*)(LPVOID);

    AllocateBufferFn allocateBuffer_ = nullptr;
    AllocateMoreFn allocateMore_ = nullptr;
    FreeBufferFn freeBuffer_ = nullptr;
};

} // namespace sim
