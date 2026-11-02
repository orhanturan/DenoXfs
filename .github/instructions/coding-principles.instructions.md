---
description: SOLID, dependency injection and naming rules for all hand-written C# and C++ code
applyTo: "**/*.cs,**/*.cpp,**/*.h,**/*.csproj,**/*.vcxproj"
---
# Coding principles

## SOLID
- One class, one reason to change (SRP). Split parsing, validation, persistence and transport into separate classes.
- Depend on abstractions (DIP): a class receives interfaces, it never creates the services it uses. Add a new behaviour by adding a new implementation, not by editing a growing `switch` (OCP). Keep interfaces small and role based (ISP). Implementations must honour the contract of the interface (LSP).
- Generated code follows the same rules where it can; fix the generator, not the output.

## C# dependency injection
- Constructor injection with `Microsoft.Extensions.DependencyInjection`. No service locator (`IServiceProvider` in business classes), no static mutable state, no `new` of services inside classes.
- The composition root is `Program` plus `AddXxx(this IServiceCollection ...)` extension methods per library (`AddDenoXfsCore` is the model). Libraries register their own services; the host only calls the extensions.
- Pick lifetimes deliberately (singleton for stateless or shared state, scoped per request, transient for builders). Device-class modules are registered as keyed services by device class.
- Anything that talks to the outside (database, sockets, files, clock, registry, native calls) sits behind an interface so tests can replace it.

## Logging
- Every application writes its own log file. C# code logs through `Microsoft.Extensions.Logging` (`ILogger<T>` injected by constructor); Serilog is only the provider, configured in the host's `Program` and `appsettings.json` (console + daily rolling file under `logs/`, readable text, not JSON). Libraries never reference Serilog.
- C++ uses spdlog behind an injected `std::shared_ptr<spdlog::logger>`, synchronous, one file per process.
- Log lines are `key=value` style and name the command, request id and result code; never log secrets.
- Local development uses plain HTTP (no HTTPS redirect or dev certificate).

## C++ dependency injection
- Classes that hold state or logic take their dependencies as constructor arguments typed as pure abstract interfaces (`I...`), held by `std::unique_ptr` or `std::shared_ptr`. No hidden globals or singletons besides the DLL entry points.
- One composition root builds the object graph (for a Service Provider DLL: when the session is created in the open call). The exported C functions (`WFP*`, `WFS*`) only translate arguments and delegate to the composition root.
- Plain C-style functions and POD structs that mirror the XFS headers are exempt; do not wrap them in interfaces.
- Memory: RAII only (`std::unique_ptr`, `std::vector`, `std::string`). XFS buffers come from `WFMAllocateBuffer`/`WFMAllocateMore` and are owned by the receiving application after posting.

## Naming
- Project and folder names have no dots (`DenoXfsCore`, `DenoXfsIdcTests`). Namespaces may contain dots (`DenoXfs.Core`).
- Interfaces start with `I` in both languages.
