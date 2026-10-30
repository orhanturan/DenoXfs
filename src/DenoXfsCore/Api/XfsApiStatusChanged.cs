using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsApiStatusChanged
{
    private IntPtr lpvOldStatus;
    private IntPtr lpvNewStatus;

    public IntPtr OldStatus
    {
        get => lpvOldStatus;
        set => lpvOldStatus = value;
    }

    public IntPtr NewStatus
    {
        get => lpvNewStatus;
        set => lpvNewStatus = value;
    }

}
