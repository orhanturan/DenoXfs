---
source: XfsCam.pdf
document: XfsCam
device_class: CAM
part: 13
section_id: ""
title: "7. C - Header file"
type: header
source_lines: [1078, 1310]
---

<!-- 7. C - Header file -->

7. C - Header file
/******************************************************************************
* *
* xfscam.h XFS - Camera (CAM) definitions *
* *
* Version 3.50 (November 18 2022) *
* *
******************************************************************************/
#ifndef __INC_XFSCAM__H
#define __INC_XFSCAM__H
#ifdef __cplusplus
extern "C" {
#endif
#include <xfsapi.h>
/* be aware of alignment */
#pragma pack (push, 1)
/* values of WFSCAMCAPS.wClass */
#define WFS_SERVICE_CLASS_CAM (10)
#define WFS_SERVICE_VERSION_CAM (0x3203) /* Version 3.50 */
#define WFS_SERVICE_NAME_CAM "CAM"
#define CAM_SERVICE_OFFSET (WFS_SERVICE_CLASS_CAM * 100)
/* CAM Info Commands */
#define WFS_INF_CAM_STATUS (CAM_SERVICE_OFFSET + 1)
#define WFS_INF_CAM_CAPABILITIES (CAM_SERVICE_OFFSET + 2)
/* CAM Execute Commands */
#define WFS_CMD_CAM_TAKE_PICTURE (CAM_SERVICE_OFFSET + 1)
#define WFS_CMD_CAM_RESET (CAM_SERVICE_OFFSET + 2)
#define WFS_CMD_CAM_TAKE_PICTURE_EX (CAM_SERVICE_OFFSET + 3)
#define WFS_CMD_CAM_SYNCHRONIZE_COMMAND (CAM_SERVICE_OFFSET + 4)
#define WFS_CMD_CAM_TAKE_PICTURE_350 (CAM_SERVICE_OFFSET + 5)
#define WFS_CMD_CAM_TAKE_VIDEO (CAM_SERVICE_OFFSET + 6)
/* CAM Messages */
#define WFS_USRE_CAM_MEDIATHRESHOLD (CAM_SERVICE_OFFSET + 1)
#define WFS_EXEE_CAM_INVALIDDATA (CAM_SERVICE_OFFSET + 2)
#define WFS_USRE_CAM_VIDEOSTART (CAM_SERVICE_OFFSET + 3)
#define WFS_USRE_CAM_VIDEOEND (CAM_SERVICE_OFFSET + 4)
#define WFS_USRE_CAM_VIDEOERROR (CAM_SERVICE_OFFSET + 5)
/* values of WFSCAMSTATUS.fwDevice */
#define WFS_CAM_DEVONLINE WFS_STAT_DEVONLINE
#define WFS_CAM_DEVOFFLINE WFS_STAT_DEVOFFLINE
#define WFS_CAM_DEVPOWEROFF WFS_STAT_DEVPOWEROFF
#define WFS_CAM_DEVNODEVICE WFS_STAT_DEVNODEVICE
#define WFS_CAM_DEVHWERROR WFS_STAT_DEVHWERROR
#define WFS_CAM_DEVUSERERROR WFS_STAT_DEVUSERERROR
#define WFS_CAM_DEVBUSY WFS_STAT_DEVBUSY
#define WFS_CAM_DEVFRAUDATTEMPT WFS_STAT_DEVFRAUDATTEMPT
#define WFS_CAM_DEVPOTENTIALFRAUD WFS_STAT_DEVPOTENTIALFRAUD
/* number of cameras supported/length of WFSCAMSTATUS.fwCameras field */
#define WFS_CAM_CAMERAS_SIZE (8)
#define WFS_CAM_CAMERAS_MAX (WFS_CAM_CAMERAS_SIZE - 1)
32

CWA 16926-12:2022 (E)
/* indices of WFSCAMSTATUS.fwMedia[...]
WFSCAMSTATUS.fwCameras [...]
WFSCAMSTATUS.usPictures[...]
WFSCAMCAPS.fwCameras [...]
WFSCAMTAKEPICT.wCamera */
#define WFS_CAM_ROOM (0)
#define WFS_CAM_PERSON (1)
#define WFS_CAM_EXITSLOT (2)
/* values of WFSCAMSTATUS.fwMedia */
#define WFS_CAM_MEDIAOK (0)
#define WFS_CAM_MEDIAHIGH (1)
#define WFS_CAM_MEDIAFULL (2)
#define WFS_CAM_MEDIAUNKNOWN (3)
#define WFS_CAM_MEDIANOTSUPP (4)
/* values of WFSCAMSTATUS.fwCameras */
#define WFS_CAM_CAMNOTSUPP (0)
#define WFS_CAM_CAMOK (1)
#define WFS_CAM_CAMINOP (2)
#define WFS_CAM_CAMUNKNOWN (3)
#define WFS_CAM_CAMRECORDING (4)
/* values of WFSCAMCAPS.fwType */
#define WFS_CAM_TYPE_CAM (1)
/* values of WFSCAMCAPS.fwCameras */
#define WFS_CAM_NOT_AVAILABLE (0)
#define WFS_CAM_AVAILABLE (1)
/* values of WFSCAMCAPS.fwCamData */
#define WFS_CAM_NOTADD (0)
#define WFS_CAM_AUTOADD (1)
#define WFS_CAM_MANADD (2)
/* values of WFSCAMCAPS.fwCharSupport */
#define WFS_CAM_ASCII (0x0001)
#define WFS_CAM_UNICODE (0x0002)
/* values of WFSCAMSTATUS.wAntiFraudModule */
#define WFS_CAM_AFMNOTSUPP (0)
#define WFS_CAM_AFMOK (1)
#define WFS_CAM_AFMINOP (2)
#define WFS_CAM_AFMDEVICEDETECTED (3)
#define WFS_CAM_AFMUNKNOWN (4)
/* values of WFSCAMCAPS.lppCameraDetail.fwMediaType */
#define WFS_CAM_MEDIAPICTURE (0x0001)
#define WFS_CAM_MEDIAVIDEO (0x0002)
/* flags of WFSCAMCAPS.fwVideoFormatSupport */
#define WFS_CAM_VIDEOFORMAT_AVI (0x0001)
#define WFS_CAM_VIDEOFORMAT_MOV (0x0002)
#define WFS_CAM_VIDEOFORMAT_WMV (0x0004)
#define WFS_CAM_VIDEOFORMAT_FLV (0x0008)
#define WFS_CAM_VIDEOFORMAT_MPEG (0x0010)
#define WFS_CAM_VIDEOFORMAT_MPEG2 (0x0020)
#define WFS_CAM_VIDEOFORMAT_MPEG4 (0x0040)
#define WFS_CAM_VIDEOFORMAT_MKV (0x0080)
#define WFS_CAM_VIDEOFORMAT_ASF (0x0100)
33

CWA 16926-12:2022 (E)
#define WFS_CAM_VIDEOFORMAT_VOB (0x0200)
/* values of WFSCAMVIDEOERROR.wFailure */
#define WFS_CAM_FAIL_MEDIAFULL (0)
#define WFS_CAM_FAIL_FILEIOERROR (1)
/* XFS CAM Errors */
#define WFS_ERR_CAM_CAMNOTSUPP (-(CAM_SERVICE_OFFSET + 0))
#define WFS_ERR_CAM_MEDIAFULL (-(CAM_SERVICE_OFFSET + 1))
#define WFS_ERR_CAM_CAMINOP (-(CAM_SERVICE_OFFSET + 2))
#define WFS_ERR_CAM_CHARSETNOTSUPP (-(CAM_SERVICE_OFFSET + 3))
#define WFS_ERR_CAM_FILEIOERROR (-(CAM_SERVICE_OFFSET + 4))
#define WFS_ERR_CAM_COMMANDUNSUPP (-(CAM_SERVICE_OFFSET + 5))
#define WFS_ERR_CAM_SYNCHRONIZEUNSUPP (-(CAM_SERVICE_OFFSET + 6))
#define WFS_ERR_CAM_CAMRECORDING (-(CAM_SERVICE_OFFSET + 7))
#define WFS_ERR_CAM_PIXELSIZENOTSUPP (-(CAM_SERVICE_OFFSET + 8))
#define WFS_ERR_CAM_FPSNOTSUPP (-(CAM_SERVICE_OFFSET + 9))
/*=================================================================*/
/* CAM Info Command Structures */
/*=================================================================*/
typedef struct _wfs_cam_status
{
WORD fwDevice;
WORD fwMedia[WFS_CAM_CAMERAS_SIZE];
WORD fwCameras[WFS_CAM_CAMERAS_SIZE];
USHORT usPictures[WFS_CAM_CAMERAS_SIZE];
LPSTR lpszExtra;
WORD wAntiFraudModule;
DWORD dwMediaSize[WFS_CAM_CAMERAS_SIZE];
} WFSCAMSTATUS, *LPWFSCAMSTATUS;
typedef struct _wfs_cam_pixelsize
{
ULONG ulSizeX;
ULONG ulSizeY;
} WFSCAMPIXELSIZE, *LPWFSCAMPIXELSIZE;
typedef struct _wfs_cam_detail
{
WORD wCamera;
WORD fwMediaType;
LPWFSCAMPIXELSIZE lpMaxPixelSize;
USHORT usMaxFps;
DWORD dwKbps;
} WFSCAMDETAIL, *LPWFSCAMDETAIL;
typedef struct _wfs_cam_caps
{
WORD wClass;
WORD fwType;
WORD fwCameras[WFS_CAM_CAMERAS_SIZE];
USHORT usMaxPictures;
WORD fwCamData;
USHORT usMaxDataLength;
WORD fwCharSupport;
LPSTR lpszExtra;
BOOL bPictureFile;
BOOL bAntiFraudModule;
LPDWORD lpdwSynchronizableCommands;
LPWFSCAMDETAIL *lppCameraDetail;
DWORD fwVideoFormatSupport;
DWORD dwMaxMediaSize;
} WFSCAMCAPS, *LPWFSCAMCAPS;
/*=================================================================*/
/* CAM Execute Command Structures */
34

CWA 16926-12:2022 (E)
/*=================================================================*/
typedef struct _wfs_cam_take_picture
{
WORD wCamera;
LPSTR lpszCamData;
LPWSTR lpszUNICODECamData;
} WFSCAMTAKEPICT, *LPWFSCAMTAKEPICT;
typedef struct _wfs_cam_take_picture_ex
{
WORD wCamera;
LPSTR lpszCamData;
LPWSTR lpszUNICODECamData;
LPSTR lpszPictureFile;
} WFSCAMTAKEPICTEX, *LPWFSCAMTAKEPICTEX;
typedef struct _wfs_cam_synchronize_command
{
DWORD dwCommand;
LPVOID lpCmdData;
} WFSCAMSYNCHRONIZECOMMAND, *LPWFSCAMSYNCHRONIZECOMMAND;
typedef struct _wfs_cam_cappos
{
ULONG ulPositionX;
ULONG ulPositionY;
} WFSCAMCAPPOS, *LPWFSCAMCAPPOS;
typedef struct _wfs_cam_cap
{
LPSTR lpszCamData;
LPSTR lpszUNICODECamData;
LPWFSCAMCAPPOS lpCamDataPosition;
} WFSCAMCAP, *LPWFSCAMCAP;
typedef struct _wfs_cam_take_picture_350
{
WORD wCamera;
LPSTR lpszPictureFile;
LPWFSCAMPIXELSIZE lpPixelSize;
LPWFSCAMCAP *lppCaptions;
} WFSCAMTAKEPICT350, *LPWFSCAMTAKEPICT350;
typedef struct _wfs_cam_take_video
{
WORD wCamera;
LPWFSCAMPIXELSIZE lpPixelSize;
USHORT usFps;
LPSTR lpszVideoFile;
LPWFSCAMCAP *lppCaptions;
} WFSCAMTAKEVIDEO, *LPWFSCAMTAKEVIDEO;
/*=================================================================*/
/* CAM Event Structures */
/*=================================================================*/
typedef struct _wfs_cam_video_error
{
WORD wCamera;
WORD wFailure;
} WFSCAMVIDEOERROR, *LPWFSCAMVIDEOERROR;
/* restore alignment */
#pragma pack (pop)
#ifdef __cplusplus
} /*extern "C"*/
#endif
35

CWA 16926-12:2022 (E)
#endif /* __INC_XFSCAM__H */
36
