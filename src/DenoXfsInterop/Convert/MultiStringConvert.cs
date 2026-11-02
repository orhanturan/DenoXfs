using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

/// <summary>
/// XFS lists of null terminated ANSI strings that end with an extra null ("a\0b\0\0"): field and form lists,
/// lpszExtra "key=value" pairs.
/// </summary>
public static class MultiStringConvert
{
    private const int MaxLength = 64 * 1024;

    /// <summary>Reads the list; null for a NULL pointer, empty for "\0".</summary>
    public static IReadOnlyList<string>? ToManaged(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;

        var items = new List<string>();
        var offset = 0;
        while (offset < MaxLength && Marshal.ReadByte(ptr, offset) != 0)
        {
            items.Add(Marshal.PtrToStringAnsi(ptr + offset) ?? string.Empty);
            while (Marshal.ReadByte(ptr, offset) != 0)
                offset++;
            offset++; // the terminator of this entry
        }
        return items;
    }

    public static IntPtr ToIntPtr(IReadOnlyList<string>? value)
    {
        IntPtr currentPtr = IntPtr.Zero;
        return ToIntPtr(ref currentPtr, value);
    }

    /// <summary>Frees the current block and writes the list as "a\0b\0\0" (an empty list as "\0").</summary>
    public static IntPtr ToIntPtr(ref IntPtr currentPtr, IReadOnlyList<string>? value)
    {
        if (currentPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(currentPtr);
            currentPtr = IntPtr.Zero;
        }

        if (value == null)
            return currentPtr;

        // Every entry ends with '\0'; StringToHGlobalAnsi converts the embedded nulls and adds the final one.
        currentPtr = Marshal.StringToHGlobalAnsi(string.Concat(value.Select(item => item + "\0")));
        return currentPtr;
    }
}
