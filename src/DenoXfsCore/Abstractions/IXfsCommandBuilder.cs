namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Base interface for all XFS command builders
/// </summary>
public interface IXfsCommandBuilder
{
    IXfsCommand Build();
}

