using System;
using System.Threading.Tasks;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Builder interface for creating OpenCommand
/// </summary>
public interface IOpenCommandBuilder : IXfsCommandBuilder
{
    IOpenCommandBuilder WithOpenRequest(OpenRequest request);
    IOpenCommandBuilder WithXfsApp(IXfsApp xfsApp);
    IOpenCommandBuilder WithWindowHandle(IntPtr handle);
    IOpenCommandBuilder WithTaskCompletionSource(TaskCompletionSource<OpenResponse> tcs);
    new IOpenCommand Build();
}

