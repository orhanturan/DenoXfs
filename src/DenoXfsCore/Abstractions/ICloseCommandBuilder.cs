using System;
using System.Threading.Tasks;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Builder interface for creating CloseCommand
/// </summary>
public interface ICloseCommandBuilder : IXfsCommandBuilder
{
    ICloseCommandBuilder WithCloseRequest(CloseRequest request);
    ICloseCommandBuilder WithXfsApp(IXfsApp xfsApp);
    ICloseCommandBuilder WithWindowHandle(IntPtr handle);
    ICloseCommandBuilder WithTaskCompletionSource(TaskCompletionSource<CloseResponse> tcs);
    new ICloseCommand Build();
}
