---
source: XfsPin.pdf
document: XfsPin
device_class: PIN
part: 6
section_id: ""
title: "11. Appendix–D (X9.143 Key Use)"
type: appendix
source_lines: [13713, 13860]
---

<!-- 11. Appendix–D (X9.143 Key Use) -->

11. Appendix–D (X9.143 Key Use)
This section contains a mapping of key usages as defined for TR-31 (see ANSI X9.143 [Ref. 35]) to the XFS use
values defined in this document. The XFS use values are those defined for the fwUse or dwUse input/output fields
of a number of different PIN commands.
Keys imported within an ANSI X9.143 key block have a usage encoded into the key block header (represented by
lpxKeyBlockHeader in the WFS_INF_PIN_KEY_DETAIL and WFS_INF_PIN_KEY_DETAIL_EX commands),
This usage specified in the key block header may be more specific than the fwUse/dwUse values defined in this
document. For consistency, the following table defines the corresponding fwUse/dwUse value for each X9.143 key
usage:
X9.143 X9.143 Definition XFS Use (fwUse/dwUse)
Value Mode(s) of Use
“B0” “X” Base Derivation Key (BDK) WFS_PIN_USEKEYDERKEY
“B1” “X” DUKPT Initial Key WFS_PIN_USEKEYDERKEY**
WFS_PIN_USEPINREMOTE
WFS_PIN_USEFUNCTION*
WFS_PIN_USECRYPT
WFS_PIN_USEMACING
“B2” “Y” Base Key Variant Key WFS_PIN_USEKEYDERKEY
(deprecated)
“B3” “X” Key Derivation Key (Non ANSI WFS_PIN_USEKEYDERKEY
X9.24)
“C0” “C”, “G”, “V” Card Verification Key (CVK) NA
“D0” “B”, “D”, “E” Symmetric Key for Data WFS_PIN_USECRYPT
Encryption
“D1” “B”, “D”, “E” Asymmetric Key for Data WFS_PIN_USERSAPUBLIC
Encryption
WFS_PIN_USERSAPRIVATE
“D2” “B”, “D”, “E” Data Encryption Key for WFS_PIN_USECRYPT
Decimalization Table
“D3” “B”, “D”, “E” Data Encryption Key for WFS_PIN_USECRYPT
Sensitive Data
“E0” “X” EMV/Chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Application Cryptograms
“E1” “X” EMV/Chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Secure Messaging for
Confidentiality
“E2” “X” EMV/Chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Secure Messaging for Integrity
“E3” “X” EMV/Chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Data Authentication Code
“E4” “X” EMV/Chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Dynamic Numbers
“E5” “X” EMV/Chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Card Personalization
“E6” “X” EMV/Chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Other
317

CWA 16926-6:2022 (E)
X9.143 X9.143 Definition XFS Use (fwUse/dwUse)
Value Mode(s) of Use
“E7” “B”, “D”, “E” EMV / Chip Asymmetric Key WFS_PIN_USERSAPUBLIC
Pair for EMV/Smart Card based
PIN/PIN Block Encryption
“F0” “X” EMV/Chip Card Key: WFS_PIN_USERSAPUBLICVERIFY
Application Cryptograms
“F1” “X” EMV/Chip Card Key: Secure WFS_PIN_USERSAPUBLICVERIFY
Messaging for Confidentiality
“F2” “X” EMV/Chip Card Key: Secure WFS_PIN_USERSAPUBLICVERIFY
Messaging for Integrity
“F3” “X” EMV/Chip Card Key: Data WFS_PIN_USERSAPUBLICVERIFY
Authentication Code
“F4” “X” EMV/Chip Card Key: Dynamic WFS_PIN_USERSAPUBLICVERIFY
Numbers
“F5” “X” EMV/Chip Card Key: Card WFS_PIN_USERSAPUBLICVERIFY
Personalization
“F6” “X” EMV/Chip Card Key: Other WFS_PIN_USERSAPUBLICVERIFY
“I0” “N” Initialization Vector (IV) NA
“K0” “B”, “D”, “E” Key Encryption Key or Key WFS_PIN_USEKEYENCKEY
Wrapping Key WFS_PIN_USESVENCKEY
“K1” “B”, “D”, “E” Key Block Protection Key, ANSI WFS_PIN_USEANSTR31MASTER
X9.143/TR-31
“K2” “B”, “D”, “E” Asymmetric Key Pair (KRD), WFS_PIN_USERSAPUBLIC
ANSI X9.139/TR-34
WFS_PIN_USERSAPRIVATE
“K3” “B”, “D”, “E”, Asymmetric Key Pair for Key WFS_PIN_USERSAPUBLIC
”X” Wrapping/Key Agreement
WFS_PIN_USERSAPRIVATE
“K4” “B”, “D”, “E” Key Block Protection Key, ISO WFS_PIN_USEANSTR31MASTER
20038
“M0” “C”, “G”, “V” MAC Key, ISO 16609 MAC WFS_PIN_USEMACING
algorithm 1 (using TDEA)
“M1” “C”, “G”, “V” MAC Key, ISO 9797-1 MAC WFS_PIN_USEMACING
Algorithm 1
“M2” “C”, “G”, “V” MAC Key, ISO 9797-1 MAC WFS_PIN_USEMACING
Algorithm 2
“M3” “C”, “G”, “V” MAC Key, ISO 9797-1 MAC WFS_PIN_USEMACING
Algorithm 3
“M4” “C”, “G”, “V” MAC Key, ISO 9797-1 MAC WFS_PIN_USEMACING
Algorithm 4
“M5” “C”, “G”, “V” MAC Key, ISO 9797-1:1999 WFS_PIN_USEMACING
MAC Algorithm 5
“M6” “C”, “G”, “V” MAC Key, ISO 9797-1:2011 WFS_PIN_USEMACING
MAC Algorithm 5/CMAC
“M7” “C”, “G”, “V” HMAC Key WFS_PIN_USEMACING
“M8” “C”, “G”, “V” MAC Key, ISO 9797-1:2011 WFS_PIN_USEMACING
MAC Algorithm 6
318

