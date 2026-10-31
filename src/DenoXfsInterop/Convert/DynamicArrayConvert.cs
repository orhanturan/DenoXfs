using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

public static class DynamicArrayConvert
{
    public static IntPtr ToIntPtr<T>(T?[]? array, out ushort count) where T : struct
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, array, out count);
    }
    
    public static IntPtr ToIntPtr<T>(ref IntPtr currentPtr, T?[]? array, out ushort count) where T : struct
    {
        count = 0;
        
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }
        
        if (array == null || array.Length == 0)
            return IntPtr.Zero;
        
        count = (ushort)array.Length;
        int elementSize = Marshal.SizeOf<T>();
        int arraySize = array.Length * elementSize;
        currentPtr = Marshal.AllocHGlobal(arraySize);
        
        IntPtr ptr = currentPtr;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].HasValue)
            {
                Marshal.StructureToPtr(array[i]!.Value, ptr, false);
            }
            else
            {
                Marshal.StructureToPtr(default(T), ptr, false);
            }
            ptr += elementSize;
        }
        
        return currentPtr;
    }
    
    public static IntPtr ToIntPtr(string?[]? array, out ushort count)
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, array, out count);
    }
    
    public static IntPtr ToIntPtr(byte[]? array, out ushort count)
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, array, out count);
    }

    public static IntPtr ToIntPtr(ref IntPtr currentPtr, byte[]? array, out ushort count)
    {
        count = 0;

        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        if (array != null && array.Length > 0)
        {
            count = (ushort)array.Length;
            currentPtr = Marshal.AllocHGlobal(array.Length);
            Marshal.Copy(array, 0, currentPtr, array.Length);
        }

        return currentPtr;
    }

    public static IntPtr ToIntPtr(byte[]? array, out uint count)
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, array, out count);
    }

    public static IntPtr ToIntPtr(ref IntPtr currentPtr, byte[]? array, out uint count)
    {
        count = 0;

        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        if (array != null && array.Length > 0)
        {
            count = (uint)array.Length;
            currentPtr = Marshal.AllocHGlobal(array.Length);
            Marshal.Copy(array, 0, currentPtr, array.Length);
        }

        return currentPtr;
    }

    public static IntPtr ToIntPtr(ref IntPtr currentPtr, string?[]? array, out ushort count)
    {
        count = 0;
        
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }
        
        if (array == null || array.Length == 0)
            return IntPtr.Zero;
        
        count = (ushort)array.Length;
        int elementSize = Marshal.SizeOf<IntPtr>();
        int arraySize = array.Length * elementSize;
        currentPtr = Marshal.AllocHGlobal(arraySize);
        
        IntPtr ptr = currentPtr;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
            {
                IntPtr stringPtr = Marshal.StringToHGlobalAnsi(array[i]);
                Marshal.WriteIntPtr(ptr, stringPtr);
            }
            else
            {
                Marshal.WriteIntPtr(ptr, IntPtr.Zero);
            }
            ptr += elementSize;
        }
        
        return currentPtr;
    }
}

