---
description: Architecture and decisions for the XFS device simulator (Service Provider DLL + web host) and the desktop test tool
applyTo: "simulator/**,tools/DenoXfsTestTool/**"
---
# Simulator and test tool

Spec version 3.40. First versions are hand written; later HtmlToXfs2 generates the per-device-class modules. Follow `coding-principles`.

## Shape
- One tool per kind, device classes are modules (not one tool per device). Cores are device agnostic; modules are registered through DI keyed by device class and referenced statically.
- `simulator/DenoXfsSimCore` (domain, interfaces), `DenoXfsSimData` (EF Core + SQLite), `DenoXfsSimHost` (ASP.NET Core + Blazor Server: WebSocket for the DLL, terminal page, API), `DenoXfsSimSp` (the single C++ Service Provider DLL, class agnostic), `DenoXfsSimIdc` (IDC simulator module, Razor Class Library: seed JSON, card reader panel, API).
- `tools/DenoXfsTestTool` (WinForms + PropertyGrid, x86): tabs `Settings | Session | Info | Commands`; Info and Commands share one data-driven command panel with one inner tab per operation (multiline tabs). Tabs span the window; under every leaf tab a `WorkArea` holds the inputs and the one XFS log: on operation tabs a low strip above the log (`WorkAreaLayout.Top`: bold Run and Cancel on the left over the full strip height, parameters in `ParameterColumns` (5 rows per column, at most 3 columns, beyond 15 parameters the columns get more rows) with the description under them, height fitted via `IPreferredHeight`), on Settings/Session a narrow left column (`SharedLogPlacement` moves the log panel to the selected tab). Reuses `DenoXfsCore`. Class modules such as `DenoXfsTestToolIdc` only add metadata.

## Service Provider DLL (`DenoXfsSimSp`)
- C++, Visual Studio solution, **Win32/x86**: `C:\Windows\SysWOW64\msxfs.dll` is 32 bit, so the whole stack is 32 bit. Registry: `HKLM\SOFTWARE\WOW6432Node\XFS\SERVICE_PROVIDERS\{IDC,...}` (all pointing to the same DLL) and `HKEY_USERS\.DEFAULT\XFS\LOGICAL_SERVICES\{name}` with `class` and `provider`. The logical name has to match a service of the terminal's model (seed: `Idc340`), otherwise WFS_ERR_SERVICE_NOT_FOUND.
- Thin client: forwards command code and decoded input to the host over one WebSocket per `WFPOpen` session (JSON frames, binary as base64) and posts the host's completions and events to `hWnd` (only to registered event classes). Host unreachable: `WFS_ERR_CONNECTION_LOST`. The host decides by the logical name whether the open succeeds (see "Hardware catalog"); the DLL returns the `helloAck` hResult.
- Logging with spdlog (header-only, synchronous, readable `key=value` lines, one file per process). No sockets or threads in `DllMain`.

## Result memory
- The host builds results as a flat buffer plus pointer fixups (`ptrSize`, `data`, `fixups`); the DLL materializes them with `WFMAllocateBuffer` (the `WFSRESULT`) and `WFMAllocateMore` (the body) after bounds-checking every fixup.
- The receiving application frees every `WFSRESULT` with `WFSFreeResult`; the SP frees only results it never delivered. One `WFSRESULT` per recipient window. Copy `lpCmdData` before `WFPExecute` returns.
- Struct layouts must match the C++ compiler (`sizeof/offsetof` from the real `INCLUDE/XFS*.H`, pack 1), see the `Native_Layout_` exports of `XfsTestGen`.
- Modules never write buffers. Commands, infos and events return a `NativeValue` (`DenoXfsSimCore/Marshalling`): a `NativeLayout` plus the value as JSON with the spec field names (`{ "fwDevice": 0, ... }`, LPBYTE as hex). The core (`ExecuteMessageHandler`, `GetInfoMessageHandler`, `EventPublisher`, `ChannelExecuteEvents`) turns it into the block with `INativeLayoutWriter`; a value that does not fit gives `WFS_ERR_INTERNAL_ERROR`.
- Layout metadata per module (`DenoXfsSimIdc/Layouts/IdcLayouts.cs`: `IdcConstantSets`, `IdcStructs`, `IdcResults`), hand written from `XFSIDC.H` until HtmlToXfs2 generates it. Offsets follow from field order. Fields missing in the value stay 0/NULL; length/count fields are filled from the data unless given; numbers may be constant names (short `HWERROR` or full `WFS_IDC_DEVHWERROR`), flags an array of names. Unknown field names are rejected.

