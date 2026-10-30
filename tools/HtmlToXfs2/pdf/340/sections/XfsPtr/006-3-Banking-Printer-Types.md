---
source: XfsPtr.pdf
document: XfsPtr
device_class: PTR
part: 9
section_id: ""
title: "3. Banking Printer Types"
type: section
source_lines: [370, 411]
---

<!-- 3. Banking Printer Types -->

3. Banking Printer Types
The XFS printer service defines and supports five types of banking printers through a common interface:
• Receipt Printer
The receipt printer is used to print cut sheet documents. It may or may not require insert or eject
operations, and often includes an operator identification device, e.g. Teller A and Teller B lights, for
shared operation.
• Journal Printer
The journal is a continuous form device used to record a hardcopy audit trail of transactions, and for
certain report printing requirements.
• Passbook Printer
The passbook device is physically and functionally the most complex printer. The XFS definition
supports automatic positioning of the book, as well as read/write capability for an optional integrated
magnetic stripe. The implementation also manages the book geometry - i.e. the margins and centerfolds -
presenting the simplest possible application interface while delivering the full range of functionality.
Some passbook devices also support the dispensing of new passbooks from up to four passbook paper
sources (upper, aux, aux2, lower). Some passbook devices may also be able to place a full passbook in a
parking station, print the new passbook and return both to the customer. Passbooks can only be dispensed
or moved from the parking station if there is no other media in the print position or in the entry/exit slot.
• Document Printer
Document printing is similar to receipt printing - a set of fields are positioned on one or more inserted
sheets of paper - but the focus is on full-size forms. It should be noted that the XFS environment supports
the printing of text and graphic fields from the application. The electronic printing of the form image (the
template portion of the form which is usually pre-printed with dot-matrix style printers) may also be
printed by the application.
• Scanner Printer
The scanner printer is a device incorporating both the capabilities to scan inserted documents and
optionally to print on them. These devices may have more than one area where documents may be
retained.
Additional hardware components, like scanners, stripe readers, OCR readers, and stamps, normally attached
directly to the printer are also controlled through this interface. Additionally the Printer and Scanning class
interface can also be used for devices that are capable of scanning without necessarily printing. This includes
devices such as Check Scanners.
The specification refers to the terms paper and media. When the term paper is used this refers to paper that is
situated in a paper supply attached to the device. The term media is used for media that is inserted by the customer
(e.g. check and other material that is scanned) or that is issued to the customer (e.g. a receipt or statement). Receipt,
document printers and also passbook printers with white passbook dispensing capability have both. As soon as the
paper gets printed it becomes media. Scanners only have media. The term media does not apply to journal printers.
When paper is in the print position it is classified as media, on some printers that maintain paper under the print
head there will always be both media and paper.
12

CWA 16926-3:2020 (E)
