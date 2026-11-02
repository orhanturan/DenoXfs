using System;
using DenoXfs.Core.Abstractions;
using DenoXfs.Api;
using DenoXfs.Api.Common;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class ExecuteCommand : IExecuteCommand
{
    private readonly ExecuteCommandInput input;

    public ExecuteCommand(ExecuteCommandInput input)
    {
        this.input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public CommandResponse Execute()
    {
        int result = XfsApi.AsyncExecute(
            input.ExecuteRequest.HService,
            input.ExecuteRequest.Command,
            input.ExecuteRequest.CmdData,
            input.ExecuteRequest.TimeOut,
            input.WindowHandle,
            out uint requestId
        );

        // If AsyncExecute failed, complete task immediately with error
        if (result != 0)
        {
            var errorResponse = new ExecuteResponse
            {
                Result = result,
                RequestId = requestId
            };
            input.TaskCompletionSource.SetResult(errorResponse);
        }

        else
        {
            input.ExecuteRequest.RequestIdAssigned?.Invoke(requestId);
        }

        return new CommandResponse(result, requestId);
    }

    public bool ProcessResult(XfsResult result, IntPtr resultHandle)
    {
        var response = new ExecuteResponse
        {
            Result = result.Result,
            RequestId = result.RequestID,
            ResultPtr = result.Buffer,
            ResultHandle = resultHandle
        };

        input.TaskCompletionSource.SetResult(response);
        return true;
    }
}

