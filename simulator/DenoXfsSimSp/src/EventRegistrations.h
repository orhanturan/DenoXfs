#pragma once

#include "XfsPlatform.h"

#include <map>
#include <mutex>
#include <vector>

namespace sim {

// Windows registered with WFPRegister and their event classes (SERVICE_EVENTS, USER_EVENTS, SYSTEM_EVENTS, EXECUTE_EVENTS).
class EventRegistrations {
public:
    void Register(HWND window, DWORD classes) {
        std::lock_guard<std::mutex> lock(mutex_);
        windows_[window] |= classes;
    }

    // A NULL window removes the classes from every window (WFPDeregister with hWndReg NULL).
    void Deregister(HWND window, DWORD classes) {
        std::lock_guard<std::mutex> lock(mutex_);
        for (auto it = windows_.begin(); it != windows_.end();) {
            if (window == nullptr || it->first == window) it->second &= ~classes;
            it = it->second == 0 ? windows_.erase(it) : std::next(it);
        }
    }

    std::vector<HWND> WindowsFor(DWORD eventClass) const {
        std::lock_guard<std::mutex> lock(mutex_);
        std::vector<HWND> windows;
        for (const auto& entry : windows_)
            if ((entry.second & eventClass) != 0) windows.push_back(entry.first);
        return windows;
    }

private:
    mutable std::mutex mutex_;
    std::map<HWND, DWORD> windows_;
};

} // namespace sim
