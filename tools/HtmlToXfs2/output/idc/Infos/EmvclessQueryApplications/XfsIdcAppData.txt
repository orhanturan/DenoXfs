using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Infos;

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
