---
source: XfsChk.pdf
document: XfsChk
device_class: CHK
part: 14
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 73]
---

<!-- Front matter -->

CEN
CWA 16926-7
WORKSHOP
December 2022
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.50 - Part 7: Check Reader/Scanner
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
Ref. No.:CWA 16926-7:2022 E

CWA 16926-7:2022 (E)
Table of Contents
European Foreword ...................................................................................................... 4
1. Introduction .......................................................................................................... 8
1.1 Background to Release 3.50 ....................................................................................................... 8
1.2 XFS Service-Specific Programming ........................................................................................... 8
2. Check Readers and Scanners .......................................................................... 10
3. References ......................................................................................................... 11
4. Info Commands ................................................................................................. 12
4.1 WFS_INF_CHK_STATUS ........................................................................................................... 12
4.2 WFS_INF_CHK_CAPABILITIES ................................................................................................ 15
4.3 WFS_INF_CHK_FORM_LIST ..................................................................................................... 18
4.4 WFS_INF_CHK_MEDIA_LIST .................................................................................................... 19
4.5 WFS_INF_CHK_QUERY_FORM ................................................................................................ 20
4.6 WFS_INF_CHK_QUERY_MEDIA ............................................................................................... 22
4.7 WFS_INF_CHK_QUERY_FIELD ................................................................................................ 24
5. Execute Commands .......................................................................................... 26
5.1 WFS_CMD_CHK_PROCESS_FORM ......................................................................................... 26
5.2 WFS_CMD_CHK_RESET ........................................................................................................... 29
5.3 WFS_CMD_CHK_SET_GUIDANCE_LIGHT .............................................................................. 30
5.4 WFS_CMD_CHK_POWER_SAVE_CONTROL .......................................................................... 32
5.5 WFS_CMD_CHK_SYNCHRONIZE_COMMAND ....................................................................... 33
6. Events ................................................................................................................. 34
6.1 WFS_EXEE_CHK_NOMEDIA ..................................................................................................... 34
6.2 WFS_EXEE_CHK_MEDIAINSERTED ........................................................................................ 35
6.3 WFS_SRVE_CHK_MEDIAINSERTED ........................................................................................ 36
6.4 WFS_EXEE_CHK_FIELDERROR .............................................................................................. 37
6.5 WFS_EXEE_CHK_FIELDWARNING .......................................................................................... 38
6.6 WFS_USRE_CHK_INKTHRESHOLD ......................................................................................... 39
6.7 WFS_SRVE_CHK_MEDIADETECTED ...................................................................................... 40
6.8 WFS_SRVE_CHK_DEVICEPOSITION ....................................................................................... 41
6.9 WFS_SRVE_CHK_POWER_SAVE_CHANGE .......................................................................... 42
7. Forms Language Usage .................................................................................... 43
7.1 Definition Syntax ........................................................................................................................ 44
7.2 XFS form/media definition files in multi-vendor environments ............................................ 45
7.3 Form and Media Measurements ............................................................................................... 46
7.4 Form Definition ........................................................................................................................... 47
2

CWA 16926-7:2022 (E)
7.5 Field Definition ........................................................................................................................... 48
7.6 Media Definition ......................................................................................................................... 51
8. C - Header file .................................................................................................... 52
3

CWA 16926-7:2022 (E)
