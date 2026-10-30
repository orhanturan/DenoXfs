---
source: XfsApi.pdf
document: XfsApi
device_class: API/SPI
part: 1
section_id: "4"
title: "Architectural and Implementation Issues"
type: section
source_lines: [534, 575]
---

<!-- 4 Architectural and Implementation Issues -->

4 Architectural and Implementation Issues
The remainder of this document provides the technical specifications for the CEN eXtensions for Financial Services
(referred to hereafter as “XFS” for brevity).
In this specification, the functions of the XFS Application Programming Interface (API) and Service Provider
Interface (SPI) are always described in terms of providing a standardized, portable interface for applications to gain
access to Service Providers. This architecture allows Service Providers to deliver an open-ended set of capabilities
to financial applications based on the Microsoft Windows operating systems, including access to peripheral devices
unique to financial institutions. Since the first priority of the CEN members for XFS implementations has been to
provide this peripheral device access capability, the examples used relate primarily to device control and physical
input/output.
The key elements of the Extensions for Financial Services are the API definition and the corresponding SPI
definition, used by the XFS Manager to communicate with the Service Providers, together with the set of
supporting services provided by the XFS Manager. These elements are combined in an XFS implementation,
providing access to financial devices and services for Windows-based applications.
The specification defines a standard set of interfaces in order to provide multi-vendor interoperability: if an
application uses the API to communicate successfully with a Service Provider, it should work with another
conformant Service Provider of the same type, developed by another vendor, without any changes. To work with
more than one hardware implementation of a device, an application must retrieve the device capability information
- this will allow the application to successfully interact with different variants of the same hardware device.
Applications that use the vendor specific fields of XFS commands may not be able to interact successfully with
another vendor’s conformant Service Provider. Applications should isolate vendor specific access to devices in
order to maximize consistent device control across multiple device Service Provider implementations. Any Service
Provider that conforms to the SPI definition can work with a range of conformant applications.
As new versions of the XFS device classes are developed and released, changes to the device class interface
specifications are inevitable. Application exposure to these changes is controlled via the version negotiation process
described later in this specification. Applications need to be updated to support new releases of XFS, but to
minimize the migration effort it is recommended that they should be developed in such a way that they can handle
additional error codes and new output literal values being added to existing commands within future versions of
XFS in a graceful manner. In addition, applications must release the memory for all events received, this includes
events that the application may be unaware at development time, i.e. the minimum processing for any XFS event
must be the release of the memory associated with the event.
For clarity, three prefixes are used in naming the function interfaces in XFS:
Function type: Prefix Functions called by Functions provided by
• API functions: WFS... • Applications • XFS Manager (and typically
passed through to WFP
functions)
• SPI functions: WFP... • XFS Manager • Service Providers
• Support/Configuration functions: WFM... • Service Providers • XFS Manager
• Applications
19

CWA 16926-1:2025 (E)
