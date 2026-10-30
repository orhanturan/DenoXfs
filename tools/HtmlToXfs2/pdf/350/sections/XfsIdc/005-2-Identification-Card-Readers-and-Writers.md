---
source: XfsIdc.pdf
document: XfsIdc
device_class: IDC
part: 8
section_id: ""
title: "2. Identification Card Readers and Writers"
type: section
source_lines: [298, 351]
---

<!-- 2. Identification Card Readers and Writers -->

2. Identification Card Readers and Writers
This section describes the functions provided by a generic identification card reader/writer service (IDC). These
descriptions include definitions of the service-specific commands that can be issued, using the WFSAsyncExecute,
WFSExecute, WFSGetInfo and WFSAsyncGetInfo functions.
This service allows for the operation of the following categories of units:
• motor driven card reader/writer
• pull through card reader (writing facilities only partially included)
• dip reader
• contactless chip card readers
• permanent chip card readers (each chip is accessed through a unique logical service)
Some motor driven card reader/writers have parking stations inside and can place identification cards there. Once a
card is in its parking station another card can be accepted by the card reader. Cards may only be moved out of a
parking station if there is no other card present in the media read/write position, the chip I/O position, the transport,
or the entry/exit slot.
The following tracks/chips and the corresponding international standards are taken into account in this document:
• Track 1 ISO 7811
• Track 2 ISO 7811
• Track 3 ISO 7811 / ISO 4909
• Cash Transfer Card Track 1 (JIS I: 8 bits/char) Japan
• Cash Transfer Card Track 3 (JIS I: 8 bits/char) Japan
• Front Track 1 (JIS II) Japan
• Watermark Sweden
• Chip (contacted) ISO 7816
• Chip (contactless) ISO 10536, ISO 14443 and ISO 18092
National standards like Transac for France are not considered, but can be easily included via the forms mechanism
(see Section 7, Form Definition).
In addition to the pure reading of the tracks mentioned above, security boxes can be used via this service to check
the data of writable tracks for manipulation. These boxes (such as CIM or MM) are sensor-equipped devices that
are able to check some other information on the card and compare it with the track data.
Persistent values are maintained through power failures, open sessions, close session and system resets.
When the service controls a permanently connected chip card, WFS_ERR_UNSUPP_COMMAND will be returned
to all commands except WFS_INF_IDC_STATUS, WFS_INF_IDC_CAPABILITIES,
WFS_CMD_IDC_CHIP_POWER, WFS_CMD_IDC_CHIP_IO and WFS_CMD_IDC_RESET.
The following defines the roles and responsibilities of an application within EMV: A distinction needs to be made
between EMV Contact support and EMV Contactless support.
When defining an EMV Contact implementation
• EMV Level 2 interaction is handled above the XFS API
• EMV Level 1 interaction is handled below the XFS API
All EMV status information that is defined as a Level 1 responsibility in the EMV specification should be handled
below the XFS API.
EMVCo grants EMV Level 1 Approvals to contact IFMs and EMVCo Level 2 Approvals to Application Kernels.
When defining an EMV Contactless implementation
The responsibilities will depend on the type of EMV Contactless Product being implemented.
10

CWA 16926-4:2022 (E)
There are different EMVCo defined product types, they can be found in the EMVCo Type Approval – Contactless
Product – Administrative Process document.
• In this specification when referring to the Contactless Product Type – Intelligent Card Reader :
The following must be included and handled below the XFS API:
• An EMVCo Approved Level 1 Contactless PCD
• Entry Point and POS System Architecture according to Book A and B
• EMV Kernels according to Book C1 to C7 (minimum one kernel needs to be supported)
The Network Interface & the Consumer, Merchant Interfaces will be managed above the XFS API.
