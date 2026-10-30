---
source: XfsApi.pdf
document: XfsApi
device_class: API/SPI
part: 1
section_id: "11"
title: "Error Codes"
type: section
source_lines: [4443, 4583]
---

<!-- 11 Error Codes -->

11 Error Codes
The following are the error codes that can be returned from a call to an XFS API or SPI function, either as a
function return or in a result structure pointed to by a completion message. Errors from service-specific commands
are defined in the specifications for each service class.
WFS_ERR_ALREADY_STARTED
A WFSStartUp has already been issued by the application, without an intervening WFSCleanUp.
WFS_ERR_API_VER_TOO_HIGH
The range of versions of XFS API support requested by the application is higher than any supported by this
particular XFS Manager implementation.
WFS_ERR_API_VER_TOO_LOW
The range of versions of XFS API support requested by the application is lower than any supported by this
particular XFS Manager implementation.
WFS_ERR_CANCELED
The request was canceled by WFSCancelAsyncRequest or WFSCancelBlockingCall.
WFS_ERR_CFG_INVALID_HKEY
The specified hKey parameter does not correspond to a currently open key.
WFS_ERR_CFG_INVALID_NAME
The value specified by the lpszValueName parameter does not exist in the specified key.
WFS_ERR_CFG_INVALID_SUBKEY
The key specified by lpszSubKey does not exist.
WFS_ERR_CFG_INVALID_VALUE
The specified value does not exist within the specified open key.
WFS_ERR_CFG_KEY_NOT_EMPTY
The specified key has subkeys and cannot be deleted. The subkeys must be deleted first.
WFS_ERR_CFG_NAME_TOO_LONG
The length of the name to be returned exceeds the length of the buffer.
WFS_ERR_CFG_NO_MORE_ITEMS
There are no more subkeys to be returned (the iSubKey parameter is greater than the index of the last subkey).
WFS_ERR_CFG_VALUE_TOO_LONG
The length of the value to be returned exceeds the length of the buffer.
WFS_ERR_CONNECTION_LOST
The connection to the service is lost.
WFS_ERR_DEV_NOT_READY
The function required device access, and the device was not ready or timed out.
WFS_ERR_HARDWARE_ERROR
The function required device access, and an error occurred on the device.
WFS_ERR_INTERNAL_ERROR
An internal inconsistency or other unexpected error occurred in the XFS subsystem.
WFS_ERR_INVALID_ADDRESS
The lpvOriginal parameter does not point to a previously allocated buffer.
WFS_ERR_INVALID_APP_HANDLE
The specified application handle is not valid, i.e. was not created by a preceding create call.
WFS_ERR_INVALID_BUFFER
The lpvData parameter is not a pointer to an allocated buffer structure.
WFS_ERR_INVALID_CATEGORY
The dwCategory issued is not supported by this service class.
133

CWA 16926-1:2020 (E)
WFS_ERR_INVALID_COMMAND
The dwCommand issued is not supported by this service class.
WFS_ERR_INVALID_EVENT_CLASS
The dwEventClass parameter specifies one or more event classes not supported by the service.
WFS_ERR_INVALID_HSERVICE
The hService parameter is not a valid service handle.
WFS_ERR_INVALID_HPROVIDER
The hProvider parameter is not a valid provider handle.
WFS_ERR_INVALID_HWND
The hWnd parameter is not a valid window handle.
WFS_ERR_INVALID_HWNDREG
The hWndReg parameter is not a valid window handle.
WFS_ERR_INVALID_POINTER
A pointer parameter does not point to accessible memory.
WFS_ERR_INVALID_DATA
The data structure passed as input parameter contains invalid data.
WFS_ERR_INVALID_REQ_ID
The RequestID parameter does not correspond to an outstanding request on the service.
WFS_ERR_INVALID_RESULT
The lpResult parameter is not a pointer to an allocated WFSRESULT structure.
WFS_ERR_INVALID_SERVPROV
The file containing the Service Provider is invalid or corrupted.
WFS_ERR_INVALID_TIMER
The hWnd and wTimerID parameters do not correspond to a currently active timer.
WFS_ERR_INVALID_TRACELEVEL
The dwTraceLevel parameter does not correspond to a valid trace level or set of levels.
WFS_ERR_LOCKED
The service is locked under a different hService.
WFS_ERR_NO_BLOCKING_CALL
There is no outstanding blocking call for the specified thread.
WFS_ERR_NO_SERVPROV
The file containing the Service Provider does not exist.
WFS_ERR_NO_SUCH_THREAD
The specified thread does not exist.
WFS_ERR_NO_TIMER
The timer could not be created.
WFS_ERR_NOT_LOCKED
The application requesting a service be unlocked had not previously performed a successful WFSLock or
WFSAsyncLock.
WFS_ERR_NOT_OK_TO_UNLOAD
The XFS Manager may not unload the Service Provider DLL.
WFS_ERR_NOT_STARTED
The application has not previously performed a successful WFSStartUp.
WFS_ERR_NOT_REGISTERED
The specified hWndReg window was not registered to receive messages for any event classes.
WFS_ERR_OP_IN_PROGRESS
A blocking operation is in progress on the thread; only WFSCancelBlockingCall and WFSIsBlocking are
permitted at this time.
WFS_ERR_OUT_OF_MEMORY
There is not enough memory available to satisfy the request.
134

CWA 16926-1:2020 (E)
WFS_ERR_SERVICE_NOT_FOUND
The logical name is not a valid Service Provider name.
WFS_ERR_SOFTWARE_ERROR
The function required access to configuration information, and an error occurred on the software.
WFS_ERR_SPI_VER_TOO_HIGH
The range of versions of XFS SPI support requested by the XFS Manager is higher than any supported by the
Service Provider for the logical service being opened.
WFS_ERR_SPI_VER_TOO_LOW
The range of versions of XFS SPI support requested by the XFS Manager is lower than any supported by the
Service Provider for the logical service being opened.
WFS_ERR_SRVC_VER_TOO_HIGH
The range of versions of the service-specific interface support requested by the application is higher than any
supported by the Service Provider for the logical service being opened.
WFS_ERR_SRVC_VER_TOO_LOW
The range of versions of the service-specific interface support requested by the application is lower than any
supported by the Service Provider for the logical service being opened.
WFS_ERR_TIMEOUT
The timeout interval expired.
WFS_ERR_UNSUPP_CATEGORY
The dwCategory issued, although valid for this service class, is not supported by this Service Provider.
WFS_ERR_UNSUPP_COMMAND
The dwCommand issued, although valid for this service class, is not supported by this Service Provider or device.
WFS_ERR_UNSUPP_DATA
The data structure passed as an input parameter although valid for this service class is not supported by this Service
Provider or device.
WFS_ERR_USER_ERROR
A user is preventing proper operation of the device.
WFS_ERR_VERSION_ERROR_IN_SRVC
Within the service, a version mismatch of two modules occurred.
WFS_ERR_FRAUD_ATTEMPT
Some devices are capable of identifying a malicious physical attack which attempts to defraud valuable information
or media. In these cases, this error code is returned to indicate the user is attempting a fraudulent act on the device.
WFS_ERR_SEQUENCE_ERROR
The requested operation is not valid at this time or in the devices current state.
WFS_ERR_AUTH_REQUIRED
The requested operation cannot be performed because it requires authentication.
135

CWA 16926-1:2020 (E)
