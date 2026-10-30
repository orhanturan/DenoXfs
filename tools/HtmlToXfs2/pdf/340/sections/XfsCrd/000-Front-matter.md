---
source: XfsCrd.pdf
document: XfsCrd
device_class: CRD
part: 7
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 60]
---

<!-- Front matter -->

CEN
CWA 16926-16
WORKSHOP
February 2020
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 16: Card Dispenser Device
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
Ref. No.:CWA 16926-16:2020 E

CWA 16926-16:2020 (E)
Table of Contents
European Foreword ...................................................................................................... 3
1. Introduction .......................................................................................................... 7
1.1 Background to Release 3.40 ....................................................................................................... 7
1.2 XFS Service-Specific Programming ........................................................................................... 7
2. Card Dispensers .................................................................................................. 9
3. References ......................................................................................................... 10
4. Info Commands ................................................................................................. 11
4.1 WFS_INF_CRD_STATUS ........................................................................................................... 11
4.2 WFS_INF_CRD_CAPABILITIES ................................................................................................ 14
4.3 WFS_INF_CRD_CARD_UNIT_INFO .......................................................................................... 16
5. Execute Commands .......................................................................................... 18
5.1 WFS_CMD_CRD_DISPENSE_CARD ........................................................................................ 18
5.2 WFS_CMD_CRD_EJECT_CARD ............................................................................................... 19
5.3 WFS_CMD_CRD_RETAIN_CARD ............................................................................................. 20
5.4 WFS_CMD_CRD_RESET ........................................................................................................... 21
5.5 WFS_CMD_CRD_SET_CARD_UNIT_INFO .............................................................................. 22
5.6 WFS_CMD_CRD_SET_GUIDANCE_LIGHT .............................................................................. 23
5.7 WFS_CMD_CRD_POWER_SAVE_CONTROL .......................................................................... 25
5.8 WFS_CMD_CRD_SYNCHRONIZE_COMMAND ....................................................................... 26
6. Events ................................................................................................................. 27
6.1 WFS_SRVE_CRD_MEDIAREMOVED ........................................................................................ 27
6.2 WFS_SRVE_CRD_MEDIADETECTED ...................................................................................... 28
6.3 WFS_USRE_CRD_CARDUNITTHRESHOLD ............................................................................ 29
6.4 WFS_SRVE_CRD_CARDUNITINFOCHANGED ........................................................................ 30
6.5 WFS_EXEE_CRD_CARDUNITERROR ...................................................................................... 31
6.6 WFS_SRVE_CRD_DEVICEPOSITION ....................................................................................... 32
6.7 WFS_SRVE_CRD_POWER_SAVE_CHANGE .......................................................................... 33
7. C-Header File ..................................................................................................... 34
2

CWA 16926-16:2020 (E)
