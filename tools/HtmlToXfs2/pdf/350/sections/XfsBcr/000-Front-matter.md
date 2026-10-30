---
source: XfsBcr.pdf
document: XfsBcr
device_class: BCR
part: 4
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 51]
---

<!-- Front matter -->

CEN
CWA 16926-17
WORKSHOP
December 2022
AGREEMENT
ICS 35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.50 - Part 17: Barcode Reader
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
Ref. No.:CWA 16926-17:2022 E

CWA 16926-17:2022 (E)
Table of Contents
European Foreword ...................................................................................................... 3
1 Introduction ............................................................................................................. 7
1.1 Background to Release 3.50 ....................................................................................................... 7
1.2 XFS Service-Specific Programming ........................................................................................... 7
2. Barcode Readers ................................................................................................. 9
3. References ......................................................................................................... 10
4. Info Commands ................................................................................................. 11
1.1 WFS_INF_BCR_STATUS ........................................................................................................... 11
1.2 WFS_INF_BCR_CAPABILITIES ................................................................................................. 14
5. Execute Commands .......................................................................................... 17
1.3 WFS_CMD_BCR_READ ............................................................................................................. 17
1.4 WFS_CMD_BCR_RESET ........................................................................................................... 19
1.5 WFS_CMD_BCR_SET_GUIDANCE_LIGHT .............................................................................. 20
1.6 WFS_CMD_BCR_POWER_SAVE_CONTROL .......................................................................... 22
1.7 WFS_CMD_BCR_SYNCHRONIZE_COMMAND ........................................................................ 23
6. Events ................................................................................................................. 24
6.1 WFS_SRVE_BCR_DEVICEPOSITION ....................................................................................... 24
1.8 WFS_SRVE_BCR_POWER_SAVE_CHANGE ........................................................................... 25
7. C - Header file .................................................................................................... 26
2

CWA 16926-17:2022 (E)
