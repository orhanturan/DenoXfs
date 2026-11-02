using System;
using System.Runtime.InteropServices;
using DenoXfs.Api;
using DenoXfs.Api.Common;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Services;

public class XfsSyncService : IXfsSyncService
{
    private readonly IXfsApp xfsApp;

    public XfsSyncService(IXfsApp xfsApp)
    {
        this.xfsApp = xfsApp ?? throw new ArgumentNullException(nameof(xfsApp));
    }

    public StartUpResponse StartUp(StartUpRequest request)
    {
        var version = NewVersion();
        int result = XfsApi.StartUp(request.VersionsRequired, ref version);
        return new StartUpResponse { Result = result, XfsVersion = version };
    }

    public OpenResponse Open(OpenRequest request)
    {
        var srvcVersion = NewVersion();
        var spiVersion = NewVersion();
        int result = XfsApi.Open(
            request.LogicalName,
            xfsApp.Handle,
            xfsApp.Name,
            request.TraceLevel,
            request.TimeOut,
            request.SrvcVersionsRequired,
            ref srvcVersion,
            ref spiVersion,
            out ushort hService);

        return new OpenResponse { Result = result, HService = hService, SrvcVersion = srvcVersion, SPIVersion = spiVersion };
    }

    public CloseResponse Close(CloseRequest request)
    {
        return new CloseResponse { Result = XfsApi.Close(request.HService) };
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        return new RegisterResponse { Result = XfsApi.Register(request.HService, request.EventClass, request.HWndReg) };
    }

    public ExecuteResponse Execute(ExecuteRequest request)
    {
        request.BlockingThreadStarted?.Invoke(GetCurrentThreadId());
        int result = XfsApi.Execute(request.HService, request.Command, request.CmdData, request.TimeOut, out IntPtr handle);
        return new ExecuteResponse { Result = ResultCode(result, handle), ResultHandle = handle, ResultPtr = BufferOf(handle) };
    }

    public GetInfoResponse GetInfo(GetInfoRequest request)
    {
        request.BlockingThreadStarted?.Invoke(GetCurrentThreadId());
        int result = XfsApi.GetInfo(request.HService, request.Category, request.QueryDetails, request.TimeOut, out IntPtr handle);
        return new GetInfoResponse { Result = ResultCode(result, handle), ResultHandle = handle, ResultPtr = BufferOf(handle) };
    }

    public int CancelBlockingCall(uint threadId) => XfsApi.CancelBlockingCall(threadId);

    public int SetTraceLevel(SetTraceLevelRequest request)
    {
        return XfsApi.SetTraceLevel(request.HService, request.TraceLevel);
    }

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();

    // WFSExecute/WFSGetInfo return the command's own result in WFSRESULT.hResult when a result block exists.
    private static int ResultCode(int apiResult, IntPtr handle)
    {
        if (apiResult != 0 || handle == IntPtr.Zero)
            return apiResult;
        return Marshal.PtrToStructure<XfsResult>(handle).Result;
    }

    private static IntPtr BufferOf(IntPtr handle)
    {
        return handle == IntPtr.Zero ? IntPtr.Zero : Marshal.PtrToStructure<XfsResult>(handle).Buffer;
    }

    private static XfsVersion NewVersion()
    {
        return new XfsVersion { Description = new byte[Xfs.WfsddescriptionLen + 1], SystemStatus = new byte[Xfs.WfsdsysstatusLen + 1] };
    }
}
