namespace DenoXfs.Sim.Core.Protocol;

/// <summary>
/// Flat result block plus pointer fixups. Each fixup is [pointerFieldOffset, targetOffset]: the receiver
/// writes (base + targetOffset) into the pointer field at pointerFieldOffset. Offset 0 is the block root
/// (what <c>WFSRESULT.lpBuffer</c> points to).
/// </summary>
public sealed record ResultBuffer(int PtrSize, string Data, IReadOnlyList<int[]> Fixups);
