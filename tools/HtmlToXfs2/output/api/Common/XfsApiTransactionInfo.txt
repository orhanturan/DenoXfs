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

    public IReadOnlyList<string>? Extra
    {
        get => MultiStringConvert.ToManaged(lpszExtra);
        set => MultiStringConvert.ToIntPtr(ref lpszExtra, value);
    }

}
