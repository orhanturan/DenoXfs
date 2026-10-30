---
source: XfsSiu.pdf
document: XfsSiu
device_class: SIU
part: 11
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 61]
---

<!-- Front matter -->

CEN
CWA 16926-10
WORKSHOP
AGREEMENT
February 2020
ICS
35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 10: Sensors and Indicators
Unit Device Class Interface - Programmer's Reference
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
CEN-CENELECManagement Centre: Rue de la Science 23, B-1040 Brussels
© 2020 CEN All rights of exploitation in any form and by any means reserved worldwide for CEN national Members.
Ref. No.:CWA 16926-10:2020 E

Table of Contents
European Foreword ...................................................................................................... 3
1. Introduction .......................................................................................................... 7
1.1 Background to Release 3.40 ....................................................................................................... 7
1.2 XFS Service-Specific Programming ........................................................................................... 7
2. Sensors and Indicators Unit ............................................................................... 9
2.1 Enhanced Audio Controller Overview...................................................................................... 10
2.2 Enhanced Microphone Controller Overview ........................................................................... 14
3. References ......................................................................................................... 15
4. Info Commands ................................................................................................. 16
4.1 WFS_INF_SIU_STATUS ............................................................................................................. 16
4.2 WFS_INF_SIU_CAPABILITIES ................................................................................................... 27
4.3 WFS_INF_SIU_GET_AUTOSTARTUP_TIME ............................................................................ 38
5. Execute Commands .......................................................................................... 40
5.1 WFS_CMD_SIU_ENABLE_EVENTS .......................................................................................... 40
5.2 WFS_CMD_SIU_SET_PORTS ................................................................................................... 49
5.3 WFS_CMD_SIU_SET_DOOR ..................................................................................................... 56
5.4 WFS_CMD_SIU_SET_INDICATOR ............................................................................................ 57
5.5 WFS_CMD_SIU_SET_AUXILIARY ............................................................................................. 59
5.6 WFS_CMD_SIU_SET_GUIDLIGHT ............................................................................................ 62
5.7 WFS_CMD_SIU_RESET ............................................................................................................. 63
5.8 WFS_CMD_SIU_POWER_SAVE_CONTROL ............................................................................ 64
5.9 WFS_CMD_SIU_SET_AUTOSTARTUP_TIME .......................................................................... 65
5.10 WFS_CMD_SIU_SYNCHRONIZE_COMMAND ......................................................................... 67
5.11 WFS_CMD_SIU_SET_GUIDLIGHT_EX ..................................................................................... 68
6. Events ................................................................................................................. 69
6.1 WFS_SRVE_SIU_PORT_STATUS ............................................................................................. 69
6.2 WFS_EXEE_SIU_PORT_ERROR ............................................................................................... 72
6.3 WFS_SRVE_SIU_POWER_SAVE_CHANGE ............................................................................. 75
7. C - Header file .................................................................................................... 76
2

CWA 16926-10:2020 (E)
