---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 338]
---

<!-- Front matter -->

CEN
CWA 16926-15
WORKSHOP
June 2025
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.51 - Part 15: Cash-In Module
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
© 2025 CEN All rights of exploitation in any form and by any means reserved worldwide for CEN national Members.
Ref. No.:CWA 16926-15:2025 E

CWA 16926-15:2025 (E)
Contents
European Foreword ............................................................................................................................. 6
1. Introduction .............................................................................................................................. 10
1.1 Background to Release 3.51 ............................................................................................................................ 10
1.2 XFS Service-Specific Programming ............................................................................................................... 10
2. Cash-In Module ......................................................................................................................... 11
3. References .................................................................................................................................. 12
4. Note Classification ................................................................................................................... 13
5. Info Commands ......................................................................................................................... 14
5.1 WFS_INF_CIM_STATUS........................................................................................................................................ 14
5.2 WFS_INF_CIM_CAPABILITIES ........................................................................................................................... 20
5.3 WFS_INF_CIM_CASH_UNIT_INFO ..................................................................................................................... 27
5.4 WFS_INF_CIM_TELLER_INFO ............................................................................................................................ 37
5.5 WFS_INF_CIM_CURRENCY_EXP ........................................................................................................................ 39
5.6 WFS_INF_CIM_BANKNOTE_TYPES .................................................................................................................. 40
5.7 WFS_INF_CIM_CASH_IN_STATUS ..................................................................................................................... 41
5.8 WFS_INF_CIM_GET_P6_INFO ............................................................................................................................. 43
5.9 WFS_INF_CIM_GET_P6_SIGNATURE ............................................................................................................... 44
5.10 WFS_INF_CIM_GET_ITEM_INFO ....................................................................................................................... 46
5.11 WFS_INF_CIM_POSITION_CAPABILITIES...................................................................................................... 48
5.12 WFS_INF_CIM_REPLENISH_TARGET .............................................................................................................. 50
5.13 WFS_INF_CIM_DEVICELOCK_STATUS ............................................................................................................ 51
5.14 WFS_INF_CIM_CASH_UNIT_CAPABILITIES ................................................................................................... 52
5.15 WFS_INF_CIM_DEPLETE_SOURCE ................................................................................................................... 54
5.16 WFS_INF_CIM_GET_ALL_ITEMS_INFO ............................................................................................................ 55
5.17 WFS_INF_CIM_GET_BLACKLIST ....................................................................................................................... 59
5.18 WFS_INF_CIM_GET_CLASSIFICATION_LIST.................................................................................................. 60
5.19 WFS_INF_CIM_CASH_UNIT_COUNT_STATUS ............................................................................................... 62
5.20 WFS_INF_CIM_PRESENT_STATUS ................................................................................................................... 64
6. E xecute Commands ................................................................................................................. 66
6.1 WFS_CMD_CIM_CASH_IN_START ..................................................................................................................... 66
6.2 WFS_CMD_CIM_CASH_IN .................................................................................................................................... 68
6.3 WFS_CMD_CIM_CASH_IN_END .......................................................................................................................... 71
6.4 WFS_CMD_CIM_CASH_IN_ROLLBACK ............................................................................................................ 73
6.5 WFS_CMD_CIM_RETRACT .................................................................................................................................. 75
6.6 WFS_CMD_CIM_OPEN_SHUTTER ..................................................................................................................... 78
2

