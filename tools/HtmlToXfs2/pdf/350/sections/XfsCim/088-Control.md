---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "Control)"
type: section
source_lines: [6525, 6601]
---

<!-- Control) -->

Control).
7. * WFS_CMD_CIM_CASH_IN initiated.
The bill recognition begins.
8. WFS_EXEE_CIM_INPUTREFUSE
(WFS_CIM_INVALIDBILL)
…
* WFS_CMD_CIM_CASH_IN
completes with WFS_SUCCESS
9. Open shutter. WFS_CMD_CIM_OPEN_SHUTTER
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPENED)
WFS_SRVE_CIM_ITEMSPRESENTED
WFS_CMD_CIM_OPEN_SHUTTER
completes with WFS_SUCCESS
10. If there are additional bunches to deliver then
this can be determined from the output parameter
of the WFS_SRVE_CIM_ITEMSPRESENTED
event.
Tell the customer that the items were not
accepted, and to take the items. The customer
should be informed that the items will be
returned in multiple bunches.
11. Customer WFS_SRVE_CIM_ITEMSTAKEN
takes the
bunch of
items.
12. Close shutter. WFS_CMD_CIM_CLOSE_SHUTTER
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
WFS_CMD_CIM_CLOSE_SHUTTER
completes with WFS_SUCCESS
156

CWA 16926-15:2025 (E)
13. Check if more refused bills need to be taken. The
wAdditionalBunches and usBunchesRemaining
fields from the last
WFS_SRVE_CIM_ITEMSPRESENTED event
are used to determine this. Note that if more
items are to be presented, the
WFS_CMD_CIM_OPEN_SHUTTER in step 9
will move the next bunch to the output position.
If wAdditionalBunches ==
WFS_CIM_ADDBUNCHONEMORE
Repeat steps 9. – 13.
Else
Go to step 14.
14. Display the amount recognized so far.
15. Ask the customer for further actions:
If the customer wants to deposit the amount:
Continue with step 16.
If the customer wants to get back all items
inserted so far see table "Multiple Bunches
Returned During
WFS_CMD_CIM_CASH_IN_ROLLBACK"
16. Transport the money into the cash units of type WFS_CMD_CIM_CASH_IN_END
WFS_CIM_TYPERECYCLING /
WFS_CIM_TYPECASHIN.
17. Credit the money to the customer's account.
18. End of transaction.
157

CWA 16926-15:2025 (E)
8.7 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK
(Explicit Shutter Control)
The following table describes the flow of a roll back operation where items are rolled back during the transaction
and the Service Provider has explicit shutter control. In this case the WFS_CMD_CIM_OPEN_SHUTTER and
WFS_CMD_CIM_CLOSE_SHUTTER commands are used. Additionally, the number of items rolled back may be
greater than the number of items that can be presented at the output position.
This flow covers the following cases:
• bShutterControl == FALSE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE,
bPresentControl == FALSE
Step Customer Application XFS Commands and Events
1.- See OK
10. Transaction
(Explicit Shutter
