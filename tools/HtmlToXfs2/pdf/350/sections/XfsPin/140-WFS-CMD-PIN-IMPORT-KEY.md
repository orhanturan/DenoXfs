---
source: XfsPin.pdf
document: XfsPin
device_class: PIN
part: 6
section_id: ""
title: "WFS_CMD_PIN_IMPORT_KEY"
type: execute_command
source_lines: [11462, 11559]
---

<!-- WFS_CMD_PIN_IMPORT_KEY -->

WFS_CMD_PIN_IMPORT_KEY
This section provides an example of the sequence of commands required to enter an encryption key securely using
the WFS_INF_PIN_SECUREKEY_DETAIL and WFS_CMD_PIN_IMPORT_KEY commands. It is recommended
that the WFS_CMD_PIN_GET_LAYOUT and WFS_CMD_PIN_IMPORT_KEY_340 commands be used for
maximum compatibility with all device types and key algorithms.
In the following sequence, the application retrieves the keyboard secure key entry mode and associated keyboard
layout and displays an image of the keyboard for the user. It then gets the first key part, verifies the KCV for the
key part and stores it. The sequence is repeated for the second key part and then finally the key is activated.
WFS_INF_PIN_SECUREKEY_DETAIL PIN
App
Display Keyboard Layout
WFS_CMD_PIN_SECUREKEY_ENTRY ( Part 1)
[part 1 master encryption key values are manually entered on the PinPad]
Verify KCV
[verify part 1 master key KCV as returned with the output data of the
WFS_CMD_PIN_SECUREKEY_ENTRY command]
WFS_CMD_PIN_IMPORT_KEY
(“MasterKey”,“”,NULL,NULL,WFS_PIN_USEKEYENCKEY |
WFS_PIN_USESECURECONSTRUCT)
[part 1 master encryption key values loaded with the secure key entry
command]
WFS_CMD_PIN_SECUREKEY_ENTRY
[part 2 master encryption key values are manually entered on the PinPad]
Verify KCV
[verify part 2 master key KCV as returned with the output data of the
WFS_CMD_PIN_SECUREKEY_ENTRY command]
WFS_CMD_PIN_IMPORT_KEY
(“MasterKey”,“”,NULL,NULL, WFS_PIN_USEKEYENCKEY |
WFS_PIN_USESECURECONSTRUCT)
[part 2 master encryption key values loaded with the secure key entry
command]
WFS_CMD_PIN_IMPORT_KEY
(“MasterKey”,“”,NULL,NULL,WFS_PIN_USEKEYENCKEY)
[activates the full master encryption key]
Verify KCV
[verify full master key KCV as returned with the output data of the
WFS_CMD_PIN_IMPORT_KEY command]
262

CWA 16926-6:2022 (E)
8.6.3 Command Usage - WFS_INF_PIN_GET_LAYOUT and
WFS_CMD_PIN_IMPORT_KEY_340
This section provides an example of the sequence of commands required to enter an encryption key securely using
the preferred WFS_INF_PIN_GET_LAYOUT and WFS_CMD_PIN_IMPORT_KEY_340 commands.
In the following sequence, the application retrieves the keyboard secure key entry mode and associated keyboard
layout and displays an image of the keyboard for the user. It then gets the first key part, verifies the KCV for the
key part and stores it. The sequence is repeated for the second key part and then finally the key is activated.
WFS_INF_PIN_GET_LAYOUT PIN
App
Display Keyboard Layout
Enter part1 of the MasterKey using the device keyboard.
WFS_CMD_PIN_SECUREKEY_ENTRY
wVerificationType = WFS_PIN_KCVZERO | WFS_PIN_KCVAES
Verify the KCV of part 1 of the MasterKey returned with the output
data of the WFS_CMD_PIN_SECUREKEY_ENTRY command
Import part 1 of the MasterKey.
WFS_CMD_PIN_IMPORT_KEY_340
lpsKey = MasterKey
lpKeyAttributes = ‘K0’, ‘A’, ‘D’, WFS_PIN_CRYPTOCONSTRUCT
lpxValue = NULL
lpsDecryptKey = NULL
dwDecryptMethod = 0
lpxVerificationData = NULL
lpsVerifyKey = NULL
lpVerifyAttributes = NULL
lpxVendorAttributes = NULL
Enter part2 of the MasterKey entered using the device keyboard.
WFS_CMD_PIN_SECUREKEY_ENTRY
wVerificationType = WFS_PIN_KCVZERO | WFS_PIN_KCVAES
Verify the KCV of part 2 of the MasterKey returned with the output
data of the WFS_CMD_PIN_SECUREKEY_ENTRY command
Import part 2 of the MasterKey.
WFS_CMD_PIN_IMPORT_KEY_340
lpsKey = MasterKey
lpKeyAttributes = ‘K0’, ‘A’, ‘D’, WFS_PIN_CRYPTOCONSTRUCT
lpxValue = NULL
lpsDecryptKey = NULL
dwDecryptMethod = 0
lpxVerificationData = NULL
lpsVerifyKey = NULL
lpVerifyAttributes = NULL
lpxVendorAttributes = NULL
Activate the MasterKey.
WFS_CMD_PIN_IMPORT_KEY_340
lpsKey = MasterKey
lpKeyAttributes = ‘K0’, ‘A’, ‘D’, 0
lpxValue = NULL
lpsDecryptKey = NULL
dwDecryptMethod = 0
lpxVerificationData = ‘00’, ‘A’, ‘V’, WFS_PIN_KCVZERO
lpsVerifyKey = NULL
lpVerifyAttributes = NULL
lpxVendorAttributes = NULL
Verify the KCV of the full MasterKey KCV as returned with the output
data of the WFS_CMD_PIN_IMPORT_KEY_340 command
263

CWA 16926-6:2022 (E)
