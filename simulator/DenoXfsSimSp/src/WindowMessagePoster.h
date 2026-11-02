#pragma once

#include "Interfaces.h"

namespace sim {

class WindowMessagePoster : public IMessagePoster {
public:
    bool Post(HWND window, UINT message, LPARAM lParam) override {
        return ::IsWindow(window) && ::PostMessage(window, message, 0, lParam) != FALSE;
    }
};

} // namespace sim
