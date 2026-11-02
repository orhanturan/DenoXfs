using System.Runtime.InteropServices;

namespace DenoXfs.TestTool.Operations;

/// <summary>Command input in unmanaged memory that lives only for the call (WFSExecute copies lpCmdData before it returns).</summary>
public static class NativeInput
{
    /// <summary>Input that is a pointer to one WORD.</summary>
    public static Task<RawResponse> WithWordAsync(ushort value, Func<IntPtr, Task<RawResponse>> send) =>
        WithBlockAsync(sizeof(ushort), block => Marshal.WriteInt16(block, unchecked((short)value)), send);

    /// <summary>Input block of <paramref name="size"/> zeroed bytes, filled by <paramref name="write"/>.</summary>
    public static async Task<RawResponse> WithBlockAsync(int size, Action<IntPtr> write, Func<IntPtr, Task<RawResponse>> send)
    {
        var block = Marshal.AllocHGlobal(size);
        try
        {
            for (var i = 0; i < size; i++)
                Marshal.WriteByte(block, i, 0);
            write(block);
            return await send(block);
        }
        finally
        {
            Marshal.FreeHGlobal(block);
        }
    }
}