CWA 16926-6:2022 (E)
| X9.143  | X9.143          | Definition  | XFS Use (fwUse/dwUse)  |
| ------- | --------------- | ----------- | ---------------------- |
| Value   | Mode(s) of Use  |             |                        |
“P0”  “B”, “D”, “E”  PIN Encryption Key  WFS_PIN_USEPINREMOTE
WFS_PIN_USEFUNCTION*
“P1”  “G”  PIN Generation Key (reserved for  WFS_PIN_USEPINLOCAL
|       |      | ANSI X9.132- 202x)         | WFS_PIN_USEFUNCTION*  |
| ----- | ---- | -------------------------- | --------------------- |
| “P2”  | “G”  | PIN Generation Key, other  | WFS_PIN_USEPINLOCAL   |
|       |      | algorithm                  | WFS_PIN_USEFUNCTION*  |
“S0”  “S”, “V”  Asymmetric Key Pair for Digital  WFS_PIN_USERSAPUBLICVERIFY
Signature
WFS_PIN_USERSAPRIVATESIGN
“S1”  “S”, “V”  Asymmetric Key Pair, CA Key  WFS_PIN_USERSAPUBLICVERIFY
WFS_PIN_USERSAPRIVATESIGN
“S2”  “S”, “V”  Asymmetric Key Pair, non-ANSI  WFS_PIN_USERSAPUBLICVERIFY
X9.24 Key
WFS_PIN_USERSAPRIVATESIGN
“V0”  “C”, “G”, “V”  PIN Verification Key, PVK, other  WFS_PIN_USEPINLOCAL
|     |     | algorithm  | WFS_PIN_USEFUNCTION*  |
| --- | --- | ---------- | --------------------- |
“V1”  “C”, “G”, “V”  PIN Verification Key, IBM 3624  WFS_PIN_USEPINLOCAL
WFS_PIN_USEFUNCTION*
“V2”  “C”, “G”, “V”  PIN Verification Key, VISA PVV  WFS_PIN_USEPINLOCAL
WFS_PIN_USEFUNCTION*
“V3”  “C”, “G”, “V”  PIN Verification Key, ANSI X9- WFS_PIN_USEPINLOCAL
|     |     | 132 algorithm 1  | WFS_PIN_USEFUNCTION*  |
| --- | --- | ---------------- | --------------------- |
“V4”  “C”, “G”, “V”  PIN Verification Key, ANSI X9- WFS_PIN_USEPINLOCAL
|     |     | 132 algorithm 2  | WFS_PIN_USEFUNCTION*  |
| --- | --- | ---------------- | --------------------- |
“V5”  “C”, “G”, “V”  PIN Verification Key, ANSI  WFS_PIN_USEPINLOCAL
|     |     | X9.132 algorithm 3  | WFS_PIN_USEFUNCTION*  |
| --- | --- | ------------------- | --------------------- |
* Note that WFS_PIN_USEFUNCTION is listed here for backward compatibility, but
WFS_PIN_USEPINLOCAL/WFS_PIN_USEPINREMOTE is the more accurate single-use value.
** The Base Derivation Key is used to derive the IPEK. When a DUKPT IPEK is loaded, derived future keys are
stored and the IPEK deleted. Therefore, while the IPEK is no longer loaded, future keys directly related to it are.
WFS_PIN_USEPINREMOTE and optionally WFS_PIN_USEFUNCTION are included as the primary use of an
IPEK future key is to create a variant for PIN encryption. If the optional variant data encryption and MAC keys are
supported, WFS_PIN_USECRYPT and WFS_PIN_USEMACING must be included. To use the optional data or
MAC keys in a WFS_CMD_PIN_CRYPT command, lpsKey must be the name of the IPEK and wAlgorithm must
be WFS_PIN_CRYPTTRIDESCBC or WFS_PIN_CRYPTTRIDESMAC. If the optional data encryption key is
being used, wMode must be WFS_PIN_MODEENCRYPT. The optional variant response data encryption and
MAC keys are not supported.
319

CWA 16926-6:2022 (E)
