---
description: HtmlToXfs2 generator pipeline, CLI and naming rules
applyTo: "tools/HtmlToXfs2/**/*.cs"
---
# HtmlToXfs2

Run from `tools/HtmlToXfs2` (paths are relative to it).

## Pipeline (markdown-first, `Program.TryProcessMarkdownDevice`)
1. `MarkdownSpecParser` parses `pdf/{ver}/md_output/Xfs{Device}.md` into `WfsCommand` objects.
2. JSON written to `int/json/{device}/{GetInfo|Execute|Events}`.
3. `StructsAndEnumsCollector` produces `Structs.json` and `Enums.json`; `HeaderEnumExtractor` and insert-missing enrich them from `INCLUDE/*.H`.
4. `ConstantsExporter`, `EnumExporter`, `StructExporter` emit C#; `HeaderValidator` validates; `OutputCopier` copies to the libraries.
5. HTML (`input/*.html`, `TocConstructor`, `HtmlConstructor`) is the legacy fallback only.

## CLI
- `--version <ver>` (default 340), `--parse-md [path] [device]`, `--validate`, `--insert-missing`, `--copy-output`.
- Default run processes devices `api` then `idc`.

## Rules
- Names are converted only through `NameConverter`.
- Headers in `INCLUDE/*.H` win over spec text for constants and struct layouts.
- Enum and parameter parsing details: `EnumParsingRules.md`, `ParameterParsingRules.md` (written for the HTML path; field and enum logic still applies).
- Spec data layout and version rules: see `xfs-markdown-pipeline` and `xfs-markdown-inputs` instructions.
