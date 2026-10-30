---
source: XfsChk.pdf
document: XfsChk
device_class: CHK
part: 14
section_id: ""
title: "7. Forms Language Usage"
type: section
source_lines: [1378, 1400]
---

<!-- 7. Forms Language Usage -->

7. Forms Language Usage
This section covers the usage of the forms language to accommodate check readers.
The forms language contains the FORMAT attribute in the XFSFIELD section. For check readers, the formatstring
is used to generate the delimiters for the check fields. For forms intended for use with check readers, the FORMAT
attribute is required. The FORMAT keyword is application defined, however may be interpreted by the Service
Provider. The following illustrates the use of the FORMAT keyword:
field Amount FORMAT ":NNNNNNNNNN:"
field AccountNum FORMAT "0000NNNNNN<"
field RouteTransit FORMAT ";NNNNNNNNN;"
Field names are not limited to the sample field names above. Punctuation marks are used in place of the standard
field separators. A capital N means a number to be read and returned. A zero (“0”) means an optional number
which, if present, is read and returned. Note that all fields on a check encoder line that have optional numbers
should place the zeros on the same end of the format string as an aid to the Service Provider in parsing the code line
(for instance, most check readers read the MICR line right to left, so optional numbers should always be on the left
side of fields which have them.).
Fields are processed in the order that they appear within the Form definition. If the device supports reading multiple
fonts, the FONT attribute of the XFSFIELD section might be significant. The name of the font (e.g. CMC7, E13B,
etc), given here, will cause the check reader to use the appropriate font.
For endorsing checks, the field description specifies the “front” or “back” of the check using the SIDE attribute, and
position relative to the trailing or (usually) leading edge of the check.
43

CWA 16926-7:2020 (E)
