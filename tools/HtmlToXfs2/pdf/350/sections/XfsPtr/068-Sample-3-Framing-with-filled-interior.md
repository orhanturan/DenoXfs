---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "Sample 3: Framing with filled interior"
type: section
source_lines: [4400, 4437]
---

<!-- Sample 3: Framing with filled interior -->

Sample 3: Framing with filled interior
|     |     |     |     |
| --- | --- | --- | --- |

| XFSFORM "Bank Details"  | When printed with the following  |     |     |
| ----------------------- | -------------------------------- | --- | --- |
field list:
| BEGIN                                      |   Owner = Mr/Mrs Jean Leroy           |     |     |
| ------------------------------------------ | ------------------------------------- | --- | --- |
|    UNIT INCH, 16, 16                       |                   21560 Hagerty Road  |     |     |
|    SIZE 121, 64                            |                   Troy, MI.           |     |     |
|    VERSION 1, 0, "13/09/96", "XFS Editor"  |                                       |     |     |
|    LANGUAGE 0x0409                         |   will print:                         |     |     |
|    XFSFIELD "Owner"                        |                                       |     |     |
|    BEGIN                                   |                                       |     |     |
|       POSITION 20, 11                      |                                       |     |     |
|                                            |                                       |     |     |
      SIZE 35, 9
|       CLASS REQUIRED  |   Mr/Mrs Jean  |     |     |
| --------------------- | -------------- | --- | --- |
Leroy
|        VERTICAL TOP  |   21560 Hagerty  |     |     |
| -------------------- | ---------------- | --- | --- |
Road
|    END                     |   Troy, MI.  |     |     |
| -------------------------- | ------------ | --- | --- |
|    XFSFRAME "Owner Frame"  |              |     |     |
|    BEGIN                   |              |     |     |
|        POSITION 19, 10     |              |     |     |

       FRAMES "Owner"
|        SIZE 37, 11      |     |     |     |
| ----------------------- | --- | --- | --- |
|        FILLCOLOR GRAY   |     |     |     |
|        FILLSTYLE CROSS  |     |     |     |
|    END                  |     |     |     |
| END                     |     |     |     |
