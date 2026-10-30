---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "(Implicit Shutter Control and Implicit Present Control)"
type: section
source_lines: [6712, 7073]
---

<!-- (Implicit Shutter Control and Implicit Present Control) -->

(Implicit Shutter Control)"
| 9.  Selection: Finish the  |     |     |
| -------------------------- | --- | --- |
transaction
10.    Transport the money into the cash  WFS_CMD_CIM_CASH_IN_END
units of type
WFS_CIM_TYPERECYCLING /
WFS_CIM_TYPECASHIN.
| 11.    | Credit the money to the customer's  |     |
| ------ | ----------------------------------- | --- |
account.
| 12.    | End of transaction.  |     |
| ------ | -------------------- | --- |
160

CWA 16926-15:2025 (E)

8.9  Customer Initiates Returning Of Previously Recognized Items (Implicit
Shutter Control)
The following table describes the flow of a cash-in transaction where the customer wants all the items to be
returned after recognition and the shutter is implicitly controlled by the Service Provider. In this case the
WFS_CMD_CIM_OPEN_SHUTTER and WFS_CMD_CIM_CLOSE_SHUTTER commands are not used.
This flow covers the following cases:
•  bShutterControl == TRUE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE
•  bShutterControl == TRUE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == FALSE

| Step  Customer             | Application  | XFS Commands and Events  |
| -------------------------- | ------------ | ------------------------ |
| 1.-8.  See OK Transaction  |              |                          |
(Implicit Shutter
Control).
| 9.  Selection: Return all the  |     |     |
| ------------------------------ | --- | --- |
items.
10.    Transport the items recognized to  WFS_CMD_CIM_CASH_IN_ROLLBACK
|     | the output position.  | The Service Provider implicitly opens the  |
| --- | --------------------- | ------------------------------------------ |
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
| 11.                      | Request removal of the money.  |     |
| ------------------------ | ------------------------------ | --- |
| 12.  Customer takes the  |                                |     |
money from the output
position.
| 13.  If bItemsTakenSensor  |     | If bItemsTakenSensor == TRUE:  |
| -------------------------- | --- | ------------------------------ |
| == FALSE: confirm          |     | WFS_SRVE_CIM_ITEMSTAKEN        |
completion or use  The Service Provider implicitly closes the
| application timeout.  |     | shutter.  |
| --------------------- | --- | --------- |
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
| 14.    | End of transaction  |     |
| ------ | ------------------- | --- |
161

CWA 16926-15:2025 (E)
8.10 OK Transaction - (Implicit Shutter Control and
WFS_EXEE_CIM_SUBCASHIN event supported)
The following table describes the chronological steps taken in the flow of a cash-in transaction where the cash-in
operation is subdivided into a number of logical operations under hardware control. In this case a
WFS_EXEE_CIM_SUBCASHIN event is generated for each sub cash-in operation. This may be the case for
instance where a device does its coin or bill recognition in batches of 25. In this case the Service Provider would
post a WFS_EXEE_CIM_SUBCASHIN event each time 25 items were processed. In this example the shutter is
implicitly controlled by the Service Provider so the WFS_CMD_CIM_OPEN_SHUTTER and
WFS_CMD_CIM_CLOSE_SHUTTER commands are not used.
This flow covers the following cases:
• bShutterControl == TRUE, bItemsInsertedSensor == TRUE
• bShutterControl == TRUE, bItemsInsertedSensor == FALSE
Step Customer Application XFS Commands and Events
1.-5. See OK Transaction
(Implicit Shutter
Control).
6. The device processes the items in batches.
Each time a batch is completed a
WFS_EXEE_CIM_SUBCASHIN event is
posted then the cash-in operation continues.
7. * WFS_CMD_CIM_CASH_IN completes.
8. Display the number of items
and/or amount recognized so far.
9. Ask the customer for further
actions:
If the customer wants to insert
more money:
Repeat from step 2.
If the customer wants to finish the
transaction:
Continue with step 10.
If the customer wants to get back
all items inserted so far see table
"Cancellation by Customer
(Implicit Shutter Control)"
10. WFS_CMD_CIM_CASH_IN_END
11. End of transaction.
162

