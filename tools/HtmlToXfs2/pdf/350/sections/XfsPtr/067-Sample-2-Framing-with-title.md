---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "Sample 2: Framing with title"
type: section
source_lines: [4350, 4399]
---

<!-- Sample 2: Framing with title -->

Sample 2: Framing with title
|                         |                                    |     |     |
| ----------------------- | ---------------------------------- | --- | --- |
| XFSFORM "Bank Details"  |   When printed with the following  |     |     |
field list:
| BEGIN  |   Owner = Mr/Mrs Jean Leroy  |     |     |
| ------ | ---------------------------- | --- | --- |

|    UNIT INCH, 16, 16                       |                 21560 Hagerty Road  |                 |     |
| ------------------------------------------ | ----------------------------------- | --------------- | --- |
|    SIZE 121, 64                            |                   Troy, MI.         |                 |     |
|    VERSION 1, 0, "13/09/96", "XFS Editor"  |                                     |                 |     |
|    LANGUAGE 0x0409                         |   will print:                       |                 |     |
|    XFSFIELD "Owner Frame Title"            |                                     |                 |     |
|    BEGIN                                   |                                     |                 |     |
|       POSITION 24, 9                       |                                     | Acc ount Owner  |     |
|       SIZE 27, 3                           |   Mr/Mrs Jean                       |                 |     |
Leroy
|       CLASS STATIC  |   21560 Hagerty  |     |     |
| ------------------- | ---------------- | --- | --- |
Road
|       HORIZONTAL CENTER  |   Troy, MI.  |     |     |
| ------------------------ | ------------ | --- | --- |
108

CWA 16926-3:2022 (E)

      VERTICAL CENTER
|       INITIALVALUE "Account Owner"  |     |     |     |
| ----------------------------------- | --- | --- | --- |
|     END                             |     |     |     |
|     XFSFIELD "Owner"                |     |     |     |
|     BEGIN                           |     |     |     |
|         POSITION 20, 11             |     |     |     |
|         SIZE 35, 9                  |     |     |     |
|         CLASS REQUIRED              |     |     |     |
|         VERTICAL TOP                |     |     |     |
|    END //"Owner"                    |     |     |     |

    XFSFRAME "Owner Frame"
|     BEGIN                          |     |     |     |
| ---------------------------------- | --- | --- | --- |
|         POSITION 19, 10            |     |     |     |
|         FRAMES "Owner"             |     |     |     |
|         SIZE 37, 11                |     |     |     |
|         TITLE "Owner Frame Title"  |     |     |     |
|         HORIZONTAL CENTER          |     |     |     |
|    END                             |     |     |     |
| END                                |     |     |     |
