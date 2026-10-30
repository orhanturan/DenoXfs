---
source: XfsCdm.pdf
document: XfsCdm
device_class: CDM
part: 5
section_id: ""
title: "8. Sub-Dispensing Command Flow"
type: section
source_lines: [4065, 4253]
---

<!-- 8. Sub-Dispensing Command Flow -->

8.  Sub-Dispensing Command Flow
“Sub-dispensing” of bills occur when a WFS_CMD_CDM_DISPENSE execute command is issued and the
required number of bills to be dispensed exceeds the CDM hardware limit for bills that can be dispensed with a
single “hardware level” dispense command. In this situation, the CDM Service Provider determines the number of
“hardware level” dispense commands required and enters what is referred to as a “sub-dispensing” operation until
the full amount has been dispensed. Through use of a “sub-dispensing” operation the application is fully removed
from “hardware level dependencies” as to how many bills can be dispensed based on hardware vendor design
limitations.
The following series of tables illustrate the steps taken on behalf of an end-user, application, XFS Service Provider,
and CDM hardware for sub-dispensing operations: All examples below assume the bPresent field in the
WFS_CMD_CDM_DISPENSE command is set to TRUE.
Sub-Dispensing Is Not Required - Transaction Successful
This table illustrates a successful WFS_CMD_CDM_DISPENSE command where sub-dispensing is not required:
| Step  End-User  | Application  | XFS SP  | CDM  |
| --------------- | ------------ | ------- | ---- |
Hardware
| 1.  User wants to  |     |     |     |
| ------------------ | --- | --- | --- |
dispense 40 USD.
| 2.    | WFS_CMD_CDM_DISPENSE  |     |     |
| ----- | --------------------- | --- | --- |
command issued.
| 3.    |     | Determines that a single “hardware level”  |     |
| ----- | --- | ------------------------------------------ | --- |
dispense command can be issued for full
dispense request.
| 4.    |     | “Hardware level” dispense command  |     |
| ----- | --- | ---------------------------------- | --- |
issued.
| 5.    |     | WFS_SRVE_CDM_SHUTTERSTATUS | Items       |
| ----- | --- | -------------------------- | ----------- |
|       |     | CHANGED(WFS_CDM_SHTOPEN)   | presented.  |
event generated
| 6.    | WFS_CMD_CDM_DISPENSE  |     |     |
| ----- | --------------------- | --- | --- |
completes successfully.
| 7.  User takes bills.  |     |                          |     |
| ---------------------- | --- | ------------------------ | --- |
| 8.                     |     | WFS_SRVE_CDM_ITEMSTAKEN  |     |
event generated.
WFS_SRVE_CDM_SHUTTERSTATUS
CHANGED(WFS_CDM_SHTCLOSED)
event generated
109

CWA 16926-5:2022 (E)
Sub-Dispensing Is Required - Command Successful
This table illustrates a successful WFS_CMD_CDM_DISPENSE command where sub-dispensing is required:
| Step  End-User  | Application  | XFS SP  | CDM  |
| --------------- | ------------ | ------- | ---- |
Hardware
| 1.  User wants to  |     |     |     |
| ------------------ | --- | --- | --- |
dispense 130 USD in
1 USD
denominations.
| 2.    | WFS_CMD_CDM_DISPENSE  |     |     |
| ----- | --------------------- | --- | --- |
command issued.
| 3.    |     | Three “hardware level” dispense  |     |
| ----- | --- | -------------------------------- | --- |
commands are required. CDM hardware
is limited to dispensing 50 bills in any
single “hardware level” dispense.
| 4.    |     | WFS_EXEE_CDM_PARTIAL- |     |
| ----- | --- | --------------------- | --- |
DISPENSE event generated.
| 5.    |     | “Hardware level” dispense command  |     |
| ----- | --- | ---------------------------------- | --- |
issued for 50 USD.
| 6.    |     | WFS_SRVE_CDM_SHUTTERSTATUS | Items       |
| ----- | --- | -------------------------- | ----------- |
|       |     | CHANGED(WFS_CDM_SHTOPEN)   | presented.  |
event generated
| 7.    |     | WFS_SRVE_CDM_SUBDISPENSEOK  |     |
| ----- | --- | --------------------------- | --- |
event generated.
| 8.  User takes bills.  |     |                          |     |
| ---------------------- | --- | ------------------------ | --- |
| 9.                     |     | WFS_SRVE_CDM_ITEMSTAKEN  |     |
event generated.
WFS_SRVE_CDM_SHUTTERSTATUS
CHANGED(WFS_CDM_SHTCLOSED)
event generated
| 10.    |     | “Hardware level” dispense command  |     |
| ------ | --- | ---------------------------------- | --- |
issued for 50 USD.
| 11.    |     | WFS_SRVE_CDM_SHUTTERSTATUS | Items       |
| ------ | --- | -------------------------- | ----------- |
|        |     | CHANGED(WFS_CDM_SHTOPEN)   | presented.  |
event generated
| 12.    |     | WFS_SRVE_CDM_SUBDISPENSE_O |     |
| ------ | --- | -------------------------- | --- |
K event generated.
| 13.  User takes bills.  |     |                          |     |
| ----------------------- | --- | ------------------------ | --- |
| 14.                     |     | WFS_SRVE_CDM_ITEMSTAKEN  |     |
event generated.
WFS_SRVE_CDM_SHUTTERSTATUS
CHANGED(WFS_CDM_SHTCLOSED)
event generated
| 15.    |     | “Hardware level” dispense command  |     |
| ------ | --- | ---------------------------------- | --- |
issued for 30 USD.
| 16.    |     | WFS_SRVE_CDM_SHUTTERSTATUS | Items       |
| ------ | --- | -------------------------- | ----------- |
|        |     | CHANGED(WFS_CDM_SHTOPEN)   | presented.  |
event generated
| 17.    |     | WFS_SRVE_CDM_SUBDISPENSEOK  |     |
| ------ | --- | --------------------------- | --- |
event generated.
| 18.    | WFS_CMD_CDM_DISPENSE  |     |     |
| ------ | --------------------- | --- | --- |
completes successfully.
| 19.  User takes bills.  |     |                          |     |
| ----------------------- | --- | ------------------------ | --- |
| 20.                     |     | WFS_SRVE_CDM_ITEMSTAKEN  |     |
event generated.
WFS_SRVE_CDM_SHUTTERSTATUS
CHANGED(WFS_CDM_SHTCLOSED)
event generated
110

