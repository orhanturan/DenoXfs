using System.Runtime.InteropServices;

namespace DenoXfs.TestTool.Operations;

/// <summary>
/// Unmanaged memory for one command input (structures, strings, arrays); everything is freed when the scope ends,
/// which is safe because WFSExecute copies lpCmdData before it returns.
/// </summary>
public sealed class NativeScope : IDisposable
{
    private readonly List<IntPtr> _blocks = new();

    /// <summary>Zeroed block of <paramref name="size"/> bytes.</summary>
    public IntPtr Block(int size)
    {
        var block = Marshal.AllocHGlobal(Math.Max(size, 1));
        for (var i = 0; i < size; i++)
            Marshal.WriteByte(block, i, 0);
        _blocks.Add(block);
        return block;
    }

    public IntPtr Ansi(string text)
    {
        var block = Marshal.StringToHGlobalAnsi(text);
        _blocks.Add(block);
        return block;
    }

    public IntPtr Bytes(byte[] bytes)
    {
        var block = Block(bytes.Length);
        Marshal.Copy(bytes, 0, block, bytes.Length);
        return block;
    }

    /// <summary>NULL terminated array of LPWFSIDCCARDDATA (WFSIDCCARDDATA: pack 1, 14 bytes).</summary>
    public IntPtr CardDataArray(IReadOnlyList<(ushort Source, byte[] Data, ushort WriteMethod)> items)
    {
        var array = Block((items.Count + 1) * IntPtr.Size);
        for (var i = 0; i < items.Count; i++)
        {
            var (source, data, writeMethod) = items[i];
            var item = Block(14);
            Marshal.WriteInt16(item, 0, unchecked((short)source));
            Marshal.WriteInt32(item, 4, data.Length);
            Marshal.WriteIntPtr(item, 8, Bytes(data));
            Marshal.WriteInt16(item, 12, unchecked((short)writeMethod));
            Marshal.WriteIntPtr(array, i * IntPtr.Size, item);
        }
        return array;
    }

    public void Dispose()
    {
        foreach (var block in _blocks)
            Marshal.FreeHGlobal(block);
        _blocks.Clear();
    }
}
