---
source: XfsDep.pdf
document: XfsDep
device_class: DEP
part: 17
section_id: ""
title: "2. Depository Unit"
type: section
source_lines: [276, 295]
---

<!-- 2. Depository Unit -->

2. Depository Unit
This specification describes the functionality of the services provided by the Depository (DEP) services under XFS,
by defining the service-specific commands that can be issued, using the WFSGetInfo, WFSAsyncGetInfo,
WFSExecute and WFSAsyncExecute functions.
A Depository is used for the acceptance and deposit of media into the device or terminal. There are two main types
of depository: an envelope depository for the deposit of media in envelopes and a night safe depository for the
deposit of bags containing bulk media.
An envelope depository accepts media, prints on the media and deposits the media into a holding container or bin.
Some envelope depositories offer the capability to dispense an envelope to the customer at the start of a transaction.
The customer takes this envelope, fills in the deposit media, possibly inscribes it and puts it into the deposit slot.
The envelope is then accepted, printed and transported into a deposit container.
The envelope dispense mechanism may be part of the envelope depository device mechanism with the same
entry/exit slot or it may be a separate mechanism with separate entry/exit slot.
Envelopes dispensed and not taken by the customer can be retracted back into the device. When the dispenser is a
separate mechanism the envelope is retracted back into the dispenser container. When the dispenser is a common
mechanism the envelope is retracted into the depository container.
A night safe depository normally only logs the deposit of a bag and does not print on the media.
9

CWA 16926-8:2020 (E)
