---
source: XfsCeu.pdf
document: XfsCeu
device_class: CEU
part: 16
section_id: ""
title: "2. Card Embossing Units"
type: section
source_lines: [285, 304]
---

<!-- 2. Card Embossing Units -->

2. Card Embossing Units
This section describes the functions provided by a generic card embossing unit (CEU). These descriptions include
definitions of the service-specific commands that can be issued, using the WFSAsyncExecute, WFSExecute,
WFSGetInfo and WFSAsyncGetInfo functions.
Embossing card units are generally viewed by XFS as compound devices with the following capabilities and
features:
• Embossing or printing of magnetic stripe card/ smart card.
• Reading/encoding magnetic stripe tracks 1, 2, and 3.
• Reading/writing smart card.
• LCD display/ keypad input.
The XFS services supporting the various embossing card unit components are outlined as follows:
• Embossing or printing of magnetic stripe card/ smart card - Card Embossing Unit (CEU) service.
• Reading/encoding magnetic stripe tracks 1, 2, and 3 - ID Card (IDC) service, however when combined
encoding/ embossing is performed the CEU service class is used.
• Reading/writing smart cards - ID Card (IDC) service, however when combined writing smart card/
embossing is performed the CEU service class is used.
• LCD display/ keypad input - Text Terminal Unit (TTU) service.
10

CWA 16926-14:2020 (E)
