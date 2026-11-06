using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Names;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Data;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Commands;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Infos;
using DenoXfs.Sim.Idc.Scenarios;
using DenoXfs.Sim.Idc.Seeds;
using DenoXfs.Sim.Idc.Ui;
using Microsoft.Extensions.DependencyInjection;

namespace DenoXfs.Sim.Idc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDenoXfsSimIdc(this IServiceCollection services)
    {
        // Cards, forms and per terminal state live in the simulator database (AddDenoXfsSimData); the seeds fill it.
        services.AddSingleton<ICardCatalog, EfCardCatalog>();
        services.AddSingleton<IIdcFormCatalog, EfFormCatalog>();
        services.AddSingleton<IIdcDeviceStateStore, EfDeviceStateStore>();
        services.AddSingleton<IDataSeeder, IdcCardSeeder>();
        services.AddSingleton<IDataSeeder, IdcFormSeeder>();
        services.AddSingleton<IDataSeeder, IdcProfileSeeder>();
        services.AddSingleton<IIdcDeviceRegistry, IdcDeviceRegistry>();
        services.AddSingleton<IIdcReaders, IdcReaders>();
        services.AddSingleton<IIdcDeviceService, IdcDeviceService>();
        services.AddSingleton<ITrackDataFormatter, TrackDataFormatter>();
        services.AddSingleton<FormReadResult>();

        // Scenario and device profile sections of this class (status, capabilities) and the device status event on a switch.
        services.AddSingleton<IIdcProfiles, IdcProfiles>();
        services.AddSingleton<IProfileSelectionListener, IdcSelectionListener>();

        // Web front end: spec names for the live log, the card reader area and panel of the terminal page.
        services.AddSingleton<IXfsCodeNameSource, IdcCodeNames>();
        services.AddSingleton<IDeviceUiModule, IdcUiModule>();

        // Info categories and commands: one line each, keyed by the class, so they serve only sessions of an IDC service.
        services.AddSingleton<IDeviceClassModule>(new DeviceClassModule(IdcProfiles.DeviceClass));
        services.AddKeyedSingleton<IInfoCommand, StatusInfo>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IInfoCommand, CapabilitiesInfo>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IInfoCommand, FormListInfo>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IInfoCommand, QueryFormInfo>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IInfoCommand, QueryIfmIdentifierInfo>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IInfoCommand, EmvclessQueryApplicationsInfo>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, ReadTrackCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, WriteTrackCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, WriteRawDataCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, ParseDataCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, SetkeyCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, SetGuidanceLightCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, PowerSaveControlCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, ParkCardCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, SynchronizeCommand>(IdcProfiles.DeviceClass);
        // EMVCLESS_CONFIGURE/PERFORM_TRANSACTION/ISSUERUPDATE have no handler: contactless is not supported (WFS_ERR_UNSUPP_COMMAND).
        services.AddKeyedSingleton<IExecuteCommand, ReadRawDataCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, EjectCardCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, RetainCardCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, ResetCountCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, ResetCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, ChipPowerCommand>(IdcProfiles.DeviceClass);
        services.AddKeyedSingleton<IExecuteCommand, ChipIoCommand>(IdcProfiles.DeviceClass);
        return services;
    }
}