CWA 16926-15:2025 (E)
6.7 WFS_CMD_CIM_CLOSE_SHUTTER ................................................................................................................... 80
6.8 WFS_CMD_CIM_SET_TELLER_INFO ................................................................................................................. 81
6.9 WFS_CMD_CIM_SET_CASH_UNIT_INFO.......................................................................................................... 82
6.10 WFS_CMD_CIM_START_EXCHANGE ................................................................................................................ 84
6.11 WFS_CMD_CIM_END_EXCHANGE ..................................................................................................................... 87
6.12 WFS_CMD_CIM_OPEN_SAFE_DOOR ................................................................................................................ 88
6.13 WFS_CMD_CIM_RESET ........................................................................................................................................ 89
6.14 WFS_CMD_CIM_CONFIGURE_CASH_IN_UNITS ............................................................................................. 92
6.15 WFS_CMD_CIM_CONFIGURE_NOTETYPES ................................................................................................... 94
6.16 WFS_CMD_CIM_CREATE_P6_SIGNATURE ..................................................................................................... 95
6.17 WFS_CMD_CIM_SET_GUIDANCE_LIGHT ........................................................................................................ 98
6.18 WFS_CMD_CIM_CONFIGURE_NOTE_READER ............................................................................................ 100
6.19 WFS_CMD_CIM_COMPARE_P6_SIGNATURE ............................................................................................... 101
6.20 WFS_CMD_CIM_POWER_SAVE_CONTROL .................................................................................................. 103
6.21 WFS_CMD_CIM_REPLENISH ............................................................................................................................ 104
6.22 WFS_CMD_CIM_SET_CASH_IN_LIMIT ........................................................................................................... 107
6.23 WFS_CMD_CIM_CASH_UNIT_COUNT ............................................................................................................ 110
6.24 WFS_CMD_CIM_DEVICE_LOCK_CONTROL .................................................................................................. 112
6.25 WFS_CMD_CIM_SET_MODE ............................................................................................................................. 115
6.26 WFS_CMD_CIM_PRESENT_MEDIA ................................................................................................................. 116
6.27 WFS_CMD_CIM_DEPLETE ................................................................................................................................ 118
6.28 WFS_CMD_CIM_SET_BLACKLIST ................................................................................................................... 121
6.29 WFS_CMD_CIM_SYNCHRONIZE_COMMAND .............................................................................................. 122
6.30 WFS_CMD_CIM_SET_CLASSIFICATION_LIST .............................................................................................. 123
6.31 WFS_CMD_CIM_PREPARE_PRESENT ............................................................................................................ 124
7. E vents ........................................................................................................................................ 125
7.1 WFS_SRVE_CIM_SAFEDOOROPEN ................................................................................................................ 125
7.2 WFS_SRVE_CIM_SAFEDOORCLOSED ............................................................................................................ 126
7.3 WFS_USRE_CIM_CASHUNITTHRESHOLD ................................................................................................... 127
7.4 WFS_SRVE_CIM_CASHUNITINFOCHANGED ............................................................................................... 128
7.5 WFS_SRVE_CIM_TELLERINFOCHANGED .................................................................................................... 129
7.6 WFS_EXEE_CIM_CASHUNITERROR ............................................................................................................... 130
7.7 WFS_SRVE_CIM_ITEMSTAKEN ....................................................................................................................... 131
7.8 WFS_SRVE_CIM_COUNTS_CHANGED ............................................................................................................ 132
7.9 WFS_EXEE_CIM_INPUTREFUSE ..................................................................................................................... 133
7.10 WFS_SRVE_CIM_ITEMSPRESENTED ............................................................................................................. 134
7.11 WFS_SRVE_CIM_ITEMSINSERTED ................................................................................................................ 135
7.12 WFS_EXEE_CIM_NOTEERROR ........................................................................................................................ 136
7.13 WFS_EXEE_CIM_SUBCASHIN ........................................................................................................................... 137
7.14 WFS_SRVE_CIM_MEDIADETECTED .............................................................................................................. 138
3

