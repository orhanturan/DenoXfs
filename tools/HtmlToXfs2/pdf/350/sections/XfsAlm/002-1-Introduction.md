---
source: XfsAlm.pdf
document: XfsAlm
device_class: ALM
part: 3
section_id: "1"
title: "Introduction"
type: section
source_lines: [379, 633]
---

<!-- 1 Introduction -->

1.  Introduction

1.1

Background to Release 3.50

The CEN/XFS Workshop aims to promote a clear and unambiguous specification defining a multi-vendor software
interface to financial peripheral devices. The XFS (eXtensions for Financial Services) specifications are developed
within the CEN (European Committee for Standardization/Information Society Standardization System) Workshop
environment. CEN Workshops aim to arrive at a European consensus on an issue that can be published as a CEN
Workshop Agreement (CWA).

The CEN/XFS Workshop encourages the participation of both banks and vendors in the deliberations required to
create an industry standard. The CEN/XFS Workshop achieves its goals by focused sub-groups working
electronically and meeting quarterly.

Release 3.50 of the XFS specification is based on a C API and is delivered with the continued promise for the
protection of technical investment for existing applications. This release of the specification extends the
functionality and capabilities of the existing devices covered by the specification:

•  Addition of E2E security

•  PIN Password Entry.

1.2

XFS Service-Specific Programming

The service classes are defined by their service-specific commands and the associated data structures, error codes,
messages, etc. These commands are used to request functions that are specific to one or more classes of Service
Providers, but not all of them, and therefore are not included in the common API for basic or administration
functions.

When a service-specific command is common among two or more classes of Service Providers, the syntax of the
command is as similar as possible across all services, since a major objective of XFS is to standardize function
codes and structures for the broadest variety of services. For example, using the WFSExecute function, the
commands to read data from various services are as similar as possible to each other in their syntax and data
structures.

In general, the specific command set for a service class is defined as a superset of the specific capabilities likely to
be provided by the developers of the services of that class; thus any particular device will normally support only a
subset of the defined command set.

There are three cases in which a Service Provider may receive a service-specific command that it does not support:

The requested capability is defined for the class of Service Providers by the XFS specification, the particular vendor
implementation of that service does not support it, and the unsupported capability is not considered to be
fundamental to the service. In this case, the Service Provider returns a successful completion, but does no operation.
An example would be a request from an application to turn on a control indicator on a passbook printer; the Service
Provider recognizes the command, but since the passbook printer it is managing does not include that indicator, the
Service Provider does no operation and returns a successful completion to the application.

The requested capability is defined for the class of Service Providers by the XFS specification, the particular vendor
implementation of that service does not support it, and the unsupported capability is considered to be fundamental
to the service. In this case, a WFS_ERR_UNSUPP_COMMAND error for Execute commands or
WFS_ERR_UNSUPP_CATEGORY error for Info commands is returned to the calling application. An example
would be a request from an application to a cash dispenser to retract items where the dispenser hardware does not
have that capability; the Service Provider recognizes the command but, since the cash dispenser it is managing is
unable to fulfil the request, returns this error.

The requested capability is not defined for the class of Service Providers by the XFS specification. In this case, a
WFS_ERR_INVALID_COMMAND error for Execute commands or WFS_ERR_INVALID_CATEGORY error
for Info commands is returned to the calling application.

This design allows implementation of applications that can be used with a range of services that provide differing
subsets of the functionalities that are defined for their service class. Applications may use the WFSGetInfo and
WFSAsyncGetInfo commands to inquire about the capabilities of the service they are about to use, and modify
their behavior accordingly, or they may use functions and then deal with error returns to make decisions as to how

7


CWA 16926-13:2022 (E)

to use the service.

8


CWA 16926-13:2022 (E)

2.  Alarms

This specification describes the functionality of the services provided by Alarms (ALM) under XFS, by defining
the service-specific commands that can be issued, using the WFSGetInfo, WFSAsyncGetInfo, WFSExecute and
WFSAsyncExecute functions. This section describes the functionality of an Alarm (ALM) service that applies to
both attended and unattended (self-service) devices.