## Test tool (`tools/DenoXfsTestTool`)
- Layers: `Settings/` (editable `TestToolSettings`, JSON store), `Session/` (`SessionController`: StartUp, Open, SetTraceLevel, Register, Close, each async or sync by setting), `Operations/` (`IOperationDefinition`, `OperationDefinition<T>` template, `OperationInvoker`, `ObjectDumper`), `Operations/Idc/` (the two samples), `Ui/` (code-only WinForms, no designer), `Logging/` (`IXfsTrafficLog`).
- Operation tabs are ordered by command code (as in the spec: Status, Capabilities, ...; ReadTrack, WriteTrack, EjectCard, ...).
- Operations without parameters (`NoParameters`, e.g. Status, Capabilities) run when their tab is clicked. `[Flags]` enum parameters get `Ui/FlagsEnumEditor` (check box drop down), registered per parameter type by `OperationPage`. `ObjectDumper` writes arrays of simple values on one line.
- Cancel: the Cancel button under Run calls `OperationRun.Cancel`; `RunningCall` learns the request id (`ExecuteRequest.RequestIdAssigned`) or the blocking thread (`BlockingThreadStarted`) and sends `WFSCancelAsyncRequest` or `WFSCancelBlockingCall` (also when pressed before the id is known).
- Event payloads: `XfsEventMessage.Buffer` is valid only inside the event handler; `XfsEventLogger` decodes it there with the `IEventPayloadDecoder` of the event (`Operations/Idc/IdcEventDecoders.cs`, `ApiEventDecoders.cs`) and dumps it.
- Two logs: the screen shows only XFS traffic (`IXfsTrafficLog`: requests with input, completions with output, events, session steps, each named like `WFS_CMD_IDC_READ_RAW_DATA (207)`); Serilog writes the detailed log to the file only (XFS dumps at Debug). `OperationRunner` logs operations, `SessionController` session steps, `XfsEventLogger` events.
- Spec names come from `IXfsCodeNameSource` per device class (`Operations/Idc/IdcCodeNames.cs`, `ApiCodeNames.cs`), hand written from `XFS*.H` because the spec names are not derivable from the constants; HtmlToXfs2 will generate them.
- Add a command: create `OperationDefinition<TParameters>` (Name, Kind, Code, Description, `SendAsync`, `Decode`) and register it in `Extensions/ServiceCollectionExtensions.cs`. `Decode` must copy everything out of the native buffer (the result is freed right after); results are dumped into the XFS log by `ObjectDumper` (bytes as spaced hex, track data as ASCII with `\xNN` for non printable bytes).
- Needs the host running first; the Service Provider DLL is loaded by `msxfs.dll` into the test tool process. Do not start the host or the test tool from automation: the owner runs them in Visual Studio (port 5080, file locks).

