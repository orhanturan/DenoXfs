using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Extensions;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcAppData
{
    private IntPtr lpAID;
    private IntPtr lpKernelIdentifier;

    public XfsIdcHexData? AID
    {
        get => lpAID.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpAID, value);
    }

    public XfsIdcHexData? KernelIdentifier
    {
        get => lpKernelIdentifier.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpKernelIdentifier, value);
    }

}
