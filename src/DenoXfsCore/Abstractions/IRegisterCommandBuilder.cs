using System;
using System.Threading.Tasks;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Builder interface for creating RegisterCommand
/// </summary>
public interface IRegisterCommandBuilder : IXfsCommandBuilder
{
    IRegisterCommandBuilder WithRegisterRequest(RegisterRequest request);
    IRegisterCommandBuilder WithXfsApp(IXfsApp xfsApp);
    IRegisterCommandBuilder WithWindowHandle(IntPtr handle);
    IRegisterCommandBuilder WithTaskCompletionSource(TaskCompletionSource<RegisterResponse> tcs);
    new IRegisterCommand Build();
}
