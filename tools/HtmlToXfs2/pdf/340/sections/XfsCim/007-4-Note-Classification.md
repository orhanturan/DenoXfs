---
source: XfsCim.pdf
document: XfsCim
device_class: CIM
part: 15
section_id: ""
title: "4. Note Classification"
type: section
source_lines: [433, 478]
---

<!-- 4. Note Classification -->

4. Note Classification
Notes are classified by the XFS CIM specification according to the following definitions:
1. Level 1 – Note not recognized.
2. Level 2 – Recognized counterfeit note.
3. Level 3 – Suspected counterfeit note.
4. Level 4 – Recognized note that is identified as genuine. This includes notes which are fit or unfit for
recycling.
This definition allows support for legislative note handling standards that may exist in various countries and
economic regions. Local requirements or device capability may dictate that notes are not classified as level 2 and
level 3; the P6 string reported by WFS_INF_CIM_CAPABILITIES lpszExtra reports whether notes are classified
into all 4 levels and whether level 2 or 3 notes can be returned to the customer.
The above classification levels can be used to support note handling functionality which includes:
1. The ability to remove counterfeit notes from circulation.
2. Reporting of recognized, counterfeit and suspected counterfeit notes.
3. Creating and reporting of note signatures in order to allow back-tracing of notes.
A note’s classification can be changed based on the note’s serial number, currency and value by specifying a
blacklist or classification list. A blacklist reclassifies a matching note as level 2, whereas a classification list can be
used to re-classify a matching note to a lower level, including classifying a genuine note as unfit for dispensing.
Once reclassified, the note will be automatically handled according to the local country specific note handling
standard or legislation for the note’s new note classification, including any level 2 or 3 note retention rules. Any
reclassification will result in the normal events and behavior, for example a
WFS_EXEE_CIM_INFO_AVAILABLE event will reflect the note’s reclassification. Reclassification can be used
to make dynamic changes to note handling procedures without a software upgrade, enabling functionality such as
taking older notes out of circulation or handling of counterfeit notes on a local basis.
Reclassification cannot be used to change a note’s classification to a higher level, for example, a note recognized as
counterfeit by the device cannot be reclassified as genuine. In addition, it is not possible to re-classify a level 2 note
as level 1. No particular use case has been identified for reclassifying Level 3 and 4 notes as level 1, but there is no
reason to restrict this reclassification.
Blacklists can be specified using WFS_CMD_CIM_SET_BLACKLIST and retrieved using
WFS_INF_CIM_GET_BLACKLIST. Classification lists can be specified using
WFS_CMD_CIM_SET_CLASSIFICATION_LIST and retrieved using
WFS_INF_CIM_GET_CLASSIFICATION_LIST. A classification list is a superset of the blacklist; any items
specified as level 2 in the classification list are considered part of the blacklist. However, it is not recommended
that both sets of commands are used by a single application, as it may lead to overlap and confusion.
The blacklist or classification list functionality can use a mask to specify serial numbers. The mask is defined as
follows: A '?' character (0x003F) is the wildcard used to match a single Unicode character, and a '*' character
(0x002A) is the wildcard used to match one or more Unicode characters.
For example, “S8H9??16?4” would represent a match for the serial numbers “S8H9231654” and “S8H9761684”. A
mask of “HD90*2” would be used in order to match serial numbers that begin with “HD90” and end with “2”, for
example “HD9028882”, “HD9083276112”. Note that the mask can only use one asterisk, and if a real character is
required then it must be preceded by a backslash, for example: '\\' for a backslash, '\*' for an asterisk or '\?' for a
question mark. Note that this flexibility means that it is possible to overlap definitions, for example “HD90*” and
“HD902*” would both match on the serial number HD9028882”.
14

CWA 16926-15:2020 (E)
