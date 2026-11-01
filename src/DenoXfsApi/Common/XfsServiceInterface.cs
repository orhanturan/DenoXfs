using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsServiceInterface
{
    private IntPtr lpdwExecuteCommands;
    private IntPtr lpdwGetInfoCategories;
    private IntPtr lpdwEvents;
    private uint dwMaximumRequests;
    private IntPtr lpdwAuthenticationRequired;

    public uint[]? ExecuteCommands
    {
        get => PrimitiveArray.ToManaged<uint>(lpdwExecuteCommands);
        set => PrimitiveArray.ToNative(ref lpdwExecuteCommands, value);
    }

    public uint[]? GetInfoCategories
    {
        get => PrimitiveArray.ToManaged<uint>(lpdwGetInfoCategories);
        set => PrimitiveArray.ToNative(ref lpdwGetInfoCategories, value);
    }

    public uint[]? Events
    {
        get => PrimitiveArray.ToManaged<uint>(lpdwEvents);
        set => PrimitiveArray.ToNative(ref lpdwEvents, value);
    }

    public uint MaximumRequests
    {
        get => dwMaximumRequests;
        set => dwMaximumRequests = value;
    }

    public uint[]? AuthenticationRequired
    {
        get => PrimitiveArray.ToManaged<uint>(lpdwAuthenticationRequired);
        set => PrimitiveArray.ToNative(ref lpdwAuthenticationRequired, value);
    }

}
