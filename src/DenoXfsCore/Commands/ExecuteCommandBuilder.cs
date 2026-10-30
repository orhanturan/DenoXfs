using System;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class ExecuteCommandBuilder : IExecuteCommandBuilder
{
    private readonly ExecuteCommandInput _input = new();

    public IExecuteCommandBuilder WithExecuteRequest(ExecuteRequest request)
    {
        _input.ExecuteRequest = request;
        return this;
    }

    public IExecuteCommandBuilder WithXfsApp(IXfsApp xfsApp)
    {
        _input.XfsApp = xfsApp;
        return this;
    }

    public IExecuteCommandBuilder WithWindowHandle(IntPtr handle)
    {
        _input.WindowHandle = handle;
        return this;
    }

    public IExecuteCommandBuilder WithTaskCompletionSource(TaskCompletionSource<ExecuteResponse> tcs)
    {
        _input.TaskCompletionSource = tcs;
        return this;
    }

    public IExecuteCommand Build()
    {
        if (_input.XfsApp == null)
            throw new InvalidOperationException("XfsApp is required");
        if (_input.ExecuteRequest == null)
            throw new InvalidOperationException("ExecuteRequest is required");

        return new ExecuteCommand(_input);
    }

    IXfsCommand IXfsCommandBuilder.Build() => Build();
}

