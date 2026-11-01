using System.Numerics;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

/// <summary>
/// Arrays of pointers to ANSI strings or structs: LPSTR*, LPWFSxxx*.
/// Either terminated by a null pointer or sized by a separate count field.
/// For terminated arrays null means "no pointer" and an empty array means "terminator only".
/// The element blocks are owned by the array and released by <see cref="Free(ref IntPtr)"/>.
/// </summary>
public static class PointerArray
{
    private static readonly int PointerSize = IntPtr.Size;

    // ---- native -> managed -------------------------------------------------------------------

    public static string?[]? ToStrings(IntPtr ptr) => Read<string>(ptr, Marshal.PtrToStringAnsi);

    public static string?[]? ToStrings(IntPtr ptr, int count) => Read<string>(ptr, count, Marshal.PtrToStringAnsi);

    public static T?[]? ToStructs<T>(IntPtr ptr) where T : struct => Read<T?>(ptr, p => Marshal.PtrToStructure<T>(p));

    public static T?[]? ToStructs<T>(IntPtr ptr, int count) where T : struct => Read<T?>(ptr, count, p => Marshal.PtrToStructure<T>(p));

    // ---- managed -> native -------------------------------------------------------------------

    public static IntPtr ToNative(ref IntPtr current, string?[]? value)
    {
        Free(ref current);
        return Write(ref current, value, Marshal.StringToHGlobalAnsi, terminator: true);
    }

    public static IntPtr ToNative<T>(ref IntPtr current, T?[]? value) where T : struct
    {
        Free(ref current);
        return Write<T?>(ref current, value, item => StructToPtr(item!.Value), terminator: true);
    }

    /// <summary>Counted variant: <paramref name="count"/> holds the old element count on entry and the new one on exit.</summary>
    public static IntPtr ToNative<TCount>(ref IntPtr current, string?[]? value, ref TCount count) where TCount : INumberBase<TCount>
    {
        Free(ref current, int.CreateChecked(count));
        count = TCount.CreateChecked(value?.Length ?? 0);
        return Write(ref current, value, Marshal.StringToHGlobalAnsi, terminator: false);
    }

    public static IntPtr ToNative<T, TCount>(ref IntPtr current, T?[]? value, ref TCount count)
        where T : struct
        where TCount : INumberBase<TCount>
    {
        Free(ref current, int.CreateChecked(count));
        count = TCount.CreateChecked(value?.Length ?? 0);
        return Write<T?>(ref current, value, item => StructToPtr(item!.Value), terminator: false);
    }

    /// <summary>Frees a null-terminated pointer array and every element block.</summary>
    public static void Free(ref IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return;

        for (var p = ptr; Marshal.ReadIntPtr(p) != IntPtr.Zero; p += PointerSize)
            Marshal.FreeHGlobal(Marshal.ReadIntPtr(p));

        Marshal.FreeHGlobal(ptr);
        ptr = IntPtr.Zero;
    }

    /// <summary>Frees a counted pointer array and every element block.</summary>
    public static void Free(ref IntPtr ptr, int count)
    {
        if (ptr == IntPtr.Zero)
            return;

        for (var i = 0; i < count; i++)
        {
            var element = Marshal.ReadIntPtr(ptr, i * PointerSize);
            if (element != IntPtr.Zero)
                Marshal.FreeHGlobal(element);
        }

        Marshal.FreeHGlobal(ptr);
        ptr = IntPtr.Zero;
    }

    // ---- helpers -----------------------------------------------------------------------------

    private static T?[]? Read<T>(IntPtr ptr, Func<IntPtr, T?> read)
    {
        if (ptr == IntPtr.Zero)
            return null;

        var count = 0;
        while (Marshal.ReadIntPtr(ptr, count * PointerSize) != IntPtr.Zero)
            count++;

        return Read(ptr, count, read);
    }

    private static T?[]? Read<T>(IntPtr ptr, int count, Func<IntPtr, T?> read)
    {
        if (ptr == IntPtr.Zero)
            return null;

        var result = new T?[Math.Max(count, 0)];
        for (var i = 0; i < result.Length; i++)
        {
            var element = Marshal.ReadIntPtr(ptr, i * PointerSize);
            result[i] = element == IntPtr.Zero ? default : read(element);
        }

        return result;
    }

    private static IntPtr Write<T>(ref IntPtr current, T?[]? value, Func<T, IntPtr> allocate, bool terminator)
    {
        if (value == null || (!terminator && value.Length == 0))
            return IntPtr.Zero;

        current = Marshal.AllocHGlobal((value.Length + (terminator ? 1 : 0)) * PointerSize);
        for (var i = 0; i < value.Length; i++)
            Marshal.WriteIntPtr(current, i * PointerSize, value[i] is { } item ? allocate(item) : IntPtr.Zero);

        if (terminator)
            Marshal.WriteIntPtr(current, value.Length * PointerSize, IntPtr.Zero);

        return current;
    }

    private static IntPtr StructToPtr<T>(T value) where T : struct
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        Marshal.StructureToPtr(value, ptr, false);
        return ptr;
    }
}
