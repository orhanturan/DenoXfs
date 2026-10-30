# XfsPin - PIN

PIN Keypad Device Class Interface

Source: `XfsPin.pdf` | sections: 178 | split level: 5

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 222 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 148 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1. Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.50 | section | 14 | [003-1.1-Background-to-Release-3-50.md](003-1.1-Background-to-Release-3-50.md) |
| 1.2 XFS Service-Specific Programming | section | 41 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2. PIN Keypad | section | 58 | [005-2-PIN-Keypad.md](005-2-PIN-Keypad.md) |
| 2.1 Encrypting Touch Screen (ETS) | section | 75 | [006-2.1-Encrypting-Touch-Screen-ETS.md](006-2.1-Encrypting-Touch-Screen-ETS.md) |
| 3. References | section | 90 | [007-3-References.md](007-3-References.md) |
| 4. Info Commands | info_commands_section | 1 | [008-4-Info-Commands.md](008-4-Info-Commands.md) |
| 4.1 WFS_INF_PIN_STATUS | info_command | 199 | [009-4.1-WFS-INF-PIN-STATUS.md](009-4.1-WFS-INF-PIN-STATUS.md) |
| 4.2 WFS_INF_PIN_CAPABILITIES | info_command | 1229 | [010-4.2-WFS-INF-PIN-CAPABILITIES.md](010-4.2-WFS-INF-PIN-CAPABILITIES.md) |
| 4.3 WFS_INF_PIN_KEY_DETAIL | info_command | 77 | [011-4.3-WFS-INF-PIN-KEY-DETAIL.md](011-4.3-WFS-INF-PIN-KEY-DETAIL.md) |
| 4.4 WFS_INF_PIN_FUNCKEY_DETAIL | info_command | 133 | [012-4.4-WFS-INF-PIN-FUNCKEY-DETAIL.md](012-4.4-WFS-INF-PIN-FUNCKEY-DETAIL.md) |
| 4.5 WFS_INF_PIN_HSM_TDATA | info_command | 13 | [013-4.5-WFS-INF-PIN-HSM-TDATA.md](013-4.5-WFS-INF-PIN-HSM-TDATA.md) |
| 4.6 WFS_INF_PIN_KEY_DETAIL_EX | info_command | 148 | [014-4.6-WFS-INF-PIN-KEY-DETAIL-EX.md](014-4.6-WFS-INF-PIN-KEY-DETAIL-EX.md) |
| 4.7 WFS_INF_PIN_SECUREKEY_DETAIL | info_command | 191 | [015-4.7-WFS-INF-PIN-SECUREKEY-DETAIL.md](015-4.7-WFS-INF-PIN-SECUREKEY-DETAIL.md) |
| 4.8 WFS_INF_PIN_QUERY_LOGICAL_HSM_DETAIL | info_command | 33 | [016-4.8-WFS-INF-PIN-QUERY-LOGICAL-HSM-DETAIL.md](016-4.8-WFS-INF-PIN-QUERY-LOGICAL-HSM-DETAIL.md) |
| 4.9 WFS_INF_PIN_QUERY_PCIPTS_DEVICE_ID | info_command | 41 | [017-4.9-WFS-INF-PIN-QUERY-PCIPTS-DEVICE-ID.md](017-4.9-WFS-INF-PIN-QUERY-PCIPTS-DEVICE-ID.md) |
| 4.10 WFS_INF_PIN_GET_LAYOUT | info_command | 228 | [018-4.10-WFS-INF-PIN-GET-LAYOUT.md](018-4.10-WFS-INF-PIN-GET-LAYOUT.md) |
| 4.11 WFS_INF_PIN_KEY_DETAIL_340 | info_command | 98 | [019-4.11-WFS-INF-PIN-KEY-DETAIL-340.md](019-4.11-WFS-INF-PIN-KEY-DETAIL-340.md) |
| 5. Execute Commands | execute_commands_section | 1 | [020-5-Execute-Commands.md](020-5-Execute-Commands.md) |
| 5.1 Normal PIN Commands | section | 2 | [021-5.1-Normal-PIN-Commands.md](021-5.1-Normal-PIN-Commands.md) |
| 5.1.1 WFS_CMD_PIN_CRYPT | execute_command | 123 | [022-5.1.1-WFS-CMD-PIN-CRYPT.md](022-5.1.1-WFS-CMD-PIN-CRYPT.md) |
| 5.1.2 WFS_CMD_PIN_IMPORT_KEY | execute_command | 136 | [023-5.1.2-WFS-CMD-PIN-IMPORT-KEY.md](023-5.1.2-WFS-CMD-PIN-IMPORT-KEY.md) |
| 5.1.3 WFS_CMD_PIN_DERIVE_KEY | execute_command | 70 | [024-5.1.3-WFS-CMD-PIN-DERIVE-KEY.md](024-5.1.3-WFS-CMD-PIN-DERIVE-KEY.md) |
| 5.1.4 WFS_CMD_PIN_GET_PIN | execute_command | 164 | [025-5.1.4-WFS-CMD-PIN-GET-PIN.md](025-5.1.4-WFS-CMD-PIN-GET-PIN.md) |
| 5.1.5 WFS_CMD_PIN_LOCAL_DES | execute_command | 83 | [026-5.1.5-WFS-CMD-PIN-LOCAL-DES.md](026-5.1.5-WFS-CMD-PIN-LOCAL-DES.md) |
| 5.1.6 WFS_CMD_PIN_CREATE_OFFSET | execute_command | 75 | [027-5.1.6-WFS-CMD-PIN-CREATE-OFFSET.md](027-5.1.6-WFS-CMD-PIN-CREATE-OFFSET.md) |
| 5.1.7 WFS_CMD_PIN_LOCAL_EUROCHEQUE | execute_command | 72 | [028-5.1.7-WFS-CMD-PIN-LOCAL-EUROCHEQUE.md](028-5.1.7-WFS-CMD-PIN-LOCAL-EUROCHEQUE.md) |
| 5.1.8 WFS_CMD_PIN_LOCAL_VISA | execute_command | 61 | [029-5.1.8-WFS-CMD-PIN-LOCAL-VISA.md](029-5.1.8-WFS-CMD-PIN-LOCAL-VISA.md) |
| 5.1.9 WFS_CMD_PIN_PRESENT_IDC | execute_command | 112 | [030-5.1.9-WFS-CMD-PIN-PRESENT-IDC.md](030-5.1.9-WFS-CMD-PIN-PRESENT-IDC.md) |
| 5.1.10 WFS_CMD_PIN_GET_PINBLOCK | execute_command | 93 | [031-5.1.10-WFS-CMD-PIN-GET-PINBLOCK.md](031-5.1.10-WFS-CMD-PIN-GET-PINBLOCK.md) |
| 5.1.11 WFS_CMD_PIN_GET_DATA | execute_command | 119 | [032-5.1.11-WFS-CMD-PIN-GET-DATA.md](032-5.1.11-WFS-CMD-PIN-GET-DATA.md) |
| 5.1.12 WFS_CMD_PIN_INITIALIZATION | execute_command | 71 | [033-5.1.12-WFS-CMD-PIN-INITIALIZATION.md](033-5.1.12-WFS-CMD-PIN-INITIALIZATION.md) |
| 5.1.13 WFS_CMD_PIN_LOCAL_BANKSYS | execute_command | 38 | [034-5.1.13-WFS-CMD-PIN-LOCAL-BANKSYS.md](034-5.1.13-WFS-CMD-PIN-LOCAL-BANKSYS.md) |
| 5.1.14 WFS_CMD_PIN_BANKSYS_IO | execute_command | 32 | [035-5.1.14-WFS-CMD-PIN-BANKSYS-IO.md](035-5.1.14-WFS-CMD-PIN-BANKSYS-IO.md) |
| 5.1.15 WFS_CMD_PIN_RESET | execute_command | 13 | [036-5.1.15-WFS-CMD-PIN-RESET.md](036-5.1.15-WFS-CMD-PIN-RESET.md) |
| 5.1.16 WFS_CMD_PIN_HSM_SET_TDATA | execute_command | 87 | [037-5.1.16-WFS-CMD-PIN-HSM-SET-TDATA.md](037-5.1.16-WFS-CMD-PIN-HSM-SET-TDATA.md) |
| 5.1.17 WFS_CMD_PIN_SECURE_MSG_SEND | execute_command | 70 | [038-5.1.17-WFS-CMD-PIN-SECURE-MSG-SEND.md](038-5.1.17-WFS-CMD-PIN-SECURE-MSG-SEND.md) |
| 5.1.18 WFS_CMD_PIN_SECURE_MSG_RECEIVE | execute_command | 66 | [039-5.1.18-WFS-CMD-PIN-SECURE-MSG-RECEIVE.md](039-5.1.18-WFS-CMD-PIN-SECURE-MSG-RECEIVE.md) |
| 5.1.19 WFS_CMD_PIN_GET_JOURNAL | execute_command | 32 | [040-5.1.19-WFS-CMD-PIN-GET-JOURNAL.md](040-5.1.19-WFS-CMD-PIN-GET-JOURNAL.md) |
| 5.1.20 WFS_CMD_PIN_IMPORT_KEY_EX | execute_command | 149 | [041-5.1.20-WFS-CMD-PIN-IMPORT-KEY-EX.md](041-5.1.20-WFS-CMD-PIN-IMPORT-KEY-EX.md) |
| 5.1.21 WFS_CMD_PIN_ENC_IO | execute_command | 92 | [042-5.1.21-WFS-CMD-PIN-ENC-IO.md](042-5.1.21-WFS-CMD-PIN-ENC-IO.md) |
| 5.1.22 WFS_CMD_PIN_HSM_INIT | execute_command | 41 | [043-5.1.22-WFS-CMD-PIN-HSM-INIT.md](043-5.1.22-WFS-CMD-PIN-HSM-INIT.md) |
| 5.1.23 WFS_CMD_PIN_SECUREKEY_ENTRY | execute_command | 164 | [044-5.1.23-WFS-CMD-PIN-SECUREKEY-ENTRY.md](044-5.1.23-WFS-CMD-PIN-SECUREKEY-ENTRY.md) |
| 5.1.24 WFS_CMD_PIN_GENERATE_KCV | execute_command | 45 | [045-5.1.24-WFS-CMD-PIN-GENERATE-KCV.md](045-5.1.24-WFS-CMD-PIN-GENERATE-KCV.md) |
| 5.1.25 WFS_CMD_PIN_SET_GUIDANCE_LIGHT | execute_command | 68 | [046-5.1.25-WFS-CMD-PIN-SET-GUIDANCE-LIGHT.md](046-5.1.25-WFS-CMD-PIN-SET-GUIDANCE-LIGHT.md) |
| 5.1.26 WFS_CMD_PIN_MAINTAIN_PIN | execute_command | 28 | [047-5.1.26-WFS-CMD-PIN-MAINTAIN-PIN.md](047-5.1.26-WFS-CMD-PIN-MAINTAIN-PIN.md) |
| 5.1.27 WFS_CMD_PIN_KEYPRESS_BEEP | execute_command | 24 | [048-5.1.27-WFS-CMD-PIN-KEYPRESS-BEEP.md](048-5.1.27-WFS-CMD-PIN-KEYPRESS-BEEP.md) |
| 5.1.28 WFS_CMD_PIN_SET_PINBLOCK_DATA | execute_command | 39 | [049-5.1.28-WFS-CMD-PIN-SET-PINBLOCK-DATA.md](049-5.1.28-WFS-CMD-PIN-SET-PINBLOCK-DATA.md) |
| 5.1.29 WFS_CMD_PIN_SET_LOGICAL_HSM | execute_command | 53 | [050-5.1.29-WFS-CMD-PIN-SET-LOGICAL-HSM.md](050-5.1.29-WFS-CMD-PIN-SET-LOGICAL-HSM.md) |
| 5.1.30 WFS_CMD_PIN_IMPORT_KEYBLOCK | execute_command | 56 | [051-5.1.30-WFS-CMD-PIN-IMPORT-KEYBLOCK.md](051-5.1.30-WFS-CMD-PIN-IMPORT-KEYBLOCK.md) |
| 5.1.31 WFS_CMD_PIN_POWER_SAVE_CONTROL | execute_command | 33 | [052-5.1.31-WFS-CMD-PIN-POWER-SAVE-CONTROL.md](052-5.1.31-WFS-CMD-PIN-POWER-SAVE-CONTROL.md) |
| 5.1.32 WFS_CMD_PIN_DEFINE_LAYOUT | execute_command | 43 | [053-5.1.32-WFS-CMD-PIN-DEFINE-LAYOUT.md](053-5.1.32-WFS-CMD-PIN-DEFINE-LAYOUT.md) |
| 5.1.33 WFS_CMD_PIN_START_AUTHENTICATE | execute_command | 94 | [054-5.1.33-WFS-CMD-PIN-START-AUTHENTICATE.md](054-5.1.33-WFS-CMD-PIN-START-AUTHENTICATE.md) |
| 5.1.34 WFS_CMD_PIN_AUTHENTICATE | execute_command | 141 | [055-5.1.34-WFS-CMD-PIN-AUTHENTICATE.md](055-5.1.34-WFS-CMD-PIN-AUTHENTICATE.md) |
| 5.1.35 WFS_CMD_PIN_GET_PINBLOCK_EX | execute_command | 113 | [056-5.1.35-WFS-CMD-PIN-GET-PINBLOCK-EX.md](056-5.1.35-WFS-CMD-PIN-GET-PINBLOCK-EX.md) |
| 5.1.36 WFS_CMD_PIN_SYNCHRONIZE_COMMAND | execute_command | 49 | [057-5.1.36-WFS-CMD-PIN-SYNCHRONIZE-COMMAND.md](057-5.1.36-WFS-CMD-PIN-SYNCHRONIZE-COMMAND.md) |
| 5.1.37 WFS_CMD_PIN_CRYPT_340 | execute_command | 176 | [058-5.1.37-WFS-CMD-PIN-CRYPT-340.md](058-5.1.37-WFS-CMD-PIN-CRYPT-340.md) |
| 5.1.38 WFS_CMD_PIN_GET_PINBLOCK_340 | execute_command | 104 | [059-5.1.38-WFS-CMD-PIN-GET-PINBLOCK-340.md](059-5.1.38-WFS-CMD-PIN-GET-PINBLOCK-340.md) |
| 5.1.39 WFS_CMD_PIN_IMPORT_KEY_340 | execute_command | 145 | [060-5.1.39-WFS-CMD-PIN-IMPORT-KEY-340.md](060-5.1.39-WFS-CMD-PIN-IMPORT-KEY-340.md) |
| 5.2 Common commands for Remote Key Loading Schemes | section | 5 | [061-5.2-Common-commands-for-Remote-Key-Loading-Schemes.md](061-5.2-Common-commands-for-Remote-Key-Loading-Schemes.md) |
| 5.2.1 WFS_CMD_PIN_START_KEY_EXCHANGE | execute_command | 35 | [062-5.2.1-WFS-CMD-PIN-START-KEY-EXCHANGE.md](062-5.2.1-WFS-CMD-PIN-START-KEY-EXCHANGE.md) |
| 5.3 Remote Key Loading Using Signatures | section | 5 | [063-5.3-Remote-Key-Loading-Using-Signatures.md](063-5.3-Remote-Key-Loading-Using-Signatures.md) |
| 5.3.1 WFS_CMD_PIN_IMPORT_RSA_PUBLIC_KEY | execute_command | 121 | [064-5.3.1-WFS-CMD-PIN-IMPORT-RSA-PUBLIC-KEY.md](064-5.3.1-WFS-CMD-PIN-IMPORT-RSA-PUBLIC-KEY.md) |
| 5.3.2 WFS_CMD_PIN_EXPORT_RSA_ISSUER_SIGNED_ITEM | execute_command | 80 | [065-5.3.2-WFS-CMD-PIN-EXPORT-RSA-ISSUER-SIGNED-ITEM.md](065-5.3.2-WFS-CMD-PIN-EXPORT-RSA-ISSUER-SIGNED-ITEM.md) |
| 5.3.3 WFS_CMD_PIN_IMPORT_RSA_SIGNED_DES_KEY | execute_command | 156 | [066-5.3.3-WFS-CMD-PIN-IMPORT-RSA-SIGNED-DES-KEY.md](066-5.3.3-WFS-CMD-PIN-IMPORT-RSA-SIGNED-DES-KEY.md) |
| 5.3.4 WFS_CMD_PIN_GENERATE_RSA_KEY_PAIR | execute_command | 65 | [067-5.3.4-WFS-CMD-PIN-GENERATE-RSA-KEY-PAIR.md](067-5.3.4-WFS-CMD-PIN-GENERATE-RSA-KEY-PAIR.md) |
| 5.3.5 WFS_CMD_PIN_EXPORT_RSA_EPP_SIGNED_ITEM | execute_command | 89 | [068-5.3.5-WFS-CMD-PIN-EXPORT-RSA-EPP-SIGNED-ITEM.md](068-5.3.5-WFS-CMD-PIN-EXPORT-RSA-EPP-SIGNED-ITEM.md) |
| 5.4 Remote Key Loading with Certificates | section | 3 | [069-5.4-Remote-Key-Loading-with-Certificates.md](069-5.4-Remote-Key-Loading-with-Certificates.md) |
| 5.4.1 WFS_CMD_PIN_LOAD_CERTIFICATE | execute_command | 42 | [070-5.4.1-WFS-CMD-PIN-LOAD-CERTIFICATE.md](070-5.4.1-WFS-CMD-PIN-LOAD-CERTIFICATE.md) |
| 5.4.2 WFS_CMD_PIN_GET_CERTIFICATE | execute_command | 46 | [071-5.4.2-WFS-CMD-PIN-GET-CERTIFICATE.md](071-5.4.2-WFS-CMD-PIN-GET-CERTIFICATE.md) |
| 5.4.3 WFS_CMD_PIN_REPLACE_CERTIFICATE | execute_command | 44 | [072-5.4.3-WFS-CMD-PIN-REPLACE-CERTIFICATE.md](072-5.4.3-WFS-CMD-PIN-REPLACE-CERTIFICATE.md) |
| 5.4.4 WFS_CMD_PIN_IMPORT_RSA_ENCIPHERED_PKCS7_KEY | execute_command | 114 | [073-5.4.4-WFS-CMD-PIN-IMPORT-RSA-ENCIPHERED-PKCS7-KEY.md](073-5.4.4-WFS-CMD-PIN-IMPORT-RSA-ENCIPHERED-PKCS7-KEY.md) |
| 5.4.5 WFS_CMD_PIN_LOAD_CERTIFICATE_EX | execute_command | 106 | [074-5.4.5-WFS-CMD-PIN-LOAD-CERTIFICATE-EX.md](074-5.4.5-WFS-CMD-PIN-LOAD-CERTIFICATE-EX.md) |
| 5.4.6 WFS_CMD_PIN_IMPORT_RSA_ENCIPHERED_PKCS7_KEY_EX | execute_command | 173 | [075-5.4.6-WFS-CMD-PIN-IMPORT-RSA-ENCIPHERED-PKCS7-KEY-EX.md](075-5.4.6-WFS-CMD-PIN-IMPORT-RSA-ENCIPHERED-PKCS7-KEY-EX.md) |
| 5.5 EMV | section | 4 | [076-5.5-EMV.md](076-5.5-EMV.md) |
| 5.5.1 WFS_CMD_PIN_EMV_IMPORT_PUBLIC_KEY | execute_command | 164 | [077-5.5.1-WFS-CMD-PIN-EMV-IMPORT-PUBLIC-KEY.md](077-5.5.1-WFS-CMD-PIN-EMV-IMPORT-PUBLIC-KEY.md) |
| 5.5.2 WFS_CMD_PIN_DIGEST | execute_command | 32 | [078-5.5.2-WFS-CMD-PIN-DIGEST.md](078-5.5.2-WFS-CMD-PIN-DIGEST.md) |
| 5.6 Entering and Changing a Password | section | 1 | [079-5.6-Entering-and-Changing-a-Password.md](079-5.6-Entering-and-Changing-a-Password.md) |
| 5.6.1 WFS_CMD_PIN_PASSWORD_ENTRY | execute_command | 192 | [080-5.6.1-WFS-CMD-PIN-PASSWORD-ENTRY.md](080-5.6.1-WFS-CMD-PIN-PASSWORD-ENTRY.md) |
| 6. Events | events_section | 1 | [081-6-Events.md](081-6-Events.md) |
| 6.1 WFS_EXEE_PIN_KEY | event | 23 | [082-6.1-WFS-EXEE-PIN-KEY.md](082-6.1-WFS-EXEE-PIN-KEY.md) |
| 6.2 WFS_SRVE_PIN_INITIALIZED | event | 12 | [083-6.2-WFS-SRVE-PIN-INITIALIZED.md](083-6.2-WFS-SRVE-PIN-INITIALIZED.md) |
| 6.3 WFS_SRVE_PIN_ILLEGAL_KEY_ACCESS | event | 28 | [084-6.3-WFS-SRVE-PIN-ILLEGAL-KEY-ACCESS.md](084-6.3-WFS-SRVE-PIN-ILLEGAL-KEY-ACCESS.md) |
| 6.4 WFS_SRVE_PIN_OPT_REQUIRED | event | 23 | [085-6.4-WFS-SRVE-PIN-OPT-REQUIRED.md](085-6.4-WFS-SRVE-PIN-OPT-REQUIRED.md) |
| 6.5 WFS_SRVE_PIN_CERTIFICATE_CHANGE | event | 13 | [086-6.5-WFS-SRVE-PIN-CERTIFICATE-CHANGE.md](086-6.5-WFS-SRVE-PIN-CERTIFICATE-CHANGE.md) |
| 6.6 WFS_SRVE_PIN_HSM_TDATA_CHANGED | event | 16 | [087-6.6-WFS-SRVE-PIN-HSM-TDATA-CHANGED.md](087-6.6-WFS-SRVE-PIN-HSM-TDATA-CHANGED.md) |
| 6.7 WFS_SRVE_PIN_HSM_CHANGED | event | 17 | [088-6.7-WFS-SRVE-PIN-HSM-CHANGED.md](088-6.7-WFS-SRVE-PIN-HSM-CHANGED.md) |
| 6.8 WFS_EXEE_PIN_ENTERDATA | event | 8 | [089-6.8-WFS-EXEE-PIN-ENTERDATA.md](089-6.8-WFS-EXEE-PIN-ENTERDATA.md) |
| 6.9 WFS_SRVE_PIN_DEVICEPOSITION | event | 20 | [090-6.9-WFS-SRVE-PIN-DEVICEPOSITION.md](090-6.9-WFS-SRVE-PIN-DEVICEPOSITION.md) |
| 6.10 WFS_SRVE_PIN_POWER_SAVE_CHANGE | event | 15 | [091-6.10-WFS-SRVE-PIN-POWER-SAVE-CHANGE.md](091-6.10-WFS-SRVE-PIN-POWER-SAVE-CHANGE.md) |
| 6.11 WFS_EXEE_PIN_LAYOUT | event | 10 | [092-6.11-WFS-EXEE-PIN-LAYOUT.md](092-6.11-WFS-EXEE-PIN-LAYOUT.md) |
| 6.12 WFS_EXEE_PIN_DUKPT_KSN | event | 17 | [093-6.12-WFS-EXEE-PIN-DUKPT-KSN.md](093-6.12-WFS-EXEE-PIN-DUKPT-KSN.md) |
| 6.13 WFS_SRVE_PIN_PASSWORD_CLEARED | event | 15 | [094-6.13-WFS-SRVE-PIN-PASSWORD-CLEARED.md](094-6.13-WFS-SRVE-PIN-PASSWORD-CLEARED.md) |
| 7. C - Header File | header | 1368 | [095-7-C-Header-File.md](095-7-C-Header-File.md) |
| 8. Appendix-A | appendix | 69 | [096-8-Appendix-A.md](096-8-Appendix-A.md) |
| 8.1 Remote Key Loading Using Signatures | section | 1 | [097-8.1-Remote-Key-Loading-Using-Signatures.md](097-8.1-Remote-Key-Loading-Using-Signatures.md) |
| 8.1.1 RSA Data Authentication and Digital Signatures | section | 41 | [098-8.1.1-RSA-Data-Authentication-and-Digital-Signatures.md](098-8.1.1-RSA-Data-Authentication-and-Digital-Signatures.md) |
| 8.1.2 RSA Secure Key Exchange using Digital Signatures | section | 92 | [099-8.1.2-RSA-Secure-Key-Exchange-using-Digital-Signatures.md](099-8.1.2-RSA-Secure-Key-Exchange-using-Digital-Signatures.md) |
| 8.1.3 Initialization Phase – Signature Issuer and ATM PIN | section | 17 | [100-8.1.3-Initialization-Phase-Signature-Issuer-and-ATM-PIN.md](100-8.1.3-Initialization-Phase-Signature-Issuer-and-ATM-PIN.md) |
| 8.1.4 Initialization Phase – Signature Issuer and Host | section | 14 | [101-8.1.4-Initialization-Phase-Signature-Issuer-and-Host.md](101-8.1.4-Initialization-Phase-Signature-Issuer-and-Host.md) |
| 8.1.5 Key Exchange – Host and ATM PIN | section | 37 | [102-8.1.5-Key-Exchange-Host-and-ATM-PIN.md](102-8.1.5-Key-Exchange-Host-and-ATM-PIN.md) |
| 8.1.6 Key Exchange (with random number) – Host and ATM PIN | section | 75 | [103-8.1.6-Key-Exchange-with-random-number-Host-and-ATM-PIN.md](103-8.1.6-Key-Exchange-with-random-number-Host-and-ATM-PIN.md) |
| 8.1.7 Enhanced RKL, Key Exchange (with random number) – Host and ATM PIN | section | 70 | [104-8.1.7-Enhanced-RKL-Key-Exchange-with-random-number-Host-and-ATM-PI.md](104-8.1.7-Enhanced-RKL-Key-Exchange-with-random-number-Host-and-ATM-PI.md) |
| 8.1.8 Default Keys and Security Item loaded during manufacture | section | 28 | [105-8.1.8-Default-Keys-and-Security-Item-loaded-during-manufacture.md](105-8.1.8-Default-Keys-and-Security-Item-loaded-during-manufacture.md) |
| 8.2 Remote Key Loading Using Certificates | section | 4 | [106-8.2-Remote-Key-Loading-Using-Certificates.md](106-8.2-Remote-Key-Loading-Using-Certificates.md) |
| 8.2.1 Certificate Exchange and Authentication | section | 38 | [107-8.2.1-Certificate-Exchange-and-Authentication.md](107-8.2.1-Certificate-Exchange-and-Authentication.md) |
| 8.2.2 Remote Key Exchange | section | 69 | [108-8.2.2-Remote-Key-Exchange.md](108-8.2.2-Remote-Key-Exchange.md) |
| 8.2.3 Replace Certificate | section | 24 | [109-8.2.3-Replace-Certificate.md](109-8.2.3-Replace-Certificate.md) |
| 8.2.4 Primary and Secondary Certificates | section | 15 | [110-8.2.4-Primary-and-Secondary-Certificates.md](110-8.2.4-Primary-and-Secondary-Certificates.md) |
| 8.2.5 TR34 BIND To Host | section | 17 | [111-8.2.5-TR34-BIND-To-Host.md](111-8.2.5-TR34-BIND-To-Host.md) |
| 8.2.6 TR34 Key Transport | section | 35 | [112-8.2.6-TR34-Key-Transport.md](112-8.2.6-TR34-Key-Transport.md) |
| 8.2.7 TR34 REBIND To New Host | section | 17 | [113-8.2.7-TR34-REBIND-To-New-Host.md](113-8.2.7-TR34-REBIND-To-New-Host.md) |
| 8.2.8 TR34 Force REBIND To New Host | section | 20 | [114-8.2.8-TR34-Force-REBIND-To-New-Host.md](114-8.2.8-TR34-Force-REBIND-To-New-Host.md) |
| 8.2.9 TR34 UNBIND From Host | section | 20 | [115-8.2.9-TR34-UNBIND-From-Host.md](115-8.2.9-TR34-UNBIND-From-Host.md) |
| 8.2.10 TR34 Force UNBIND From Host | section | 22 | [116-8.2.10-TR34-Force-UNBIND-From-Host.md](116-8.2.10-TR34-Force-UNBIND-From-Host.md) |
| 8.3 German ZKA GeldKarte (Deutsche Kreditwirtschaft) | section | 5 | [117-8.3-German-ZKA-GeldKarte-Deutsche-Kreditwirtschaft.md](117-8.3-German-ZKA-GeldKarte-Deutsche-Kreditwirtschaft.md) |
| 8.3.1 How to use the SECURE_MSG commands | section | 25 | [118-8.3.1-How-to-use-the-SECURE-MSG-commands.md](118-8.3.1-How-to-use-the-SECURE-MSG-commands.md) |
| 8.3.2 Protocol WFS_PIN_PROTISOAS | section | 28 | [119-8.3.2-Protocol-WFS-PIN-PROTISOAS.md](119-8.3.2-Protocol-WFS-PIN-PROTISOAS.md) |
| 8.3.3 Protocol WFS_PIN_PROTISOLZ | section | 28 | [120-8.3.3-Protocol-WFS-PIN-PROTISOLZ.md](120-8.3.3-Protocol-WFS-PIN-PROTISOLZ.md) |
| 8.3.4 Protocol WFS_PIN_PROTISOPS | section | 9 | [121-8.3.4-Protocol-WFS-PIN-PROTISOPS.md](121-8.3.4-Protocol-WFS-PIN-PROTISOPS.md) |
| 8.3.5 Protocol WFS_PIN_PROTCHIPZKA | section | 13 | [122-8.3.5-Protocol-WFS-PIN-PROTCHIPZKA.md](122-8.3.5-Protocol-WFS-PIN-PROTCHIPZKA.md) |
| 8.3.6 Protocol WFS_PIN_PROTRAWDATA | section | 10 | [123-8.3.6-Protocol-WFS-PIN-PROTRAWDATA.md](123-8.3.6-Protocol-WFS-PIN-PROTRAWDATA.md) |
| 8.3.7 Protocol WFS_PIN_PROTPBM | section | 23 | [124-8.3.7-Protocol-WFS-PIN-PROTPBM.md](124-8.3.7-Protocol-WFS-PIN-PROTPBM.md) |
| 8.3.8 Protocol WFS_PIN_PROTHSMLDI | section | 23 | [125-8.3.8-Protocol-WFS-PIN-PROTHSMLDI.md](125-8.3.8-Protocol-WFS-PIN-PROTHSMLDI.md) |
| 8.3.9 Protocol WFS_PIN_PROTGENAS | section | 175 | [126-8.3.9-Protocol-WFS-PIN-PROTGENAS.md](126-8.3.9-Protocol-WFS-PIN-PROTGENAS.md) |
| 8.3.10 Protocol WFS_PIN_PROTCHIPINCHG | section | 17 | [127-8.3.10-Protocol-WFS-PIN-PROTCHIPINCHG.md](127-8.3.10-Protocol-WFS-PIN-PROTCHIPINCHG.md) |
| 8.3.11 Protocol WFS_PIN_PROTPINCMP | section | 85 | [128-8.3.11-Protocol-WFS-PIN-PROTPINCMP.md](128-8.3.11-Protocol-WFS-PIN-PROTPINCMP.md) |
| 8.3.12 Protocol WFS_PIN_PROTISOPINCHG | section | 27 | [129-8.3.12-Protocol-WFS-PIN-PROTISOPINCHG.md](129-8.3.12-Protocol-WFS-PIN-PROTISOPINCHG.md) |
| 8.3.13 Command Sequence | section | 550 | [130-8.3.13-Command-Sequence.md](130-8.3.13-Command-Sequence.md) |
| 8.4 EMV Support | section | 9 | [131-8.4-EMV-Support.md](131-8.4-EMV-Support.md) |
| 8.4.1 Keys loading | section | 58 | [132-8.4.1-Keys-loading.md](132-8.4.1-Keys-loading.md) |
| 8.4.2 PIN Block Management | section | 10 | [133-8.4.2-PIN-Block-Management.md](133-8.4.2-PIN-Block-Management.md) |
| 8.4.3 SHA-1 Digest | section | 8 | [134-8.4.3-SHA-1-Digest.md](134-8.4.3-SHA-1-Digest.md) |
| 8.5 French Cartes Bancaires | section | 11 | [135-8.5-French-Cartes-Bancaires.md](135-8.5-French-Cartes-Bancaires.md) |
| 8.5.1 Data Structure for WFS_CMD_PIN_ENC_IO | execute_command | 84 | [136-8.5.1-Data-Structure-for-WFS-CMD-PIN-ENC-IO.md](136-8.5.1-Data-Structure-for-WFS-CMD-PIN-ENC-IO.md) |
| 8.5.2 Command Sequence | section | 73 | [137-8.5.2-Command-Sequence.md](137-8.5.2-Command-Sequence.md) |
| 8.6 Secure Key Entry | section | 3 | [138-8.6-Secure-Key-Entry.md](138-8.6-Secure-Key-Entry.md) |
| 8.6.1 Keyboard Layout | section | 182 | [139-8.6.1-Keyboard-Layout.md](139-8.6.1-Keyboard-Layout.md) |
| WFS_CMD_PIN_IMPORT_KEY | execute_command | 98 | [140-WFS-CMD-PIN-IMPORT-KEY.md](140-WFS-CMD-PIN-IMPORT-KEY.md) |
| 8.7 WFS_PIN_USERESTRICTEDKEYENCKEY key usage | api_function | 3 | [141-8.7-WFS-PIN-USERESTRICTEDKEYENCKEY-key-usage.md](141-8.7-WFS-PIN-USERESTRICTEDKEYENCKEY-key-usage.md) |
| 8.7.1 Command Usage | section | 114 | [142-8.7.1-Command-Usage.md](142-8.7.1-Command-Usage.md) |
| 8.8 WFS_CMD_PIN_IMPORT_KEY_340 command Input/Output Parameters | execute_command | 70 | [143-8.8-WFS-CMD-PIN-IMPORT-KEY-340-command-Input-Output-Parameters.md](143-8.8-WFS-CMD-PIN-IMPORT-KEY-340-command-Input-Output-Parameters.md) |
| 8.8.2 Importing a 16-byte DES key for PIN encryption with a key check value in the input | section | 52 | [144-8.8.2-Importing-a-16-byte-DES-key-for-PIN-encryption-with-a-key-ch.md](144-8.8.2-Importing-a-16-byte-DES-key-for-PIN-encryption-with-a-key-ch.md) |
| 8.8.3 Importing a 16-byte DES key for MACing (MAC Algorithm 3) | section | 49 | [145-8.8.3-Importing-a-16-byte-DES-key-for-MACing-MAC-Algorithm-3.md](145-8.8.3-Importing-a-16-byte-DES-key-for-MACing-MAC-Algorithm-3.md) |
| 8.8.4 Importing a 2048-bit Host RSA public key | section | 55 | [146-8.8.4-Importing-a-2048-bit-Host-RSA-public-key.md](146-8.8.4-Importing-a-2048-bit-Host-RSA-public-key.md) |
| 8.8.5 Importing a 24-byte DES symmetric data encryption key via X9.143 keyblock | section | 37 | [147-8.8.5-Importing-a-24-byte-DES-symmetric-data-encryption-key-via-X9.md](147-8.8.5-Importing-a-24-byte-DES-symmetric-data-encryption-key-via-X9.md) |
| 8.9 Entering passwords using the WFS_CMD_PIN_PASSWORD_ENTRY command. | execute_command | 13 | [148-8.9-Entering-passwords-using-the-WFS-CMD-PIN-PASSWORD-ENTRY-comm.md](148-8.9-Entering-passwords-using-the-WFS-CMD-PIN-PASSWORD-ENTRY-comm.md) |
| 8.9.1 Entering passwords individually to allow secure key parts to be loaded | section | 3 | [149-8.9.1-Entering-passwords-individually-to-allow-secure-key-parts-to.md](149-8.9.1-Entering-passwords-individually-to-allow-secure-key-parts-to.md) |
| 8.9.2 Entering and changing a password | section | 9 | [150-8.9.2-Entering-and-changing-a-password.md](150-8.9.2-Entering-and-changing-a-password.md) |
| 9. Appendix-B (Country Specific WFS_CMD_PIN_ENC_IO protocols) | execute_command | 2 | [151-9-Appendix-B-Country-Specific-WFS-CMD-PIN-ENC-IO-protocols.md](151-9-Appendix-B-Country-Specific-WFS-CMD-PIN-ENC-IO-protocols.md) |
| 9.1 Luxemburg Protocol | section | 94 | [152-9.1-Luxemburg-Protocol.md](152-9.1-Luxemburg-Protocol.md) |
| 9.1.1 WFS_CMD_ENC_IO_LUX_LOAD_APPKEY | execute_command | 82 | [153-9.1.1-WFS-CMD-ENC-IO-LUX-LOAD-APPKEY.md](153-9.1.1-WFS-CMD-ENC-IO-LUX-LOAD-APPKEY.md) |
| 9.1.2 WFS_CMD_ENC_IO_LUX_GENERATE_MAC | execute_command | 46 | [154-9.1.2-WFS-CMD-ENC-IO-LUX-GENERATE-MAC.md](154-9.1.2-WFS-CMD-ENC-IO-LUX-GENERATE-MAC.md) |
| 9.1.3 WFS_CMD_ENC_IO_LUX_CHECK_MAC | execute_command | 49 | [155-9.1.3-WFS-CMD-ENC-IO-LUX-CHECK-MAC.md](155-9.1.3-WFS-CMD-ENC-IO-LUX-CHECK-MAC.md) |
| 9.1.4 WFS_CMD_ENC_IO_LUX_BUILD_PINBLOCK | execute_command | 48 | [156-9.1.4-WFS-CMD-ENC-IO-LUX-BUILD-PINBLOCK.md](156-9.1.4-WFS-CMD-ENC-IO-LUX-BUILD-PINBLOCK.md) |
| 9.1.5 WFS_CMD_ENC_IO_LUX_DECRYPT_TDES | execute_command | 50 | [157-9.1.5-WFS-CMD-ENC-IO-LUX-DECRYPT-TDES.md](157-9.1.5-WFS-CMD-ENC-IO-LUX-DECRYPT-TDES.md) |
| 9.1.6 WFS_CMD_ENC_IO_LUX_ENCRYPT_TDES | execute_command | 51 | [158-9.1.6-WFS-CMD-ENC-IO-LUX-ENCRYPT-TDES.md](158-9.1.6-WFS-CMD-ENC-IO-LUX-ENCRYPT-TDES.md) |
| 9.1.7 Luxemburg-specific Header File | header | 141 | [159-9.1.7-Luxemburg-specific-Header-File.md](159-9.1.7-Luxemburg-specific-Header-File.md) |
| 9.2 China Protocol | section | 102 | [160-9.2-China-Protocol.md](160-9.2-China-Protocol.md) |
| 9.2.1 WFS_CMD_ENC_IO_CHN_DIGEST | execute_command | 43 | [161-9.2.1-WFS-CMD-ENC-IO-CHN-DIGEST.md](161-9.2.1-WFS-CMD-ENC-IO-CHN-DIGEST.md) |
| 9.2.2 WFS_CMD_ENC_IO_CHN_SET_SM2_PARAM | execute_command | 62 | [162-9.2.2-WFS-CMD-ENC-IO-CHN-SET-SM2-PARAM.md](162-9.2.2-WFS-CMD-ENC-IO-CHN-SET-SM2-PARAM.md) |
| 9.2.3 WFS_CMD_ENC_IO_CHN_IMPORT_SM2_PUBLIC_KEY | execute_command | 109 | [163-9.2.3-WFS-CMD-ENC-IO-CHN-IMPORT-SM2-PUBLIC-KEY.md](163-9.2.3-WFS-CMD-ENC-IO-CHN-IMPORT-SM2-PUBLIC-KEY.md) |
| 9.2.4 WFS_CMD_ENC_IO_CHN_SIGN | execute_command | 62 | [164-9.2.4-WFS-CMD-ENC-IO-CHN-SIGN.md](164-9.2.4-WFS-CMD-ENC-IO-CHN-SIGN.md) |
| 9.2.5 WFS_CMD_ENC_IO_CHN_VERIFY | execute_command | 35 | [165-9.2.5-WFS-CMD-ENC-IO-CHN-VERIFY.md](165-9.2.5-WFS-CMD-ENC-IO-CHN-VERIFY.md) |
| 9.2.6 WFS_CMD_ENC_IO_CHN_EXPORT_SM2_ISSUER_SIGNED_ITEM | execute_command | 76 | [166-9.2.6-WFS-CMD-ENC-IO-CHN-EXPORT-SM2-ISSUER-SIGNED-ITEM.md](166-9.2.6-WFS-CMD-ENC-IO-CHN-EXPORT-SM2-ISSUER-SIGNED-ITEM.md) |
| 9.2.7 WFS_CMD_ENC_IO_CHN_GENERATE_SM2_KEY_PAIR | execute_command | 56 | [167-9.2.7-WFS-CMD-ENC-IO-CHN-GENERATE-SM2-KEY-PAIR.md](167-9.2.7-WFS-CMD-ENC-IO-CHN-GENERATE-SM2-KEY-PAIR.md) |
| 9.2.8 WFS_CMD_ENC_IO_CHN_EXPORT_SM2_EPP_SIGNED_ITEM | execute_command | 90 | [168-9.2.8-WFS-CMD-ENC-IO-CHN-EXPORT-SM2-EPP-SIGNED-ITEM.md](168-9.2.8-WFS-CMD-ENC-IO-CHN-EXPORT-SM2-EPP-SIGNED-ITEM.md) |
| 9.2.9 WFS_CMD_ENC_IO_CHN_IMPORT_SM2_SIGNED_SM4_KEY | execute_command | 128 | [169-9.2.9-WFS-CMD-ENC-IO-CHN-IMPORT-SM2-SIGNED-SM4-KEY.md](169-9.2.9-WFS-CMD-ENC-IO-CHN-IMPORT-SM2-SIGNED-SM4-KEY.md) |
| 9.2.10 China-specific Header File | header | 248 | [170-9.2.10-China-specific-Header-File.md](170-9.2.10-China-specific-Header-File.md) |
| 10. Appendix–C (Standardized lpszExtra fields) | appendix | 4 | [171-10-Appendix-C-Standardized-lpszExtra-fields.md](171-10-Appendix-C-Standardized-lpszExtra-fields.md) |
| 10.1 WFS_INF_PIN_STATUS | info_command | 36 | [172-10.1-WFS-INF-PIN-STATUS.md](172-10.1-WFS-INF-PIN-STATUS.md) |
| 10.2 WFS_INF_PIN_CAPABILITIES | info_command | 134 | [173-10.2-WFS-INF-PIN-CAPABILITIES.md](173-10.2-WFS-INF-PIN-CAPABILITIES.md) |
| 11. Appendix–D (X9.143 Key Use) | appendix | 148 | [174-11-Appendix-D-X9-143-Key-Use.md](174-11-Appendix-D-X9-143-Key-Use.md) |
| 12. Appendix-E (DUKPT) | appendix | 8 | [175-12-Appendix-E-DUKPT.md](175-12-Appendix-E-DUKPT.md) |
| 12.1 Default Key Name | section | 10 | [176-12.1-Default-Key-Name.md](176-12.1-Default-Key-Name.md) |
| 13. Appendix-F Diagram Source | appendix | 6 | [177-13-Appendix-F-Diagram-Source.md](177-13-Appendix-F-Diagram-Source.md) |
