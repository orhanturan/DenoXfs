using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

public static class ZeroTerminatedArrayConvert
{
    public static IntPtr ToIntPtr<T>(T?[]? array) where T : struct
    {
        return ToIntPtr(array, item => item.HasValue ? StructureConvert.ToIntPtr<T>(item.Value) : IntPtr.Zero);
    }
    
    public static IntPtr ToIntPtr(string?[]? array)
    {
        return ToIntPtr(array, item => item != null ? Marshal.StringToHGlobalAnsi(item) : IntPtr.Zero);
    }

    public static IntPtr ToIntPtr(ref IntPtr currentPtr, string?[]? array)
    {
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        currentPtr = ToIntPtr(array);
        return currentPtr;
    }

    public static IntPtr ToIntPtr<T>(ref IntPtr currentPtr, T?[]? array) where T : struct
    {
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        currentPtr = ToIntPtr(array);
        return currentPtr;
    }
    
    private static IntPtr ToIntPtr<T>(T[]? array, Func<T, IntPtr> elementConverter)
    {
        if (array == null || array.Length == 0)
            return IntPtr.Zero;
        
        int elementSize = Marshal.SizeOf<IntPtr>();
        int arraySize = (array.Length + 1) * elementSize;
        IntPtr ptr = Marshal.AllocHGlobal(arraySize);
        
        IntPtr currentPtr = ptr;
        for (int i = 0; i < array.Length; i++)
        {
            IntPtr elementPtr = elementConverter(array[i]);
            Marshal.WriteIntPtr(currentPtr, elementPtr);
            currentPtr += elementSize;
        }
        
        Marshal.WriteIntPtr(currentPtr, IntPtr.Zero);
        
        return ptr;
    }
}

