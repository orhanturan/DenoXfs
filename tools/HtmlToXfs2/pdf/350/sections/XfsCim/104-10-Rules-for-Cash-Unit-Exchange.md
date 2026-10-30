---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "10. Rules for Cash Unit Exchange"
type: section
source_lines: [7958, 8084]
---

<!-- 10. Rules for Cash Unit Exchange -->

10. Rules for Cash Unit Exchange
The XFS Start and End Exchange commands should be used by applications to supply the latest information with
regards to cash unit replenishment state and content. This guarantees a certain amount of control to an application
as to which denominations are stored in which position as well as the general physical state of the logical/physical
cash units.
If a cash unit is removed from the CIM outside of the Start/End Exchange operations and subsequently reinserted
the status of the physical cash unit should be set to WFS_CIM_STATCUMANIP to indicate to the application that
the physical cash unit has been removed, reinserted and possibly tampered with. While the cash unit has this status
the Service Provider should not attempt to use it as part of a cash-in operation. The WFS_CIM_STATCUMANIP
status should not change until the next Start/End Exchange operation is performed, even if the cash unit is replaced
in its original position.
If all the physical cash units belonging to a logical cash unit are manipulated the parent logical cash unit that the
physical cash units belong to should also have its status set to WFS_CIM_STATCUMANIP.
When a cash unit is removed and/or replaced outside of the Start/End Exchange operations the original logical cash
unit information such as the values, currency and counts should be preserved in the Cash Unit Info structure
reported to the application for accounting purposes until the next Start/End Exchange operations, even if the cash
unit physically contains a different denomination.
Mixed Media Processing:
Where the device supports cash units that can store non-CIM items, a counters update to those cash units applied by
the CIM interface can also be seen in the other interfaces available to the compound device.
The CIM ulCount on a shared bin (of type WFS_CIM_TYPECASHIN) reports the total number of banknotes,
checks or coins of all types in the cash unit. This is for the following reasons:
1. ulCount on CIM has the same meaning as ulCount on IPM. That is the number of items of any type in the bin.
2. ulMaximum, is truly representative of the capacity of the physical bin and software thresholds can accurately
reflect the state of the bin.
3. Use of ulCount representing items from both interfaces gives the greatest flexibility. Dedicated CIM or IPM bins
and therefore counts can still be achieved through bin configuration.
4. The actual number of notes can be determined from lpNoteNumberList.
The following table describes the effect on the IPM counts where an application causes counter changes to a shared
cassette using the CIM interface. The example assumes the starting position of a shared CIM cash unit/IPM media
bin:
From WFSCIMCASHIN:
fwType = WFS_CIM_TYPECASHIN
fwItemType = WFS_CIM_CITYPALL|WFS_CIM_CITYPIPM
ulCashInCount = 0
ulCount = 0
And the IPM starting position for the shared CIM cash unit/IPM media bin:
From WFSIPMMEDIABIN:
fwType = WFS_IPM_TYPEMEDIAIN
wMediaType = WFS_IPM_MEDIATYPCOMPOUND
ulMediaInCount = 0
ulCount = 0
Application Activity CIM Counts on the shared IPM Counts on the
cash unit shared media bin
1. A customer enters 10 good notes and 10 good ulCashInCount = 10 ulMediaInCount = 10
checks in the same transaction. ulCount = 20 ulCount = 20
2. Replenishment activity removes all items from ulCashInCount = 0 ulMediaInCount = 0
the cash unit and clears the counts using ulCount = 0 ulCount = 0
WFS_CMD_CIM_END_EXCHANGE
187

