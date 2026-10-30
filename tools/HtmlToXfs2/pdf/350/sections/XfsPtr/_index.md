# XfsPtr - PTR

Printer Device Class Interface

Source: `XfsPtr.pdf` | sections: 76 | split level: 5

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 117 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 148 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1. Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.50 | section | 14 | [003-1.1-Background-to-Release-3-50.md](003-1.1-Background-to-Release-3-50.md) |
| 1.2 XFS Service-Specific Programming | section | 41 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2. Banking Printers | section | 44 | [005-2-Banking-Printers.md](005-2-Banking-Printers.md) |
| 3. Banking Printer Types | section | 42 | [006-3-Banking-Printer-Types.md](006-3-Banking-Printer-Types.md) |
| 4. Forms Model | section | 42 | [007-4-Forms-Model.md](007-4-Forms-Model.md) |
| 5. References | section | 8 | [008-5-References.md](008-5-References.md) |
| 6. Command Overview | section | 38 | [009-6-Command-Overview.md](009-6-Command-Overview.md) |
| 7. Info Commands | info_commands_section | 1 | [010-7-Info-Commands.md](010-7-Info-Commands.md) |
| 7.1 WFS_INF_PTR_STATUS | info_command | 371 | [011-7.1-WFS-INF-PTR-STATUS.md](011-7.1-WFS-INF-PTR-STATUS.md) |
| 7.2 WFS_INF_PTR_CAPABILITIES | info_command | 396 | [012-7.2-WFS-INF-PTR-CAPABILITIES.md](012-7.2-WFS-INF-PTR-CAPABILITIES.md) |
| 7.3 WFS_INF_PTR_FORM_LIST | info_command | 12 | [013-7.3-WFS-INF-PTR-FORM-LIST.md](013-7.3-WFS-INF-PTR-FORM-LIST.md) |
| 7.4 WFS_INF_PTR_MEDIA_LIST | info_command | 12 | [014-7.4-WFS-INF-PTR-MEDIA-LIST.md](014-7.4-WFS-INF-PTR-MEDIA-LIST.md) |
| 7.5 WFS_INF_PTR_QUERY_FORM | info_command | 103 | [015-7.5-WFS-INF-PTR-QUERY-FORM.md](015-7.5-WFS-INF-PTR-QUERY-FORM.md) |
| 7.6 WFS_INF_PTR_QUERY_MEDIA | info_command | 112 | [016-7.6-WFS-INF-PTR-QUERY-MEDIA.md](016-7.6-WFS-INF-PTR-QUERY-MEDIA.md) |
| 7.7 WFS_INF_PTR_QUERY_FIELD | info_command | 125 | [017-7.7-WFS-INF-PTR-QUERY-FIELD.md](017-7.7-WFS-INF-PTR-QUERY-FIELD.md) |
| 7.8 WFS_INF_PTR_CODELINE_MAPPING | info_command | 58 | [018-7.8-WFS-INF-PTR-CODELINE-MAPPING.md](018-7.8-WFS-INF-PTR-CODELINE-MAPPING.md) |
| 8. Execute Commands | execute_commands_section | 1 | [019-8-Execute-Commands.md](019-8-Execute-Commands.md) |
| 8.1 WFS_CMD_PTR_CONTROL_MEDIA | execute_command | 160 | [020-8.1-WFS-CMD-PTR-CONTROL-MEDIA.md](020-8.1-WFS-CMD-PTR-CONTROL-MEDIA.md) |
| 8.2 WFS_CMD_PTR_PRINT_FORM | execute_command | 229 | [021-8.2-WFS-CMD-PTR-PRINT-FORM.md](021-8.2-WFS-CMD-PTR-PRINT-FORM.md) |
| 8.3 WFS_CMD_PTR_READ_FORM | execute_command | 158 | [022-8.3-WFS-CMD-PTR-READ-FORM.md](022-8.3-WFS-CMD-PTR-READ-FORM.md) |
| 8.4 WFS_CMD_PTR_RAW_DATA | execute_command | 94 | [023-8.4-WFS-CMD-PTR-RAW-DATA.md](023-8.4-WFS-CMD-PTR-RAW-DATA.md) |
| 8.5 WFS_CMD_PTR_MEDIA_EXTENTS | execute_command | 67 | [024-8.5-WFS-CMD-PTR-MEDIA-EXTENTS.md](024-8.5-WFS-CMD-PTR-MEDIA-EXTENTS.md) |
| 8.6 WFS_CMD_PTR_RESET_COUNT | execute_command | 24 | [025-8.6-WFS-CMD-PTR-RESET-COUNT.md](025-8.6-WFS-CMD-PTR-RESET-COUNT.md) |
| 8.7 WFS_CMD_PTR_READ_IMAGE | execute_command | 194 | [026-8.7-WFS-CMD-PTR-READ-IMAGE.md](026-8.7-WFS-CMD-PTR-READ-IMAGE.md) |
| 8.8 WFS_CMD_PTR_RESET | execute_command | 65 | [027-8.8-WFS-CMD-PTR-RESET.md](027-8.8-WFS-CMD-PTR-RESET.md) |
| 8.9 WFS_CMD_PTR_RETRACT_MEDIA | execute_command | 42 | [028-8.9-WFS-CMD-PTR-RETRACT-MEDIA.md](028-8.9-WFS-CMD-PTR-RETRACT-MEDIA.md) |
| 8.10 WFS_CMD_PTR_DISPENSE_PAPER | execute_command | 50 | [029-8.10-WFS-CMD-PTR-DISPENSE-PAPER.md](029-8.10-WFS-CMD-PTR-DISPENSE-PAPER.md) |
| 8.11 WFS_CMD_PTR_SET_GUIDANCE_LIGHT | execute_command | 81 | [030-8.11-WFS-CMD-PTR-SET-GUIDANCE-LIGHT.md](030-8.11-WFS-CMD-PTR-SET-GUIDANCE-LIGHT.md) |
| 8.12 WFS_CMD_PTR_PRINT_RAW_FILE | execute_command | 141 | [031-8.12-WFS-CMD-PTR-PRINT-RAW-FILE.md](031-8.12-WFS-CMD-PTR-PRINT-RAW-FILE.md) |
| 8.13 WFS_CMD_PTR_LOAD_DEFINITION | execute_command | 44 | [032-8.13-WFS-CMD-PTR-LOAD-DEFINITION.md](032-8.13-WFS-CMD-PTR-LOAD-DEFINITION.md) |
| 8.14 WFS_CMD_PTR_SUPPLY_REPLENISH | execute_command | 50 | [033-8.14-WFS-CMD-PTR-SUPPLY-REPLENISH.md](033-8.14-WFS-CMD-PTR-SUPPLY-REPLENISH.md) |
| 8.15 WFS_CMD_PTR_POWER_SAVE_CONTROL | execute_command | 37 | [034-8.15-WFS-CMD-PTR-POWER-SAVE-CONTROL.md](034-8.15-WFS-CMD-PTR-POWER-SAVE-CONTROL.md) |
| 8.16 WFS_CMD_PTR_CONTROL_PASSBOOK | execute_command | 47 | [035-8.16-WFS-CMD-PTR-CONTROL-PASSBOOK.md](035-8.16-WFS-CMD-PTR-CONTROL-PASSBOOK.md) |
| 8.17 WFS_CMD_PTR_SET_BLACK_MARK_MODE | execute_command | 24 | [036-8.17-WFS-CMD-PTR-SET-BLACK-MARK-MODE.md](036-8.17-WFS-CMD-PTR-SET-BLACK-MARK-MODE.md) |
| 8.18 WFS_CMD_PTR_SYNCHRONIZE_COMMAND | execute_command | 49 | [037-8.18-WFS-CMD-PTR-SYNCHRONIZE-COMMAND.md](037-8.18-WFS-CMD-PTR-SYNCHRONIZE-COMMAND.md) |
| 9. Events | events_section | 1 | [038-9-Events.md](038-9-Events.md) |
| 9.1 WFS_EXEE_PTR_NOMEDIA | event | 13 | [039-9.1-WFS-EXEE-PTR-NOMEDIA.md](039-9.1-WFS-EXEE-PTR-NOMEDIA.md) |
| 9.2 WFS_EXEE_PTR_MEDIAINSERTED | event | 8 | [040-9.2-WFS-EXEE-PTR-MEDIAINSERTED.md](040-9.2-WFS-EXEE-PTR-MEDIAINSERTED.md) |
| 9.3 WFS_EXEE_PTR_FIELDERROR | event | 39 | [041-9.3-WFS-EXEE-PTR-FIELDERROR.md](041-9.3-WFS-EXEE-PTR-FIELDERROR.md) |
| 9.4 WFS_EXEE_PTR_FIELDWARNING | event | 8 | [042-9.4-WFS-EXEE-PTR-FIELDWARNING.md](042-9.4-WFS-EXEE-PTR-FIELDWARNING.md) |
| 9.5 WFS_USRE_PTR_RETRACTBINTHRESHOLD | event | 21 | [043-9.5-WFS-USRE-PTR-RETRACTBINTHRESHOLD.md](043-9.5-WFS-USRE-PTR-RETRACTBINTHRESHOLD.md) |
| 9.6 WFS_SRVE_PTR_MEDIATAKEN | event | 11 | [044-9.6-WFS-SRVE-PTR-MEDIATAKEN.md](044-9.6-WFS-SRVE-PTR-MEDIATAKEN.md) |
| 9.7 WFS_USRE_PTR_PAPERTHRESHOLD | event | 31 | [045-9.7-WFS-USRE-PTR-PAPERTHRESHOLD.md](045-9.7-WFS-USRE-PTR-PAPERTHRESHOLD.md) |
| 9.8 WFS_USRE_PTR_TONERTHRESHOLD | event | 14 | [046-9.8-WFS-USRE-PTR-TONERTHRESHOLD.md](046-9.8-WFS-USRE-PTR-TONERTHRESHOLD.md) |
| 9.9 WFS_SRVE_PTR_MEDIAINSERTED | event | 9 | [047-9.9-WFS-SRVE-PTR-MEDIAINSERTED.md](047-9.9-WFS-SRVE-PTR-MEDIAINSERTED.md) |
| 9.10 WFS_USRE_PTR_LAMPTHRESHOLD | event | 14 | [048-9.10-WFS-USRE-PTR-LAMPTHRESHOLD.md](048-9.10-WFS-USRE-PTR-LAMPTHRESHOLD.md) |
| 9.11 WFS_USRE_PTR_INKTHRESHOLD | event | 14 | [049-9.11-WFS-USRE-PTR-INKTHRESHOLD.md](049-9.11-WFS-USRE-PTR-INKTHRESHOLD.md) |
| 9.12 WFS_SRVE_PTR_MEDIADETECTED | event | 28 | [050-9.12-WFS-SRVE-PTR-MEDIADETECTED.md](050-9.12-WFS-SRVE-PTR-MEDIADETECTED.md) |
| 9.13 WFS_SRVE_PTR_RETRACTBINSTATUS | event | 19 | [051-9.13-WFS-SRVE-PTR-RETRACTBINSTATUS.md](051-9.13-WFS-SRVE-PTR-RETRACTBINSTATUS.md) |
| 9.14 WFS_EXEE_PTR_MEDIAPRESENTED | event | 17 | [052-9.14-WFS-EXEE-PTR-MEDIAPRESENTED.md](052-9.14-WFS-EXEE-PTR-MEDIAPRESENTED.md) |
| 9.15 WFS_SRVE_PTR_DEFINITIONLOADED | event | 22 | [053-9.15-WFS-SRVE-PTR-DEFINITIONLOADED.md](053-9.15-WFS-SRVE-PTR-DEFINITIONLOADED.md) |
| 9.16 WFS_EXEE_PTR_MEDIAREJECTED | event | 33 | [054-9.16-WFS-EXEE-PTR-MEDIAREJECTED.md](054-9.16-WFS-EXEE-PTR-MEDIAREJECTED.md) |
| 9.17 WFS_SRVE_PTR_MEDIAPRESENTED | event | 18 | [055-9.17-WFS-SRVE-PTR-MEDIAPRESENTED.md](055-9.17-WFS-SRVE-PTR-MEDIAPRESENTED.md) |
| 9.18 WFS_SRVE_PTR_MEDIAAUTORETRACTED | event | 25 | [056-9.18-WFS-SRVE-PTR-MEDIAAUTORETRACTED.md](056-9.18-WFS-SRVE-PTR-MEDIAAUTORETRACTED.md) |
| 9.19 WFS_SRVE_PTR_DEVICEPOSITION | event | 20 | [057-9.19-WFS-SRVE-PTR-DEVICEPOSITION.md](057-9.19-WFS-SRVE-PTR-DEVICEPOSITION.md) |
| 9.20 WFS_SRVE_PTR_POWER_SAVE_CHANGE | event | 15 | [058-9.20-WFS-SRVE-PTR-POWER-SAVE-CHANGE.md](058-9.20-WFS-SRVE-PTR-POWER-SAVE-CHANGE.md) |
| 10. Form, Sub-Form, Field, Frame, Table and Media Definitions | section | 3 | [059-10-Form-Sub-Form-Field-Frame-Table-and-Media-Definitions.md](059-10-Form-Sub-Form-Field-Frame-Table-and-Media-Definitions.md) |
| 10.1 Definition Syntax | section | 35 | [060-10.1-Definition-Syntax.md](060-10.1-Definition-Syntax.md) |
| 10.2 Form and Media Measurements | section | 32 | [061-10.2-Form-and-Media-Measurements.md](061-10.2-Form-and-Media-Measurements.md) |
| 10.3 Form Definition | section | 99 | [062-10.3-Form-Definition.md](062-10.3-Form-Definition.md) |
| 10.4 SubForm Definition | section | 53 | [063-10.4-SubForm-Definition.md](063-10.4-SubForm-Definition.md) |
| 10.5 Field Definition | section | 236 | [064-10.5-Field-Definition.md](064-10.5-Field-Definition.md) |
| 10.6 Frame Definition | section | 172 | [065-10.6-Frame-Definition.md](065-10.6-Frame-Definition.md) |
| Sample 1: Simple framing | section | 97 | [066-Sample-1-Simple-framing.md](066-Sample-1-Simple-framing.md) |
| Sample 2: Framing with title | section | 50 | [067-Sample-2-Framing-with-title.md](067-Sample-2-Framing-with-title.md) |
| Sample 3: Framing with filled interior | section | 38 | [068-Sample-3-Framing-with-filled-interior.md](068-Sample-3-Framing-with-filled-interior.md) |
| Sample 4: Repeated Framing | section | 34 | [069-Sample-4-Repeated-Framing.md](069-Sample-4-Repeated-Framing.md) |
| 10.7 Media Definition | section | 70 | [070-10.7-Media-Definition.md](070-10.7-Media-Definition.md) |
| 10.8 XFS Form/Media Definition Files in Multi-Vendor Environments | section | 18 | [071-10.8-XFS-Form-Media-Definition-Files-in-Multi-Vendor-Environments.md](071-10.8-XFS-Form-Media-Definition-Files-in-Multi-Vendor-Environments.md) |
| 11.1 Single Page / Single Wad Printing with immediate Media Control | section | 48 | [072-11.1-Single-Page-Single-Wad-Printing-with-immediate-Media-Control.md](072-11.1-Single-Page-Single-Wad-Printing-with-immediate-Media-Control.md) |
| 11.2 Single Page / Single Wad Printing with separate Media Control | section | 231 | [073-11.2-Single-Page-Single-Wad-Printing-with-separate-Media-Control.md](073-11.2-Single-Page-Single-Wad-Printing-with-separate-Media-Control.md) |
| 11.5 Printing with immediate Media Control and bMediaPresented == FALSE | section | 44 | [074-11.5-Printing-with-immediate-Media-Control-and-bMediaPresented-FA.md](074-11.5-Printing-with-immediate-Media-Control-and-bMediaPresented-FA.md) |
| 12. C-Header File | header | 794 | [075-12-C-Header-File.md](075-12-C-Header-File.md) |
