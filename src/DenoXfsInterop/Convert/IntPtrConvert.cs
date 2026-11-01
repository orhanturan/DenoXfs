using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

public static class IntPtrConvert
{
    public static T ToStruct<T>(IntPtr ptr) where T : struct
    {
        if (ptr == IntPtr.Zero)
            throw new ArgumentNullException(nameof(ptr), "Pointer cannot be null");
        
        return Marshal.PtrToStructure<T>(ptr);
    }
    
    public static T? ToStructSafe<T>(IntPtr ptr) where T : struct
    {
        if (ptr == IntPtr.Zero)
            return null;
        
        return Marshal.PtrToStructure<T>(ptr);
    }

    public static string? ToWString(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;

        return Marshal.PtrToStringUni(ptr);
    }

    public static string? ToString(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;

        return Marshal.PtrToStringAnsi(ptr);
    }
}

