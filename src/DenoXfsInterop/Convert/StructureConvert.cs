using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

public static class StructureConvert
{
    public static IntPtr ToIntPtr<T>(T? value) where T : struct
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, value);
    }
    
    public static IntPtr ToIntPtr<T>(ref IntPtr currentPtr, T? value) where T : struct
    {
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }
        
        if (value != null)
        {
            int size = Marshal.SizeOf<T>();
            currentPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value.Value, currentPtr, false);
        }
        
        return currentPtr;
    }
}

