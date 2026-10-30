---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "Shutter Control)"
type: section
source_lines: [7030, 7482]
---

<!-- Shutter Control) -->

Shutter Control).
10.    Transport the items recognized to  * WFS_CMD_CIM_CASH_IN_ROLLBACK
|        | an internal position.  | initiated                       |
| ------ | ---------------------- | ------------------------------- |
| 11.    |                        | * WFS_CMD_CIM_CASH_IN_ROLLBACK  |
completes.
12.    Present items to the customer.  * WFS_CMD_CIM_PRESENT_MEDIA
initiated.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
| 13.    |     | * WFS_CMD_CIM_PRESENT_MEDIA  |
| ------ | --- | ---------------------------- |
completes.
| 14.                      | Request removal of the money.  |     |
| ------------------------ | ------------------------------ | --- |
| 15.  Customer takes the  |                                |     |
money from the output
position.
| 16.    |     | WFS_SRVE_CIM_ITEMSTAKEN  |
| ------ | --- | ------------------------ |
The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
| 17.    | End of transaction.  |     |
| ------ | -------------------- | --- |
167

CWA 16926-15:2020 (E)
8.16 Multiple Bunch Timeout Handling
The following sections describe flows where the Service Provider could potentially present refused items in
multiple bunches during the WFS_CMD_CIM_CASH_IN command. As the WFS_CMD_CIM_CASH_IN timeout
(dwTimeout parameter in WFSAsyncExecute or WFSExecute) may elapse before the last bunch is presented,
resulting in a WFS_ERR_TIMEOUT in the completion event, it is recommended that the application take control
by specifying a long dwTimeout and use timers to allow sufficient time for user interaction before cancelling the
command. dwTimeout should be set sufficiently long to allow for any scenario; it could be set to
WFS_INDEFINITE_WAIT as the command would be explicitly cancelled by the application if timers elapse.
Each flow covers the following cases:
• bShutterControl == TRUE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE,
bPresentControl == TRUE
8.16.1 No Items Inserted
In this flow, the user does not insert items within the required time, therefore the application cancels the
WFS_CMD_CIM_CASH_IN command using WFS_CMD_CIM_CASH_IN_END.
Step Customer Application XFS Command
1. Customer selects cash-in WFS_CMD_CIM_CASH_IN_START
operation.
2. * WFS_CMD_CIM_CASH_IN initiated
with a long timeout (for example,
WFS_INDEFINITE_WAIT) using
WFSAsyncExecute
The Service Provider implicitly opens the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_EXEE_CIM_INSERTITEMS event
is sent when the shutter is fully open and
the device is ready to begin accepting
items.
3. Ask the customer to insert money.
Application sets an insertion timer.
4. Customer does not insert
money.
5. The insertion timer elapses WFSCancelAsyncRequest is executed to end
the WFS_CMD_CIM_CASH_IN command.
6. * If command cancellation is supported the
WFS_CMD_CIM_CASH_IN completes
with WFS_ERR_CANCELED.
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
7. Transaction cancelled WFS_CMD_CIM_CASH_IN_END
8. End of transaction.
8.16.2 First Bunch Not Taken
In this flow, the user does not take the first returned bunch within the required time, therefore the application
cancels the WFS_CMD_CIM_CASH_IN command. The same sequence can be extended to any bunch other than
the last bunch as this would complete the WFS_CMD_CIM_CASH_IN command; each time a new bunch is
presented a new presentation timer should be set.
168