CWA 16926-15:2025 (E)
8.11 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN (Implicit
Shutter Control and Implicit Present Control)
The following table describes the flow of a cash-in transaction where items are rejected during the transaction and
the Service Provider has implicit shutter and present control. In this case the WFS_CMD_CIM_OPEN_SHUTTER,
WFS_CMD_CIM_CLOSE_SHUTTER and WFS_CMD_CIM_PRESENT_MEDIA commands are not used.
Additionally, the number of items refused may be greater than the number of items that can be presented at the
output position. Due to the complexity of this scenario, shutter and present control must be implicit. Therefore,
there is no corresponding flow for explicit shutter and present control.
This flow covers the following cases:
• bShutterControl == TRUE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE,
bPresentControl == TRUE
Step Customer Application XFS Command
1.-5. See OK Transaction
(Implicit Shutter
Control).
6. As a result of the bill processing n bunches
of items must be returned to the customer.
7. WFS_EXEE_CIM_INPUTREFUSE
8. Return bunch 1 of items to customer.
The Service Provider implicitly opens the
shutter and implicitly presents the bunch of
items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
9. Tell the customer that the items
were not accepted, and to take the
items. The customer should be
informed that the items will be
returned in multiple bunches. If
there are additional bunches to
deliver then this can be determined
from the output parameter of the
WFS_SRVE_CIM_ITEMSPRESE
NTED event.
10. Customer takes the WFS_SRVE_CIM_ITEMSTAKEN
bunch of items. The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
11. Return bunch 2 of items to customer. The
Service Provider implicitly opens the shutter
and implicitly presents the bunch of items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
163

CWA 16926-15:2025 (E)

| 12.    | Tell the customer that the items  |     |
| ------ | --------------------------------- | --- |
were not accepted, and to take the
items. The customer should be
informed that the items will be
returned in multiple bunches. If
there are additional bunches to
deliver then this can be determined
from the output parameter of the
WFS_SRVE_CIM_ITEMSPRESE
NTED event.
| 13.  Customer takes the  |     | WFS_SRVE_CIM_ITEMSTAKEN                     |
| ------------------------ | --- | ------------------------------------------- |
| bunch of items.          |     | The Service Provider implicitly closes the  |
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
| 14.    |     | Repeat steps 11.-13. until bunches 3 to n-1  |
| ------ | --- | -------------------------------------------- |
are returned to the customer.
| 15.    |     | Return bunch n (last) of items to customer.  |
| ------ | --- | -------------------------------------------- |
The Service Provider implicitly opens the
shutter and implicitly presents the bunch of
items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
| 16.    |     | * WFS_CMD_CIM_CASH_IN completes  |
| ------ | --- | -------------------------------- |
with WFS_SUCCESS.
| 17.    | Tell the customer to take the items.  |     |
| ------ | ------------------------------------- | --- |
The customer should be informed
that this is the final bunch.
| 18.  Customer takes the  |     |     |
| ------------------------ | --- | --- |
bunch of items.
| 19.    |     | WFS_SRVE_CIM_ITEMSTAKEN  |
| ------ | --- | ------------------------ |
The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
| 20.    | Display the amount recognized so  |     |
| ------ | --------------------------------- | --- |
far.
| 21.    | Ask the customer for further  |     |
| ------ | ----------------------------- | --- |
actions:

If the customer wants to deposit the
amount:
Continue with step 21.

If the customer wants to get back all
items inserted so far see table
"Cancellation by Customer
(Implicit Shutter Control)"
22.    Transport the money into the cash  WFS_CMD_CIM_CASH_IN_END
units of type
WFS_CIM_TYPERECYCLING /
WFS_CIM_TYPECASHIN.
| 23.    | Credit the money to the customer's  |     |
| ------ | ----------------------------------- | --- |
account.
| 24.    | End of transaction.  |     |
| ------ | -------------------- | --- |
164

