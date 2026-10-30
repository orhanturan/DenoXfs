# XfsVdm - VDM

Vendor Dependent Mode Device Class Interface

Source: `XfsVdm.pdf` | sections: 29 | split level: 3

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 122 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 249 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1 Introduction | section | 2 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.40 | section | 36 | [003-1.1-Background-to-Release-3-40.md](003-1.1-Background-to-Release-3-40.md) |
| 1.2 XFS Service-Specific Programming | section | 54 | [004-1.2-XFS-Service-Specific-Programming.md](004-1.2-XFS-Service-Specific-Programming.md) |
| 2 Vendor Dependent Mode | section | 60 | [005-2-Vendor-Dependent-Mode.md](005-2-Vendor-Dependent-Mode.md) |
| 2.1 VDM Entry triggered by XFS Application | section | 72 | [006-2.1-VDM-Entry-triggered-by-XFS-Application.md](006-2.1-VDM-Entry-triggered-by-XFS-Application.md) |
| 2.2 VDM Entry triggered by Vendor Dependent Switch | section | 65 | [007-2.2-VDM-Entry-triggered-by-Vendor-Dependent-Switch.md](007-2.2-VDM-Entry-triggered-by-Vendor-Dependent-Switch.md) |
| 2.3 VDM Exit triggered by XFS Application | section | 66 | [008-2.3-VDM-Exit-triggered-by-XFS-Application.md](008-2.3-VDM-Exit-triggered-by-XFS-Application.md) |
| 2.4 VDM Exit triggered by Vendor Dependent Switch | section | 61 | [009-2.4-VDM-Exit-triggered-by-Vendor-Dependent-Switch.md](009-2.4-VDM-Exit-triggered-by-Vendor-Dependent-Switch.md) |
| 2.5 Controlling / Determining the Active Interface | section | 13 | [010-2.5-Controlling-Determining-the-Active-Interface.md](010-2.5-Controlling-Determining-the-Active-Interface.md) |
| Vendor Dependent Application independent of the VDM Service Provider | section | 80 | [011-Vendor-Dependent-Application-independent-of-the-VDM-Service.md](011-Vendor-Dependent-Application-independent-of-the-VDM-Service.md) |
| Vendor Dependent Application under Control of the VDM Service Provider | section | 32 | [012-Vendor-Dependent-Application-under-Control-of-the-VDM-Servic.md](012-Vendor-Dependent-Application-under-Control-of-the-VDM-Servic.md) |
| 4.1 WFS_INF_VDM_STATUS | info_command | 151 | [013-4.1-WFS-INF-VDM-STATUS.md](013-4.1-WFS-INF-VDM-STATUS.md) |
| 4.2 WFS_INF_VDM_CAPABILITIES | info_command | 78 | [014-4.2-WFS-INF-VDM-CAPABILITIES.md](014-4.2-WFS-INF-VDM-CAPABILITIES.md) |
| 4.3 WFS_INF_VDM_ACTIVE_INTERFACE | info_command | 43 | [015-4.3-WFS-INF-VDM-ACTIVE-INTERFACE.md](015-4.3-WFS-INF-VDM-ACTIVE-INTERFACE.md) |
| 5 Execute Commands | execute_commands_section | 2 | [016-5-Execute-Commands.md](016-5-Execute-Commands.md) |
| 5.1 WFS_CMD_VDM_ENTER_MODE_REQ | execute_command | 68 | [017-5.1-WFS-CMD-VDM-ENTER-MODE-REQ.md](017-5.1-WFS-CMD-VDM-ENTER-MODE-REQ.md) |
| 5.2 WFS_CMD_VDM_ENTER_MODE_ACK | execute_command | 35 | [018-5.2-WFS-CMD-VDM-ENTER-MODE-ACK.md](018-5.2-WFS-CMD-VDM-ENTER-MODE-ACK.md) |
| 5.3 WFS_CMD_VDM_EXIT_MODE_REQ | execute_command | 48 | [019-5.3-WFS-CMD-VDM-EXIT-MODE-REQ.md](019-5.3-WFS-CMD-VDM-EXIT-MODE-REQ.md) |
| 5.4 WFS_CMD_VDM_EXIT_MODE_ACK | execute_command | 31 | [020-5.4-WFS-CMD-VDM-EXIT-MODE-ACK.md](020-5.4-WFS-CMD-VDM-EXIT-MODE-ACK.md) |
| 5.5 WFS_CMD_VDM_SET_ACTIVE_INTERFACE | execute_command | 51 | [021-5.5-WFS-CMD-VDM-SET-ACTIVE-INTERFACE.md](021-5.5-WFS-CMD-VDM-SET-ACTIVE-INTERFACE.md) |
| 6 Events | events_section | 2 | [022-6-Events.md](022-6-Events.md) |
| 6.1 WFS_SRVE_VDM_ENTER_MODE_REQ | event | 17 | [023-6.1-WFS-SRVE-VDM-ENTER-MODE-REQ.md](023-6.1-WFS-SRVE-VDM-ENTER-MODE-REQ.md) |
| 6.2 WFS_SRVE_VDM_EXIT_MODE_REQ | event | 17 | [024-6.2-WFS-SRVE-VDM-EXIT-MODE-REQ.md](024-6.2-WFS-SRVE-VDM-EXIT-MODE-REQ.md) |
| 6.3 WFS_SYSE_VDM_MODEENTERED | api_function | 17 | [025-6.3-WFS-SYSE-VDM-MODEENTERED.md](025-6.3-WFS-SYSE-VDM-MODEENTERED.md) |
| 6.4 WFS_SYSE_VDM_MODEEXITED | api_function | 17 | [026-6.4-WFS-SYSE-VDM-MODEEXITED.md](026-6.4-WFS-SYSE-VDM-MODEEXITED.md) |
| 6.5 WFS_SRVE_VDM_INTERFACE_CHANGED | event | 29 | [027-6.5-WFS-SRVE-VDM-INTERFACE-CHANGED.md](027-6.5-WFS-SRVE-VDM-INTERFACE-CHANGED.md) |
| 7 C-Header file | header | 142 | [028-7-C-Header-file.md](028-7-C-Header-file.md) |
