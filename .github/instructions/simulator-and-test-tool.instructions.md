---
description: Architecture and decisions for the XFS device simulator (Service Provider DLL + web host) and the desktop test tool
applyTo: "simulator/**,tools/DenoXfsTestTool/**"
---
# Simulator and test tool

Spec version 3.40. First versions are hand written; later HtmlToXfs2 generates the per-device-class modules. Follow `coding-principles`.

## Shape
- One tool per kind, device classes are modules (not one tool per device). Cores are device agnostic; modules are registered through DI keyed by device class and referenced statically.
- `simulator/DenoXfsSimCore` (domain, interfaces), `DenoXfsSimData` (EF Core + SQLite), `DenoXfsSimHost` (ASP.NET Core: WebSocket for the DLL, web UI, API), `DenoXfsSimSp` (the single C++ Service Provider DLL, class agnostic), `DenoXfsSimIdc` (IDC simulator module, seed JSON, web pages).
- `tools/DenoXfsTestTool` (WinForms + PropertyGrid, x86): tabs `Settings | Session | Info | Commands`; Info and Commands share one data-driven command panel with one inner tab per operation (multiline tabs). Tabs span the window; under every leaf tab a `WorkArea` holds the inputs (Run on top) in a narrow left column and the one XFS log on the right (`SharedLogPlacement` moves the log panel to the selected tab). Reuses `DenoXfsCore`. Class modules such as `DenoXfsTestToolIdc` only add metadata.

## Service Provider DLL (`DenoXfsSimSp`)
- C++, Visual Studio solution, **Win32/x86**: `C:\Windows\SysWOW64\msxfs.dll` is 32 bit, so the whole stack is 32 bit. Registry: `HKLM\SOFTWARE\WOW6432Node\XFS\SERVICE_PROVIDERS\{IDC,...}` (all pointing to the same DLL) and `HKEY_USERS\.DEFAULT\XFS\LOGICAL_SERVICES\{name}` with `class` and `provider`. Default logical service `IDC`; any name containing "IDC" is accepted.
- Thin client: forwards command code and decoded input to the host over one WebSocket per `WFPOpen` session (JSON frames, binary as base64) and posts the host's completions and events to `hWnd` (only to registered event classes). Host unreachable: `WFS_ERR_CONNECTION_LOST`.
- Logging with spdlog (header-only, synchronous, readable `key=value` lines, one file per process). No sockets or threads in `DllMain`.

## Result memory
- The host builds results as a flat buffer plus pointer fixups (`ptrSize`, `data`, `fixups`); the DLL materializes them with `WFMAllocateBuffer` (the `WFSRESULT`) and `WFMAllocateMore` (the body) after bounds-checking every fixup.
- The receiving application frees every `WFSRESULT` with `WFSFreeResult`; the SP frees only results it never delivered. One `WFSRESULT` per recipient window. Copy `lpCmdData` before `WFPExecute` returns.
- Struct layouts must match the C++ compiler (`sizeof/offsetof` from the real `INCLUDE/XFS*.H`, pack 1), see the `Native_Layout_` exports of `XfsTestGen`.

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
  - Web front end `/idc`:
    - insert/take card;
    - reader settings (auto remove, retain bin size and threshold);
    - guidance light, power save time;
    - card editor (tracks with `\xNN`, ATR, APDU/response rows);
    - forms.
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
- Not done: scenarios, form editing on the web page, DLL memory counters, x86 layout test (`sizeof/offsetof`).
- Registry: `simulator/registry/DenoXfsSimSp.reg` (provider `DENOXFS_IDC`, logical service `IDC`). Another project's `SERVICE_PROVIDERS\IDC` exists on the dev machine; do not overwrite it.
- Logs: host `simulator/DenoXfsSimHost/logs`, test tool `bin/.../logs`, DLL `%TEMP%\DenoXfsSimSp-<pid>.log`.

## Host
- SQLite (`DenoXfsSimData`, EF Core 9). Tables:
  - `Terminal(Address1, Address2?)`, resolved from the connection's remote IP;
  - `Card`, `CardChipResponse`;
  - `DeviceData(DeviceClass, Category, Json)`: IDC forms are category `Forms`;
  - `DeviceState(TerminalId, DeviceClass, Json)`: IDC settings + retain count;
  - `NoteType` later.
- `DatabaseInitializer` applies the migrations at start and runs the idempotent `IDataSeeder`s (`DenoXfsSimIdc/Seeds/*.json`, embedded). They only add what is missing by name, so edited cards stay.
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