CWA 16926-15:2020 (E)
Step Customer Application XFS Commands and Events
1.-3. See No Items Inserted
4. Customer inserts money If bItemsInsertedSensor == TRUE:
WFS_SRVE_CIM_ITEMSINSERTED
The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
The bill recognition begins.
5. Insertion timer cancelled
6. As a result of the bill processing n
bunches of items must be returned to the
customer.
7. WFS_EXEE_CIM_INPUTREFUSE
8. Return bunch 1 of items to customer.
The Service Provider implicitly opens the
shutter and implicitly presents the bunch
of items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
9. Tell the customer that the items
were not accepted, and to take
the items. The customer should
be informed that the items will
be returned in multiple bunches.
If there are additional bunches
to deliver then this can be
determined from the output
parameter of the
WFS_SRVE_CIM_ITEMSPRE
SENTED event.
Presentation timer set
10. Customer does not take the The presentation timer elapses WFSCancelAsyncRequest is executed to
items end the WFS_CMD_CIM_CASH_IN
command.
* If command cancellation is supported
the WFS_CMD_CIM_CASH_IN
completes with
WFS_ERR_CANCELED.
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
11. All items are retracted. WFS_CMD_CIM_RETRACT
12. End of transaction.
8.16.3 Last Bunch Taken
In this flow, two bunches are to be returned & the user takes all of the returned bunches within the required time,
therefore WFS_CMD_CIM_CASH_IN command completes normally.
169

CWA 16926-15:2020 (E)
| Step  Customer                    | Application  | XFS Commands and Events  |
| --------------------------------- | ------------ | ------------------------ |
| 1.-9.  See First Bunch Not Taken  |              |                          |
| 10.  Customer takes the bunch     |              | WFS_SRVE_CIM_ITEMSTAKEN  |
The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
11.    Presentation timer cancelled  Return bunch 2 of items to customer. The
Service Provider implicitly opens the
shutter and implicitly presents the bunch
of items.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_SRVE_CIM_ITEMSPRESENTED
| 12.    |     | * WFS_CMD_CIM_CASH_IN  |
| ------ | --- | ---------------------- |
completes with WFS_SUCCESS.
| 13.  Customer takes the bunch of  |     |     |
| --------------------------------- | --- | --- |
items.
| 14.    |     | WFS_SRVE_CIM_ITEMSTAKEN  |
| ------ | --- | ------------------------ |
The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
| 15.    | Display the amount recognized  |     |
| ------ | ------------------------------ | --- |
so far.
| 16.    | Ask the customer for further  |     |
| ------ | ----------------------------- | --- |
actions:

If the customer wants to deposit
the amount:
Continue with step 17.

If the customer wants to get
back all items inserted so far see
table "Cancellation by
Customer (Implicit Shutter
Control)"
17.    Transport the money into the  WFS_CMD_CIM_CASH_IN_END
cash units of type
WFS_CIM_TYPERECYCLIN
G / WFS_CIM_TYPECASHIN.
| 18.    | Credit the money to the  |     |
| ------ | ------------------------ | --- |
customer's account.
| 19.    | End of transaction.  |     |
| ------ | -------------------- | --- |
170

CWA 16926-15:2020 (E)
8.17 Exchange using DEPOSITINTO (Implicit Shutter Control)
The following table describes an Exchange using the WFS_CIM_DEPOSITINTO parameter to specify that items
will be deposited using the deposit entrance. The shutter is implicitly controlled by the Service Provider. In this
case the WFS_CMD_CIM_OPEN_SHUTTER and WFS_CMD_CIM_CLOSE_SHUTTER commands are not
explicitly used by the application.
Although this re-uses Cash In transaction commands to move the items, the Exchange is not restricted by the
maximum number of items in a Cash In transaction (fwIntermediateStacker) as the Exchange can be performed
using multiple deposits. Items may be returned or captured per local policy and configuration. Despite using the
standard Cash In transaction commands, this sequence does not constitute one or more Cash In transactions
therefore is not reported by WFS_INF_CIM_CASH_IN_STATUS. Other Cash In transaction commands such as
WFS_CMD_CIM_CASH_IN_ROLLBACK can be used if required. Note also that in this example flow, each
bunch will be transported to cash units before additional items can be inserted; it is equally valid to accept multiple
bunches before depositing the items to the cash units.
This example flow covers cases where all the items are accepted during WFS_CMD_CIM_CASH_IN;
unrecognized items may be deposited to a cash unit with the fwItemType containing WFS_CIM_CITYPLEVEL1.
Refer to other example flows for how refused items would be handled.
This flow covers the following case:
•  bShutterControl == TRUE, bItemsInsertedSensor == TRUE, fwIntermediateStacker != 0

