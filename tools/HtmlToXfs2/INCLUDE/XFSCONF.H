/******************************************************************************
*                                                                             *
* xfsconf.h     XFS - definitions for the Configuration functions             *
*                                                                             *
*               Version 3.40  (December 6 2019)                               *
*                                                                             *
******************************************************************************/

#ifndef __INC_XFSCONF__H
#define __INC_XFSCONF__H

#ifdef __cplusplus
extern "C" {
#endif

	/******* Common **************************************************************/

#include    <xfsapi.h>

/*   be aware of alignment   */
#pragma pack(push,1)

// following HKEY and PHKEY are already defined in WINREG.H
// so definition has been removed
// typedef HANDLE  HKEY;
// typedef HANDLE * PHKEY;

/******* Values of hKey *******************************************************/

#define     WFS_CFG_HKEY_XFS_ROOT               ((HKEY)1)
#define     WFS_CFG_HKEY_MACHINE_XFS_ROOT       ((HKEY)2)
#define     WFS_CFG_HKEY_USER_DEFAULT_XFS_ROOT  ((HKEY)3)
#define     WFS_CFG_CURRENT_USER_XFS_ROOT       ((HKEY)4)
// The following values are added for backwards compatibility reasons
#define     WFS_CFG_MACHINE_XFS_ROOT            WFS_CFG_HKEY_MACHINE_XFS_ROOT
#define     WFS_CFG_USER_DEFAULT_XFS_ROOT       WFS_CFG_HKEY_USER_DEFAULT_XFS_ROOT

/******* Values of lpdwDisposition *******************************************/

#define     WFS_CFG_CREATED_NEW_KEY             (0)
#define     WFS_CFG_OPENED_EXISTING_KEY         (1)

/******* Configuration Functions *********************************************/

	HRESULT extern  WINAPI  WFMCloseKey(HKEY hKey);

	HRESULT extern  WINAPI  WFMCreateKey(HKEY hKey, LPCSTR lpszSubKey, PHKEY phkResult, LPDWORD lpdwDisposition);

	HRESULT extern  WINAPI  WFMDeleteKey(HKEY hKey, LPCSTR lpszSubKey);

	HRESULT extern  WINAPI  WFMDeleteValue(HKEY hKey, LPCSTR lpszValue);

	HRESULT extern  WINAPI  WFMEnumKey(HKEY hKey, DWORD iSubKey, LPSTR lpszName, LPDWORD lpcchName, PFILETIME lpftLastWrite);

	HRESULT extern  WINAPI  WFMEnumValue(HKEY hKey, DWORD iValue, LPSTR lpszValue, LPDWORD lpcchValue, LPSTR lpszData, LPDWORD lpcchData);

	HRESULT extern  WINAPI  WFMOpenKey(HKEY hKey, LPCSTR lpszSubKey, PHKEY phkResult);

	HRESULT extern  WINAPI  WFMQueryValue(HKEY hKey, LPCSTR lpszValueName, LPSTR lpszData, LPDWORD lpcchData);

	HRESULT extern  WINAPI  WFMSetValue(HKEY hKey, LPCSTR lpszValueName, LPSTR lpszData, DWORD cchData);


	/*   restore alignment   */
#pragma pack(pop)

#ifdef __cplusplus
}       /*extern "C"*/
#endif

#endif  /* __INC_XFSCONF__H */

