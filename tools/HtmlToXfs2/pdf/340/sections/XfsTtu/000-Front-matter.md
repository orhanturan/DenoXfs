---
source: XfsTtu.pdf
document: XfsTtu
device_class: TTU
part: 10
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 74]
---

<!-- Front matter -->

CEN
CWA 16926-9
WORKSHOP
February 2020
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 9: Text Terminal Unit
Device Class Interface - Programmer's Reference
This CEN Workshop Agreement has been drafted and approved by a Workshop of representatives of interested parties, the
constitution of which is indicated in the foreword of this Workshop Agreement.
The formal process followed by the Workshop in the development of this Workshop Agreement has been endorsed by the
National Members of CEN but neither the National Members of CEN nor the CEN-CENELEC Management Centre can be held
accountable for the technical content of this CEN Workshop Agreement or possible conflicts with standards or legislation.
This CEN Workshop Agreement can in no way be held as being an official standard developed by CEN and its Members.
This CEN Workshop Agreement is publicly available as a reference document from the CEN Members National Standard Bodies.
CEN members are the national standards bodies of Austria, Belgium, Bulgaria, Croatia, Cyprus, Czech Republic, Denmark, Estonia, Finland,
France, Germany, Greece, Hungary, Iceland, Ireland, Italy, Latvia, Lithuania, Luxembourg, Malta, Netherlands, Norway, Poland, Portugal,
Republic of North Macedonia, Romania, Serbia, Slovakia, Slovenia, Spain, Sweden, Switzerland, Turkey and United Kingdom.
EUROPEAN COMMITTEE FOR STANDARDIZATION
COMITÉ EUROPÉEN DE NORMALISATION
EUROPÄISCHES KOMITEE FÜR NORMUNG
CEN-CENELEC Management Centre: Rue de la Science 23, B-1040 Brussels
© 2020 CEN All rights of exploitation in any form and by any means reserved worldwide for CEN national Members.
Ref. No.:CWA 16926-9:2020 E

Table of Contents
European Foreword ...................................................................................................... 4
1. Introduction .......................................................................................................... 8
1.1 Background to Release 3.40 ....................................................................................................... 8
1.2 XFS Service-Specific Programming ........................................................................................... 8
2. Text Terminal Units ........................................................................................... 10
3. References ......................................................................................................... 11
4. Info Commands ................................................................................................. 12
4.1 WFS_INF_TTU_STATUS ............................................................................................................ 12
4.2 WFS_INF_TTU_CAPABILITIES.................................................................................................. 15
4.3 WFS_INF_TTU_FORM_LIST ...................................................................................................... 18
4.4 WFS_INF_TTU_QUERY_FORM ................................................................................................. 19
4.5 WFS_INF_TTU_QUERY_FIELD ................................................................................................. 20
4.6 WFS_INF_TTU_KEY_DETAIL .................................................................................................... 22
5. Execute Commands .......................................................................................... 24
5.1 WFS_CMD_TTU_BEEP .............................................................................................................. 24
5.2 WFS_CMD_TTU_CLEARSCREEN ............................................................................................. 25
5.3 WFS_CMD_TTU_DISPLIGHT ..................................................................................................... 26
5.4 WFS_CMD_TTU_SET_LED ........................................................................................................ 27
5.5 WFS_CMD_TTU_SET_RESOLUTION ....................................................................................... 28
5.6 WFS_CMD_TTU_WRITE_FORM ............................................................................................... 29
5.7 WFS_CMD_TTU_READ_FORM ................................................................................................. 30
5.8 WFS_CMD_TTU_WRITE ............................................................................................................ 32
5.9 WFS_CMD_TTU_READ .............................................................................................................. 34
5.10 WFS_CMD_TTU_RESET ............................................................................................................ 37
5.11 WFS_CMD_TTU_DEFINE_KEYS ............................................................................................... 38
5.12 WFS_CMD_TTU_POWER_SAVE_CONTROL ........................................................................... 40
5.13 WFS_CMD_TTU_SET_LED_EX ................................................................................................. 41
5.14 WFS_CMD_TTU_SYNCHRONIZE_COMMAND ........................................................................ 42
6. Events ................................................................................................................. 43
6.1 WFS_EXEE_TTU_FIELDERROR ............................................................................................... 43
6.2 WFS_EXEE_TTU_FIELDWARNING ........................................................................................... 44
6.3 WFS_EXEE_TTU_KEY ............................................................................................................... 45
6.4 WFS_SRVE_TTU_DEVICEPOSITION ........................................................................................ 46
6.5 WFS_SRVE_TTU_POWER_SAVE_CHANGE ............................................................................ 47
7. Form and Field Definitions ............................................................................... 48
2

CWA 16926-9:2020 (E)
7.1 Definition Syntax ........................................................................................................................ 48
7.2 XFS form/media definition files in multi-vendor environments ............................................ 49
7.3 Form Definition .......................................................................................................................... 50
7.4 Field Definition .......................................................................................................................... 51
8. C - Header file .................................................................................................... 53
3

CWA 16926-9:2020 (E)
