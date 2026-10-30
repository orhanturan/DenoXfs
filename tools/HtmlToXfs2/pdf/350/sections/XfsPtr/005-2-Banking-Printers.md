---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "2. Banking Printers"
type: section
source_lines: [322, 365]
---

<!-- 2. Banking Printers -->

2. Banking Printers
This specification describes the functionality of the services provided by banking printers and scanning devices
under XFS, focusing on the following areas:
• application programming for printing
• print document definition
• integration with the Windows architecture
• scanning images for devices such as check scanners
These descriptions include definitions of the service-specific commands that can be issued, using the
WFSAsyncExecute, WFSExecute, WFSGetInfo and WFSAsyncGetInfo functions.
The requirements for printing in banking applications are significantly different from those of the conventional PC
environment, and the XFS support delivers the foundation for financial application printing, including:
• Controlled access to shared printers
The banking printers can be shared between workstations and the XFS layer provides the ability for the
application to manage ownership of a print device. This allows an application to identify the operator
granted control of the printer, and to ensure that a teller printing multiple documents is not interrupted by
work for other applications.
• Application controlled printing
In the banking environment, it is necessary for the application to receive positive feedback on the
availability of print devices, and the success or failure of individual print operations. The XFS printer
support provides a standard mechanism for application retrieval of this status information.
• Management of printing peripherals
Distributed banking networks require the ability to track the availability and failure of printing
peripherals on a branch and system-wide basis. Through the XFS WFSRegister function monitoring
programs can collect error alerts from the banking printers.
• Vendor independent API and document definition
All of the XFS peripheral implementations are designed around a standardized family of APIs to allow
application code portability across vendor hardware platforms. With printers, it is also recognized that
banks invest a significant amount of resource in the authoring of print documents. The XFS printer
service class is implemented around a forms model which also standardizes the basic document
definition. This extends the investment protection provided by XFS compliant systems to include this
additional part of the application development.
• Windows printing integration
It is possible for a banking printer to offer printing capabilities that can be accessed by non-banking
specific applications, such as general office productivity packages. This would not, for example, be true
for a receipt printer, but it could be the case for a device with document printing capabilities. A vendor
may choose an XFS implementation that allows both types of applications (XFS and Windows
applications using the Windows printing subsystem) to share the printing devices. The vendor should
specify any impact this approach has on XFS subsystem operation, such as error reporting.
Full implementation of the above features depends on the individual vendor-supplied Service Providers. This
specification outlines the functionality and requirements for applications using the XFS printer and scanning
services, and for the development of those services.
11

CWA 16926-3:2022 (E)