CWA 16926-15:2025 (E)
7.15 WFS_EXEE_CIM_INPUT_P6 .............................................................................................................................. 139
7.16 WFS_EXEE_CIM_INFO_AVAILABLE ............................................................................................................... 140
7.17 WFS_EXEE_CIM_INSERTITEMS ...................................................................................................................... 141
7.18 WFS_SRVE_CIM_DEVICEPOSITION ............................................................................................................... 142
7.19 WFS_SRVE_CIM_POWER_SAVE_CHANGE .................................................................................................... 143
7.20 WFS_EXEE_CIM_INCOMPLETEREPLENISH ................................................................................................ 144
7.21 WFS_EXEE_CIM_INCOMPLETEDEPLETE ..................................................................................................... 145
7.22 WFS_SRVE_CIM_SHUTTERSTATUSCHANGED........................................................................................... 146
7.23 WFS_SRVE_CIM_COUNTACCURACYCHANGED .......................................................................................... 147
7.24 WFS_EXEE_CIM_INCOMPLETERETRACT .................................................................................................... 148
8. A TM Cash-In Transaction Flow - Application Guidelines ....................................... 149
8.1 OK Transaction (Explicit Shutter Control) ............................................................................................... 150
8.2 Cancellation by Customer (Explicit Shutter Control) ........................................................................... 151
8.3 Stacker Becomes Full (Explicit Shutter Control) ................................................................................... 152
8.4 Bill Recognition Error (Explicit Shutter Control) .................................................................................. 154
8.5 OK Transaction (Explicit Shutter Control) - Level 2 and 3 Note classification Supported ..... 155
8.6 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN Refused Notes (Explicit Shutter
Control) ............................................................................................................................................................................ 156
8.7 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK (Explicit Shutter
Control) ............................................................................................................................................................................ 158
8.8 OK Transaction (Implicit Shutter Control) .............................................................................................. 160
8.9 Customer Initiates Returning Of Previously Recognized Items (Implicit Shutter Control) .. 161
8.10 OK Transaction - (Implicit Shutter Control and WFS_EXEE_CIM_SUBCASHIN event supported)
162
8.11 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN (Implicit Shutter Control and
Implicit Present Control) ........................................................................................................................................... 163
8.12 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK (Implicit Shutter
Control and Implicit Present Control) .................................................................................................................. 165
8.13 Retracting Items When Multiple Bunches Are Returned During WFS_CMD_CIM_CASH_IN
(Implicit Shutter Control and Implicit Present Control) ............................................................................... 167
8.14 Bill Recognition Error (WFS_CMD_CIM_PRESENT_MEDIA Command Supported) ..................... 168
8.15 Cancellation by Customer (Implicit Shutter Control and WFS_CMD_CIM_PRESENT_MEDIA
Command Supported) ................................................................................................................................................. 169
8.16 Multiple Bunch Timeout Handling .............................................................................................................. 170
8.16.1 No Items Inserted ........................................................................................................................................................ 170
8.16.2 First Bunch Not Taken .............................................................................................................................................. 170
8.17 Exchange using DEPOSITINTO (Implicit Shutter Control) ................................................................. 173
8.16.3 Last Bunch Taken ........................................................................................................................................................ 171
8.18 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN Refused Notes (using
WFS_CMD_CIM_PREPARE_PRESENT) ..................................................................................................................... 175
8.19 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK (using
WFS_CMD_CIM_PREPARE_PRESENT) ..................................................................................................................... 177
9. ATM Mixed Media Transaction Flow – Application Guidelines ............................ 179
9.1 Mixed Media OK Transaction ........................................................................................................................ 181
4

CWA 16926-15:2025 (E)
9.2 Mixed Media Cancellation by Customer .................................................................................................... 183
9.3 Mixed Media Cancellation by Customer on Cash Part Only ............................................................... 184
9.4 Mixed Media Multiple Refused Items ......................................................................................................... 185
10. Rules for Cash Unit Exchange ........................................................................................... 187
11. Events Associated with Cash Unit Status Changes .................................................... 190
11.1 One Physical Cash Unit Goes HIGH .............................................................................................................. 190
11.2 Last Physical Cash Unit Goes HIGH ............................................................................................................. 191
11.3 One Physical Cash Unit Goes INOP .............................................................................................................. 192
11.4 Last Physical Cash Unit Goes FULL .............................................................................................................. 193
12. A ppendix E – Cash Acceptor E2E Authentication ...................................................... 194
12.1 Secure Cash In Data Parameter Example Data ....................................................................................... 194
12.2 Secure E2E Transaction (Implicit Shutter Control) - OK .................................................................... 195
12.3 Secure E2E Transaction - Customer Returns Notes Using Rollback (Implicit Shutter Control)
197
12.4 Secure E2E Transaction - Customer Returns Notes Using Rollback But Does Not Remove The
Notes (Implicit Shutter Control) ............................................................................................................................. 199
12.5 Secure E2E Transaction - Customer Deposit Consists of Multiple Cash-in Transactions
(Implicit Shutter Control) .......................................................................................................................................... 201
13. C - Header file ......................................................................................................................... 203
5

CWA 16926-15:2025 (E)
Foreword
This CEN Workshop Agreement (CWA 16926-15:2025) has been developed in accordance with
the CEN-CENELEC Guide 29 “CEN/CENELEC Workshop Agreements – The rapid way to
consensus” and with the relevant provisions of CEN/CENELEC Internal Regulations - Part 2. It
was approved by the CEN Workshop “eXtensions for Financial Services”, the secretariat of which
is held by AFNOR, consisting of representatives of interested parties on 2023-10-10, the
constitution of which was supported by CEN following the public call for participation made on
1998-06-24. However, this CEN Workshop Agreement does not necessarily include all relevant
stakeholders.
The final text of this CEN Workshop Agreement was provided to CEN for publication on 2025-
24-28.
The following organizations and individuals developed and approved this CEN Workshop
•A greement:
• AURIGA SPA
• CIMA SPA
• DIEBOLD NIXDORF SYSTEMS GMBH
• FIS BANKING SOLUTIONS UK LTD (OTS)
• FUJITSU TECHNOLOGY SOLUTIONS
• GLORY LTD
• GRG BANKING EQUIPMENT HK CO LTD
• HITACHI CHANNEL SOLUTIONS CORP
• HYOSUNG TNS INC
• JIANGSU GUOGUANG ELECTRONIC INFORMATION TECHNOLOGY
• KAL
• KEBA HANDOVER AUTOMATION GMBH
• NCR FSG
• NEXUS SOFTWARE
• OBERTHUR CASH PROTECTION
• OKI ELECTRIC INDUSTRY SHENZHEN
• SALZBURGER BANKEN SOFTWARE
• SECURE INNOVATION
SIGMA SPA
6

