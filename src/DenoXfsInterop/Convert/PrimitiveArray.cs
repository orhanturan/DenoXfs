using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DenoXfs.Interop.Convert;

/// <summary>
/// Arrays of plain values (byte, ushort, uint, enums...) behind a pointer: LPBYTE, LPWORD, LPDWORD.
/// Either terminated by a zero element or sized by a separate count field.
/// For terminated arrays null means "no pointer" and an empty array means "terminator only".
/// </summary>
public static unsafe class PrimitiveArray
{
    /// <summary>Reads elements up to (not including) the first zero element.</summary>
    public static T[]? ToManaged<T>(IntPtr ptr) where T : unmanaged
    {
        if (ptr == IntPtr.Zero)
            return null;

        var count = 0;
        while (!IsZero(Unsafe.ReadUnaligned<T>((byte*)ptr + count * sizeof(T))))
            count++;

        return ToManaged<T>(ptr, count);
    }

    public static T[]? ToManaged<T>(IntPtr ptr, int count) where T : unmanaged
    {
        if (ptr == IntPtr.Zero || count <= 0)
            return ptr == IntPtr.Zero ? null : [];

        var result = new T[count];
        new ReadOnlySpan<byte>((void*)ptr, count * sizeof(T)).CopyTo(MemoryMarshal.AsBytes(result.AsSpan()));
        return result;
    }

    /// <summary>Writes the elements followed by a zero element. Frees <paramref name="current"/> first.</summary>
    public static IntPtr ToNative<T>(ref IntPtr current, T[]? value) where T : unmanaged
    {
        Free(ref current);
        if (value == null)
            return IntPtr.Zero;

        current = Allocate(value, terminator: true);
        return current;
    }

    /// <summary>Writes the elements without terminator and reports their number. Frees <paramref name="current"/> first.</summary>
    public static IntPtr ToNative<T, TCount>(ref IntPtr current, T[]? value, out TCount count)
        where T : unmanaged
        where TCount : INumberBase<TCount>
    {
        Free(ref current);
        count = TCount.CreateChecked(value?.Length ?? 0);
        if (value == null || value.Length == 0)
            return IntPtr.Zero;

        current = Allocate(value, terminator: false);
        return current;
    }

    public static void Free(ref IntPtr ptr)
    {
        if (ptr != IntPtr.Zero)
            Marshal.FreeHGlobal(ptr);
        ptr = IntPtr.Zero;
    }

    private static IntPtr Allocate<T>(T[] value, bool terminator) where T : unmanaged
    {
        var bytes = value.Length * sizeof(T);
        var ptr = Marshal.AllocHGlobal(bytes + (terminator ? sizeof(T) : 0));
        MemoryMarshal.AsBytes(value.AsSpan()).CopyTo(new Span<byte>((void*)ptr, bytes));
        if (terminator)
            new Span<byte>((byte*)ptr + bytes, sizeof(T)).Clear();
        return ptr;
    }

    private static bool IsZero<T>(T value) where T : unmanaged
        => MemoryMarshal.AsBytes(new ReadOnlySpan<T>(in value)).IndexOfAnyExcept((byte)0) < 0;
}
