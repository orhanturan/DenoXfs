---
description: Layering and conventions for DenoXfsCore
applyTo: "src/DenoXfsCore/**"
---
# DenoXfsCore

- `Api/`: P/Invoke layer (`XfsApi`, `Xfs.cs`) and marshalled `Xfs*` structs. Keep layouts identical to the C headers in `tools/HtmlToXfs2/INCLUDE`.
- `Abstractions/`: public interfaces (`IXfsApp`, `IXfsSession`, `IXfsService`, `IXfsMessageProcessor`, command and builder interfaces).
- `Commands/`: Open/Execute/GetInfo commands and builders; builders are transient and obtained through `ICommandBuilderFactory`.
- `Convert/`: marshalling helpers (strings, arrays, structures, IntPtr).
- `Services/`: session, service and the hidden-window message pump (`MessageWindow`, `XfsMessageProcessor`, a hosted service).
- Register everything via `AddDenoXfsCore(appName)` in `Extensions/ServiceCollectionExtensions.cs`.
- Release every `WFSRESULT` with `WFSFreeResult`; XFS memory must not be freed by other means.
- Ignore `Models/XfsSample2` and `Api/HtmlToXfs2.code-workspace`; they are leftovers.
