using System;
using DenoXfs.Core.Abstractions;
using DenoXfs.Api;
using DenoXfs.Api.Common;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Commands;

public class RegisterCommand : IRegisterCommand
{
    private readonly RegisterCommandInput input;

    public RegisterCommand(RegisterCommandInput input)
    {
        this.input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public CommandResponse Execute()
    {
        var request = input.RegisterRequest;
        // Events go to the message window unless the caller names another one.
        IntPtr eventWindow = request.HWndReg != IntPtr.Zero ? request.HWndReg : input.WindowHandle;

        int result = XfsApi.AsyncRegister(request.HService, request.EventClass, eventWindow, input.WindowHandle, out uint requestId);

        // If AsyncRegister failed, complete task immediately with error
        if (result != 0)
        {
            input.TaskCompletionSource.SetResult(new RegisterResponse { Result = result, RequestId = requestId });
        }

        return new CommandResponse(result, requestId);
    }

    public bool ProcessResult(XfsResult result, IntPtr resultHandle)
    {
        input.TaskCompletionSource.SetResult(new RegisterResponse { Result = result.Result, RequestId = result.RequestID });
        return false;
    }
}
