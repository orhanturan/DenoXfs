---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "Control)"
type: section
source_lines: [6602, 6653]
---

<!-- Control) -->

Control).
11. Selection: Return
all the items.
12. Transport the items recognized to the WFS_CMD_CIM_CASH_IN_ROLLBACK
output position.
WFS_CMD_CIM_CASH_IN_ROLLBACK
completes with WFS_SUCCESS.
13. Open shutter. WFS_CMD_CIM_OPEN_SHUTTER
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
WFS_CMD_CIM_OPEN_SHUTTER
completes with WFS_SUCCESS
14. Tell the customer to take the items. The
customer should be informed that the
items will be returned in multiple bunches.
If there are additional bunches to deliver
then this can be determined from the
output parameter of the
WFS_SRVE_CIM_ITEMSPRESENTED
event.
15. Customer takes the WFS_SRVE_CIM_ITEMSTAKEN
bunch of items.
16. Close shutter. WFS_CMD_CIM_CLOSE_SHUTTER
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
WFS_CMD_CIM_CLOSE_SHUTTER
completes with WFS_SUCCESS
158

CWA 16926-15:2025 (E)
17. Check if more bills need to be taken. The
wAdditionalBunches and
usBunchesRemaining fields from the last
WFS_SRVE_CIM_ITEMSPRESENTED
event is used to determine this. Note that if
more items are to be presented, the
WFS_CMD_CIM_OPEN_SHUTTER in
step 13 will move the next bunch to the
output position.
If wAdditionalBunches ==
WFS_CIM_ADDBUNCHONEMORE
Repeat steps 13. – 17.
Else
Go to step 18.
18. End of transaction.
159

CWA 16926-15:2025 (E)
