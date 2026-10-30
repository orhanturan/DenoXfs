---
source: XfsIdc.pdf
document: XfsIdc
device_class: IDC
part: 8
section_id: ""
title: "7. Form Description"
type: section
source_lines: [3046, 3193]
---

<!-- 7. Form Description -->

7. Form Description
This section describes the forms mechanism used to define the tracks to be read or written. Forms are contained in a
single file, with one section for each defined form. The name of each section is the form name parameter in the
WFS_CMD_IDC_READ_TRACK and WFS_CMD_IDC_WRITE_TRACK commands.
The way to specify the location of a form file is vendor dependent.
As an example the following registry information can be used:
WOSA/XFS_ROOT
FORMS
IDCU
formfile=<path><filename>
The read form defines which tracks should be read in the WFS_CMD_IDC_READ_TRACK command and what
the response should be to a read failure. The read form can also be used to define logical track data, i.e. fields like
“account number”, “issuer identifier”, and their position within the physical track data. For example, the output
parameter of the WFS_CMD_IDC_READ_TRACK command with input parameter lpstrFormName =
READTRACK3GERMAN could look like (see example 1 below):
"TRACK3:MII=59\0COUNTRY=280\0ISSUERID=50050500\0ACCOUNT=1234567890\0LUHNT3=1\0EXPIR
ATION=9912\0SECURE=1\0\0\0"
The write form defines which track is to be written, the logical track data that is handed over in the
WFS_CMD_IDC_WRITE_TRACK command, and how the write data is to be converted to the physical data to be
written.
Reserved Keywords/Operands 1 Meaning
[] Form name delimiters.
TRACK1 Keyword to identify track 1.
TRACK2 Keyword to identify track 2.
TRACK3 Keyword to identify track 3.
TRACK1_JIS1 Keyword to identify JIS I track 1.
TRACK3_JIS1 Keyword to identify JIS I track 3.
FRONTTRACK1 Keyword to identify front track 1 (in some
countries this track is known as JIS II track).
FIELDSEPT1 Value of field separator of track 1.
FIELDSEPT2 Value of field separator of track 2.
FIELDSEPT3 Value of field separator of track 3.
FIELDSEPTFRONT1 Value of field separator of front track 1.
FIELDSEPT1_JIS1 Value of field separator of JIS I track 1.
FIELDSEPT3_JIS1 Value of field separator of JIS I track 3.
READ Description of read action; the track
identifier keywords are processed left to
right.
WRITE Description of write action.
ALL Read or write the complete track.
SECURE Do the security check via the security
module (CIM86 or MM). This check is done
on Track 3 only.
& Read/write all tracks specified, abort reading
on read failure.
| Read/write at least one of the tracks
specified, continue reading on read failure.
1 Attributes are not required in any mandatory order.
79

