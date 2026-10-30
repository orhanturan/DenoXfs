---
source: XfsVdm.pdf
document: XfsVdm
device_class: VDM
part: 12
section_id: ""
title: "Vendor Dependent Application independent of the VDM Service Provider"
type: section
source_lines: [801, 880]
---

<!-- Vendor Dependent Application independent of the VDM Service Provider -->

Vendor Dependent Application

Supervisor/Maintenance application is in control.

WFS_CMD_VDM_SET_ACTIVE_INTERFACE

WFS_SRVE_VDM_INTERFACE_CHANGED

WFS_SRVE_VDM_INTERFACE_CHANGED

WFS_CMD_VDM_ENTER_MODE_REQ

WFS_SRVE_VDM_ENTER_MODE_REQ

WFS_SRVE_VDM_ENTER_MODE_REQ

WFS_CMD_VDM_ENTER_MODE_ACK

WFS_CMD_VDM_ENTER_MODE_ACK

WFS_SYSE_VDM_MODEENTERED

WFS_SYSE_VDM_MODEENTERED

VDA takes control, and at some point the interface is changed.

Vendor dependent means of indicating interface change

WFS_SRVE_VDM_INTERFACE_CHANGED

WFS_SRVE_VDM_INTERFACE_CHANGED

VDA completes.

WFS_CMD_VDM_EXIT_MODE_REQ

WFS_SRVE_VDM_EXIT_MODE_REQ

WFS_SRVE_VDM_EXIT_MODE_REQ

WFS_CMD_VDM_EXIT_MODE_ACK

WFS_CMD_VDM_EXIT_MODE_ACK

WFS_SYSE_VDM_MODEEXITED

WFS_SYSE_VDM_MODEEXITED

Supervisor/Maintenance application regains control.

WFS_INF_VDM_ACTIVE_INTERFACE

14


2.5.2  Vendor Dependent Application under Control of the VDM Service Provider

CWA 16926-11:2020 (E)

Multivendor Supervisor/
Maintenance Application

VDM Service Provider

Supervisor/Maintenance application is in control.

WFS_CMD_VDM_SET_ACTIVE_INTERFACE

WFS_SRVE_VDM_INTERFACE_CHANGED

WFS_CMD_VDM_ENTER_MODE_REQ

WFS_SRVE_VDM_ENTER_MODE_REQ

WFS_CMD_VDM_ENTER_MODE_ACK

WFS_SYSE_VDM_MODEENTERED

WFS_SRVE_VDM_INTERFACE_CHANGED
