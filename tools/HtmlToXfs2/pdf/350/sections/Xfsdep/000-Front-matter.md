---
source: Xfsdep.pdf
document: Xfsdep
device_class: DEP
part: 17
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 66]
---

<!-- Front matter -->

CEN
CWA 16926-8
WORKSHOP
December 2022
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.50 - Part 8: Depository Device Class
Interface - Programmer's Reference
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
Ref. No.:CWA 16926-8:2022 E

CWA 16926-8:2022 (E)
Table of Contents
European Foreword ...................................................................................................... 3
1. Introduction .......................................................................................................... 7
1.1 Background to Release 3.50 ....................................................................................................... 7
1.2 XFS Service-Specific Programming ........................................................................................... 7
2. Depository Unit .................................................................................................... 9
3. References ......................................................................................................... 10
4. Info Commands ................................................................................................. 11
4.1 WFS_INF_DEP_STATUS ........................................................................................................... 11
4.2 WFS_INF_DEP_CAPABILITIES ................................................................................................. 16
5. Execute Commands .......................................................................................... 19
5.1 WFS_CMD_DEP_ENTRY ........................................................................................................... 19
5.2 WFS_CMD_DEP_DISPENSE ..................................................................................................... 21
5.3 WFS_CMD_DEP_RETRACT ...................................................................................................... 22
5.4 WFS_CMD_DEP_RESET_COUNT ............................................................................................. 23
5.5 WFS_CMD_DEP_RESET ........................................................................................................... 24
5.6 WFS_CMD_DEP_SET_GUIDANCE_LIGHT .............................................................................. 25
5.7 WFS_CMD_DEP_SUPPLY_REPLENISH .................................................................................. 27
5.8 WFS_CMD_DEP_POWER_SAVE_CONTROL .......................................................................... 28
5.9 WFS_CMD_DEP_SYNCHRONIZE_COMMAND ........................................................................ 29
6. Events ................................................................................................................. 30
6.1 WFS_SRVE_DEP_ENVTAKEN .................................................................................................. 30
6.2 WFS_EXEE_DEP_ENVDEPOSITED .......................................................................................... 31
6.3 WFS_EXEE_DEP_DEPOSITERROR ......................................................................................... 32
6.4 WFS_USRE_DEP_DEPTHRESHOLD ........................................................................................ 33
6.5 WFS_USRE_DEP_TONERTHRESHOLD .................................................................................. 34
6.6 WFS_USRE_DEP_ENVTHRESHOLD ........................................................................................ 35
6.7 WFS_SRVE_DEP_CONTINSERTED ......................................................................................... 36
6.8 WFS_SRVE_DEP_CONTREMOVED ......................................................................................... 37
6.9 WFS_SRVE_DEP_ENVINSERTED ............................................................................................ 38
6.10 WFS_SRVE_DEP_MEDIADETECTED ....................................................................................... 39
6.11 WFS_EXEE_DEP_INSERTDEPOSIT ......................................................................................... 40
6.12 WFS_SRVE_DEP_DEVICEPOSITION ....................................................................................... 41
6.13 WFS_SRVE_DEP_POWER_SAVE_CHANGE ........................................................................... 42
7. C - Header file .................................................................................................... 43
2

CWA 16926-8:2022 (E)
