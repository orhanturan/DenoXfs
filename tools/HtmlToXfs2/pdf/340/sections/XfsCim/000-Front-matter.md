---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 169]
---

<!-- Front matter -->

CEN
CWA 16926-15
WORKSHOP
February 2020
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 15: Cash-In Module Device
Class Interface - Programmer's Reference
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
Ref. No.:CWA 16926-15:2020 E

CWA 16926-15:2020 (E)
Table of Contents
European Foreword ...................................................................................................... 6
1. Introduction ........................................................................................................ 10
1.1 Background to Release 3.40 ..................................................................................................... 10
1.2 XFS Service-Specific Programming ......................................................................................... 10
2. Cash-In Module .................................................................................................. 12
3. References ......................................................................................................... 13
4. Note Classification ............................................................................................ 14
5. Info Commands ................................................................................................. 15
5.1 WFS_INF_CIM_STATUS ............................................................................................................ 15
5.2 WFS_INF_CIM_CAPABILITIES .................................................................................................. 21
5.3 WFS_INF_CIM_CASH_UNIT_INFO ........................................................................................... 28
5.4 WFS_INF_CIM_TELLER_INFO .................................................................................................. 38
5.5 WFS_INF_CIM_CURRENCY_EXP ............................................................................................. 40
5.6 WFS_INF_CIM_BANKNOTE_TYPES ........................................................................................ 41
5.7 WFS_INF_CIM_CASH_IN_STATUS .......................................................................................... 42
5.8 WFS_INF_CIM_GET_P6_INFO .................................................................................................. 44
5.9 WFS_INF_CIM_GET_P6_SIGNATURE...................................................................................... 45
5.10 WFS_INF_CIM_GET_ITEM_INFO .............................................................................................. 47
5.11 WFS_INF_CIM_POSITION_CAPABILITIES .............................................................................. 49
5.12 WFS_INF_CIM_REPLENISH_TARGET ..................................................................................... 51
5.13 WFS_INF_CIM_DEVICELOCK_STATUS................................................................................... 52
5.14 WFS_INF_CIM_CASH_UNIT_CAPABILITIES ........................................................................... 53
5.15 WFS_INF_CIM_DEPLETE_SOURCE......................................................................................... 55
5.16 WFS_INF_CIM_GET_ALL_ITEMS_INFO................................................................................... 56
5.17 WFS_INF_CIM_GET_BLACKLIST ............................................................................................. 60
5.18 WFS_INF_CIM_GET_CLASSIFICATION_LIST ......................................................................... 61
5.19 WFS_INF_CIM_CASH_UNIT_COUNT_STATUS ....................................................................... 63
5.20 WFS_INF_CIM_PRESENT_STATUS ......................................................................................... 65
6. Execute Commands .......................................................................................... 67
6.1 WFS_CMD_CIM_CASH_IN_START .......................................................................................... 67
6.2 WFS_CMD_CIM_CASH_IN ........................................................................................................ 69
6.3 WFS_CMD_CIM_CASH_IN_END ............................................................................................... 72
6.4 WFS_CMD_CIM_CASH_IN_ROLLBACK .................................................................................. 74
6.5 WFS_CMD_CIM_RETRACT ....................................................................................................... 76
6.6 WFS_CMD_CIM_OPEN_SHUTTER ........................................................................................... 79
6.7 WFS_CMD_CIM_CLOSE_SHUTTER ......................................................................................... 81
2

CWA 16926-15:2020 (E)
6.8 WFS_CMD_CIM_SET_TELLER_INFO....................................................................................... 82
6.9 WFS_CMD_CIM_SET_CASH_UNIT_INFO ................................................................................ 83
6.10 WFS_CMD_CIM_START_EXCHANGE ...................................................................................... 85
6.11 WFS_CMD_CIM_END_EXCHANGE .......................................................................................... 88
6.12 WFS_CMD_CIM_OPEN_SAFE_DOOR...................................................................................... 89
6.13 WFS_CMD_CIM_RESET ............................................................................................................ 90
6.14 WFS_CMD_CIM_CONFIGURE_CASH_IN_UNITS .................................................................... 92
6.15 WFS_CMD_CIM_CONFIGURE_NOTETYPES ........................................................................... 94
6.16 WFS_CMD_CIM_CREATE_P6_SIGNATURE ............................................................................ 95
6.17 WFS_CMD_CIM_SET_GUIDANCE_LIGHT ............................................................................... 98
6.18 WFS_CMD_CIM_CONFIGURE_NOTE_READER ................................................................... 100
6.19 WFS_CMD_CIM_COMPARE_P6_SIGNATURE ...................................................................... 101
6.20 WFS_CMD_CIM_POWER_SAVE_CONTROL ......................................................................... 103
6.21 WFS_CMD_CIM_REPLENISH .................................................................................................. 104
6.22 WFS_CMD_CIM_SET_CASH_IN_LIMIT .................................................................................. 107
6.23 WFS_CMD_CIM_CASH_UNIT_COUNT ................................................................................... 110
6.24 WFS_CMD_CIM_DEVICE_LOCK_CONTROL ......................................................................... 112
6.25 WFS_CMD_CIM_SET_MODE .................................................................................................. 115
6.26 WFS_CMD_CIM_PRESENT_MEDIA ........................................................................................ 116
6.27 WFS_CMD_CIM_DEPLETE ...................................................................................................... 118
6.28 WFS_CMD_CIM_SET_BLACKLIST ......................................................................................... 120
6.29 WFS_CMD_CIM_SYNCHRONIZE_COMMAND ....................................................................... 121
6.30 WFS_CMD_CIM_SET_CLASSIFICATION_LIST ..................................................................... 122
6.31 WFS_CMD_CIM_PREPARE_PRESENT .................................................................................. 123
7. Events ............................................................................................................... 124
7.1 WFS_SRVE_CIM_SAFEDOOROPEN ...................................................................................... 124
7.2 WFS_SRVE_CIM_SAFEDOORCLOSED ................................................................................. 125
7.3 WFS_USRE_CIM_CASHUNITTHRESHOLD ........................................................................... 126
7.4 WFS_SRVE_CIM_CASHUNITINFOCHANGED ....................................................................... 127
7.5 WFS_SRVE_CIM_TELLERINFOCHANGED ............................................................................ 128
7.6 WFS_EXEE_CIM_CASHUNITERROR ..................................................................................... 129
7.7 WFS_SRVE_CIM_ITEMSTAKEN ............................................................................................. 130
7.8 WFS_SRVE_CIM_COUNTS_CHANGED ................................................................................. 131
7.9 WFS_EXEE_CIM_INPUTREFUSE ........................................................................................... 132
7.10 WFS_SRVE_CIM_ITEMSPRESENTED.................................................................................... 133
7.11 WFS_SRVE_CIM_ITEMSINSERTED ....................................................................................... 134
7.12 WFS_EXEE_CIM_NOTEERROR .............................................................................................. 135
7.13 WFS_EXEE_CIM_SUBCASHIN ............................................................................................... 136
7.14 WFS_SRVE_CIM_MEDIADETECTED ...................................................................................... 137
7.15 WFS_EXEE_CIM_INPUT_P6 .................................................................................................... 138
7.16 WFS_EXEE_CIM_INFO_AVAILABLE...................................................................................... 139
3

