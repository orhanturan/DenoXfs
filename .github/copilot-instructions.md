# DenoXfs

Goal: generate C# libraries and applications for CEN/XFS (eXtensions for Financial Services) from the XFS C headers and the spec markdown.

## Solution map
- `src/DenoXfsInterop` (`DenoXfs.Interop`): hand-written marshalling helpers; no dependencies.
- `src/DenoXfsApi` (`DenoXfs.Api`): `XfsApi` P/Invoke functions plus types generated from `XFSAPI.H`.
- `src/DenoXfsCore` (`DenoXfs.Core`): session/service/message pump, command builders, DI (`AddDenoXfsCore`); refs Api and Interop.
- `src/DenoXfsIdc` (`DenoXfs.Idc`): generated ID card reader types; refs Api and Interop. Template for other device classes.
- `tools/HtmlToXfs2`: generator CLI. Reads `INCLUDE/*.H` headers (types, constants) and the spec markdown (usage per section) and emits C# into the Api and device libraries.
- `tests/HtmlToXfs2.Tests`: tests for the generator (e.g. `MarkdownSpecParser`, type planning).

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
See `.github/instructions/` (Cursor reads these via `AGENTS.md`): xfs-library-layout, xfs-core, xfs-device-libs, htmltoxfs2, xfs-markdown-pipeline, xfs-markdown-inputs.
