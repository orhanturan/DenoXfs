using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcCardData
{
    private IdcDataSource wDataSource;
    private IdcStatus wStatus;
    private uint ulDataLength;
    private IntPtr lpbData;
    private IdcWriteMethod fwWriteMethod;

    public IdcDataSource DataSource
    {
        get => wDataSource;
        set => wDataSource = value;
    }

    public IdcStatus Status
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
        get => IntPtrConvert.ToByteArray(lpbData);
        set => ByteArrayConvert.ToIntPtr(ref lpbData, value);
    }

    public IdcWriteMethod WriteMethod
    {
        get => fwWriteMethod;
        set => fwWriteMethod = value;
    }

}
