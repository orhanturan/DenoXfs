using System;
using System.Runtime.InteropServices;


namespace DenoXfs.Core.Api;

public static class XfsApi
{
    private const string DllName = "msxfs.dll";

    

    [DllImport(DllName, EntryPoint = "WFSCancelAsyncRequest", CallingConvention = CallingConvention.Winapi)]
    public static extern int CancelAsyncRequest(ushort hService, uint requestId);
    
    [DllImport(DllName, EntryPoint = "WFSCancelBlockingCall", CallingConvention = CallingConvention.Winapi)]
    public static extern int CancelBlockingCall(uint dwThreadId);
    
    [DllImport(DllName, EntryPoint = "WFSCleanUp", CallingConvention = CallingConvention.Winapi)]
    public static extern int CleanUp();
    
    [DllImport(DllName, EntryPoint = "WFSClose", CallingConvention = CallingConvention.Winapi)]
    public static extern int Close(ushort hService);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncClose", CallingConvention = CallingConvention.Winapi)]
    public static extern int AsyncClose(ushort hService, IntPtr hWnd, out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFSCreateAppHandle", CallingConvention = CallingConvention.Winapi)]
    public static extern int CreateAppHandle(out IntPtr lphApp);
    
    [DllImport(DllName, EntryPoint = "WFSDeregister", CallingConvention = CallingConvention.Winapi)]
    public static extern int Deregister(ushort hService, uint dwEventClass, IntPtr hWndReg);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncDeregister", CallingConvention = CallingConvention.Winapi)]
    public static extern int AsyncDeregister(ushort hService, uint dwEventClass, IntPtr hWndReg, IntPtr hWnd, out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFSDestroyAppHandle", CallingConvention = CallingConvention.Winapi)]
    public static extern int DestroyAppHandle(IntPtr hApp);
    
    [DllImport(DllName, EntryPoint = "WFSExecute", CallingConvention = CallingConvention.Winapi)]
    public static extern int Execute(ushort hService, uint dwCommand, IntPtr lpCmdData, uint dwTimeOut, out IntPtr lppResult);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncExecute", CallingConvention = CallingConvention.Winapi)]
    public static extern int AsyncExecute(ushort hService, uint dwCommand, IntPtr lpCmdData, uint dwTimeOut, IntPtr hWnd, out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFSFreeResult", CallingConvention = CallingConvention.Winapi)]
    public static extern int FreeResult(IntPtr lpResult);
    
    [DllImport(DllName, EntryPoint = "WFSGetInfo", CallingConvention = CallingConvention.Winapi)]
    public static extern int GetInfo(ushort hService, uint dwCategory, IntPtr lpQueryDetails, uint dwTimeOut, out IntPtr lppResult);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncGetInfo", CallingConvention = CallingConvention.Winapi)]
    public static extern int AsyncGetInfo(ushort hService, uint dwCategory, IntPtr lpQueryDetails, uint dwTimeOut, IntPtr hWnd, out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFSIsBlocking", CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsBlocking();
    
    [DllImport(DllName, EntryPoint = "WFSLock", CallingConvention = CallingConvention.Winapi)]
    public static extern int Lock(ushort hService, uint dwTimeOut, out IntPtr lppResult);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncLock", CallingConvention = CallingConvention.Winapi)]
    public static extern int AsyncLock(ushort hService, uint dwTimeOut, IntPtr hWnd, out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFSOpen", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public static extern int Open(
        [MarshalAs(UnmanagedType.LPStr)] string lpszLogicalName,
        IntPtr hApp,
        [MarshalAs(UnmanagedType.LPStr)] string lpszAppId,
        uint dwTraceLevel,
        uint dwTimeOut,
        uint dwSrvcVersionsRequired,
        ref XfsVersion lpSrvcVersion,
        ref XfsVersion lpSPIVersion,
        out ushort lphService);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncOpen", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public static extern int AsyncOpen(
        [MarshalAs(UnmanagedType.LPStr)] string lpszLogicalName,
        IntPtr hApp,
        [MarshalAs(UnmanagedType.LPStr)] string lpszAppId,
        uint dwTraceLevel,
        uint dwTimeOut,
        out ushort lphService,
        IntPtr hWnd,
        uint dwSrvcVersionsRequired,
        ref XfsVersion lpSrvcVersion,
        ref XfsVersion lpSPIVersion,
        out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFSRegister", CallingConvention = CallingConvention.Winapi)]
    public static extern int Register(ushort hService, uint dwEventClass, IntPtr hWndReg);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncRegister", CallingConvention = CallingConvention.Winapi)]
    public static extern int AsyncRegister(ushort hService, uint dwEventClass, IntPtr hWndReg, IntPtr hWnd, out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFSSetBlockingHook", CallingConvention = CallingConvention.Winapi)]
    public static extern int SetBlockingHook(IntPtr lpBlockFunc, out IntPtr lppPrevFunc);
    
    [DllImport(DllName, EntryPoint = "WFSStartUp", CallingConvention = CallingConvention.Winapi)]
    public static extern int StartUp(uint dwVersionsRequired, ref XfsVersion lpWfsVersion);
    
    [DllImport(DllName, EntryPoint = "WFSUnhookBlockingHook", CallingConvention = CallingConvention.Winapi)]
    public static extern int UnhookBlockingHook();
    
    [DllImport(DllName, EntryPoint = "WFSUnlock", CallingConvention = CallingConvention.Winapi)]
    public static extern int Unlock(ushort hService);
    
    [DllImport(DllName, EntryPoint = "WFSAsyncUnlock", CallingConvention = CallingConvention.Winapi)]
    public static extern int AsyncUnlock(ushort hService, IntPtr hWnd, out uint lpRequestId);
    
    [DllImport(DllName, EntryPoint = "WFMSetTraceLevel", CallingConvention = CallingConvention.Winapi)]
    public static extern int SetTraceLevel(ushort hService, uint dwTraceLevel);
}






