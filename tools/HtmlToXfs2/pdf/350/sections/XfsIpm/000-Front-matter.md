---
source: XfsIpm.pdf
document: XfsIpm
device_class: IPM
part: 19
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 109]
---

<!-- Front matter -->

CEN
CWA 16926-18
WORKSHOP
December 2022
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.50 - Part 18: Item Processing
Module Device Class Interface - Programmer's Reference
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
Ref. No.:CWA 16926-18:2022 E

CWA 16926-18:2022 (E)
Table of Contents
European Foreword ...................................................................................................... 4
1. Introduction .......................................................................................................... 8
1.1 Background to Release 3.50 ....................................................................................................... 8
1.2 XFS Service-Specific Programming ........................................................................................... 8
2. Item Processing Module ................................................................................... 10
2.1 Devices with a Stacker .............................................................................................................. 12
2.1.1 Automatic Accept/Refuse ...................................................................................................................12
2.1.2 Application Controlled Accept/Refuse ...............................................................................................12
2.2 Device without a Stacker ........................................................................................................... 14
2.2.1 Multi-Feed Devices without a Stacker ................................................................................................14
2.2.2 Single-Feed Devices ............................................................................................................................14
3. References ......................................................................................................... 15
4. Info Commands ................................................................................................. 16
4.1 WFS_INF_IPM_STATUS ............................................................................................................ 16
4.2 WFS_INF_IPM_CAPABILITIES .................................................................................................. 23
4.3 WFS_INF_IPM_CODELINE_MAPPING ..................................................................................... 31
4.4 WFS_INF_IPM_MEDIA_BIN_INFO ............................................................................................ 32
4.5 WFS_INF_IPM_TRANSACTION_STATUS ................................................................................ 35
4.6 WFS_INF_IPM_MEDIA_BIN_CAPABILITIES ............................................................................ 39
5. Execute Commands .......................................................................................... 41
5.1 WFS_CMD_IPM_MEDIA_IN ....................................................................................................... 41
5.2 WFS_CMD_IPM_MEDIA_IN_END .............................................................................................. 46
5.3 WFS_CMD_IPM_MEDIA_IN_ROLLBACK ................................................................................. 49
5.4 WFS_CMD_IPM_READ_IMAGE ................................................................................................ 51
5.5 WFS_CMD_IPM_SET_DESTINATION ....................................................................................... 56
5.6 WFS_CMD_IPM_PRESENT_MEDIA .......................................................................................... 57
5.7 WFS_CMD_IPM_RETRACT_MEDIA .......................................................................................... 59
5.8 WFS_CMD_IPM_PRINT_TEXT .................................................................................................. 61
5.9 WFS_CMD_IPM_SET_MEDIA_BIN_INFO ................................................................................. 62
5.10 WFS_CMD_IPM_RESET ............................................................................................................ 63
5.11 WFS_CMD_IPM_SET_GUIDANCE_LIGHT ............................................................................... 65
5.12 WFS_CMD_IPM_GET_NEXT_ITEM ........................................................................................... 66
5.13 WFS_CMD_IPM_ACTION_ITEM ................................................................................................ 68
5.14 WFS_CMD_IPM_EXPEL_MEDIA ............................................................................................... 70
5.15 WFS_CMD_IPM_GET_IMAGE_AFTER_PRINT ........................................................................ 71
5.16 WFS_CMD_IPM_ACCEPT_ITEM ............................................................................................... 73
5.17 WFS_CMD_IPM_SUPPLY_REPLENISH ................................................................................... 74
5.18 WFS_CMD_IPM_POWER_SAVE_CONTROL ........................................................................... 75
2

CWA 16926-18:2020 (E)
5.19 WFS_CMD_IPM_SET_MODE ..................................................................................................... 76
5.20 WFS_CMD_IPM_SYNCHRONIZE_COMMAND ......................................................................... 77
6. Events ................................................................................................................. 78
6.1 WFS_EXEE_IPM_NOMEDIA ...................................................................................................... 78
6.2 WFS_EXEE_IPM_MEDIAINSERTED ......................................................................................... 79
6.3 WFS_USRE_IPM_MEDIABINTHRESHOLD .............................................................................. 80
6.4 WFS_SRVE_IPM_MEDIABININFOCHANGED .......................................................................... 81
6.5 WFS_EXEE_IPM_MEDIABINERROR ........................................................................................ 82
6.6 WFS_SRVE_IPM_MEDIATAKEN ............................................................................................... 83
6.7 WFS_USRE_IPM_TONERTHRESHOLD ................................................................................... 84
6.8 WFS_USRE_IPM_SCANNERTHRESHOLD .............................................................................. 85
6.9 WFS_USRE_IPM_INKTHRESHOLD .......................................................................................... 86
6.10 WFS_SRVE_IPM_MEDIADETECTED ........................................................................................ 87
6.11 WFS_EXEE_IPM_MEDIAPRESENTED ..................................................................................... 88
6.12 WFS_EXEE_IPM_MEDIAREFUSED .......................................................................................... 89
6.13 WFS_EXEE_IPM_MEDIADATA ................................................................................................. 91
6.14 WFS_USRE_IPM_MICRTHRESHOLD ....................................................................................... 94
6.15 WFS_EXEE_IPM_MEDIAREJECTED ........................................................................................ 95
6.16 WFS_SRVE_IPM_DEVICEPOSITION ........................................................................................ 96
6.17 WFS_SRVE_IPM_POWER_SAVE_CHANGE ............................................................................ 97
6.18 WFS_SRVE_IPM_SHUTTERSTATUSCHANGED ..................................................................... 98
7. Command and Event Flows .............................................................................. 99
7.1 Devices with Stacker ................................................................................................................. 99
7.1.1 Bunch Media Processing (OK flow) ...................................................................................................99
7.1.2 Bunch Media Processing (Some Media Items Returned) ..................................................................100
7.1.3 Bunch Media Processing with Errors ................................................................................................101
7.1.4 Bunch media processing with Rollback ............................................................................................102
7.1.5 Bunch media processing with Retract ...............................................................................................103
7.1.6 Bunch Media Processing - Application Refuse Decision (All OK flow) ..........................................103
7.1.7 Bunch Media Processing - Application Refuse Decision (Some items refused) ...............................104
7.2 Devices without Stacker .......................................................................................................... 106
7.2.1 Bunch Media Processing (OK flow) .................................................................................................106
7.2.2 Bunch Media Processing (Some Media Items Returned) ..................................................................107
7.2.3 Bunch Media Processing with Errors ................................................................................................108
8. ATM Mixed Media Transaction Flow – Application Guidelines.................... 110
9. C-Header File ................................................................................................... 111
3

CWA 16926-18:2022 (E)
