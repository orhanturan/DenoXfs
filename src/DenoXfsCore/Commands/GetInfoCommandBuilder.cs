using System;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class GetInfoCommandBuilder : IGetInfoCommandBuilder
{
    private readonly GetInfoCommandInput _input = new();

    public IGetInfoCommandBuilder WithGetInfoRequest(GetInfoRequest request)
    {
        _input.GetInfoRequest = request;
        return this;
    }

    public IGetInfoCommandBuilder WithXfsApp(IXfsApp xfsApp)
    {
        _input.XfsApp = xfsApp;
        return this;
    }

    public IGetInfoCommandBuilder WithWindowHandle(IntPtr handle)
    {
        _input.WindowHandle = handle;
        return this;
    }

    public IGetInfoCommandBuilder WithTaskCompletionSource(TaskCompletionSource<GetInfoResponse> tcs)
    {
        _input.TaskCompletionSource = tcs;
        return this;
    }

    public IGetInfoCommand Build()
    {
        if (_input.XfsApp == null)
            throw new InvalidOperationException("XfsApp is required");
        if (_input.GetInfoRequest == null)
            throw new InvalidOperationException("GetInfoRequest is required");

        return new GetInfoCommand(_input);
    }

    IXfsCommand IXfsCommandBuilder.Build() => Build();
}

