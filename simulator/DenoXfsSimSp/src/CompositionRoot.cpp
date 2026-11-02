#include "CompositionRoot.h"

#include "DynamicXfsManager.h"
#include "RegistryHostEndpointProvider.h"
#include "ResultMaterializer.h"
#include "WindowMessagePoster.h"
#include "WinHttpWebSocket.h"
#include "idc/IdcInputDecoders.h"

#include <spdlog/sinks/basic_file_sink.h>

namespace sim {

namespace {

std::shared_ptr<spdlog::logger> CreateLogger() {
    char temp[MAX_PATH] = {};
    ::GetTempPathA(MAX_PATH, temp);
    const std::string path = std::string(temp) + "DenoXfsSimSp-" + std::to_string(::GetCurrentProcessId()) + ".log";

    // Synchronous file logger: no background thread inside a DLL.
    auto logger = std::make_shared<spdlog::logger>("DenoXfsSimSp", std::make_shared<spdlog::sinks::basic_file_sink_mt>(path, false));
    logger->set_pattern("%Y-%m-%d %H:%M:%S.%e [%l] [t%t] %v");
    logger->set_level(spdlog::level::trace);
    logger->flush_on(spdlog::level::info);
    return logger;
}

class SessionFactory : public ISessionFactory {
public:
    SessionFactory(std::shared_ptr<IHostConnectionFactory> connections, std::shared_ptr<IHostEndpointProvider> endpoint,
                   std::shared_ptr<IResultMaterializer> materializer, std::shared_ptr<IMessagePoster> poster,
                   std::shared_ptr<const std::vector<std::shared_ptr<ICommandInputDecoder>>> decoders,
                   std::shared_ptr<const std::vector<std::shared_ptr<ICommandInputDecoder>>> infoDecoders,
                   std::shared_ptr<spdlog::logger> log)
        : connections_(std::move(connections)), endpoint_(std::move(endpoint)), materializer_(std::move(materializer)),
          poster_(std::move(poster)), decoders_(std::move(decoders)), infoDecoders_(std::move(infoDecoders)), log_(std::move(log)) {}

    std::shared_ptr<SpSession> Create(HSERVICE service, std::function<void(HSERVICE)> onEnded) override {
        SessionDependencies deps;
        deps.connection = connections_->Create();
        deps.materializer = materializer_;
        deps.poster = poster_;
        deps.decoders = decoders_;
        deps.infoDecoders = infoDecoders_;
        deps.log = log_;
        deps.hostUrl = endpoint_->GetHostUrl();
        deps.onEnded = std::move(onEnded);
        return std::make_shared<SpSession>(service, std::move(deps));
    }

private:
    std::shared_ptr<IHostConnectionFactory> connections_;
    std::shared_ptr<IHostEndpointProvider> endpoint_;
    std::shared_ptr<IResultMaterializer> materializer_;
    std::shared_ptr<IMessagePoster> poster_;
    std::shared_ptr<const std::vector<std::shared_ptr<ICommandInputDecoder>>> decoders_;
    std::shared_ptr<const std::vector<std::shared_ptr<ICommandInputDecoder>>> infoDecoders_;
    std::shared_ptr<spdlog::logger> log_;
};

std::unique_ptr<ServiceProvider> Build() {
    auto log = CreateLogger();
    auto manager = std::make_shared<DynamicXfsManager>();
    auto materializer = std::make_shared<ResultMaterializer>(manager, log);

    auto decoders = std::make_shared<std::vector<std::shared_ptr<ICommandInputDecoder>>>(idc::CommandInputDecoders());
    auto infoDecoders = std::make_shared<std::vector<std::shared_ptr<ICommandInputDecoder>>>(idc::InfoInputDecoders());

    auto factory = std::make_shared<SessionFactory>(std::make_shared<WinHttpWebSocketFactory>(),
                                                    std::make_shared<RegistryHostEndpointProvider>(), materializer,
                                                    std::make_shared<WindowMessagePoster>(), decoders, infoDecoders, log);
    log->info("service provider created pid={}", ::GetCurrentProcessId());
    return std::make_unique<ServiceProvider>(factory, log);
}

} // namespace

ServiceProvider& GetServiceProvider() {
    static std::unique_ptr<ServiceProvider> instance = Build();
    return *instance;
}

} // namespace sim
