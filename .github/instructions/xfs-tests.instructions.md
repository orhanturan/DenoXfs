---
description: Generated struct conversion tests (XfsTestGen, DenoXfsTestNative) and Interop tests
applyTo: "tests/**,tools/XfsTestGen/**,src/DenoXfsInterop/**"
---
# Tests

Project names have no dots (`DenoXfsIdcTests`, not `DenoXfs.Idc.Tests`).

## Struct conversion tests (generated)
- `tools/XfsTestGen` reads the same `TypePlan` as HtmlToXfs2 (`TypeGenerator.BuildPlan`) and writes:
  - `tests/DenoXfsTestNative/Generated/*.g.h|cpp`: per struct `Fill_`, `Free_`, `Verify_` plus the exports `Native_Create_`, `Native_Free_`, `Native_Verify_`, `Native_Layout_`. Built from the real `INCLUDE/XFS*.H` (pack 1), so the C++ compiler is the layout oracle.
  - `tests/DenoXfs{Library}Tests/Generated/**/{Struct}Tests.g.cs` and `NativeMethods.g.cs`: layout (size, offsets), native to managed, managed to native; scenario 0 = filled, 1 = zero/null.
- Run from the repo: `dotnet run --project tools/XfsTestGen [--devices api,idc] [--version 340]`. Never edit `Generated/`; change `Classifier`, `CppWriter` or `CsWriter` instead.
- Field kinds are decided in `FieldModel.cs` (`Classifier`) from the planned type string, mirroring `PlanWriter.ParseField`. A new type marker in `TypePlanner`/`PlanWriter` needs a matching `Kind`. Unsupported fields (raw `LPVOID`, SYSTEMTIME) are listed by the generator as "not covered"; WFSRESULT and union structs are skipped.
- Regenerate after every change to `TypePlanner` or `PlanWriter`, then rebuild the native DLL.

## Building and running
- The native project is a Visual Studio C++ project; `dotnet build` cannot build it. Use VS, or `MSBuild DenoXfs.slnx /p:Platform="Any CPU"` from a VS install.
- The test projects are x64 and copy `DenoXfsTestNative.dll` from `tests/bin/native/x64/{Configuration}`; build the solution (or the vcxproj with `-p:SolutionDir=<repo>\`) before `dotnet test`.
- `tests/DenoXfsInteropTests` holds hand-written unit tests for `PrimitiveArray` and `PointerArray` (C# to C#).

## Known gaps
- `lpszExtra` is tested only as one string. The spec form `key=value\0key=value\0\0` needs a new managed type (planned: `string[]` with a `MultiString` converter) and is postponed.
- Tests are struct based. A command layer (input/output transport per command, missing command classes such as `ReadRawData`) is not generated yet.
- Not covered: WFSRESULT and union structs, raw `LPVOID` fields, `SYSTEMTIME`. `XfsTestGen` prints them on every run.

## Interop converters (`src/DenoXfsInterop/Convert`)
- Two array shapes cover XFS: `PrimitiveArray` (byte, ushort, uint, enums; zero-terminated or counted) and `PointerArray` (string or struct pointers; null-terminated or counted).
- Terminated arrays keep null (no pointer) and empty (terminator only) apart; counted arrays write a null pointer for empty.
- Managed code allocates with `AllocHGlobal`; memory received from XFS is never freed with these helpers (use `WFSFreeResult`).
