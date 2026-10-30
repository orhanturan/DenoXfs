---
description: PDF-to-Markdown conversion workflow, spec versioning and output layout for XFS docs
applyTo: "tools/HtmlToXfs2/**"
---
# Copilot repository instructions

## General guidance
- This repository contains a PDF-to-Markdown conversion workflow and XFS specification docs.
- Prefer the smallest useful context for AI work. Do not send a whole PDF or a giant Markdown file when a specific section is enough.
- Use section outputs as the default AI context when available. Full-document Markdown files remain the source-of-truth reference.

## Versioning
- Documents are organized per spec version: pdf/{version}/ (for example pdf/340/).
- The active version is currently 340. Do not hardcode a version in code or docs; resolve paths from the selected version.
- Do not use other version folders (for example 350) unless the user explicitly selects that version.
- The app selects the version with `--version <ver>` (default: 340).

## Output layout
- Preferred AI input: pdf/{version}/sections/
- Source-of-truth full docs: pdf/{version}/md_output/
- Legacy workflow: pdf/md_output/
- Do not overwrite legacy output directories unless the user explicitly requests it.

## When to use which data source
- Use pdf/{version}/sections for: summarization, question answering, extraction, indexing, and RAG-style tasks.
- Use pdf/{version}/md_output for: source verification, human review, and fallback when a section file is missing.
- Use pdf/md_output only for the original legacy workflow unless the user asks to keep that path active.

## Conversion workflow
- When converting PDFs with MarkItDown, write to a separate output directory; do not overwrite existing docs in pdf/md_output.
- Write converted output to pdf/{version}/md_output for the selected version.
- Preserve original PDF stems when generating Markdown files.
- If a PDF name differs from the canonical map (for example XfsClasses or Xfsdep), keep the output file name stable and use metadata aliases when needed.

## Section splitting
- For AI input, prefer sectioned .md files with one section per file.
- Keep section files in per-document folders such as pdf/{version}/sections/XfsPin/.
- Each section file should carry frontmatter or equivalent metadata with at least: document, version, section_id, title, source PDF, and source line range.
- Prefer _index.md plus README.md for discovery and navigation.

## Search and lookup
- Prefer .index.json and .toc.json files when looking for section ranges and references.
- Use master.index.json when resolving command names or mapping names to documents.
- For user questions, start from the relevant section or command, then expand only if needed.

## Response style
- Keep AI answers tightly scoped to the relevant file, section, or command.
- Avoid dumping long raw documents unless the user explicitly asks for full text.
- When answering a spec question, answer from the most relevant section and cite the document and section title.

## Repository-specific rules
- Do not disturb the existing pdf/md_output legacy data without explicit instruction.
- Use --md-dir for any custom output directory in the PDF builder scripts.
- Preserve backward compatibility for the default build_index.py and build_toc.py workflow.
- For new work, prefer the pdf/{version}/sections pattern for AI tasks and the pdf/{version}/md_output pattern for conversion and validation.
