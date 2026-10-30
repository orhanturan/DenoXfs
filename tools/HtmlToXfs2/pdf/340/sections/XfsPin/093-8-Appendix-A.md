---
source: XfsPin.pdf
document: XfsPin
device_class: PIN
part: 6
section_id: ""
title: "8. Appendix-A"
type: appendix
source_lines: [8940, 9019]
---

<!-- 8. Appendix-A -->

8.  Appendix-A
This section provides extended explanation of concepts and functionality needing further clarification. The
terminology as described below is used within the following sections.
Definitions and Abbreviations
ATM  Automated Teller Machine, used here for any type of self-service terminal,
regardless whether it actually dispenses cash
| CA  | Certificate Authority  |     |     |
| --- | ---------------------- | --- | --- |
Certificate  A data structure that contains a public key and a name that allows certification of a
public key belonging to a specific individual. This is certified using digital
signatures.
| Host  | The remote system that an ATM communicates with.   |     |     |
| ----- | -------------------------------------------------- | --- | --- |
| KTK   | Key Transport Key                                  |     |     |
| PKI   | Public Key Infrastructure                          |     |     |
Private Key  That key of an entity’s key pair that should only be used by that entity.
| Public Key        | That key of an entity’s key pair that can be made public.   |     |     |
| ----------------- | ----------------------------------------------------------- | --- | --- |
| Symmetric Key     | A key used with symmetric cryptography                      |     |     |
| Verification Key  | A key that is used to verify the validity of a certificate  |     |     |
SignatureIssuer  An entity that signs the ATM´s public key at production time, may be the ATM
manufacturer

Notation of Cryptographic Items and Functions
| SKE  | The private key belonging to entity E  |     |     |
| ---- | -------------------------------------- | --- | --- |
| PKE  | The public belonging to entity E       |     |     |
The private key belonging to the ATM/PIN
SKATM
| PKATM   | The public key belonging to the ATM/PIN        |     |     |
| ------- | ---------------------------------------------- | --- | --- |
| SKHOST  | The private key belonging to the Host          |     |     |
| PKHOST  | The public key belonging to the Host           |     |     |
| SKSI    | The private key belonging to Signature Issuer  |     |     |
The public key belonging to Signature Issuer
PKSI
| SKROOT  | The root private key belonging to the Host  |     |     |
| ------- | ------------------------------------------- | --- | --- |
| PKROOT  | The root public key belonging to the Host   |     |     |
| KNAME   | A symmetric key                             |     |     |
CertHOST  A Certificate that contains the public verification of the host and is signed by a
trusted Certificate Authority.
CertATM  A Certificate that contains the ATM/PIN public verification or encipherment key,
which is signed by a trusted Certificate Authority.
The Certificate of a new Certificate Authority
CertCA
| RATM   | Random Number of the ATM/PIN  |     |     |
| ------ | ----------------------------- | --- | --- |
| IHOST  | Identifier of the Host        |     |     |
| KKTK   | Key Transport Key             |     |     |
| RHOST  | Random number of the Host     |     |     |
Identifier of the ATM/PIN
IATM
| TPATM         | Thumb Print of the ATM/PIN                            |     |     |
| ------------- | ----------------------------------------------------- | --- | --- |
| Sign(SKE)[D]  | The signing of data block D, using the private key SK |     |     |
E
Recover(PKE)[S]  The recovery of the data block D from the signature S, using the private key PK E
RSACrypt(PKE)[D]  RSA Encryption of the data block D using the public key PK
E
Hashing of a message M of arbitrary length to a 20 Byte hash value
Hash [M]
Des(K) [D]  DES encipherment of an 8 byte data block D using the secret key K
Des-1(K)[D]  DES decipherment of an 8 byte data block D using the 8 byte secret key K
Des3(K)[D]  Triple DES encipherment of an 8 byte data block D using the 16 byte secret key K =
|     | (K  || K ), equivalent to Des(K | ) [ Des-1(K ) [ Des(K | ) [D] ] ]   |
| --- | ------------------------------- | --------------------- | ----------- |
|     | L R                             | L R                   | L           |
Des3-1 (K) [D]  Triple DES decipherment of an 8 byte data block D using the 16 byte secret key K =
|       | ), equivalent to Des-1 (K            | ) [ Des-1 (K   |               |
| ----- | ------------------------------------ | -------------- | ------------- |
|       | (K L  || K R                         | L ) [ Des (K R | L ) [D] ] ]   |
| RndE  | A random number created by entity E  |                |               |
Unique Identifier for entity E
UIE
| ( A || B )  | Concatenation of A and B  |     |     |
| ----------- | ------------------------- | --- | --- |
200

CWA 16926-6:2020 (E)