CWA 16926-15:2025 (E)
The CWA is published as a multi-part document, consisting of:
Part 1: Application Programming Interface (API) - Service Provider Interface (SPI) - Programmer's
Reference
Part 2: Service Classes Definition - Programmer's Reference
Part 3: Printer and Scanning Device Class Interface - Programmer's Reference
Part 4: Identification Card Device Class Interface - Programmer's Reference
Part 5: Cash Dispenser Device Class Interface - Programmer's Reference
Part 6: PIN Keypad Device Class Interface - Programmer's Reference
Part 7: Check Reader/Scanner Device Class Interface - Programmer's Reference
Part 8: Depository Device Class Interface - Programmer's Reference
Part 9: Text Terminal Unit Device Class Interface - Programmer's Reference
Part 10: Sensors and Indicators Unit Device Class Interface - Programmer's Reference
Part 11: Vendor Dependent Mode Device Class Interface - Programmer's Reference
Part 12: Camera Device Class Interface - Programmer's Reference
Part 13: Alarm Device Class Interface - Programmer's Reference
Part 14: Card Embossing Unit Device Class Interface - Programmer's Reference
Part 15: Cash-In Module Device Class Interface - Programmer's Reference
Part 16: Card Dispenser Device Class Interface - Programmer's Reference
Part 17: Barcode Reader Device Class Interface - Programmer's Reference
Part 18: Item Processing Module Device Class Interface - Programmer's Reference
Part 19: Biometrics Device Class Interface - Programmer's Reference
Parts 20 - 28: Reserved for future use.
Parts 29 through 47 constitute an optional addendum to this CWA. They define the integration
between the SNMP standard and the set of status and statistical information exported by the Service
Providers.
Part 29: XFS MIB Architecture and SNMP Extensions - Programmer’s Reference
Part 30: XFS MIB Device Specific Definitions - Printer Device Class
Part 31: XFS MIB Device Specific Definitions - Identification Card Device Class
Part 32: XFS MIB Device Specific Definitions - Cash Dispenser Device Class
Part 33: XFS MIB Device Specific Definitions - PIN Keypad Device Class
Part 34: XFS MIB Device Specific Definitions - Check Reader/Scanner Device Class
Part 35: XFS MIB Device Specific Definitions - Depository Device Class
Part 36: XFS MIB Device Specific Definitions - Text Terminal Unit Device Class
Part 37: XFS MIB Device Specific Definitions - Sensors and Indicators Unit Device Class
Part 38: XFS MIB Device Specific Definitions - Camera Device Class
Part 39: XFS MIB Device Specific Definitions - Alarm Device Class
Part 40: XFS MIB Device Specific Definitions - Card Embossing Unit Class
Part 41: XFS MIB Device Specific Definitions - Cash-In Module Device Class
Part 42: Reserved for future use.
7

