# XfsIdc - IDC

Identification Card Device Class Interface

Source: `XfsIdc.pdf` | sections: 59 | split level: 5

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 93 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 148 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1. Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.50 | section | 14 | [003-1.1-Background-to-Release-3-50.md](003-1.1-Background-to-Release-3-50.md) |
| 1.2 XFS Service-Specific Programming | section | 41 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2. Identification Card Readers and Writers | section | 54 | [005-2-Identification-Card-Readers-and-Writers.md](005-2-Identification-Card-Readers-and-Writers.md) |
| 2.1 Support for EMV Intelligent Contactless Card Readers | section | 46 | [006-2.1-Support-for-EMV-Intelligent-Contactless-Card-Readers.md](006-2.1-Support-for-EMV-Intelligent-Contactless-Card-Readers.md) |
| 3. References | section | 9 | [007-3-References.md](007-3-References.md) |
| 4. Info Commands | info_commands_section | 1 | [008-4-Info-Commands.md](008-4-Info-Commands.md) |
| 4.1 WFS_INF_IDC_STATUS | info_command | 318 | [009-4.1-WFS-INF-IDC-STATUS.md](009-4.1-WFS-INF-IDC-STATUS.md) |
| 4.2 WFS_INF_IDC_CAPABILITIES | info_command | 284 | [010-4.2-WFS-INF-IDC-CAPABILITIES.md](010-4.2-WFS-INF-IDC-CAPABILITIES.md) |
| 4.3 WFS_INF_IDC_FORM_LIST | info_command | 12 | [011-4.3-WFS-INF-IDC-FORM-LIST.md](011-4.3-WFS-INF-IDC-FORM-LIST.md) |
| 4.4 WFS_INF_IDC_QUERY_FORM | info_command | 79 | [012-4.4-WFS-INF-IDC-QUERY-FORM.md](012-4.4-WFS-INF-IDC-QUERY-FORM.md) |
| 4.5 WFS_INF_IDC_QUERY_IFM_IDENTIFIER | info_command | 38 | [013-4.5-WFS-INF-IDC-QUERY-IFM-IDENTIFIER.md](013-4.5-WFS-INF-IDC-QUERY-IFM-IDENTIFIER.md) |
| 4.6 WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS | info_command | 26 | [014-4.6-WFS-INF-IDC-EMVCLESS-QUERY-APPLICATIONS.md](014-4.6-WFS-INF-IDC-EMVCLESS-QUERY-APPLICATIONS.md) |
| 5. Execute Commands | execute_commands_section | 1 | [015-5-Execute-Commands.md](015-5-Execute-Commands.md) |
| 5.1 WFS_CMD_IDC_READ_TRACK | execute_command | 102 | [016-5.1-WFS-CMD-IDC-READ-TRACK.md](016-5.1-WFS-CMD-IDC-READ-TRACK.md) |
| 5.2 WFS_CMD_IDC_WRITE_TRACK | execute_command | 103 | [017-5.2-WFS-CMD-IDC-WRITE-TRACK.md](017-5.2-WFS-CMD-IDC-WRITE-TRACK.md) |
| 5.3 WFS_CMD_IDC_EJECT_CARD | execute_command | 64 | [018-5.3-WFS-CMD-IDC-EJECT-CARD.md](018-5.3-WFS-CMD-IDC-EJECT-CARD.md) |
| 5.4 WFS_CMD_IDC_RETAIN_CARD | execute_command | 51 | [019-5.4-WFS-CMD-IDC-RETAIN-CARD.md](019-5.4-WFS-CMD-IDC-RETAIN-CARD.md) |
| 5.5 WFS_CMD_IDC_RESET_COUNT | execute_command | 17 | [020-5.5-WFS-CMD-IDC-RESET-COUNT.md](020-5.5-WFS-CMD-IDC-RESET-COUNT.md) |
| 5.6 WFS_CMD_IDC_SETKEY | execute_command | 24 | [021-5.6-WFS-CMD-IDC-SETKEY.md](021-5.6-WFS-CMD-IDC-SETKEY.md) |
| 5.7 WFS_CMD_IDC_READ_RAW_DATA | execute_command | 231 | [022-5.7-WFS-CMD-IDC-READ-RAW-DATA.md](022-5.7-WFS-CMD-IDC-READ-RAW-DATA.md) |
| 5.8 WFS_CMD_IDC_WRITE_RAW_DATA | execute_command | 107 | [023-5.8-WFS-CMD-IDC-WRITE-RAW-DATA.md](023-5.8-WFS-CMD-IDC-WRITE-RAW-DATA.md) |
| 5.9 WFS_CMD_IDC_CHIP_IO | execute_command | 74 | [024-5.9-WFS-CMD-IDC-CHIP-IO.md](024-5.9-WFS-CMD-IDC-CHIP-IO.md) |
| 5.10 WFS_CMD_IDC_RESET | execute_command | 46 | [025-5.10-WFS-CMD-IDC-RESET.md](025-5.10-WFS-CMD-IDC-RESET.md) |
| 5.11 WFS_CMD_IDC_CHIP_POWER | execute_command | 51 | [026-5.11-WFS-CMD-IDC-CHIP-POWER.md](026-5.11-WFS-CMD-IDC-CHIP-POWER.md) |
| 5.12 WFS_CMD_IDC_PARSE_DATA | execute_command | 55 | [027-5.12-WFS-CMD-IDC-PARSE-DATA.md](027-5.12-WFS-CMD-IDC-PARSE-DATA.md) |
| 5.13 WFS_CMD_IDC_SET_GUIDANCE_LIGHT | execute_command | 78 | [028-5.13-WFS-CMD-IDC-SET-GUIDANCE-LIGHT.md](028-5.13-WFS-CMD-IDC-SET-GUIDANCE-LIGHT.md) |
| 5.14 WFS_CMD_IDC_POWER_SAVE_CONTROL | execute_command | 37 | [029-5.14-WFS-CMD-IDC-POWER-SAVE-CONTROL.md](029-5.14-WFS-CMD-IDC-POWER-SAVE-CONTROL.md) |
| 5.15 WFS_CMD_IDC_PARK_CARD | execute_command | 57 | [030-5.15-WFS-CMD-IDC-PARK-CARD.md](030-5.15-WFS-CMD-IDC-PARK-CARD.md) |
| 5.16 WFS_CMD_IDC_EMVCLESS_CONFIGURE | execute_command | 108 | [031-5.16-WFS-CMD-IDC-EMVCLESS-CONFIGURE.md](031-5.16-WFS-CMD-IDC-EMVCLESS-CONFIGURE.md) |
| 5.17 WFS_CMD_IDC_EMVCLESS_PERFORM_TRANSACTION | execute_command | 293 | [032-5.17-WFS-CMD-IDC-EMVCLESS-PERFORM-TRANSACTION.md](032-5.17-WFS-CMD-IDC-EMVCLESS-PERFORM-TRANSACTION.md) |
| 5.18 WFS_CMD_IDC_EMVCLESS_ISSUERUPDATE | execute_command | 103 | [033-5.18-WFS-CMD-IDC-EMVCLESS-ISSUERUPDATE.md](033-5.18-WFS-CMD-IDC-EMVCLESS-ISSUERUPDATE.md) |
| 5.19 WFS_CMD_IDC_SYNCHRONIZE_COMMAND | execute_command | 49 | [034-5.19-WFS-CMD-IDC-SYNCHRONIZE-COMMAND.md](034-5.19-WFS-CMD-IDC-SYNCHRONIZE-COMMAND.md) |
| 6. Events | events_section | 1 | [035-6-Events.md](035-6-Events.md) |
| 6.1 WFS_EXEE_IDC_INVALIDTRACKDATA | event | 25 | [036-6.1-WFS-EXEE-IDC-INVALIDTRACKDATA.md](036-6.1-WFS-EXEE-IDC-INVALIDTRACKDATA.md) |
| 6.2 WFS_EXEE_IDC_MEDIAINSERTED | event | 7 | [037-6.2-WFS-EXEE-IDC-MEDIAINSERTED.md](037-6.2-WFS-EXEE-IDC-MEDIAINSERTED.md) |
| 6.3 WFS_SRVE_IDC_MEDIAREMOVED | event | 10 | [038-6.3-WFS-SRVE-IDC-MEDIAREMOVED.md](038-6.3-WFS-SRVE-IDC-MEDIAREMOVED.md) |
| 6.4 WFS_EXEE_IDC_MEDIARETAINED | event | 7 | [039-6.4-WFS-EXEE-IDC-MEDIARETAINED.md](039-6.4-WFS-EXEE-IDC-MEDIARETAINED.md) |
| 6.5 WFS_EXEE_IDC_INVALIDMEDIA | event | 8 | [040-6.5-WFS-EXEE-IDC-INVALIDMEDIA.md](040-6.5-WFS-EXEE-IDC-INVALIDMEDIA.md) |
| 6.6 WFS_SRVE_IDC_CARDACTION | event | 27 | [041-6.6-WFS-SRVE-IDC-CARDACTION.md](041-6.6-WFS-SRVE-IDC-CARDACTION.md) |
| 6.7 WFS_USRE_IDC_RETAINBINTHRESHOLD | event | 16 | [042-6.7-WFS-USRE-IDC-RETAINBINTHRESHOLD.md](042-6.7-WFS-USRE-IDC-RETAINBINTHRESHOLD.md) |
| 6.8 WFS_SRVE_IDC_MEDIADETECTED | event | 18 | [043-6.8-WFS-SRVE-IDC-MEDIADETECTED.md](043-6.8-WFS-SRVE-IDC-MEDIADETECTED.md) |
| 6.9 WFS_SRVE_IDC_RETAINBINREMOVED | event | 7 | [044-6.9-WFS-SRVE-IDC-RETAINBINREMOVED.md](044-6.9-WFS-SRVE-IDC-RETAINBINREMOVED.md) |
| 6.10 WFS_SRVE_IDC_RETAINBININSERTED | event | 7 | [045-6.10-WFS-SRVE-IDC-RETAINBININSERTED.md](045-6.10-WFS-SRVE-IDC-RETAINBININSERTED.md) |
| 6.11 WFS_EXEE_IDC_INSERTCARD | event | 8 | [046-6.11-WFS-EXEE-IDC-INSERTCARD.md](046-6.11-WFS-EXEE-IDC-INSERTCARD.md) |
| 6.12 WFS_SRVE_IDC_DEVICEPOSITION | event | 20 | [047-6.12-WFS-SRVE-IDC-DEVICEPOSITION.md](047-6.12-WFS-SRVE-IDC-DEVICEPOSITION.md) |
| 6.13 WFS_SRVE_IDC_POWER_SAVE_CHANGE | event | 15 | [048-6.13-WFS-SRVE-IDC-POWER-SAVE-CHANGE.md](048-6.13-WFS-SRVE-IDC-POWER-SAVE-CHANGE.md) |
| 6.14 WFS_EXEE_IDC_TRACKDETECTED | event | 22 | [049-6.14-WFS-EXEE-IDC-TRACKDETECTED.md](049-6.14-WFS-EXEE-IDC-TRACKDETECTED.md) |
| 6.15 WFS_EXEE_IDC_EMVCLESSREADSTATUS | event | 23 | [050-6.15-WFS-EXEE-IDC-EMVCLESSREADSTATUS.md](050-6.15-WFS-EXEE-IDC-EMVCLESSREADSTATUS.md) |
| 6.16 WFS_SRVE_IDC_MEDIARETAINED | event | 9 | [051-6.16-WFS-SRVE-IDC-MEDIARETAINED.md](051-6.16-WFS-SRVE-IDC-MEDIARETAINED.md) |
| 7. Form Description | section | 148 | [052-7-Form-Description.md](052-7-Form-Description.md) |
| 8. C-Header file | header | 609 | [053-8-C-Header-file.md](053-8-C-Header-file.md) |
| 9. Intelligent Contactless Card Sequence Diagrams | section | 5 | [054-9-Intelligent-Contactless-Card-Sequence-Diagrams.md](054-9-Intelligent-Contactless-Card-Sequence-Diagrams.md) |
| 9.1 Single Tap Transaction Without Issuer Update Processing | section | 5 | [055-9.1-Single-Tap-Transaction-Without-Issuer-Update-Processing.md](055-9.1-Single-Tap-Transaction-Without-Issuer-Update-Processing.md) |
| 9.2 Double Tap Transaction With Issuer Update Processing | section | 5 | [056-9.2-Double-Tap-Transaction-With-Issuer-Update-Processing.md](056-9.2-Double-Tap-Transaction-With-Issuer-Update-Processing.md) |
| 9.3 Card Removed Before Completion | section | 5 | [057-9.3-Card-Removed-Before-Completion.md](057-9.3-Card-Removed-Before-Completion.md) |
| Appendix A. Diagram Source | appendix | 6 | [058-Appendix-A-Diagram-Source.md](058-Appendix-A-Diagram-Source.md) |
