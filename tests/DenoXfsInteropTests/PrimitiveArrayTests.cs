using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfsInteropTests;

public class PrimitiveArrayTests
{
    private enum Media : ushort { Present = 1, NotPresent = 2, Jammed = 3 }

    private enum Wide : uint { A = 1, B = 0x80000000 }

    [Fact]
    public void Terminated_ushort_round_trip()
    {
        var ptr = IntPtr.Zero;
        PrimitiveArray.ToNative(ref ptr, new ushort[] { 1, 2, 3 });
        Assert.Equal(0, Marshal.ReadInt16(ptr, 6)); // terminator
        Assert.Equal(new ushort[] { 1, 2, 3 }, PrimitiveArray.ToManaged<ushort>(ptr));
        PrimitiveArray.Free(ref ptr);
        Assert.Equal(IntPtr.Zero, ptr);
    }

    [Fact]
    public void Terminated_enums_use_the_underlying_width()
    {
        var narrow = IntPtr.Zero;
        var wide = IntPtr.Zero;
        PrimitiveArray.ToNative(ref narrow, new[] { Media.Present, Media.Jammed });
        PrimitiveArray.ToNative(ref wide, new[] { Wide.A, Wide.B });

        Assert.Equal(3, Marshal.ReadInt16(narrow, 2));
        Assert.Equal(new[] { Media.Present, Media.Jammed }, PrimitiveArray.ToManaged<Media>(narrow));
        Assert.Equal(new[] { Wide.A, Wide.B }, PrimitiveArray.ToManaged<Wide>(wide));

        PrimitiveArray.Free(ref narrow);
        PrimitiveArray.Free(ref wide);
    }

    [Fact]
    public void Terminated_distinguishes_null_from_empty()
    {
        var ptr = IntPtr.Zero;
        Assert.Equal(IntPtr.Zero, PrimitiveArray.ToNative(ref ptr, (uint[]?)null));
        Assert.Null(PrimitiveArray.ToManaged<uint>(ptr));

        PrimitiveArray.ToNative(ref ptr, Array.Empty<uint>());
        Assert.NotEqual(IntPtr.Zero, ptr);
        Assert.Empty(PrimitiveArray.ToManaged<uint>(ptr)!);
        PrimitiveArray.Free(ref ptr);
    }

    [Fact]
    public void Terminated_stops_at_first_zero_element()
    {
        var raw = Marshal.AllocHGlobal(8);
        Marshal.WriteInt32(raw, 0, 7);
        Marshal.WriteInt32(raw, 4, 0);
        Assert.Equal(new uint[] { 7 }, PrimitiveArray.ToManaged<uint>(raw));
        Marshal.FreeHGlobal(raw);
    }

    [Fact]
    public void Counted_bytes_keep_embedded_zeros_and_report_count_in_the_field_type()
    {
        byte[] data = [1, 2, 3, 0, 5, 0, 0, 7];
        var ptr = IntPtr.Zero;

        PrimitiveArray.ToNative(ref ptr, data, out uint wideCount);
        Assert.Equal(8u, wideCount);
        Assert.Equal(data, PrimitiveArray.ToManaged<byte>(ptr, (int)wideCount));

        PrimitiveArray.ToNative(ref ptr, data, out ushort narrowCount);
        Assert.Equal((ushort)8, narrowCount);
        PrimitiveArray.Free(ref ptr);
    }

    [Fact]
    public void Counted_null_and_empty_both_write_a_null_pointer()
    {
        var ptr = IntPtr.Zero;
        PrimitiveArray.ToNative(ref ptr, (byte[]?)null, out uint count);
        Assert.Equal((IntPtr.Zero, 0u), (ptr, count));

        PrimitiveArray.ToNative(ref ptr, Array.Empty<byte>(), out count);
        Assert.Equal((IntPtr.Zero, 0u), (ptr, count));
        Assert.Null(PrimitiveArray.ToManaged<byte>(ptr, 0));
    }

    [Fact]
    public void Setting_again_replaces_the_previous_block()
    {
        var ptr = IntPtr.Zero;
        PrimitiveArray.ToNative(ref ptr, new ushort[] { 1, 2, 3, 4, 5 });
        PrimitiveArray.ToNative(ref ptr, new ushort[] { 9 });
        Assert.Equal(new ushort[] { 9 }, PrimitiveArray.ToManaged<ushort>(ptr));
        PrimitiveArray.Free(ref ptr);
    }
}
