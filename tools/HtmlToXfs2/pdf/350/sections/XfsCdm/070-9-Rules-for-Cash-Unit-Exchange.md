---
source: XfsCdm.pdf
document: XfsCdm
device_class: CDM
part: 5
section_id: ""
title: "9. Rules for Cash Unit Exchange"
type: section
source_lines: [4254, 4273]
---

<!-- 9. Rules for Cash Unit Exchange -->

9. Rules for Cash Unit Exchange
The XFS Start and End Exchange commands should be used by applications to supply the latest information with
regards to cash unit replenishment state and content. This guarantees a certain amount of control to an application
as to which denominations are stored in which position as well as the general physical state of the logical/physical
cash units.
If a cash unit is removed from the CDM outside of the Start/End Exchange operations and subsequently reinserted
the status of the physical cash unit should be set to WFS_CDM_STATCUMANIP to indicate to the application that
the physical cash unit has been removed, reinserted and possibly tampered with. While the cash unit has this status
the Service Provider should not attempt to use it as part of a Dispense operation. The
WFS_CDM_STATCUMANIP status should not change until the next Start/End Exchange operation is performed,
even if the cash unit is replaced in its original position.
If all the physical cash units belonging to a logical cash unit are manipulated the parent logical cash unit that the
physical cash units belong to should also have its status set to WFS_CDM_STATCUMANIP.
When a cash unit is removed and/or replaced outside of the Start/End Exchange operations the original logical cash
unit information such as the values, currency and counts should be preserved in the Cash Unit Info structure
reported to the application for accounting purposes until the next Start/End Exchange operations, even if the cash
unit physically contains a different denomination.
112

CWA 16926-5:2022 (E)
