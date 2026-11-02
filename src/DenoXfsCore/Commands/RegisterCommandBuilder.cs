using System;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class RegisterCommandBuilder : IRegisterCommandBuilder
{
    private readonly RegisterCommandInput _input = new();

    public IRegisterCommandBuilder WithRegisterRequest(RegisterRequest request)
    {
        _input.RegisterRequest = request;
        return this;
    }

    public IRegisterCommandBuilder WithXfsApp(IXfsApp xfsApp)
    {
        _input.XfsApp = xfsApp;
        return this;
    }

    public IRegisterCommandBuilder WithWindowHandle(IntPtr handle)
    {
        _input.WindowHandle = handle;
        return this;
    }

    public IRegisterCommandBuilder WithTaskCompletionSource(TaskCompletionSource<RegisterResponse> tcs)
    {
        _input.TaskCompletionSource = tcs;
        return this;
    }

    public IRegisterCommand Build()
    {
        if (_input.XfsApp == null)
            throw new InvalidOperationException("XfsApp is required");
        if (_input.RegisterRequest == null)
            throw new InvalidOperationException("RegisterRequest is required");

        return new RegisterCommand(_input);
    }

    IXfsCommand IXfsCommandBuilder.Build() => Build();
}