CWA 16926-15:2025 (E)
3. A further customer enters 10 good notes and 10 ulCashInCount = 10 ulMediaInCount = 10
good checks in the same transaction. ulCount = 20 ulCount = 20
4. Replenishment activity removes only cash ulCashInCount = 0 ulMediaInCount = 10
items from the cash unit. The CIM counts are ulCount = 10 ulCount = 10
adjusted using
WFS_CMD_CIM_SET_CASH_UNIT_INFO
ulCashInCount is set to 0, and ulCount is set to
10
5. A further customer enters 10 good notes and 10 ulCashInCount = 10 ulMediaInCount = 20
good checks in the same transaction. ulCount = 30 ulCount = 30
6. Replenishment activity removes only checks ulCashInCount = 10 ulMediaInCount = 0
(20 items) from the cash unit. The counts are ulCount = 10 ulCount = 10
adjusted using
WFS_CMD_IPM_SET_MEDIA_BIN_INFO.
ulMediaInCount is set to 0, and ulCount is set
to 10
Multiple Physical Cash Units:
Where a logical cash unit contains more than one physical cash unit and is configured to accept or dispense more
than one note ID, the breakdown of notes contained within each physical cash unit can be tracked or specified if the
Service Provider supports the NOTENUMBERLIST string in the physical cash unit lpszExtra (see
WFSCIMPHCU). Support for this is defined by the bPhysicalNoteList capability.
It is not mandatory to specify the NOTENUMBERLIST string in an Exchange even if supported; the Service
Provider will track the counts from the point of the replenishment.
The following flow shows how this can be used:
User/Application Activity Logical Cash Unit Counts Physical Cash Unit Counts
1 The device is replenished by ulCount = 1000 lppPhysical[0]:
inserting two physical cash units
lpNoteNumberList: ulCount = 500
which are associated with one
logical cash unit. The first physical usNumOfNoteNumbers = 2 NOTENUMBERLIST=1,500
cash unit contains 500 x usNoteID 1,
lppNoteNumber[0].usNoteID = 1
the second cash unit contains 500 x
usNoteID 2. Application performs an lppNoteNumber[0].ulCount = 500 lppPhysical[1]:
Exchange to set the counts including
lppNoteNumber[1].usNoteID = 2 ulCount = 500
the NOTENUMBERLIST in the
physical cash units. lppNoteNumber[1].ulCount = 500 NOTENUMBERLIST=2,500
2 After several transactions, the first ulCount = 1600 lppPhysical[0]:
. physical cash unit is full and requires
lpNoteNumberList: ulCount = 1000
replenishment. Application queries
the counts. usNumOfNoteNumbers = 2 NOTENUMBERLIST=1,800;2,200
lppNoteNumber[0].usNoteID = 1
lppNoteNumber[0].ulCount = 900 lppPhysical[1]:
lppNoteNumber[1].usNoteID = 2 ulCount = 600
lppNoteNumber[1].ulCount = 700 NOTENUMBERLIST=1,100;2,500
188

CWA 16926-15:2025 (E)
3 The first physical cash unit is ulCount = 600 lppPhysical[0]:
. removed. The logical cash unit
lpNoteNumberList: ulCount = 600
counts can now report only what is
in the remaining physical cash unit. usNumOfNoteNumbers = 2 NOTENUMBERLIST=1,100;2,500
Application queries the counts
lppNoteNumber[0].usNoteID = 1
lppNoteNumber[0].ulCount = 100
lppNoteNumber[1].usNoteID = 2
lppNoteNumber[1].ulCount = 500
4 A new cash unit is inserted ulCount = 900 lppPhysical[0]:
. containing 300 x usNoteID 1. As the
lpNoteNumberList: ulCount = 300
application already knows the
contents of the remaining physical usNumOfNoteNumbers = 2 NOTENUMBERLIST=1,300
cash unit, the logical cash unit
lppNoteNumber[0].usNoteID = 1
counts can be calculated.
Application performs an Exchange lppNoteNumber[0].ulCount = 400 lppPhysical[1]:
to set the counts.
lppNoteNumber[1].usNoteID = 2 ulCount = 600
lppNoteNumber[1].ulCount = 500 NOTENUMBERLIST=1,100;2,500
189

CWA 16926-15:2025 (E)
