---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "WFS_CMD_CIM_PRESENT_MEDIA Command Supported)"
type: execute_command
source_lines: [7483, 7533]
---

<!-- WFS_CMD_CIM_PRESENT_MEDIA Command Supported) -->

WFS_CMD_CIM_PRESENT_MEDIA and WFS_CMD_IPM_PRESENT_MEDIA commands to work
concurrently.
The Mixed Media mode can be determined by calling WFS_INF_CIM_STATUS or WFS_INF_IPM_STATUS
command and checking the value of the wMixedMode field.
Where an error occurs both CIM and IPM interfaces will report it. To recover the device a reset command can be
called on either of the interfaces. Reset calls on both CIM and IPM interfaces are not required.
Application refusal (in the IPM interface) is not supported in Mixed Media mode.
To initiate a Mixed Media transaction the WFS_CMD_CIM_CASH_IN_START command must be called. There is
no equivalent command to the WFS_CMD_CIM_CASH_IN_START command on the IPM interface.
Commands and their counterparts:
This table lists the counterpart IPM commands which must be called as well as the CIM commands when in Mixed
Media processing mode.
CIM command IPM Command
WFS_CMD_CIM_CASH_IN WFS_CMD_IPM_MEDIA_IN
WFS_CMD_CIM_CASH_IN_END WFS_CMD_IPM_MEDIA_IN_END or where
bMixedDepositAndRollback is TRUE
WFS_CMD_IPM_MEDIA_IN_ROLLBACK
WFS_CMD_CIM_CASH_IN_ROLLBACK WFS_CMD_IPM_MEDIA_IN_ROLLBACK or where
bMixedDepositAndRollback is TRUE
WFS_CMD_IPM_MEDIA_IN_END
WFS_CMD_CIM_PRESENT_MEDIA WFS_CMD_IPM_PRESENT_MEDIA
WFS_CMD_CIM_RETRACT WFS_CMD_IPM_RETRACT_MEDIA
177

CWA 16926-15:2020 (E)
Events and their Counterparts
The CIM and IPM interfaces both have a range of events to inform the application of device activity. During Mixed
Media processing events fired from each interface can describe the same situation (i.e. items presented). In these
cases the recommendation to application developers is to rely on a single interface for these duplicate notifications.
The choice of which interface to use to handle specific events will be based on factors such as current codebase or
application presentation requirements.
CIM Event IPM Event
WFS_USRE_CIM_CASHUNITTHRESHOLD WFS_USRE_IPM_MEDIABINTHRESHOLD
WFS_SRVE_CIM_CASHUNITINFOCHANGED WFS_SRVE_IPM_MEDIABININFOCHANGED
WFS_EXEE_CIM_CASHUNITERROR WFS_EXEE_IPM_MEDIABINERROR
WFS_SRVE_CIM_ITEMSTAKEN WFS_SRVE_IPM_MEDIATAKEN
WFS_SRVE_CIM_COUNTS_CHANGED WFS_SRVE_IPM_MEDIABININFOCHANGED
WFS_EXEE_CIM_INPUTREFUSE WFS_EXEE_IPM_MEDIAREFUSED
WFS_SRVE_CIM_ITEMSPRESENTED WFS_EXEE_IPM_MEDIAPRESENTED
WFS_SRVE_CIM_ITEMSINSERTED WFS_EXEE_IPM_MEDIAINSERTED
WFS_EXEE_CIM_SUBCASHIN WFS_EXEE_IPM_MEDIADATA
WFS_SRVE_CIM_MEDIADETECTED WFS_SRVE_IPM_MEDIADETECTED
WFS_EXEE_CIM_INSERTITEMS WFS_EXEE_IPM_NOMEDIA
WFS_SRVE_CIM_DEVICEPOSITION WFS_SRVE_IPM_DEVICEPOSITION
WFS_SRVE_CIM_POWER_SAVE_CHANGE WFS_SRVE_IPM_POWER_SAVE_CHANGE
The following sections describe the flow of a Mixed Media transaction on a compound CIM/IPM device. These
application flows are provided as guidelines only. In all cases WFSCIMPOSCAPS.bPresentControl == TRUE
unless otherwise stated.
178

CWA 16926-15:2020 (E)
