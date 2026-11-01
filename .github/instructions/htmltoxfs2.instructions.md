---
description: HtmlToXfs2 generator pipeline, CLI and naming rules
applyTo: "tools/HtmlToXfs2/**/*.cs"
---
# HtmlToXfs2

Run from `tools/HtmlToXfs2` (paths are relative to it).

## Pipeline (markdown-first, `Program.TryProcessMarkdownDevice`)
1. `MarkdownSpecParser` parses `pdf/{ver}/md_output/Xfs{Device}.md` into `WfsCommand` objects.
2. JSON written to `int/json/{device}/{GetInfo|Execute|Events}`.
3. `StructsAndEnumsCollector` produces `Structs.json` and `Enums.json`; `HeaderEnumExtractor` and insert-missing enrich them from `INCLUDE/*.H` (used for validation, not for C# generation).
4. `TypeGenerator` emits the C# from the header: `HeaderParser` reads defines, `values of STRUCT.field` comment groups and typedef structs; `SectionUsage` counts struct use per spec section; `TypePlanner` decides names, groups (Infos, Commands, Events, Shared, Common, Errors) and the command subfolder; `PlanWriter` renders constants, enums and structs to `output/{device}/`. `HeaderValidator` validates; `OutputCopier` copies to `src/DenoXfsApi` and `src/DenoXfs{Device}`, replacing group trees but not hand-written root files.
5. HTML (`input/*.html`, `TocConstructor`, `HtmlConstructor`) is the legacy fallback only.

## CLI
- `--version <ver>` (default 340), `--parse-md [path] [device]`, `--validate`, `--insert-missing`, `--copy-output` (needs `DENO_SRC`).
- Default run processes devices `api` then `idc`; `idc` planning depends on the `api` plan (alias enums).

## Consumers of the type plan
- `TypeGenerator.BuildPlan(device, version)` returns the same plan `Run` writes. `tools/XfsTestGen` builds its tests from it. When `TypePlanner` or `PlanWriter` gain a new field marker (`#Enum`, `@Struct`, `[@Struct]`, `*BYTE[len]`, ...), add the matching `Kind` in `tools/XfsTestGen/FieldModel.cs`, regenerate the tests and rebuild the native DLL (see `xfs-tests`).
- Generated property converters come from `PlanWriter.ParseField` and use `PrimitiveArray` / `PointerArray` from `DenoXfsInterop`.

## Rules
- Names are converted only through `NameConverter` and `naming-overrides.json` (struct tags, enum renames/merges, owner remaps, member names).
- Headers in `INCLUDE/*.H` win over spec text for constants, enum membership and struct layouts.
- A type clash inside a library, or enum owners with different widths, stops generation with a message; fix it through an override.
- `int/json/{device}/Placement.json` explains every Local/Shared decision.
- Enum and parameter parsing details: `EnumParsingRules.md`, `ParameterParsingRules.md` (written for the HTML path; they describe the legacy JSON, not C# output).
- Spec data layout and version rules: see `xfs-markdown-pipeline` and `xfs-markdown-inputs` instructions.
