---
source: XfsVdm.pdf
document: XfsVdm
device_class: VDM
part: 12
section_id: "2"
title: "Vendor Dependent Mode"
type: section
source_lines: [464, 523]
---

<!-- 2 Vendor Dependent Mode -->

2.  Vendor Dependent Mode

This specification describes the functionality of the services provided by the Vendor Dependent Mode (VDM)
Service Provider under XFS, by defining the service-specific commands that can be issued, using the WFSGetInfo,
WFSAsyncGetInfo, WFSExecute and WFSAsyncExecute functions.

In all device classes there needs to be some method of going into a vendor specific mode to allow for capabilities
which go beyond the scope of the current XFS specifications. A typical usage of such a mode might be to handle
some configuration or diagnostic type of function or perhaps perform some 'off-line' testing of the device. These
functions are normally available on Self-Service devices in a mode traditionally referred to as Maintenance Mode
or Supervisor Mode and usually require operator intervention. It is those vendor-specific functions not covered by
(and not required to be covered by) XFS Service Providers that will be available once the device is in Vendor
Dependent Mode.

This service provides the mechanism for switching to and from Vendor Dependent Mode. The VDM Service
Provider can be seen as the central point through which all Enter and Exit VDM requests are synchronized.

Entry into, or exit from, Vendor Dependent Mode can be initiated either by an application or by the VDM Service
Provider itself. If initiated by an application, then this application needs to issue the appropriate command to
request entry or exit. If initiated by the VDM Service Provider i.e. some vendor dependent switch, then these
request commands are in-appropriate and not issued.

Once the entry request has been made, all registered applications will be notified of the entry request by an event
message. These applications must attempt to close all open sessions with XFS Service Providers (except for
specific Service Providers which explicitly allow sessions to remain open) as soon as possible and then issue an
acknowledgement command to the VDM Service Provider when ready. Once all applications have acknowledged,
the VDM Service Provider will issue event messages to these applications to indicate that the System is in Vendor
Dependent Mode.

Similarly, once the exit request has been made all registered applications will be notified of the exit request by an
event message. These applications must then issue an acknowledgement command to the VDM Service Provider
immediately. Once all applications have acknowledged, the VDM Service Provider will issue event messages to
these applications to indicate that the system has exited from Vendor Dependent Mode.

Thus, XFS compliant applications that do not request entry to Vendor Dependent Mode, must comply with the
following:

•  Every XFS application should open a session with the VDM Service Provider passing a valid ApplId and

then register for all VDM entry and exit notices.

•  Before opening a session with any other XFS Service Provider, check the status of the VDM Service

Provider. If Vendor Dependent Mode is not “Inactive”, do not open a session.

•  When getting a VDM entry notice, close all open sessions with all XFS Service Providers which require

sessions to be closed as soon as possible and issue an acknowledgement for the entry to VDM.

•  When getting a VDM exit notice, acknowledge at once.

•  When getting a VDM exited notice, re-open any required sessions with other XFS Service Providers.

This is mandatory for self-service but optional for branch.

9


CWA 16926-11:2020 (E)
