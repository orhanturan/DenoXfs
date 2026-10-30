using System;
using System.Threading.Tasks;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Builder interface for creating ExecuteCommand
/// </summary>
public interface IExecuteCommandBuilder : IXfsCommandBuilder
{
    IExecuteCommandBuilder WithExecuteRequest(ExecuteRequest request);
    IExecuteCommandBuilder WithXfsApp(IXfsApp xfsApp);
    IExecuteCommandBuilder WithWindowHandle(IntPtr handle);
    IExecuteCommandBuilder WithTaskCompletionSource(TaskCompletionSource<ExecuteResponse> tcs);
    new IExecuteCommand Build();
}