CWA 16926-5:2022 (E)
Sub-Dispensing Is Required - Command Unsuccessful
This table illustrates an unsuccessful WFS_CMD_CDM_DISPENSE command where sub-dispensing is required
and the end-user does not take the bills during the second “hardware level” dispense, resulting in a timeout
condition.
| Step  End-User  | Application  | XFS SP  | CDM  |
| --------------- | ------------ | ------- | ---- |
Hardware
| 1.  User wants to  |     |     |     |
| ------------------ | --- | --- | --- |
dispense 130 USD in
1 USD
denominations.
| 2.    | WFS_CMD_CDM_DISPENSE  |     |     |
| ----- | --------------------- | --- | --- |
command issued.
| 3.    |     | Three “hardware level” dispense  |     |
| ----- | --- | -------------------------------- | --- |
commands are required. CDM hardware
is limited to dispensing 50 bills in any
single “hardware level” dispense
command.
| 4.    |     | WFS_EXEE_CDM_PARTIAL- |     |
| ----- | --- | --------------------- | --- |
DISPENSE event generated.
| 5.    |     | “Hardware level” dispense command  |     |
| ----- | --- | ---------------------------------- | --- |
issued for 50 USD.
| 6.    |     | WFS_SRVE_CDM_SHUTTERSTATUS | Items      |
| ----- | --- | -------------------------- | ---------- |
|       |     | CHANGED(WFS_CDM_SHTOPEN)   | presented  |
event generated
| 7.    |     | WFS_SRVE_CDM_SUBDISPENSEOK  |     |
| ----- | --- | --------------------------- | --- |
event generated.
| 8.  User takes bills.  |     |                          |     |
| ---------------------- | --- | ------------------------ | --- |
| 9.                     |     | WFS_SRVE_CDM_ITEMSTAKEN  |     |
event generated.
WFS_SRVE_CDM_SHUTTERSTATUS
CHANGED(WFS_CDM_SHTCLOSED)
event generated
| 10.    |     | “Hardware level” dispense command  |     |
| ------ | --- | ---------------------------------- | --- |
issued for 50 USD.
| 11.    |     | WFS_SRVE_CDM_SHUTTERSTATUS | Items       |
| ------ | --- | -------------------------- | ----------- |
|        |     | CHANGED(WFS_CDM_SHTOPEN)   | presented.  |
event generated
| 12.    |     | WFS_SRVE_CDM_SUBDISPENSEOK  |     |
| ------ | --- | --------------------------- | --- |
event generated.
| 13.  User does not take  |     |     |     |
| ------------------------ | --- | --- | --- |
bills.
| 14.    |     | Timeout occurs waiting on end-user to  |     |
| ------ | --- | -------------------------------------- | --- |
take bills.
| 15.    | WFS_CMD_CDM_DISPENSE  |     |     |
| ------ | --------------------- | --- | --- |
completes with
WFS_ERR_CDM_ITEMSNOT-
TAKEN.
111

CWA 16926-5:2022 (E)
