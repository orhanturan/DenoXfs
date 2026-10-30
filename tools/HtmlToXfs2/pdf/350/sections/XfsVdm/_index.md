# XfsVdm - VDM

Vendor Dependent Mode Device Class Interface

Source: `XfsVdm.pdf` | sections: 27 | split level: 5

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 128 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 275 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1 Introduction | section | 2 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.50 | section | 20 | [003-1.1-Background-to-Release-3-50.md](003-1.1-Background-to-Release-3-50.md) |
| 1.2 XFS Service-Specific Programming | section | 55 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2 Vendor Dependent Mode | section | 60 | [005-2-Vendor-Dependent-Mode.md](005-2-Vendor-Dependent-Mode.md) |
| 2.1 VDM Entry triggered by XFS Application | section | 72 | [006-2.1-VDM-Entry-triggered-by-XFS-Application.md](006-2.1-VDM-Entry-triggered-by-XFS-Application.md) |
| 2.2 VDM Entry triggered by Vendor Dependent Switch | section | 65 | [007-2.2-VDM-Entry-triggered-by-Vendor-Dependent-Switch.md](007-2.2-VDM-Entry-triggered-by-Vendor-Dependent-Switch.md) |
| 2.3 VDM Exit triggered by XFS Application | section | 66 | [008-2.3-VDM-Exit-triggered-by-XFS-Application.md](008-2.3-VDM-Exit-triggered-by-XFS-Application.md) |
| 2.4 VDM Exit triggered by Vendor Dependent Switch | section | 61 | [009-2.4-VDM-Exit-triggered-by-Vendor-Dependent-Switch.md](009-2.4-VDM-Exit-triggered-by-Vendor-Dependent-Switch.md) |
| 2.5 Controlling / Determining the Active Interface | section | 34 | [010-2.5-Controlling-Determining-the-Active-Interface.md](010-2.5-Controlling-Determining-the-Active-Interface.md) |
| 4.1 WFS_INF_VDM_STATUS | info_command | 151 | [011-4.1-WFS-INF-VDM-STATUS.md](011-4.1-WFS-INF-VDM-STATUS.md) |
| 4.2 WFS_INF_VDM_CAPABILITIES | info_command | 78 | [012-4.2-WFS-INF-VDM-CAPABILITIES.md](012-4.2-WFS-INF-VDM-CAPABILITIES.md) |
| 4.3 WFS_INF_VDM_ACTIVE_INTERFACE | info_command | 43 | [013-4.3-WFS-INF-VDM-ACTIVE-INTERFACE.md](013-4.3-WFS-INF-VDM-ACTIVE-INTERFACE.md) |
| 5 Execute Commands | execute_commands_section | 2 | [014-5-Execute-Commands.md](014-5-Execute-Commands.md) |
| 5.1 WFS_CMD_VDM_ENTER_MODE_REQ | execute_command | 68 | [015-5.1-WFS-CMD-VDM-ENTER-MODE-REQ.md](015-5.1-WFS-CMD-VDM-ENTER-MODE-REQ.md) |
| 5.2 WFS_CMD_VDM_ENTER_MODE_ACK | execute_command | 35 | [016-5.2-WFS-CMD-VDM-ENTER-MODE-ACK.md](016-5.2-WFS-CMD-VDM-ENTER-MODE-ACK.md) |
| 5.3 WFS_CMD_VDM_EXIT_MODE_REQ | execute_command | 48 | [017-5.3-WFS-CMD-VDM-EXIT-MODE-REQ.md](017-5.3-WFS-CMD-VDM-EXIT-MODE-REQ.md) |
| 5.4 WFS_CMD_VDM_EXIT_MODE_ACK | execute_command | 31 | [018-5.4-WFS-CMD-VDM-EXIT-MODE-ACK.md](018-5.4-WFS-CMD-VDM-EXIT-MODE-ACK.md) |
| 5.5 WFS_CMD_VDM_SET_ACTIVE_INTERFACE | execute_command | 51 | [019-5.5-WFS-CMD-VDM-SET-ACTIVE-INTERFACE.md](019-5.5-WFS-CMD-VDM-SET-ACTIVE-INTERFACE.md) |
| 6 Events | events_section | 2 | [020-6-Events.md](020-6-Events.md) |
| 6.1 WFS_SRVE_VDM_ENTER_MODE_REQ | event | 17 | [021-6.1-WFS-SRVE-VDM-ENTER-MODE-REQ.md](021-6.1-WFS-SRVE-VDM-ENTER-MODE-REQ.md) |
| 6.2 WFS_SRVE_VDM_EXIT_MODE_REQ | event | 17 | [022-6.2-WFS-SRVE-VDM-EXIT-MODE-REQ.md](022-6.2-WFS-SRVE-VDM-EXIT-MODE-REQ.md) |
| 6.3 WFS_SYSE_VDM_MODEENTERED | api_function | 17 | [023-6.3-WFS-SYSE-VDM-MODEENTERED.md](023-6.3-WFS-SYSE-VDM-MODEENTERED.md) |
| 6.4 WFS_SYSE_VDM_MODEEXITED | api_function | 17 | [024-6.4-WFS-SYSE-VDM-MODEEXITED.md](024-6.4-WFS-SYSE-VDM-MODEEXITED.md) |
| 6.5 WFS_SRVE_VDM_INTERFACE_CHANGED | event | 29 | [025-6.5-WFS-SRVE-VDM-INTERFACE-CHANGED.md](025-6.5-WFS-SRVE-VDM-INTERFACE-CHANGED.md) |
| 7 C-Header file | header | 142 | [026-7-C-Header-file.md](026-7-C-Header-file.md) |
