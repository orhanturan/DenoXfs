---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "WFS_CMD_CIM_PREPARE_PRESENT)"
type: execute_command
source_lines: [7533, 7572]
---

<!-- WFS_CMD_CIM_PREPARE_PRESENT) -->

WFS_CMD_CIM_PREPARE_PRESENT
completes with WFS_SUCCESS
14. Open shutter. WFS_CMD_CIM_OPEN_SHUTTER
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
WFS_CMD_CIM_OPEN_SHUTTER
completes with WFS_SUCCESS
15. Tell the customer to take the items. The
customer should be informed that the
items will be returned in multiple bunches.
If there are additional bunches to deliver
then this can be determined from the
output parameter of the
WFS_SRVE_CIM_ITEMSPRESENTED
event.
16. Customer takes the WFS_SRVE_CIM_ITEMSTAKEN
bunch of items.
17. Close shutter. WFS_CMD_CIM_CLOSE_SHUTTER
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
WFS_CMD_CIM_CLOSE_SHUTTER
completes with WFS_SUCCESS
177

CWA 16926-15:2025 (E)

| 18.    | If more items need to be taken:  |     |
| ------ | -------------------------------- | --- |
|        |      Repeat steps 13. – 18.      |     |
Else
     Go to step 19.
| 19.    | End of transaction.  |     |
| ------ | -------------------- | --- |

178

CWA 16926-15:2025 (E)
