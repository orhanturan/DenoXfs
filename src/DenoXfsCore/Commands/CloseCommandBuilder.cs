using System;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class CloseCommandBuilder : ICloseCommandBuilder
{
    private readonly CloseCommandInput _input = new();

    public ICloseCommandBuilder WithCloseRequest(CloseRequest request)
    {
        _input.CloseRequest = request;
        return this;
    }

    public ICloseCommandBuilder WithXfsApp(IXfsApp xfsApp)
    {
        _input.XfsApp = xfsApp;
        return this;
    }

    public ICloseCommandBuilder WithWindowHandle(IntPtr handle)
    {
        _input.WindowHandle = handle;
        return this;
    }

    public ICloseCommandBuilder WithTaskCompletionSource(TaskCompletionSource<CloseResponse> tcs)
    {
        _input.TaskCompletionSource = tcs;
        return this;
    }

    public ICloseCommand Build()
    {
        if (_input.XfsApp == null)
            throw new InvalidOperationException("XfsApp is required");
        if (_input.CloseRequest == null)
            throw new InvalidOperationException("CloseRequest is required");

        return new CloseCommand(_input);
    }

    IXfsCommand IXfsCommandBuilder.Build() => Build();
}
