using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Data;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Commands;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Infos;
using DenoXfs.Sim.Idc.Seeds;
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
        services.AddSingleton<IIdcDeviceRegistry, IdcDeviceRegistry>();
        services.AddSingleton<IIdcDeviceService, IdcDeviceService>();
        services.AddSingleton<ITrackDataFormatter, TrackDataFormatter>();
        services.AddSingleton<FormReadResult>();

        // Info categories and commands: one line each.
        services.AddSingleton<IInfoCommand, StatusInfo>();
        services.AddSingleton<IInfoCommand, CapabilitiesInfo>();
        services.AddSingleton<IInfoCommand, FormListInfo>();
        services.AddSingleton<IInfoCommand, QueryFormInfo>();
        services.AddSingleton<IInfoCommand, QueryIfmIdentifierInfo>();
        services.AddSingleton<IInfoCommand, EmvclessQueryApplicationsInfo>();
        services.AddSingleton<IExecuteCommand, ReadTrackCommand>();
        services.AddSingleton<IExecuteCommand, WriteTrackCommand>();
        services.AddSingleton<IExecuteCommand, WriteRawDataCommand>();
        services.AddSingleton<IExecuteCommand, ParseDataCommand>();
        services.AddSingleton<IExecuteCommand, SetkeyCommand>();
        services.AddSingleton<IExecuteCommand, SetGuidanceLightCommand>();
        services.AddSingleton<IExecuteCommand, PowerSaveControlCommand>();
        services.AddSingleton<IExecuteCommand, ParkCardCommand>();
        services.AddSingleton<IExecuteCommand, SynchronizeCommand>();
        // EMVCLESS_CONFIGURE/PERFORM_TRANSACTION/ISSUERUPDATE have no handler: contactless is not supported (WFS_ERR_UNSUPP_COMMAND).
        services.AddSingleton<IExecuteCommand, ReadRawDataCommand>();
        services.AddSingleton<IExecuteCommand, EjectCardCommand>();
        services.AddSingleton<IExecuteCommand, RetainCardCommand>();
        services.AddSingleton<IExecuteCommand, ResetCountCommand>();
        services.AddSingleton<IExecuteCommand, ResetCommand>();
        services.AddSingleton<IExecuteCommand, ChipPowerCommand>();
        services.AddSingleton<IExecuteCommand, ChipIoCommand>();
        return services;
    }
}
