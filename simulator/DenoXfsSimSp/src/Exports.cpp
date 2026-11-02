// Exported SPI entry points (XFSSPI.H). They only translate arguments and delegate to the ServiceProvider.
#include "CompositionRoot.h"

BOOL APIENTRY DllMain(HMODULE, DWORD, LPVOID) { return TRUE; }

extern "C" {

HRESULT WINAPI WFPOpen(HSERVICE hService, LPCSTR lpszLogicalName, HAPP, LPCSTR lpszAppID, DWORD dwTraceLevel, DWORD,
                       HWND hWnd, REQUESTID ReqID, HPROVIDER, DWORD dwSPIVersionsRequired, LPWFSVERSION lpSPIVersion,
                       DWORD dwSrvcVersionsRequired, LPWFSVERSION lpSrvcVersion) {
    sim::OpenArguments arguments;
    arguments.logicalName = lpszLogicalName ? lpszLogicalName : "";
    arguments.appId = lpszAppID ? lpszAppID : "";
    arguments.traceLevel = dwTraceLevel;
    arguments.spiVersionsRequired = dwSPIVersionsRequired;
    arguments.srvcVersionsRequired = dwSrvcVersionsRequired;
    arguments.window = hWnd;
    arguments.requestId = ReqID;
    return sim::GetServiceProvider().Open(hService, arguments, lpSPIVersion, lpSrvcVersion);
}

HRESULT WINAPI WFPClose(HSERVICE hService, HWND hWnd, REQUESTID ReqID) {
    return sim::GetServiceProvider().Close(hService, hWnd, ReqID);
}

HRESULT WINAPI WFPExecute(HSERVICE hService, DWORD dwCommand, LPVOID lpCmdData, DWORD dwTimeOut, HWND hWnd, REQUESTID ReqID) {
    return sim::GetServiceProvider().Execute(hService, dwCommand, lpCmdData, dwTimeOut, hWnd, ReqID);
}

HRESULT WINAPI WFPGetInfo(HSERVICE hService, DWORD dwCategory, LPVOID lpQueryDetails, DWORD dwTimeOut, HWND hWnd, REQUESTID ReqID) {
    return sim::GetServiceProvider().GetInfo(hService, dwCategory, lpQueryDetails, dwTimeOut, hWnd, ReqID);
}

HRESULT WINAPI WFPRegister(HSERVICE hService, DWORD dwEventClass, HWND hWndReg, HWND hWnd, REQUESTID ReqID) {
    return sim::GetServiceProvider().Register(hService, dwEventClass, hWndReg, hWnd, ReqID);
}

HRESULT WINAPI WFPDeregister(HSERVICE hService, DWORD dwEventClass, HWND hWndReg, HWND hWnd, REQUESTID ReqID) {
    return sim::GetServiceProvider().Deregister(hService, dwEventClass, hWndReg, hWnd, ReqID);
}

HRESULT WINAPI WFPLock(HSERVICE hService, DWORD, HWND hWnd, REQUESTID ReqID) {
    return sim::GetServiceProvider().CompleteLocally(hService, WFS_LOCK_COMPLETE, hWnd, ReqID);
}

HRESULT WINAPI WFPUnlock(HSERVICE hService, HWND hWnd, REQUESTID ReqID) {
    return sim::GetServiceProvider().CompleteLocally(hService, WFS_UNLOCK_COMPLETE, hWnd, ReqID);
}

HRESULT WINAPI WFPCancelAsyncRequest(HSERVICE hService, REQUESTID ReqID) {
    return sim::GetServiceProvider().Cancel(hService, ReqID);
}

HRESULT WINAPI WFPSetTraceLevel(HSERVICE, DWORD) { return WFS_SUCCESS; }

HRESULT WINAPI WFPUnloadService() { return WFS_SUCCESS; }

} // extern "C"
