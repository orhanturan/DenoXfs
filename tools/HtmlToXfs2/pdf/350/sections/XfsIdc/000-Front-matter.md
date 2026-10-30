---
source: XfsIdc.pdf
document: XfsIdc
device_class: IDC
part: 8
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 93]
---

<!-- Front matter -->

CEN
CWA 16926-4
WORKSHOP
December 2022
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.50 - Part 4: Identification Card
Device Class Interface - Programmer's Reference
This CEN Workshop Agreement has been drafted and approved by a Workshop of representatives of interested parties, the
constitution of which is indicated in the foreword of this Workshop Agreement.
The formal process followed by the Workshop in the development of this Workshop Agreement has been endorsed by the
National Members of CEN but neither the National Members of CEN nor the CEN-CENELEC Management Centre can be held
accountable for the technical content of this CEN Workshop Agreement or possible conflicts with standards or legislation.
This CEN Workshop Agreement can in no way be held as being an official standard developed by CEN and its Members.
This CEN Workshop Agreement is publicly available as a reference document from the CEN Members National Standard Bodies.
CEN members are the national standards bodies of Austria, Belgium, Bulgaria, Croatia, Cyprus, Czech Republic, Denmark, Estonia, Finland, France,
Germany, Greece, Hungary, Iceland, Ireland, Italy, Latvia, Lithuania, Luxembourg, Malta, Netherlands, Norway, Poland, Portugal, Republic of North
Macedonia, Romania, Serbia, Slovakia, Slovenia, Spain, Sweden, Switzerland, Türkiye and United Kingdom.
EUROPEAN COMMITTEE FOR STANDARDIZATION
COMITÉ EUROPÉEN DE NORMALISATION
EUROPÄISCHES KOMITEE FÜR NORMUNG
CEN-CENELEC Management Centre: Rue de la Science 23, B-1040 Brussels
© 2022 CEN All rights of exploitation in any form and by any means reserved worldwide for CEN national Members.
Ref. No.:CWA 16926-4:2022 E

CWA 16926-4:2022 (E)
Table of Contents
European Foreword ...................................................................................................... 4
1. Introduction .......................................................................................................... 8
1.1 Background to Release 3.50 ....................................................................................................... 8
1.2 XFS Service-Specific Programming ........................................................................................... 8
2. Identification Card Readers and Writers ......................................................... 10
2.1 Support for EMV Intelligent Contactless Card Readers ......................................................... 11
3. References ......................................................................................................... 12
4. Info Commands ................................................................................................. 13
4.1 WFS_INF_IDC_STATUS ............................................................................................................. 13
4.2 WFS_INF_IDC_CAPABILITIES .................................................................................................. 19
4.3 WFS_INF_IDC_FORM_LIST ....................................................................................................... 24
4.4 WFS_INF_IDC_QUERY_FORM .................................................................................................. 25
4.5 WFS_INF_IDC_QUERY_IFM_IDENTIFIER ................................................................................ 27
4.6 WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS ........................................................... 28
5. Execute Commands .......................................................................................... 29
5.1 WFS_CMD_IDC_READ_TRACK ................................................................................................ 29
5.2 WFS_CMD_IDC_WRITE_TRACK .............................................................................................. 31
5.3 WFS_CMD_IDC_EJECT_CARD ................................................................................................. 33
5.4 WFS_CMD_IDC_RETAIN_CARD ............................................................................................... 35
5.5 WFS_CMD_IDC_RESET_COUNT .............................................................................................. 36
5.6 WFS_CMD_IDC_SETKEY .......................................................................................................... 37
5.7 WFS_CMD_IDC_READ_RAW_DATA ........................................................................................ 38
5.8 WFS_CMD_IDC_WRITE_RAW_DATA ...................................................................................... 42
5.9 WFS_CMD_IDC_CHIP_IO .......................................................................................................... 44
5.10 WFS_CMD_IDC_RESET ............................................................................................................. 46
5.11 WFS_CMD_IDC_CHIP_POWER ................................................................................................ 47
5.12 WFS_CMD_IDC_PARSE_DATA ................................................................................................ 48
5.13 WFS_CMD_IDC_SET_GUIDANCE_LIGHT ............................................................................... 49
5.14 WFS_CMD_IDC_POWER_SAVE_CONTROL ........................................................................... 51
5.15 WFS_CMD_IDC_PARK_CARD .................................................................................................. 52
5.16 WFS_CMD_IDC_EMVCLESS_CONFIGURE ............................................................................. 53
5.17 WFS_CMD_IDC_EMVCLESS_PERFORM_TRANSACTION .................................................... 55
5.18 WFS_CMD_IDC_EMVCLESS_ISSUERUPDATE ...................................................................... 60
5.19 WFS_CMD_IDC_SYNCHRONIZE_COMMAND ......................................................................... 62
6. Events ................................................................................................................. 63
6.1 WFS_EXEE_IDC_INVALIDTRACKDATA .................................................................................. 63
2

CWA 16926-4:2022 (E)
6.2 WFS_EXEE_IDC_MEDIAINSERTED ......................................................................................... 64
6.3 WFS_SRVE_IDC_MEDIAREMOVED ......................................................................................... 65
6.4 WFS_EXEE_IDC_MEDIARETAINED ......................................................................................... 66
6.5 WFS_EXEE_IDC_INVALIDMEDIA ............................................................................................. 67
6.6 WFS_SRVE_IDC_CARDACTION ............................................................................................... 68
6.7 WFS_USRE_IDC_RETAINBINTHRESHOLD ............................................................................. 69
6.8 WFS_SRVE_IDC_MEDIADETECTED ........................................................................................ 70
6.9 WFS_SRVE_IDC_RETAINBINREMOVED ................................................................................. 71
6.10 WFS_SRVE_IDC_RETAINBININSERTED ................................................................................. 72
6.11 WFS_EXEE_IDC_INSERTCARD ................................................................................................ 73
6.12 WFS_SRVE_IDC_DEVICEPOSITION ........................................................................................ 74
6.13 WFS_SRVE_IDC_POWER_SAVE_CHANGE ............................................................................ 75
6.14 WFS_EXEE_IDC_TRACKDETECTED ....................................................................................... 76
6.15 WFS_EXEE_IDC_EMVCLESSREADSTATUS ........................................................................... 77
6.16 WFS_SRVE_IDC_MEDIARETAINED ......................................................................................... 78
7. Form Description ............................................................................................... 79
8. C-Header file ...................................................................................................... 82
9. Intelligent Contactless Card Sequence Diagrams .......................................... 93
9.1 Single Tap Transaction Without Issuer Update Processing ................................................. 94
9.2 Double Tap Transaction With Issuer Update Processing ...................................................... 95
9.3 Card Removed Before Completion .......................................................................................... 96
Appendix A. Diagram Source .................................................................................. 97
3

CWA 16926-4:2022 (E)
