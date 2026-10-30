using System;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Api;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class GetInfoCommand : IGetInfoCommand
{
    private readonly GetInfoCommandInput input;

    public GetInfoCommand(GetInfoCommandInput input)
    {
        this.input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public CommandResponse Execute()
    {
        int result = XfsApi.AsyncGetInfo(
            input.GetInfoRequest.HService,
            input.GetInfoRequest.Category,
            input.GetInfoRequest.QueryDetails,
            input.GetInfoRequest.TimeOut,
            input.WindowHandle,
            out uint requestId
        );

        // If AsyncGetInfo failed, complete task immediately with error
        if (result != 0)
        {
            var errorResponse = new GetInfoResponse
            {
                Result = result,
                RequestId = requestId
            };
            input.TaskCompletionSource.SetResult(errorResponse);
        }

        return new CommandResponse(result, requestId);
    }

    public void ProcessResult(XfsResult result)
    {
        var response = new GetInfoResponse
        {
            Result = result.Result,
            RequestId = result.RequestID,
            ResultPtr = result.Buffer
        };

        input.TaskCompletionSource.SetResult(response);
    }
}

