using System;
using DenoXfs.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DenoXfs.Core.Services;

/// <summary>
/// Factory for creating command builders using DI container
/// </summary>
public class CommandBuilderFactory : ICommandBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CommandBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public TBuilder Create<TBuilder>() where TBuilder : class, IXfsCommandBuilder
    {
        return _serviceProvider.GetRequiredService<TBuilder>();
    }
}

