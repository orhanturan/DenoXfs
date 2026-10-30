---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "8. ATM Cash-In Transaction Flow - Application Guidelines"
type: section
source_lines: [6066, 6084]
---

<!-- 8. ATM Cash-In Transaction Flow - Application Guidelines -->

8. ATM Cash-In Transaction Flow - Application Guidelines
The following table is a summary of the application flows required given the possible values for bShutterControl
and bItemsTakenSensor for a successful cash-in transaction. In all cases bPresentControl == TRUE.
bItemsInsertedSensor == TRUE bItemsInsertedSensor == FALSE
bShutterControl == TRUE WFS_CMD_CIM_CASH_IN_START WFS_CMD_CIM_CASH_IN_START
WFS_CMD_CIM_CASH_IN WFS_CMD_CIM_CASH_IN
InsertedEvent generated
WFS_CMD_CIM_CASH_IN_END WFS_CMD_CIM_CASH_IN_END
bShutterControl == FALSE WFS_CMD_CIM_CASH_IN_START WFS_CMD_CIM_CASH_IN_START
WFS_CMD_CIM_OPEN_SHUTTER WFS_CMD_CIM_OPEN_SHUTTER
InsertedEvent generated User Input
WFS_CMD_CIM_CLOSE_SHUTTER WFS_CMD_CIM_CLOSE_SHUTTER
WFS_CMD_CIM_CASH_IN WFS_CMD_CIM_CASH_IN
WFS_CMD_CIM_CASH_IN_END WFS_CMD_CIM_CASH_IN_END
The following sections describe the flow of a cash-in transaction on a Self-Service CIM. These application flows
are provided as guidelines only.
147

CWA 16926-15:2020 (E)
