# XfsCeu - CEU

Card Embossing Unit Device Class Interface

Source: `XfsCeu.pdf` | sections: 41 | split level: 3

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 75 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 145 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1. Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.40 | section | 22 | [003-1.1-Background-to-Release-3-40.md](003-1.1-Background-to-Release-3-40.md) |
| 1.2 XFS Service-Specific Programming | section | 41 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2. Card Embossing Units | section | 20 | [005-2-Card-Embossing-Units.md](005-2-Card-Embossing-Units.md) |
| 3. References | section | 6 | [006-3-References.md](006-3-References.md) |
| 4. Info Commands | info_commands_section | 1 | [007-4-Info-Commands.md](007-4-Info-Commands.md) |
| 4.1 WFS_INF_CEU_STATUS | info_command | 186 | [008-4.1-WFS-INF-CEU-STATUS.md](008-4.1-WFS-INF-CEU-STATUS.md) |
| 4.2 WFS_INF_CEU_CAPABILITIES | info_command | 90 | [009-4.2-WFS-INF-CEU-CAPABILITIES.md](009-4.2-WFS-INF-CEU-CAPABILITIES.md) |
| 4.3 WFS_INF_CEU_FORM_LIST | info_command | 12 | [010-4.3-WFS-INF-CEU-FORM-LIST.md](010-4.3-WFS-INF-CEU-FORM-LIST.md) |
| 4.4 WFS_INF_CEU_MEDIA_LIST | info_command | 12 | [011-4.4-WFS-INF-CEU-MEDIA-LIST.md](011-4.4-WFS-INF-CEU-MEDIA-LIST.md) |
| 4.5 WFS_INF_CEU_QUERY_FORM | info_command | 38 | [012-4.5-WFS-INF-CEU-QUERY-FORM.md](012-4.5-WFS-INF-CEU-QUERY-FORM.md) |
| 4.6 WFS_INF_CEU_QUERY_MEDIA | info_command | 80 | [013-4.6-WFS-INF-CEU-QUERY-MEDIA.md](013-4.6-WFS-INF-CEU-QUERY-MEDIA.md) |
| 4.7 WFS_INF_CEU_QUERY_FIELD | info_command | 72 | [014-4.7-WFS-INF-CEU-QUERY-FIELD.md](014-4.7-WFS-INF-CEU-QUERY-FIELD.md) |
| 5. Execute Commands | execute_commands_section | 1 | [015-5-Execute-Commands.md](015-5-Execute-Commands.md) |
| 5.1 WFS_CMD_CEU_EMBOSS_CARD | execute_command | 113 | [016-5.1-WFS-CMD-CEU-EMBOSS-CARD.md](016-5.1-WFS-CMD-CEU-EMBOSS-CARD.md) |
| 5.2 WFS_CMD_CEU_RESET | execute_command | 32 | [017-5.2-WFS-CMD-CEU-RESET.md](017-5.2-WFS-CMD-CEU-RESET.md) |
| 5.3 WFS_CMD_CEU_POWER_SAVE_CONTROL | execute_command | 37 | [018-5.3-WFS-CMD-CEU-POWER-SAVE-CONTROL.md](018-5.3-WFS-CMD-CEU-POWER-SAVE-CONTROL.md) |
| 5.4 WFS_CMD_CEU_EMBOSS_CARD_EX | execute_command | 142 | [019-5.4-WFS-CMD-CEU-EMBOSS-CARD-EX.md](019-5.4-WFS-CMD-CEU-EMBOSS-CARD-EX.md) |
| 5.5 WFS_CMD_CEU_SUPPLY_REPLENISH | execute_command | 37 | [020-5.5-WFS-CMD-CEU-SUPPLY-REPLENISH.md](020-5.5-WFS-CMD-CEU-SUPPLY-REPLENISH.md) |
| 5.6 WFS_CMD_CEU_SYNCHRONIZE_COMMAND | execute_command | 49 | [021-5.6-WFS-CMD-CEU-SYNCHRONIZE-COMMAND.md](021-5.6-WFS-CMD-CEU-SYNCHRONIZE-COMMAND.md) |
| 6. Events | events_section | 1 | [022-6-Events.md](022-6-Events.md) |
| 6.1 WFS_SRVE_CEU_INPUTBINTHRESHOLD | event | 13 | [023-6.1-WFS-SRVE-CEU-INPUTBINTHRESHOLD.md](023-6.1-WFS-SRVE-CEU-INPUTBINTHRESHOLD.md) |
| 6.2 WFS_SRVE_CEU_OUTPUTBINTHRESHOLD | event | 13 | [024-6.2-WFS-SRVE-CEU-OUTPUTBINTHRESHOLD.md](024-6.2-WFS-SRVE-CEU-OUTPUTBINTHRESHOLD.md) |
| 6.3 WFS_SRVE_CEU_RETAINBINTHRESHOLD | event | 13 | [025-6.3-WFS-SRVE-CEU-RETAINBINTHRESHOLD.md](025-6.3-WFS-SRVE-CEU-RETAINBINTHRESHOLD.md) |
| 6.4 WFS_EXEE_CEU_FIELDERROR | event | 36 | [026-6.4-WFS-EXEE-CEU-FIELDERROR.md](026-6.4-WFS-EXEE-CEU-FIELDERROR.md) |
| 6.5 WFS_EXEE_CEU_FIELDWARNING | event | 8 | [027-6.5-WFS-EXEE-CEU-FIELDWARNING.md](027-6.5-WFS-EXEE-CEU-FIELDWARNING.md) |
| 6.6 WFS_SRVE_CEU_MEDIAREMOVED | event | 7 | [028-6.6-WFS-SRVE-CEU-MEDIAREMOVED.md](028-6.6-WFS-SRVE-CEU-MEDIAREMOVED.md) |
| 6.7 WFS_SRVE_CEU_MEDIADETECTED | event | 16 | [029-6.7-WFS-SRVE-CEU-MEDIADETECTED.md](029-6.7-WFS-SRVE-CEU-MEDIADETECTED.md) |
| 6.8 WFS_EXEE_CEU_EMBOSS_FAILURE | event | 38 | [030-6.8-WFS-EXEE-CEU-EMBOSS-FAILURE.md](030-6.8-WFS-EXEE-CEU-EMBOSS-FAILURE.md) |
| 6.9 WFS_SRVE_CEU_DEVICEPOSITION | event | 20 | [031-6.9-WFS-SRVE-CEU-DEVICEPOSITION.md](031-6.9-WFS-SRVE-CEU-DEVICEPOSITION.md) |
| 6.10 WFS_SRVE_CEU_POWER_SAVE_CHANGE | event | 15 | [032-6.10-WFS-SRVE-CEU-POWER-SAVE-CHANGE.md](032-6.10-WFS-SRVE-CEU-POWER-SAVE-CHANGE.md) |
| 6.11 WFS_USRE_CEU_TONERTHRESHOLD | event | 23 | [033-6.11-WFS-USRE-CEU-TONERTHRESHOLD.md](033-6.11-WFS-USRE-CEU-TONERTHRESHOLD.md) |
| 7. Embossing Form, Field and Media Definitions | section | 2 | [034-7-Embossing-Form-Field-and-Media-Definitions.md](034-7-Embossing-Form-Field-and-Media-Definitions.md) |
| 7.1 Definition Syntax | section | 27 | [035-7.1-Definition-Syntax.md](035-7.1-Definition-Syntax.md) |
| 7.2 Embossing Form and Media Measurements | section | 21 | [036-7.2-Embossing-Form-and-Media-Measurements.md](036-7.2-Embossing-Form-and-Media-Measurements.md) |
| 7.3 Embossing Form Definition | section | 54 | [037-7.3-Embossing-Form-Definition.md](037-7.3-Embossing-Form-Definition.md) |
| 7.4 Embossing Field Definition | section | 69 | [038-7.4-Embossing-Field-Definition.md](038-7.4-Embossing-Field-Definition.md) |
| 7.5 Media Definition | section | 38 | [039-7.5-Media-Definition.md](039-7.5-Media-Definition.md) |
| 8. C-Header file | header | 352 | [040-8-C-Header-file.md](040-8-C-Header-file.md) |