## Current state
- Verified end to end through the real 32 bit Manager: open, register, `WFS_CMD_IDC_READ_RAW_DATA`, free result.
- All IDC infos and commands written, plus cancel, event payloads and the database (unit tested in `tests/DenoXfsSimTests`, DLL builds; owner's end to end check pending):
  - Host: `IInfoCommand` dispatch, per session execute queue with timeout, `ISessionRegistry` + `IEventPublisher`, IDC device state (`Device/IdcDevice`) and rules (`IIdcDeviceService`).
  - Forms (`Forms/`): `IIdcFormCatalog` and `ITrackDataFormatter`, shared by READ_TRACK and PARSE_DATA.
  - Web front end: see "Web front end" below (the old `/idc` page redirects to `/`).
  - Cancel: DLL `WFPCancelAsyncRequest` → `cancel` message → `IRequestCancellation`; the request (running or still queued) completes with `WFS_ERR_CANCELED`.
  - DLL:
    - command input decoders and separate GetInfo query decoders (codes overlap, e.g. 204);
    - `WFPRegister`/`WFPDeregister` tracking;
    - host `event` messages posted to the windows;
    - `cancel` messages (`WFS_ERR_INVALID_REQ_ID` for an unknown request).
  - Test tool: one operation per IDC info and command.
  - Contactless commands have no host handler and return `WFS_ERR_UNSUPP_COMMAND`.
- Generator: double null terminated `LPSTR` fields are listed in `fieldTypes` of `tools/HtmlToXfs2/naming-overrides.json`.
  - Type `MULTISZ` → `IReadOnlyList<string>?` via `MultiStringConvert`; XfsTestGen `Kind.MultiString`.
  - Regenerate: `HtmlToXfs2 --generate-types`, then `--copy-output` (needs `DENO_SRC`), then XfsTestGen.
  - Build `DenoXfsTestNative` with `-p:SolutionDir=<repo>\`.
- Scenarios: steps 1-3 done (layout writer; profiles and selection; Blazor terminal page). Next: step 4 (command/info rules + ready-made scenarios), then 5 (GUI scenario editor), 6 (card source / card taken). See "Scenarios".
- Hardware catalog (2026-11-04): vendor/model/device/service tables, terminals defined before use, open by name, routing by class, `/hardware` page; unit tested, owner's check in VS pending.
- Not done: form editing on the web page, DLL memory counters, x86 layout test (`sizeof/offsetof`), device state per device (several IDC services on one terminal).
- Registry: `simulator/registry/DenoXfsSimSp.reg` (provider `DENOXFS_IDC`, logical services `Idc340` and `IDC`; `IDC` opens only if a service with that name is added on `/hardware`). Another project's `SERVICE_PROVIDERS\IDC` exists on the dev machine; do not overwrite it.
- Logs: host `simulator/DenoXfsSimHost/logs`, test tool `bin/.../logs`, DLL `%TEMP%\DenoXfsSimSp-<pid>.log`.

## Web front end (Blazor Server, owner's decisions 2026-11-03)
- One terminal per web user: `/` shows the terminal whose IP is the browser's IP (`TerminalAddress.Normalize`, also used for the DLL connections; `?ip=` shows another). Header: `Terminal <terminal_id> · <IP>`, the model (`Deno · Deno26`), sessions, link to `/hardware`, scenario and device profile drop downs, link to `/scenarios`. An IP without a terminal shows a message with a link to define it (`/hardware?tab=terminals&ip=...`) and the live log (rejected opens are listed there).
- Only areas whose module class (`IDeviceUiModule.DeviceClass`) the terminal's model opens are active; the others are grey.
- `/hardware`: tab Catalog (tree vendor → model → device → service; add, edit, delete in the panel on the right, deletes ask there first; the service class list holds only the classes the device type allows, that a module implements and that the device has no service for) and tab Terminals (Terminal Id, address 1/2, model, sessions; add, edit, delete). REST: `GET /api/hardware`, `GET/POST /api/terminals`, `PUT/DELETE /api/terminals/{terminalId}`.
- Left: our own ATM SVG (`Components/AtmView.razor`): screen, PIN pad, card reader, receipt printer, cash dispenser, deposit. An area belongs to a module (`IDeviceUiModule`: area, title, panel component, `VisualAsync` → attention/fault/light/media/screen hint); areas without a module are grey. Right: the panel of the selected area (`DynamicComponent` with `Address`); a device that starts to need the user (a read waits for a card, card at the exit) opens its panel. Bottom: live log (`ITrafficFeed` filled by `ITrafficRecorder` from the message handlers, events and sessions; names from `IXfsCodeNames`: `ApiCodeNames`, `IdcCodeNames`).
- Live updates without polling: `ITerminalNotifier.Changed(address)` raised by `IdcDevice.Changed`, the traffic feed and selection changes; components re-render with `InvokeAsync(StateHasChanged)`.
- IDC panel (`DenoXfsSimIdc/Ui/IdcPanel.razor`): tabs Reader (state, card tiles to insert, take card, settings), Test cards (editor, `CardDtos`), Forms. Later modules (PIN pad, ...) add their own `IDeviceUiModule`.
- `/scenarios`: read only list until the GUI editor (step 5). REST APIs `/api/idc/*`, `/api/profiles/*` stay for automation.

## Hardware catalog (owner's decisions 2026-11-04)
- Vendor (Deno) → model (Deno26) → device (physical module, e.g. CardReader; a model may hold several of one type) → service (XFS logical service, e.g. `Idc340`, class `IDC`). A device opens at most one service per XFS class its type allows: two card readers give two IDC services, a Recycler one CDM and one CIM (shared cassettes), a Dispenser one CDM.
- Device types and their classes are fixed in code (`DenoXfsSimCore/Hardware/DeviceTypes.cs`): a type's behaviour needs code anyway.
- A terminal is defined before use and has exactly one model. Its key is the ISO 8583 field 41 Terminal Id, "ans 8": up to 8 printable ASCII characters, right padded with spaces by `TerminalIds.Normalize` before saving (stored `char(8)`, compared exactly), never changed (delete and add again). Address 1 and 2 are unique each; the catalog also rejects an IP used by another terminal, and a lookup takes the first match by Terminal Id. No terminal row is created automatically any more.
- `WFPOpen`: `HelloMessageHandler` resolves the logical name (case insensitive) among the services of the model of the terminal of the connection's IP (`IServiceResolver`). Unknown IP, unknown name or a class without a module → `WFS_ERR_SERVICE_NOT_FOUND`; otherwise `ISessionRegistry.Bind(channel, ResolvedService)`. A service name is unique within a model (checked by the catalog).
- Routing by class: modules register commands and infos keyed by their class (`AddKeyedSingleton<IExecuteCommand, X>("IDC")`) and an `IDeviceClassModule`; the core builds `IClassHandlers<T>` from them in the composition root. Execute/GetInfo use the class of the session's service; an unopened session or another class gives `WFS_ERR_UNSUPP_COMMAND` / `WFS_ERR_UNSUPP_CATEGORY`. `ExecuteContext.ServiceName` holds the logical service.
- Events: `IEventPublisher.PublishAsync(address, xfsClass, ...)` reaches only the sessions of that class.
- Core `Hardware/`: `IHardwareCatalog` and `ITerminalCatalog` check the rules over a cached `HardwareSnapshot` (`IHardwareSnapshots`), save through `IHardwareStore` / `ITerminalStore` and redraw the terminal pages. EF stores in `DenoXfsSimData/HardwareStores.cs`; `HardwareSeeder` fills an empty catalog with Deno → Deno26 → CardReader → Idc340 and an empty terminal table with `DEV00001` on `127.0.0.1` (deletions on the web stay).
- Device state is still per terminal + class (`deno_device_state`). Several services of one class on a terminal (two card readers) and the shared physical state of a Recycler need state per device: next step when a second module comes.

## Host
- SQLite (`DenoXfsSimData`, EF Core 9). Database names (only in the database; C# stays PascalCase): every table `deno_` + singular lower snake_case entity name, columns snake_case, PK `id` (terminal: `terminal_id`), FK `<entity>_id`, booleans `is_`, constraints `pk_<table>`, `fk_<table>_<columns>`, `ix_<table>_<columns>`. One convention in `SimDbContext.OnModelCreating` sets them; new entities need no names. Tables:
  - `deno_vendor(name)`, `deno_terminal_model(vendor_id, name)`, `deno_model_device(model_id, device_type, name)`, `deno_xfs_service(model_device_id, name, xfs_class)`: hardware catalog, deletes cascade downwards;
  - `deno_terminal(terminal_id char(8) PK, address1, address2?, model_id NOT NULL restrict, scenario_id?, device_profile_id?)`;
  - `deno_card`, `deno_card_chip_response`;
  - `deno_device_data(xfs_class, category, json)`: IDC forms are category `Forms`;
  - `deno_device_state(terminal_id, xfs_class, json)`: IDC settings + retain count; not stored for an IP without a terminal;
  - `deno_profile(kind, name NOCASE, description, based_on)` + `deno_profile_section(profile_id, xfs_class, json)`: scenarios and device profiles; `scenario_id` / `device_profile_id` of the terminal (null = Default, FK set null); a selection needs a terminal;
  - `NoteType` later.
- `DatabaseInitializer` applies the migrations at start and runs the idempotent `IDataSeeder`s (`HardwareSeeder`; `DenoXfsSimIdc/Seeds/*.json`, embedded). The IDC seeders only add what is missing by name, so edited cards stay.
- Migrations restarted on 2026-11-04 with one `Initial` (new names); an older `data/DenoXfsSim.db` must be deleted once.
- Another database later (SQL Server Express, PostgreSQL): the code stays, but (1) migrations are provider specific: a second migrations assembly (e.g. `DenoXfsSimDataSqlServer`) or a new initial migration; (2) `NOCASE` is applied only on SQLite (`Database.IsSqlite()`): SQL Server is case insensitive by default, PostgreSQL needs `citext` or a collation, and the Terminal Id needs a case sensitive collation on SQL Server; (3) SQL Server rejects the two SET NULL FKs of `deno_terminal` to `deno_profile` (multiple cascade paths): make one NO ACTION; (4) the `SqliteConnectionStringBuilder` path logic in the host goes behind a `Database:Provider` setting. Lower snake_case names need no quoting on any provider.
- Database file `data/DenoXfsSim.db` under the host content root (`ConnectionStrings:Simulator`).
- New migration: `dotnet ef migrations add <Name> --project simulator/DenoXfsSimData` (local tool, `dotnet-tools.json`).
- A pending `READ_RAW_DATA` waits for the user to pick a card on the terminal's web page; the host knows whether a page is connected.
- The host owns the device: per terminal state (media, chip power, retain count, lights, settings) and decides which event goes out when. The web UI is the simulator's front end, not only a card picker: reader settings (e.g. auto remove after eject), card selection/insert, take card, trigger removal, guidance lights; later scenarios (e.g. status values, READ_RAW_DATA returns invalid media).
- Execute requests of a session run one at a time with their timeout (`WFS_ERR_TIMEOUT`); the receive loop is never blocked, so GetInfo works while a read waits for a card. Events travel host → DLL as `event` messages (execute events to the request's window, the other classes to every window registered for them).

## IDC behaviour (owner's decisions)
- Cards: Track1/2/3 (ASCII text, may hold binary bytes), ATR, and chip APDU/response pairs (e.g. application selection, card number) used by `CHIP_IO`. Card chosen on the web page = inserted (`WFS_EXEE_IDC_MEDIAINSERTED`).
- `READ_RAW_DATA`: card inside → data; no card → `WFS_EXEE_IDC_INSERTCARD`, wait for a card from the web page (timeout).
- `EJECT_CARD`: card inside → OK, card at the exit. Auto remove setting on: `WFS_SRVE_IDC_MEDIAREMOVED` right after; off: sent when the user takes the card on the web page.
- `RETAIN_CARD`: card inside → retained, `WFS_EXEE_IDC_MEDIARETAINED`, `WFS_USRE_IDC_RETAINBINTHRESHOLD` when the bin state changes (high/full). `RESET_COUNT` sets the count to zero.
- `RESET`, `CHIP_POWER`, `CHIP_IO`, `PARSE_DATA`: implemented.
- `READ_TRACK` waits for a card like `READ_RAW_DATA` and formats it through a form (`TRACK2:PAN=..\0EXPIRY=..\0\0\0`); a track missing for the form's rule (`|` any, `&` all) gives `WFS_EXEE_IDC_INVALIDTRACKDATA` and `WFS_ERR_IDC_INVALIDDATA`.
- `WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS`: fixed answer. One IFM identifier. Forms from the database (seeded; editing later).
- `SETKEY`, `WRITE_RAW_DATA`, `WRITE_TRACK`: success when the input is valid. `SET_GUIDANCE_LIGHT`: OK, the web page shows the light when it is defined. `POWER_SAVE_CONTROL`, `PARK_CARD`: success.
- Contactless not supported: `EMVCLESS_CONFIGURE/PERFORM_TRANSACTION/ISSUERUPDATE` return an error; `SYNCHRONIZE_COMMAND` returns `WFS_ERR_IDC_COMMANDUNSUPP`.
- Work order: phase 1 = infrastructure, Status, Capabilities, ReadRawData, Eject, Retain, ResetCount, Reset, ChipPower, ChipIo, events, web front end; phase 2 = the rest.

## Scenarios (owner's decisions, 2026-11-02)
- All in the host; the DLL does not change. Per terminal two selections on the web page, stored in SQLite:
  - **Device profile** = hardware (capabilities, forms), e.g. `Motorised`, `Swipe`;
  - **Scenario** = behaviour and health (status, card source, card taken, command/info rules), e.g. `Default`, `Interactive`, `DeviceError`, `SlowDevice`, `NoResponse`.
- One JSON per device class each; fallback by deep merge onto `Default` (objects merge, arrays/scalars replace), optional `BasedOn`. Web JSON editor, strict (unknown properties rejected). No per-terminal conditional properties yet.
- The device always behaves as Default: nothing is derived from status. A non-default scenario states explicitly what each command returns (DeviceError sets the status *and* `commands."*"`).
- Rules under `commands` / `infos` keyed by spec name (`WFS_CMD_IDC_EJECT_CARD`), lookup exact > `*` > normal: `delayMs`, `hResult` (handler not run), `noResponse` (ends only by timeout/cancel/session close), `events` (with optional `payload`), `output`.
- `output` sets any field by spec name: patch by default (handler runs, the given fields overwrite; arrays matched by the layout's key field, e.g. `wDataSource`, else by index); `"replace": true` builds the output only from the scenario. Status/capabilities: fields not given come from state/device profile.
- Card source `Auto` (configured card, delay; still INSERTCARD/MEDIAINSERTED; missing card → Interactive) or `Interactive`; card taken `Auto` (replaces the `AutoRemoveAfterEject` setting) or `Interactive`. Retain capacity/threshold move into the device profile; retain count stays device state.
- No rule for a scenario switch during a running command (whatever is read at that moment applies); start/end-of-scenario commands maybe later. A switch that changes the device status publishes `WFS_SYSE_DEVICE_STATUS`.
- **Open thinking point: complex scenarios / scripting** (JSON now, decide when needed, e.g. "second read fails", "card X → Y", "10 % timeout", "jam after 3 ejects"):
  - Extension point (step 3): `IScenarioRule.ApplyAsync(RuleContext, ct) → RuleOutcome?`; context = terminal, command, input, device state, execute events, output `NativeValue`; outcome = delay / hResult / noResponse / output patch / events. Step 3 has only `JsonRule`; a `ScriptRule` would be chosen by `"script": ...` in the same JSON.
  - Options (none chosen): (a) JSON additions `times` / `sequence` / `when`; (b) C# scenario plugin project, compiled and debugged in VS, no sandbox, reload needed; (c) TypeScript in a Monaco editor (types from a `.d.ts` of the rule API, compiled in the browser) run by Jint (`TimeoutInterval`, `MaxStatements`, `LimitMemory`, no CLR access; debug by `log()` and a dry-run button); (d) Roslyn C# scripts from the web (compile errors shown, weak sandbox in .NET Core).
  - Sandbox concern: a script runs in the host process (endless loop blocks the terminal's queue or the host, memory blow-up, file/network access by mistake).
- Built (step 2): core `Scenarios/` (`IProfileCatalog`, `ITerminalSelectionStore` → `CachedTerminalSelections`, `IEffectiveProfileResolver` with cache, `IJsonMerger`, `IProfileSelectionService` + `IProfileSelectionListener`); EF stores in `DenoXfsSimData/ProfileStores.cs` (Data references Core); seeds `DenoXfsSimIdc/Seeds/profiles.json` (`IdcProfileSeeder` adds missing profiles and missing top-level keys only); `IIdcProfiles` (section keys `status` in both kinds, `capabilities` in device profiles; status = state ⊕ device profile ⊕ scenario); `IdcSelectionListener` publishes `WFS_SYSE_DEVICE_STATUS` (`WFSDEVSTATUS`, `ApiLayouts`) when fwDevice changes; host `POST /api/profiles/selection`, `GET /api/profiles`; `/idc` shows scenario, device profile and device state per terminal.
- Steps (revised 2026-11-03, UI first): (1) result models + layout writer ✔; (2) catalogs, tables, resolver, selection ✔; (3) Blazor terminal page ✔; (4) command/info rules + ready-made scenarios (Default, Interactive, DeviceError, Offline, ChipError, MagstripeError, CardJammed, RetainBinFull, SlowDevice, NoResponse; device profiles Default, Swipe, DipChipOnly); (5) GUI scenario editor generated from the layout metadata (add, copy, edit, delete; inherit per field; commands table; JSON as advanced tab); (6) card source / card taken.
