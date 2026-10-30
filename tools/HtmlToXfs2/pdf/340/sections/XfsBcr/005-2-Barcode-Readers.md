---
source: XfsBcr.pdf
document: XfsBcr
device_class: BCR
part: 4
section_id: ""
title: "2. Barcode Readers"
type: section
source_lines: [261, 275]
---

<!-- 2. Barcode Readers -->

2. Barcode Readers
This specification describes the functionality of a Barcode Reader (BCR) Service Provider. It defines the service-
specific commands that can be issued to the Service Provider using the WFSGetInfo, WFSAsyncGetInfo,
WFSExecute and WFSAsyncExecute functions.
Persistent values are maintained through power failures, open sessions, close session and system resets.
This extension to XFS specifications defines the functionality of BCR service.
A Barcode Reader scans barcodes using any scanning technology. The device logic converts light signals or image
recognition into application data and transmits it to the host system.
The basic operation of the Barcode Reader is managed using WFSExecute/WFSAsyncExecute functions.
When an application wants to read a barcode, it issues a WFS_CMD_BCR_READ command to prepare the scanner
to read any barcode presented to it. When a document is presented to the BCR and a barcode type is recognized, a
completion event is received which contains the barcode data that has been read.
9

CWA 16926-17:2020 (E)