CWA 16926-15:2025 (E)
8.12 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN_ROLLBACK
(Implicit Shutter Control and Implicit Present Control)
The following table describes the flow of a roll back operation where items are rolled back during the transaction
and the Service Provider has implicit shutter and present control. In this case the
WFS_CMD_CIM_OPEN_SHUTTER, WFS_CMD_CIM_CLOSE_SHUTTER and
WFS_CMD_CIM_PRESENT_MEDIA commands are not used. Additionally, the number of items rolled back may
be greater than the number of items that can be presented at the output position. Due to the complexity of this
scenario, shutter and present control must be implicit. Therefore, there is no corresponding flow for explicit shutter
and present control.
This flow covers the following cases:
• bShutterControl == TRUE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE,
bPresentControl == TRUE
Step Customer Application XFS Command
1.-9. See Customer Initiates
Returning Of Previously
Recognized Items
(Implicit Shutter
Control).
10. Initiate the roll back operation. * WFS_CMD_CIM_CASH_IN_ROLLBACK
11. The Service Provider begins the roll back.
As a result of this n bunches of items must be
returned to the customer.
12. Return bunch 1 of items to customer.
The Service Provider implicitly opens the
shutter and implicitly presents the bunch of
items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
13. Tell the customer to take the
items. The customer should be
informed that the items will be
returned in multiple bunches. If
there are additional bunches to
deliver then this can be
determined from the output
parameter of the
WFS_SRVE_CIM_ITEMSPRES
ENTED event.
14. Customer takes the WFS_SRVE_CIM_ITEMSTAKEN
bunch of items. The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
15. Repeat steps 11.-14. until bunches 2 to n-1
are returned to the customer.
16. Return bunch n (last) of items to customer.
The Service Provider implicitly opens the
shutter and implicitly presents the bunch of
items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
165

CWA 16926-15:2025 (E)

| 17.    |     | * WFS_CMD_CIM_CASH_IN_ROLLBACK  |
| ------ | --- | ------------------------------- |
completes with WFS_SUCCESS.
| 18.    | Tell the customer to take the  |     |
| ------ | ------------------------------ | --- |
items. The customer should be
informed that this is the final
bunch.
| 19.  Customer takes the  |     |     |
| ------------------------ | --- | --- |
bunch of items.
| 20.    |     | WFS_SRVE_CIM_ITEMSTAKEN  |
| ------ | --- | ------------------------ |
The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
| 21.    | End of transaction.  |     |
| ------ | -------------------- | --- |
166

CWA 16926-15:2025 (E)
8.13 Retracting Items When Multiple Bunches Are Returned During
WFS_CMD_CIM_CASH_IN (Implicit Shutter Control and Implicit Present
Control)
The following table describes the flow of a cash-in transaction where items are returned back during the transaction
and the Service Provider has implicit shutter and present control. In this case the
WFS_CMD_CIM_OPEN_SHUTTER, WFS_CMD_CIM_CLOSE_SHUTTER and
WFS_CMD_CIM_PRESENT_MEDIA commands are not used. Additionally, the number of items returned may be
greater than the number of items that can be presented at the output position. Due to the complexity of this scenario,
shutter and present control must be implicit. Therefore, there is no corresponding flow for explicit shutter and
present control.
This flow covers the following cases:
• bShutterControl == TRUE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE,
bPresentControl == TRUE
Step Customer Application XFS Command
1.-5. See OK Transaction
(Implicit Shutter
Control).
6. As a result of the bill processing n bunches
of items must be returned to the customer.
7. WFS_EXEE_CIM_INPUTREFUSE
8. Return bunch 1 of items to customer.
The Service Provider implicitly opens the
shutter and implicitly presents the bunch of
items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
9. Tell the customer that the items
were not accepted, and to take the
items. The customer should be
informed that the items will be
returned in multiple bunches. If
there are additional bunches to
deliver then this can be determined
from the output parameter of the
WFS_SRVE_CIM_ITEMSPRESE
NTED event.
10. Customer does not take
the bunch of items.
11. After some time the application WFSCancelAsyncRequest is executed to end
timeout waiting for the items to be the WFS_CMD_CIM_CASH_IN command.
taken is reached
12. * If command cancellation is supported the
WFS_CMD_CIM_CASH_IN completes
with WFS_ERR_CANCELED.
13. All items are retracted. WFS_CMD_CIM_RETRACT
14. End of transaction.
167

CWA 16926-15:2025 (E)
