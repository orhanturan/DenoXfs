---
source: XfsCam.pdf
document: XfsCam
device_class: CAM
part: 13
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 50]
---

<!-- Front matter -->

CEN
CWA 16926-12
WORKSHOP
February 2020
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 12: Camera Device Class
Interface - Programmer's Reference
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
Ref. No.:CWA 16926-12:2020 E

CWA 16926-12:2020 (E)
Table of Contents
European Foreword ...................................................................................................... 3
1. Introduction .......................................................................................................... 7
1.1 Background to Release 3.40 ....................................................................................................... 7
1.2 XFS Service-Specific Programming ........................................................................................... 7
2. Banking Cameras ................................................................................................ 9
3. References ......................................................................................................... 10
4. Info Commands ................................................................................................. 11
4.1 WFS_INF_CAM_STATUS ........................................................................................................... 11
4.2 WFS_INF_CAM_CAPABILITIES ................................................................................................ 14
5. Execute Commands .......................................................................................... 16
5.1 WFS_CMD_CAM_TAKE_PICTURE ........................................................................................... 16
5.2 WFS_CMD_CAM_RESET ........................................................................................................... 17
5.3 WFS_CMD_CAM_TAKE_PICTURE_EX .................................................................................... 18
5.4 WFS_CMD_CAM_SYNCHRONIZE_COMMAND ....................................................................... 20
6. Events ................................................................................................................. 21
6.1 WFS_USRE_CAM_MEDIATHRESHOLD ................................................................................... 21
6.2 WFS_EXEE_CAM_INVALIDDATA ............................................................................................. 22
7. C - Header file .................................................................................................... 23
2

CWA 16926-12:2020 (E)
