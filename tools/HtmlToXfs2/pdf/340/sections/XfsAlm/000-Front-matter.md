---
source: XfsAlm.pdf
document: XfsAlm
device_class: ALM
part: 3
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 99]
---

<!-- Front matter -->

CEN

WORKSHOP

AGREEMENT

ICS 35.200; 35.240.15; 35.240.40

  CWA 16926-13

February 2020

English version

Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 13: Alarm Device Class
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
C O M I T É   E U R O P É E N   D E   N O R M A L I S A T I O N
E U R O P Ä I S C H E S   K O M I T E E   F Ü R   N O R M U N G

CEN-CENELEC Management Centre:  Rue de la Science 23,  B-1040 Brussels

© 2020 CEN  All rights of exploitation in any form and by any means reserved worldwide for CEN national Members.

Ref. No.:CWA 16926-13:2020 E


CWA 16926-13:2020 (E)

Table of Contents

European Foreword ...................................................................................................... 3

1.

Introduction .......................................................................................................... 7
1.1  Background to Release 3.40 ....................................................................................................... 7

1.2  XFS Service-Specific Programming ........................................................................................... 7

2.

3.

4.

Alarms .................................................................................................................. 9

References ......................................................................................................... 10

Info Commands ................................................................................................. 11
4.1  WFS_INF_ALM_STATUS ........................................................................................................... 11

4.2  WFS_INF_ALM_CAPABILITIES ................................................................................................ 13

5.

Execute Commands .......................................................................................... 14
5.1  WFS_CMD_ALM_SET_ALARM ................................................................................................. 14

5.2  WFS_CMD_ALM_RESET_ALARM ............................................................................................ 15

5.3  WFS_CMD_ALM_RESET ........................................................................................................... 16

5.4  WFS_CMD_ALM_SYNCHRONIZE_COMMAND ....................................................................... 17

6.

Events ................................................................................................................. 18
6.1  WFS_SRVE_ALM_DEVICE_SET ............................................................................................... 18

6.2  WFS_SRVE_ALM_DEVICE_RESET .......................................................................................... 19

7.

C - Header file .................................................................................................... 20

2


CWA 16926-13:2020 (E)