CWA 16926-4:2022 (E)
FIELDSEPPOSn Position of the nth occurrence of field
separator on track. FIELDSEPPOS0
specifies the beginning of the data.
, Separator in a list of logical fields.
DEFAULT String for default substitution of track data to
be written, that is not defined explicitly by
the form fields. DEFAULT also allows an
application to input fewer fields than those
defined by the form.
? Reserved value for DEFAULT keyword:
substitute track data to write with its value
read before.
ENDTRACK Represents the end of the data. It is used to
identify fields positioned after the last field
separator.
Notes
The & and | operands may be combined in a single READ statement; for example:
• read track3 or track2, trying track3 first:
READ= TRACK3 | TRACK2
• read track 3 and at least one of track2 or track1:
READ= TRACK3 & (TRACK2 | TRACK1)
or:
READ= TRACK2 | TRACK1 & TRACK3
The keywords FIELDSEPPOS0 and ENDTRACK are used as follows:
• read the first 2 bytes of a track:
FIRST= FIELDSEPPOS0 + 1, FIELDSEPPOS0 + 2
• read the last 2 bytes of a track:
LAST= ENDTRACK – 2, ENDTRACK – 1
Use of field separators in track layouts is to replace optional fields and terminate variable length
fields.
Write forms are designed for updating specific fields without altering the position of the field
separators.
The application may alter the position of the field separators by rewriting the card tracks (ALL
option or DEFAULT option with default track data).
It is valid to define a field that spans another field separator, e.g. FIELDSEPPOS1+1,
FIELDSEPPOS3+1 is valid as is FIELDSEPPOS3-4, FIELDSEPPOS3-1 where a field separator
(e.g. FIELDSEPPOS2) lies within this range on the data read from the card. During a read track
the field separator is returned within the track data. During a write track the application must
ensure the correct number of field separators at the correct location with the correct spacing is
included in the data, otherwise a WFS_ERR_IDC_DATASYNTAX error will be returned.
Example 1 Reading tracks:
[READTRACK3GERMAN]
FIELDSEPT1= = /* field separator of track 1 */
FIELDSEPT2= = /* field separator of track 2 */
FIELDSEPT3= = /* field separator of track 3 */
READ= TRACK3 & TRACK1 & TRACK2 /* all tracks must be read */
/* read logical fields as defined below; also check the security */
TRACK3= MII, COUNTRY, ISSUERID, ACCOUNT, LUHNT3, EXPIRATION, SECURE
MII= FIELDSEPPOS0 + 3, FIELDSEPPOS0 + 4
ISSUERID= FIELDSEPPOS0 + 5, FIELDSEPPOS1 - 1
ACCOUNT= FIELDSEPPOS1 + 1, FIELDSEPPOS2 - 2
LUHNT3= FIELDSEPPOS2 - 1, FIELDSEPPOS2 – 1
TRACK2= ALL /* return track 2 complete, don’t return logical fields*/
TRACK1= ALL /* return track 1 complete, don’t return logical fields*/
80

CWA 16926-4:2022 (E)
All tracks must be read (‘READ’), that is, the read fails if an error occurs on reading any one of
the tracks (the ‘&’ operand). The field “major industry identifier” (‘MII’) is located after the first
field separator (‘FIELDSEPPOS1’) and its length is two bytes. The “issuer identifier” field
(‘ISSUERID’) is located after the MII field, with a length of eight bytes. The next field, “account
number” (‘ACCOUNT’) is variable length; it ends before the luhn digit field (‘LUHNT3’) that is
the last digit in front of the second field separator (‘FIELDSEPPOS2’).
Example 2 Write a track:
[WRITETRACK3]
FIELDSEPT3= =
DEFAULT= ? /* fields not specified in the write form are to be left
unchanged, i.e. read and the same data written back to them */
WRITE= TRACK3
TRACK3= RETRYCOUNT, DATE
RETRYCOUNT= FIELDSEPPOS2 + 22, FIELDSEPPOS2 + 22
DATE= FIELDSEPPOS5 + 1, FIELDSEPPOS5 + 4
Track 3 is to be written. In the example only the retry counter and the date of the last transaction
are updated, the other fields are unchanged.
A sample of input data to be used with this form is as follows:
RETRYCOUNT=3\0DATE=3132\0\0
Example 3 Write a track:
[WRITETRACK3ALL]
WRITE= TRACK3
TRACK3= ALL
Track 3 is to be written. By specifying ALL, the data passed in the
WFS_CMD_IDC_WRITE_TRACK command is written to the physical track without formatting.
A sample of input data to be used with this form is as follows:
ALL=123456789123\0\0
Example 4 Reading tracks:
[READTRACK2ANDFRONTTRACK1]
READ= TRACK2&FRONTTRACK1 /* track 2 and front track 1 must be read */
TRACK2= ALL
FRONTTRACK1= ALL
Track 2 and Front track 1 are to be read by the WFS_CMD_IDC_READ_TRACK command. By
specifying ‘&’, reading is aborted if either track fails to read. By specifying ‘ALL’ the physical
track is read to the output data without field level formatting.
A sample of output data produced with this form is as follows:
TRACK2:ALL=123456789123\0\0FRONTTRACK1:ALL=123456789123\0\0\0
81

CWA 16926-4:2022 (E)
