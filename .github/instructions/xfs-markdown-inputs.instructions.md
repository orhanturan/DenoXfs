---
description: Rules for choosing between full, sectioned and legacy XFS spec Markdown
applyTo: "tools/HtmlToXfs2/**"
---
# Markdown usage rules for this repository

## Purpose
This repository produces multiple layers of Markdown output from XFS PDF specs. Use them in a disciplined way so the downstream C# and JSON generation pipeline gets clean, structured, and traceable content.

## Versioning
- Outputs are stored per spec version under pdf/{version}/.
- The active version is currently 340. Never hardcode the version; resolve it from the `--version` option (default 340).
- Other versions (for example 350) are not used unless explicitly selected.

## Output layers

### 1) Full source Markdown
Location: pdf/{version}/md_output/
Use for:
- source-of-truth document reference
- thorough review or debugging
- fallback when a section file is missing
- validation of the extraction pipeline

Do not use this as the default AI/context input when a smaller section is available.

### 2) Sectioned Markdown
Location: pdf/{version}/sections/
Use for:
- extracting single topics
- building C# DTOs and JSON payloads
- chunked retrieval or RAG-style pipelines
- narrow and traceable question-answering

This is the preferred input for downstream pipelines that operate on one section at a time.

### 3) Legacy Markdown
Location: pdf/md_output/
Use for:
- legacy workflow compatibility
- comparison with the versioned batches
- existing tooling that still expects the original output folder

Do not overwrite without explicit user instruction.

## Required file pattern
Each section file should include frontmatter or equivalent metadata with at least:
- document
- version
- device_class
- part
- section_id
- title
- type
- source_lines

Example metadata shape:
---
source: XfsPin.pdf
version: "{version}"
document: XfsPin
device_class: PIN
part: 6
section_id: "4.1"
title: "WFS_INF_PIN_STATUS"
type: info_command
source_lines: [650, 848]
---

## How to use these files
- Prefer the smallest relevant section file over the entire document.
- Use the sectioned directory for parsing into C# structures and JSON objects.
- Use the full markdown file only when the section boundary is unclear or when you need the original wording.
- Use README.md and _index.md to navigate between documents and sections.
- Use .index.json and .toc.json to confirm section ranges, titles, and command mappings.

## Generation pipeline guidance
When generating C# structures or JSON from these files:
1. Resolve the active version and read the relevant section markdown file.
2. Parse the frontmatter metadata.
3. Use the body text as the section content.
4. Create a Section model with section_id, title, type, source file, and content.
5. Aggregate sections into a Document model by document name and device class.
6. Serialize to JSON for downstream use.

## Priority order
1. pdf/{version}/sections for section-level pipeline work
2. pdf/{version}/md_output for source verification
3. pdf/md_output only for legacy compatibility tasks
