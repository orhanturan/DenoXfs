---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "WFS_CMD_CIM_PREPARE_PRESENT)"
type: execute_command
source_lines: [7446, 7532]
---

<!-- WFS_CMD_CIM_PREPARE_PRESENT) -->

WFS_CMD_CIM_PREPARE_PRESEN
T completes with WFS_SUCCESS
10. Open shutter. WFS_CMD_CIM_OPEN_SHUTTER
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPENED)
WFS_SRVE_CIM_ITEMSPRESENTED
WFS_CMD_CIM_OPEN_SHUTTER
completes with WFS_SUCCESS
11. If there are additional bunches to deliver then
this can be determined from the output parameter
of the WFS_SRVE_CIM_ITEMSPRESENTED
event or the
WFS_INF_CIM_PRESENT_STATUS
command.
Tell the customer that the items were not
accepted, and to take the items. The customer
should be informed that the items will be
returned in multiple bunches.
12. Customer WFS_SRVE_CIM_ITEMSTAKEN
takes the
bunch of
items.
175

CWA 16926-15:2025 (E)

| 13.    | Close shutter.  | WFS_CMD_CIM_CLOSE_SHUTTER  |
| ------ | --------------- | -------------------------- |
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)

WFS_CMD_CIM_CLOSE_SHUTTER
completes with WFS_SUCCESS
| 14.    | If more refused items need to be taken:   |     |
| ------ | ----------------------------------------- | --- |
|        |      Repeat steps 9. – 14.                |     |
Else
     Go to step 15.
| 15.    | Display the amount recognized so far.  |     |
| ------ | -------------------------------------- | --- |
| 16.    | Ask the customer for further actions:  |     |

If the customer wants to deposit the amount:
Continue with step 17.

If the customer wants to get back all items
inserted so far see table "Cancellation by
Customer (Explicit Shutter Control)"
17.    Transport the money into the cash units of type  WFS_CMD_CIM_CASH_IN_END
WFS_CIM_TYPERECYCLING /
WFS_CIM_TYPECASHIN.
| 18.    | Credit the money to the customer's account.  |     |
| ------ | -------------------------------------------- | --- |
| 19.    | End of transaction.                          |     |

176

CWA 16926-15:2025 (E)
8.19 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK
(using WFS_CMD_CIM_PREPARE_PRESENT)
The following table describes the flow of a roll back operation where items are rolled back during the transaction.
The application use WFS_CMD_CIM_PREPARE_PRESENT commands to move items to the output position. The
Service Provider has explicit shutter control. In this case the WFS_CMD_CIM_OPEN_SHUTTER and
WFS_CMD_CIM_CLOSE_SHUTTER commands are used. Additionally, the number of items rolled back may be
greater than the number of items that can be presented at the output position.
This flow covers the following cases:
• bShutterControl == FALSE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE,
bPresentControl == FALSE, bPreparePresent == TRUE
Step Customer Application XFS Commands and Events
1.- See OK
10. Transaction
(Explicit Shutter
Control).
11. Selection: Return
all the items.
12. Transport the items recognized to the WFS_CMD_CIM_CASH_IN_ROLLBACK
output position.
*
WFS_CMD_CIM_CASH_IN_ROLLBACK
reset the lpTotalReturnedItems output
parameter of
WFS_INF_CIM_PRESENT_STATUS.
WFS_CMD_CIM_CASH_IN_ROLLBACK
completes with WFS_SUCCESS.
13. Move items to be rolled back to the output WFS_CMD_CIM_PREPARE_PRESENT
position.
