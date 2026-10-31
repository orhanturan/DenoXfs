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
    
    public static T?[]? ToZeroTerminatedArray<T>(IntPtr ptr) where T : struct
    {
        if (ptr == IntPtr.Zero)
            return null;
        
        var list = new System.Collections.Generic.List<T?>();
        IntPtr currentPtr = ptr;
        int elementSize = Marshal.SizeOf<IntPtr>();
        
        while (true)
        {
            IntPtr elementPtr = Marshal.ReadIntPtr(currentPtr);
            if (elementPtr == IntPtr.Zero)
                break;
            
            T? element = ToStructSafe<T>(elementPtr);
            list.Add(element);
            currentPtr += elementSize;
        }
        
        return list.ToArray();
    }
    
    public static string?[]? ToZeroTerminatedArray(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;
        
        var list = new System.Collections.Generic.List<string?>();
        IntPtr currentPtr = ptr;
        int elementSize = Marshal.SizeOf<IntPtr>();
        
        while (true)
        {
            IntPtr stringPtr = Marshal.ReadIntPtr(currentPtr);
            if (stringPtr == IntPtr.Zero)
                break;
            
            string? str = Marshal.PtrToStringAnsi(stringPtr);
            list.Add(str);
            currentPtr += elementSize;
        }
        
        return list.ToArray();
    }
    
    public static T?[]? ToDynamicArray<T>(IntPtr ptr, ushort count) where T : struct
    {
        if (ptr == IntPtr.Zero || count == 0)
            return null;
        
        var array = new T?[count];
        IntPtr currentPtr = ptr;
        int elementSize = Marshal.SizeOf<T>();
        
        for (int i = 0; i < count; i++)
        {
            T element = Marshal.PtrToStructure<T>(currentPtr);
            array[i] = element;
            currentPtr += elementSize;
        }
        
        return array;
    }
    
    public static string?[]? ToDynamicArray(IntPtr ptr, ushort count)
    {
        if (ptr == IntPtr.Zero || count == 0)
            return null;
        
        var array = new string?[count];
        IntPtr currentPtr = ptr;
        int elementSize = Marshal.SizeOf<IntPtr>();
        
        for (int i = 0; i < count; i++)
        {
            IntPtr stringPtr = Marshal.ReadIntPtr(currentPtr);
            if (stringPtr != IntPtr.Zero)
            {
                array[i] = Marshal.PtrToStringAnsi(stringPtr);
            }
            else
            {
                array[i] = null;
            }
            currentPtr += elementSize;
        }
        
        return array;
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

    public static byte[]? ToByteArray(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;

        var list = new System.Collections.Generic.List<byte>();
        int offset = 0;
        while (true)
        {
            byte b = Marshal.ReadByte(ptr, offset++);
            if (b == 0)
                break;
            list.Add(b);
        }

        return list.Count > 0 ? list.ToArray() : null;
    }

    public static byte[]? ToByteArray(IntPtr ptr, int count)
    {
        if (ptr == IntPtr.Zero || count <= 0)
            return null;

        var array = new byte[count];
        Marshal.Copy(ptr, array, 0, count);
        return array;
    }

    public static uint[]? ToUInt32Array(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;

        var list = new System.Collections.Generic.List<uint>();
        int offset = 0;
        while (true)
        {
            uint value = (uint)Marshal.ReadInt32(ptr, offset);
            if (value == 0)
                break;
            list.Add(value);
            offset += sizeof(uint);
        }

        return list.Count > 0 ? list.ToArray() : null;
    }
}

