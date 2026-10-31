using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsApiTransactionInfo
{
    private IntPtr lpszTransactionID;
    private IntPtr lpszExtra;

    public string? TransactionID
    {
        get => IntPtrConvert.ToWString(lpszTransactionID);
        set => WStringConvert.ToIntPtr(ref lpszTransactionID, value);
    }

    public string? Extra
    {
        get => IntPtrConvert.ToString(lpszExtra);
        set => StringConvert.ToIntPtr(ref lpszExtra, value);
    }

}
