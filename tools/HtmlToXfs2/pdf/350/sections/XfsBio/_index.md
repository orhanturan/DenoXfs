# XfsBio - BIO

Biometric Device Class Interface

Source: `XfsBio.pdf` | sections: 42 | split level: 5

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 76 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 138 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1. Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.50 | section | 14 | [003-1.1-Background-to-Release-3-50.md](003-1.1-Background-to-Release-3-50.md) |
| 1.2 XFS Service-Specific Programming | section | 41 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2. Biometric Devices | section | 9 | [005-2-Biometric-Devices.md](005-2-Biometric-Devices.md) |
| 2.1 Enrollment | section | 7 | [006-2.1-Enrollment.md](006-2.1-Enrollment.md) |
| 2.2 Biometric Matching | section | 22 | [007-2.2-Biometric-Matching.md](007-2.2-Biometric-Matching.md) |
| 2.3 Biometric Device Types | section | 21 | [008-2.3-Biometric-Device-Types.md](008-2.3-Biometric-Device-Types.md) |
| 2.4 Biometric Data Security | section | 25 | [009-2.4-Biometric-Data-Security.md](009-2.4-Biometric-Data-Security.md) |
| 3. References | section | 11 | [010-3-References.md](010-3-References.md) |
| 4. Info Commands | info_commands_section | 1 | [011-4-Info-Commands.md](011-4-Info-Commands.md) |
| 4.1 WFS_INF_BIO_STATUS | info_command | 167 | [012-4.1-WFS-INF-BIO-STATUS.md](012-4.1-WFS-INF-BIO-STATUS.md) |
| 4.2 WFS_INF_BIO_CAPABILITIES | info_command | 249 | [013-4.2-WFS-INF-BIO-CAPABILITIES.md](013-4.2-WFS-INF-BIO-CAPABILITIES.md) |
| 4.3 WFS_INF_BIO_STORAGE_INFO | info_command | 54 | [014-4.3-WFS-INF-BIO-STORAGE-INFO.md](014-4.3-WFS-INF-BIO-STORAGE-INFO.md) |
| 4.4 WFS_INF_BIO_KEY_INFO | info_command | 43 | [015-4.4-WFS-INF-BIO-KEY-INFO.md](015-4.4-WFS-INF-BIO-KEY-INFO.md) |
| 5. Execute Commands | execute_commands_section | 1 | [016-5-Execute-Commands.md](016-5-Execute-Commands.md) |
| 5.1 WFS_CMD_BIO_READ | execute_command | 130 | [017-5.1-WFS-CMD-BIO-READ.md](017-5.1-WFS-CMD-BIO-READ.md) |
| 5.2 WFS_CMD_BIO_IMPORT | execute_command | 53 | [018-5.2-WFS-CMD-BIO-IMPORT.md](018-5.2-WFS-CMD-BIO-IMPORT.md) |
| 5.3 WFS_CMD_BIO_MATCH | execute_command | 139 | [019-5.3-WFS-CMD-BIO-MATCH.md](019-5.3-WFS-CMD-BIO-MATCH.md) |
| 5.4 WFS_CMD_BIO_SET_MATCH | execute_command | 31 | [020-5.4-WFS-CMD-BIO-SET-MATCH.md](020-5.4-WFS-CMD-BIO-SET-MATCH.md) |
| 5.5 WFS_CMD_BIO_CLEAR | execute_command | 23 | [021-5.5-WFS-CMD-BIO-CLEAR.md](021-5.5-WFS-CMD-BIO-CLEAR.md) |
| 5.6 WFS_CMD_BIO_RESET | execute_command | 24 | [022-5.6-WFS-CMD-BIO-RESET.md](022-5.6-WFS-CMD-BIO-RESET.md) |
| 5.7 WFS_CMD_BIO_SET_DATA_PERSISTENCE | execute_command | 26 | [023-5.7-WFS-CMD-BIO-SET-DATA-PERSISTENCE.md](023-5.7-WFS-CMD-BIO-SET-DATA-PERSISTENCE.md) |
| 5.8 WFS_CMD_BIO_SET_GUIDANCE_LIGHT | execute_command | 63 | [024-5.8-WFS-CMD-BIO-SET-GUIDANCE-LIGHT.md](024-5.8-WFS-CMD-BIO-SET-GUIDANCE-LIGHT.md) |
| 5.9 WFS_CMD_BIO_POWER_SAVE_CONTROL | execute_command | 33 | [025-5.9-WFS-CMD-BIO-POWER-SAVE-CONTROL.md](025-5.9-WFS-CMD-BIO-POWER-SAVE-CONTROL.md) |
| 5.10 WFS_CMD_BIO_SYNCHRONIZE_COMMAND | execute_command | 49 | [026-5.10-WFS-CMD-BIO-SYNCHRONIZE-COMMAND.md](026-5.10-WFS-CMD-BIO-SYNCHRONIZE-COMMAND.md) |
| 6. Events | events_section | 1 | [027-6-Events.md](027-6-Events.md) |
| 6.1 WFS_EXEE_BIO_PRESENTSUBJECT | event | 9 | [028-6.1-WFS-EXEE-BIO-PRESENTSUBJECT.md](028-6.1-WFS-EXEE-BIO-PRESENTSUBJECT.md) |
| 6.2 WFS_EXEE_BIO_SUBJECTDETECTED | event | 8 | [029-6.2-WFS-EXEE-BIO-SUBJECTDETECTED.md](029-6.2-WFS-EXEE-BIO-SUBJECTDETECTED.md) |
| 6.3 WFS_EXEE_BIO_REMOVESUBJECT | event | 8 | [030-6.3-WFS-EXEE-BIO-REMOVESUBJECT.md](030-6.3-WFS-EXEE-BIO-REMOVESUBJECT.md) |
| 6.4 WFS_SRVE_BIO_SUBJECTREMOVED | event | 8 | [031-6.4-WFS-SRVE-BIO-SUBJECTREMOVED.md](031-6.4-WFS-SRVE-BIO-SUBJECTREMOVED.md) |
| 6.5 WFS_SRVE_BIO_DATACLEARED | event | 27 | [032-6.5-WFS-SRVE-BIO-DATACLEARED.md](032-6.5-WFS-SRVE-BIO-DATACLEARED.md) |
| 6.6 WFS_EXEE_BIO_ORIENTATION | event | 8 | [033-6.6-WFS-EXEE-BIO-ORIENTATION.md](033-6.6-WFS-EXEE-BIO-ORIENTATION.md) |
| 6.7 WFS_SRVE_BIO_DEVICEPOSITION | event | 20 | [034-6.7-WFS-SRVE-BIO-DEVICEPOSITION.md](034-6.7-WFS-SRVE-BIO-DEVICEPOSITION.md) |
| 6.8 WFS_SRVE_BIO_POWER_SAVE_CHANGE | event | 15 | [035-6.8-WFS-SRVE-BIO-POWER-SAVE-CHANGE.md](035-6.8-WFS-SRVE-BIO-POWER-SAVE-CHANGE.md) |
| 7. Biometric Device Command Flows – Application Guidelines | section | 3 | [036-7-Biometric-Device-Command-Flows-Application-Guidelines.md](036-7-Biometric-Device-Command-Flows-Application-Guidelines.md) |
| 7.1 Biometric Enrollment Command Flow | section | 59 | [037-7.1-Biometric-Enrollment-Command-Flow.md](037-7.1-Biometric-Enrollment-Command-Flow.md) |
| 7.2 Biometric Match Command Flow – Separate Scan and Match | section | 55 | [038-7.2-Biometric-Match-Command-Flow-Separate-Scan-and-Match.md](038-7.2-Biometric-Match-Command-Flow-Separate-Scan-and-Match.md) |
| 7.3 Biometric Match Command Flow – Combined Scan and Match | section | 64 | [039-7.3-Biometric-Match-Command-Flow-Combined-Scan-and-Match.md](039-7.3-Biometric-Match-Command-Flow-Combined-Scan-and-Match.md) |
| 7.4 Biometric Scan-Only Command Flow | section | 40 | [040-7.4-Biometric-Scan-Only-Command-Flow.md](040-7.4-Biometric-Scan-Only-Command-Flow.md) |
| 8. C - Header file | header | 354 | [041-8-C-Header-file.md](041-8-C-Header-file.md) |
