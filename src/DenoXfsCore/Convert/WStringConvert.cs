using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Core.Convert;

public static class WStringConvert
{
    public static IntPtr ToIntPtr(string? value)
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, value);
    }

    public static IntPtr ToIntPtr(ref IntPtr currentPtr, string? value)
    {
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        if (value != null)
            currentPtr = Marshal.StringToHGlobalUni(value);

        return currentPtr;
    }
}
