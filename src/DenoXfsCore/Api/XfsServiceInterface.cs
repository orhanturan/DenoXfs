using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

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
        get => IntPtrConvert.ToUInt32Array(lpdwExecuteCommands);
        set => UInt32ArrayConvert.ToIntPtr(ref lpdwExecuteCommands, value);
    }

    public uint[]? GetInfoCategories
    {
        get => IntPtrConvert.ToUInt32Array(lpdwGetInfoCategories);
        set => UInt32ArrayConvert.ToIntPtr(ref lpdwGetInfoCategories, value);
    }

    public uint[]? Events
    {
        get => IntPtrConvert.ToUInt32Array(lpdwEvents);
        set => UInt32ArrayConvert.ToIntPtr(ref lpdwEvents, value);
    }

    public uint MaximumRequests
    {
        get => dwMaximumRequests;
        set => dwMaximumRequests = value;
    }

    public uint[]? AuthenticationRequired
    {
        get => IntPtrConvert.ToUInt32Array(lpdwAuthenticationRequired);
        set => UInt32ArrayConvert.ToIntPtr(ref lpdwAuthenticationRequired, value);
    }

}
