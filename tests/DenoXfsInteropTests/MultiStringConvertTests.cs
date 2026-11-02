using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfsInteropTests;

public class MultiStringConvertTests
{
    [Fact]
    public void List_round_trips_as_double_null_terminated_ansi()
    {
        var ptr = IntPtr.Zero;
        MultiStringConvert.ToIntPtr(ref ptr, ["PAN", "EXPIRY", "SERVICECODE"]);

        var bytes = new byte[24];
        Marshal.Copy(ptr, bytes, 0, bytes.Length);
        Assert.Equal("PAN\0EXPIRY\0SERVICECODE\0\0"u8.ToArray(), bytes);
        Assert.Equal(["PAN", "EXPIRY", "SERVICECODE"], MultiStringConvert.ToManaged(ptr)!);

        MultiStringConvert.ToIntPtr(ref ptr, null);
        Assert.Equal(IntPtr.Zero, ptr);
    }

    [Fact]
    public void Empty_list_is_a_single_null_and_a_null_pointer_is_null()
    {
        var ptr = MultiStringConvert.ToIntPtr([]);
        Assert.Equal(0, Marshal.ReadByte(ptr));
        Assert.Empty(MultiStringConvert.ToManaged(ptr)!);
        Marshal.FreeHGlobal(ptr);

        Assert.Null(MultiStringConvert.ToManaged(IntPtr.Zero));
    }
}
