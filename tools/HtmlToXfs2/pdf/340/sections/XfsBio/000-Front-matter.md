---
source: XfsBio.pdf
document: XfsBio
device_class: BIO
part: 18
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 76]
---

<!-- Front matter -->

CEN
CWA 16926-19
WORKSHOP
April 2020
AGREEMENT
ICS 35.240.15; 35.200; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 19: Biometrics Device
Class Interface Proposal - Programmer's Reference
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
Ref. No.:CWA 16926-19:2020 E

CWA 16926-19:2020 (E)
Table of Contents
European Foreword ...................................................................................................... 4
1. Introduction .......................................................................................................... 7
1.1 Background to Release 3.40 ....................................................................................................... 7
1.2 XFS Service-Specific Programming ........................................................................................... 7
2. Biometric Devices................................................................................................ 9
2.1 Enrollment .................................................................................................................................... 9
2.2 Biometric Matching ...................................................................................................................... 9
2.3 Biometric Device Types ............................................................................................................ 10
2.4 Biometric Data Security ............................................................................................................ 10
3. References ......................................................................................................... 11
4. Info Commands ................................................................................................. 12
4.1 WFS_INF_BIO_STATUS ............................................................................................................ 12
4.2 WFS_INF_BIO_CAPABILITIES .................................................................................................. 15
4.3 WFS_INF_BIO_STORAGE_INFO .............................................................................................. 20
4.4 WFS_INF_BIO_KEY_INFO ......................................................................................................... 21
5. Execute Commands .......................................................................................... 22
5.1 WFS_CMD_BIO_READ .............................................................................................................. 22
5.2 WFS_CMD_BIO_IMPORT .......................................................................................................... 25
5.3 WFS_CMD_BIO_MATCH ........................................................................................................... 26
5.4 WFS_CMD_BIO_SET_MATCH .................................................................................................. 29
5.5 WFS_CMD_BIO_CLEAR ............................................................................................................ 30
5.6 WFS_CMD_BIO_RESET ............................................................................................................ 31
5.7 WFS_CMD_BIO_SET_DATA_PERSISTENCE .......................................................................... 32
5.8 WFS_CMD_BIO_SET_GUIDANCE_LIGHT ............................................................................... 33
5.9 WFS_CMD_BIO_POWER_SAVE_CONTROL ........................................................................... 35
5.10 WFS_CMD_BIO_SYNCHRONIZE_COMMAND ......................................................................... 36
6. Events ................................................................................................................. 37
6.1 WFS_EXEE_BIO_PRESENTSUBJECT ..................................................................................... 37
6.2 WFS_EXEE_BIO_SUBJECTDETECTED ................................................................................... 38
6.3 WFS_EXEE_BIO_REMOVESUBJECT....................................................................................... 39
6.4 WFS_SRVE_BIO_SUBJECTREMOVED .................................................................................... 40
6.5 WFS_SRVE_BIO_DATACLEARED ........................................................................................... 41
6.6 WFS_EXEE_BIO_ORIENTATION .............................................................................................. 42
6.7 WFS_SRVE_BIO_DEVICEPOSITION ........................................................................................ 43
6.8 WFS_SRVE_BIO_POWER_SAVE_CHANGE ............................................................................ 44
2

CWA 16926-19:2020 (E)
7. Biometric Device Command Flows – Application Guidelines ....................... 45
7.1 Biometric Enrollment Command Flow ..................................................................................... 45
7.2 Biometric Match Command Flow – Separate Scan and Match ............................................. 46
7.3 Biometric Match Command Flow – Combined Scan and Match ........................................... 47
7.4 Biometric Scan-Only Command Flow ...................................................................................... 48
8. C - Header file .................................................................................................... 49
3

CWA 16926-19:2020 (E)