The Alarm device class is provided as a separate service due to the need to set or reset an Alarm when one or more
logical services associated with an attended CDM or unattended (self-service) device are locked. Because logical
services can be locked by the application the Alarm is implemented in a separate device class to ensure that a set
(trigger) or reset operation can be performed at any time.

9


CWA 16926-13:2022 (E)

3.  References

1. XFS Application Programming Interface (API)/Service Provider Interface (SPI), Programmer’s Reference
Revision 3.50

10


CWA 16926-13:2022 (E)

4.  Info Commands

4.1

WFS_INF_ALM_STATUS

Description

This command is used to request the Alarm status.

Input Param  None.

Output Param  LPWFSALMSTATUS lpStatus;

typedef struct _wfs_alm_status

{
WORD
BOOL
LPSTR
WORD
} WFSALMSTATUS, *LPWFSALMSTATUS;

fwDevice;
bAlarmSet;
lpszExtra;
wAntiFraudModule;

fwDevice
Specifies the state of the alarm device as one of the following flags:

Value
WFS_ALM_DEVONLINE

WFS_ALM_DEVOFFLINE

WFS_ALM_DEVPOWEROFF

WFS_ALM_DEVNODEVICE

WFS_ALM_DEVUSERERROR

WFS_ALM_DEVHWERROR

WFS_ALM_DEVBUSY

WFS_ALM_DEVFRAUDATTEMPT

WFS_ALM_DEVPOTENTIALFRAUD

Meaning
The device is present, powered on and online
(i.e. operational, not busy processing a
request and not in an error state).
The device is offline (e.g. the operator has
taken the device offline by turning a switch).
The device is powered off or physically not
connected.
There is no device intended to be there; e.g.
this type of self service machine does not
contain such a device or it is internally not
configured.
The device is present but a person is
preventing proper device operation. The
application should suspend the device
operation or remove the device from service
until the Service Provider generates a device
state change event indicating the condition
of the device has changed e.g. the error is
removed (WFS_ALM_DEVONLINE) or a
permanent error condition has occurred
(WFS_ALM_DEVHWERROR).
The device is present but inoperable due to a
hardware fault that prevents it from being
used.
The device is busy and unable to process an
execute command at this time.
The device is present but is inoperable
because it has detected a fraud attempt.
The device has detected a potential fraud
attempt and is capable of remaining in
service. In this case the application should
make the decision as to whether to take the
device offline.

bAlarmSet
Specifies the state of the Alarm as either Reset (FALSE) or Set (TRUE).

11


CWA 16926-13:2022 (E)

lpszExtra
Pointer to a list of vendor-specific, or any other extended, information. The information is
returned as a series of “key=value” strings so that it is easily extensible by Service Providers.
Each string is null-terminated, with the final string terminating with two null characters. An
empty list may be indicated by either a NULL pointer or a pointer to two consecutive null
characters.

wAntiFraudModule
Specifies the state of the anti-fraud module as one of the following values:

Value
WFS_ALM_AFMNOTSUPP
WFS_ALM_AFMOK

WFS_ALM_AFMINOP
WFS_ALM_AFMDEVICEDETECTED

WFS_ALM_AFMUNKNOWN

Meaning
No anti-fraud module is available.
Anti-fraud module is in a good state and no
foreign device is detected.
Anti-fraud module is inoperable.
Anti-fraud module detected the presence of a
foreign device.
The state of the anti-fraud module cannot be
determined.

Error Codes

Only the generic error codes defined in [Ref. 1] can be generated by this command.

Comments

Applications which require or expect specific information to be present in the lpszExtra parameter
may not be device or vendor-independent.

In the case where communications with the device has been lost, the fwDevice field will report
WFS_ALM_DEVPOWEROFF when the device has been removed or
WFS_ALM_DEVHWERROR if the communications are unexpectedly lost. All other fields
should contain a value based on the following rules and priority:

1.  Report the value as unknown.

2.  Report the value as a general h/w error.

3.  Report the value as the last known value.

12


CWA 16926-13:2022 (E)
