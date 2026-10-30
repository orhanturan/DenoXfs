---
source: XfsCeu.pdf
document: XfsCeu
device_class: CEU
part: 16
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 75]
---

<!-- Front matter -->

CEN
CWA 16926-14
WORKSHOP
February 2020
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 14: Card Embossing Unit
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
Ref. No.:CWA 16926-14:2020 E

CWA 16926-14:2020 (E)
Table of Contents
European Foreword ...................................................................................................... 4
1. Introduction .......................................................................................................... 8
1.1 Background to Release 3.40 ....................................................................................................... 8
1.2 XFS Service-Specific Programming ........................................................................................... 8
2. Card Embossing Units ...................................................................................... 10
3. References ......................................................................................................... 11
4. Info Commands ................................................................................................. 12
4.1 WFS_INF_CEU_STATUS ........................................................................................................... 12
4.2 WFS_INF_CEU_CAPABILITIES ................................................................................................. 16
4.3 WFS_INF_CEU_FORM_LIST ..................................................................................................... 18
4.4 WFS_INF_CEU_MEDIA_LIST .................................................................................................... 19
4.5 WFS_INF_CEU_QUERY_FORM ................................................................................................ 20
4.6 WFS_INF_CEU_QUERY_MEDIA ............................................................................................... 21
4.7 WFS_INF_CEU_QUERY_FIELD ................................................................................................ 23
5. Execute Commands .......................................................................................... 25
5.1 WFS_CMD_CEU_EMBOSS_CARD ........................................................................................... 25
5.2 WFS_CMD_CEU_RESET ........................................................................................................... 27
5.3 WFS_CMD_CEU_POWER_SAVE_CONTROL .......................................................................... 28
5.4 WFS_CMD_CEU_EMBOSS_CARD_EX .................................................................................... 29
5.5 WFS_CMD_CEU_SUPPLY_REPLENISH .................................................................................. 32
5.6 WFS_CMD_CEU_SYNCHRONIZE_COMMAND ........................................................................ 33
6. Events ................................................................................................................. 34
6.1 WFS_SRVE_CEU_INPUTBINTHRESHOLD .............................................................................. 34
6.2 WFS_SRVE_CEU_OUTPUTBINTHRESHOLD .......................................................................... 35
6.3 WFS_SRVE_CEU_RETAINBINTHRESHOLD ........................................................................... 36
6.4 WFS_EXEE_CEU_FIELDERROR .............................................................................................. 37
6.5 WFS_EXEE_CEU_FIELDWARNING .......................................................................................... 38
6.6 WFS_SRVE_CEU_MEDIAREMOVED ........................................................................................ 39
6.7 WFS_SRVE_CEU_MEDIADETECTED....................................................................................... 40
6.8 WFS_EXEE_CEU_EMBOSS_FAILURE..................................................................................... 41
6.9 WFS_SRVE_CEU_DEVICEPOSITION ....................................................................................... 42
6.10 WFS_SRVE_CEU_POWER_SAVE_CHANGE .......................................................................... 43
6.11 WFS_USRE_CEU_TONERTHRESHOLD .................................................................................. 44
7. Embossing Form, Field and Media Definitions ............................................... 45
7.1 Definition Syntax ........................................................................................................................ 45
7.2 Embossing Form and Media Measurements ........................................................................... 46
2

CWA 16926-14:2020 (E)
7.3 Embossing Form Definition ...................................................................................................... 47
7.4 Embossing Field Definition ....................................................................................................... 48
7.5 Media Definition ......................................................................................................................... 49
8. C-Header file ...................................................................................................... 50
3

CWA 16926-14:2020 (E)
