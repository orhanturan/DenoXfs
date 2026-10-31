using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

public static class ByteArrayConvert
{
    public static IntPtr ToIntPtr(byte[]? value)
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, value);
    }

    public static IntPtr ToIntPtr(ref IntPtr currentPtr, byte[]? value)
    {
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        if (value != null && value.Length > 0)
        {
            currentPtr = Marshal.AllocHGlobal(value.Length);
            Marshal.Copy(value, 0, currentPtr, value.Length);
        }

        return currentPtr;
    }
}