| Step  User                   | Application  | XFS Commands and Events     |
| ---------------------------- | ------------ | --------------------------- |
| 1.  User selects to perform  |              | WFS_CMD_CIM_START_EXCHANGE  |
| a replenishment using        |              | with fwExchangeType ==      |
| the deposit entrance.        |              | WFS_CIM_DEPOSITINTO.        |
| 2.                           |              | WFS_CMD_CIM_CASH_IN_START   |
called to specify the input position.
| 3.    |     | WFS_CMD_CIM_CASH_IN initiated  |
| ----- | --- | ------------------------------ |
The Service Provider implicitly opens the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTOPEN)
WFS_EXEE_CIM_INSERTITEMS event is
sent when the shutter is fully open and the
device is ready to begin accepting items.
| 4.                       | Ask the user to insert items.  |                             |
| ------------------------ | ------------------------------ | --------------------------- |
| 5.  User inserts items.  |                                |                             |
| 6.                       |                                | WFS_SRVE_CIM_ITEMSINSERTED  |
The Service Provider implicitly closes the
shutter.
…
WFS_SRVE_CIM_SHUTTERSTATUS-
CHANGED(WFS_CIM_SHTCLOSED)
The bill recognition begins.
| 7.    |     | WFS_CMD_CIM_CASH_IN command  |
| ----- | --- | ---------------------------- |
completes.
| 8.    | Display the number of items  |     |
| ----- | ---------------------------- | --- |
and/or amount recognized so far.
| 8.    | Transport the items into the  | WFS_CMD_CIM_CASH_IN_END  |
| ----- | ----------------------------- | ------------------------ |
designated cash units.
171

CWA 16926-15:2020 (E)
| 9.    | Ask the user for further actions:  |     |
| ----- | ---------------------------------- | --- |

If the user wants to insert more
items:
Repeat from step 2.

If the user wants to complete the
Exchange operation:
Continue with step 10.
| 10.  Selection: Complete  |     |                                 |
| ------------------------- | --- | ------------------------------- |
| 11.                       |     | WFS_CMD_CIM_END_EXCHANGE. This  |
can be specified with a NULL input
parameter as all the notes will have been
counted and cash unit counts adjusted
accordingly during the preceding operations.
| 12.    | End of Exchange.  |     |
| ------ | ----------------- | --- |
172

CWA 16926-15:2020 (E)
8.18 Multiple Bunches Returned During WFS_CMD_CIM_CASH_IN Refused
Notes (using WFS_CMD_CIM_PREPARE_PRESENT)
The following table describes the flow of a cash-in transaction where items are rejected during the transaction. The
application uses WFS_CMD_CIM_PREPARE_PRESENT commands to move items to the output position. The
Service Provider has explicit shutter control. In this case the WFS_CMD_CIM_OPEN_SHUTTER and
WFS_CMD_CIM_CLOSE_SHUTTER commands are used for the user to take items. Additionally, the number of
items refused may be greater than the number of items that can be presented at the output position.
This flow covers the following cases:
• bShutterControl == FALSE, bItemsInsertedSensor == TRUE, bItemsTakenSensor == TRUE,
bPresentControl == FALSE, bPreparePresent == TRUE
Step Customer Application XFS Commands and Events
1.-6. See OK
Transaction
(Explicit
Shutter
Control).
7. * WFS_CMD_CIM_CASH_IN initiated.
The bill recognition begins.
* WFS_CMD_CIM_CASH_IN resets the
lpTotalReturnedItems output parameter
of
WFS_INF_CIM_PRESENT_STATUS.
8. WFS_EXEE_CIM_INPUTREFUSE
(WFS_CIM_INVALIDBILL)
…
* WFS_CMD_CIM_CASH_IN
completes with WFS_SUCCESS
9. Move refused items to the output position. WFS_CMD_CIM_PREPARE_PRESEN
T
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
173

CWA 16926-15:2020 (E)
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

174

CWA 16926-15:2020 (E)
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
175

CWA 16926-15:2020 (E)
| 18.    | If more items need to be taken:  |     |
| ------ | -------------------------------- | --- |
|        |      Repeat steps 13. – 18.      |     |
Else
     Go to step 19.
| 19.    | End of transaction.  |     |
| ------ | -------------------- | --- |

176

CWA 16926-15:2020 (E)
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
