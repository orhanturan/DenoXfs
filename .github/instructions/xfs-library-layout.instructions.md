---
description: Library split, folder groups, namespaces and naming for the XFS libraries under src
applyTo: "src/**"
---
# XFS library layout

## Libraries and dependencies
- `DenoXfsInterop` (`DenoXfs.Interop`): hand-written marshalling (`Convert`, `Extensions`). No dependencies.
- `DenoXfsApi` (`DenoXfs.Api`): `XfsApi.cs` and `SystemTime.cs` (hand-written) plus everything generated from `XFSAPI.H`. Refs Interop.
- `DenoXfs{Device}` (`DenoXfs.Idc`, ...): generated only. Refs Api and Interop, never Core.
- `DenoXfsCore` (`DenoXfs.Core`): hand-written runtime (session, service, message pump, command builders, DI). Refs Api and Interop, knows no device.
- Dependencies only point downwards: Interop <- Api <- device libraries; Core -> Api, Interop.

## Generated folders
- Library root: constants class (`Xfs.cs`, `Idc.cs`) and the command-id enums (`IdcInfoId`, `IdcCommandId`, `IdcEventId`). Their namespace is `DenoXfs.{Library}`.
- `Infos/{Command}/`, `Commands/{Command}/`, `Events/{Command}/`: types used by that one spec command. `{Command}` is the command name without `WFS_{INF|CMD|EXEE|SRVE|USRE}_{DEVICE}_` (`Status`, `EjectCard`).
- `Shared/` (`Common/` in Api, kept flat): types used by 2+ commands, or defined only in the C header.
- `Errors/`: the `WFS_ERR_*` enum (`IdcError`, `XfsError`). `IdcError` does not derive from `XfsError`.
- Namespace is `DenoXfs.{Library}.{Group}` even when the file sits in a command subfolder. A subfolder does not add a namespace segment.
- Every file in a group folder is generated and replaced wholesale by `--copy-output`; never put hand-written files there. The library root is not wiped, so `XfsApi.cs` and `SystemTime.cs` stay.

## Naming
- Device struct: `Xfs` + tag without `wfs_`, PascalCase (`XfsIdcStatus`, `XfsIdcEjectCard`). Api structs stay `Xfs` + tag (`XfsResult`).
- Device field enum: `{Device}{Field}{Role}`. Role is `Status`, `Capability` (one value) or `Capabilities` (`[Flags]`), or the struct short name (`IdcDirectionParkCard`). The same `#define` set on several structs drops the role (`IdcEjectPosition`, `IdcMedia`).
- Api field enums keep `{OwnerShort}{Field}` (`DevStatus` + override).
- Command-id enum members drop `Inf`/`Cmd`/`Exee`/`Srve`/`Usre` and the device name (`Status = Idc.InfIdcStatus`). A colliding event name keeps the category (`ExeeMediaRetained`).
- Enum values that only alias `WFS_STAT_*` reuse the Api enum when the underlying width matches, otherwise the device keeps its own copy.
- Exceptions to these rules belong in `tools/HtmlToXfs2/naming-overrides.json`, never in generated files.

## Generated code
- Fix `tools/HtmlToXfs2` and re-run it instead of editing generated files; see the htmltoxfs2 rules.
- `tools/HtmlToXfs2/int/json/{device}/Placement.json` records why each type landed in its group.
