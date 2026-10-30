---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "9. ATM Mixed Media Transaction Flow – Application Guidelines"
type: section
source_lines: [7573, 7650]
---

<!-- 9. ATM Mixed Media Transaction Flow – Application Guidelines -->

9. ATM Mixed Media Transaction Flow – Application Guidelines
Compound CIM/IPM deposit devices are able to accept and process different types of media such as cash and
checks. In order to improve the speed and usability of deposit devices it may be desirable to allow a bunch of items
deposited to contain a variety of media types. Typically this is a bunch containing both cash and checks and is
termed ‘Mixed Media processing’.
During this type of transaction the customer will insert cash and checks together in one bunch. The device will
identify each item. Items not positively identified may be immediately returned to the customer. All remaining
items can be deposited and shared deposit bins can be configured to receive mixed items. The application can also
choose to return all items. Additionally the specification allows for depositing all checks and returning all cash or
vice-versa depending on requirements.
In order to facilitate devices of differing hardware design and to support reuse of the XFS API, Mixed Media
processing is achieved by initiating a CIM and an IPM transaction in parallel. The application and Service
Providers must be able to handle concurrent CIM and IPM commands and events. The application will use the
WFS_CMD_CIM_SET_MODE or WFS_CMD_IPM_SET_MODE command to activate Mixed Media processing.
The literals used (i.e. WFS_CIM_IPMMIXEDMEDIA) describe the modes and indicate the nature of the
compound device. This allows applications to open the correct interfaces to drive the transaction.
Mixed Media processing commands that move media in the device require commands to be called on both CIM and
IPM interfaces. See the table below for a list of CIM commands and their IPM counterparts. Where the operation is
to be cancelled the application is required to cancel only one command on either the CIM or IPM interface.
Applications must be aware that the command that was NOT explicitly cancelled may complete with a
WFS_ERR_CANCELED error.
For example the application must call both WFS_CMD_CIM_CASH_IN and WFS_CMD_IPM_MEDIA_IN
commands to initiate the transaction. If an application wishes to cancel the transaction before items are inserted,
only the WFS_CMD_CIM_CASH_IN command can be cancelled and the WFS_CMD_IPM_MEDIA_IN
command will also be cancelled.
Devices suitable for Mixed Media processing must report WFSCIMCAPS.bShutterControl == TRUE to allow
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
179

CWA 16926-15:2025 (E)
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
180

CWA 16926-15:2025 (E)
