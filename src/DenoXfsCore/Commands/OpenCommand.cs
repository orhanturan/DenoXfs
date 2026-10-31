using System;
using DenoXfs.Core.Abstractions;
using DenoXfs.Api;
using DenoXfs.Api.Common;
using DenoXfs.Core.Models;
using DenoXfs.Core.Models.Responses;


namespace DenoXfs.Core.Commands;

public class OpenCommand : IOpenCommand
{
    private readonly OpenCommandInput input;
    private XfsVersion srvcVersion;
    private XfsVersion spiVersion;

    public OpenCommand(OpenCommandInput input)
    {
        this.input = input ?? throw new ArgumentNullException(nameof(input));
    }

    public CommandResponse Execute()
    {
        int result = XfsApi.AsyncOpen(
            input.OpenRequest.LogicalName,
            input.XfsApp.Handle,
            input.XfsApp.Name,
            input.OpenRequest.TraceLevel,
            input.OpenRequest.TimeOut,
            out ushort hService,
            input.WindowHandle,
            input.OpenRequest.SrvcVersionsRequired,
            ref srvcVersion,
            ref spiVersion,
            out uint requestId
        );

        // If AsyncOpen failed, complete task immediately with error
        if (result != 0) // WFS_SUCCESS = 0
        {
            var errorResponse = new OpenResponse
            {
                Result = result,
                HService = hService,
                RequestId = requestId,
                SrvcVersion = srvcVersion,
                SPIVersion = spiVersion
            };
            input.TaskCompletionSource.SetResult(errorResponse);
        }

        return new CommandResponse(result, requestId);
    }

    public void ProcessResult(XfsResult result)
    {
        // Note: WFS_OPEN_COMPLETE's result.Buffer is always null, so we use versions
        // which were updated by AsyncOpen via ref parameters
        var response = new OpenResponse
        {
            Result = result.Result,
            HService = result.Service,
            RequestId = result.RequestID,
            SrvcVersion = srvcVersion,
            SPIVersion = spiVersion
        };

        input.TaskCompletionSource.SetResult(response);
    }
}

