---
source: XfsVdm.pdf
document: XfsVdm
device_class: VDM
part: 12
section_id: ""
title: "Front matter"
type: front_matter
source_lines: [1, 128]
---

<!-- Front matter -->

CEN

WORKSHOP

AGREEMENT

ICS 35.200; 35.240.15; 35.240.40

  CWA 16926-11

December 2022

English version

Extensions for Financial Services (XFS) interface
specification Release 3.50 - Part 11: Vendor Dependent
Mode Device Class Interface - Programmer's Reference

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
C O M I T É   E U R O P É E N   D E   N O R M A L I S A T I O N
E U R O P Ä I S C H E S   K O M I T E E   F Ü R   N O R M U N G

CEN-CENELEC Management Centre:  Rue de la Science 23,  B-1040 Brussels

© 2022 CEN  All rights of exploitation in any form and by any means reserved worldwide for CEN national Members.

Ref. No.:CWA 16926-11:2022 E


CWA 16926-11:2022 (E)

Table of Contents

European Foreword ...................................................................................................... 3

1.

Introduction .......................................................................................................... 7

1.1  Background to Release 3.50 ....................................................................................................... 7

1.2  XFS Service-Specific Programming ........................................................................................... 7

2.

Vendor Dependent Mode .................................................................................... 9

2.1  VDM Entry triggered by XFS Application ................................................................................ 10

2.2  VDM Entry triggered by Vendor Dependent Switch ............................................................... 11

2.3  VDM Exit triggered by XFS Application ................................................................................... 12

2.4  VDM Exit triggered by Vendor Dependent Switch .................................................................. 13

2.5  Controlling / Determining the Active Interface ....................................................................... 14
Vendor Dependent Application independent of the VDM Service Provider ......................................14
Vendor Dependent Application under Control of the VDM Service Provider ....................................15

2.5.1
2.5.2

3.

4.

References ......................................................................................................... 16

Info Commands ................................................................................................. 17

4.1  WFS_INF_VDM_STATUS ........................................................................................................... 17

4.2  WFS_INF_VDM_CAPABILITIES ................................................................................................ 19

4.3  WFS_INF_VDM_ACTIVE_INTERFACE ..................................................................................... 20

5.

Execute Commands .......................................................................................... 21

5.1  WFS_CMD_VDM_ENTER_MODE_REQ .................................................................................... 21

5.2  WFS_CMD_VDM_ENTER_MODE_ACK .................................................................................... 22

5.3  WFS_CMD_VDM_EXIT_MODE_REQ ........................................................................................ 23

5.4  WFS_CMD_VDM_EXIT_MODE_ACK ........................................................................................ 24

5.5  WFS_CMD_VDM_SET_ACTIVE_INTERFACE .......................................................................... 25

6.

Events ................................................................................................................. 26

6.1  WFS_SRVE_VDM_ENTER_MODE_REQ .................................................................................. 26

6.2  WFS_SRVE_VDM_EXIT_MODE_REQ....................................................................................... 27

6.3  WFS_SYSE_VDM_MODEENTERED ......................................................................................... 28

6.4  WFS_SYSE_VDM_MODEEXITED .............................................................................................. 29

6.5  WFS_SRVE_VDM_INTERFACE_CHANGED ............................................................................ 30

7.

C-Header file ...................................................................................................... 31

2


CWA 16926-11:2022 (E)
