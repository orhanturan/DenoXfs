---
source: XfsCdm.pdf
document: XfsCdm
device_class: CDM
part: 5
section_id: ""
title: "12. Appendix E – Cash Dispenser E2E Authentication"
type: appendix
source_lines: [4494, 4504]
---

<!-- 12. Appendix E – Cash Dispenser E2E Authentication -->

12. Appendix E – Cash Dispenser E2E Authentication
This section describes how to perform End to End authentication for a successful Dispense/Present process using
the WFS_CMD_API_GET_COMMAND_NONCE, WFS_CMD_API_SECURE_COMMAND commands and
WFS_INF_API_SECURE_QUERY queries. For a full description of these commands see section 13.2 of the API
document.
An application can determine what functionality must be protected using E2E Authentication by calling the
WFS_INF_API_SERVICE_INFO command and referencing the lpE2EAuthentication parameter. This points to a
structure containing fields which detail the commands and queries that require authentication and also the timeout
for the nonce obtained from the device. If no authentication is required, then lpE2EAuthentication is NULL.
Specified below is a description of the data that can be sent as part of a CDM Dispense process, and this is followed
by flow diagrams showing how various use cases can be executed.
