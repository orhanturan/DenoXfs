---
source: XfsChk.pdf
document: XfsChk
device_class: CHK
part: 14
section_id: ""
title: "2. Check Readers and Scanners"
type: section
source_lines: [282, 299]
---

<!-- 2. Check Readers and Scanners -->

2. Check Readers and Scanners
This specification describes the XFS service class of check readers and scanners. Check image scanners are treated
as a special case of check readers, i.e. image-enabled instances of the latter. This class includes devices with a range
of features, from small hand-held read-only devices through which checks are manually swiped one at a time, to
desktop units which automatically feed the check one at a time; recording the MICR data and check image, and
endorse or encode the check. The specification of this service class includes definitions of the service-specific
commands that can be issued, using the WFSAsyncExecute, WFSExecute, WFSGetInfo and WFSAsyncGetInfo
functions.
In the U.S., checks are always encoded in magnetic ink for reading by Magnetic Ink Character Recognition
(MICR), and a single font is always used. In Europe some countries use MICR and some use Optical Character
Recognition (OCR) character sets, with different fonts, for their checks.
In all countries, typical fields found encoded on a check include the bank ID number and the account number. Part
of the processing done by the bank is to also encode the amount on the check, usually done by having an operator
enter the handwritten or typewritten face amount on a numeric keypad.
This service class is currently defined only for attended branch service.
10

CWA 16926-7:2020 (E)