CWA 16926-15:2025 (E)
Part 43: XFS MIB Device Specific Definitions - Vendor Dependent Mode Device Class
Part 44: XFS MIB Application Management
Part 45: XFS MIB Device Specific Definitions - Card Dispenser Device Class
Part 46: XFS MIB Device Specific Definitions - Barcode Reader Device Class
Part 47: XFS MIB Device Specific Definitions - Item Processing Module Device Class
Part 48: XFS MIB Device Specific Definitions - Biometrics Device Class
Parts 49 - 60 are reserved for future use.
Part 61: Application Programming Interface (API) - Migration from Version 3.40 (CWA 16296:2020)
to Version 3.51 (this CWA) - Service Provider Interface (SPI) - Programmer's Reference
Part 62: Printer and Scanning Device Class Interface - Migration from Version 3.40 (CWA
16296:2020) to Version 3.50 (this CWA) - Programmer's Reference
Part 63: Identification Card Device Class Interface - Migration from Version 3.40 (CWA 16296:2020)
to Version 3.50 (this CWA) - Programmer's Reference
Part 64: Cash Dispenser Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to
Version 3.50 (this CWA) - Programmer's Reference
Part 65: PIN Keypad Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to
Version 3.50 (this CWA) - Programmer's Reference
Part 66: Check Reader/Scanner Device Class Interface - Migration from Version 3.40 (CWA
16296:2020) to Version 3.50 (this CWA) - Programmer's Reference
Part 67: Depository Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to
Version 3.50 (this CWA) - Programmer's Reference
Part 68: Text Terminal Unit Device Class Interface - Migration from Version 3.40 (CWA 16296:2020)
to Version 3.50 (this CWA) - Programmer's Reference
Part 69: Sensors and Indicators Unit Device Class Interface - Migration from Version 3.40 (CWA
16296:2020) to Version 3.50 (this CWA) - Programmer's Reference
Part 70: Vendor Dependent Mode Device Class Interface - Migration from Version 3.40 (CWA
16296:2020) to Version 3.50 (this CWA) - Programmer's Reference
Part 71: Camera Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to Version
3.50 (this CWA) - Programmer's Reference
Part 72: Alarm Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to Version
3.50 (this CWA) - Programmer's Reference
Part 73: Card Embossing Unit Device Class Interface - Migration from Version 3.40 (CWA
16296:2020) to Version 3.50 (this CWA) - Programmer's Reference
Part 74: Cash-In Module Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to
Version 3.51 (this CWA) - Programmer's Reference
Part 75: Card Dispenser Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to
Version 3.50 (this CWA) - Programmer's Reference
Part 76: Barcode Reader Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to
Version 3.50 (this CWA) - Programmer's Reference
Part 77: Item Processing Module Device Class Interface - Migration from Version 3.40 (CWA
16296:2020) to Version 3.50 (this CWA) - Programmer's Reference
Part 78: Biometric Device Class Interface - Migration from Version 3.40 (CWA 16296:2020) to
Version 3.50 (this CWA) - Programmer's Reference
8In addition to these Programmer's Reference specifications, the reader of this CWA is also referred

CWA 16926-15:2025 (E)
to a complementary document, called Release Notes. The Release Notes contain clarifications and
explanations on the CWA specifications, which are not requiring functional changes. The current
version of the Release Notes is available online from: https://www.cencenelec.eu/areas-of-
work/cen-sectors/digital-society-cen/cwa-download-area/.
The information in this document represents the Workshop's current views on the issues discussed
as of the date of publication. It is provided for informational purposes only and is subject to change
without notice. CEN makes no warranty, express or implied, with respect to this document.
Revision History:
3.00 October 18, 2000 Initial Release.
3.10 November 29, 2007 For a description of changes from version 3.00 to
version 3.10 see the CIM 3.10 Migration document.
3.20 March 2, 2011 For a description of changes from version 3.10 to
version 3.20 see the CIM 3.20 Migration document.
3.30 March 19, 2015 For a description of changes from version 3.20 to
version 3.30 see the CIM 3.30 Migration document.
3.40 December 06, 2019 For a description of changes from version 3.30 to
version 3.40 see the CIM 3.40 Migration document.
3.50 November 18, 2022 For a description of changes from version 3.40 to
version 3.50 see the CIM 3.50 Migration document.
3.51 October 12, 2023 For a description of changes from version 3.40 to
version 3.51 see the CIM 3.51 Migration document.
Attention is drawn to the possibility that some elements of this document may be subject to patent
rights. CEN-CENELEC policy on patent rights is described in CEN-CENELEC Guide 8 “Guidelines for
Implementation of the Common IPR Policy on Patent”. CEN shall not be held responsible for
identifying any or all such patent rights.
Although the Workshop parties have made every effort to ensure the reliability and accuracy of
technical and non-technical descriptions, the Workshop is not able to guarantee, explicitly or
implicitly, the correctness of this document. Anyone who applies this CEN Workshop Agreement
shall be aware that neither the Workshop, nor CEN, can be held liable for damages or losses of any
kind whatsoever. The use of this CEN Workshop Agreement does not relieve users of their
responsibility for their own actions, and they apply this document at their own risk. The CEN
Workshop Agreement should not be construed as legal advice authoritatively endorsed by
CEN/CENELEC.
9

CWA 16926-15:2025 (E)
