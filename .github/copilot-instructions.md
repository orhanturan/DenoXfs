# DenoXfs

Goal: generate C# libraries and applications for CEN/XFS (eXtensions for Financial Services) from the XFS C headers and the spec markdown.

Pipeline (spec version 3.40 for now): spec PDF -> markdown (markitdown) -> JSON per command/struct -> C# structs and enums (`tools/HtmlToXfs2`) -> device libraries and middleware on top -> generated conversion tests (`tools/XfsTestGen`). The C header wins over the spec text for constants and layouts.

## Working agreements
- Never run `git commit`, `git push` or other history-changing git commands; the owner makes every commit. Leave changes in the working tree and say what changed.
- Keep shared rules in `.github/` (read by Copilot, Cursor, VS Code and Claude through `AGENTS.md`). Tool-specific files only reference them, they do not copy them.

## Solution map
- `src/DenoXfsInterop` (`DenoXfs.Interop`): hand-written marshalling helpers; no dependencies.
- `src/DenoXfsApi` (`DenoXfs.Api`): `XfsApi` P/Invoke functions plus types generated from `XFSAPI.H`.
- `src/DenoXfsCore` (`DenoXfs.Core`): session/service/message pump, command builders, DI (`AddDenoXfsCore`); refs Api and Interop.
- `src/DenoXfsIdc` (`DenoXfs.Idc`): generated ID card reader types; refs Api and Interop. Template for other device classes.
- `tools/HtmlToXfs2`: generator CLI. Reads `INCLUDE/*.H` headers (types, constants) and the spec markdown (usage per section) and emits C# into the Api and device libraries.
- `tests/HtmlToXfs2.Tests`: tests for the generator (e.g. `MarkdownSpecParser`, type planning).
- `tools/XfsTestGen`: generates the struct conversion tests (C++ native side and C# xunit) from the HtmlToXfs2 type plan.
- `tests/DenoXfsTestNative` (C++), `tests/DenoXfsApiTests`, `tests/DenoXfsIdcTests` (generated), `tests/DenoXfsInteropTests` (hand-written).

## Conventions
- Target `net9.0-windows`, `Nullable` and `ImplicitUsings` enabled.
- Generated C# files are produced by HtmlToXfs2; fix the generator, not the output, unless told otherwise.
- Spec version comes from `--version` (default 340); never hardcode it.

## XFS glossary
- Prefixes: `WFS` = API (apps call), `WFP` = SPI (Manager calls providers), `WFM` = support/config functions.
- `hService` = session handle from WFSOpen; `hApp` = application identity; `WFSRESULT` = result buffer that must be released with `WFSFreeResult`.
- Command prefixes: `WFS_INF_` GetInfo, `WFS_CMD_` Execute, `WFS_EXEE_`/`WFS_SRVE_`/`WFS_USRE_`/`WFS_SYSE_` events.
- Device classes: IDC, CDM, CIM, PIN, PTR, CHK, DEP, TTU, SIU, VDM, CAM, ALM, CEU, CRD, BCR, IPM, BIO.

## Scoped rules
See `.github/instructions/` (Cursor reads these via `AGENTS.md`): xfs-library-layout, xfs-core, xfs-device-libs, htmltoxfs2, xfs-markdown-pipeline, xfs-markdown-inputs, xfs-tests.
