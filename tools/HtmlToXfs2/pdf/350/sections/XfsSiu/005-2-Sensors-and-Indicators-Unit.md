---
source: XfsSiu.pdf
document: XfsSiu
device_class: SIU
part: 11
section_id: ""
title: "2. Sensors and Indicators Unit"
type: section
source_lines: [266, 286]
---

<!-- 2. Sensors and Indicators Unit -->

2. Sensors and Indicators Unit
This specification describes the functionality of the services provided by the Sensors and Indicators Unit (SIU)
services under WOSA/XFS, by defining the service-specific commands that can be issued, using the WFSGetInfo,
WFSAsyncGetInfo, WFSExecute and WFSAsyncExecute functions.
This section describes the functions provided by a generic Sensors and Indicators Unit service. This service allows
for the operation of the following categories of ports:
• Door sensors, such as cabinet, safe or vandal shield doors.
• Alarm sensors, such as tamper, seismic or heat sensors.
• Generic sensors, such as proximity or ambient light sensors.
• Key switch sensors, such as the ATM operator switch.
• Lamp/sign indicators, such as fascia light or audio indicators.
Note that while the SIU device class provides some basic support for guidance lights, extended guidance
light functionality is specified in the individual device class specifications. Therefore it is recommended
that device guidance lights be supported and controlled via the individual device classes.
• Auxiliary indicators.
• Enhanced Audio Controller, for use by the partially sighted.
In self-service devices, the sensors and indicators unit is capable of dealing with external sensors, such as door
switches, locks, alarms and proximity sensors, as well as external indicators, such as turning on lamps or heating.
9

CWA 16926-10:2022 (E)