CWA 16926-15:2020 (E)
7.17 WFS_EXEE_CIM_INSERTITEMS ............................................................................................. 140
7.18 WFS_SRVE_CIM_DEVICEPOSITION ...................................................................................... 141
7.19 WFS_SRVE_CIM_POWER_SAVE_CHANGE ......................................................................... 142
7.20 WFS_EXEE_CIM_INCOMPLETEREPLENISH ........................................................................ 143
7.21 WFS_EXEE_CIM_INCOMPLETEDEPLETE ............................................................................ 144
7.22 WFS_SRVE_CIM_SHUTTERSTATUSCHANGED ................................................................... 145
7.23 WFS_SRVE_CIM_COUNTACCURACYCHANGED ................................................................. 146
8. ATM Cash-In Transaction Flow - Application Guidelines ............................ 147
8.1 OK Transaction (Explicit Shutter Control) ............................................................................ 148
8.2 Cancellation by Customer (Explicit Shutter Control) ........................................................... 149
8.3 Stacker Becomes Full (Explicit Shutter Control) .................................................................. 150
8.4 Bill Recognition Error (Explicit Shutter Control) .................................................................. 152
8.5 OK Transaction (Explicit Shutter Control) - Level 2 and 3 Note classification Supported153
8.6 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN Refused Notes (Explicit
Shutter Control) ................................................................................................................................. 154
8.7 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK (Explicit
Shutter Control) ................................................................................................................................. 156
8.8 OK Transaction (Implicit Shutter Control) ............................................................................. 158
8.9 Customer Initiates Returning Of Previously Recognized Items (Implicit Shutter Control)159
8.10 OK Transaction - (Implicit Shutter Control and WFS_EXEE_CIM_SUBCASHIN event
supported) .......................................................................................................................................... 160
8.11 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN (Implicit Shutter Control
and Implicit Present Control) ........................................................................................................... 161
8.12 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK (Implicit
Shutter Control and Implicit Present Control)................................................................................ 163
8.13 Retracting Items When Multiple Bunches Are Returned During WFS_CMD_CIM_CASH_IN
(Implicit Shutter Control and Implicit Present Control) ................................................................. 165
8.14 Bill Recognition Error (WFS_CMD_CIM_PRESENT_MEDIA Command Supported) ......... 166
8.15 Cancellation by Customer (Implicit Shutter Control and
WFS_CMD_CIM_PRESENT_MEDIA Command Supported) .......................................................... 167
8.16 Multiple Bunch Timeout Handling .......................................................................................... 168
8.16.1 No Items Inserted ..............................................................................................................................168
8.16.2 First Bunch Not Taken ......................................................................................................................168
8.16.3 Last Bunch Taken .............................................................................................................................169
8.17 Exchange using DEPOSITINTO (Implicit Shutter Control) .................................................. 171
8.18 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN Refused Notes (using
WFS_CMD_CIM_PREPARE_PRESENT) .......................................................................................... 173
8.19 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK (using
WFS_CMD_CIM_PREPARE_PRESENT) .......................................................................................... 175
9. ATM Mixed Media Transaction Flow – Application Guidelines.................... 177
9.1 Mixed Media OK Transaction .................................................................................................. 179
9.2 Mixed Media Cancellation by Customer ................................................................................ 181
9.3 Mixed Media Cancellation by Customer on Cash Part Only ................................................ 182
9.4 Mixed Media Multiple Refused Items ..................................................................................... 183
4

CWA 16926-15:2020 (E)
10. Rules for Cash Unit Exchange ....................................................................... 185
11. Events Associated with Cash Unit Status Changes ..................................... 188
11.1 One Physical Cash Unit Goes HIGH....................................................................................... 188
11.2 Last Physical Cash Unit Goes HIGH ...................................................................................... 189
11.3 One Physical Cash Unit Goes INOP ....................................................................................... 190
11.4 Last Physical Cash Unit Goes FULL ...................................................................................... 191
12. C - Header file .................................................................................................. 192
5

CWA 16926-15:2020 (E)
