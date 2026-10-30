---
source: XfsIpm.pdf
document: XfsIpm
device_class: IPM
part: 19
section_id: ""
title: "2. Item Processing Module"
type: section
source_lines: [319, 364]
---

<!-- 2. Item Processing Module -->

2. Item Processing Module
This specification describes the XFS service class for Item Processing Modules (IPM). The specification of this
service class includes definitions of the service-specific commands that can be issued, using the
WFSAsyncExecute, WFSExecute, WFSGetInfo and WFSAsyncGetInfo functions.
This service class is currently defined only for self service devices.
In the U.S., checks are always encoded in magnetic ink for reading by Magnetic Ink Character Recognition
(MICR), and a single font is always used. In Europe some countries use MICR and some use Optical Character
Recognition (OCR) character sets, with different fonts, for their checks.
Item Processing Modules accept one or more media items (Checks, Giros, etc) and process these items according to
application requirements. The IPM class supports devices that can handle a single item as well as those devices that
can handle bunches of items. The following are the three principle device types:
• Single Item: can accept and process a single item at a time.
• Multi-Item Feed with no stacker (known as an escrow in some environments): can accept a bunch of
media from the customer but each item has to be processed fully (i.e. deposited in a bin or returned) before
the next item can be processed.
• Multi-Item Feed with a stacker: can accept a bunch of media from the customer and all items can be
processed together.
The IPM class provides applications with an interface to control the following functions (depending on the
capabilities of the specific underlying device):
• Capture an image of the front of an item in multiple formats and bit depths.
• Capture an image of the back of an item in multiple formats and bit depths.
• Read the code line of an item using MICR reader.
• Read the code line of an item using OCR.
• Endorse (print text) on an item.
• Stamp an item.
• Return an item to the customer.
• Deposit an item in a bin.
• Retract items left by the customer.
The IPM device class uses the concept of a Media-In transaction to track and control a customer’s interaction with
the device. A Media-In transaction consists of one or more WFS_CMD_IPM_MEDIA_IN commands. The
transaction is initiated by the first WFS_CMD_IPM_MEDIA_IN command and remains active until the transaction
is either confirmed through WFS_CMD_IPM_MEDIA_IN_END, or terminated by
WFS_CMD_IPM_MEDIA_IN_ROLLBACK, WFS_CMD_IPM_RETRACT_MEDIA or
WFS_CMD_IPM_RESET. While a transaction is active the WFS_INF_IPM_TRANSACTION_STATUS
command reports the status of the current transaction. When a transaction is not active the
WFS_INF_IPM_TRANSACTION_STATUS command reports the status of the last transaction as well as some
current status values.
There are primarily two types of devices supported by the IPM, those devices with a stacker and those without.
In this the specification the terms “long edge” and “short edge” are used to describe the orientation of a check and
length of its edges. The diagram below illustrates these definitions.
10

CWA 16926-18:2020 (E)
11

CWA 16926-18:2020 (E)
