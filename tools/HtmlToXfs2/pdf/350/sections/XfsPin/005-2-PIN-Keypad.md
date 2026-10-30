---
source: XfsPin.pdf
document: XfsPin
device_class: PIN
part: 6
section_id: ""
title: "2. PIN Keypad"
type: section
source_lines: [427, 484]
---

<!-- 2. PIN Keypad -->

2. PIN Keypad
This section describes the application program interface for personal identification number keypads (PIN pads) and
other encryption/decryption devices. This description includes definitions of the service-specific commands that
can be issued, using the WFSAsyncExecute, WFSExecute, WFSGetInfo and WFSAsyncGetInfo functions.
This section describes the general interface for the following functions:
• Administration of encryption devices
• Loading of encryption keys
• Encryption / decryption
• Entering Personal Identification Numbers (PINs)
• PIN verification
• PIN block generation (encrypted PIN)
• Clear text data handling
• Function key handling
• PIN presentation to chipcard
• Read and write safety critical Terminal Data from/to HSM
• HSM and Chipcard Authentication
• EMV 4.0 PIN blocks, EMV 4.0 public key loading, static and dynamic data verification
If the PIN pad device has local display capability, display handling should be handled using the Text Terminal Unit
(TTU) interface.
The adoption of this specification does not imply the adoption of a specific security standard.
Important Notes:
• This revision of this specification does not define all key management procedures; some key management
is still vendor-specific.
• Key space management is customer-specific, and is therefore handled by vendor-specific mechanisms.
• Only numeric PIN pads are handled in this specification.
This specification also supports the Hardware Security Module (HSM), which is necessary for the German ZKA
Electronic Purse transactions. Furthermore the HSM stores terminal specific data.
This data will be compared against the message data fields (Sent and Received ISO8583 messages) prior to HSM-
MAC generation/verification. HSM-MACs are generated/verified only if the message fields match the data stored.
Keys used for cryptographic HSM functions are stored separate from other keys. This must be considered when
importing keys.
This version of PIN pad complies to the current ZKA specification 3.0. It supports loading and unloading against
card account for both card types (Type 0 and Type 1) of the ZKA electronic purse. It also covers the necessary
functionality for ‘Loading against other legal tender’.
Key values are passed to the API as binary hexadecimal values, for example:
0123456789ABCDEF = 0x01 0x23 0x45 0x67 0x89 0xAB 0xCD 0xEF
When hex values are passed to the API within strings, the hex digits 0xA to 0xF can be represented by characters in
the ranges ‘a’ to ‘f’ or ‘A’ to ‘F’.
The following commands and events were initially added to support the German ZKA standard, but may also be
used for other national standards:
• WFS_INF_PIN_HSM_TDATA
• WFS_CMD_PIN_HSM_SET_TDATA
• WFS_CMD_PIN_SECURE_MSG_SEND
12

CWA 16926-6:2022 (E)
• WFS_CMD_PIN_SECURE_MSG_RECEIVE
• WFS_CMD_PIN_GET_JOURNAL
• WFS_SRVE_PIN_OPT_REQUIRED
• WFS_CMD_PIN_HSM_INIT
• WFS_SRVE_PIN_HSM_TDATA_CHANGED
Certain levels of the PCI EPP security standards specify that if a key encryption key is deleted or replaced, then all
keys in the hierarchy under that key encryption key are also removed. Key encryption keys have the
WFS_PIN_USEKEYENCKEY type of access. Applications can check impact of key deletion using
WFS_INF_PIN_KEY_DETAIL or WFS_INF_PIN_KEY_DETAIL_EX.
13

CWA 16926-6:2022 (E)
