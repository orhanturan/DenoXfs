using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfsInteropTests;

public class PointerArrayTests
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct Item
    {
        public ushort Id;
        public uint Value;
    }

    [Fact]
    public void Terminated_strings_round_trip()
    {
        var ptr = IntPtr.Zero;
        PointerArray.ToNative(ref ptr, new string?[] { "one", "two", "three" });
        Assert.Equal(IntPtr.Zero, Marshal.ReadIntPtr(ptr, 3 * IntPtr.Size));
        Assert.Equal(new string?[] { "one", "two", "three" }, PointerArray.ToStrings(ptr));
        PointerArray.Free(ref ptr);
        Assert.Equal(IntPtr.Zero, ptr);
    }

    [Fact]
    public void Terminated_structs_round_trip()
    {
        var items = new Item?[] { new Item { Id = 1, Value = 10 }, new Item { Id = 2, Value = 20 } };
        var ptr = IntPtr.Zero;
        PointerArray.ToNative(ref ptr, items);
        Assert.Equal(items, PointerArray.ToStructs<Item>(ptr));
        PointerArray.Free(ref ptr);
    }

    [Fact]
    public void Terminated_distinguishes_null_from_empty()
    {
        var ptr = IntPtr.Zero;
        PointerArray.ToNative(ref ptr, (string?[]?)null);
        Assert.Null(PointerArray.ToStrings(ptr));

        PointerArray.ToNative(ref ptr, Array.Empty<string?>());
        Assert.NotEqual(IntPtr.Zero, ptr);
        Assert.Empty(PointerArray.ToStrings(ptr)!);
        PointerArray.Free(ref ptr);
    }

    [Fact]
    public void Counted_strings_keep_null_entries()
    {
        var ptr = IntPtr.Zero;
        ushort count = 0;
        PointerArray.ToNative(ref ptr, new string?[] { "a", null, "c" }, ref count);
        Assert.Equal((ushort)3, count);
        Assert.Equal(new string?[] { "a", null, "c" }, PointerArray.ToStrings(ptr, count));
        PointerArray.Free(ref ptr, count);
    }

    [Fact]
    public void Counted_setter_releases_the_previous_elements_and_updates_the_count()
    {
        var ptr = IntPtr.Zero;
        uint count = 0;
        PointerArray.ToNative(ref ptr, new Item?[] { new Item { Id = 1 }, new Item { Id = 2 } }, ref count);
        PointerArray.ToNative(ref ptr, new Item?[] { new Item { Id = 9 } }, ref count);
        Assert.Equal(1u, count);
        Assert.Equal(9, PointerArray.ToStructs<Item>(ptr, (int)count)![0]!.Value.Id);

        PointerArray.ToNative(ref ptr, (Item?[]?)null, ref count);
        Assert.Equal((IntPtr.Zero, 0u), (ptr, count));
    }
}
