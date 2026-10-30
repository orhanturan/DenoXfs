using System;
using System.Threading.Tasks;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Builder interface for creating GetInfoCommand
/// </summary>
public interface IGetInfoCommandBuilder : IXfsCommandBuilder
{
    IGetInfoCommandBuilder WithGetInfoRequest(GetInfoRequest request);
    IGetInfoCommandBuilder WithXfsApp(IXfsApp xfsApp);
    IGetInfoCommandBuilder WithWindowHandle(IntPtr handle);
    IGetInfoCommandBuilder WithTaskCompletionSource(TaskCompletionSource<GetInfoResponse> tcs);
    new IGetInfoCommand Build();
}

