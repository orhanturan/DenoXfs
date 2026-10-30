---
source: XfsBio.pdf
document: XfsBio
device_class: BIO
part: 18
section_id: ""
title: "8. C - Header file"
type: header
source_lines: [1792, 2145]
---

<!-- 8. C - Header file -->

8. C - Header file
/******************************************************************************
* *
* xfsbio.h XFS - Biometrics (BIO) definitions *
* *
* Version 3.40 (December 6 2019) *
* *
******************************************************************************/
#ifndef __INC_XFSBIO__H
#define __INC_XFSBIO__H
#ifdef __cplusplus
extern "C" {
#endif
#include <xfsapi.h>
/* be aware of alignment */
#pragma pack (push, 1)
/* values of WFSBIOCAPS.wClass */
#define WFS_SERVICE_CLASS_BIO (17)
#define WFS_SERVICE_CLASS_NAME_BIO "BIO"
#define WFS_SERVICE_CLASS_VERSION_BIO (0x2803) /* Version 3.40 */
#define BIO_SERVICE_OFFSET (WFS_SERVICE_CLASS_BIO * 100)
/* BIO Info Commands */
#define WFS_INF_BIO_STATUS (BIO_SERVICE_OFFSET + 1)
#define WFS_INF_BIO_CAPABILITIES (BIO_SERVICE_OFFSET + 2)
#define WFS_INF_BIO_STORAGE_INFO (BIO_SERVICE_OFFSET + 3)
#define WFS_INF_BIO_KEY_INFO (BIO_SERVICE_OFFSET + 4)
/* BIO Execute Commands */
#define WFS_CMD_BIO_READ (BIO_SERVICE_OFFSET + 1)
#define WFS_CMD_BIO_IMPORT (BIO_SERVICE_OFFSET + 2)
#define WFS_CMD_BIO_MATCH (BIO_SERVICE_OFFSET + 3)
#define WFS_CMD_BIO_SET_MATCH (BIO_SERVICE_OFFSET + 4)
#define WFS_CMD_BIO_CLEAR (BIO_SERVICE_OFFSET + 5)
#define WFS_CMD_BIO_RESET (BIO_SERVICE_OFFSET + 6)
#define WFS_CMD_BIO_SET_DATA_PERSISTENCE (BIO_SERVICE_OFFSET + 7)
#define WFS_CMD_BIO_SET_GUIDANCE_LIGHT (BIO_SERVICE_OFFSET + 8)
#define WFS_CMD_BIO_POWER_SAVE_CONTROL (BIO_SERVICE_OFFSET + 9)
#define WFS_CMD_BIO_SYNCHRONIZE_COMMAND (BIO_SERVICE_OFFSET + 10)
/* BIO Events */
#define WFS_EXEE_BIO_PRESENTSUBJECT (BIO_SERVICE_OFFSET + 1)
#define WFS_EXEE_BIO_SUBJECTDETECTED (BIO_SERVICE_OFFSET + 3)
#define WFS_EXEE_BIO_REMOVESUBJECT (BIO_SERVICE_OFFSET + 4)
#define WFS_SRVE_BIO_SUBJECTREMOVED (BIO_SERVICE_OFFSET + 5)
#define WFS_SRVE_BIO_DATACLEARED (BIO_SERVICE_OFFSET + 6)
#define WFS_USRE_BIO_ORIENTATION (BIO_SERVICE_OFFSET + 7)
#define WFS_SRVE_BIO_DEVICEPOSITION (BIO_SERVICE_OFFSET + 8)
#define WFS_SRVE_BIO_POWER_SAVE_CHANGE (BIO_SERVICE_OFFSET + 9)
/* values of WFSBIOSTATUS.fwDevice */
#define WFS_BIO_DEVONLINE WFS_STAT_DEVONLINE
#define WFS_BIO_DEVOFFLINE WFS_STAT_DEVOFFLINE
#define WFS_BIO_DEVPOWEROFF WFS_STAT_DEVPOWEROFF
#define WFS_BIO_DEVNODEVICE WFS_STAT_DEVNODEVICE
49

CWA 16926-19:2020 (E)
#define WFS_BIO_DEVHWERROR WFS_STAT_DEVHWERROR
#define WFS_BIO_DEVUSERERROR WFS_STAT_DEVUSERERROR
#define WFS_BIO_DEVBUSY WFS_STAT_DEVBUSY
#define WFS_BIO_DEVFRAUDATTEMPT WFS_STAT_DEVFRAUDATTEMPT
#define WFS_BIO_DEVPOTENTIALFRAUD WFS_STAT_DEVPOTENTIALFRAUD
/* values of WFSBIOSTATUS.dwSubject */
#define WFS_BIO_SUBJECTPRESENT (1)
#define WFS_BIO_SUBJECTNOTPRESENT (2)
#define WFS_BIO_SUBJECTUNKNOWN (3)
#define WFS_BIO_SUBJECTNOTSUPPORTED (4)
/* Size and max index of dwGuidLights array */
#define WFS_BIO_GUIDLIGHTS_SIZE (32)
#define WFS_BIO_GUIDLIGHTS_MAX (WFS_BIO_GUIDLIGHTS_SIZE - 1)
/* Indices of WFSBIOSTATUS.dwGuidLights [...]
WFSBIOCAPS.dwGuidLights [...] */
#define WFS_BIO_GUIDANCE_BIO (0)
/* Values of WFSBIOSTATUS.dwGuidLights [...]
WFSBIOCAPS.dwGuidLights [...],
WFSBIOSETGUIDLIGHT.wGuidLight */
#define WFS_BIO_GUIDANCE_NOT_AVAILABLE (0x00000000)
#define WFS_BIO_GUIDANCE_OFF (0x00000001)
#define WFS_BIO_GUIDANCE_SLOW_FLASH (0x00000004)
#define WFS_BIO_GUIDANCE_MEDIUM_FLASH (0x00000008)
#define WFS_BIO_GUIDANCE_QUICK_FLASH (0x00000010)
#define WFS_BIO_GUIDANCE_CONTINUOUS (0x00000080)
#define WFS_BIO_GUIDANCE_RED (0x00000100)
#define WFS_BIO_GUIDANCE_GREEN (0x00000200)
#define WFS_BIO_GUIDANCE_YELLOW (0x00000400)
#define WFS_BIO_GUIDANCE_BLUE (0x00000800)
#define WFS_BIO_GUIDANCE_CYAN (0x00001000)
#define WFS_BIO_GUIDANCE_MAGENTA (0x00002000)
#define WFS_BIO_GUIDANCE_WHITE (0x00004000)
#define WFS_BIO_GUIDANCE_ENTRY (0x00100000)
#define WFS_BIO_GUIDANCE_EXIT (0x00200000)
/* values of WFSBIOSTATUS.wDevicePosition
WFSBIODEVICEPOSITION.wPosition */
#define WFS_BIO_DEVICEINPOSITION (0)
#define WFS_BIO_DEVICENOTINPOSITION (1)
#define WFS_BIO_DEVICEPOSUNKNOWN (2)
#define WFS_BIO_DEVICEPOSNOTSUPP (3)
/* values of WFSBIOSTATUS.wAntiFraudModule */
#define WFS_BIO_AFMNOTSUPP (0)
#define WFS_BIO_AFMOK (1)
#define WFS_BIO_AFMINOP (2)
#define WFS_BIO_AFMDEVICEDETECTED (3)
#define WFS_BIO_AFMUNKNOWN (4)
/* values of WFSBIOCAPS.fwType */
#define WFS_BIO_TYPE_FACIAL_FEATURES (0x0001)
#define WFS_BIO_TYPE_VOICE (0x0002)
#define WFS_BIO_TYPE_FINGERPRINT (0x0004)
#define WFS_BIO_TYPE_FINGERVEIN (0x0008)
#define WFS_BIO_TYPE_IRIS (0x0010)
#define WFS_BIO_TYPE_RETINA (0x0020)
#define WFS_BIO_TYPE_HAND_GEOMETRY (0x0040)
#define WFS_BIO_TYPE_THERMAL_FACE (0x0080)
50

CWA 16926-19:2020 (E)
#define WFS_BIO_TYPE_THERMAL_HAND (0x0100)
#define WFS_BIO_TYPE_PALM_VEIN (0x0200)
#define WFS_BIO_TYPE_SIGNATURE (0x0400)
/* values of WFSBIOCAPS.fwDataFormats and
WFSBIODATATYPE.dwFormat and
WFSBIOREAD.lpdwDataFormats */
#define WFS_BIO_ISOFID (0x0001)
#define WFS_BIO_ISOFMD (0x0002)
#define WFS_BIO_ANSIFID (0x0004)
#define WFS_BIO_ANSIFMD (0x0008)
#define WFS_BIO_QSO (0x0010)
#define WFS_BIO_WSQ (0x0020)
#define WFS_BIO_RESERVED_RAW_1 (0x0040)
#define WFS_BIO_RESERVED_TEMPLATE_1 (0x0080)
#define WFS_BIO_RESERVED_RAW_2 (0x0100)
#define WFS_BIO_RESERVED_TEMPLATE_2 (0x0200)
#define WFS_BIO_RESERVED_RAW_3 (0x0400)
#define WFS_BIO_RESERVED_TEMPLATE_3 (0x0800)
/* values of WFSBIOCAPS.fwEncryptionAlgorithms and
WFSBIODATATYPE.dwAlgorithm */
#define WFS_BIO_CRYPT_NONE (0x0000)
#define WFS_BIO_CRYPT_TRIDESECB (0x0001)
#define WFS_BIO_CRYPT_TRIDESCBC (0x0002)
#define WFS_BIO_CRYPT_TRIDESCFB (0x0004)
#define WFS_BIO_CRYPT_RSA (0x0008)
/* values of WFSBIOCAPS.fwStorage */
#define WFS_BIO_STORAGE_NONE (0x0000)
#define WFS_BIO_STORAGE_SECURE (0x0001)
#define WFS_BIO_STORAGE_CLEAR (0x0002)
/* values of WFSBIOCAPS.fwPersistenceModes and
WFSBIOSTATUS.dwDataPersistence and
WFSBIOPERSISTDATA.dwPersistenceMode */
#define WFS_BIO_PS_NONE (0x0000)
#define WFS_BIO_PS_PERSIST (0x0001)
#define WFS_BIO_PS_AUTOCLEAR (0x0002)
/* values of WFSBIOCAPS.dwMatchSupported */
#define WFS_BIO_MTC_STORED_MATCH_NONE (0x0000)
#define WFS_BIO_MTC_STORED_MATCH (0x0001)
#define WFS_BIO_MTC_COMBINED_MATCH (0x0002)
/* values of WFSBIOCAPS.fwScanModes and
WFSBIOREAD.usMode */
#define WFS_BIO_MODE_SCAN (0x0001)
#define WFS_BIO_MODE_MATCH (0x0002)
/* values of WFSBIOCAPS.fwCompareModes and
WFSBIOMATCH.usCompareMode */
#define WFS_BIO_COMP_NONE (0x0000)
#define WFS_BIO_COMP_VERIFY (0x0001)
#define WFS_BIO_COMP_IDENTIFY (0x0002)
/* values of WFSBIOCAPS.fwClearData and
WFSBIOCLEAR.fwClearData and
WFSBIORESET.fwClearData and
WFSBIODATACLEARED.fwClearData */
#define WFS_BIO_CLR_NONE (0x0000)
#define WFS_BIO_CLR_SCANNEDDATA (0x0001)
51

CWA 16926-19:2020 (E)
#define WFS_BIO_CLR_IMPORTEDDATA (0x0002)
#define WFS_BIO_CLR_SETMATCHDATA (0x0004)
/* values of WFSBIOKEYINFO.dwUse */
#define WFS_BIO_USECRYPT (0x0001)
#define WFS_BIO_USERSAPUBLIC (0x0002)
/* XFS BIO Errors */
#define WFS_ERR_BIO_NOIMPORTEDDATA (-(BIO_SERVICE_OFFSET + 0))
#define WFS_ERR_BIO_READFAILED (-(BIO_SERVICE_OFFSET + 1))
#define WFS_ERR_BIO_MODENOTSUPP (-(BIO_SERVICE_OFFSET + 2))
#define WFS_ERR_BIO_FORMATNOTSUPP (-(BIO_SERVICE_OFFSET + 3))
#define WFS_ERR_BIO_INVALIDDATA (-(BIO_SERVICE_OFFSET + 4))
#define WFS_ERR_BIO_CAPACITYEXCEEDED (-(BIO_SERVICE_OFFSET + 5))
#define WFS_ERR_BIO_INVALIDIDENTIFIER (-(BIO_SERVICE_OFFSET + 6))
#define WFS_ERR_BIO_NOCAPTUREDDATA (-(BIO_SERVICE_OFFSET + 7))
#define WFS_ERR_BIO_KEYNOTFOUND (-(BIO_SERVICE_OFFSET + 8))
#define WFS_ERR_BIO_INVALID_PORT (-(BIO_SERVICE_OFFSET + 9))
#define WFS_ERR_BIO_POWERSAVETOOSHORT (-(BIO_SERVICE_OFFSET + 10))
#define WFS_ERR_BIO_COMMANDUNSUPP (-(BIO_SERVICE_OFFSET + 11))
#define WFS_ERR_BIO_SYNCHRONIZEUNSUPP (-(BIO_SERVICE_OFFSET + 12))
#define WFS_ERR_BIO_INVALIDCOMPAREMODE (-(BIO_SERVICE_OFFSET + 13))
#define WFS_ERR_BIO_INVALIDTHRESHOLD (-(BIO_SERVICE_OFFSET + 14))
/*=================================================================*/
/* BIO Info Command Structures and variables */
/*=================================================================*/
typedef struct _wfs_bio_status
{
WORD fwDevice;
DWORD dwSubject;
BOOL bCaptured;
DWORD dwDataPersistence;
DWORD dwRemainingStorage;
LPSTR lpszExtra;
WORD wDevicePosition;
DWORD dwGuidLights[WFS_BIO_GUIDLIGHTS_SIZE];
USHORT usPowerSaveRecoveryTime;
WORD wAntiFraudModule;
} WFSBIOSTATUS, *LPWFSBIOSTATUS;
typedef struct _wfs_bio_caps
{
WORD wClass;
DWORD fwType;
BOOL bCompound;
USHORT usMaxCapture;
DWORD dwTemplateStorage;
DWORD fwDataFormats;
DWORD fwEncryptionAlgorithms;
WORD fwStorage;
DWORD fwPersistenceModes;
DWORD dwMatchSupported;
WORD fwScanModes;
WORD fwCompareModes;
DWORD fwClearData;
LPSTR lpszExtra;
DWORD dwGuidLights[WFS_BIO_GUIDLIGHTS_SIZE];
BOOL bPowerSaveControl;
BOOL bAntiFraudModule;
LPDWORD lpdwSynchronizableCommands;
} WFSBIOCAPS, *LPWFSBIOCAPS;
typedef struct _wfs_bio_data_type
{
52

CWA 16926-19:2020 (E)
DWORD dwFormat;
DWORD dwAlgorithm;
LPSTR lpszKeyName;
} WFSBIODATATYPE, *LPWFSBIODATATYPE;
typedef struct _wfs_bio_storage
{
USHORT usIdentifier;
LPWFSBIODATATYPE lpType;
} WFSBIOSTORAGE, *LPWFSBIOSTORAGE;
typedef struct _wfs_bio_storage_list
{
USHORT usCount;
LPWFSBIOSTORAGE *lppStorageList;
} WFSBIOSTORAGELIST, *LPWFSBIOSTORAGELIST;
typedef struct _wfs_bio_key_info
{
LPSTR lpszKeyName;
DWORD dwUse;
BOOL bLoaded;
} WFSBIOKEYINFO, *LPWFSBIOKEYINFO;
/*=================================================================*/
/* BIO Execute Command Structures */
/*=================================================================*/
typedef struct _wfs_bio_read
{
USHORT usCount;
LPWFSBIODATATYPE *lppTypes;
USHORT usNumCaptures;
USHORT usMode;
} WFSBIOREAD, *LPWFSBIOREAD;
typedef struct _wfs_bio_hex_data
{
USHORT usLength;
LPBYTE lpbData;
} WFSXBIODATA, *LPWFSXBIODATA;
typedef struct _wfs_bio_data
{
LPWFSBIODATATYPE lpType;
LPWFSXBIODATA lpxData;
} WFSBIODATA, *LPWFSBIODATA;
typedef struct _wfs_bio_read_data
{
USHORT usCount;
LPWFSBIODATA *lppBioDataList;
} WFSBIOREADDATA, *LPWFSBIOREADDATA;
typedef struct _wfs_bio_import_data
{
USHORT usCount;
LPWFSBIODATA *lppBioDataList;
} WFSBIOIMPORTDATA, *LPWFSBIOIMPORTDATA;
typedef struct _wfs_bio_match
{
USHORT usCompareMode;
USHORT usIdentifier;
USHORT usMaximum;
USHORT usThreshold;
} WFSBIOMATCH, *LPWFSWFSBIOMATCH;
typedef struct _wfs_bio_candidate
{
53

CWA 16926-19:2020 (E)
USHORT usConfidenceLevel;
USHORT usIdentifier;
LPWFSBIODATA lpData;
} WFSBIOCANDIDATE, *LPWFSBIOCANDIDATE;
typedef struct _wfs_bio_match_result
{
USHORT usCount;
LPWFSBIOCANDIDATE *lppTemplateList;
} WFSBIOMATCHRESULT, *LPWFSBIOMATCHRESULT;
typedef struct _wfs_bio_clear
{
DWORD fwClearData;
} WFSBIOCLEAR, *LPWFSBIOCLEAR;
typedef struct _wfs_bio_reset
{
DWORD fwClearData;
} WFSBIORESET, *LPWFSBIORESET;
typedef struct _wfs_bio_persist_data
{
DWORD dwPersistenceMode;
} WFSBIOPERSISTDATA, *LPWFSBIOPERSISTDATA;
typedef struct _wfs_bio_set_guidlight
{
WORD wGuidLight;
DWORD dwCommand;
} WFSBIOSETGUIDLIGHT, *LPWFSBIOSETGUIDLIGHT;
typedef struct _wfs_bio_power_save_control
{
USHORT usMaxPowerSaveRecoveryTime;
} WFSBIOPOWERSAVECONTROL, *LPWFSBIOPOWERSAVECONTROL;
typedef struct _wfs_bio_synchronize_command
{
DWORD dwCommand;
LPVOID lpCmdData;
} WFSBIOSYNCHRONIZECOMMAND, *LPWFSBIOSYNCHRONIZECOMMAND;
/*=================================================================*/
/* BIO Events Structures */
/*=================================================================*/
typedef struct _wfs_bio_data_cleared
{
DWORD fwClearData;
} WFSBIODATACLEARED, *LPWFSBIODATACLEARED;
typedef struct _wfs_bio_device_position
{
WORD wPosition;
} WFSBIODEVICEPOSITION, *LPWFSBIODEVICEPOSITION;
typedef struct _wfs_bio_power_save_change
{
USHORT usPowerSaveRecoveryTime;
} WFSBIOPOWERSAVECHANGE, *LPWFSBIOPOWERSAVECHANGE;
/* restore alignment */
#pragma pack (pop)
#ifdef __cplusplus
} /*extern "C"*/
54

CWA 16926-19:2020 (E)
#endif
#endif /* __INC_XFSBIO__H */
55
