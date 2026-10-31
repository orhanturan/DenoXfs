using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Shared;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessUi
{
    private ushort wMessageId;
    private IdcStatusEmvclessUi wStatus;
    private uint ulHoldTime;
    private IdcValueQualifierEmvclessUi wValueQualifier;
    private IntPtr lpszValue;
    private IntPtr lpszCurrencyCode;
    private IntPtr lpszLanguagePreferenceData;

    public ushort MessageId
    {
        get => wMessageId;
        set => wMessageId = value;
    }

    public IdcStatusEmvclessUi Status
    {
        get => wStatus;
        set => wStatus = value;
    }

    public uint HoldTime
    {
        get => ulHoldTime;
        set => ulHoldTime = value;
    }

    public IdcValueQualifierEmvclessUi ValueQualifier
    {
        get => wValueQualifier;
        set => wValueQualifier = value;
    }

    public string? Value
    {
        get => IntPtrConvert.ToString(lpszValue);
        set => StringConvert.ToIntPtr(ref lpszValue, value);
    }

    public string? CurrencyCode
    {
        get => IntPtrConvert.ToString(lpszCurrencyCode);
        set => StringConvert.ToIntPtr(ref lpszCurrencyCode, value);
    }

    public string? LanguagePreferenceData
    {
        get => IntPtrConvert.ToString(lpszLanguagePreferenceData);
        set => StringConvert.ToIntPtr(ref lpszLanguagePreferenceData, value);
    }

}
