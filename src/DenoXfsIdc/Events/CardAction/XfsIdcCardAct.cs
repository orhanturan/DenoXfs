using System.Runtime.InteropServices;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Events;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcCardAct
{
    private IdcActionCardAct wAction;
    private IdcMedia wPosition;

    public IdcActionCardAct Action
    {
        get => wAction;
        set => wAction = value;
    }

    public IdcMedia Position
    {
        get => wPosition;
        set => wPosition = value;
    }

}
