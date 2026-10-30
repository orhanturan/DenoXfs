---
source: XfsCam.pdf
document: XfsCam
device_class: CAM
part: 13
section_id: ""
title: "2. Banking Cameras"
type: section
source_lines: [258, 270]
---

<!-- 2. Banking Cameras -->

2. Banking Cameras
This specification describes the functionality of the services provided by the Camera (CAM) services under XFS,
by defining the service-specific commands that can be issued, using the WFSGetInfo, WFSAsyncGetInfo,
WFSExecute and WFSAsyncExecute functions.
Banking camera systems usually consist of a recorder, a video mixer and one or more cameras. If there are several
cameras, each camera focuses a special place within the self-service area (e.g. the room, the customer or the cash
tray). By using the video mixer it can be decided, which of the cameras should take the next photo. Furthermore
data can be given to be inserted in the photo (e.g. date, time or bank code).
If there is only one camera that can switch to take photos from different positions, it is presented by the Service
Provider as a set of cameras, one for each of its possible positions.
9

CWA 16926-12:2022 (E)
