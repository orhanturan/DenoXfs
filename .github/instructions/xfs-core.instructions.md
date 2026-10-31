---
description: Layering and conventions for DenoXfsCore
applyTo: "src/DenoXfsCore/**"
---
# DenoXfsCore

Hand-written runtime only. Generated types and P/Invoke live in `DenoXfsApi`, marshalling helpers in `DenoXfsInterop` (see `xfs-library-layout`).

- `Abstractions/`: public interfaces (`IXfsApp`, `IXfsSession`, `IXfsService`, `IXfsMessageProcessor`, command and builder interfaces).
- `Commands/`: Open/Execute/GetInfo commands and builders; builders are transient and obtained through `ICommandBuilderFactory`.
- `Services/`: session, service and the hidden-window message pump (`MessageWindow`, `XfsMessageProcessor`, a hosted service).
- `Models/`: requests, responses and command inputs.
- Register everything via `AddDenoXfsCore(appName)` in `Extensions/ServiceCollectionExtensions.cs`.
- Release every `WFSRESULT` with `WFSFreeResult`; XFS memory must not be freed by other means.
- Core must not reference device libraries; `Models/XfsSample2` is a leftover and can be ignored.
