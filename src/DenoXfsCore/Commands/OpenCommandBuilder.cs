using System;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class OpenCommandBuilder : IOpenCommandBuilder
{
    private readonly OpenCommandInput _input = new();

    public IOpenCommandBuilder WithOpenRequest(OpenRequest request)
    {
        _input.OpenRequest = request;
        return this;
    }

    public IOpenCommandBuilder WithXfsApp(IXfsApp xfsApp)
    {
        _input.XfsApp = xfsApp;
        return this;
    }

    public IOpenCommandBuilder WithWindowHandle(IntPtr handle)
    {
        _input.WindowHandle = handle;
        return this;
    }

    public IOpenCommandBuilder WithTaskCompletionSource(TaskCompletionSource<OpenResponse> tcs)
    {
        _input.TaskCompletionSource = tcs;
        return this;
    }

    public IOpenCommand Build()
    {
        if (_input.XfsApp == null)
            throw new InvalidOperationException("XfsApp is required");
        if (_input.OpenRequest == null)
            throw new InvalidOperationException("OpenRequest is required");

        return new OpenCommand(_input);
    }

    IXfsCommand IXfsCommandBuilder.Build() => Build();
}

