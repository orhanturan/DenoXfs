using System.Runtime.InteropServices;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcSynchronizeCommand
{
    private uint dwCommand;
    private IntPtr lpCmdData;

    public uint Command
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
