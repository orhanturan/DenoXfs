---
source: XfsApi.pdf
document: XfsApi
device_class: API/SPI
part: 1
section_id: ""
title: "WFS_SYSE_USER_ERROR and WFS_SYSE_FRAUD_ATTEMPT"
type: api_function
source_lines: [4294, 4396]
---

<!-- WFS_SYSE_USER_ERROR and WFS_SYSE_FRAUD_ATTEMPT -->

WFS_SYSE_USER_ERROR and WFS_SYSE_FRAUD_ATTEMPT
Hardware and software errors are reported as system events. In most cases, this is in addition to being reported via
the WFS_ERR_HARDWARE_ERROR (or device class specific error code), the
WFS_ERR_SOFTWARE_ERROR or WFS_ERR_USER_ERROR error code that is returned as the command
completion.
In order to supply the maximum information, these events should be sent as soon as an error is detected. In
particular, if an error is detected during the processing of an execute command, then the event should be sent before
the command completion event.
The WFSRESULT data structure (defined in Section 9.1), is utilized as follows:
Field Description
RequestID Request ID of the request being processed when the error occurred, zero if no request was
being processed when the error occurred.
hService Service handle identifying the session that the event is sent to.
tsTimestamp Time the error occurred (local time, in a Win32/Win64 SYSTEMTIME structure).
hResult Result handle of the request being processed when the error occurred, zero if no request
was being processed.
u.dwEventID The ID of the error.
Value Meaning
WFS_SYSE_HARDWARE_ERROR The error is a hardware error
WFS_SYSE_SOFTWARE_ERROR The error is a software error
WFS_SYSE_USER_ERROR The error is a user error
WFS_SYSE_FRAUD_ATTEMPT Some devices are capable of identifying a
malicious physical attack which attempts to
defraud valuable information or media. In this
circumstance, this error event is returned to
indicate a fraud attempt has occurred.
lpBuffer Pointer to a WFSHWERROR structure:
typedef struct _wfs_hwerror {
LPSTR lpszLogicalName;
LPSTR lpszPhysicalName;
LPSTR lpszWorkstationName;
LPSTR lpszAppID;
DWORD dwAction;
DWORD dwSize;
LPBYTE lpbDescription;
} WFSHWERROR, *LPWFSHWERROR;
The members of this structure are:
Field Description
lpszLogicalName Pointer to the logical service name of the service that generated the error
lpszPhysicalName Pointer to the physical service name of the service that generated the error
lpszWorkstationName Pointer to the name of the workstation in which the logical service name is defined (if any)
lpszAppID Pointer to the application ID associated with the session that generated the error (if any)
dwAction The action required to manage the error. Possible values are:
Value Meaning
WFS_ERR_ACT_NOACTION No action required or error was autorecovered.
WFS_ERR_ACT_RESET Reset device to attempt recovery using
WFS_CMD_XXX_RESET, but should not be
used excessively. Intervention is not required
although if repeated attempts are unsuccessful
then WFS_ERR_ACT_HWMAINT may be
reported.
WFS_ERR_ACT_SWERROR A software error occurred. Contact software
vendor.
129

CWA 16926-1:2020 (E)
|     | WFS_ERR_ACT_CONFIG  |     | A configuration error occurred. Check  |     |
| --- | ------------------- | --- | -------------------------------------- | --- |
configuration.
WFS_ERR_ACT_HWCLEAR  Recovery is not possible. A manual intervention
for clearing the device is required. This value is
only used for hardware errors. This value is
typically returned when a hardware error has
occurred which requires banking personnel
specific maintenance, e.g. ‘replace paper’, or
‘remove cards from retain bin’.
|     | WFS_ERR_ACT_HWMAINT  |     | Recovery is not possible. A technical  |     |
| --- | -------------------- | --- | -------------------------------------- | --- |
maintenance intervention is required. This value
is only used for hardware errors and fraud
attempts. This value is typically returned when a
hardware error or fraud attempt has occurred
which requires field engineer specific
maintenance activity.
WFS_CMD_XXX_RESET may be used to
attempt recovery after intervention, but should
not be used excessively – Vendor Dependent
Mode may be required to recover the device.
WFS_ERR_ACT_SUSPEND  Device will attempt auto recovery and will
advise any further action required via a Device
Status Event.
dwSize  The size in bytes of the following description
lpbDescription  Pointer to a vendor-specific description of the error.

Note:
The following table describes what dwAction may be returned for the various Hardware, Software, User Error and
Fraud Attempt Events. The dwAction definitions above give guidance on what an application should do next when
one of these events is received. Care should be taken to avoid calling WFS_CMD_XXX_RESET excessively
without intervention, as this may lead to damage to the device or media contained in the device if for example
media is jammed in the device:
  Generated on  Generated on Software  Generated on User  Generated on Fraud
|            | Hardware Event?  | Event?  | Event?  | Event?  |
| ---------- | ---------------- | ------- | ------- | ------- |
| _NOACTION  | Yes              | Yes     | Yes     | Yes     |
| _RESET     | Yes              | Yes     | Yes     | No      |
| _SWERROR   | No               | Yes     | No      | No      |
| _CONFIG    | Yes              | Yes     | No      | No      |
| _HWCLEAR   | Yes              | No      | No      | No      |
| _HWMAINT   | Yes              | No      | No      | Yes     |
| _SUSPEND   | No               | No      | Yes     | No      |
130

CWA 16926-1:2020 (E)
