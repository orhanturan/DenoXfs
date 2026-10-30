# XfsApi - API/SPI

Application Programming Interface / Service Provider Interface

Source: `XfsApi.pdf` | sections: 128 | split level: 3

| Section | Type | Lines | File |
|---|---|---|---|
| Front matter | front_matter | 170 | [000-Front-matter.md](000-Front-matter.md) |
| European Foreword | section | 145 | [001-European-Foreword.md](001-European-Foreword.md) |
| 1 Introduction | section | 1 | [002-1-Introduction.md](002-1-Introduction.md) |
| 1.1 Background to Release 3.40 | section | 25 | [003-1.1-Background-to-Release-3-40.md](003-1.1-Background-to-Release-3-40.md) |
| 2 References | section | 6 | [004-2-References.md](004-2-References.md) |
| 3 XFS (eXtensions for Financial Services) Overview | section | 17 | [005-3-XFS-eXtensions-for-Financial-Services-Overview.md](005-3-XFS-eXtensions-for-Financial-Services-Overview.md) |
| 3.1 Architecture | section | 95 | [006-3.1-Architecture.md](006-3.1-Architecture.md) |
| 3.2 API and SPI Summary | section | 44 | [007-3.2-API-and-SPI-Summary.md](007-3.2-API-and-SPI-Summary.md) |
| 3.3 Device Classes | section | 6 | [008-3.3-Device-Classes.md](008-3.3-Device-Classes.md) |
| 3.4 Unicode Encoding Summary | section | 10 | [009-3.4-Unicode-Encoding-Summary.md](009-3.4-Unicode-Encoding-Summary.md) |
| 4 Architectural and Implementation Issues | section | 42 | [010-4-Architectural-and-Implementation-Issues.md](010-4-Architectural-and-Implementation-Issues.md) |
| 4.1 The XFS Manager | section | 23 | [011-4.1-The-XFS-Manager.md](011-4.1-The-XFS-Manager.md) |
| 4.2 Service Providers | section | 5 | [012-4.2-Service-Providers.md](012-4.2-Service-Providers.md) |
| 4.2.1 Service Provider Functionality | section | 29 | [013-4.2.1-Service-Provider-Functionality.md](013-4.2.1-Service-Provider-Functionality.md) |
| 4.2.2 Service Provider “Packaging” | section | 10 | [014-4.2.2-Service-Provider-Packaging.md](014-4.2.2-Service-Provider-Packaging.md) |
| 4.3 Asynchronous, Synchronous and Immediate Functions | section | 10 | [015-4.3-Asynchronous-Synchronous-and-Immediate-Functions.md](015-4.3-Asynchronous-Synchronous-and-Immediate-Functions.md) |
| 4.3.1 Asynchronous Functions | section | 17 | [016-4.3.1-Asynchronous-Functions.md](016-4.3.1-Asynchronous-Functions.md) |
| 4.3.2 Synchronous Functions | section | 27 | [017-4.3.2-Synchronous-Functions.md](017-4.3.2-Synchronous-Functions.md) |
| 4.3.3 Immediate Functions | section | 12 | [018-4.3.3-Immediate-Functions.md](018-4.3.3-Immediate-Functions.md) |
| 4.4 Processing API Functions | section | 19 | [019-4.4-Processing-API-Functions.md](019-4.4-Processing-API-Functions.md) |
| 4.5 Opening a Session | section | 54 | [020-4.5-Opening-a-Session.md](020-4.5-Opening-a-Session.md) |
| 4.6 Closing a Session | section | 27 | [021-4.6-Closing-a-Session.md](021-4.6-Closing-a-Session.md) |
| 4.7 Configuration Information | section | 164 | [022-4.7-Configuration-Information.md](022-4.7-Configuration-Information.md) |
| 4.8 Exclusive Service and Device Access | section | 34 | [023-4.8-Exclusive-Service-and-Device-Access.md](023-4.8-Exclusive-Service-and-Device-Access.md) |
| 4.8.1 Lock Policy for Independent Devices | section | 46 | [024-4.8.1-Lock-Policy-for-Independent-Devices.md](024-4.8.1-Lock-Policy-for-Independent-Devices.md) |
| 4.8.2 Compound Devices | section | 67 | [025-4.8.2-Compound-Devices.md](025-4.8.2-Compound-Devices.md) |
| 4.9 Timeout | section | 16 | [026-4.9-Timeout.md](026-4.9-Timeout.md) |
| 4.10 Function Status Return | section | 41 | [027-4.10-Function-Status-Return.md](027-4.10-Function-Status-Return.md) |
| 4.11 Notification Mechanisms - Registering for Events | events_section | 60 | [028-4.11-Notification-Mechanisms-Registering-for-Events.md](028-4.11-Notification-Mechanisms-Registering-for-Events.md) |
| 4.12 Application Processes, Threads and Blocking Functions | section | 79 | [029-4.12-Application-Processes-Threads-and-Blocking-Functions.md](029-4.12-Application-Processes-Threads-and-Blocking-Functions.md) |
| 4.13 Vendor Dependent Mode | section | 14 | [030-4.13-Vendor-Dependent-Mode.md](030-4.13-Vendor-Dependent-Mode.md) |
| 4.14 Memory Management | section | 77 | [031-4.14-Memory-Management.md](031-4.14-Memory-Management.md) |
| 4.15 Command Synchronization | section | 10 | [032-4.15-Command-Synchronization.md](032-4.15-Command-Synchronization.md) |
| 4.16 Binary Interface | section | 59 | [033-4.16-Binary-Interface.md](033-4.16-Binary-Interface.md) |
| 5 Application Programming Interface (API) Functions | section | 24 | [034-5-Application-Programming-Interface-API-Functions.md](034-5-Application-Programming-Interface-API-Functions.md) |
| 5.1 WFSCancelAsyncRequest | api_function | 1 | [035-5.1-WFSCancelAsyncRequest.md](035-5.1-WFSCancelAsyncRequest.md) |
| 5.2 WFSCancelBlockingCall | api_function | 1 | [036-5.2-WFSCancelBlockingCall.md](036-5.2-WFSCancelBlockingCall.md) |
| 5.3 WFSCleanUp | api_function | 2 | [037-5.3-WFSCleanUp.md](037-5.3-WFSCleanUp.md) |
| 5.4 WFSClose | api_function | 2 | [038-5.4-WFSClose.md](038-5.4-WFSClose.md) |
| 5.5 WFSAsyncClose | api_function | 1 | [039-5.5-WFSAsyncClose.md](039-5.5-WFSAsyncClose.md) |
| 5.6 WFSCreateAppHandle | api_function | 2 | [040-5.6-WFSCreateAppHandle.md](040-5.6-WFSCreateAppHandle.md) |
| 5.7 WFSDeregister | api_function | 2 | [041-5.7-WFSDeregister.md](041-5.7-WFSDeregister.md) |
| 5.8 WFSAsyncDeregister | api_function | 1 | [042-5.8-WFSAsyncDeregister.md](042-5.8-WFSAsyncDeregister.md) |
| 5.9 WFSDestroyAppHandle | api_function | 1 | [043-5.9-WFSDestroyAppHandle.md](043-5.9-WFSDestroyAppHandle.md) |
| 5.10 WFSExecute | api_function | 2 | [044-5.10-WFSExecute.md](044-5.10-WFSExecute.md) |
| 5.11 WFSAsyncExecute | api_function | 1 | [045-5.11-WFSAsyncExecute.md](045-5.11-WFSAsyncExecute.md) |
| 5.12 WFSFreeResult | api_function | 1 | [046-5.12-WFSFreeResult.md](046-5.12-WFSFreeResult.md) |
| 5.13 WFSGetInfo | api_function | 2 | [047-5.13-WFSGetInfo.md](047-5.13-WFSGetInfo.md) |
| 5.14 WFSAsyncGetInfo | api_function | 1 | [048-5.14-WFSAsyncGetInfo.md](048-5.14-WFSAsyncGetInfo.md) |
| 5.15 WFSIsBlocking | api_function | 1 | [049-5.15-WFSIsBlocking.md](049-5.15-WFSIsBlocking.md) |
| 5.16 WFSLock | api_function | 2 | [050-5.16-WFSLock.md](050-5.16-WFSLock.md) |
| 5.17 WFSAsyncLock | api_function | 1 | [051-5.17-WFSAsyncLock.md](051-5.17-WFSAsyncLock.md) |
| 5.18 WFSOpen | api_function | 2 | [052-5.18-WFSOpen.md](052-5.18-WFSOpen.md) |
| 5.19 WFSAsyncOpen | api_function | 1 | [053-5.19-WFSAsyncOpen.md](053-5.19-WFSAsyncOpen.md) |
| 5.20 WFSRegister | api_function | 2 | [054-5.20-WFSRegister.md](054-5.20-WFSRegister.md) |
| 5.21 WFSAsyncRegister | api_function | 1 | [055-5.21-WFSAsyncRegister.md](055-5.21-WFSAsyncRegister.md) |
| 5.22 WFSSetBlockingHook | api_function | 1 | [056-5.22-WFSSetBlockingHook.md](056-5.22-WFSSetBlockingHook.md) |
| 5.23 WFSStartUp | api_function | 2 | [057-5.23-WFSStartUp.md](057-5.23-WFSStartUp.md) |
| 5.24 WFSUnhookBlockingHook | api_function | 1 | [058-5.24-WFSUnhookBlockingHook.md](058-5.24-WFSUnhookBlockingHook.md) |
| 5.25 WFSUnlock | api_function | 2 | [059-5.25-WFSUnlock.md](059-5.25-WFSUnlock.md) |
| 5.26 WFSAsyncUnlock | api_function | 1385 | [060-5.26-WFSAsyncUnlock.md](060-5.26-WFSAsyncUnlock.md) |
| 6 Service Provider Interface (SPI) Functions | section | 12 | [061-6-Service-Provider-Interface-SPI-Functions.md](061-6-Service-Provider-Interface-SPI-Functions.md) |
| 6.1 WFPCancelAsyncRequest | spi_function | 76 | [062-6.1-WFPCancelAsyncRequest.md](062-6.1-WFPCancelAsyncRequest.md) |
| 6.2 WFPClose | spi_function | 37 | [063-6.2-WFPClose.md](063-6.2-WFPClose.md) |
| 6.3 WFPDeregister | spi_function | 54 | [064-6.3-WFPDeregister.md](064-6.3-WFPDeregister.md) |
| 6.4 WFPExecute | spi_function | 72 | [065-6.4-WFPExecute.md](065-6.4-WFPExecute.md) |
| 6.5 WFPGetInfo | spi_function | 77 | [066-6.5-WFPGetInfo.md](066-6.5-WFPGetInfo.md) |
| 6.6 WFPLock | spi_function | 45 | [067-6.6-WFPLock.md](067-6.6-WFPLock.md) |
| 6.7 WFPOpen | spi_function | 168 | [068-6.7-WFPOpen.md](068-6.7-WFPOpen.md) |
| 6.8 WFPRegister | spi_function | 43 | [069-6.8-WFPRegister.md](069-6.8-WFPRegister.md) |
| 6.9 WFPSetTraceLevel | spi_function | 49 | [070-6.9-WFPSetTraceLevel.md](070-6.9-WFPSetTraceLevel.md) |
| 6.10 WFPUnloadService | spi_function | 18 | [071-6.10-WFPUnloadService.md](071-6.10-WFPUnloadService.md) |
| 6.11 WFPUnlock | spi_function | 35 | [072-6.11-WFPUnlock.md](072-6.11-WFPUnlock.md) |
| 7 Support Functions | section | 4 | [073-7-Support-Functions.md](073-7-Support-Functions.md) |
| 7.1 WFMAllocateBuffer | support_function | 27 | [074-7.1-WFMAllocateBuffer.md](074-7.1-WFMAllocateBuffer.md) |
| 7.2 WFMAllocateMore | support_function | 25 | [075-7.2-WFMAllocateMore.md](075-7.2-WFMAllocateMore.md) |
| 7.3 WFMFreeBuffer | support_function | 18 | [076-7.3-WFMFreeBuffer.md](076-7.3-WFMFreeBuffer.md) |
| 7.4 WFMGetTraceLevel | support_function | 24 | [077-7.4-WFMGetTraceLevel.md](077-7.4-WFMGetTraceLevel.md) |
| 7.5 WFMKillTimer | support_function | 13 | [078-7.5-WFMKillTimer.md](078-7.5-WFMKillTimer.md) |
| 7.6 WFMOutputTraceData | support_function | 15 | [079-7.6-WFMOutputTraceData.md](079-7.6-WFMOutputTraceData.md) |
| 7.7 WFMReleaseDLL | support_function | 24 | [080-7.7-WFMReleaseDLL.md](080-7.7-WFMReleaseDLL.md) |
| 7.8 WFMSetTimer | support_function | 22 | [081-7.8-WFMSetTimer.md](081-7.8-WFMSetTimer.md) |
| 7.9 WFMSetTraceLevel | support_function | 57 | [082-7.9-WFMSetTraceLevel.md](082-7.9-WFMSetTraceLevel.md) |
| 8 Configuration Functions | section | 3 | [083-8-Configuration-Functions.md](083-8-Configuration-Functions.md) |
| 8.1 WFMCloseKey | support_function | 15 | [084-8.1-WFMCloseKey.md](084-8.1-WFMCloseKey.md) |
| 8.2 WFMCreateKey | support_function | 35 | [085-8.2-WFMCreateKey.md](085-8.2-WFMCreateKey.md) |
| 8.3 WFMDeleteKey | support_function | 23 | [086-8.3-WFMDeleteKey.md](086-8.3-WFMDeleteKey.md) |
| 8.4 WFMDeleteValue | support_function | 19 | [087-8.4-WFMDeleteValue.md](087-8.4-WFMDeleteValue.md) |
| 8.5 WFMEnumKey | support_function | 42 | [088-8.5-WFMEnumKey.md](088-8.5-WFMEnumKey.md) |
| 8.6 WFMEnumValue | support_function | 44 | [089-8.6-WFMEnumValue.md](089-8.6-WFMEnumValue.md) |
| 8.7 WFMOpenKey | support_function | 24 | [090-8.7-WFMOpenKey.md](090-8.7-WFMOpenKey.md) |
| 8.8 WFMQueryValue | support_function | 29 | [091-8.8-WFMQueryValue.md](091-8.8-WFMQueryValue.md) |
| 8.9 WFMSetValue | support_function | 25 | [092-8.9-WFMSetValue.md](092-8.9-WFMSetValue.md) |
| 9 Data Structures | section | 1 | [093-9-Data-Structures.md](093-9-Data-Structures.md) |
| 9.1 WFSRESULT | api_function | 37 | [094-9.1-WFSRESULT.md](094-9.1-WFSRESULT.md) |
| 9.2 WFSVERSION | api_function | 25 | [095-9.2-WFSVERSION.md](095-9.2-WFSVERSION.md) |
| 10 Messages | section | 2 | [096-10-Messages.md](096-10-Messages.md) |
| 10.1 Command Completions and Events | events_section | 10 | [097-10.1-Command-Completions-and-Events.md](097-10.1-Command-Completions-and-Events.md) |
| 10.1.1 Command Completion Messages | section | 9 | [098-10.1.1-Command-Completion-Messages.md](098-10.1.1-Command-Completion-Messages.md) |
| 10.1.2 Event Messages | events_section | 10 | [099-10.1.2-Event-Messages.md](099-10.1.2-Event-Messages.md) |
| 10.2 WFS_TIMER_EVENT | api_function | 9 | [100-10.2-WFS-TIMER-EVENT.md](100-10.2-WFS-TIMER-EVENT.md) |
| 10.3 WFS_SYSE_DEVICE_STATUS | api_function | 50 | [101-10.3-WFS-SYSE-DEVICE-STATUS.md](101-10.3-WFS-SYSE-DEVICE-STATUS.md) |
| 10.4 WFS_SYSE_UNDELIVERABLE_MSG | api_function | 36 | [102-10.4-WFS-SYSE-UNDELIVERABLE-MSG.md](102-10.4-WFS-SYSE-UNDELIVERABLE-MSG.md) |
| 10.5 WFS_SYSE_APP_DISCONNECT | api_function | 24 | [103-10.5-WFS-SYSE-APP-DISCONNECT.md](103-10.5-WFS-SYSE-APP-DISCONNECT.md) |
| WFS_SYSE_USER_ERROR and WFS_SYSE_FRAUD_ATTEMPT | api_function | 103 | [104-WFS-SYSE-USER-ERROR-and-WFS-SYSE-FRAUD-ATTEMPT.md](104-WFS-SYSE-USER-ERROR-and-WFS-SYSE-FRAUD-ATTEMPT.md) |
| 10.7 WFS_SYSE_LOCK_REQUESTED | api_function | 15 | [105-10.7-WFS-SYSE-LOCK-REQUESTED.md](105-10.7-WFS-SYSE-LOCK-REQUESTED.md) |
| 10.8 WFS_SYSE_VERSION_ERROR | api_function | 31 | [106-10.8-WFS-SYSE-VERSION-ERROR.md](106-10.8-WFS-SYSE-VERSION-ERROR.md) |
| 11 Error Codes | section | 141 | [107-11-Error-Codes.md](107-11-Error-Codes.md) |
| 12 Common GetInfo, Execute Commands and Messages | execute_commands_section | 1 | [108-12-Common-GetInfo-Execute-Commands-and-Messages.md](108-12-Common-GetInfo-Execute-Commands-and-Messages.md) |
| 12.1 Common GetInfo Commands | info_commands_section | 1 | [109-12.1-Common-GetInfo-Commands.md](109-12.1-Common-GetInfo-Commands.md) |
| 12.1.1 WFS_INF_API_TRANSACTION_STATE | info_command | 42 | [110-12.1.1-WFS-INF-API-TRANSACTION-STATE.md](110-12.1.1-WFS-INF-API-TRANSACTION-STATE.md) |
| 12.1.2 WFS_INF_API_SERVICE_INFO | info_command | 173 | [111-12.1.2-WFS-INF-API-SERVICE-INFO.md](111-12.1.2-WFS-INF-API-SERVICE-INFO.md) |
| 12.2 Common Execute Commands | execute_commands_section | 1 | [112-12.2-Common-Execute-Commands.md](112-12.2-Common-Execute-Commands.md) |
| 12.2.1 WFS_CMD_API_SET_TRANSACTION_STATE | execute_command | 18 | [113-12.2.1-WFS-CMD-API-SET-TRANSACTION-STATE.md](113-12.2.1-WFS-CMD-API-SET-TRANSACTION-STATE.md) |
| 12.3 Common Messages | section | 1 | [114-12.3-Common-Messages.md](114-12.3-Common-Messages.md) |
| 12.3.1 WFS_SRVE_API_STATUS_CHANGED | event | 22 | [115-12.3.1-WFS-SRVE-API-STATUS-CHANGED.md](115-12.3.1-WFS-SRVE-API-STATUS-CHANGED.md) |
| 12.3.2 WFS_EXEE_API_ERROR_INFO | event | 24 | [116-12.3.2-WFS-EXEE-API-ERROR-INFO.md](116-12.3.2-WFS-EXEE-API-ERROR-INFO.md) |
| 13 Appendix A - Planned Enhancements and Extensions | appendix | 17 | [117-13-Appendix-A-Planned-Enhancements-and-Extensions.md](117-13-Appendix-A-Planned-Enhancements-and-Extensions.md) |
| 13.1 Event and System Management | events_section | 11 | [118-13.1-Event-and-System-Management.md](118-13.1-Event-and-System-Management.md) |
| 14 Appendix B - XFS Workshop Contacts | appendix | 13 | [119-14-Appendix-B-XFS-Workshop-Contacts.md](119-14-Appendix-B-XFS-Workshop-Contacts.md) |
| 15 Appendix C - ATM Devices Synchronization Flow | appendix | 3 | [120-15-Appendix-C-ATM-Devices-Synchronization-Flow.md](120-15-Appendix-C-ATM-Devices-Synchronization-Flow.md) |
| 15.1 Synchronized Media Ejection | section | 33 | [121-15.1-Synchronized-Media-Ejection.md](121-15.1-Synchronized-Media-Ejection.md) |
| 16 Appendix D – Win64 Migration Considerations | appendix | 34 | [122-16-Appendix-D-Win64-Migration-Considerations.md](122-16-Appendix-D-Win64-Migration-Considerations.md) |
| 17 Appendix D - C-Header files | header | 1 | [123-17-Appendix-D-C-Header-files.md](123-17-Appendix-D-C-Header-files.md) |
| 17.1 XFSAPI.H | section | 378 | [124-17.1-XFSAPI-H.md](124-17.1-XFSAPI-H.md) |
| 17.2 XFSADMIN.H | section | 40 | [125-17.2-XFSADMIN-H.md](125-17.2-XFSADMIN-H.md) |
| 17.3 XFSCONF.H | section | 59 | [126-17.3-XFSCONF-H.md](126-17.3-XFSCONF-H.md) |
| 17.4 XFSSPI.H | section | 46 | [127-17.4-XFSSPI-H.md](127-17.4-XFSSPI-H.md) |
