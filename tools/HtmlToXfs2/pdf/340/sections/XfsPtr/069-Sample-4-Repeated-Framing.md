---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "Sample 4: Repeated Framing"
type: section
source_lines: [4423, 4458]
---

<!-- Sample 4: Repeated Framing -->

Sample 4: Repeated Framing
|     |     |     |     |
| --- | --- | --- | --- |

XFSFORM "Smart Account Number"  When printed with the following
field list:
| BEGIN                 |   Account Number[0]=0  |     |     |
| --------------------- | ---------------------- | --- | --- |
|    UNIT INCH, 16, 16  |   Account Number[1]=1  |     |     |
|    SIZE 121, 64       |   Account Number[2]=2  |     |     |
   VERSION 1, 0, "13/09/96", "XFS Editor"    Account Number[3]=3
|     LANGUAGE 0x0409           |   Account Number[4]=4  |     |     |
| ----------------------------- | ---------------------- | --- | --- |
|    XFSFIELD "Account Number"  |   Account Number[5]=5  |     |     |
|    BEGIN                      |   Account Number[6]=6  |     |     |
|       POSITION 20, 8          |   Account Number[7]=7  |     |     |

|       SIZE 4, 4       | Account Number[8]=8    |     |     |
| --------------------- | ---------------------- | --- | --- |
|       INDEX 12, 4, 0  |   Account Number[9]=9  |     |     |
109

CWA 16926-3:2020 (E)
HORIZONTAL CENTER Account Number[10]=0
VERTICAL CENTER Account Number[11]=1
END
XFSFRAME "A/N Frame"
BEGIN
POSITION 20, 8 will print
SIZE 4, 4
REPEATONX 12, 4 0 1 2 3 4 5 6 7 8 9 0 1
END
END
110

CWA 16926-3:2020 (E)
