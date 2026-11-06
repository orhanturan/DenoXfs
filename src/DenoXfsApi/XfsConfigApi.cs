using System;
using System.Runtime.InteropServices;
using System.Text;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace DenoXfs.Api;

/// <summary>
/// XFS configuration functions of xfs_conf.dll (XFS API chapter 8): the XFS part of the registry as the XFS manager
/// sees it, e.g. LOGICAL_SERVICES below <see cref="UserDefaultXfsRoot"/>.
/// </summary>
public static class XfsConfigApi
{
    private const string DllName = "xfs_conf.dll";

    /// <summary>WFS_CFG_HKEY_XFS_ROOT.</summary>
    public static readonly IntPtr XfsRoot = new(1);

    /// <summary>WFS_CFG_HKEY_MACHINE_XFS_ROOT: HKEY_LOCAL_MACHINE\SOFTWARE\XFS (SERVICE_PROVIDERS).</summary>
    public static readonly IntPtr MachineXfsRoot = new(2);

    /// <summary>WFS_CFG_HKEY_USER_DEFAULT_XFS_ROOT: HKEY_USERS\.DEFAULT\XFS (LOGICAL_SERVICES).</summary>
    public static readonly IntPtr UserDefaultXfsRoot = new(3);

    /// <summary>WFS_CFG_CURRENT_USER_XFS_ROOT.</summary>
    public static readonly IntPtr CurrentUserXfsRoot = new(4);

    [DllImport(DllName, EntryPoint = "WFMCloseKey", CallingConvention = CallingConvention.Winapi)]
    public static extern int CloseKey(IntPtr hKey);

    [DllImport(DllName, EntryPoint = "WFMOpenKey", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public static extern int OpenKey(IntPtr hKey, [MarshalAs(UnmanagedType.LPStr)] string? lpszSubKey, out IntPtr phkResult);

    [DllImport(DllName, EntryPoint = "WFMEnumKey", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public static extern int EnumKey(IntPtr hKey, uint iSubKey, StringBuilder lpszName, ref uint lpcchName, out FILETIME lpftLastWrite);

    [DllImport(DllName, EntryPoint = "WFMQueryValue", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public static extern int QueryValue(IntPtr hKey, [MarshalAs(UnmanagedType.LPStr)] string lpszValueName, StringBuilder lpszData, ref uint lpcchData);
}
