using System;
using DenoXfs.Core.Abstractions;
using DenoXfs.Api;
using DenoXfs.Api.Common;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class CloseCommand : ICloseCommand
{
    private readonly CloseCommandInput input;

    public CloseCommand(CloseCommandInput input)
    {
        this.input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public CommandResponse Execute()
    {
        int result = XfsApi.AsyncClose(input.CloseRequest.HService, input.WindowHandle, out uint requestId);

        // If AsyncClose failed, complete task immediately with error
        if (result != 0)
        {
            input.TaskCompletionSource.SetResult(new CloseResponse { Result = result, RequestId = requestId });
        }

        return new CommandResponse(result, requestId);
    }

    public bool ProcessResult(XfsResult result, IntPtr resultHandle)
    {
        input.TaskCompletionSource.SetResult(new CloseResponse { Result = result.Result, RequestId = result.RequestID });
        return false;
    }
}
