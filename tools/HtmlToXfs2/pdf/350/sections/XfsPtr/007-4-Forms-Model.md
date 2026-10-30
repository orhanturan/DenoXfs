---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "4. Forms Model"
type: section
source_lines: [408, 449]
---

<!-- 4. Forms Model -->

4. Forms Model
The XFS printing class functionality is based on a “forms” model for printing. Banking documents are represented
as a series of text and/or graphic fields output from the application, and positioned on the document by the XFS
printing system.
The form is an object which includes the positioning and presentation information for each of the fields in the
document. The application selects a form, and supplies only the field data and the control parameters to fully define
the print document.
The form objects are owned and managed by the XFS printing service. To optimize maintainability of the system,
the application can query the service for the list of fields required to print a given form. Through this mechanism, it
is not necessary to duplicate the field contents of forms in application authoring data. The figure below outlines the
printing process from the application's view.
Query Form Info
List of Field Names
Application XFS Print
Print Command w/
List of Field Values
Completion Status
The XFS implementation recognizes that the form object must be supported by job-specific data to fully address
printing requirements. As an example, a form defining a passbook print line will need to have its origin defined
externally in order to be reused for different passbook lines. These job specific parameters are supplied on the call
to the WFSExecute: WFS_CMD_PTR_PRINT_FORM command.
In some cases, the application wants to print a block of data without considering it as a series of separate fields. One
example is a line of journal data, fully formatted by the application. This can be handled by defining a one field
form, or by use of the WFSExecute: WFS_CMD_PTR_RAW_DATA command.
The document definition under XFS printing is standardized to provide portability across vendor implementations.
The standard has been defined at the source language level for the document definition, allowing vendor differences
at the runtime level to manage implementation specific dependencies, providing several areas where vendors can
provide value-added extensions. As an example, a vendor providing a graphical form definition tool can produce
the field definition object format directly. The XFS requirements for portability are:
• A vendor must be able to export print format in the standardized field definition source format for
portability to other systems.
• A vendor must be able to import document formats produced on other systems in the standardized field
definition source format.
• A vendor can extend the field definition source language, but any verbs included in the standard must be
implemented strictly as defined by the standard. Import and export facilities must be tolerant of source
language extensions, reporting but ignoring the exceptions.
The document definition also recognizes that unique hardware restrictions may require tuning of field positioning
from one vendor's platform to another. To enhance portability, the XFS document format has specifically been
defined to allow a single reference adjustment for all fields to avoid forcing the customer to reposition each field.
13

CWA 16926-3:2022 (E)
