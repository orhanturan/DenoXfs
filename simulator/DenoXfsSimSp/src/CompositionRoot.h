#pragma once

#include "ServiceProvider.h"

namespace sim {

// The single place that builds the object graph. Built on first use (never inside DllMain).
ServiceProvider& GetServiceProvider();

} // namespace sim
