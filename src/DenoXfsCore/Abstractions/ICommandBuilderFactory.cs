namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Factory interface for creating command builders
/// </summary>
public interface ICommandBuilderFactory
{
    TBuilder Create<TBuilder>() where TBuilder : class, IXfsCommandBuilder;
}

