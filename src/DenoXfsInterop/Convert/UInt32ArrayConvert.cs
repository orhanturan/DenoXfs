using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

public static class UInt32ArrayConvert
{
    public static IntPtr ToIntPtr(uint[]? value)
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, value);
    }

    public static IntPtr ToIntPtr(ref IntPtr currentPtr, uint[]? value)
    {
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        if (value != null && value.Length > 0)
        {
            // Null-terminated array (XFS convention: array is terminated by 0)
            int byteSize = (value.Length + 1) * sizeof(uint);
            currentPtr = Marshal.AllocHGlobal(byteSize);

            for (int i = 0; i < value.Length; i++)
                Marshal.WriteInt32(currentPtr, i * sizeof(uint), (int)value[i]);

            Marshal.WriteInt32(currentPtr, value.Length * sizeof(uint), 0);
        }

        return currentPtr;
    }
}
