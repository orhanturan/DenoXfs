#pragma once

#include "XfsPlatform.h"

#include <map>
#include <mutex>
#include <optional>
#include <string>
#include <vector>

namespace sim {

struct PendingRequest {
    REQUESTID id = 0;
    HWND window = nullptr;
    std::string kind;   // "execute", "getinfo", "close", ...
    DWORD code = 0;     // command or category
};

// Requests sent to the host that still wait for their completion. Routing table, not a memory tracker.
class PendingRequests {
public:
    void Add(const PendingRequest& request) {
        std::lock_guard<std::mutex> lock(mutex_);
        requests_[request.id] = request;
    }

    std::optional<PendingRequest> Take(REQUESTID id) {
        std::lock_guard<std::mutex> lock(mutex_);
        const auto found = requests_.find(id);
        if (found == requests_.end()) return std::nullopt;
        PendingRequest request = found->second;
        requests_.erase(found);
        return request;
    }

    // Looks a request up without removing it (execute events go to the window of their request).
    std::optional<PendingRequest> Find(REQUESTID id) {
        std::lock_guard<std::mutex> lock(mutex_);
        const auto found = requests_.find(id);
        if (found == requests_.end()) return std::nullopt;
        return found->second;
    }

    std::vector<PendingRequest> TakeAll() {
        std::lock_guard<std::mutex> lock(mutex_);
        std::vector<PendingRequest> all;
        for (const auto& entry : requests_) all.push_back(entry.second);
        requests_.clear();
        return all;
    }

private:
    std::mutex mutex_;
    std::map<REQUESTID, PendingRequest> requests_;
};

} // namespace sim
