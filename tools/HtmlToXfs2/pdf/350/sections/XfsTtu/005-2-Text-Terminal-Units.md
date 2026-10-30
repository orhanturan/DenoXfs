---
source: XfsTtu.pdf
document: XfsTtu
device_class: TTU
part: 10
section_id: ""
title: "2. Text Terminal Units"
type: section
source_lines: [278, 295]
---

<!-- 2. Text Terminal Units -->

2. Text Terminal Units
This specification describes the functionality of the services provided by text terminal unit (TTU) services under
XFS, by defining the service-specific commands that can be issued, using the WFSGetInfo, WFSAsyncGetInfo,
WFSExecute and WFSAsyncExecute functions.
This section describes the functions provided by a generic Text Terminal Unit (TTU) service. A Text Terminal Unit
is a text i/o device, which applies both to ATM operator panels and to displays incorporated in devices such as PIN
pads and printers. This service allows for the following categories of functions:
• Forms oriented input and output
• Direct display output
• Keyboard input
• LED settings and control
All position indexes are zero based, where column zero, row zero is the top-leftmost position.
If the device has no shift key, the WFS_CMD_TTU_READ_FORM and WFS_CMD_TTU_READ commands will
return only upper case letters. If the device has a shift key, these commands return upper and lower case letters as
governed by the user's use of the shift key.
10

CWA 16926-9:2022 (E)
