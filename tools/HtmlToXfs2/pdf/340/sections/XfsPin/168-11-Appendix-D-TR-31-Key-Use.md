---
source: XfsPin.pdf
document: XfsPin
device_class: PIN
part: 6
section_id: ""
title: "11. Appendix–D (TR-31 Key Use)"
type: appendix
source_lines: [13303, 13375]
---

<!-- 11. Appendix–D (TR-31 Key Use) -->

11. Appendix–D (TR-31 Key Use)
This section contains a mapping of key usages as defined for TR-31 (see ANS X9 TR-31 2010 [Ref. 35]) to the
XFS use values defined in this document. The XFS use values are those defined for the fwUse or dwUse
input/output fields of a number of different PIN commands.
Keys imported within an ANS TR-31 key block have a usage encoded into the key block header (represented by
lpxKeyBlockHeader in the WFS_INF_PIN_KEY_DETAIL and WFS_INF_PIN_KEY_DETAIL_EX commands),
This usage specified in the key block header may be more specific than the fwUse/dwUse values defined in this
document. For consistency, the following table defines the corresponding fwUse/dwUse value for each TR-31 key
usage:
TR-31 TR-31 Definition XFS Use (fwUse/dwUse)
Value Mode(s) of Use
“B0” “X” BDK Base Derivation Key WFS_PIN_USEKEYDERKEY
“B1” “X” DUKPT Initial Key (also known WFS_PIN_USEKEYDERKEY**
as IPEK) WFS_PIN_USEPINREMOTE
WFS_PIN_USEFUNCTION*
WFS_PIN_USECRYPT
WFS_PIN_USEMACING
“C0” “C”, “G”, “V” CVK Card Verification Key NA
“D0” “B”, “D”, “E” Data Encryption using ECB, WFS_PIN_USECRYPT
CBC, CFB, OFB, CCM or CTR
“E0” “X” EMV/chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Application cryptograms
“E1” “X” EMV/chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Secure Messaging for
Confidentiality
“E2” “X” EMV/chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Secure Messaging for Integrity
“E3” “X” EMV/chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Data Authentication Code
“E4” “X” EMV/chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Dynamic Numbers
“E5” “X” EMV/chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Card Personalization
“E6” “X” EMV/chip Issuer Master Key: WFS_PIN_USERSAPUBLICVERIFY
Other
“I0” “N” Initialization Vector (IV) NA
“K0” “B”, “D”, “E” Key Encryption or wrapping WFS_PIN_USEKEYENCKEY
WFS_PIN_USESVENCKEY
“K1” “B”, “D”, “E” TR-31 Key Block Protection Key WFS_PIN_USEANSTR31MASTER
“M0” “C”, “G”, “V” ISO 16609 MAC algorithm 1 WFS_PIN_USEMACING
(using TDEA)
“M1” “C”, “G”, “V” ISO 9797-1 MAC Algorithm 1 WFS_PIN_USEMACING
“M2” “C”, “G”, “V” ISO 9797-1 MAC Algorithm 2 WFS_PIN_USEMACING
“M3” “C”, “G”, “V” ISO 9797-1 MAC Algorithm 3 WFS_PIN_USEMACING
“M4” “C”, “G”, “V” ISO 9797-1 MAC Algorithm 4 WFS_PIN_USEMACING
308

CWA 16926-6:2020 (E)
TR-31 TR-31 Definition XFS Use (fwUse/dwUse)
Value Mode(s) of Use
“M5” “C”, “G”, “V” ISO 9797-1 MAC Algorithm 5 WFS_PIN_USEMACING
“P0” “B”, “D”, “E” PIN Encryption WFS_PIN_USEPINREMOTE
WFS_PIN_USEFUNCTION*
“V0” “C”, “G”, “V” PIN verification, KPV, other WFS_PIN_USEPINLOCAL
algorithm WFS_PIN_USEFUNCTION*
“V1” “C”, “G”, “V” PIN verification, IBM 3624 WFS_PIN_USEPINLOCAL
WFS_PIN_USEFUNCTION*
“V2” “C”, “G”, “V” PIN Verification, VISA PVV WFS_PIN_USEPINLOCAL
WFS_PIN_USEFUNCTION*
* Note that WFS_PIN_USEFUNCTION is listed here for backward compatibility, but
WFS_PIN_USEPINLOCAL/WFS_PIN_USEPINREMOTE is the more accurate single-use value.
** The Base Derivation Key is used to derive the IPEK. When a DUKPT IPEK is loaded, derived future keys are
stored and the IPEK deleted. Therefore, while the IPEK is no longer loaded, future keys directly related to it are.
WFS_PIN_USEPINREMOTE and optionally WFS_PIN_USEFUNCTION are included as the primary use of an
IPEK future key is to create a variant for PIN encryption. If the optional variant data encryption and MAC keys are
supported, WFS_PIN_USECRYPT and WFS_PIN_USEMACING must be included. To use the optional data or
MAC keys in a WFS_PIN_CMD_CRYPT command, lpsKey must be the name of the IPEK and wAlgorithm must
be WFS_PIN_CRYPTTRIDESCBC or WFS_PIN_CRYPTTRIDESMAC. If the optional data encryption key is
being used, wMode must be WFS_PIN_MODEENCRYPT. The optional variant response data encryption and
MAC keys are not supported.
309

CWA 16926-6:2015 (E)
