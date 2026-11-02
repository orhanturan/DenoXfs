#include "DynamicXfsManager.h"

namespace sim {

DynamicXfsManager::DynamicXfsManager() {
    // The Manager loaded this DLL, so msxfs.dll is already in the process; LoadLibrary only adds a reference.
    HMODULE module = ::LoadLibraryA("msxfs.dll");
    if (module == nullptr) return;
    allocateBuffer_ = reinterpret_cast<AllocateBufferFn>(::GetProcAddress(module, "WFMAllocateBuffer"));
    allocateMore_ = reinterpret_cast<AllocateMoreFn>(::GetProcAddress(module, "WFMAllocateMore"));
    freeBuffer_ = reinterpret_cast<FreeBufferFn>(::GetProcAddress(module, "WFMFreeBuffer"));
}

HRESULT DynamicXfsManager::AllocateBuffer(ULONG size, ULONG flags, LPVOID* data) {
    return allocateBuffer_ ? allocateBuffer_(size, flags, data) : WFS_ERR_INTERNAL_ERROR;
}

HRESULT DynamicXfsManager::AllocateMore(ULONG size, LPVOID original, LPVOID* data) {
    return allocateMore_ ? allocateMore_(size, original, data) : WFS_ERR_INTERNAL_ERROR;
}

HRESULT DynamicXfsManager::FreeBuffer(LPVOID data) {
    return freeBuffer_ ? freeBuffer_(data) : WFS_ERR_INTERNAL_ERROR;
}

} // namespace sim
