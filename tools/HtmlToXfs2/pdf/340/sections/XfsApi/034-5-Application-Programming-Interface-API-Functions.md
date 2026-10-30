---
source: XfsApi.pdf
document: XfsApi
device_class: API/SPI
part: 1
section_id: "5"
title: "Application Programming Interface (API) Functions"
type: section
source_lines: [1462, 1485]
---

<!-- 5 Application Programming Interface (API) Functions -->

5 Application Programming Interface (API) Functions
The functions defined by the XFS API are divided into:
• Basic functions that are common to all classes of financial services.
• Administration functions, used for the special purpose of administering services.
• Service-specific commands that are peculiar to a single service class or a group of them and that are sent to
services using basic functions (WFSExecute, WFSAsyncExecute, WFSGetInfo, WFSAsyncGetInfo).
The benefit of grouping functions that are common to all services is evident: programmers can immediately focus
on those operations that are common through all services and thus can easily build a high level model of interaction
with the Service Providers.
The basic functions are defined in this section, in alphabetical order, except that the asynchronous version of each
command is described immediately following the synchronous version. For example, WFSAsyncExecute is placed
immediately following WFSExecute. The table on the next page lists all the basic functions. This set of basic
functions may be expanded in future releases of this specification, if new functions are determined to be useful for
all Service Providers.
The administration functions have not yet been fully defined; they are outlined in Appendix A. - Planned
Enhancements and Extensions.
The service-specific commands are defined in separate specifications-one for each service class. In addition, the
XFS SNMP MIB architecture specification defines a number of category codes that are common across all service
classes.
45

CWA 16926-1:2020 (E)
The table below summarizes the XFS API functions, and the sections in which they are defined.
Section Function Mode Description
