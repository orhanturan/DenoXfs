# XfsCim - CIM

Cash-In Module Device Class Interface

Source: `XfsCim.pdf` | sections: 113 | split level: 5

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 338 | [000-Front-matter.md](000-Front-matter.md) |
| 1. Introduction | section | 1 | [001-1-Introduction.md](001-1-Introduction.md) |
| 1.1 Background to Release 3.51 | section | 13 | [002-1.1-Background-to-Release-3-51.md](002-1.1-Background-to-Release-3-51.md) |
| 1.2 XFS Service-Specific Programming | section | 38 | [003-1.2-XFS-Service-Specific-Programming.md](003-1.2-XFS-Service-Specific-Programming.md) |
| 2. Cash-In Module | section | 41 | [004-2-Cash-In-Module.md](004-2-Cash-In-Module.md) |
| 3. References | section | 13 | [005-3-References.md](005-3-References.md) |
| 4. Note Classification | section | 50 | [006-4-Note-Classification.md](006-4-Note-Classification.md) |
| 5. Info Commands | info_commands_section | 1 | [007-5-Info-Commands.md](007-5-Info-Commands.md) |
| 5.1 WFS_INF_CIM_STATUS | info_command | 341 | [008-5.1-WFS-INF-CIM-STATUS.md](008-5.1-WFS-INF-CIM-STATUS.md) |
| 5.2 WFS_INF_CIM_CAPABILITIES | info_command | 403 | [009-5.2-WFS-INF-CIM-CAPABILITIES.md](009-5.2-WFS-INF-CIM-CAPABILITIES.md) |
| 5.3 WFS_INF_CIM_CASH_UNIT_INFO | info_command | 570 | [010-5.3-WFS-INF-CIM-CASH-UNIT-INFO.md](010-5.3-WFS-INF-CIM-CASH-UNIT-INFO.md) |
| 5.4 WFS_INF_CIM_TELLER_INFO | info_command | 97 | [011-5.4-WFS-INF-CIM-TELLER-INFO.md](011-5.4-WFS-INF-CIM-TELLER-INFO.md) |
| 5.5 WFS_INF_CIM_CURRENCY_EXP | info_command | 38 | [012-5.5-WFS-INF-CIM-CURRENCY-EXP.md](012-5.5-WFS-INF-CIM-CURRENCY-EXP.md) |
| 5.6 WFS_INF_CIM_BANKNOTE_TYPES | info_command | 43 | [013-5.6-WFS-INF-CIM-BANKNOTE-TYPES.md](013-5.6-WFS-INF-CIM-BANKNOTE-TYPES.md) |
| 5.7 WFS_INF_CIM_CASH_IN_STATUS | info_command | 80 | [014-5.7-WFS-INF-CIM-CASH-IN-STATUS.md](014-5.7-WFS-INF-CIM-CASH-IN-STATUS.md) |
| 5.8 WFS_INF_CIM_GET_P6_INFO | info_command | 53 | [015-5.8-WFS-INF-CIM-GET-P6-INFO.md](015-5.8-WFS-INF-CIM-GET-P6-INFO.md) |
| 5.9 WFS_INF_CIM_GET_P6_SIGNATURE | info_command | 100 | [016-5.9-WFS-INF-CIM-GET-P6-SIGNATURE.md](016-5.9-WFS-INF-CIM-GET-P6-SIGNATURE.md) |
| 5.10 WFS_INF_CIM_GET_ITEM_INFO | info_command | 98 | [017-5.10-WFS-INF-CIM-GET-ITEM-INFO.md](017-5.10-WFS-INF-CIM-GET-ITEM-INFO.md) |
| 5.11 WFS_INF_CIM_POSITION_CAPABILITIES | info_command | 113 | [018-5.11-WFS-INF-CIM-POSITION-CAPABILITIES.md](018-5.11-WFS-INF-CIM-POSITION-CAPABILITIES.md) |
| 5.12 WFS_INF_CIM_REPLENISH_TARGET | info_command | 36 | [019-5.12-WFS-INF-CIM-REPLENISH-TARGET.md](019-5.12-WFS-INF-CIM-REPLENISH-TARGET.md) |
| 5.13 WFS_INF_CIM_DEVICELOCK_STATUS | info_command | 49 | [020-5.13-WFS-INF-CIM-DEVICELOCK-STATUS.md](020-5.13-WFS-INF-CIM-DEVICELOCK-STATUS.md) |
| 5.14 WFS_INF_CIM_CASH_UNIT_CAPABILITIES | info_command | 99 | [021-5.14-WFS-INF-CIM-CASH-UNIT-CAPABILITIES.md](021-5.14-WFS-INF-CIM-CASH-UNIT-CAPABILITIES.md) |
| 5.15 WFS_INF_CIM_DEPLETE_SOURCE | info_command | 36 | [022-5.15-WFS-INF-CIM-DEPLETE-SOURCE.md](022-5.15-WFS-INF-CIM-DEPLETE-SOURCE.md) |
| 5.16 WFS_INF_CIM_GET_ALL_ITEMS_INFO | info_command | 211 | [023-5.16-WFS-INF-CIM-GET-ALL-ITEMS-INFO.md](023-5.16-WFS-INF-CIM-GET-ALL-ITEMS-INFO.md) |
| 5.17 WFS_INF_CIM_GET_BLACKLIST | info_command | 41 | [024-5.17-WFS-INF-CIM-GET-BLACKLIST.md](024-5.17-WFS-INF-CIM-GET-BLACKLIST.md) |
| 5.18 WFS_INF_CIM_GET_CLASSIFICATION_LIST | info_command | 68 | [025-5.18-WFS-INF-CIM-GET-CLASSIFICATION-LIST.md](025-5.18-WFS-INF-CIM-GET-CLASSIFICATION-LIST.md) |
| 5.19 WFS_INF_CIM_CASH_UNIT_COUNT_STATUS | info_command | 95 | [026-5.19-WFS-INF-CIM-CASH-UNIT-COUNT-STATUS.md](026-5.19-WFS-INF-CIM-CASH-UNIT-COUNT-STATUS.md) |
| 5.20 WFS_INF_CIM_PRESENT_STATUS | info_command | 85 | [027-5.20-WFS-INF-CIM-PRESENT-STATUS.md](027-5.20-WFS-INF-CIM-PRESENT-STATUS.md) |
| 6.1 WFS_CMD_CIM_CASH_IN_START | execute_command | 99 | [028-6.1-WFS-CMD-CIM-CASH-IN-START.md](028-6.1-WFS-CMD-CIM-CASH-IN-START.md) |
| 6.2 WFS_CMD_CIM_CASH_IN | execute_command | 153 | [029-6.2-WFS-CMD-CIM-CASH-IN.md](029-6.2-WFS-CMD-CIM-CASH-IN.md) |
| 6.3 WFS_CMD_CIM_CASH_IN_END | execute_command | 88 | [030-6.3-WFS-CMD-CIM-CASH-IN-END.md](030-6.3-WFS-CMD-CIM-CASH-IN-END.md) |
| 6.4 WFS_CMD_CIM_CASH_IN_ROLLBACK | execute_command | 94 | [031-6.4-WFS-CMD-CIM-CASH-IN-ROLLBACK.md](031-6.4-WFS-CMD-CIM-CASH-IN-ROLLBACK.md) |
| 6.5 WFS_CMD_CIM_RETRACT | execute_command | 148 | [032-6.5-WFS-CMD-CIM-RETRACT.md](032-6.5-WFS-CMD-CIM-RETRACT.md) |
| 6.6 WFS_CMD_CIM_OPEN_SHUTTER | execute_command | 65 | [033-6.6-WFS-CMD-CIM-OPEN-SHUTTER.md](033-6.6-WFS-CMD-CIM-OPEN-SHUTTER.md) |
| 6.7 WFS_CMD_CIM_CLOSE_SHUTTER | execute_command | 52 | [034-6.7-WFS-CMD-CIM-CLOSE-SHUTTER.md](034-6.7-WFS-CMD-CIM-CLOSE-SHUTTER.md) |
| 6.8 WFS_CMD_CIM_SET_TELLER_INFO | execute_command | 38 | [035-6.8-WFS-CMD-CIM-SET-TELLER-INFO.md](035-6.8-WFS-CMD-CIM-SET-TELLER-INFO.md) |
| 6.9 WFS_CMD_CIM_SET_CASH_UNIT_INFO | execute_command | 66 | [036-6.9-WFS-CMD-CIM-SET-CASH-UNIT-INFO.md](036-6.9-WFS-CMD-CIM-SET-CASH-UNIT-INFO.md) |
| 6.10 WFS_CMD_CIM_START_EXCHANGE | execute_command | 158 | [037-6.10-WFS-CMD-CIM-START-EXCHANGE.md](037-6.10-WFS-CMD-CIM-START-EXCHANGE.md) |
| 6.11 WFS_CMD_CIM_END_EXCHANGE | execute_command | 53 | [038-6.11-WFS-CMD-CIM-END-EXCHANGE.md](038-6.11-WFS-CMD-CIM-END-EXCHANGE.md) |
| 6.12 WFS_CMD_CIM_OPEN_SAFE_DOOR | execute_command | 15 | [039-6.12-WFS-CMD-CIM-OPEN-SAFE-DOOR.md](039-6.12-WFS-CMD-CIM-OPEN-SAFE-DOOR.md) |
| 6.13 WFS_CMD_CIM_RESET | execute_command | 131 | [040-6.13-WFS-CMD-CIM-RESET.md](040-6.13-WFS-CMD-CIM-RESET.md) |
| 6.14 WFS_CMD_CIM_CONFIGURE_CASH_IN_UNITS | execute_command | 79 | [041-6.14-WFS-CMD-CIM-CONFIGURE-CASH-IN-UNITS.md](041-6.14-WFS-CMD-CIM-CONFIGURE-CASH-IN-UNITS.md) |
| 6.15 WFS_CMD_CIM_CONFIGURE_NOTETYPES | execute_command | 22 | [042-6.15-WFS-CMD-CIM-CONFIGURE-NOTETYPES.md](042-6.15-WFS-CMD-CIM-CONFIGURE-NOTETYPES.md) |
| 6.16 WFS_CMD_CIM_CREATE_P6_SIGNATURE | execute_command | 125 | [043-6.16-WFS-CMD-CIM-CREATE-P6-SIGNATURE.md](043-6.16-WFS-CMD-CIM-CREATE-P6-SIGNATURE.md) |
| 6.17 WFS_CMD_CIM_SET_GUIDANCE_LIGHT | execute_command | 79 | [044-6.17-WFS-CMD-CIM-SET-GUIDANCE-LIGHT.md](044-6.17-WFS-CMD-CIM-SET-GUIDANCE-LIGHT.md) |
| 6.18 WFS_CMD_CIM_CONFIGURE_NOTE_READER | execute_command | 34 | [045-6.18-WFS-CMD-CIM-CONFIGURE-NOTE-READER.md](045-6.18-WFS-CMD-CIM-CONFIGURE-NOTE-READER.md) |
| 6.19 WFS_CMD_CIM_COMPARE_P6_SIGNATURE | execute_command | 97 | [046-6.19-WFS-CMD-CIM-COMPARE-P6-SIGNATURE.md](046-6.19-WFS-CMD-CIM-COMPARE-P6-SIGNATURE.md) |
| 6.20 WFS_CMD_CIM_POWER_SAVE_CONTROL | execute_command | 38 | [047-6.20-WFS-CMD-CIM-POWER-SAVE-CONTROL.md](047-6.20-WFS-CMD-CIM-POWER-SAVE-CONTROL.md) |
| 6.21 WFS_CMD_CIM_REPLENISH | execute_command | 132 | [048-6.21-WFS-CMD-CIM-REPLENISH.md](048-6.21-WFS-CMD-CIM-REPLENISH.md) |
| 6.22 WFS_CMD_CIM_SET_CASH_IN_LIMIT | execute_command | 121 | [049-6.22-WFS-CMD-CIM-SET-CASH-IN-LIMIT.md](049-6.22-WFS-CMD-CIM-SET-CASH-IN-LIMIT.md) |
| 6.23 WFS_CMD_CIM_CASH_UNIT_COUNT | execute_command | 76 | [050-6.23-WFS-CMD-CIM-CASH-UNIT-COUNT.md](050-6.23-WFS-CMD-CIM-CASH-UNIT-COUNT.md) |
| 6.24 WFS_CMD_CIM_DEVICE_LOCK_CONTROL | execute_command | 115 | [051-6.24-WFS-CMD-CIM-DEVICE-LOCK-CONTROL.md](051-6.24-WFS-CMD-CIM-DEVICE-LOCK-CONTROL.md) |
| 6.25 WFS_CMD_CIM_SET_MODE | execute_command | 33 | [052-6.25-WFS-CMD-CIM-SET-MODE.md](052-6.25-WFS-CMD-CIM-SET-MODE.md) |
| 6.26 WFS_CMD_CIM_PRESENT_MEDIA | execute_command | 73 | [053-6.26-WFS-CMD-CIM-PRESENT-MEDIA.md](053-6.26-WFS-CMD-CIM-PRESENT-MEDIA.md) |
| 6.27 WFS_CMD_CIM_DEPLETE | execute_command | 125 | [054-6.27-WFS-CMD-CIM-DEPLETE.md](054-6.27-WFS-CMD-CIM-DEPLETE.md) |
| 6.28 WFS_CMD_CIM_SET_BLACKLIST | execute_command | 36 | [055-6.28-WFS-CMD-CIM-SET-BLACKLIST.md](055-6.28-WFS-CMD-CIM-SET-BLACKLIST.md) |
| 6.29 WFS_CMD_CIM_SYNCHRONIZE_COMMAND | execute_command | 50 | [056-6.29-WFS-CMD-CIM-SYNCHRONIZE-COMMAND.md](056-6.29-WFS-CMD-CIM-SYNCHRONIZE-COMMAND.md) |
| 6.30 WFS_CMD_CIM_SET_CLASSIFICATION_LIST | execute_command | 25 | [057-6.30-WFS-CMD-CIM-SET-CLASSIFICATION-LIST.md](057-6.30-WFS-CMD-CIM-SET-CLASSIFICATION-LIST.md) |
| 6.31 WFS_CMD_CIM_PREPARE_PRESENT | execute_command | 50 | [058-6.31-WFS-CMD-CIM-PREPARE-PRESENT.md](058-6.31-WFS-CMD-CIM-PREPARE-PRESENT.md) |
| 7.1 WFS_SRVE_CIM_SAFEDOOROPEN | event | 7 | [059-7.1-WFS-SRVE-CIM-SAFEDOOROPEN.md](059-7.1-WFS-SRVE-CIM-SAFEDOOROPEN.md) |
| 7.2 WFS_SRVE_CIM_SAFEDOORCLOSED | event | 7 | [060-7.2-WFS-SRVE-CIM-SAFEDOORCLOSED.md](060-7.2-WFS-SRVE-CIM-SAFEDOORCLOSED.md) |
| 7.3 WFS_USRE_CIM_CASHUNITTHRESHOLD | event | 30 | [061-7.3-WFS-USRE-CIM-CASHUNITTHRESHOLD.md](061-7.3-WFS-USRE-CIM-CASHUNITTHRESHOLD.md) |
| 7.4 WFS_SRVE_CIM_CASHUNITINFOCHANGED | event | 31 | [062-7.4-WFS-SRVE-CIM-CASHUNITINFOCHANGED.md](062-7.4-WFS-SRVE-CIM-CASHUNITINFOCHANGED.md) |
| 7.5 WFS_SRVE_CIM_TELLERINFOCHANGED | event | 10 | [063-7.5-WFS-SRVE-CIM-TELLERINFOCHANGED.md](063-7.5-WFS-SRVE-CIM-TELLERINFOCHANGED.md) |
| 7.6 WFS_EXEE_CIM_CASHUNITERROR | event | 43 | [064-7.6-WFS-EXEE-CIM-CASHUNITERROR.md](064-7.6-WFS-EXEE-CIM-CASHUNITERROR.md) |
| 7.7 WFS_SRVE_CIM_ITEMSTAKEN | event | 34 | [065-7.7-WFS-SRVE-CIM-ITEMSTAKEN.md](065-7.7-WFS-SRVE-CIM-ITEMSTAKEN.md) |
| 7.8 WFS_SRVE_CIM_COUNTS_CHANGED | event | 20 | [066-7.8-WFS-SRVE-CIM-COUNTS-CHANGED.md](066-7.8-WFS-SRVE-CIM-COUNTS-CHANGED.md) |
| 7.9 WFS_EXEE_CIM_INPUTREFUSE | event | 39 | [067-7.9-WFS-EXEE-CIM-INPUTREFUSE.md](067-7.9-WFS-EXEE-CIM-INPUTREFUSE.md) |
| 7.10 WFS_SRVE_CIM_ITEMSPRESENTED | event | 47 | [068-7.10-WFS-SRVE-CIM-ITEMSPRESENTED.md](068-7.10-WFS-SRVE-CIM-ITEMSPRESENTED.md) |
| 7.11 WFS_SRVE_CIM_ITEMSINSERTED | event | 35 | [069-7.11-WFS-SRVE-CIM-ITEMSINSERTED.md](069-7.11-WFS-SRVE-CIM-ITEMSINSERTED.md) |
| 7.12 WFS_EXEE_CIM_NOTEERROR | event | 19 | [070-7.12-WFS-EXEE-CIM-NOTEERROR.md](070-7.12-WFS-EXEE-CIM-NOTEERROR.md) |
| 7.13 WFS_EXEE_CIM_SUBCASHIN | event | 13 | [071-7.13-WFS-EXEE-CIM-SUBCASHIN.md](071-7.13-WFS-EXEE-CIM-SUBCASHIN.md) |
| 7.14 WFS_SRVE_CIM_MEDIADETECTED | event | 11 | [072-7.14-WFS-SRVE-CIM-MEDIADETECTED.md](072-7.14-WFS-SRVE-CIM-MEDIADETECTED.md) |
| 7.15 WFS_EXEE_CIM_INPUT_P6 | event | 18 | [073-7.15-WFS-EXEE-CIM-INPUT-P6.md](073-7.15-WFS-EXEE-CIM-INPUT-P6.md) |
| 7.16 WFS_EXEE_CIM_INFO_AVAILABLE | event | 24 | [074-7.16-WFS-EXEE-CIM-INFO-AVAILABLE.md](074-7.16-WFS-EXEE-CIM-INFO-AVAILABLE.md) |
| 7.17 WFS_EXEE_CIM_INSERTITEMS | event | 7 | [075-7.17-WFS-EXEE-CIM-INSERTITEMS.md](075-7.17-WFS-EXEE-CIM-INSERTITEMS.md) |
| 7.18 WFS_SRVE_CIM_DEVICEPOSITION | event | 20 | [076-7.18-WFS-SRVE-CIM-DEVICEPOSITION.md](076-7.18-WFS-SRVE-CIM-DEVICEPOSITION.md) |
| 7.19 WFS_SRVE_CIM_POWER_SAVE_CHANGE | event | 15 | [077-7.19-WFS-SRVE-CIM-POWER-SAVE-CHANGE.md](077-7.19-WFS-SRVE-CIM-POWER-SAVE-CHANGE.md) |
| 7.20 WFS_EXEE_CIM_INCOMPLETEREPLENISH | event | 19 | [078-7.20-WFS-EXEE-CIM-INCOMPLETEREPLENISH.md](078-7.20-WFS-EXEE-CIM-INCOMPLETEREPLENISH.md) |
| 7.21 WFS_EXEE_CIM_INCOMPLETEDEPLETE | event | 18 | [079-7.21-WFS-EXEE-CIM-INCOMPLETEDEPLETE.md](079-7.21-WFS-EXEE-CIM-INCOMPLETEDEPLETE.md) |
| 7.22 WFS_SRVE_CIM_SHUTTERSTATUSCHANGED | event | 40 | [080-7.22-WFS-SRVE-CIM-SHUTTERSTATUSCHANGED.md](080-7.22-WFS-SRVE-CIM-SHUTTERSTATUSCHANGED.md) |
| 7.23 WFS_SRVE_CIM_COUNTACCURACYCHANGED | event | 10 | [081-7.23-WFS-SRVE-CIM-COUNTACCURACYCHANGED.md](081-7.23-WFS-SRVE-CIM-COUNTACCURACYCHANGED.md) |
| 7.24 WFS_EXEE_CIM_INCOMPLETERETRACT | event | 47 | [082-7.24-WFS-EXEE-CIM-INCOMPLETERETRACT.md](082-7.24-WFS-EXEE-CIM-INCOMPLETERETRACT.md) |
| 8.1 OK Transaction (Explicit Shutter Control) | section | 66 | [083-8.1-OK-Transaction-Explicit-Shutter-Control.md](083-8.1-OK-Transaction-Explicit-Shutter-Control.md) |
| 8.2 Cancellation by Customer (Explicit Shutter Control) | section | 48 | [084-8.2-Cancellation-by-Customer-Explicit-Shutter-Control.md](084-8.2-Cancellation-by-Customer-Explicit-Shutter-Control.md) |
| 8.3 Stacker Becomes Full (Explicit Shutter Control) | section | 82 | [085-8.3-Stacker-Becomes-Full-Explicit-Shutter-Control.md](085-8.3-Stacker-Becomes-Full-Explicit-Shutter-Control.md) |
| 8.4 Bill Recognition Error (Explicit Shutter Control) | section | 73 | [086-8.4-Bill-Recognition-Error-Explicit-Shutter-Control.md](086-8.4-Bill-Recognition-Error-Explicit-Shutter-Control.md) |
| 8.5 OK Transaction (Explicit Shutter Control) - Level 2 and 3 Note classification Supported | section | 70 | [087-8.5-OK-Transaction-Explicit-Shutter-Control-Level-2-and-3-Note-c.md](087-8.5-OK-Transaction-Explicit-Shutter-Control-Level-2-and-3-Note-c.md) |
| Control) | section | 77 | [088-Control.md](088-Control.md) |
| Control) | section | 52 | [089-Control.md](089-Control.md) |
| 8.8 OK Transaction (Implicit Shutter Control) | section | 58 | [090-8.8-OK-Transaction-Implicit-Shutter-Control.md](090-8.8-OK-Transaction-Implicit-Shutter-Control.md) |
| (Implicit Shutter Control and Implicit Present Control) | section | 362 | [091-Implicit-Shutter-Control-and-Implicit-Present-Control.md](091-Implicit-Shutter-Control-and-Implicit-Present-Control.md) |
| 8.14 Bill Recognition Error (WFS_CMD_CIM_PRESENT_MEDIA Command Supported) | execute_command | 98 | [092-8.14-Bill-Recognition-Error-WFS-CMD-CIM-PRESENT-MEDIA-Command-Sup.md](092-8.14-Bill-Recognition-Error-WFS-CMD-CIM-PRESENT-MEDIA-Command-Sup.md) |
| 8.16 Multiple Bunch Timeout Handling | section | 11 | [093-8.16-Multiple-Bunch-Timeout-Handling.md](093-8.16-Multiple-Bunch-Timeout-Handling.md) |
| 8.16.1 No Items Inserted | section | 32 | [094-8.16.1-No-Items-Inserted.md](094-8.16.1-No-Items-Inserted.md) |
| 8.16.2 First Bunch Not Taken | section | 119 | [095-8.16.2-First-Bunch-Not-Taken.md](095-8.16.2-First-Bunch-Not-Taken.md) |
| 8.17 Exchange using DEPOSITINTO (Implicit Shutter Control) | section | 112 | [096-8.17-Exchange-using-DEPOSITINTO-Implicit-Shutter-Control.md](096-8.17-Exchange-using-DEPOSITINTO-Implicit-Shutter-Control.md) |
| WFS_CMD_CIM_PREPARE_PRESENT) | execute_command | 87 | [097-WFS-CMD-CIM-PREPARE-PRESENT.md](097-WFS-CMD-CIM-PREPARE-PRESENT.md) |
| WFS_CMD_CIM_PREPARE_PRESENT) | execute_command | 40 | [098-WFS-CMD-CIM-PREPARE-PRESENT.md](098-WFS-CMD-CIM-PREPARE-PRESENT.md) |
| 9. ATM Mixed Media Transaction Flow – Application Guidelines | section | 78 | [099-9-ATM-Mixed-Media-Transaction-Flow-Application-Guidelines.md](099-9-ATM-Mixed-Media-Transaction-Flow-Application-Guidelines.md) |
| 9.1 Mixed Media OK Transaction | section | 109 | [100-9.1-Mixed-Media-OK-Transaction.md](100-9.1-Mixed-Media-OK-Transaction.md) |
| 9.2 Mixed Media Cancellation by Customer | section | 60 | [101-9.2-Mixed-Media-Cancellation-by-Customer.md](101-9.2-Mixed-Media-Cancellation-by-Customer.md) |
| 9.3 Mixed Media Cancellation by Customer on Cash Part Only | section | 52 | [102-9.3-Mixed-Media-Cancellation-by-Customer-on-Cash-Part-Only.md](102-9.3-Mixed-Media-Cancellation-by-Customer-on-Cash-Part-Only.md) |
| 9.4 Mixed Media Multiple Refused Items | section | 86 | [103-9.4-Mixed-Media-Multiple-Refused-Items.md](103-9.4-Mixed-Media-Multiple-Refused-Items.md) |
| 10. Rules for Cash Unit Exchange | section | 127 | [104-10-Rules-for-Cash-Unit-Exchange.md](104-10-Rules-for-Cash-Unit-Exchange.md) |
| 11. Events Associated with Cash Unit Status Changes | events_section | 3 | [105-11-Events-Associated-with-Cash-Unit-Status-Changes.md](105-11-Events-Associated-with-Cash-Unit-Status-Changes.md) |
| 11.1 One Physical Cash Unit Goes HIGH | section | 18 | [106-11.1-One-Physical-Cash-Unit-Goes-HIGH.md](106-11.1-One-Physical-Cash-Unit-Goes-HIGH.md) |
| 11.2 Last Physical Cash Unit Goes HIGH | section | 19 | [107-11.2-Last-Physical-Cash-Unit-Goes-HIGH.md](107-11.2-Last-Physical-Cash-Unit-Goes-HIGH.md) |
| 11.3 One Physical Cash Unit Goes INOP | section | 25 | [108-11.3-One-Physical-Cash-Unit-Goes-INOP.md](108-11.3-One-Physical-Cash-Unit-Goes-INOP.md) |
| 11.4 Last Physical Cash Unit Goes FULL | section | 32 | [109-11.4-Last-Physical-Cash-Unit-Goes-FULL.md](109-11.4-Last-Physical-Cash-Unit-Goes-FULL.md) |
| 12.1 Secure Cash In Data Parameter Example Data | section | 32 | [110-12.1-Secure-Cash-In-Data-Parameter-Example-Data.md](110-12.1-Secure-Cash-In-Data-Parameter-Example-Data.md) |
| 12.2 Secure E2E Transaction (Implicit Shutter Control) - OK | section | 391 | [111-12.2-Secure-E2E-Transaction-Implicit-Shutter-Control-OK.md](111-12.2-Secure-E2E-Transaction-Implicit-Shutter-Control-OK.md) |
| 13. C - Header file | header | 1106 | [112-13-C-Header-file.md](112-13-C-Header-file.md) |
