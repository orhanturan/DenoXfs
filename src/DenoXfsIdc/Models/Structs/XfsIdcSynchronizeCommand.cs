using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcSynchronizeCommand
{
    private IdcCommand dwCommand;
    private IntPtr lpCmdData;

    public IdcCommand Command
    {
        get => dwCommand;
        set => dwCommand = value;
    }

    public IntPtr CmdData
    {
        get => lpCmdData;
        set => lpCmdData = value;
    }

}
