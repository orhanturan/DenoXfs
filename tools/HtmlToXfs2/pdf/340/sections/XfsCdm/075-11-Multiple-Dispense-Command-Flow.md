---
source: XfsCdm.pdf
document: XfsCdm
device_class: CDM
part: 5
section_id: ""
title: "11. Multiple Dispense Command Flow"
type: section
source_lines: [4327, 4464]
---

<!-- 11. Multiple Dispense Command Flow -->

11. Multiple Dispense Command Flow
The Multiple Dispense feature occurs when a WFS_CMD_CDM_DISPENSE execute command is issued multiple
times to place items on the stacker. Adding to the stacked items with a second or third dispense may be required
where the initially picked Cash Unit fails to fulfill the full dispense request. The application can choose to add to
the stacker from another Cash Unit to fulfill the request. Additionally this feature covers the requirement to
dispense multiple currencies with a mix number other than WFS_CDM_INDIVIDUAL. Multiple currencies can be
picked separately with the bunch of items assembled on the stacker before presentation to the customer.

Applications can refer to WFSCDMCAPS.fwMoveItems to determine if the Service Provider supports the feature. If
a Service Provider supports this feature but an application does not wish to use it, the application should check
WFSCDMSTATUS.fwIntermediateStacker to determine whether items on the stacker need to be purged prior to a
dispense.
Multiple Dispense Example ‘Out of Notes’

In the following example WFSCDMCAPS.fwMoveItems reports WFS_CDM_TOSTACKER. The CDM has 2
logical Cash Units. Cash Unit 1 has 20USD, Cash Unit 2 has 10USD. Cash Unit 1 has only 2 notes left.
This table illustrates multiple WFS_CMD_CDM_DISPENSE commands to fulfill a dispense request.

| Step  End-User  | Application  | XFS SP  | CDM  |
| --------------- | ------------ | ------- | ---- |
Hardware
| 1.  User wants to  | WFS_CMD_CDM_DISPENSE  |     | 2 of 3 items  |
| ------------------ | --------------------- | --- | ------------- |
| dispense 60        | usMixNumber =         |     | picked and    |
| USD.               | WFS_CDM_INDIVIDUAL    |     | stacked.      |
bPresent = FALSE

Request USD20x3 from Cash Unit 1.

| 2.    |     | WFS_EXEE_CDM_INCOMPLETEDISP |     |
| ----- | --- | --------------------------- | --- |
ENSE event.

lpDenomination output records that
2x20USD were stacked.

| 3.    |     | WFS_CMD_CDM_DISPENSE completes  |     |
| ----- | --- | ------------------------------- | --- |
WFS_ERR_CDM_NOTDISPENSABLE
| 4.    | The application decides that the          |     | 2 of 2 notes   |
| ----- | ----------------------------------------- | --- | -------------- |
|       | dispense can be fulfilled and calculates  |     | picked and     |
|       | the 20USD shortfall can be made up        |     | stacked        |
|       | from the 10USD Cash Unit.                 |     | together with  |
|       |                                           |     | the items      |
|       | WFS_CMD_CDM_DISPENSE                      |     | already        |
|       | usMixNumber =                             |     | stacked.       |
WFS_CDM_INDIVIDUAL
bPresent = FALSE

Request USD10x2 from Cash Unit 2.

| 5.    |     | WFS_CMD_CDM_DISPENSE completes  |     |
| ----- | --- | ------------------------------- | --- |
WFS_SUCCESS
lpDenomination output records that
2x10USD were stacked.

6.    Call WFS_CMD_CDM_PRESENT  WFS_SRVE_CDM_SHUTTERSTATUS- Items
|     |     | CHANGED(WFS_CDM_SHTOPEN)  | presented.  |
| --- | --- | ------------------------- | ----------- |
event generated
| 7.    |     | WFS_CMD_CDM_PRESENT completes  |     |
| ----- | --- | ------------------------------ | --- |
WFS_SUCCESS
| 8.  User takes  |     | WFS_SRVE_CDM_ITEMSTAKEN event  |     |
| --------------- | --- | ------------------------------ | --- |
| bills.          |     | generated.                     |     |
WFS_SRVE_CDM_SHUTTERSTATUS-
CHANGED(WFS_CDM_SHTCLOSED)
event generated
|     |     |     |     |
| --- | --- | --- | --- |
116

CWA 16926-5:2020 (E)
Multiple Dispense Example ‘Multiple Currency with Vendor Mix’

In the following example WFSCDMCAPS.fwMoveItems reports WFS_CDM_TOSTACKER.
The CDM has 2 logical Cash Units with good supply of notes in each. Cash Unit 1 has 20CHF, Cash Unit 2 has
20EUR.

This table illustrates multiple WFS_CMD_CDM_DISPENSE commands to fulfill a dispense request of EUR and
CHF notes with vendor defined mix number. WFSCDMMIXTYPE.usMixNumber = 1.

| Step  End-User  | Application  | XFS SP  | CDM  |
| --------------- | ------------ | ------- | ---- |
Hardware
1.  User wants to  Application will split the dispense    100 EUR
dispense 100  stacking all money before a single  picked and
| EUR and 100  | present.  |     | stacked.  |
| ------------ | --------- | --- | --------- |
| CHF.         |           |     |           |
WFS_CMD_CDM_DISPENSE
usMixNumber = 1
bPresent = FALSE

Request 100 EUR.

| 2.    |     | WFS_CMD_CDM_DISPENSE  |     |
| ----- | --- | --------------------- | --- |
completes WFS_SUCCESS
lpDenomination output records that
5x20EUR were stacked.

| 3.    | WFS_CMD_CDM_DISPENSE  |     | 100 CHF     |
| ----- | --------------------- | --- | ----------- |
|       | usMixNumber =         |     | picked and  |
|       | WFS_CDM_INDIVIDUAL    |     | stacked.    |
bPresent = FALSE

Request 100 CHF.

| 4.    |     | WFS_CMD_CDM_DISPENSE  |     |
| ----- | --- | --------------------- | --- |
completes WFS_SUCCESS
lpDenomination output records that
5x20CHF were stacked.

5.    Call WFS_CMD_CDM_PRESENT  WFS_SRVE_CDM_SHUTTERSTATUS Items
|     |     | CHANGED(WFS_CDM_SHTOPEN)  | presented.  |
| --- | --- | ------------------------- | ----------- |
event generated
| 6.    |     | WFS_CMD_CDM_PRESENT completes  |     |
| ----- | --- | ------------------------------ | --- |
WFS_SUCCESS
| 7.  User takes  |     | WFS_SRVE_CDM_ITEMSTAKEN  |     |
| --------------- | --- | ------------------------ | --- |
| bills.          |     | event generated.         |     |
WFS_SRVE_CDM_SHUTTERSTATUS
CHANGED(WFS_CDM_SHTCLOSED)
event generated
|     |     |     |     |
| --- | --- | --- | --- |

117

CWA 16926-5:2020 (E)
