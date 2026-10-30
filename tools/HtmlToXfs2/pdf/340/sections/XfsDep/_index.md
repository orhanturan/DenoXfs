# XfsDep - DEP

Depository Device Class Interface

Source: `XfsDep.pdf` | sections: 35 | split level: 3

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 66 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 145 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1. Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.40 | section | 22 | [003-1.1-Background-to-Release-3-40.md](003-1.1-Background-to-Release-3-40.md) |
| 1.2 XFS Service-Specific Programming | section | 41 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2. Depository Unit | section | 20 | [005-2-Depository-Unit.md](005-2-Depository-Unit.md) |
| 3. References | section | 7 | [006-3-References.md](006-3-References.md) |
| 4. Info Commands | info_commands_section | 1 | [007-4-Info-Commands.md](007-4-Info-Commands.md) |
| 4.1 WFS_INF_DEP_STATUS | info_command | 274 | [008-4.1-WFS-INF-DEP-STATUS.md](008-4.1-WFS-INF-DEP-STATUS.md) |
| 4.2 WFS_INF_DEP_CAPABILITIES | info_command | 152 | [009-4.2-WFS-INF-DEP-CAPABILITIES.md](009-4.2-WFS-INF-DEP-CAPABILITIES.md) |
| 5. Execute Commands | execute_commands_section | 1 | [010-5-Execute-Commands.md](010-5-Execute-Commands.md) |
| 5.1 WFS_CMD_DEP_ENTRY | execute_command | 88 | [011-5.1-WFS-CMD-DEP-ENTRY.md](011-5.1-WFS-CMD-DEP-ENTRY.md) |
| 5.2 WFS_CMD_DEP_DISPENSE | execute_command | 25 | [012-5.2-WFS-CMD-DEP-DISPENSE.md](012-5.2-WFS-CMD-DEP-DISPENSE.md) |
| 5.3 WFS_CMD_DEP_RETRACT | execute_command | 61 | [013-5.3-WFS-CMD-DEP-RETRACT.md](013-5.3-WFS-CMD-DEP-RETRACT.md) |
| 5.4 WFS_CMD_DEP_RESET_COUNT | execute_command | 16 | [014-5.4-WFS-CMD-DEP-RESET-COUNT.md](014-5.4-WFS-CMD-DEP-RESET-COUNT.md) |
| 5.5 WFS_CMD_DEP_RESET | execute_command | 57 | [015-5.5-WFS-CMD-DEP-RESET.md](015-5.5-WFS-CMD-DEP-RESET.md) |
| 5.6 WFS_CMD_DEP_SET_GUIDANCE_LIGHT | execute_command | 81 | [016-5.6-WFS-CMD-DEP-SET-GUIDANCE-LIGHT.md](016-5.6-WFS-CMD-DEP-SET-GUIDANCE-LIGHT.md) |
| 5.7 WFS_CMD_DEP_SUPPLY_REPLENISH | execute_command | 38 | [017-5.7-WFS-CMD-DEP-SUPPLY-REPLENISH.md](017-5.7-WFS-CMD-DEP-SUPPLY-REPLENISH.md) |
| 5.8 WFS_CMD_DEP_POWER_SAVE_CONTROL | execute_command | 37 | [018-5.8-WFS-CMD-DEP-POWER-SAVE-CONTROL.md](018-5.8-WFS-CMD-DEP-POWER-SAVE-CONTROL.md) |
| 5.9 WFS_CMD_DEP_SYNCHRONIZE_COMMAND | execute_command | 49 | [019-5.9-WFS-CMD-DEP-SYNCHRONIZE-COMMAND.md](019-5.9-WFS-CMD-DEP-SYNCHRONIZE-COMMAND.md) |
| 6. Events | events_section | 1 | [020-6-Events.md](020-6-Events.md) |
| 6.1 WFS_SRVE_DEP_ENVTAKEN | event | 7 | [021-6.1-WFS-SRVE-DEP-ENVTAKEN.md](021-6.1-WFS-SRVE-DEP-ENVTAKEN.md) |
| 6.2 WFS_EXEE_DEP_ENVDEPOSITED | event | 7 | [022-6.2-WFS-EXEE-DEP-ENVDEPOSITED.md](022-6.2-WFS-EXEE-DEP-ENVDEPOSITED.md) |
| 6.3 WFS_EXEE_DEP_DEPOSITERROR | event | 11 | [023-6.3-WFS-EXEE-DEP-DEPOSITERROR.md](023-6.3-WFS-EXEE-DEP-DEPOSITERROR.md) |
| 6.4 WFS_USRE_DEP_DEPTHRESHOLD | event | 14 | [024-6.4-WFS-USRE-DEP-DEPTHRESHOLD.md](024-6.4-WFS-USRE-DEP-DEPTHRESHOLD.md) |
| 6.5 WFS_USRE_DEP_TONERTHRESHOLD | event | 18 | [025-6.5-WFS-USRE-DEP-TONERTHRESHOLD.md](025-6.5-WFS-USRE-DEP-TONERTHRESHOLD.md) |
| 6.6 WFS_USRE_DEP_ENVTHRESHOLD | event | 15 | [026-6.6-WFS-USRE-DEP-ENVTHRESHOLD.md](026-6.6-WFS-USRE-DEP-ENVTHRESHOLD.md) |
| 6.7 WFS_SRVE_DEP_CONTINSERTED | event | 7 | [027-6.7-WFS-SRVE-DEP-CONTINSERTED.md](027-6.7-WFS-SRVE-DEP-CONTINSERTED.md) |
| 6.8 WFS_SRVE_DEP_CONTREMOVED | event | 7 | [028-6.8-WFS-SRVE-DEP-CONTREMOVED.md](028-6.8-WFS-SRVE-DEP-CONTREMOVED.md) |
| 6.9 WFS_SRVE_DEP_ENVINSERTED | event | 7 | [029-6.9-WFS-SRVE-DEP-ENVINSERTED.md](029-6.9-WFS-SRVE-DEP-ENVINSERTED.md) |
| 6.10 WFS_SRVE_DEP_MEDIADETECTED | event | 36 | [030-6.10-WFS-SRVE-DEP-MEDIADETECTED.md](030-6.10-WFS-SRVE-DEP-MEDIADETECTED.md) |
| 6.11 WFS_EXEE_DEP_INSERTDEPOSIT | event | 8 | [031-6.11-WFS-EXEE-DEP-INSERTDEPOSIT.md](031-6.11-WFS-EXEE-DEP-INSERTDEPOSIT.md) |
| 6.12 WFS_SRVE_DEP_DEVICEPOSITION | event | 20 | [032-6.12-WFS-SRVE-DEP-DEVICEPOSITION.md](032-6.12-WFS-SRVE-DEP-DEVICEPOSITION.md) |
| 6.13 WFS_SRVE_DEP_POWER_SAVE_CHANGE | event | 15 | [033-6.13-WFS-SRVE-DEP-POWER-SAVE-CHANGE.md](033-6.13-WFS-SRVE-DEP-POWER-SAVE-CHANGE.md) |
| 7. C - Header file | header | 284 | [034-7-C-Header-file.md](034-7-C-Header-file.md) |
