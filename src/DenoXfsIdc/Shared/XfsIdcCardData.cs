using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Shared;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcCardData
{
    private IdcDataSource wDataSource;
    private IdcDataStatus wStatus;
    private uint ulDataLength;
    private IntPtr lpbData;
    private IdcWriteMethod fwWriteMethod;

    public IdcDataSource DataSource
    {
        get => wDataSource;
        set => wDataSource = value;
    }

    public IdcDataStatus Status
    {
        get => wStatus;
        set => wStatus = value;
    }

    public uint DataLength
    {
        get => ulDataLength;
        set => ulDataLength = value;
    }

    public byte[]? Data
    {
        get => PrimitiveArray.ToManaged<byte>(lpbData, (int)ulDataLength);
        set => PrimitiveArray.ToNative(ref lpbData, value, out ulDataLength);
    }

    public IdcWriteMethod WriteMethod
    {
        get => fwWriteMethod;
        set => fwWriteMethod = value;
    }

}
