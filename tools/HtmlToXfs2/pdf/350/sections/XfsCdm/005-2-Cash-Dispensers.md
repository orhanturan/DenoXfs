---
source: XfsCdm.pdf
document: XfsCdm
device_class: CDM
part: 5
section_id: ""
title: "2. Cash Dispensers"
type: section
source_lines: [330, 369]
---

<!-- 2. Cash Dispensers -->

2. Cash Dispensers
This specification describes the functionality of an XFS compliant Cash Dispenser Module (CDM) Service
Provider. It defines the service-specific commands that can be issued to the Service Provider using the
WFSGetInfo, WFSAsyncGetInfo, WFSExecute and WFSAsyncExecute functions.
Persistent values are maintained through power failures, open sessions, close session and system resets.
This specification covers the dispensing of items. An “item” is defined as any media that can be dispensed and
includes coupons, documents, bills and coins. However, if coins and bills are both to be dispensed separate Service
Providers must be implemented for each.
All currency parameters in this specification are expressed as a quantity of minimum dispense units, as defined in
the description of the WFS_INF_CDM_CURRENCY_EXP command.
There are two types of CDM: Self-Service CDM and Teller CDM. A Self-Service CDM operates in an automated
environment, while a Teller CDM has an operator present. The functionality provided by the following commands
is only applicable to a Teller CDM:
WFS_CMD_CDM_SET_TELLER_INFO
WFS_INF_CDM_TELLER_INFO
It is possible for the CDM to be part of a compound device with the Cash-In Module (CIM). This CIM\CDM
combination is referred to throughout this specification as a “Cash Recycler”. For details of the CIM interface see
[Ref. 3].
If the device is a Cash Recycler then, if cash unit exchanges are required on both interfaces, the exchanges cannot
be performed concurrently. An exchange on one interface must be complete (the
WFS_CMD_CDM_END_EXCHANGE must have completed) before an exchange can start on the other interface.
The WFS_ERR_CDM_EXCHANGEACTIVE error code will be returned if the correct sequence is not adhered to.
The CIM interface can be used for all exchange operations on recycle devices, and the CIM interface should be
used if the device has recycle units of multiple currencies and/or denominations (including multiple note identifiers
associated with the same denomination).
The event WFS_SRVE_CDM_COUNTS_CHANGED will be posted if an operation on the CIM interface affects
the cash unit counts which are available through the CDM interface.
The following commands on the CIM interface may affect the CDM counts:
WFS_CMD_CIM_CASH_IN
WFS_CMD_CIM_CASH_IN_END
WFS_CMD_CIM_CASH_IN_ROLLBACK
WFS_CMD_CIM_RETRACT
WFS_CMD_CIM_SET_CASH_IN_UNIT_INFO
WFS_CMD_CIM_END_EXCHANGE
WFS_CMD_CIM_RESET
WFS_CMD_CIM_REPLENISH
WFS_CMD_CIM_CASH_UNIT_COUNT
11

CWA 16926-5:2022 (E)
