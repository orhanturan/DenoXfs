---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "Sample 1: Simple framing"
type: section
source_lines: [4243, 4335]
---

<!-- Sample 1: Simple framing -->

Sample 1: Simple framing

XFSFORM "Multiple Balances"    When printed with the following
field list:
| BEGIN                               |   Account[0]=0123456789123001  |     |
| ----------------------------------- | ------------------------------ | --- |
|    UNIT INCH, 16, 16                |   Account[1]=0123456789123002  |     |
|    SIZE 91, 64                      |   Account[2]=0123456789123003  |     |
|    VERSION 1, 0, "13/09/96", "XFS"  |   Balance[0]=$17465.12         |     |

|    LANGUAGE 0x0409        | Balance[1] =$2458.23   |            |
| ------------------------- | ---------------------- | ---------- |
| XFSFIELD "Account Title"  |   Balance[2]=$6542.78  |            |
|    BEGIN                  |   Will print:          |            |
|       POSITION 15, 4      |   Account              | Balance    |
|       SIZE 30, 4          |   012345678912300      | $17465.12  |
1
|       CLASS STATIC  |   012345678912300 | $2458.23  |
| ------------------- | ----------------- | --------- |
2
|       HORIZONTAL CENTER  |   012345678912300 | $6542.78  |
| ------------------------ | ----------------- | --------- |
3
|       INITIALVALUE "Account"  |     |     |
| ----------------------------- | --- | --- |
|    END                        |     |     |
   XFSFIELD "Balance Title"    When printed with the following
field list:
|    BEGIN                      |   Account[0]=0123456789123001  |            |
| ----------------------------- | ------------------------------ | ---------- |
|       POSITION 45, 4          |   Balance[0]=$17465.12         |            |
|       SIZE 30, 4              |   Will print:                  |            |
|       CLASS STATIC            |                                |            |
|       HORIZONTAL CENTER       |   Account                      | Balance    |
|       INITIALVALUE "Balance"  |   012345678912300              | $17465.12  |
1
107

CWA 16926-3:2020 (E)
|    END                 |     |     |     |
| ---------------------- | --- | --- | --- |
|                        |     |     |     |
|    XFSFIELD "Account"  |     |     |     |
|    BEGIN               |     |     |     |
|       POSITION 15, 8   |     |     |     |
|       SIZE 30, 4       |     |     |     |
|       INDEX 10, 0, 3   |     |     |     |
|    END //"Account"     |     |     |     |
|    XFSFIELD "Balance"  |     |     |     |
|    BEGIN               |     |     |     |
|       POSITION 45, 8   |     |     |     |
|       SIZE 30, 4       |     |     |     |

      INDEX 10, 0, 3
|       HORIZONTAL RIGHT        |     |     |     |
| ----------------------------- | --- | --- | --- |
|    END //"Balance"            |     |     |     |
|    XFSFRAME "Account Title"   |     |     |     |
|    BEGIN                      |     |     |     |
|       POSITION 15, 4          |     |     |     |
|       FRAMES "Account Title"  |     |     |     |
|       SIZE 30, 4              |     |     |     |
|       STYLE DOUBLE_THIN       |     |     |     |
|    END                        |     |     |     |

   XFSFRAME "Balance Title"
|    BEGIN                       |     |     |     |
| ------------------------------ | --- | --- | --- |
|        POSITION 45, 4          |     |     |     |
|        FRAMES "Balance Title"  |     |     |     |
|        SIZE 30, 4              |     |     |     |
|        STYLE DOUBLE_THIN       |     |     |     |
|    END                         |     |     |     |
|    XFSFRAME "Account"          |     |     |     |
|    BEGIN                       |     |     |     |
|        POSITION 15, 8          |     |     |     |

       FRAMES "Account"
|        SIZE 30, 34        |     |     |     |
| ------------------------- | --- | --- | --- |
|        STYLE DOUBLE_THIN  |     |     |     |
|    END                    |     |     |     |
|    XFSFRAME "Balance"     |     |     |     |
|    BEGIN                  |     |     |     |
|        POSITION 45, 8     |     |     |     |
|        FRAMES "Balance"   |     |     |     |
|        SIZE 30, 34        |     |     |     |
|        STYLE DOUBLE_THIN  |     |     |     |

   END
| END  |     |     |     |
| ---- | --- | --- | --- |
