---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "6. Command Overview"
type: section
source_lines: [460, 497]
---

<!-- 6. Command Overview -->

6. Command Overview
The basic operation of the print devices is managed using the WFSGetInfo/WFSAsyncGetInfo and
WFSExecute/WFSAsyncExecute functions, with two primary commands:
WFS_INF_PTR_QUERY_FORM
This command retrieves the form header information, and the list of fields. It is performed using
WFSGetInfo, which means that it can be performed even when the service is locked by another user.
WFS_CMD_PTR_PRINT_FORM
This command is performed using WFSExecute, and includes as parameter data the name of the form to
select and the required field data values.
This approach combines in the most efficient manner the four logical steps required to print a form:
• Selecting a document form object.
• Querying the service for the list of fields.
• Supplying the data for each field.
• Issuing the print command.
By using a WFSGetInfo command for retrieval of the list of field names, rather than WFSExecute (which is
blocked when the service is locked by another application), it is possible for an application to assemble the required
set of fields for a form before locking the service. This minimizes the time that each application request ties up the
service. Using WFSGetInfo, it is also possible to query the attributes of a particular field. This command is
generally not required for most applications.
The combination of form selection, field value presentation, and the print action into an atomic command - the
WFSExecute: WFS_CMD_PTR_PRINT_FORM command - makes it possible to express a complete print
operation with one API call. This implementation allows an application to perform a print operation without
locking and subsequently unlocking the service (although locking may still be desirable for other reasons). To do
multiple print operations without allowing other applications to intersperse their print requests, it is still necessary
to use the lock functions. Where these multiple print functions represent a series of passbook lines (using the
INDEX capability in the field definition), the WFSExecute: WFS_CMD_PTR_PRINT_FORM command provides
support for management of the print line number. Note that if a form contains a tabular field (i.e. one with a non-
zero INDEX value), and data is not supplied for some of the lines in the “table”, then those lines are left blank.
For printers with the capability to read from a passbook (OCR, MICR and/or magnetic stripe), the data is read with
the WFSExecute: WFS_CMD_PTR_READ_FORM command. The data is written using the WFSExecute:
WFS_CMD_PTR_PRINT_FORM command. Since these devices are usable only for passbook operations, they are
not defined as separate logical devices.
Finally, the WFSExecute: WFS_CMD_PTR_PRINT_RAW_FILE command can be used to print a file that
contains a complete print job in the native printer language. This file will have been created through the Windows
GDI.
15

CWA 16926-3:2020 (E)
