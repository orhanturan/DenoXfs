---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "2. Cash-In Module"
type: section
source_lines: [391, 431]
---

<!-- 2. Cash-In Module -->

2. Cash-In Module
This specification describes the functionality of an XFS compliant Cash-In Module (CIM) Service Provider. It
defines the service-specific commands that can be issued to the Service Provider using the WFSGetInfo,
WFSAsyncGetInfo, WFSExecute and WFSAsyncExecute functions.
Persistent values are maintained through power failures, open sessions, close session and system resets.
This specification covers the acceptance of items. An “item” is defined as any media that can be accepted and
includes coupons, documents, bills and coins. However, if coins and bills are both to be accepted separate Service
Providers must be implemented for each.
All currency parameters in this specification are expressed as a quantity of minimum dispense units, as defined in
the description of the WFS_INF_CIM_CURRENCY_EXP command.
There are two types of CIM: Self-Service CIM and Teller CIM. A Self-Service CIM operates in an automated
environment, while a Teller CIM has an operator present. The functionality provided by the following commands is
only applicable to a Teller CIM:
WFS_CMD_CIM_SET_TELLER_INFO
WFS_INF_CIM_SET_TELLER_INFO
It is possible for the CIM to be part of a compound device with the Cash Dispenser Module (CDM). This
CIM\CDM combination is referred to throughout this specification as a “cash recycler”. For details of the CDM
interface see [Ref. 3].
If the device is a cash recycler then, if cash unit exchanges are required on both interfaces, the exchanges cannot be
performed concurrently. An exchange on one interface must be complete (the
WFS_CMD_CIM_END_EXCHANGE must have completed) before an exchange can start on the other interface.
The WFS_ERR_CIM_EXCHANGEACTIVE error code will be returned if the correct sequence is not adhered to.
The CIM interface can be used for all exchange operations on cash recycle devices, and this interface should be
used for cash units of multiple currencies and/or denominations (including multiple note identifiers associated with
the same denomination).
The event WFS_SRVE_CIM_COUNTS_CHANGED will be posted if an operation on the CDM interface affects
the recycle cash unit counts which are available through the CIM interface.
The following commands on the CDM interface may affect the CIM counts:
WFS_CMD_CDM_DISPENSE
WFS_CMD_CDM_PRESENT
WFS_CMD_CDM_RETRACT
WFS_CMD_CDM_COUNT
WFS_CMD_CDM_REJECT
WFS_CMD_CDM_SET_CASH_UNIT_INFO
WFS_CMD_CDM_END_EXCHANGE
WFS_CMD_CDM_CALIBRATE_CASH_UNIT
WFS_CMD_CDM_RESET
WFS_CMD_CDM_TEST_CASH_UNITS
11

CWA 16926-15:2025 (E)
