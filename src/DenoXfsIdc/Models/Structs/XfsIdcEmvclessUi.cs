using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessUi
{
    private ushort wMessageId;
    private IdcStatus wStatus;
    private uint ulHoldTime;
    private IdcValueQualifier wValueQualifier;
    private IntPtr lpszValue;
    private IntPtr lpszCurrencyCode;
    private IntPtr lpszLanguagePreferenceData;

    public ushort MessageId
    {
        get => wMessageId;
        set => wMessageId = value;
    }

    public IdcStatus Status
    {
        get => wStatus;
        set => wStatus = value;
    }

    public uint HoldTime
    {
        get => ulHoldTime;
        set => ulHoldTime = value;
    }

    public IdcValueQualifier ValueQualifier
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
