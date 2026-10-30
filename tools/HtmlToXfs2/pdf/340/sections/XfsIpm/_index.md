# XfsIpm - IPM

Item Processing Module Device Class Interface

Source: `XfsIpm.pdf` | sections: 75 | split level: 3

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 109 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 145 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1. Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.40 | section | 22 | [003-1.1-Background-to-Release-3-40.md](003-1.1-Background-to-Release-3-40.md) |
| 1.2 XFS Service-Specific Programming | section | 41 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2. Item Processing Module | section | 46 | [005-2-Item-Processing-Module.md](005-2-Item-Processing-Module.md) |
| 2.1 Devices with a Stacker | section | 5 | [006-2.1-Devices-with-a-Stacker.md](006-2.1-Devices-with-a-Stacker.md) |
| 2.1.1 Automatic Accept/Refuse | section | 22 | [007-2.1.1-Automatic-Accept-Refuse.md](007-2.1.1-Automatic-Accept-Refuse.md) |
| 2.1.2 Application Controlled Accept/Refuse | section | 35 | [008-2.1.2-Application-Controlled-Accept-Refuse.md](008-2.1.2-Application-Controlled-Accept-Refuse.md) |
| 2.2 Device without a Stacker | section | 3 | [009-2.2-Device-without-a-Stacker.md](009-2.2-Device-without-a-Stacker.md) |
| 2.2.1 Multi-Feed Devices without a Stacker | section | 19 | [010-2.2.1-Multi-Feed-Devices-without-a-Stacker.md](010-2.2.1-Multi-Feed-Devices-without-a-Stacker.md) |
| 2.2.2 Single-Feed Devices | section | 21 | [011-2.2.2-Single-Feed-Devices.md](011-2.2.2-Single-Feed-Devices.md) |
| 3. References | section | 8 | [012-3-References.md](012-3-References.md) |
| 4. Info Commands | info_commands_section | 1 | [013-4-Info-Commands.md](013-4-Info-Commands.md) |
| 4.1 WFS_INF_IPM_STATUS | info_command | 398 | [014-4.1-WFS-INF-IPM-STATUS.md](014-4.1-WFS-INF-IPM-STATUS.md) |
| 4.2 WFS_INF_IPM_CAPABILITIES | info_command | 440 | [015-4.2-WFS-INF-IPM-CAPABILITIES.md](015-4.2-WFS-INF-IPM-CAPABILITIES.md) |
| 4.3 WFS_INF_IPM_CODELINE_MAPPING | info_command | 62 | [016-4.3-WFS-INF-IPM-CODELINE-MAPPING.md](016-4.3-WFS-INF-IPM-CODELINE-MAPPING.md) |
| 4.4 WFS_INF_IPM_MEDIA_BIN_INFO | info_command | 130 | [017-4.4-WFS-INF-IPM-MEDIA-BIN-INFO.md](017-4.4-WFS-INF-IPM-MEDIA-BIN-INFO.md) |
| 4.5 WFS_INF_IPM_TRANSACTION_STATUS | info_command | 236 | [018-4.5-WFS-INF-IPM-TRANSACTION-STATUS.md](018-4.5-WFS-INF-IPM-TRANSACTION-STATUS.md) |
| 4.6 WFS_INF_IPM_MEDIA_BIN_CAPABILITIES | info_command | 64 | [019-4.6-WFS-INF-IPM-MEDIA-BIN-CAPABILITIES.md](019-4.6-WFS-INF-IPM-MEDIA-BIN-CAPABILITIES.md) |
| 5. Execute Commands | execute_commands_section | 1 | [020-5-Execute-Commands.md](020-5-Execute-Commands.md) |
| 5.1 WFS_CMD_IPM_MEDIA_IN | execute_command | 275 | [021-5.1-WFS-CMD-IPM-MEDIA-IN.md](021-5.1-WFS-CMD-IPM-MEDIA-IN.md) |
| 5.2 WFS_CMD_IPM_MEDIA_IN_END | execute_command | 135 | [022-5.2-WFS-CMD-IPM-MEDIA-IN-END.md](022-5.2-WFS-CMD-IPM-MEDIA-IN-END.md) |
| 5.3 WFS_CMD_IPM_MEDIA_IN_ROLLBACK | execute_command | 69 | [023-5.3-WFS-CMD-IPM-MEDIA-IN-ROLLBACK.md](023-5.3-WFS-CMD-IPM-MEDIA-IN-ROLLBACK.md) |
| 5.4 WFS_CMD_IPM_READ_IMAGE | execute_command | 295 | [024-5.4-WFS-CMD-IPM-READ-IMAGE.md](024-5.4-WFS-CMD-IPM-READ-IMAGE.md) |
| 5.5 WFS_CMD_IPM_SET_DESTINATION | execute_command | 39 | [025-5.5-WFS-CMD-IPM-SET-DESTINATION.md](025-5.5-WFS-CMD-IPM-SET-DESTINATION.md) |
| 5.6 WFS_CMD_IPM_PRESENT_MEDIA | execute_command | 62 | [026-5.6-WFS-CMD-IPM-PRESENT-MEDIA.md](026-5.6-WFS-CMD-IPM-PRESENT-MEDIA.md) |
| 5.7 WFS_CMD_IPM_RETRACT_MEDIA | execute_command | 101 | [027-5.7-WFS-CMD-IPM-RETRACT-MEDIA.md](027-5.7-WFS-CMD-IPM-RETRACT-MEDIA.md) |
| 5.8 WFS_CMD_IPM_PRINT_TEXT | execute_command | 56 | [028-5.8-WFS-CMD-IPM-PRINT-TEXT.md](028-5.8-WFS-CMD-IPM-PRINT-TEXT.md) |
| 5.9 WFS_CMD_IPM_SET_MEDIA_BIN_INFO | execute_command | 48 | [029-5.9-WFS-CMD-IPM-SET-MEDIA-BIN-INFO.md](029-5.9-WFS-CMD-IPM-SET-MEDIA-BIN-INFO.md) |
| 5.10 WFS_CMD_IPM_RESET | execute_command | 74 | [030-5.10-WFS-CMD-IPM-RESET.md](030-5.10-WFS-CMD-IPM-RESET.md) |
| 5.11 WFS_CMD_IPM_SET_GUIDANCE_LIGHT | execute_command | 70 | [031-5.11-WFS-CMD-IPM-SET-GUIDANCE-LIGHT.md](031-5.11-WFS-CMD-IPM-SET-GUIDANCE-LIGHT.md) |
| 5.12 WFS_CMD_IPM_GET_NEXT_ITEM | execute_command | 70 | [032-5.12-WFS-CMD-IPM-GET-NEXT-ITEM.md](032-5.12-WFS-CMD-IPM-GET-NEXT-ITEM.md) |
| 5.13 WFS_CMD_IPM_ACTION_ITEM | execute_command | 71 | [033-5.13-WFS-CMD-IPM-ACTION-ITEM.md](033-5.13-WFS-CMD-IPM-ACTION-ITEM.md) |
| 5.14 WFS_CMD_IPM_EXPEL_MEDIA | execute_command | 23 | [034-5.14-WFS-CMD-IPM-EXPEL-MEDIA.md](034-5.14-WFS-CMD-IPM-EXPEL-MEDIA.md) |
| 5.15 WFS_CMD_IPM_GET_IMAGE_AFTER_PRINT | execute_command | 102 | [035-5.15-WFS-CMD-IPM-GET-IMAGE-AFTER-PRINT.md](035-5.15-WFS-CMD-IPM-GET-IMAGE-AFTER-PRINT.md) |
| 5.16 WFS_CMD_IPM_ACCEPT_ITEM | execute_command | 36 | [036-5.16-WFS-CMD-IPM-ACCEPT-ITEM.md](036-5.16-WFS-CMD-IPM-ACCEPT-ITEM.md) |
| 5.17 WFS_CMD_IPM_SUPPLY_REPLENISH | execute_command | 37 | [037-5.17-WFS-CMD-IPM-SUPPLY-REPLENISH.md](037-5.17-WFS-CMD-IPM-SUPPLY-REPLENISH.md) |
| 5.18 WFS_CMD_IPM_POWER_SAVE_CONTROL | execute_command | 37 | [038-5.18-WFS-CMD-IPM-POWER-SAVE-CONTROL.md](038-5.18-WFS-CMD-IPM-POWER-SAVE-CONTROL.md) |
| 5.19 WFS_CMD_IPM_SET_MODE | execute_command | 34 | [039-5.19-WFS-CMD-IPM-SET-MODE.md](039-5.19-WFS-CMD-IPM-SET-MODE.md) |
| 5.20 WFS_CMD_IPM_SYNCHRONIZE_COMMAND | execute_command | 49 | [040-5.20-WFS-CMD-IPM-SYNCHRONIZE-COMMAND.md](040-5.20-WFS-CMD-IPM-SYNCHRONIZE-COMMAND.md) |
| 6. Events | events_section | 1 | [041-6-Events.md](041-6-Events.md) |
| 6.1 WFS_EXEE_IPM_NOMEDIA | event | 8 | [042-6.1-WFS-EXEE-IPM-NOMEDIA.md](042-6.1-WFS-EXEE-IPM-NOMEDIA.md) |
| 6.2 WFS_EXEE_IPM_MEDIAINSERTED | event | 8 | [043-6.2-WFS-EXEE-IPM-MEDIAINSERTED.md](043-6.2-WFS-EXEE-IPM-MEDIAINSERTED.md) |
| 6.3 WFS_USRE_IPM_MEDIABINTHRESHOLD | event | 13 | [044-6.3-WFS-USRE-IPM-MEDIABINTHRESHOLD.md](044-6.3-WFS-USRE-IPM-MEDIABINTHRESHOLD.md) |
| 6.4 WFS_SRVE_IPM_MEDIABININFOCHANGED | event | 20 | [045-6.4-WFS-SRVE-IPM-MEDIABININFOCHANGED.md](045-6.4-WFS-SRVE-IPM-MEDIABININFOCHANGED.md) |
| 6.5 WFS_EXEE_IPM_MEDIABINERROR | event | 31 | [046-6.5-WFS-EXEE-IPM-MEDIABINERROR.md](046-6.5-WFS-EXEE-IPM-MEDIABINERROR.md) |
| 6.6 WFS_SRVE_IPM_MEDIATAKEN | event | 19 | [047-6.6-WFS-SRVE-IPM-MEDIATAKEN.md](047-6.6-WFS-SRVE-IPM-MEDIATAKEN.md) |
| 6.7 WFS_USRE_IPM_TONERTHRESHOLD | event | 18 | [048-6.7-WFS-USRE-IPM-TONERTHRESHOLD.md](048-6.7-WFS-USRE-IPM-TONERTHRESHOLD.md) |
| 6.8 WFS_USRE_IPM_SCANNERTHRESHOLD | event | 25 | [049-6.8-WFS-USRE-IPM-SCANNERTHRESHOLD.md](049-6.8-WFS-USRE-IPM-SCANNERTHRESHOLD.md) |
| 6.9 WFS_USRE_IPM_INKTHRESHOLD | event | 18 | [050-6.9-WFS-USRE-IPM-INKTHRESHOLD.md](050-6.9-WFS-USRE-IPM-INKTHRESHOLD.md) |
| 6.10 WFS_SRVE_IPM_MEDIADETECTED | event | 28 | [051-6.10-WFS-SRVE-IPM-MEDIADETECTED.md](051-6.10-WFS-SRVE-IPM-MEDIADETECTED.md) |
| 6.11 WFS_EXEE_IPM_MEDIAPRESENTED | event | 28 | [052-6.11-WFS-EXEE-IPM-MEDIAPRESENTED.md](052-6.11-WFS-EXEE-IPM-MEDIAPRESENTED.md) |
| 6.12 WFS_EXEE_IPM_MEDIAREFUSED | event | 103 | [053-6.12-WFS-EXEE-IPM-MEDIAREFUSED.md](053-6.12-WFS-EXEE-IPM-MEDIAREFUSED.md) |
| 6.13 WFS_EXEE_IPM_MEDIADATA | event | 171 | [054-6.13-WFS-EXEE-IPM-MEDIADATA.md](054-6.13-WFS-EXEE-IPM-MEDIADATA.md) |
| 6.14 WFS_USRE_IPM_MICRTHRESHOLD | event | 17 | [055-6.14-WFS-USRE-IPM-MICRTHRESHOLD.md](055-6.14-WFS-USRE-IPM-MICRTHRESHOLD.md) |
| 6.15 WFS_EXEE_IPM_MEDIAREJECTED | event | 37 | [056-6.15-WFS-EXEE-IPM-MEDIAREJECTED.md](056-6.15-WFS-EXEE-IPM-MEDIAREJECTED.md) |
| 6.16 WFS_SRVE_IPM_DEVICEPOSITION | event | 20 | [057-6.16-WFS-SRVE-IPM-DEVICEPOSITION.md](057-6.16-WFS-SRVE-IPM-DEVICEPOSITION.md) |
| 6.17 WFS_SRVE_IPM_POWER_SAVE_CHANGE | event | 15 | [058-6.17-WFS-SRVE-IPM-POWER-SAVE-CHANGE.md](058-6.17-WFS-SRVE-IPM-POWER-SAVE-CHANGE.md) |
| 6.18 WFS_SRVE_IPM_SHUTTERSTATUSCHANGED | event | 29 | [059-6.18-WFS-SRVE-IPM-SHUTTERSTATUSCHANGED.md](059-6.18-WFS-SRVE-IPM-SHUTTERSTATUSCHANGED.md) |
| 7. Command and Event Flows | events_section | 1 | [060-7-Command-and-Event-Flows.md](060-7-Command-and-Event-Flows.md) |
| 7.1 Devices with Stacker | section | 1 | [061-7.1-Devices-with-Stacker.md](061-7.1-Devices-with-Stacker.md) |
| 7.1.1 Bunch Media Processing (OK flow) | section | 37 | [062-7.1.1-Bunch-Media-Processing-OK-flow.md](062-7.1.1-Bunch-Media-Processing-OK-flow.md) |
| 7.1.2 Bunch Media Processing (Some Media Items Returned) | section | 53 | [063-7.1.2-Bunch-Media-Processing-Some-Media-Items-Returned.md](063-7.1.2-Bunch-Media-Processing-Some-Media-Items-Returned.md) |
| 7.1.3 Bunch Media Processing with Errors | section | 55 | [064-7.1.3-Bunch-Media-Processing-with-Errors.md](064-7.1.3-Bunch-Media-Processing-with-Errors.md) |
| 7.1.4 Bunch media processing with Rollback | section | 24 | [065-7.1.4-Bunch-media-processing-with-Rollback.md](065-7.1.4-Bunch-media-processing-with-Rollback.md) |
| 7.1.5 Bunch media processing with Retract | section | 8 | [066-7.1.5-Bunch-media-processing-with-Retract.md](066-7.1.5-Bunch-media-processing-with-Retract.md) |
| 7.1.6 Bunch Media Processing - Application Refuse Decision (All OK flow) | section | 45 | [067-7.1.6-Bunch-Media-Processing-Application-Refuse-Decision-All-OK-fl.md](067-7.1.6-Bunch-Media-Processing-Application-Refuse-Decision-All-OK-fl.md) |
| 7.1.7 Bunch Media Processing - Application Refuse Decision (Some items refused) | section | 72 | [068-7.1.7-Bunch-Media-Processing-Application-Refuse-Decision-Some-item.md](068-7.1.7-Bunch-Media-Processing-Application-Refuse-Decision-Some-item.md) |
| 7.2 Devices without Stacker | section | 4 | [069-7.2-Devices-without-Stacker.md](069-7.2-Devices-without-Stacker.md) |
| 7.2.1 Bunch Media Processing (OK flow) | section | 41 | [070-7.2.1-Bunch-Media-Processing-OK-flow.md](070-7.2.1-Bunch-Media-Processing-OK-flow.md) |
| 7.2.2 Bunch Media Processing (Some Media Items Returned) | section | 56 | [071-7.2.2-Bunch-Media-Processing-Some-Media-Items-Returned.md](071-7.2.2-Bunch-Media-Processing-Some-Media-Items-Returned.md) |
| 7.2.3 Bunch Media Processing with Errors | section | 71 | [072-7.2.3-Bunch-Media-Processing-with-Errors.md](072-7.2.3-Bunch-Media-Processing-with-Errors.md) |
| 8. ATM Mixed Media Transaction Flow – Application Guidelines | section | 6 | [073-8-ATM-Mixed-Media-Transaction-Flow-Application-Guidelines.md](073-8-ATM-Mixed-Media-Transaction-Flow-Application-Guidelines.md) |
| 9. C-Header File | header | 851 | [074-9-C-Header-File.md](074-9-C-Header-File.md) |
