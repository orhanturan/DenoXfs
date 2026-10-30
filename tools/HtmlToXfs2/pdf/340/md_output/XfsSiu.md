CEN
CWA 16926-10
WORKSHOP
AGREEMENT
February 2020
ICS
35.200; 35.240.15; 35.240.40
English version
Extensions for Financial Services (XFS) interface
specification Release 3.40 - Part 10: Sensors and Indicators
Unit Device Class Interface - Programmer's Reference
This CEN Workshop Agreement has been drafted and approved by a Workshop of representatives of interested parties, the
constitution of which is indicated in the foreword of this Workshop Agreement.
The formal process followed by the Workshop in the development of this Workshop Agreement has been endorsed by the
National Members of CEN but neither the National Members of CEN nor the CEN-CENELEC Management Centre can be held
accountable for the technical content of this CEN Workshop Agreement or possible conflicts with standards or legislation.
This CEN Workshop Agreement can in no way be held as being an official standard developed by CEN and its Members.
This CEN Workshop Agreement is publicly available as a reference document from the CEN Members National Standard Bodies.
CEN members are the national standards bodies of Austria, Belgium, Bulgaria, Croatia, Cyprus, Czech Republic, Denmark, Estonia, Finland,
France, Germany, Greece, Hungary, Iceland, Ireland, Italy, Latvia, Lithuania, Luxembourg, Malta, Netherlands, Norway, Poland, Portugal,
Republic of North Macedonia, Romania, Serbia, Slovakia, Slovenia, Spain, Sweden, Switzerland, Turkey and United Kingdom.
EUROPEAN COMMITTEE FOR STANDARDIZATION
COMITÉ EUROPÉEN DE NORMALISATION
EUROPÄISCHES KOMITEE FÜR NORMUNG
CEN-CENELECManagement Centre: Rue de la Science 23, B-1040 Brussels
© 2020 CEN All rights of exploitation in any form and by any means reserved worldwide for CEN national Members.
Ref. No.:CWA 16926-10:2020 E

Table of Contents
European Foreword ...................................................................................................... 3
1. Introduction .......................................................................................................... 7
1.1 Background to Release 3.40 ....................................................................................................... 7
1.2 XFS Service-Specific Programming ........................................................................................... 7
2. Sensors and Indicators Unit ............................................................................... 9
2.1 Enhanced Audio Controller Overview...................................................................................... 10
2.2 Enhanced Microphone Controller Overview ........................................................................... 14
3. References ......................................................................................................... 15
4. Info Commands ................................................................................................. 16
4.1 WFS_INF_SIU_STATUS ............................................................................................................. 16
4.2 WFS_INF_SIU_CAPABILITIES ................................................................................................... 27
4.3 WFS_INF_SIU_GET_AUTOSTARTUP_TIME ............................................................................ 38
5. Execute Commands .......................................................................................... 40
5.1 WFS_CMD_SIU_ENABLE_EVENTS .......................................................................................... 40
5.2 WFS_CMD_SIU_SET_PORTS ................................................................................................... 49
5.3 WFS_CMD_SIU_SET_DOOR ..................................................................................................... 56
5.4 WFS_CMD_SIU_SET_INDICATOR ............................................................................................ 57
5.5 WFS_CMD_SIU_SET_AUXILIARY ............................................................................................. 59
5.6 WFS_CMD_SIU_SET_GUIDLIGHT ............................................................................................ 62
5.7 WFS_CMD_SIU_RESET ............................................................................................................. 63
5.8 WFS_CMD_SIU_POWER_SAVE_CONTROL ............................................................................ 64
5.9 WFS_CMD_SIU_SET_AUTOSTARTUP_TIME .......................................................................... 65
5.10 WFS_CMD_SIU_SYNCHRONIZE_COMMAND ......................................................................... 67
5.11 WFS_CMD_SIU_SET_GUIDLIGHT_EX ..................................................................................... 68
6. Events ................................................................................................................. 69
6.1 WFS_SRVE_SIU_PORT_STATUS ............................................................................................. 69
6.2 WFS_EXEE_SIU_PORT_ERROR ............................................................................................... 72
6.3 WFS_SRVE_SIU_POWER_SAVE_CHANGE ............................................................................. 75
7. C - Header file .................................................................................................... 76
2

CWA 16926-10:2020 (E)
European Foreword
This CEN Workshop Agreement has been developed in accordance with the CEN-CENELEC Guide 29
“CEN/CENELEC Workshop Agreements – The way to rapid consensus” and with the relevant provisions of
CEN/CENELEC Internal Regulations – Part 2. It was approved by a Workshop of representatives of interested
parties on 2019-10-08, the constitution of which was supported by CEN following several public calls for
participation, the first of which was made on 1998-06-24. However, this CEN Workshop Agreement does not
necessarily include all relevant stakeholders.
The final text of this CEN Workshop Agreement was provided to CEN for publication on 2019-12-12.
The following organizations and individuals developed and approved this CEN Workshop Agreement:
• ATM Japan LTD
• AURIGA SPA
• BANK OF AMERICA
• CASHWAY TECHNOLOGY
• CHINAL ECTRONIC FINANCIAL EQUIPMENT SYSTEM CO.
• CIMA SPA
• CLEAR2PAY SCOTLAND LIMITED
• DIEBOLD NIXDORF
• EASTERN COMMUNICATIONS CO. LTD – EASTCOM
• FINANZ INFORMATIK
• FUJITSU FRONTECH LIMITED
• FUJITSU TECHNOLOGY
• GLORY LTD
• GRG BANKING EQUIPMENT HK CO LTD
• HESS CASH SYSTEMS GMBH & CO. KG
• HITACHI OMRON TS CORP.
• HYOSUNG TNS INC
• JIANGSU GUOGUANG ELECTRONIC INFORMATION TECHNOLOGY
• KAL
• KEBA AG
• NCR FSG
• NEC CORPORATION
• OKI ELECTRIC INDUSTRY SHENZHEN
3

CWA 16926-10:2020 (E)
• OKI ELECTRONIC INDUSTRY CO
• PERTO S/A
• REINER GMBH & CO KG
• SALZBURGER BANKEN SOFTWARE
• SIGMA SPA
• TEB
• ZIJIN FULCRUM TECHNOLOGY CO
It is possible that some elements of this CEN/CWA may be subject to patent rights. The CEN-CENELEC policy on
patent rights is set out in CEN-CENELEC Guide 8 “Guidelines for Implementation of the Common IPR Policy on
Patents (and other statutory intellectual property rights based on inventions)”. CEN shall not be held responsible for
identifying any or all such patent rights.
The Workshop participants have made every effort to ensure the reliability and accuracy of the technical and non-
technical content of CWA 16926-10, but this does not guarantee, either explicitly or implicitly, its correctness.
Users of CWA 16926-10 should be aware that neither the Workshop participants, nor CEN can be held liable for
damages or losses of any kind whatsoever which may arise from its application. Users of CWA 16926-10 do so on
their own responsibility and at their own risk.
The CWA is published as a multi-part document, consisting of:
Part 1: Application Programming Interface (API) - Service Provider Interface (SPI) - Programmer's Reference
Part 2: Service Classes Definition - Programmer's Reference
Part 3: Printer and Scanning Device Class Interface - Programmer's Reference
Part 4: Identification Card Device Class Interface - Programmer's Reference
Part 5: Cash Dispenser Device Class Interface - Programmer's Reference
Part 6: PIN Keypad Device Class Interface - Programmer's Reference
Part 7: Check Reader/Scanner Device Class Interface - Programmer's Reference
Part 8: Depository Device Class Interface - Programmer's Reference
Part 9: Text Terminal Unit Device Class Interface - Programmer's Reference
Part 10: Sensors and Indicators Unit Device Class Interface - Programmer's Reference
Part 11: Vendor Dependent Mode Device Class Interface - Programmer's Reference
Part 12: Camera Device Class Interface - Programmer's Reference
Part 13: Alarm Device Class Interface - Programmer's Reference
Part 14: Card Embossing Unit Device Class Interface - Programmer's Reference
Part 15: Cash-In Module Device Class Interface - Programmer's Reference
Part 16: Card Dispenser Device Class Interface - Programmer's Reference
Part 17: Barcode Reader Device Class Interface - Programmer's Reference
Part 18: Item Processing Module Device Class Interface - Programmer's Reference
Part 19: Biometrics Device Class Interface - Programmer's Reference
Parts 20 - 28: Reserved for future use.
Parts 29 through 47 constitute an optional addendum to this CWA. They define the integration between the SNMP
standard and the set of status and statistical information exported by the Service Providers.
Part 29: XFS MIB Architecture and SNMP Extensions - Programmer’s Reference
4

CWA 16926-10:2020 (E)
Part 30: XFS MIB Device Specific Definitions - Printer Device Class
Part 31: XFS MIB Device Specific Definitions - Identification Card Device Class
Part 32: XFS MIB Device Specific Definitions - Cash Dispenser Device Class
Part 33: XFS MIB Device Specific Definitions - PIN Keypad Device Class
Part 34: XFS MIB Device Specific Definitions - Check Reader/Scanner Device Class
Part 35: XFS MIB Device Specific Definitions - Depository Device Class
Part 36: XFS MIB Device Specific Definitions - Text Terminal Unit Device Class
Part 37: XFS MIB Device Specific Definitions - Sensors and Indicators Unit Device Class
Part 38: XFS MIB Device Specific Definitions - Camera Device Class
Part 39: XFS MIB Device Specific Definitions - Alarm Device Class
Part 40: XFS MIB Device Specific Definitions - Card Embossing Unit Class
Part 41: XFS MIB Device Specific Definitions - Cash-In Module Device Class
Part 42: Reserved for future use.
Part 43: XFS MIB Device Specific Definitions - Vendor Dependent Mode Device Class
Part 44: XFS MIB Application Management
Part 45: XFS MIB Device Specific Definitions - Card Dispenser Device Class
Part 46: XFS MIB Device Specific Definitions - Barcode Reader Device Class
Part 47: XFS MIB Device Specific Definitions - Item Processing Module Device Class
Part 48: XFS MIB Device Specific Definitions - Biometrics Device Class
Parts 49 - 60 are reserved for future use.
Part 61: Application Programming Interface (API) - Migration from Version 3.30 (CWA 16926) to Version 3.40
(this CWA) - Service Provider Interface (SPI) - Programmer's Reference
Part 62: Printer and Scanning Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40
(this CWA) - Programmer's Reference
Part 63: Identification Card Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40
(this CWA) - Programmer's Reference
Part 64: Cash Dispenser Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this
CWA) - Programmer's Reference
Part 65: PIN Keypad Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this
CWA) - Programmer's Reference
Part 66: Check Reader/Scanner Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version
3.40 (this CWA) - Programmer's Reference
Part 67: Depository Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this
CWA) - Programmer's Reference
Part 68: Text Terminal Unit Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40
(this CWA) - Programmer's Reference
Part 69: Sensors and Indicators Unit Device Class Interface - Migration from Version 3.30 (CWA 16926) to
Version 3.40 (this CWA) - Programmer's Reference
Part 70: Vendor Dependent Mode Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version
3.40 (this CWA) - Programmer's Reference
Part 71: Camera Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this CWA) -
Programmer's Reference
Part 72: Alarm Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this CWA) -
Programmer's Reference
5

CWA 16926-10:2020 (E)
Part 73: Card Embossing Unit Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40
(this CWA) - Programmer's Reference
Part 74: Cash-In Module Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this
CWA) - Programmer's Reference
Part 75: Card Dispenser Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this
CWA) - Programmer's Reference
Part 76: Barcode Reader Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version 3.40 (this
CWA) - Programmer's Reference
Part 77: Item Processing Module Device Class Interface - Migration from Version 3.30 (CWA 16926) to Version
3.40 (this CWA) - Programmer's Reference
In addition to these Programmer's Reference specifications, the reader of this CWA is also referred to a
complementary document, called Release Notes. The Release Notes contain clarifications and explanations on the
CWA specifications, which are not requiring functional changes. The current version of the Release Notes is
available online from: https://www.cen.eu/work/Sectors/Digital_society/Pages/WSXFS.aspx.
The information in this document represents the Workshop's current views on the issues discussed as of the date of
publication. It is provided for informational purposes only and is subject to change without notice. CEN makes no
warranty, express or implied, with respect to this document.
6

CWA 16926-10:2020 (E)
1. Introduction
1.1 Background to Release 3.40
The CEN/XFS Workshop aims to promote a clear and unambiguous specification defining a multi-vendor software
interface to financial peripheral devices. The XFS (eXtensions for Financial Services) specifications are developed
within the CEN (European Committee for Standardization/Information Society Standardization System) Workshop
environment. CEN Workshops aim to arrive at a European consensus on an issue that can be published as a CEN
Workshop Agreement (CWA).
The CEN/XFS Workshop encourages the participation of both banks and vendors in the deliberations required to
create an industry standard. The CEN/XFS Workshop achieves its goals by focused sub-groups working
electronically and meeting quarterly.
Release 3.40 of the XFS specification is based on a C API and is delivered with the continued promise for the
protection of technical investment for existing applications. This release of the specification extends the
functionality and capabilities of the existing devices covered by the specification. Notable enhancements include:
• Common API level based ‘Service Information’ command to report Service Provider information,
data and versioning.
• Common API level based events to report changes in status and invalid parameters.
• Support for Advanced Encryption Standard (AES) in PIN.
• VDM Entry Without Closing XFS Service Providers.
• Addition of a Biometrics device class.
• CDM/CIM Note Classification List handling.
• Support for Derived Unique Key Per Transaction (DUKPT) in PIN.
• Addition of Transaction Start/End commands.
• Addition of explicit CIM Prepare/Present commands.
1.2 XFS Service-Specific Programming
The service classes are defined by their service-specific commands and the associated data structures, error codes,
messages, etc. These commands are used to request functions that are specific to one or more classes of Service
Providers, but not all of them, and therefore are not included in the common API for basic or administration
functions.
When a service-specific command is common among two or more classes of Service Providers, the syntax of the
command is as similar as possible across all services, since a major objective of XFS is to standardize function
codes and structures for the broadest variety of services. For example, using the WFSExecute function, the
commands to read data from various services are as similar as possible to each other in their syntax and data
structures.
In general, the specific command set for a service class is defined as a superset of the specific capabilities likely to
be provided by the developers of the services of that class; thus any particular device will normally support only a
subset of the defined command set.
There are three cases in which a Service Provider may receive a service-specific command that it does not support:
The requested capability is defined for the class of Service Providers by the XFS specification, the particular vendor
implementation of that service does not support it, and the unsupported capability is not considered to be
fundamental to the service. In this case, the Service Provider returns a successful completion, but does no operation.
An example would be a request from an application to turn on a control indicator on a passbook printer; the Service
Provider recognizes the command, but since the passbook printer it is managing does not include that indicator, the
Service Provider does no operation and returns a successful completion to the application.
The requested capability is defined for the class of Service Providers by the XFS specification, the particular vendor
implementation of that service does not support it, and the unsupported capability is considered to be fundamental to
7

CWA 16926-10:2020 (E)
the service. In this case, a WFS_ERR_UNSUPP_COMMAND error for Execute commands or
WFS_ERR_UNSUPP_CATEGORY error for Info commands is returned to the calling application. An example
would be a request from an application to a cash dispenser to retract items where the dispenser hardware does not
have that capability; the Service Provider recognizes the command but, since the cash dispenser it is managing is
unable to fulfil the request, returns this error.
The requested capability is not defined for the class of Service Providers by the XFS specification. In this case, a
WFS_ERR_INVALID_COMMAND error for Execute commands or WFS_ERR_INVALID_CATEGORY error
for Info commands is returned to the calling application.
This design allows implementation of applications that can be used with a range of services that provide differing
subsets of the functionalities that are defined for their service class. Applications may use the WFSGetInfo and
WFSAsyncGetInfo commands to inquire about the capabilities of the service they are about to use, and modify
their behavior accordingly, or they may use functions and then deal with error returns to make decisions as to how to
use the service.
8

CWA 16926-10:2020 (E)
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

CWA 16926-10:2020 (E)
2.1 Enhanced Audio Controller Overview
The Enhanced Audio Controller is provided to support the requirements of the American Disabilities Act. The
Enhanced Audio Controller device controls how private and public audio are broadcast when a headset is inserted
into/removed from the Audio Jack, and when the Handset is off-hook/on-hook. In the following ‘Privacy Device’ is
used to refer to either the headset or handset. This device allows audio feedback publicly and/or via the consumer’s
Privacy Device (vendor hardware permitting). For privacy, the device allows input to only be directed to the
consumers’ Privacy Device. In ‘auto’ and ‘semi-auto’ mode (and where the vendor’s hardware allows), public
transmission of audio can be automatically inhibited when the consumer’s Privacy Device is activated. In ‘auto’
mode (and where the vendor’s hardware allows), public transmission of audio can be automatically re-activated
when the consumer’s Privacy Device is deactivated.
The Enhanced Audio Controller provides the application with the following information:
• If a Privacy Device is activated (headset connected/handset off the hook).
• Whether the audio output is to the speakers or to the Privacy Device.
• Privacy/public mode: i.e. whether the activation of the Privacy Device automatically switches public audio
on or off.
The device is managed by the sensors WFS_SIU_ENHANCEDAUDIO, WFS_SIU_HANDSETSENSOR, and an
auxiliary WFS_SIU_ENHANCEDAUDIOCONTROL.
The WFS_SIU_ENHANCEDAUDIO sensor is used to:
• Provide information on the presence of the Audio Jack device.
• To report whether a headset is currently attached.
• Report state change events when a headset is inserted or removed.
The WFS_SIU_HANDSETSENSOR sensor is used to:
• Provide information on the presence of the handset device.
• To report whether a handset is currently off the hook.
• Report state change events when a handset is taken off the hook or put on the hook.
The WFS_SIU_ENHANCEDAUDIOCONTROL auxiliary is used to control the behavior of the Enhanced Audio
Controller. It allows the application to:
• Set the mode of the Enhanced Audio Controller - auto mode, semi-auto mode or manual mode.
• Set the state of the Enhanced Audio Controller- public or private.
A full description of auto, semi-auto and manual mode, as well as public and private states is contained in the
following pages.
The following describes the device behavior during auto and manual mode.
Auto Mode
In auto mode, when a consumer activates a Privacy Device, the audio is automatically directed to the Privacy
Device and the audio is no longer sent to the speakers. When the Privacy Device is deactivated, the audio is
redirected to the speakers. If more than one Privacy Device has been activated, audio is not redirected to the
speakers until all Privacy Devices have been deactivated. The following state diagram completely describes the
behavior of the device in auto mode.
State Description
Auto Mode Public Audio output is played through the public speakers only.
Auto Mode Private Audio is played through the consumer’s Privacy Device only.
10

CWA 16926-10:2020 (E)
Privacy Device deactivated,
Privacy Device deactivated # activated > 0
WFS_SIU_PRIVATEAUDIO_AUTO
Privacy Device activated
Auto Mode Auto Mode
Public Privacy Device deactivated, # activated == 0 Private
WFS_SIU_PUBLICAUDIO_AUTO
Privacy Device activated
Auto Mode State Diagram 1
The dashed-line transitions are caused by application calls to WFS_CMD_SIU_SET_PORTS or
WFS_CMD_SIU_SET_AUXILIARY for the WFS_SIU_ENHANCEDAUDIOCONTROL auxiliary with values of
WFS_SIU_PRIVATEAUDIO_AUTO or WFS_SIU_PUBLICAUDIO_AUTO.
Note that some vendor implementations may not have the ability to allow the application to command the Service
Provider to transition between public and private states. To determine if this feature is available, the application can
query the field fwAuxiliaries[WFS_SIU_ENHANCEDAUDIOCONTROL] in the WFSSIUCAPS structure.
11

CWA 16926-10:2020 (E)
Semi-Auto Mode
This mode is required to ensure customer sensitive information is not broadcast via the public speakers when the
consumer’s Privacy Device is deliberately or otherwise deactivated.
In semi-auto mode, when a consumer’s Privacy Device is activated, the audio is automatically directed to the
Privacy Device and the audio is no longer sent to the speakers. When the Privacy Device is deactivated the audio
remains directed at the existing interface (i.e. not the speakers). If required, the application must explicitly return the
device to its public state if audio is required via the speakers. The following state diagram completely describes the
behavior of the device in auto mode.
State Description
Semi-Auto Mode Public Audio output is played through the public speakers only.
Semi-Auto Mode Private Audio is played through the consumer’s Privacy Device only.
Privacy Device deactivated
Privacy Device deactivated
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO
Semi-auto Semi-auto
Privacy Device activated
Mode Mode
Public Private
WFS_SIU_PUBLICAUDIO_SEMI_AUTO
Privacy Device activated
Semi-Auto Mode State Diagram 2
The dashed-line transitions are caused by application calls to WFS_CMD_SIU_SET_PORTS or
WFS_CMD_SIU_SET_AUXILIARY for the WFS_SIU_ENHANCEDAUDIOCONTROL auxiliary with values of
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO or WFS_SIU_PUBLICAUDIO_SEMI_AUTO.
Manual Mode
In manual mode, when a consumer’s Privacy Device is activated, the audio remains directed at the existing interface
(i.e. the speaker), The application must explicitly change to the other mode, if required. Note that the application
must explicitly return the device to its public state if audio is required via the speakers. The following state diagram
completely describes the behavior of the device in manual mode.
State Description
Manual Mode Public Audio output is played through the public speakers only.
Manual Mode Private Audio is played through the consumer’s Privacy Device only.
12

CWA 16926-10:2020 (E)
Privacy Device activated Privacy Device activated
WFS_SIU_PRIVATEAUDIO_MANUAL
Manual Manual
Mode Mode
Public Private
WFS_SIU_PUBLICAUDIO_MANUAL
Privacy Device deactivated Privacy Device deactivated
Manual Mode State Diagram 1
The dashed-line transitions are caused by application calls to WFS_CMD_SIU_SET_PORTS or
WFS_CMD_SIU_SET_AUXILIARY for the WFS_SIU_ENHANCEDAUDIOCONTROL auxiliary with values of
WFS_SIU_PRIVATEAUDIO_MANUAL or WFS_SIU_PUBLICAUDIO_MANUAL.
Inter-Mode Behavior
The values described in the previous sections (_AUTO, _SEMI_AUTO, and _MANUAL, etc) can also be used to
move from one mode to another. This will then change the mode of the device.
Notes:
• Note that if a vendor device does not support auto mode, or semi-auto mode then the
WFS_EXEE_SIU_PORT_ERROR event is received on any attempt to call
WFS_CMD_SIU_SET_PORTS, etc. with the WFS_SIU_PUBLICAUDIO_AUTO,
WFS_SIU_PRIVATEAUDIO_AUTO, WFS_SIU_PUBLICAUDIO_SEMI_AUTO, and
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO settings. The same event is generated if calls to change the
mode to manual are received when the vendor device does not support manual mode.
• The existing WFS_SIU_VOLUME auxiliary can be used to control the volume setting of any audio
delivered to a connected Privacy Device, as well as the speakers. Independent volume control of the
speakers and Privacy Device is not supported.
• Any ‘beep’ tones generated by the PINPAD, etc will be fed to a connected Privacy Device (vendor
hardware permitting).
13

CWA 16926-10:2020 (E)
2.2 Enhanced Microphone Controller Overview
The Enhanced Microphone Controller is provided to support a system of one or more microphones. Its behavior is
very similar to that of the Enhanced Audio Controller. The Enhanced Microphone Controller device controls how
private and public audio input behave when a headset microphone is inserted into/removed from the
Audio/Microphone Jack, and when the Handset with an integrated microphone is off-hook/on-hook. The device
allows audio input publicly (via a microphone in the fascia) and/or via the consumer’s Privacy Device (vendor
hardware permitting). For improved audio clarity or privacy, the device allows input to only be directed to the
consumers’ Privacy Device. In ‘auto’ and ‘semi-auto’ mode (and where the vendor’s hardware allows), public
transmission of audio can be automatically inhibited when the consumer’s Privacy Device is activated. In ‘auto’
mode (and where the vendor’s hardware allows), public transmission of audio can be automatically re-activated
when the consumer’s Privacy Device is deactivated.
The Enhanced Microphone Controller provides the application with the following information:
• If a Privacy Device is activated (headset connected/handset off the hook).
• Whether the audio input is from the fascia microphone or from the Privacy Device.
• Privacy/public mode: i.e. whether the activation of the Privacy Device automatically switches public
microphone on or off.
The device is managed by the sensors WFS_SIU_HEADSETMICROPHONE, WFS_SIU_HANDSETSENSOR,
and an auxiliary WFS_SIU_ENHANCEDMICROPHONECONTROL.
The WFS_SIU_HEADSETMICROPHONE sensor is used to:
• Provide information on the presence of the Microphone Jack device.
• To report whether a headset/external microphone is currently attached.
• Report state change events when a headset/external microphone is inserted or removed.
Some systems may contain a headset jack that enables both audio input and output via a single jack. In this case, the
WFS_SIU_ENHANCEDAUDIO capability will report WFS_SIU_BIDIRECTIONAL, and when a bi-directional
headset is inserted into the jack, the status for both the WFS_SIU_ENHANCEDAUDIO and
WFS_SIU_HEADSETMICROPHONE ports will change appropriately.
The WFS_SIU_HANDSETSENSOR sensor is the same sensor used for private audio output. However, the
WFS_SIU_HANDSETSENSOR capability flag WFS_SIU_MICROPHONE indicates whether the Handset also
contains an integrated microphone for audio input.
The WFS_SIU_ENHANCEDMICROPHONECONTROL auxiliary is used to control the behavior of the Enhanced
Microphone Controller. It allows the application to:
• Set the mode of the Enhanced Microphone Controller - auto mode, semi-auto mode or manual mode.
• Set the state of the Enhanced Microphone Controller- public or private.
The Enhanced Microphone Controller modes and state transitions are very much like those for the Enhanced Audio
Controller. The Enhanced Microphone Controller can be set to auto, semi-auto or manual modes. State transitions
occur in the same way that they are described in the state diagrams in the previous section, either when changed
manually by the application or when a Privacy Devices is activated/deactivated (depending on the mode). Note that
if the WFS_SIU_FASCIAMICROPHONE indicates WFS_SIU_NOT_AVAILABLE, none of the public modes for
the microphone will be supported. The Enhanced Audio Controller and Enhanced Microphone Controller states are
independent of each other, but can be linked under certain transitions while in auto mode or semi-auto mode. For
example, if a handset that contains microphone is activated, both controllers can transition to the private state. On
the other hand, if a handset that does not contain a microphone is activated, the Enhanced Audio Controller may
transition to the private state while the Enhanced Microphone Controller remains in the public state. Likewise, if a
headset that contains a microphone is plugged into a jack that supports integrated audio input and output, both
controllers can transition to the private state. Note that in this case, both the WFS_SIU_ENHANCEDAUDIO and
WFS_SIU_HEADSETMICROPHONE sensors would change state to WFS_SIU_PRESENT regardless of the
mode.
14

CWA 16926-10:2020 (E)
3. References
1. XFS Application Programming Interface (API)/Service Provider Interface ( SPI), Programmer’s Reference
Revision 3.40
2. PCI Security Standards Council PCI DSS v3.1 Requirements and Security Assessment Procedures
https://www.pcisecuritystandards.org/documents/PCI_DSS_v3-1.pdf
15

CWA 16926-10:2020 (E)
4. Info Commands
4.1 WFS_INF_SIU_STATUS
Description This command reports the full range of information available, including the information that is
provided by the Service Provider.
Input Param None.
Output Param LPWFSSIUSTATUS lpStatus;
typedef struct _wfs_siu_status
{
WORD fwDevice;
WORD fwSensors [WFS_SIU_SENSORS_SIZE];
WORD fwDoors [WFS_SIU_DOORS_SIZE];
WORD fwIndicators [WFS_SIU_INDICATORS_SIZE];
WORD fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
WORD fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
LPSTR lpszExtra;
USHORT usPowerSaveRecoveryTime;
WORD wAntiFraudModule;
DWORD fwGuidLightsEx [WFS_SIU_GUIDLIGHTS_SIZE_EX];
} WFSSIUSTATUS, *LPWFSSIUSTATUS;
fwDevice
Specifies the state of the Sensors and Indicators Unit device as one of the following flags:
Value Meaning
WFS_SIU_DEVONLINE The device is online (i.e. powered on and
operable).
WFS_SIU_DEVOFFLINE The device is offline (e.g. the operator has
taken the device offline by turning a switch).
WFS_SIU_DEVPOWEROFF The device is powered off or physically not
connected.
WFS_SIU_DEVNODEVICE There is no device intended to be there; e.g.
this type of self service machine does not
contain such a device or it is internally not
configured.
WFS_SIU_DEVHWERROR The device is inoperable due to a hardware
error.
WFS_SIU_DEVUSERERROR The device is present but a person is
preventing proper operation.
WFS_SIU_DEVBUSY The device is busy and unable to process an
execute command at this time.
WFS_SIU_DEVFRAUDATTEMPT The device is present but is inoperable
because it has detected a fraud attempt.
WFS_SIU_DEVPOTENTIALFRAUD The device has detected a potential fraud
attempt and is capable of remaining in
service. In this case the application should
make the decision as to whether to take the
device offline.
fwSensors [...]
Specifies the state of the sensors. A number of sensor types are defined below. Vendor specific
sensors are defined starting from the end of the array. The maximum sensor index is
WFS_SIU_SENSORS_MAX.
fwSensors [WFS_SIU_OPERATORSWITCH]
Specifies the state of the Operator Switch(es). This switch is used to tell the terminal if an
operator/supervisor wants to change the state from Run to Operators/Supervisors mode or vice
versa. The Run mode is used for normal consumer operations/transactions. The Maintenance
mode is used when replenishing the terminal. The Supervisor mode is used when operating the
terminal for service and testing. Supervisor mode has higher priority than Maintenance mode.
Specified as one of the following flags:
16

CWA 16926-10:2020 (E)
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_RUN The switch is in Run mode.
WFS_SIU_MAINTENANCE The switch is in Maintenance mode.
WFS_SIU_SUPERVISOR The switch is in Supervisor mode.
fwSensors [WFS_SIU_TAMPER]
Specifies the state of the Tamper Sensor for the terminal. This sensor indicates whether the
terminal has been tampered with (such as a burglar attempt). Specified as one of the following
flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF There is no indication of a tampering
attempt.
WFS_SIU_ON There has been a tampering attempt.
fwSensors [WFS_SIU_INTTAMPER]
Specifies the state of the Internal Tamper Sensor for the internal alarm. This sensor indicates
whether the internal alarm has been tampered with (such as a burglar attempt). Specified as one of
the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF There is no indication of a tampering
attempt.
WFS_SIU_ON There has been a tampering attempt.
fwSensors [WFS_SIU_SEISMIC]
Specifies the state of the Seismic Sensor. This sensor indicates whether the terminal has been
shaken (e.g. burglar attempt or seismic activity). Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF The seismic activity has not been high
enough to trigger the sensor.
WFS_SIU_ON The seismic or other activity has triggered
the sensor.
fwSensors [WFS_SIU_HEAT]
Specifies the state of the Heat Sensor. This sensor is triggered by excessive heat (fire) near the
terminal. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF The heat has not been high enough to trigger
the sensor.
WFS_SIU_ON The heat has been high enough to trigger the
sensor.
fwSensors [WFS_SIU_PROXIMITY]
Specifies the state of the Proximity Sensor. This sensor is triggered by movements around the
terminal. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_PRESENT The sensor is showing that there is someone
present at the terminal.
WFS_SIU_NOT_PRESENT The sensor can not sense any people around
the terminal.
17

CWA 16926-10:2020 (E)
fwSensors [WFS_SIU_AMBLIGHT]
Specifies the state of the Ambient Light Sensor. This sensor indicates the level of ambient light
around the terminal. Interpretation of this value is vendor-specific and therefore it is not
guaranteed to report a consistent actual ambient light level across different vendor hardware.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_VERY_DARK The level of light is: very dark.
WFS_SIU_DARK The level of light is: dark.
WFS_SIU_MEDIUM_LIGHT The level of light is: medium light.
WFS_SIU_LIGHT The level of light is: light.
WFS_SIU_VERY_LIGHT The level of light is: very light.
fwSensors [WFS_SIU_ENHANCEDAUDIO]
Specifies the presence or absence of a consumer’s headphone connected to the Audio Jack.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_PRESENT There is a headset connected.
WFS_SIU_NOT_PRESENT There is no headset connected.
fwSensors [WFS_SIU_BOOT_SWITCH]
Specifies the state of the Boot Switch Sensor. This sensor is triggered whenever the terminal is
about to be rebooted or shutdown due to a delayed effect switch. Specified as one of the following
flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF The sensor has not been triggered.
WFS_SIU_ON The terminal is about to be rebooted or
shutdown.
fwSensors [WFS_SIU_CONSUMER_DISPLAY]
Specifies the state of the Consumer Display. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF The Consumer Display is switched off.
WFS_SIU_ON The Consumer Display is in a good state and
is turned on.
WFS_SIU_DISPLAY_ERROR The Consumer Display is in an error state.
fwSensors [WFS_SIU_OPERATOR_CALL_BUTTON]
Specifies the state of the Operator Call Button as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF The Operator Call Button is released (not
pressed).
WFS_SIU_ON The Operator Call Button is being pressed.
fwSensors [WFS_SIU_HANDSETSENSOR]
Specifies the state of the Handset, which is a device similar to a telephone receiver. Specified as
one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF_THE_HOOK The Handset is off the hook.
WFS_SIU_ON_THE_HOOK The Handset is on the hook.
18

CWA 16926-10:2020 (E)
fwSensors [WFS_SIU_GENERALINPUTPORT]
Specifies the state of the vendor dependent General-Purpose Input Ports as a bitmap. Before
making use of the General-Purpose Input Ports the vendor should examine if the required
functionality is covered in this or other device classes as a vendor independent feature. Each bit of
this value represents one General-Purpose Input Port and is specified as one of the following
binary values:
Value Meaning
0 The General-Purpose Input Port is turned
off.
1 The General-Purpose Input Port is turned on.
The following flags can be used to reference each General-Purpose Input Port.
Value Meaning
WFS_SIU_GPP1 General-Purpose Input Port 1.
WFS_SIU_GPP2 General-Purpose Input Port 2.
…
WFS_SIU_GPP16 General-Purpose Input Port 16.
fwSensors [WFS_SIU_HEADSETMICROPHONE]
Specifies the presence or absence of a consumer’s headset microphone connected to the
Microphone Jack. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_PRESENT There is a headset microphone connected.
WFS_SIU_NOT_PRESENT There is no headset microphone connected.
fwSensors [WFS_SIU_FASCIAMICROPHONE]
Specifies the state of the Fascia Microphone as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_OFF The Fascia Microphone is turned off.
WFS_SIU_ON The Fascia Microphone is turned on.
fwDoors [...]
Specifies the state of the doors. A number of door types are defined below. Vendor specific doors
are defined starting from the end of the array. The maximum door index is
WFS_SIU_DOORS_MAX.
fwDoors [WFS_SIU_CABINET]
Specifies a summary of the states of the Cabinet Doors. A more detailed status may be available
through the door specific state for e.g. WFS_SIU_CABINET_REAR. Cabinet Doors are doors
that open up for consumables, and hardware that does not have to be in a secure place. Specified
as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_CLOSED All Cabinet Doors are closed.
WFS_SIU_OPEN At least one of the Cabinet Doors is open.
WFS_SIU_LOCKED All Cabinet Doors are closed and locked.
WFS_SIU_BOLTED All Cabinet Doors are closed, locked and
bolted.
WFS_SIU_TAMPERED At least one of the Cabinet Doors has
potentially been tampered with.
fwDoors [WFS_SIU_SAFE]
Specifies the state of the Safe Doors. Safe Doors are doors that open up for secure hardware, such
as the note dispenser, the security device, etc. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_CLOSED The Safe Doors are closed.
WFS_SIU_OPEN At least one of the Safe Doors is open.
19

CWA 16926-10:2020 (E)
WFS_SIU_LOCKED The Safe Doors are closed and locked.
WFS_SIU_BOLTED The Safe Doors are closed, locked and
bolted.
WFS_SIU_TAMPERED At least one of the Safe Doors has potentially
been tampered with.
fwDoors [WFS_SIU_VANDALSHIELD]
Specifies the state of the Vandal Shield. The Vandal Shield is a door that open up for consumer
access to the terminal. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_CLOSED The Vandal Shield is closed.
WFS_SIU_OPEN The Vandal Shield is open.
WFS_SIU_LOCKED The Vandal Shield is closed and locked.
WFS_SIU_SERVICE The Vandal Shield is in service position.
WFS_SIU_KEYBOARD The Vandal Shield position permits access to
the keyboard.
WFS_SIU_AJAR The Vandal Shield is ajar.
WFS_SIU_JAMMED The Vandal Shield is jammed.
WFS_SIU_TAMPERED The Vandal Shield has potentially been
tampered with
fwDoors [WFS_SIU_CABINET_FRONT]
Specifies the overall state of the Front Cabinet Doors (the overall status for all cabinet doors is
available through the status for WFS_SIU_CABINET). The front is defined as the side facing the
customer/consumer. Cabinet Doors are doors that open up for consumables, and hardware that
does not have to be in a secure place. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_CLOSED All Front Cabinet Doors are closed.
WFS_SIU_OPEN At least one of the Front Cabinet Doors is
open.
WFS_SIU_LOCKED All Front Cabinet Doors are closed and
locked.
WFS_SIU_BOLTED All Front Cabinet Doors are closed, locked
and bolted.
WFS_SIU_TAMPERED At least one of the Front Cabinet Doors has
potentially been tampered with.
fwDoors [WFS_SIU_CABINET_REAR]
Specifies the overall state of the Rear Cabinet Doors (the overall status for all cabinet doors is
available through the status for WFS_SIU_CABINET). The rear is defined as the side opposite
the side facing the customer/consumer. Cabinet Doors are doors that open up for consumables,
and hardware that does not have to be in a secure place. Specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_CLOSED All Rear Cabinet Doors are closed.
WFS_SIU_OPEN At least one of the Rear Cabinet Doors is
open.
WFS_SIU_LOCKED All Rear Cabinet Doors are closed and
locked.
WFS_SIU_BOLTED All Rear Cabinet Doors are closed, locked
and bolted.
WFS_SIU_TAMPERED At least one of the Rear Cabinet Doors has
potentially been tampered with.
fwDoors [WFS_SIU_CABINET_LEFT]
Specifies the overall state of the Left Cabinet Doors (the overall status for all cabinet doors is
available through the status for WFS_SIU_CABINET). The left is defined as the side to the left as
seen by the customer/consumer. Cabinet Doors are doors that open up for consumables, and
hardware that does not have to be in a secure place. Specified as one of the following flags:
20

CWA 16926-10:2020 (E)

| Value                  | Meaning                                    |     |
| ---------------------- | ------------------------------------------ | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.               |     |
| WFS_SIU_CLOSED         | All Left Cabinet Doors are closed.         |     |
| WFS_SIU_OPEN           | At least one of the Left Cabinet Doors is  |     |
open.
| WFS_SIU_LOCKED  | All Left Cabinet Doors are closed and  |     |
| --------------- | -------------------------------------- | --- |
locked.
| WFS_SIU_BOLTED  | All Left Cabinet Doors are closed, locked  |     |
| --------------- | ------------------------------------------ | --- |
and bolted.
WFS_SIU_TAMPERED  At least one of the Left Cabinet Doors has
potentially been tampered with
fwDoors [WFS_SIU_CABINET_RIGHT]
Specifies the overall state of the Right Cabinet Doors (the overall status for all cabinet doors is
available through the status for WFS_SIU_CABINET). The right is defined as the side to the right
as seen by the customer/consumer. Cabinet Doors are doors that open up for consumables, and
hardware that does not have to be in a secure place. Specified as one of the following flags:
| Value                  | Meaning                                     |     |
| ---------------------- | ------------------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.                |     |
| WFS_SIU_CLOSED         | All Right Cabinet Doors are closed.         |     |
| WFS_SIU_OPEN           | At least one of the Right Cabinet Doors is  |     |
open.
| WFS_SIU_LOCKED  | All Right Cabinet Doors are closed and  |     |
| --------------- | --------------------------------------- | --- |
locked.
| WFS_SIU_BOLTED  | All Right Cabinet Doors are closed, locked  |     |
| --------------- | ------------------------------------------- | --- |
and bolted.
WFS_SIU_TAMPERED  At least one of the Right Cabinet Doors has
potentially been tampered with
fwIndicators [...]
Specifies the state of the indicators. A number of indicator types are defined below. Vendor
specific indicators are defined starting from the end of the array. The maximum indicator index is
WFS_SIU_INDICATORS_MAX.
fwIndicators [WFS_SIU_OPENCLOSE]
Specifies the state of the Open/Closed Indicator as one of the following flags:
| Value                  | Meaning                                 |     |
| ---------------------- | --------------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.            |     |
| WFS_SIU_CLOSED         | The terminal is closed for a consumer.  |     |
| WFS_SIU_OPEN           | The terminal is open to be used by a    |     |
consumer.
fwIndicators [WFS_SIU_FASCIALIGHT]
Specifies the state of the Fascia Light as one of the following flags:
| Value                  | Meaning                          |     |
| ---------------------- | -------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.     |     |
| WFS_SIU_OFF            | The Fascia Light is turned off.  |     |
| WFS_SIU_ON             | The Fascia Light is turned on.   |     |
fwIndicators [WFS_SIU_AUDIO]
Specifies the state of the Audio Indicator as one of the following flags of type A and B, or as
WFS_SIU_CONTINUOUS in combination with one of the flags of type B: Interpretation of this
value is vendor-specific and therefore it is not possible to guarantee a consistent actual sound
pattern across different vendor hardware.
| Value                  | Meaning                             | Type  |
| ---------------------- | ----------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.        | A     |
| WFS_SIU_OFF            | The Audio Indicator is turned off.  | A     |
| WFS_SIU_KEYPRESS       | The Audio Indicator sounds a key    | B     |
click signal.
| WFS_SIU_EXCLAMATION  | The Audio Indicator sounds an   | B   |
| -------------------- | ------------------------------- | --- |
exclamation signal.
21

CWA 16926-10:2020 (E)
| WFS_SIU_WARNING  | The Audio Indicator sounds a   | B   |
| ---------------- | ------------------------------ | --- |
warning signal.
| WFS_SIU_ERROR  | The Audio Indicator sounds an   | B   |
| -------------- | ------------------------------- | --- |
error signal.
| WFS_SIU_CRITICAL  | The Audio Indicator sounds a   | B   |
| ----------------- | ------------------------------ | --- |
critical signal.
| WFS_SIU_CONTINUOUS  | The Audio Indicator sound is   | C   |
| ------------------- | ------------------------------ | --- |
turned on continuously.
fwIndicators [WFS_SIU_HEATING]
Specifies the state of the Internal Heating as one of the following flags:
| Value                  | Meaning                              |     |
| ---------------------- | ------------------------------------ | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.         |     |
| WFS_SIU_OFF            | The Internal Heating is turned off.  |     |
| WFS_SIU_ON             | The Internal Heating is turned on.   |     |
fwIndicators [WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT]
Specifies the state of the Consumer Display Backlight as one of the following:
| Value                  | Meaning                                   |     |
| ---------------------- | ----------------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.              |     |
| WFS_SIU_OFF            | The Consumer Display Backlight is turned  |     |
off.
| WFS_SIU_ON  | The Consumer Display Backlight is turned  |     |
| ----------- | ----------------------------------------- | --- |
on.
fwIndicators [WFS_SIU_SIGNAGEDISPLAY]
Specifies the state of the Signage Display. The Signage Display is a lighted banner or marquee
that can be used to display information or an advertisement. Any dynamic data displayed must be
loaded by a means external to the Service Provider. Specified as one of the following flags:
| Value                  | Meaning                             |     |
| ---------------------- | ----------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.        |     |
| WFS_SIU_OFF            | The Signage Display is turned off.  |     |
| WFS_SIU_ON             | The Signage Display is turned on.   |     |
fwIndicators [WFS_SIU_TRANSINDICATOR]
Specifies the state of the Transaction Indicators as a bitmap. Each bit of this value represents one
Transaction Indicator and is specified as one of the following binary values:
| Value  | Meaning                                   |     |
| ------ | ----------------------------------------- | --- |
| 0      | The Transaction Indicator is turned off.  |     |
| 1      | The Transaction Indicator is turned on.   |     |
The following flags can be used to reference each Transaction Indicator.
| Value          | Meaning                   |     |
| -------------- | ------------------------- | --- |
| WFS_SIU_LAMP1  | Transaction Indicator 1.  |     |
| WFS_SIU_LAMP2  | Transaction Indicator 2.  |     |
…
| WFS_SIU_LAMP16  | Transaction Indicator 16.  |     |
| --------------- | -------------------------- | --- |
fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
Specifies the state of the vendor dependent General-Purpose Output Ports as a bitmap. Before
making use of the General-Purpose Output Ports the vendor should examine if the required
functionality is covered in this or other device classes as a vendor independent feature. Each bit of
this value represents one General-Purpose Output Port and is specified as one of the following
binary values:
| Value  | Meaning                                    |     |
| ------ | ------------------------------------------ | --- |
| 0      | The General-Purpose Output Port is turned  |     |
off.
| 1   | The General-Purpose Output Port is turned  |     |
| --- | ------------------------------------------ | --- |
on.
22

CWA 16926-10:2020 (E)

The following flags can be used to reference each General-Purpose Output Port.
| Value         | Meaning                         |     |
| ------------- | ------------------------------- | --- |
| WFS_SIU_GPP1  | General-Purpose Output Port 1.  |     |
| WFS_SIU_GPP2  | General-Purpose Output Port 2.  |     |
…
| WFS_SIU_GPP16  | General-Purpose Output Port 16.  |     |
| -------------- | -------------------------------- | --- |
fwAuxiliaries [...]
Specifies the state of the auxiliary indicators. A number of auxiliary indicator types are defined
below. Vendor specific auxiliaries are defined starting from the end of the array. The maximum
auxiliary index is WFS_SIU_AUXILIARIES_MAX.
fwAuxiliaries [WFS_SIU_VOLUME]
Specifies the value of the Volume Control. The value of Volume Control is defined in an interval
from 1 to 1000 where 1 is the lowest volume level and 1000 is the highest volume level. The
interval is defined in logarithmic steps, e.g. a volume control on a radio. Note: The Volume
Control field is handled as unsigned short. Interpretation of this value is vendor-specific and
therefore it is not possible to guarantee a consistent actual volume level across different vendor
hardware.
| Value                  | Meaning                                        |     |
| ---------------------- | ---------------------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.                   |     |
| 1, ..., 1000           | The volume level. This field is handled as an  |     |
unsigned short.
fwAuxiliaries [WFS_SIU_UPS]
Specifies the state of the Uninterruptible Power Supply device as WFS_SIU_NOT_AVAILABLE
or as a combination of the following flags of type B:
| Value                  | Meaning                         | Type  |
| ---------------------- | ------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.    | A     |
| WFS_SIU_AVAILABLE      | The UPS is available.           | B     |
| WFS_SIU_LOW            | The charge level of the UPS is  | B     |
low.
| WFS_SIU_ENGAGED   | The UPS is engaged.              | B   |
| ----------------- | -------------------------------- | --- |
| WFS_SIU_POWERING  | The UPS is powering the system.  | B   |
The main power supply is off.
| WFS_SIU_RECOVERED  | The UPS was engaged when the  | B   |
| ------------------ | ----------------------------- | --- |
main power went off.
fwAuxiliaries[WFS_SIU_REMOTE_STATUS_MONITOR]
Specifies the state of the Remote Status Monitor device (which uses LEDs for displaying the status
of the system). Possible states are WFS_SIU_NOT_AVAILABLE or a combination of one of
each flag of type B, C and D:
| Value                  | Meaning                       | Type  |
| ---------------------- | ----------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.  | A     |
| WFS_SIU_GREEN_LED_ON   | The green LED is on.          | B     |
| WFS_SIU_GREEN_LED_OFF  | The green LED is off.         | B     |
| WFS_SIU_AMBER_LED_ON   | The amber LED is on.          | C     |
| WFS_SIU_AMBER_LED_OFF  | The amber LED is off.         | C     |
| WFS_SIU_RED_LED_ON     | The red LED is on.            | D     |
| WFS_SIU_RED_LED_OFF    | The red LED is off.           | D     |
fwAuxiliaries[WFS_SIU_AUDIBLE_ALARM]
Species the state of the Audible Alarm device as one of the following flags:
| Value                  | Meaning                       |     |
| ---------------------- | ----------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | The status is not available.  |     |
| WFS_SIU_OFF            | The Alarm is turned off.      |     |
| WFS_SIU_ON             | The Alarm is turned on.       |     |
23

CWA 16926-10:2020 (E)
fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
Specifies the state of the Enhanced Audio Controller. The Enhanced Audio Controller controls
how private and public audio are broadcast when the headset is inserted into/removed from the
audio jack and when the handset is off-hook/on-hook. In the following, Privacy Device is used to
refer to either the headset or handset. The Enhanced Audio Controller state is specified as one of
the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
WFS_SIU_PUBLICAUDIO_MANUAL The Enhanced Audio Controller is in manual
mode and is in the public state (i.e. audio
will be played through speakers). Activating
a Privacy Device (headset connected/handset
off-hook) will have no impact, i.e. Output
will remain through the speakers & no audio
will be directed to the Privacy Device.
WFS_SIU_PUBLICAUDIO_AUTO The Enhanced Audio Controller is in auto
mode and is in the public state (i.e. audio
will be played through speakers). When a
Privacy Device is activated, the device will
go to the private state.
WFS_SIU_PUBLICAUDIO_SEMI_AUTO The Enhanced Audio Controller is in semi-
auto mode and is in the public state (i.e.
audio will be played through speakers).
When a Privacy Device is activated, the
device will go to the private state.
WFS_SIU_PRIVATEAUDIO_MANUAL The Enhanced Audio Controller is in manual
mode and is in the private state (i.e. audio
will be played only through a connected
Privacy Device). In private mode, no audio is
transmitted through the speakers.
WFS_SIU_PRIVATEAUDIO_AUTO The Enhanced Audio Controller is in auto
mode and is in the private state (i.e. audio
will be played only through a connected
Privacy Device). In private mode, no audio is
transmitted through the speakers. When a
Privacy Device is deactivated (headset
disconnected/handset on-hook), the device
will go to the public state. Where there is
more than one Privacy Device, the device
will go to the public state only when all
Privacy Devices have been deactivated.
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO The Enhanced Audio Controller is in semi-
auto mode and is in the private state (i.e.
audio will be played only through a
connected Privacy Device). In private mode,
no audio is transmitted through the speakers.
When a Privacy Device is deactivated, the
device will remain in the private state.
fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
Specifies the state of the Enhanced Microphone Controller. The Enhanced Microphone Controller
controls how private and public audio input are transmitted when the headset is inserted
into/removed from the audio jack and when the handset is off-hook/on-hook. In the following,
Privacy Device is used to refer to either the headset or handset. The Enhanced Microphone
Controller state is specified as one of the following flags:
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
24

CWA 16926-10:2020 (E)
WFS_SIU_PUBLICAUDIO_MANUAL The Enhanced Microphone Controller is in
manual mode and is in the public state (i.e.
the microphone in the fascia is active).
Activating a Privacy Device (headset
connected/handset off-hook) will have no
impact, i.e. input will remain through the
fascia microphone and any microphone
associated with the Privacy Device will not
be active.
WFS_SIU_PUBLICAUDIO_AUTO The Enhanced Microphone Controller is in
auto mode and is in the public state (i.e. the
microphone in the fascia is active). When a
Privacy Device with a microphone is
activated, the device will go to the private
state.
WFS_SIU_PUBLICAUDIO_SEMI_AUTO The Enhanced Microphone Controller is in
semi-auto mode and is in the public state (i.e.
the microphone in the fascia is active). When
a Privacy Device with a microphone is
activated, the device will go to the private
state.
WFS_SIU_PRIVATEAUDIO_MANUAL The Enhanced Microphone Controller is in
manual mode and is in the private state (i.e.
audio input will be via a microphone in the
Privacy Device). In private mode, no audio
input is transmitted through the fascia
microphone.
WFS_SIU_PRIVATEAUDIO_AUTO The Enhanced Microphone Controller is in
auto mode and is in the private state (i.e.
audio input will be via a microphone in the
Privacy Device). In private mode, no audio
input is transmitted through the fascia
microphone. When a Privacy Device with a
microphone is deactivated (headset
disconnected/handset on-hook), the device
will go to the public state. Where there is
more than one Privacy Device with a
microphone, the device will go to the public
state only when all such Privacy Devices
have been deactivated.
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO The Enhanced Microphone Controller is in
semi-auto mode and is in the private state
(i.e. audio input will be via a microphone in
the Privacy Device). In private mode, no
audio is transmitted through the fascia
microphone. When a Privacy Device with a
microphone is deactivated, the device will
remain in the private state.
fwAuxiliaries [WFS_SIU_MICROPHONEVOLUME]
Specifies the value of the Microphone Volume Control. The value of Microphone Volume Control
is defined in an interval from 1 to 1000 where 1 is the lowest volume level and 1000 is the highest
volume level. The interval is defined in logarithmic steps, e.g. a volume control on a radio. Note:
The Microphone Volume Control field is handled as unsigned short. Interpretation of this value is
vendor-specific and therefore it is not possible to guarantee a consistent actual volume level across
different vendor hardware.
Value Meaning
WFS_SIU_NOT_AVAILABLE The status is not available.
1, ..., 1000 The microphone volume level. This field is
handled as an unsigned short.
25

CWA 16926-10:2020 (E)
fwGuidLights [...]
Specifies the state of the guidance lights. See WFS_INF_SIU_CAPABILITIES fwGuidLights for
supported guidance lights and states. The state of each guidance light can be
WFS_SIU_NOT_AVAILABLE, WFS_SIU_OFF or a combination of one flash rate (type B) and
optionally, one color (type C). Vendor specific guidance lights are defined starting from the end of
the array.
Note the maximum guidance light identifier is WFS_SIU_GUIDLIGHTS_MAX.and direction
flags (Type D) cannot be reported from this field as the field type is WORD. It is recommended
that applications use the fwGuidLightsEx.lpszExtra pointer to a list of vendor-specific, or any
other extended, information. The information is returned as a series of “key=value” strings so that
it is easily extensible by Service Providers. Each string is null-terminated, with the final string
terminating with two null characters. An empty list may be indicated by either a NULL pointer or
a pointer to two consecutive null characters.
usPowerSaveRecoveryTime
Specifies the actual number of seconds required by the device to resume its normal operational
state from the current power saving mode. This value is zero if either the power saving mode has
not been activated or no power save control is supported.
wAntiFraudModule
Specifies the state of the anti-fraud module as one of the following values:
Value Meaning
WFS_SIU_AFMNOTSUPP No anti-fraud module is available.
WFS_SIU_AFMOK Anti-fraud module is in a good state and no
foreign device is detected.
WFS_SIU_AFMINOP Anti-fraud module is inoperable.
WFS_SIU_AFMDEVICEDETECTED Anti-fraud module detected the presence of a
foreign device.
WFS_SIU_AFMUNKNOWN The state of the anti-fraud module cannot be
determined.
fwGuidLightsEx [...]
Specifies the state of the guidance lights. See WFS_INF_SIU_CAPABILITIES fwGuidLightsEx
for supported guidance lights and states. The state of each guidance light can be
WFS_SIU_NOT_AVAILABLE, WFS_SIU_OFF or a combination of one flash rate (type B) and
optionally, one color (type C) and one direction (type D) flags. Vendor specific guidance lights are
defined starting from the end of the array. The maximum guidance light index is
WFS_SIU_GUIDLIGHTS_MAX_EX.
Error Codes Only the generic error codes defined in [Ref. 1] can be generated by this command.
Comments Applications which require or expect specific information to be present in the lpszExtra parameter
may not be device or vendor-independent.
In the case where the value to be reported in a status field cannot be determined because of a
communications failure, then the status for that field will be reported as
WFS_SIU_NOT_AVAILABLE. fwDevice will report WFS_SIU_DEVONLINE so long as at
least one status field can be reported.
26

CWA 16926-10:2020 (E)

4.2  WFS_INF_SIU_CAPABILITIES
Description  This command is used to retrieve the capabilities of the Sensors and Indicators Unit.
Input Param  None.
Output Param  LPWFSSIUCAPS lpCaps;
typedef struct _wfs_siu_caps
  {
  WORD     wClass;
  WORD     fwType;
  WORD     fwSensors [WFS_SIU_SENSORS_SIZE];
  WORD     fwDoors [WFS_SIU_DOORS_SIZE];
  WORD     fwIndicators [WFS_SIU_INDICATORS_SIZE];
  WORD     fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
  WORD     fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
  LPSTR     lpszExtra;
  BOOL     bPowerSaveControl;
  WORD     fwAutoStartupMode;
  BOOL     bAntiFraudModule;
  LPDWORD    lpdwSynchronizableCommands;
  LPWFSSIUTRMINFO lpTerminalInformation;
  DWORD     fwGuidLightsEx [WFS_SIU_GUIDLIGHTS_SIZE_EX];
  } WFSSIUCAPS, *LPWFSSIUCAPS;
wClass
Specifies the logical service class as WFS_SERVICE_CLASS_SIU.
fwType
Specifies the type of sensors and indicators supported by this device as a combination of the
following flags:
| Value               | Meaning                             |     |
| ------------------- | ----------------------------------- | --- |
| WFS_SIU_SENSORS     | The device supports input sensors.  |     |
| WFS_SIU_DOORS       | The device support door sensors.    |     |
| WFS_SIU_INDICATORS  | The device supports indicators.     |     |
WFS_SIU_AUXILIARIES  The device supports auxiliary indicators.
| WFS_SIU_GUIDLIGHTS  | The device supports guidance lights.  |     |
| ------------------- | ------------------------------------- | --- |
fwSensors [...]
Specifies which sensors are available, and if so, which states they can take. A number of sensor
types are defined below. Vendor specific sensors are defined starting from the end of the array.
The maximum sensor index is WFS_SIU_SENSORS_MAX.
fwSensors [WFS_SIU_OPERATORSWITCH]
Specifies whether the Operator Switch is available, and if so, which states it can take. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                       | Type  |
| ---------------------- | ----------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Operator Switch   | A     |
available.
| WFS_SIU_RUN  | The switch can be set in Run   | B   |
| ------------ | ------------------------------ | --- |
mode.
| WFS_SIU_MAINTENANCE  | The switch can be set in   | B   |
| -------------------- | -------------------------- | --- |
Maintenance mode.
| WFS_SIU_SUPERVISOR  | The switch can be set in   | B   |
| ------------------- | -------------------------- | --- |
Supervisors mode.
fwSensors [WFS_SIU_TAMPER]
Specifies whether the Tamper Sensor for the terminal is available. Specified as one of the
following flags:
| Value                  | Meaning                               |     |
| ---------------------- | ------------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | There is no Tamper Sensor available.  |     |
| WFS_SIU_AVAILABLE      | The Tamper Sensor is available.       |     |
27

CWA 16926-10:2020 (E)
fwSensors [WFS_SIU_INTTAMPER]
Specifies whether the Internal Tamper Sensor for internal alarm is available. Specified as one of
the following flags:
| Value                  | Meaning                             |     |
| ---------------------- | ----------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | There is no Internal Tamper Sensor  |     |
available.
| WFS_SIU_AVAILABLE  | The Internal Tamper Sensor is available.  |     |
| ------------------ | ----------------------------------------- | --- |
fwSensors [WFS_SIU_SEISMIC]
Specifies whether the Seismic Sensor is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Seismic Sensor available.
| WFS_SIU_AVAILABLE  | The Seismic Sensor is available.  |     |
| ------------------ | --------------------------------- | --- |
fwSensors [WFS_SIU_HEAT]
Specifies whether the Heat Sensor is available. Specified as one of the following flags:
| Value                  | Meaning                             |     |
| ---------------------- | ----------------------------------- | --- |
| WFS_SIU_NOT_AVAILABLE  | There is no Heat Sensor available.  |     |
| WFS_SIU_AVAILABLE      | The Heat Sensor is available.       |     |
fwSensors [WFS_SIU_PROXIMITY]
Specifies whether the Proximity Sensor is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Proximity Sensor available.
| WFS_SIU_AVAILABLE  | The Proximity Sensor is available.  |     |
| ------------------ | ----------------------------------- | --- |
fwSensors [WFS_SIU_AMBLIGHT]
Specifies whether the Ambient Light Sensor is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Ambient Light Sensor available.
| WFS_SIU_AVAILABLE  | The Ambient Light Sensor is available.  |     |
| ------------------ | --------------------------------------- | --- |
fwSensors [WFS_SIU_ENHANCEDAUDIO]
Specifies whether the Audio Jack is present, and if so, which modes it supports. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                            | Type  |
| ---------------------- | ---------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Audio Jack available.  | A     |
| WFS_SIU_MANUAL         | The Audio Jack is available and    | B     |
supports manual mode.
| WFS_SIU_AUTO  | The Audio Jack is available and   | B   |
| ------------- | --------------------------------- | --- |
supports auto mode.
| WFS_SIU_SEMI_AUTO  | The Audio Jack is available and   | B   |
| ------------------ | --------------------------------- | --- |
supports semi-auto mode.
| WFS_SIU_BIDIRECTIONAL  | The Audio Jack is available and   | B   |
| ---------------------- | --------------------------------- | --- |
can support headphones that have
|     | an integrated microphone via a  |     |
| --- | ------------------------------- | --- |
|     | single jack.                    |     |
fwSensors [WFS_SIU_BOOT_SWITCH]
Specifies whether the Boot Switch is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Boot Switch Sensor available.
| WFS_SIU_AVAILABLE  | The Boot Switch Sensor is available.  |     |
| ------------------ | ------------------------------------- | --- |
fwSensors [WFS_SIU_CONSUMER_DISPLAY]
Specifies whether the Consumer Display Sensor is available. Specified as one of the following
flags:
28

CWA 16926-10:2020 (E)

| Value                  | Meaning                              |     |
| ---------------------- | ------------------------------------ | --- |
| WFS_SIU_NOT_AVAILABLE  | There is no Consumer Display Sensor  |     |
available.
WFS_SIU_AVAILABLE  The Consumer Display Sensor is available.
fwSensors [WFS_SIU_OPERATOR_CALL_BUTTON]
Specifies whether the Operator Call Button is available. The Operator Call Button does not
actually call the operator but just sends a signal to the application. Specified as one of the
following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Operator Call Button available.
| WFS_SIU_AVAILABLE  | The Operator Call Button is available.  |     |
| ------------------ | --------------------------------------- | --- |
fwSensors [WFS_SIU_HANDSETSENSOR]
Specifies whether the Handset is present, and if so, which modes it supports. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                           | Type  |
| ---------------------- | --------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Handset available.    | A     |
| WFS_SIU_MANUAL         | The Handset is available and it   | B     |
supports manual mode.
| WFS_SIU_AUTO  | The Handset is available and it   | B   |
| ------------- | --------------------------------- | --- |
supports auto mode.
| WFS_SIU_SEMI_AUTO  | The Handset is available and it   | B   |
| ------------------ | --------------------------------- | --- |
supports semi-auto mode.
| WFS_SIU_MICROPHONE  | The Handset is available and  | B   |
| ------------------- | ----------------------------- | --- |
|                     | contains an embedded          |     |
|                     | microphone for audio input.   |     |
fwSensors [WFS_SIU_GENERALINPUTPORT]
Specifies whether the vendor dependent General-Purpose Input Ports are available. Before making
use of the General-Purpose Input Ports the vendor should examine if the required functionality is
covered in this or other device classes as a vendor independent feature. This value is a bitmap and
each bit of this value represents one General-Purpose Input Port. Each bit is specified as one of the
following binary values:
| Value  | Meaning                                |     |
| ------ | -------------------------------------- | --- |
| 0      | The General-Purpose Input Port is not  |     |
available.
| 1   | The General-Purpose Input Port is available.  |     |
| --- | --------------------------------------------- | --- |
The following flags can be used to reference each General-Purpose Input Port.
| Value         | Meaning                        |     |
| ------------- | ------------------------------ | --- |
| WFS_SIU_GPP1  | General-Purpose Input Port 1.  |     |
| WFS_SIU_GPP2  | General-Purpose Input Port 2.  |     |
…
| WFS_SIU_GPP16  | General-Purpose Input Port 16.  |     |
| -------------- | ------------------------------- | --- |
fwSensors [WFS_SIU_HEADSETMICROPHONE]
Specifies whether the Microphone Jack is present, and if so, which modes it supports. If the
fwSensors [WFS_SIU_ENHANCEDAUDIO] capability indicates the presence of a bi-directional
Audio Jack then both sensors reference the same physical jack. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                           | Type  |
| ---------------------- | --------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Microphone Jack       | A     |
|                        | available.                        |       |
| WFS_SIU_MANUAL         | The Microphone Jack is available  | B     |
and supports manual mode.
| WFS_SIU_AUTO  | The Microphone Jack is available  | B   |
| ------------- | --------------------------------- | --- |
and supports auto mode.
| WFS_SIU_SEMI_AUTO  | The Microphone Jack is available  | B   |
| ------------------ | --------------------------------- | --- |
and supports semi-auto mode.
29

CWA 16926-10:2020 (E)
fwSensors [WFS_SIU_FASCIAMICROPHONE]
Specifies whether a Fascia Microphone (for public audio input) is present. Specified as one of the
following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Fascia Microphone available.
| WFS_SIU_AVAILABLE  | The Fascia Microphone is available.  |     |
| ------------------ | ------------------------------------ | --- |
fwDoors [...]
Specifies which doors are available, and if so, which states they can take. A number of door types
are defined below. Vendor specific doors are defined starting from the end of the array. The
maximum door index is WFS_SIU_DOORS_MAX.
fwDoors [WFS_SIU_CABINET]
Specifies whether at least one Cabinet Doors is available, and if so, which states they can take.
Specified as WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                    | Type  |
| ---------------------- | -------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Cabinet Door   | A     |
available.
| WFS_SIU_CLOSED  | At least one of the Cabinet Doors   | B   |
| --------------- | ----------------------------------- | --- |
can detect a closed state.
| WFS_SIU_OPEN  | At least one of the Cabinet Doors   | B   |
| ------------- | ----------------------------------- | --- |
can detect an open state.
| WFS_SIU_LOCKED  | At least one of the Cabinet Doors   | B   |
| --------------- | ----------------------------------- | --- |
can be locked.
| WFS_SIU_BOLTED  | At least one of the Cabinet Doors   | B   |
| --------------- | ----------------------------------- | --- |
can be bolted.
| WFS_SIU_TAMPERED  | At least one of the Cabinet Doors   | B   |
| ----------------- | ----------------------------------- | --- |
can detect potential tampering.
fwDoors [WFS_SIU_SAFE]
Specifies whether the Safe Doors are available, and if so, which states they can take. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                           | Type  |
| ---------------------- | --------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Safe Door available.  | A     |
| WFS_SIU_CLOSED         | The Safe Doors can be closed.     | B     |
| WFS_SIU_OPEN           | The Safe Doors can be open.       | B     |
| WFS_SIU_LOCKED         | The Safe Doors can be locked.     | B     |
| WFS_SIU_BOLTED         | The Safe Doors can be bolted.     | B     |
| WFS_SIU_TAMPERED       | The Safe Doors can detect         | B     |
potential tampering.
fwDoors [WFS_SIU_VANDALSHIELD]
Specifies whether the Vandal Shield is available, and if so, which states it can take. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                     | Type  |
| ---------------------- | --------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Vandal Shield   | A     |
available.
| WFS_SIU_CLOSED   | The Vandal Shield can be closed.  | B   |
| ---------------- | --------------------------------- | --- |
| WFS_SIU_OPEN     | The Vandal Shield can be open.    | B   |
| WFS_SIU_LOCKED   | The Vandal Shield can be locked.  | B   |
| WFS_SIU_SERVICE  | The Vandal Shield can be in       | B   |
service position.
| WFS_SIU_KEYBOARD  | The Vandal Shield can be in   | B   |
| ----------------- | ----------------------------- | --- |
position that permits access to
the keyboard.
| WFS_SIU_TAMPERED  | The Vandal Shield can detect   | B   |
| ----------------- | ------------------------------ | --- |
potential tampering.
30

CWA 16926-10:2020 (E)

fwDoors [WFS_SIU_CABINET_FRONT]
Specifies whether at least one Front Cabinet Door is available, and if so, which states they can
take (the overall capability for all cabinet doors is available through the capability for
WFS_SIU_CABINET). The front is defined as the side facing the customer/consumer. Specified
as WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                          | Type  |
| ---------------------- | -------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Front Cabinet Door   | A     |
available.
| WFS_SIU_CLOSED  | At least one of the Front Cabinet   | B   |
| --------------- | ----------------------------------- | --- |
Doors can detect a closed state.
| WFS_SIU_OPEN  | At least one of the Front Cabinet   | B   |
| ------------- | ----------------------------------- | --- |
Doors can detect an open state.
| WFS_SIU_LOCKED  | At least one of the Front Cabinet   | B   |
| --------------- | ----------------------------------- | --- |
Doors can be locked.
| WFS_SIU_BOLTED  | At least one of the Front Cabinet   | B   |
| --------------- | ----------------------------------- | --- |
Doors can be bolted.
| WFS_SIU_TAMPERED  | At least one of the Front Cabinet  | B   |
| ----------------- | ---------------------------------- | --- |
Doors can detect potential tampering.
fwDoors [WFS_SIU_CABINET_REAR]
Specifies whether at least one Rear Cabinet Door is available, and if so, which states they can take
(the overall capability for all cabinet doors is available through the capability for
WFS_SIU_CABINET). The rear is defined as the side opposite the side facing the
customer/consumer. Specified as WFS_SIU_NOT_AVAILABLE or as a combination of the
following flags of type B:
| Value                  | Meaning                         | Type  |
| ---------------------- | ------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Rear Cabinet Door   | A     |
available.
| WFS_SIU_CLOSED  | At least one of the Rear Cabinet   | B   |
| --------------- | ---------------------------------- | --- |
Doors can detect a closed state.
| WFS_SIU_OPEN  | At least one of the Rear Cabinet   | B   |
| ------------- | ---------------------------------- | --- |
Doors can detect an open state.
| WFS_SIU_LOCKED  | At least one of the Rear Cabinet   | B   |
| --------------- | ---------------------------------- | --- |
Doors can be locked.
| WFS_SIU_BOLTED  | At least one of the Rear Cabinet   | B   |
| --------------- | ---------------------------------- | --- |
Doors can be bolted.
| WFS_SIU_TAMPERED  | At least one of the Rear Cabinet  | B   |
| ----------------- | --------------------------------- | --- |
Doors can detect potential tampering.
fwDoors [WFS_SIU_CABINET_LEFT]
Specifies whether at least one Left Cabinet Door is available, and if so, which states they can take
(the overall capability for all cabinet doors is available through the capability for
WFS_SIU_CABINET). The left is defined as the side to the left as seen by the
customer/consumer. Specified as WFS_SIU_NOT_AVAILABLE or as a combination of the
following flags of type B:
| Value                  | Meaning                         | Type  |
| ---------------------- | ------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Left Cabinet Door   | A     |
available.
| WFS_SIU_CLOSED  | At least one of the Left Cabinet  | B   |
| --------------- | --------------------------------- | --- |
Doors can detect a closed state.
| WFS_SIU_OPEN  | At least one of the Left Cabinet   | B   |
| ------------- | ---------------------------------- | --- |
Doors can detect an open state.
| WFS_SIU_LOCKED  | At least one of the Left Cabinet   | B   |
| --------------- | ---------------------------------- | --- |
Doors can be locked.
| WFS_SIU_BOLTED  | At least one of the Left Cabinet   | B   |
| --------------- | ---------------------------------- | --- |
Doors can be bolted.
| WFS_SIU_TAMPERED  | At least one of the Left Cabinet  | B   |
| ----------------- | --------------------------------- | --- |
Doors can detect potential tampering.
31

CWA 16926-10:2020 (E)
fwDoors [WFS_SIU_CABINET_RIGHT]
Specifies whether at least one Right Cabinet Door is available, and if so, which states they can
take (the overall capability for all cabinet doors is available through the capability for
WFS_SIU_CABINET). The right is defined as the side to the right as seen by the
customer/consumer. Specified as WFS_SIU_NOT_AVAILABLE or as a combination of the
following flags of type B:
| Value                  | Meaning                          | Type  |
| ---------------------- | -------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Right Cabinet Door   | A     |
available.
| WFS_SIU_CLOSED  | At least one of the Right Cabinet   | B   |
| --------------- | ----------------------------------- | --- |
Doors can detect a closed state.
| WFS_SIU_OPEN  | At least one of the Right Cabinet   | B   |
| ------------- | ----------------------------------- | --- |
Doors can detect an open state.
| WFS_SIU_LOCKED  | At least one of the Right Cabinet   | B   |
| --------------- | ----------------------------------- | --- |
Doors can be locked.
| WFS_SIU_BOLTED  | At least one of the Right Cabinet   | B   |
| --------------- | ----------------------------------- | --- |
Doors can be bolted.
| WFS_SIU_TAMPERED  | At least one of the Right Cabinet  | B   |
| ----------------- | ---------------------------------- | --- |
Doors can detect potential tampering.
fwIndicators [...]
Specifies which indicators are available, and if so, which states they can take. A number of
indicator types are defined below. Vendor specific indicators are defined starting from the end of
the array. The maximum indicator index is WFS_SIU_INDICATORS_MAX.
fwIndicators [WFS_SIU_OPENCLOSE]
Specifies whether the Open/Closed Indicator is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Open/Closed Indicator available.
| WFS_SIU_AVAILABLE  | The Open/Closed Indicator is available.  |     |
| ------------------ | ---------------------------------------- | --- |
fwIndicators [WFS_SIU_FASCIALIGHT]
Specifies whether the Fascia Light is available. Specified as one of the following flags:
| Value                  | Meaning                              |     |
| ---------------------- | ------------------------------------ | --- |
| WFS_SIU_NOT_AVAILABLE  | There is no Fascia Light available.  |     |
| WFS_SIU_AVAILABLE      | The Fascia Light is available.       |     |
fwIndicators [WFS_SIU_AUDIO]
Specifies whether the Audio Indicator device is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Audio Indicator available.
| WFS_SIU_AVAILABLE  | The Audio Indicator is available.  |     |
| ------------------ | ---------------------------------- | --- |
fwIndicators [WFS_SIU_HEATING]
Specifies whether the internal Heating device is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Heating device available.
| WFS_SIU_AVAILABLE  | The Heating device is available.  |     |
| ------------------ | --------------------------------- | --- |
fwIndicators [WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT]
Specifies whether the Consumer Display Backlight is available. Specified as one of the following
flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Consumer Display Backlight
device available or it is not controllable by
the application.
| WFS_SIU_AVAILABLE  | The Consumer Display Backlight device is  |     |
| ------------------ | ----------------------------------------- | --- |
available and is controllable by the
application.
32

CWA 16926-10:2020 (E)

fwIndicators [WFS_SIU_SIGNAGEDISPLAY]
Specifies whether the Signage Display is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Signage Display available.
| WFS_SIU_AVAILABLE  | The Signage Display is available.  |     |
| ------------------ | ---------------------------------- | --- |
fwIndicators [WFS_SIU_TRANSINDICATOR]
Specifies whether the Transaction Indicators are available as a bitmap. Each bit of this value
represents one Transaction Indicator and is specified as one of the following binary values:
| Value  | Meaning                                      |     |
| ------ | -------------------------------------------- | --- |
| 0      | The Transaction Indicator is not available.  |     |
| 1      | The Transaction Indicator is available.      |     |
The following flags can be used to reference each Transaction Indicator.
| Value          | Meaning                   |     |
| -------------- | ------------------------- | --- |
| WFS_SIU_LAMP1  | Transaction Indicator 1.  |     |
| WFS_SIU_LAMP2  | Transaction Indicator 2.  |     |
…
| WFS_SIU_LAMP16  | Transaction Indicator 16.  |     |
| --------------- | -------------------------- | --- |
fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
Specifies whether the vendor dependent General-Purpose Output Ports are available. Before
making use of the General-Purpose Output Ports the vendor should examine if the required
functionality is covered in this or other device classes as a vendor independent feature. This value
is a bitmap and each bit of this value represents one General-Purpose Output Port. Each bit is
specified as one of the following binary values:
| Value  | Meaning                                 |     |
| ------ | --------------------------------------- | --- |
| 0      | The General-Purpose Output Port is not  |     |
available.
| 1   | The General-Purpose Output Port is  |     |
| --- | ----------------------------------- | --- |
available.
The following flags can be used to reference each General-Purpose Output Port.
| Value         | Meaning                         |     |
| ------------- | ------------------------------- | --- |
| WFS_SIU_GPP1  | General-Purpose Output Port 1.  |     |
| WFS_SIU_GPP2  | General-Purpose Output Port 2.  |     |
…
| WFS_SIU_GPP16  | General-Purpose Output Port 16.  |     |
| -------------- | -------------------------------- | --- |
fwAuxiliaries [...]
Specifies which auxiliaries are available, and if so, which states they can take. A number of
auxiliary indicator types are defined below. Vendor specific auxiliaries are defined starting from
the end of the array. The maximum auxiliary index is WFS_SIU_AUXILIARIES_MAX.
fwAuxiliaries [WFS_SIU_VOLUME]
Specifies whether the Volume Control is available, and if so, the increment/decrement value
recommended by the vendor.
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Volume Control available.
| 1, ..., 1000  | The recommended increment/decrement  |     |
| ------------- | ------------------------------------ | --- |
value for the Volume Control.
fwAuxiliaries [WFS_SIU_UPS]
Specifies whether the UPS device is available, and if so, which states it can take. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
| Value                  | Meaning                         | Type  |
| ---------------------- | ------------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no UPS available.      | A     |
| WFS_SIU_AVAILABLE      | The UPS is available.           | B     |
| WFS_SIU_LOW            | The UPS can indicate that its   | B     |
charge level is low.
33

CWA 16926-10:2020 (E)
| WFS_SIU_ENGAGED  | The UPS can be engaged and   | B   |
| ---------------- | ---------------------------- | --- |
disengaged by the application.
| WFS_SIU_POWERING  | The UPS can indicate that it is   | B   |
| ----------------- | --------------------------------- | --- |
powering the system while the
main power supply is off.
| WFS_SIU_RECOVERED  | The UPS can indicate that it was   | B   |
| ------------------ | ---------------------------------- | --- |
engaged when the main power
went off.
fwAuxiliaries [WFS_SIU_REMOTE_STATUS_MONITOR]
Specifies whether the Remote Status Monitor device is available. The Remote Status Monitor
device uses LEDs for displaying the status of the system. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Remote Status Monitor device
available.
| WFS_SIU_AVAILABLE  | The Remote Status Monitor device is  |     |
| ------------------ | ------------------------------------ | --- |
available.
fwAuxiliaries [WFS_SIU_AUDIBLE_ALARM]
Specifies whether the Audible Alarm device is available. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NOT_AVAILABLE  There is no Audible Alarm device available.
| WFS_SIU_AVAILABLE  | The Audible Alarm device is available.  |     |
| ------------------ | --------------------------------------- | --- |
fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
Specifies whether the Enhanced Audio Controller is available, and if so, which modes it supports.
The Enhanced Audio Controller controls how private and public audio are broadcast when the
headset is inserted into/removed from the audio jack and when the handset is off-hook/on-hook. In
the following Privacy Device is used to refer to either the headset or handset. The modes it
supports are specified as WFS_SIU_NOT_AVAILABLE or as a combination of the following
flags of type B:
| Value                  | Meaning                      | Type  |
| ---------------------- | ---------------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Enhanced Audio   | A     |
Controller available.
WFS_SIU_HEADSET_DETECTION  The Enhanced Audio Controller   B
is available and supports Privacy
Device activation/deactivation.
The device is able to report events
to indicate Privacy Device
activation/deactivation.
WFS_SIU_MODE_CONTROLLABLE  The Enhanced Audio Controller   B
is available and supports
application control of the
Privacy Device mode via the
WFS_CMD_SIU_SET_PORTS and
WFS_CMD_SIU_SET_AUXILIARY
command.
fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
Specifies whether the Enhanced Microphone Controller is available, and if so, which modes it
supports. The Enhanced Microphone Controller controls how private and public audio input are
transmitted when the headset is inserted into/removed from the audio jack and when the handset is
off-hook/on-hook. In the following Privacy Device is used to refer to either the headset or handset.
The modes it supports are specified as WFS_SIU_NOT_AVAILABLE or as a combination of the
following flags of type B:
| Value                  | Meaning               | Type  |
| ---------------------- | --------------------- | ----- |
| WFS_SIU_NOT_AVAILABLE  | There is no Enhanced  | A     |
Microphone Controller available.
| WFS_SIU_HEADSET_DETECTION  | The Enhanced Microphone   | B   |
| -------------------------- | ------------------------- | --- |
Controller is available and supports
34

CWA 16926-10:2020 (E)
Privacy Device activation/deactivation.
The device is able to report events
to indicate Privacy Device
activation/deactivation.
WFS_SIU_MODE_CONTROLLABLE The Enhanced Microphone B
Controller is available and
Supports application control of the
Privacy Device mode via the
WFS_CMD_SIU_SET_PORTS and
WFS_CMD_SIU_SET_AUXILIARY
command.
fwAuxiliaries [WFS_SIU_MICROPHONEVOLUME]
Specifies whether the Microphone Volume Control is available, and if so, the
increment/decrement value recommended by the vendor.
Value Meaning
WFS_SIU_NOT_AVAILABLE There is no Microphone Volume Control
available.
1, ..., 1000 The recommended increment/decrement
value for the Microphone Volume Control.
fwGuidLights [...]
Specifies which guidance lights are available. Vendor specific guidance lights are defined starting
from the end of the array. See WFS_INF_SIU_CAPABILITIES fwGuidLightsEx for standard
guidance light identifiers and states.
Note the maximum guidance light identifier is WFS_SIU_GUIDLIGHTS_MAX and direction
flags (Type D) cannot be reported from this field as the field type is WORD. It is recommended
that applications use fwGuidLightsEx.lpszExtra
Pointer to a list of vendor-specific, or any other extended, information. The information is
returned as a series of “key=value” strings so that it is easily extensible by Service Providers.
Each string is null-terminated, with the final string terminating with two null characters. An empty
list may be indicated by either a NULL pointer or a pointer to two consecutive null characters.
bPowerSaveControl
Specifies whether power saving control is available. This can either be TRUE if available or
FALSE if not available.
fwAutoStartupMode
Specifies which mode of the auto start-up control is supported. Specified as
WFS_SIU_NOT_AVAILABLE or as a combination of the following flags of type B:
Value Meaning Type
WFS_SIU_NOT_AVAILABLE There is no auto start-up control A
available.
WFS_SIU_AUTOSTARTUP_SPECIFIC The device supports one-time B
auto start-up on a specific date
at a specific time.
WFS_SIU_AUTOSTARTUP_DAILY The device supports auto B
start-up every day at a specific
time.
WFS_SIU_AUTOSTARTUP_WEEKLY The device supports auto B
start-up at a specified time
on a specific day of every week.
bAntiFraudModule
Specifies whether the anti-fraud module is available. This can either be TRUE if available or
FALSE if not available.
lpdwSynchronizableCommands
Pointer to a zero-terminated list of DWORDs which contains the execute command IDs that can
be synchronized. If no execute command can be synchronized then this parameter will be NULL.
35

CWA 16926-10:2020 (E)
lpTerminalInformation
Specifies identification information about the terminal, for example the model ID and serial
number of the terminal [Ref. 2].
typedef struct _wfs_siu_trm_info
  {
  LPWSTR    lpszModelID;
  LPWSTR    lpszSerialNumber;
  } WFSSIUTRMINFO, *LPWFSSIUTRMINFO;
lpszModelID
Specifies a UNICODE string which identifies the model ID of the terminal, This string value is
terminated with a null character. lpszModelID is NULL when the model ID is unknown.
lpszSerialNumber
Specifies a UNICODE string which identifies the unique serial number of the terminal, This
string value is terminated with a null character. lpszSerialNumber is NULL when the serial
number is unknown.
fwGuidLightsEx [...]
Specifies which guidance light identifiers are available. Vendor specific guidance light identifiers
are defined starting from the end of the array. The maximum guidance light identifier is
WFS_SIU_GUIDLIGHTS_MAX_EX.
The elements of this array are specified as a combination of the following flags and indicate all of
the possible flash rates (type B) , colors (type C) and directions (type D) that the guidance light
indicator is capable of handling. If the guidance light indicator only supports one color then no
value of type C is returned. If the guidance light indicator does not support direction then no value
of type D is returned. A value of WFS_SIU_NOT_AVAILABLE indicates that the guidance light
indicator is not available. In this case, the guidance light indicator may be available using the
device specific Service Provider.
Note that direction flags (Type D) cannot be reported from fwGuidLights as the field type is
WORD.
| Value  | Meaning  | Type  |
| ------ | -------- | ----- |
WFS_SIU_NOT_AVAILABLE  There is no guidance light control  A
available at this position.
| WFS_SIU_OFF           | The light can be off.        | B   |
| --------------------- | ---------------------------- | --- |
| WFS_SIU_SLOW_FLASH    | The light can blink slowly.  | B   |
| WFS_SIU_MEDIUM_FLASH  | The light can blink medium   | B   |
frequency.
| WFS_SIU_QUICK_FLASH  | The light can blink quickly.  | B   |
| -------------------- | ----------------------------- | --- |
| WFS_SIU_CONTINUOUS   | The light can be              | B   |
continuous (steady).
| WFS_SIU_GUIDANCE_RED      | The light can be red.      | C   |
| ------------------------- | -------------------------- | --- |
| WFS_SIU_GUIDANCE_GREEN    | The light can be green.    | C   |
| WFS_SIU_GUIDANCE_YELLOW   | The light can be yellow.   | C   |
| WFS_SIU_GUIDANCE_BLUE     | The light can be blue.     | C   |
| WFS_SIU_GUIDANCE_CYAN     | The light can be cyan.     | C   |
| WFS_SIU_GUIDANCE_MAGENTA  | The light can be magenta.  | C   |
| WFS_SIU_GUIDANCE_WHITE    | The light can be white.    | C   |
WFS_SIU_GUIDANCE_ENTRY  The light can be in the entry state.  D
WFS_SIU_GUIDANCE_EXIT  The light can be in the exit state.  D
The following standard guidance light identifiers are available.
WFS_SIU_CARDUNIT
Specifies whether the Guidance Light Indicator on the Card Unit (IDC) is available.
WFS_SIU_PINPAD
Specifies whether the Guidance Light Indicator on the PIN pad unit is available.
WFS_SIU_NOTESDISPENSER
Specifies whether the Guidance Light Indicator on the note dispenser unit is available.
36

CWA 16926-10:2020 (E)
WFS_SIU_COINDISPENSER
Specifies whether the Guidance Light Indicator on the coin dispenser unit is available.
WFS_SIU_RECEIPTPRINTER
Specifies whether the Guidance Light Indicator on the receipt printer unit is available.
WFS_SIU_PASSBOOKPRINTER
Specifies whether the Guidance Light Indicator on the passbook printer unit is available.
WFS_SIU_ENVDEPOSITORY
Specifies whether the Guidance Light Indicator on the envelope depository unit is available.
WFS_SIU_CHEQUEUNIT
Specifies whether the Guidance Light Indicator on the cheque processing unit is available.
WFS_SIU_BILLACCEPTOR
Specifies whether the Guidance Light Indicator on the bill acceptor unit is available.
WFS_SIU_ENVDISPENSER
Specifies whether the Guidance Light Indicator on the envelope dispenser unit is available.
WFS_SIU_DOCUMENTPRINTER
Specifies whether the Guidance Light Indicator on the document printer is available.
WFS_SIU_COINACCEPTOR
Specifies whether the Guidance Light Indicator on the coin acceptor is available.
WFS_SIU_SCANNER
Specifies whether the Guidance Light Indicator on the scanner unit is available.
The following identifiers are only reported from fwGuidLightsEx as they are greater than
WFS_SIU_GUIDLIGHTS_MAX,
WFS_SIU_CONTACTLESS
Specifies whether the Guidance Light Indicator on contactless reader is available.
WFS_SIU_CARDUNIT2
Specifies whether the Guidance Light Indicator on the scanner unit is available.
WFS_SIU_NOTESDISPENSER2
Specifies whether the Guidance Light Indicator on the second note dispenser unit is available.
WFS_SIU_BILLACCEPTOR2
Specifies whether the Guidance Light Indicator on the second bill acceptor unit is available.
Error Codes Only the generic error codes defined in [Ref. 1] can be generated by this command.
Comments Applications which require or expect specific information to be present in the lpszExtra parameter
may not be device or vendor-independent.
37

CWA 16926-10:2020 (E)
4.3 WFS_INF_SIU_GET_AUTOSTARTUP_TIME
Description This command is used to retrieve the availability of the auto start-up time function as well as the
current configuration of the auto start-up time.
Input Param None.
Output Param LPWFSSIUGETSTARTUPTIME lpGetStartupTime;
typedef struct _wfs_siu_get_startup_time
{
WORD wMode;
LPSYSTEMTIME lpStartTime;
} WFSSIUGETSTARTUPTIME, *LPWFSSIUGETSTARTUPTIME;
wMode
Specifies the current auto start-up control mode configured as one of the following flags.
Value Meaning
WFS_SIU_AUTOSTARTUP_CLEAR No auto start-up time is configured.
WFS_SIU_AUTOSTARTUP_SPECIFIC One-time auto start-up at a specific time on a
specific date has been configured.
In the lpStartTime parameter, only wYear,
wMonth, wDay, wHour and wMinute are
relevant. All other field must be ignored.
WFS_SIU_AUTOSTARTUP_DAILY Auto start-up every day has been configured.
In the lpStartTime parameter, only wHour
and wMinute are relevant. All other fields
must be ignored.
WFS_SIU_AUTOSTARTUP_WEEKLY Auto start-up at a specified time on a specific
day of every week has been configured.
In the lpStartTime parameter, only
wDayOfWeek, wHour and wMinute are
relevant. All other fields must be ignored.
lpStartTime
Specifies the current auto start-up time configuration.
Win32 SYSTEMTIME structure:
wYear
Specifies the year. The value should be ignored if it is not relevant to the wMode value.
wMonth
Specifies the month. The value should be ignored if it is not relevant to the wMode value.
wDayOfWeek
Specifies the day of the week, in values from 0 (Sunday) to 6 (Saturday). The value should be
ignored if it is not relevant to the wMode value.
wDay
Specifies the day of the month. The value should be ignored if it is not relevant to the wMode
value.
wHour
Specifies the hour. The value should be ignored if it is not relevant to the wMode value.
wMinute
Specifies the minute. The value should be ignored if it is not relevant to the wMode value.
wSecond
This field is not used and will be zero.
wMilliseconds
This field is not used and will be zero.
Error Codes Only the generic error codes defined in [Ref. 1] can be generated by this command.
Events None.
38

CWA 16926-10:2020 (E)
Comments None.
39

CWA 16926-10:2020 (E)
5. Execute Commands
5.1 WFS_CMD_SIU_ENABLE_EVENTS
Description This command is used to enable or disable events from the Sensors and Indicators Unit. The
default condition is that all events are disabled. The events are only enabled or disabled for the
session which sends the command, all other sessions are unaffected.
Input Param LPWFSSIUENABLE lpEnable;
typedef struct _wfs_siu_enable
{
WORD fwSensors [WFS_SIU_SENSORS_SIZE];
WORD fwDoors [WFS_SIU_DOORS_SIZE];
WORD fwIndicators [WFS_SIU_INDICATORS_SIZE];
WORD fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
WORD fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
LPSTR lpszExtra;
} WFSSIUENABLE, *LPWFSSIUENABLE;
fwSensors [...]
Specifies which of the sensors that should report changes. A number of sensor types are defined
below. Vendor specific sensors are defined starting from the end of the array. The maximum
sensor index is WFS_SIU_SENSORS_MAX.
fwSensors [WFS_SIU_OPERATORSWITCH]
Specifies whether the Operator Switch should report whenever the switch changes the operating
mode. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Operators Switch should report
whenever it changes mode from Run to
Maintenance or Supervisor mode or vice
versa.
WFS_SIU_DISABLE_EVENT The Operators Switch should not report any
changes of it operating mode.
fwSensors [WFS_SIU_TAMPER]
Specifies whether the Tamper Sensor should report whenever someone tampers with the terminal.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Tamper Sensor should report whenever
it detects any tampering attempt.
WFS_SIU_DISABLE_EVENT The Tamper Sensor should not report any
changes of its status.
fwSensors [WFS_SIU_INTTAMPER]
Specifies whether the Internal Tamper Sensor should report whenever someone tampers with the
internal alarm. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Internal Tamper Sensor should report
whenever it detects any tampering attempt.
WFS_SIU_DISABLE_EVENT The Internal Tamper Sensor should not
report any changes of its status.
fwSensors [WFS_SIU_SEISMIC]
Specifies whether the Seismic Sensor should report whenever any seismic activity is detected.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
40

CWA 16926-10:2020 (E)
WFS_SIU_ENABLE_EVENT The Seismic Sensor should report whenever
it detects any seismic activity.
WFS_SIU_DISABLE_EVENT The Seismic Sensor should not report any
changes of its status.
fwSensors [WFS_SIU_HEAT]
Specifies whether the Heat Sensor should report whenever any excessive heat is detected.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Heat Sensor should report whenever it
detects any excessive heat.
WFS_SIU_DISABLE_EVENT The Heat Sensor should not report any
changes of its status.
fwSensors [WFS_SIU_PROXIMITY]
Specifies whether the Proximity Sensor should report whenever any movement is detected close to
the terminal. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Proximity Sensor should report
whenever it detects any movement.
WFS_SIU_DISABLE_EVENT The Proximity Sensor should not report any
changes of its status.
fwSensors [WFS_SIU_AMBLIGHT]
Specifies whether the Ambient Light Sensor should report whenever it detects changes in the
ambient light. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Ambient Light Sensor should report
whenever it detects a change.
WFS_SIU_DISABLE_EVENT The Ambient Light Sensor should not report
any change.
fwSensors [WFS_SIU_ENHANCEDAUDIO]
Specifies whether the Audio Jack should report whenever it detects changes in the audio jack.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Audio Jack should report whenever it
detects a headset being connected or
disconnected.
WFS_SIU_DISABLE_EVENT The Audio Jack should not report any change
in headset connection state.
fwSensors [WFS_SIU_BOOT_SWITCH]
Specifies whether the Boot Switch should report whenever the delayed effect boot switch is used.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Boot Switch should report whenever it
detects the terminal is going to be rebooted
or shutdown.
WFS_SIU_DISABLE_EVENT The Boot Switch should not report any
changes of its status.
fwSensors [WFS_SIU_CONSUMER_DISPLAY]
Specifies whether the Consumer Display Sensor should report whenever it detects changes to the
consumer display. Specified as one of the following flags:
41

CWA 16926-10:2020 (E)
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Consumer Display Sensor should report
whenever it detects any changes of its status.
WFS_SIU_DISABLE_EVENT The Consumer Display Sensor should not
report any changes of its status.
fwSensors [WFS_SIU_OPERATOR_CALL_BUTTON]
Specifies whether the Operator Call Button should report whenever the Operator Call Button is
pressed or released. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Operator Call Button should report
whenever it detects that it is pressed or
released.
WFS_SIU_DISABLE_EVENT The Operator Call Button should not report
any changes of its status.
fwSensors [WFS_SIU_HANDSETSENSOR]
Specifies whether the Handset Sensor should report whenever it detects changes of its status.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Handset Sensor should report whenever
the handset is picked up or put down.
WFS_SIU_DISABLE_EVENT The Handset Sensor should not report any
changes of its status.
fwSensors [WFS_SIU_GENERALINPUTPORT]
Specifies whether the General-Purpose Input Port should report whenever it detects changes to any
one of the General-Purpose Input Ports. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The General-Purpose Input Port should
report whenever any General-Purpose Input
Port is turned on or off.
WFS_SIU_DISABLE_EVENT The General-Purpose Input Port should not
report any changes of its status.
fwSensors [WFS_SIU_HEADSETMICROPHONE]
Specifies whether the Microphone Jack should report whenever it detects changes in the
microphone jack. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Microphone Jack should report
whenever it detects a headset microphone
being connected or disconnected.
WFS_SIU_DISABLE_EVENT The Microphone Jack should not report any
change in headset microphone connection
state.
fwDoors [...]
Specifies which of the doors should report changes. A number of door types are defined below.
Vendor specific doors are defined starting from the end of the array. The maximum door index is
WFS_SIU_DOORS_MAX.
fwDoors [WFS_SIU_CABINET]
Specifies whether the Cabinet Doors should report whenever the doors are opened, closed, bolted
or locked. Specified as one of the following flags:
42

CWA 16926-10:2020 (E)
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Cabinet Doors should report whenever
the doors are opened, closed, locked or
bolted.
WFS_SIU_DISABLE_EVENT The Cabinet Doors sensor should not report
any changes of the doors status.
fwDoors [WFS_SIU_SAFE]
Specifies whether the Safe Doors should report whenever the doors are opened, closed, bolted or
locked. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Safe Doors should report whenever the
doors are opened, closed, locked or bolted.
WFS_SIU_DISABLE_EVENT The Safe Doors should not report any
changes of the doors status.
fwDoors [WFS_SIU_VANDALSHIELD]
Specifies whether the Vandal Shield should report whenever the shield changed position.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Vandal Shield should report whenever
the doors are opened or closed.
WFS_SIU_DISABLE_EVENT The Vandal Shield should not report any
changes of the status.
fwDoors [WFS_SIU_CABINET_FRONT]
Specifies whether the Front Cabinet Doors should report whenever the front doors are opened,
closed, bolted or locked. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Front Cabinet Doors should report
whenever the doors are opened, closed,
locked or bolted.
WFS_SIU_DISABLE_EVENT The Front Cabinet Doors sensor should not
report any changes of the doors status.
fwDoors [WFS_SIU_CABINET_REAR]
Specifies whether the Rear Cabinet Doors should report whenever the rear doors are opened,
closed, bolted or locked. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Rear Cabinet Doors should report
whenever the doors are opened, closed,
locked or bolted.
WFS_SIU_DISABLE_EVENT The Rear Cabinet Doors sensor should not
report any changes of the doors status.
fwDoors [WFS_SIU_CABINET_LEFT]
Specifies whether the Left Cabinet Doors should report whenever the left doors are opened,
closed, bolted or locked. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Left Cabinet Doors should report
whenever the doors are opened, closed,
locked or bolted.
WFS_SIU_DISABLE_EVENT The Left Cabinet Doors sensor should not
report any changes of the doors status.
43

CWA 16926-10:2020 (E)
fwDoors [WFS_SIU_CABINET_RIGHT]
Specifies whether the Right Cabinet Doors should report whenever the right doors are opened,
closed, bolted or locked. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Right Cabinet Doors should report
whenever the doors are opened, closed,
locked or bolted.
WFS_SIU_DISABLE_EVENT The Right Cabinet Doors sensor should not
report any changes of the doors status.
fwIndicators [...]
Specifies which of the indicators should report changes. A number of indicator types are defined
below. Vendor specific indicators are defined starting from the end of the array. The maximum
indicator index is WFS_SIU_INDICATORS_MAX.
fwIndicators [WFS_SIU_OPENCLOSE]
Specifies whether the Open/Closed Indicator should report whenever it is turned on (set to open)
or turned off (set to closed). Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Open/Closed Indicator should report
whenever it is turned on or off.
WFS_SIU_DISABLE_EVENT The Open/Closed Indicator should not report
any changes of the indicator.
fwIndicators [WFS_SIU_FASCIALIGHT]
Specifies whether the Fascia Light should report whenever it is turned on or turned off. Specified
as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Fascia Light should report whenever it is
turned on or off.
WFS_SIU_DISABLE_EVENT The Fascia Light should not report any
changes.
fwIndicators [WFS_SIU_AUDIO]
Specifies whether the Audio Indicator should report whenever it is turned on or turned off.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Audio Indicator should report whenever
it is turned on or off.
WFS_SIU_DISABLE_EVENT The Audio Indicator should not report any
changes.
fwIndicators [WFS_SIU_HEATING]
Specifies whether the Heating device should report whenever it is turned on or turned off.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Heating device should report whenever
it is turned on or off.
WFS_SIU_DISABLE_EVENT The Heating device should not report any
changes.
fwIndicators [WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT]
Specifies whether the Consumer Display Backlight should report whenever it is turned on or
turned off. Specified as one of the following flags:
44

CWA 16926-10:2020 (E)
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Consumer Display Backlight should
report whenever it is turned on or off.
WFS_SIU_DISABLE_EVENT The Consumer Display Backlight should not
report any changes.
fwIndicators [WFS_SIU_SIGNAGEDISPLAY]
Specifies whether the Signage Display should report whenever it is turned on or turned off.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Signage Display should report whenever
it is turned on or off.
WFS_SIU_DISABLE_EVENT The Signage Display should not report any
changes.
fwIndicators [WFS_SIU_TRANSINDICATOR]
Specifies whether the Transaction Indicators should report whenever any one of them is turned on
or turned off. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Transaction Indicators should report
whenever any Transaction Indicator is turned
on or off.
WFS_SIU_DISABLE_EVENT The Transaction Indicators should not report
any changes.
fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
Specifies whether the General-Purpose Output Ports should report whenever any one of them is
turned on or turned off. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The General-Purpose Output Ports should
report whenever any General-Purpose Output
Port is turned on or off.
WFS_SIU_DISABLE_EVENT The General-Purpose Output Ports should
not report any changes.
fwAuxiliaries [...]
Specifies which of the auxiliary indicators should report changes. A number of auxiliary indicator
types are defined below. Vendor specific indicators are defined starting from the end of the array.
The maximum indicator index is WFS_SIU_AUXILIARIES_MAX.
fwAuxiliaries[WFS_SIU_VOLUME]
Specifies whether the Volume Control device should report whenever it is changed. Specified as
one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Volume Control device should report
whenever it is changed.
WFS_SIU_DISABLE_EVENT The Volume Control device should not
report any changes.
fwAuxiliaries[WFS_SIU_UPS]
Specifies whether the UPS device should report whenever it is changed. Specified as one of the
following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
45

CWA 16926-10:2020 (E)
WFS_SIU_ENABLE_EVENT The UPS device should report whenever it is
changed.
WFS_SIU_DISABLE_EVENT The UPS device should not report any
changes.
fwAuxiliaries[WFS_SIU_REMOTE_STATUS_MONITOR]
Specifies whether the Remote Status Monitor device should report whenever it is changed.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Remote Status Monitor device should
report whenever it is changed.
WFS_SIU_DISABLE_EVENT The Remote Status Monitor device should
not report any changes.
fwAuxiliaries[WFS_SIU_AUDIBLE_ALARM]
Specifies whether the Audible Alarm device should report whenever it is changed. Specified as
one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Audible Alarm device should report
whenever it is changed.
WFS_SIU_DISABLE_EVENT The Audible Alarm device should not report
any changes.
fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
Specifies whether the Enhanced Audio Controller should report whenever it changes status
(assuming the device is capable of generating events). Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Enhanced Audio Controller should
report whenever it is changed.
WFS_SIU_DISABLE_EVENT The Enhanced Audio Controller device
should not report any changes.
fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
Specifies whether the Enhanced Microphone Controller should report whenever it changes status
(assuming the device is capable of generating events). Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Enhanced Microphone Controller should
report whenever it is changed.
WFS_SIU_DISABLE_EVENT The Enhanced Microphone Controller device
should not report any changes.
fwAuxiliaries[WFS_SIU_MICROPHONEVOLUME]
Specifies whether the Microphone Volume Control device should report whenever it is changed.
Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
WFS_SIU_ENABLE_EVENT The Microphone Volume Control device
should report whenever it is changed.
WFS_SIU_DISABLE_EVENT The Microphone Volume Control device
should not report any changes.
fwGuidLights [...]
Specifies which of the Guidance Light Indicators should report whenever any of them changes its
state. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current reporting status.
46

CWA 16926-10:2020 (E)
WFS_SIU_ENABLE_EVENT The Guidance Light Indicators should report
whenever any of them changes its state.
WFS_SIU_DISABLE_EVENT The Guidance Light Indicators should not
report any changes of their states.
fwGuidLights [WFS_SIU_CARDUNIT]
Specifies whether the Guidance Light Indicator on the Card Unit (IDC) should report whenever it
changes status.
fwGuidLights [WFS_SIU_PINPAD]
Specifies whether the Guidance Light Indicator on the PIN pad unit should report whenever it
changes status.
fwGuidLights [WFS_SIU_NOTESDISPENSER]
Specifies whether the Guidance Light Indicator on the note dispenser unit should report whenever
it changes status.
fwGuidLights [WFS_SIU_COINDISPENSER]
Specifies whether the Guidance Light Indicator on the coin dispenser unit should report whenever
it changes status.
fwGuidLights [WFS_SIU_RECEIPTPRINTER]
Specifies whether the Guidance Light Indicator on the receipt printer unit should report whenever
it changes status.
fwGuidLights [WFS_SIU_PASSBOOKPRINTER]
Specifies whether the Guidance Light Indicator on the passbook printer unit should report
whenever it changes status.
fwGuidLights [WFS_SIU_ENVDEPOSITORY]
Specifies whether the Guidance Light Indicator on the envelope depository unit should report
whenever it changes status.
fwGuidLights [WFS_SIU_CHEQUEUNIT]
Specifies whether the Guidance Light Indicator on the cheque processing unit should report
whenever it changes status.
fwGuidLights [WFS_SIU_BILLACCEPTOR]
Specifies whether the Guidance Light Indicator on the bill acceptor unit should report whenever it
changes status.
fwGuidLights [WFS_SIU_ENVDISPENSER]
Specifies whether the Guidance Light Indicator on the envelope dispenser unit should report
whenever it changes status.
fwGuidLights [WFS_SIU_DOCUMENTPRINTER]
Specifies whether the Guidance Light Indicator on the document printer should report whenever it
changes status.
fwGuidLights [WFS_SIU_COINACCEPTOR]
Specifies whether the Guidance Light Indicator on the coin acceptor should report whenever it
changes status.
fwGuidLights [WFS_SIU_SCANNER]
Specifies whether the Guidance Light Indicator on the scanner unit should report whenever it
changes status.
lpszExtra
Pointer to a list of vendor-specific, or any other extended, information. The information is
returned as a series of “key=value” strings so that it is easily extensible by Service Providers.
Each string is null-terminated, with the final string terminating with two null characters. An empty
list may be indicated by either a NULL pointer or a pointer to two consecutive null characters.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
47

CWA 16926-10:2020 (E)
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to enable or disable events to a
port was invalid because the port does not
exist.
WFS_ERR_SIU_SYNTAX The command was invoked with incorrect
input data. E.g. an attempt to both enable and
disable events to the same port was made.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while enabling or
disabling events on one or more ports.
Comments No action has been taken if this command returns an error. If a hardware error occurs while
executing the command, the command will return OK, but execute event(s) will be generated
which indicate(s) the port(s) which have failed.
48

CWA 16926-10:2020 (E)
5.2 WFS_CMD_SIU_SET_PORTS
Description This command is used to set or clear one or more output ports (indicators) in the Sensors and
Indicators Unit.
Input Param LPWFSSIUSETPORTS lpSetPorts;
typedef struct _wfs_siu_set_ports
{
WORD fwDoors [WFS_SIU_DOORS_SIZE];
WORD fwIndicators [WFS_SIU_INDICATORS_SIZE];
WORD fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
WORD fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
LPSTR lpszExtra;
} WFSSIUSETPORTS, *LPWFSSIUSETPORTS;
fwDoors [WFS_SIU_CABINET]
Specifies whether all Cabinet Doors should be bolted or unbolted. Specified as one of the
following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current status of the
Cabinet Doors.
WFS_SIU_BOLT All Cabinet Doors are bolted.
WFS_SIU_UNBOLT All Cabinet Doors are unbolted.
fwDoors [WFS_SIU_SAFE]
Specifies whether the Safe Doors should be bolted or unbolted. Specified as one of the following
flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current status of the Safe
Doors.
WFS_SIU_BOLT The Safe Doors are bolted.
WFS_SIU_UNBOLT The Safe Doors are unbolted.
fwDoors [WFS_SIU_VANDALSHIELD]
Specifies whether the Vandal Shield should change position. Specified as one of the following
flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current position of the
Vandal Shield.
WFS_SIU_CLOSED The Vandal Shield is closed.
WFS_SIU_OPEN The Vandal Shield is opened.
WFS_SIU_SERVICE The Vandal Shield is set in service position.
WFS_SIU_KEYBOARD The Vandal Shield is set in position that
permits access to the keyboard.
fwDoors [WFS_SIU_CABINET_FRONT]
Specifies whether the Front Cabinet Doors should be bolted or unbolted. Specified as one of the
following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current status of the Front
Cabinet Doors.
WFS_SIU_BOLT All Front Cabinet Doors are bolted.
WFS_SIU_UNBOLT All Front Cabinet Doors are unbolted.
fwDoors [WFS_SIU_CABINET_REAR]
Specifies whether the Rear Cabinet Doors should be bolted or unbolted. Specified as one of the
following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current status of the Rear
Cabinet Doors.
49

CWA 16926-10:2020 (E)
| WFS_SIU_BOLT    | All Rear Cabinet Doors are bolted.    |     |
| --------------- | ------------------------------------- | --- |
| WFS_SIU_UNBOLT  | All Rear Cabinet Doors are unbolted.  |     |
fwDoors [WFS_SIU_CABINET_LEFT]
Specifies whether the Left Cabinet Doors should be bolted or unbolted. Specified as one of the
following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NO_CHANGE  Do not change the current status of the Left
Cabinet Doors.
| WFS_SIU_BOLT    | All Left Cabinet Doors are bolted.    |     |
| --------------- | ------------------------------------- | --- |
| WFS_SIU_UNBOLT  | All Left Cabinet Doors are unbolted.  |     |
fwDoors [WFS_SIU_CABINET_RIGHT]
Specifies whether the Right Cabinet Doors should be bolted or unbolted. Specified as one of the
following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NO_CHANGE  Do not change the current status of the Right
Cabinet Doors.
| WFS_SIU_BOLT    | All Right Cabinet Doors are bolted.    |     |
| --------------- | -------------------------------------- | --- |
| WFS_SIU_UNBOLT  | All Right Cabinet Doors are unbolted.  |     |
fwIndicators [WFS_SIU_OPENCLOSE]
Specifies whether the Open/Closed Indicator should show Open or Close to a consumer. Specified
as one of the following flags:
| Value              | Meaning                                  |     |
| ------------------ | ---------------------------------------- | --- |
| WFS_SIU_NO_CHANGE  | Do not change the current status of the  |     |
Open/Closed Indicator.
| WFS_SIU_CLOSED  | The Open/Closed Indicator is changed to  |     |
| --------------- | ---------------------------------------- | --- |
show that the terminal is closed for a
consumer.
| WFS_SIU_OPEN  | The Open/Closed Indicator is changed to  |     |
| ------------- | ---------------------------------------- | --- |
show that the terminal is open to be used by
a consumer.
fwIndicators [WFS_SIU_FASCIALIGHT]
Specifies whether the Fascia Lights should be turned on or off. Specified as one of the following
flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NO_CHANGE  Do not change the current status of the light.
| WFS_SIU_OFF  | The Fascia Light is turned off.  |     |
| ------------ | -------------------------------- | --- |
| WFS_SIU_ON   | The Fascia Light is turned on.   |     |
fwIndicators [WFS_SIU_AUDIO]
Specifies whether the Audio Indicator should be turned on or off. Specified as one of the
following flags of type A and B, or as WFS_SIU_CONTINUOUS in combination with one of the
flags of type B:
| Value              | Meaning                            | Type  |
| ------------------ | ---------------------------------- | ----- |
| WFS_SIU_NO_CHANGE  | Do not change the current status   | A     |
of the Audio Indicator.
| WFS_SIU_OFF       | The Audio Indicator is turned off.   | A   |
| ----------------- | ------------------------------------ | --- |
| WFS_SIU_KEYPRESS  | The Audio Indicator sounds a key  B  |     |
click signal.
| WFS_SIU_EXCLAMATION  | The Audio Indicator sounds an   | B   |
| -------------------- | ------------------------------- | --- |
exclamation signal.
| WFS_SIU_WARNING  | The Audio Indicator sounds a   | B   |
| ---------------- | ------------------------------ | --- |
warning signal.
| WFS_SIU_ERROR  | The Audio Indicator sounds an   | B   |
| -------------- | ------------------------------- | --- |
error signal.
| WFS_SIU_CRITICAL  | The Audio Indicator sounds a   | B   |
| ----------------- | ------------------------------ | --- |
critical error signal.
50

CWA 16926-10:2020 (E)

| WFS_SIU_CONTINUOUS  | The Audio Indicator sound is   | C   |
| ------------------- | ------------------------------ | --- |
turned on continuously.
fwIndicators [WFS_SIU_HEATING]
Specifies whether the Internal Heating device should be turned on or off. Specified as one of the
following flags:
| Value              | Meaning                                  |     |
| ------------------ | ---------------------------------------- | --- |
| WFS_SIU_NO_CHANGE  | Do not change the current status of the  |     |
Internal Heating device.
| WFS_SIU_OFF  | The Internal Heating device is turned off.  |     |
| ------------ | ------------------------------------------- | --- |
| WFS_SIU_ON   | The Internal Heating device is turned on.   |     |
fwIndicators [WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT]
Specifies whether the Consumer Display Backlight should be turned on or off. Specified as one of
the following flags:
| Value              | Meaning                                  |     |
| ------------------ | ---------------------------------------- | --- |
| WFS_SIU_NO_CHANGE  | Do not change the current status of the  |     |
Consumer Display Backlight.
| WFS_SIU_ON  | The Consumer Display Backlight is turned  |     |
| ----------- | ----------------------------------------- | --- |
on.
| WFS_SIU_OFF  | The Consumer Display Backlight is turned  |     |
| ------------ | ----------------------------------------- | --- |
off.
fwIndicators [WFS_SIU_SIGNAGEDISPLAY]
Specifies whether the Consumer Display Backlight should be turned on or off. Specified as one of
the following flags:
| Value              | Meaning                                  |     |
| ------------------ | ---------------------------------------- | --- |
| WFS_SIU_NO_CHANGE  | Do not change the current status of the  |     |
Signage Display.
| WFS_SIU_ON   | The Signage Display is turned on.   |     |
| ------------ | ----------------------------------- | --- |
| WFS_SIU_OFF  | The Signage Display is turned off.  |     |
fwIndicators [WFS_SIU_TRANSINDICATOR]
Specifies whether the Transaction Indicators should be turned on or off. All Transaction Indicators
must be specified and each bit of this value represents one Transaction Indicator. Each bit is
specified as one of the following binary values:
| Value  | Meaning                                   |     |
| ------ | ----------------------------------------- | --- |
| 0      | The Transaction Indicator is turned off.  |     |
| 1      | The Transaction Indicator is turned on.   |     |
The following flags can be used to reference each Transaction Indicator.
| Value          | Meaning                   |     |
| -------------- | ------------------------- | --- |
| WFS_SIU_LAMP1  | Transaction Indicator 1.  |     |
| WFS_SIU_LAMP2  | Transaction Indicator 2.  |     |
…
| WFS_SIU_LAMP16  | Transaction Indicator 16.  |     |
| --------------- | -------------------------- | --- |
fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
Specifies whether the General-Purpose Output Ports should be turned on or off. All General-
Purpose Output Ports must be specified and each bit of this value represents one General-Purpose
Output Port. Each bit is specified as one of the following binary values:
| Value  | Meaning                                    |     |
| ------ | ------------------------------------------ | --- |
| 0      | The General-Purpose Output Port is turned  |     |
off.
| 1   | The General-Purpose Output Port is turned  |     |
| --- | ------------------------------------------ | --- |
on.
The following flags can be used to reference each General-Purpose Output Port.
| Value         | Meaning                         |     |
| ------------- | ------------------------------- | --- |
| WFS_SIU_GPP1  | General-Purpose Output Port 1.  |     |
51

CWA 16926-10:2020 (E)
| WFS_SIU_GPP2  | General-Purpose Output Port 2.  |     |
| ------------- | ------------------------------- | --- |
…
| WFS_SIU_GPP16  | General-Purpose Output Port 16.  |     |
| -------------- | -------------------------------- | --- |
fwAuxiliaries [WFS_SIU_VOLUME]
Specifies whether the value of the Volume Control should be changed. If so, the value of Volume
Control is defined in an interval from 1 to 1000 where 1 is the lowest volume level and 1000 is the
highest volume level. Specified as one of the following values:
| Value              | Meaning                                  |     |
| ------------------ | ---------------------------------------- | --- |
| WFS_SIU_NO_CHANGE  | Do not change the current status of the  |     |
Volume Control.
| 1, ..., 1000  | The volume level. This field is handled as an  |     |
| ------------- | ---------------------------------------------- | --- |
unsigned short. If a value greater than 1000
is used, the provider will map the value to
1000.
fwAuxiliaries [WFS_SIU_UPS]
Specifies whether the UPS device should be engaged or disengaged. The UPS device should not
be engaged when the charge level is low. Specified as one of the following flags:
| Value  | Meaning  |     |
| ------ | -------- | --- |
WFS_SIU_NO_CHANGE  Do not change the current status of the UPS
device.
| WFS_SIU_ENGAGE     | Engage the UPS.     |     |
| ------------------ | ------------------- | --- |
| WFS_SIU_DISENGAGE  | Disengage the UPS.  |     |
fwAuxiliaries[WFS_SIU_REMOTE_STATUS_MONITOR]
Specifies whether the state of the Remote Status Monitor device should be changed. Specified as
WFS_SIU_NO_CHANGE or a combination of one or more of the following flags of type B, C
and D, with at most one flag from each type.
| Value              | Meaning                            | Type  |
| ------------------ | ---------------------------------- | ----- |
| WFS_SIU_NO_CHANGE  | Do not change the current status   | A     |
of the Remote Status Monitor
device.
| WFS_SIU_GREEN_LED_ON  | Turn on the green LED on the   | B   |
| --------------------- | ------------------------------ | --- |
Remote Status Monitor device.
| WFS_SIU_GREEN_LED_OFF  | Turn off the green LED on the   | B   |
| ---------------------- | ------------------------------- | --- |
Remote Status Monitor device.
| WFS_SIU_AMBER_LED_ON  | Turn on the amber LED on the   | C   |
| --------------------- | ------------------------------ | --- |
Remote Status Monitor device.
| WFS_SIU_AMBER_LED_OFF  | Turn off the amber LED on the   | C   |
| ---------------------- | ------------------------------- | --- |
Remote Status Monitor device.
| WFS_SIU_RED_LED_ON  | Turn on the red LED on the   | D   |
| ------------------- | ---------------------------- | --- |
Remote Status Monitor device.
| WFS_SIU_RED_LED_OFF  | Turn off the red LED on the   | D   |
| -------------------- | ----------------------------- | --- |
Remote Status Monitor device.
fwAuxiliaries[WFS_SIU_AUDIBLE_ALARM]
Specifies whether the state of the Audible Alarm device should be changed. Specified as one of
the following flags:
| Value              | Meaning                                  |     |
| ------------------ | ---------------------------------------- | --- |
| WFS_SIU_NO_CHANGE  | Do not change the status of the Audible  |     |
Alarm device.
| WFS_SIU_OFF  | Turn off the Audible Alarm device.  |     |
| ------------ | ----------------------------------- | --- |
| WFS_SIU_ON   | Turn on the Audible Alarm device.   |     |
fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
Specifies whether the state of the Enhanced Audio Controller should be changed. Note that this
will only be acted upon for hardware environments that return
WFS_SIU_MODE_CONTROLLABLE for the WFS_SIU_ENHANCEDAUDIOCONTROL
auxiliary in the WFS_INF_SIU_CAPABILITIES command. Specified as one of the following
flags:
52

CWA 16926-10:2020 (E)
Value Meaning
WFS_SIU_NO_CHANGE Do not change the status of the Enhanced
Audio Controller.
WFS_SIU_PUBLICAUDIO_MANUAL Set the Enhanced Audio Controller to
manual mode, public state (i.e. audio will be
played through speakers only).
WFS_SIU_PUBLICAUDIO_AUTO Set the Enhanced Audio Controller to auto
mode, public state (i.e. audio will be played
through speakers). When a Privacy Device is
activated (headset connected/handset off-
hook), the device will go to the private state.
WFS_SIU_PUBLICAUDIO_SEMI_AUTO Set the Enhanced Audio Controller to semi-
auto mode, public state (i.e. audio will be
played through speakers). When a Privacy
Device is activated, the device will go to the
private state.
WFS_SIU_PRIVATEAUDIO_MANUAL Set the Enhanced Audio Controller to
manual mode, private state (i.e. audio will be
played only through a connected Privacy
Device). In private mode, no audio is
transmitted through the speakers.
WFS_SIU_PRIVATEAUDIO_AUTO Set the Enhanced Audio Controller to auto
mode, private state (i.e. audio will be played
only through an activated Privacy Device).
In private mode, no audio is transmitted
through the speakers. When a Privacy
Device is deactivated (headset
disconnected/handset on-hook), the device
will go to the public state.
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO Set the Enhanced Audio Controller to semi-
auto mode, private state (i.e. audio will be
played only through an activated Privacy
Device). In private mode, no audio is
transmitted through the speakers. When a
Privacy Device is deactivated, the device
will remain in the private state.
fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
Specifies whether the state of the Enhanced Microphone Controller should be changed. Note that
this will only be acted upon for hardware environments that return
WFS_SIU_MODE_CONTROLLABLE for the
WFS_SIU_ENHANCEDMICROPHONECONTROL auxiliary in the
WFS_INF_SIU_CAPABILITIES command. Specified as one of the following flags:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the status of the Enhanced
Microphone Controller.
WFS_SIU_PUBLICAUDIO_MANUAL Set the Enhanced Microphone Controller to
manual mode, public state (i.e. only the
microphone in the fascia is active).
WFS_SIU_PUBLICAUDIO_AUTO Set the Enhanced Microphone Controller to
auto mode, public state (i.e. only the
microphone in the fascia is active). When a
Privacy Device with a microphone is
activated (headset connected/handset off-
hook), the device will go to the private state.
53

CWA 16926-10:2020 (E)
WFS_SIU_PUBLICAUDIO_SEMI_AUTO Set the Enhanced Microphone Controller to
semi-auto mode, public state (i.e. only the
microphone in the fascia is active). When a
Privacy Device with a microphone is
activated, the device will go to the private
state.
WFS_SIU_PRIVATEAUDIO_MANUAL Set the Enhanced Microphone Controller to
manual mode, private state (i.e. audio input
will be only via a microphone in the Privacy
Device). In private mode, no audio input is
transmitted through the fascia microphone.
WFS_SIU_PRIVATEAUDIO_AUTO Set the Enhanced Microphone Controller to
auto mode, private state (i.e. audio input will
be only via a microphone in the Privacy
Device). In private mode, no audio input is
transmitted through the fascia microphone.
When a Privacy Device with a microphone is
deactivated (headset disconnected/handset
on-hook), the device will go to the public
state.
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO Set the Enhanced Microphone Controller to
semi-auto mode, private state (i.e. audio
input will be only via a microphone in the
Privacy Device). In private mode, no audio
input is transmitted through the fascia
microphone. When a Privacy Device with a
microphone is deactivated, the device will
remain in the private state.
fwAuxiliaries [WFS_SIU_MICROPHONEVOLUME]
Specifies whether the value of the Microphone Volume Control should be changed. If so, the
value of Microphone Volume Control is defined in an interval from 1 to 1000 where 1 is the
lowest volume level and 1000 is the highest volume level. Specified as one of the following
values:
Value Meaning
WFS_SIU_NO_CHANGE Do not change the current status of the
Microphone Volume Control.
1, ..., 1000 The volume level. This field is handled as an
unsigned short. If a value greater than 1000
is used, the provider will map the value to
1000.
fwGuidLights [...]Specifies the state of the guidance light indicator. See
WFS_INF_SIU_CAPABILITIES fwGuidLights for supported guidance light indicators and states.
Note that direction flags (Type D) cannot be specified as the field type is WORD. It is
recommended that applications use the WFS_CMD_SIU_SET_GUIDLIGHT_EX command.
lpszExtra
Pointer to a list of vendor-specific, or any other extended, information. The information is
returned as a series of “key=value” strings so that it is easily extensible by Service Providers.
Each string is null-terminated, with the final string terminating with two null characters. An empty
list may be indicated by either a NULL pointer or a pointer to two consecutive null characters.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to set a port to a new value was
invalid because the port does not exist or the
port is pre-configured as an input port.
54

CWA 16926-10:2020 (E)
WFS_ERR_SIU_SYNTAX The command was invoked with incorrect
input data.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while attempting to set or
clear one or more output ports (indicators).
Comments No action has been taken if this command returns an error. If a hardware error occurs while
executing the command, the command will return OK, but execute event(s) will be generated
which indicate(s) the port(s) which have failed.
55

CWA 16926-10:2020 (E)
5.3 WFS_CMD_SIU_SET_DOOR
Description This command is used to set the status of one of the doors.
Input Param LPWFSSIUSETDOOR lpSetDoor;
typedef struct _wfs_siu_set_door
{
WORD wDoor;
WORD fwCommand;
} WFSSIUSETDOOR, *LPWFSSIUSETDOOR;
wDoor
Specifies the index of the door to set as one of the following values:
Value Meaning
WFS_SIU_CABINET Bolt/unbolt all Cabinet Doors.
WFS_SIU_SAFE Bolt/unbolt the Safe Doors.
WFS_SIU_VANDALSHIELD Set position of the Vandal Shield.
WFS_SIU_CABINET_FRONT Bolt/unbolt all Front Cabinet Doors.
WFS_SIU_CABINET_REAR Bolt/unbolt all Rear Cabinet Doors.
WFS_SIU_CABINET_LEFT Bolt/unbolt all Left Cabinet Doors.
WFS_SIU_CABINET_RIGHT Bolt/unbolt all Right Cabinet Doors.
fwCommand
Specifies if the Cabinet or Safe doors should be bolted or unbolted or if the position of the Vandal
Shield should be changed, as one of the following flags:
Value Meaning
WFS_SIU_BOLT Bolt the Safe or Cabinet Doors.
WFS_SIU_UNBOLT Unbolt the Safe or Cabinet Doors.
WFS_SIU_OPEN Open the Vandal Shield.
WFS_SIU_SERVICE Position the Vandal Shield in service
position.
WFS_SIU_KEYBOARD Position the Vandal Shield to permit access
to the keyboard.
WFS_SIU_CLOSED Close the Vandal Shield.
See WFS_CMD_SIU_SET_PORTS command for a detailed description.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to set a port to a new value was
invalid because the port does not exist or the
port is pre-configured as an input port.
WFS_ERR_SIU_SYNTAX The command was invoked with incorrect
input data.
WFS_ERR_SIU_PORT_ERROR A hardware error occurred while executing
the command.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while attempting to set the
status of the door.
Comments None.
56

CWA 16926-10:2020 (E)
5.4 WFS_CMD_SIU_SET_INDICATOR
Description This command is used to set the status of an indicator.
Input Param LPWFSSIUSETINDICATOR lpSetIndicator;
typedef struct _wfs_siu_set_indicator
{
WORD wIndicator;
WORD fwCommand;
} WFSSIUSETINDICATOR, *LPWFSSIUSETINDICATOR;
wIndicator
Specifies the index of the indicator to set as one of the following values:
Value Meaning
WFS_SIU_OPENCLOSE Set Open/Close Indicator for the consumer.
WFS_SIU_FASCIALIGHT Turn on/off the Fascia Light.
WFS_SIU_AUDIO Turn on/off the Audio Indicator.
WFS_SIU_HEATING Turn on/off the Heating device.
WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT
Turn on/off the Consumer Display Backlight.
WFS_SIU_SIGNAGEDISPLAY Turn on/off the Signage Display device.
WFS_SIU_TRANSINDICATOR Turn on/off the Transaction Indicators.
WFS_SIU_GENERALOUTPUTPORT Turn on/off the General-Purpose Output
Ports.
fwCommand
Specifies the commands for the Open/Close Indicator, Fascia Light, Audio Indicator, Heating
device, Consumer Display Backlight, Signage Display and General-Purpose Output Ports as one
of the following flags:
Value Meaning
WFS_SIU_CLOSED The Open/Close Indicator is changed to
show that the terminal is closed for a
consumer.
WFS_SIU_OPEN The Open/Close Indicator is changed to
show that the terminal is open to be used by
a consumer.
WFS_SIU_KEYPRESS The Audio Indicator sounds a key click
signal.
WFS_SIU_EXCLAMATION The Audio Indicator sounds an exclamation
signal.
WFS_SIU_WARNING The Audio Indicator sounds a warning
signal.
WFS_SIU_ERROR The Audio Indicator sounds an error signal.
WFS_SIU_CRITICAL The Audio Indicator sounds a critical error
signal.
WFS_SIU_CONTINUOUS The Audio Indicator sound is turned on
continuously.
WFS_SIU_OFF The Audio Indicator, Fascia Light, Heating
device, Consumer Display Backlight or
Signage Display is turned off.
WFS_SIU_ON The Fascia Light, Heating device, Consumer
Display Backlight or Signage Display is
turned on.
For Transaction Indicators specifies whether the Transaction Indicators should be turned on or off.
All Transaction Indicators must be specified and each bit of this value represents one Transaction
Indicator. Each bit is specified as one of the following binary values:
Value Meaning
0 The Transaction Indicator is turned off.
1 The Transaction Indicator is turned on.
57

CWA 16926-10:2020 (E)
The following flags can be used to reference each Transaction Indicator.
Value Meaning
WFS_SIU_LAMP1 Transaction Indicator 1.
WFS_SIU_LAMP2 Transaction Indicator 2.
…
WFS_SIU_LAMP16 Transaction Indicator 16.
For General-Purpose Output Ports specifies whether the General-Purpose Output Ports should be
turned on or off. All General-Purpose Output Ports must be specified and each bit of this value
represents one General-Purpose Output Port. Each bit is specified as one of the following binary
values:
Value Meaning
0 The General-Purpose Output Port is turned
off.
1 The General-Purpose Output Port is turned
on.
The following flags can be used to reference each General-Purpose Output Port.
Value Meaning
WFS_SIU_GPP1 General-Purpose Output Port 1.
WFS_SIU_GPP2 General-Purpose Output Port 2.
…
WFS_SIU_GPP16 General-Purpose Output Port 16.
See WFS_CMD_SIU_SET_PORTS command for a detailed description.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to set a port to a new value was
invalid because the port does not exist or the
port is pre-configured as an input port.
WFS_ERR_SIU_SYNTAX The command was invoked with incorrect
input data.
WFS_ERR_SIU_PORT_ERROR A hardware error occurred while executing
the command.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while attempting to set the
status of the indicator.
Comments None.
58

CWA 16926-10:2020 (E)
5.5 WFS_CMD_SIU_SET_AUXILIARY
Description This command is used to set the status of an auxiliary indicator.
Input Param LPWFSSIUSETAUXILIARY lpSetAuxiliary;
typedef struct _wfs_siu_set_auxiliary
{
WORD wAuxiliary;
WORD fwCommand;
} WFSSIUSETAUXILIARY, *LPWFSSIUSETAUXILIARY;
wAuxiliary
Specifies the index of the auxiliary indicator to set as one of the following values:
Value Meaning
WFS_SIU_VOLUME Set the value of the Volume Control.
WFS_SIU_UPS Set the value of the UPS.
WFS_SIU_REMOTE_STATUS_MONITOR Set the value of the Remote Status Monitor.
WFS_SIU_AUDIBLE_ALARM Set the value of the Audible Alarm.
WFS_SIU_ENHANCEDAUDIOCONTROL Set the value of the Enhanced Audio
Controller.
WFS_SIU_ENHANCEDMICROPHONECONTROL Set the value of the Enhanced
Microphone Controller.
WFS_SIU_MICROPHONEVOLUME Set the value of the Microphone Volume
Control.
fwCommand
It specifies the values for the auxiliary specified by wAuxiliary. Specified as one of the following
values:
Value Meaning
1, ..., 1000 The volume level. This field is handled as an
unsigned short. If a value greater than 1000
is used, the provider will map the value to
1000.
WFS_SIU_ENGAGE Engage the UPS.
WFS_SIU_DISENGAGE Disengage the UPS.
WFS_SIU_GREEN_LED_ON Turn on the green LED on the Remote Status
Monitor.
WFS_SIU_GREEN_LED_OFF Turn off the green LED on the Remote
Status Monitor.
WFS_SIU_AMBER_LED_ON Turn on the amber LED on the Remote
Status Monitor.
WFS_SIU_AMBER_LED_OFF Turn off the amber LED on the Remote
Status Monitor.
WFS_SIU_RED_LED_ON Turn on the red LED on the Remote Status
Monitor.
WFS_SIU_RED_LED_OFF Turn off the red LED on the Remote Status
Monitor.
WFS_SIU_OFF Turn off the Audible Alarm.
WFS_SIU_ON Turn on the Audible Alarm.
WFS_SIU_PUBLICAUDIO_MANUAL Set the Enhanced Audio or Microphone
Controller to manual mode, public state (i.e.
audio will be played through speakers only).
WFS_SIU_PUBLICAUDIO_AUTO Set the Enhanced Audio or Microphone
Controller to auto mode, public state (i.e.
audio will be played through speakers).
When a Privacy Device is activated (headset
connected/handset off-hook), the device will
go to the private state.
59

CWA 16926-10:2020 (E)
WFS_SIU_PUBLICAUDIO_SEMI_AUTO Set the Enhanced Audio or Microphone
Controller to semi-auto mode, public state
(i.e. audio will be played through speakers).
When a Privacy Device is activated, the
device will go to the private state.
WFS_SIU_PRIVATEAUDIO_MANUAL Set the Enhanced Audio or Microphone
Controller to manual mode, private state (i.e.
audio will be played only through a
connected Privacy Device). In private mode,
no audio is transmitted through the speakers.
WFS_SIU_PRIVATEAUDIO_AUTO Set the Enhanced Audio or Microphone
Controller to auto mode, private state (i.e.
audio will be played only through an
activated Privacy Device). In private mode,
no audio is transmitted through the speakers.
When a Privacy Device is deactivated
(headset disconnected/handset on-hook), the
device will go to the public state.
WFS_SIU_PRIVATEAUDIO_SEMI_AUTO Set the Enhanced Audio or Microphone
Controller to semi-auto mode, private state
(i.e. audio will be played only through an
activated Privacy Device). In private mode,
no audio is transmitted through the speakers.
When a Privacy Device is deactivated, the
device will remain in the private state.
See WFS_CMD_SIU_SET_PORTS command for a detailed description.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to set a port to a new value was
invalid because the port does not exist or the
port is pre-configured as an input port.
WFS_ERR_SIU_SYNTAX The command was invoked with incorrect
input data.
WFS_ERR_SIU_PORT_ERROR A hardware error occurred while executing
the command.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while attempting to set the
status of the auxiliary indicator.
Comments When wAuxiliary is any value other than WFS_SIU_REMOTE_STATUS_MONITOR the
fwCommand parameter should contain one of the values that correspond to the auxiliary defined in
wAuxiliary.
When wAuxiliary is WFS_SIU_REMOTE_STATUS_MONITOR then the fwCommand parameter
may be specified as a combination of one or more of the following flags of type A, B and C, with
at most one flag from each type.
Value Meaning Type
WFS_SIU_GREEN_LED_ON Turn on the green LED on the A
Remote Status Monitor device.
WFS_SIU_GREEN_LED_OFF Turn off the green LED on the A
Remote Status Monitor device.
WFS_SIU_AMBER_LED_ON Turn on the amber LED on the B
Remote Status Monitor device.
60

CWA 16926-10:2020 (E)

| WFS_SIU_AMBER_LED_OFF  | Turn off the amber LED on the   | B   |
| ---------------------- | ------------------------------- | --- |
Remote Status Monitor device.
| WFS_SIU_RED_LED_ON  | Turn on the red LED on the   | C   |
| ------------------- | ---------------------------- | --- |
Remote Status Monitor device.
| WFS_SIU_RED_LED_OFF  | Turn off the red LED on the   | C   |
| -------------------- | ----------------------------- | --- |
Remote Status Monitor device.
61

CWA 16926-10:2020 (E)
5.6 WFS_CMD_SIU_SET_GUIDLIGHT
Description This command is used to set the status of a guidance light indicator. This command has been
retained for backwards compatibility only. It is recommended that applications use the
WFS_CMD_SIU_SET_GUIDLIGHT_EX command.
Input Param LPWFSSIUSETGUIDLIGHT lpSetGuidLight;
typedef struct _wfs_siu_set_guidlight
{
WORD wGuidLight;
WORD fwCommand;
} WFSSIUSETGUIDLIGHT, *LPWFSSIUSETGUIDLIGHT;
wGuidLight
Specifies the identifier of the guidance light indicator to set. The maximum guidance light
identifier supported by this command is WFS_SIU_GUIDLIGHTS_MAX. See
WFS_INF_SIU_CAPABILITIES fwGuidLights for supported guidance light indicator identifiers.
fwCommand
Specifies the state of the guidance light indicator. This command only supports flash rate (type B)
commands. See WFS_INF_SIU_CAPABILITIES fwGuidLights for supported flash rates.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to set a port to a new value was
invalid because the port does not exist or the
port is pre-configured as an input port.
WFS_ERR_SIU_SYNTAX The command was invoked with incorrect
input data.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while attempting to set or
clear one or more output ports (indicators).
Comments The slow and medium flash rates must not be greater than 2.0 Hz. It should be noted that in order
to comply with American Disabilities Act guidelines only a slow or medium flash rate must be
used.
62

CWA 16926-10:2020 (E)
5.7 WFS_CMD_SIU_RESET
Description This command is used by the application to perform a hardware reset which will attempt to return
the SIU devices to a known good state. This command does not over-ride a lock obtained on
another application or service handle.
Input Param None.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_PORT_ERROR A hardware error occurred while executing
the command.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while attempting to set or
clear one or more output ports (indicators).
Comments None.
63

CWA 16926-10:2020 (E)
5.8 WFS_CMD_SIU_POWER_SAVE_CONTROL
Description This command activates or deactivates the power-saving mode.
If the Service Provider receives another execute command while in power saving mode the
Service Provider automatically exits the power saving mode and executes the requested command.
If the Service Provider receives an information command while in power saving mode, the Service
Provider will not exit the power saving mode.
The SIU class power saving control covers hardware such as consumer display, transaction
indicators, fans, etc. The actual hardware covered by the SIU power saving control is device and
vendor dependent and configuration options may be offered by the vendor. The SIU class power
saving control does not cover hardware supported by other XFS device classes.
Input Param LPWFSSIUPOWERSAVECONTROL lpPowerSaveControl;
typedef struct _wfs_siu_power_save_control
{
USHORT usMaxPowerSaveRecoveryTime;
} WFSSIUPOWERSAVECONTROL, *LPWFSSIUPOWERSAVECONTROL;
usMaxPowerSaveRecoveryTime
Specifies the maximum number of seconds in which the device must be able to return to its normal
operating state when exiting power save mode. The device will be set to the highest possible
power save mode within this constraint. If usMaxPowerSaveRecoveryTime is set to zero then the
device will exit the power saving mode.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_POWERSAVETOOSHORT The power saving mode has not been
activated because the device is not able to
resume from the power saving mode within
the specified
usMaxPowerSaveRecoveryTime value.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_SRVE_SIU_POWER_SAVE_CHANGE The power save recovery time has changed.
Comments None.
64

CWA 16926-10:2020 (E)
5.9 WFS_CMD_SIU_SET_AUTOSTARTUP_TIME
Description This command is used to set the time at which the machine will automatically start. It is also used
to disable automatic start-up.
If a new start-up time is set by this command it will replace any previously set start-up time.
Before the auto start-up can take place the operating system must be shut down.
Input Param LPWFSSIUSETSTARTUPTIME lpSetStartupTime;
typedef struct _wfs_siu_set_startup_time
{
WORD wMode;
LPSYSTEMTIME lpStartTime;
} WFSSIUSETSTARTUPTIME, *LPWFSSIUSETSTARTUPTIME;
wMode
Specifies the mode of the auto start-up control as one of the following flags.
Value Meaning
WFS_SIU_AUTOSTARTUP_CLEAR Disables the previously set automatic start-up
time. The lpStartTime input parameter is
ignored if this mode is set.
WFS_SIU_AUTOSTARTUP_SPECIFIC One-time auto start-up at a specific time on a
specific date. In the lpStartTime parameter,
wYear, wMonth, wDay, wHour and wMinute
must be set and all other fields are ignored.
WFS_SIU_AUTOSTARTUP_DAILY Auto start-up at the same time every day. In
the lpStartTime parameter, wHour and
wMinute must be set and all other fields are
ignored.
WFS_SIU_AUTOSTARTUP_WEEKLY Auto start-up at a specified time on a specific
day of every week. In the lpStartTime
parameter, wDayOfWeek, wHour and
wMinute must be set and all other fields are
ignored.
lpStartTime
Specifies the auto start-up time.
Win32 SYSTEMTIME structure:
wYear
Specifies the year. This value is ignored if it is not relevant to the wMode value.
wMonth
Specifies the month. This value is ignored if it is not relevant to the wMode value.
wDayOfWeek
Specifies the day of the week, in values from 0 (Sunday) to 6 (Saturday). This value is ignored
if it is not relevant to the wMode value.
wDay
Specifies the day of the month. This value is ignored if it is not relevant to the wMode value.
wHour
Specifies the hour. This value is ignored if it is not relevant to the wMode value.
wMinute
Specifies the minute. This value is ignored if it is not relevant to the wMode value.
wSecond
This field is not used and must be zero.
wMilliseconds
This field is not used and must be zero.
65

CWA 16926-10:2020 (E)
Output Param None.
Error Codes Only the generic error codes defined in [Ref. 1] can be generated by this command.
Events None.
Comments None.
66

CWA 16926-10:2020 (E)
5.10 WFS_CMD_SIU_SYNCHRONIZE_COMMAND
Description This command is used to reduce response time of a command (e.g. for synchronization with
display) as well as to synchronize actions of the different device classes. This command is
intended to be used only on hardware which is capable of synchronizing functionality within a
single device class or with other device classes.
The list of execute commands which this command supports for synchronization is retrieved in the
lpdwSynchronizableCommands parameter of the WFS_INF_SIU_CAPABILITIES.
This command is optional, i.e. any other command can be called without having to call it in
advance. Any preparation that occurs by calling this command will not affect any other subsequent
command. However, any subsequent execute command other than the one that was specified in the
dwCommand input parameter will execute normally and may invalidate the pending
synchronization. In this case the application should call the
WFS_CMD_SIU_SYNCHRONIZE_COMMAND again in order to start a synchronization.
Input Param LPWFSSIUSYNCHRONIZECOMMAND lpSynchronizeCommand;
typedef struct _wfs_siu_synchronize_command
{
DWORD dwCommand;
LPVOID lpCmdData;
} WFSSIUSYNCHRONIZECOMMAND, *LPWFSSIUSYNCHRONIZECOMMAND;
dwCommand
The command ID of the command to be synchronized and executed next.
lpCmdData
Pointer to data or a data structure that represents the parameter that is normally associated with the
command that is specified in dwCommand. For example, if dwCommand is
WFS_CMD_SIU_SET_INDICATOR then lpCmdData will point to a WFSSIUSETINDICATOR
structure. This parameter can be NULL if no command input parameter is needed or if this detail
is not needed to synchronize for the command.
It will be device-dependent whether the synchronization is effective or not in the case where the
application synchronizes for a command with this command specifying a parameter but
subsequently executes the synchronized command with a different parameter. This case should not
result in an error; however, the preparation effect could be different from what the application
expects. The application should, therefore, make sure to use the same parameter between
lpCmdData of this command and the subsequent corresponding execute command.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_COMMANDUNSUPP The command specified in the dwCommand
field is not supported by the Service
Provider.
WFS_ERR_SIU_SYNCHRONIZEUNSUPP The preparation for the command specified
in the dwCommand with the parameter
specified in the lpCmdData is not supported
by the Service Provider.
Events Only the generic events defined in [Ref. 1] can be generated by this command.
Comments For sample flows of this synchronization see the [Ref 1] Appendix C.
67

CWA 16926-10:2020 (E)
5.11 WFS_CMD_SIU_SET_GUIDLIGHT_EX
Description This command is used to set the status of a guidance light indicator.
Input Param LPWFSSIUSETGUIDLIGHTEX lpSetGuidLightEx;
typedef struct _wfs_siu_set_guidlightex
{
WORD wGuidLight;
DWORD fwCommand;
} WFSSIUSETGUIDLIGHTEX, *LPWFSSIUSETGUIDLIGHTEX;
wGuidLight
Specifies the identifier of the guidance light indicator to set. See WFS_INF_SIU_CAPABILITIES
fwGuidLightsEx for supported guidance light indicator identifiers.
fwCommand
Specifies the state of the guidance light indicator. See WFS_INF_SIU_CAPABILITIES
fwGuidLightsEx for supported states.
Output Param None.
Error Codes In addition to the generic error codes defined in [Ref. 1], the following error codes can be
generated by this command:
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to set a port to a new value was
invalid because the port does not exist or the
port is pre-configured as an input port.
WFS_ERR_SIU_SYNTAX The command was invoked with incorrect
input data.
Events In addition to the generic events defined in [Ref. 1], the following events can be generated by this
command:
Value Meaning
WFS_EXEE_SIU_PORT_ERROR An error occurred while attempting to set or
clear one or more output ports (indicators).
Comments The slow and medium flash rates must not be greater than 2.0 Hz. It should be noted that in order
to comply with American Disabilities Act guidelines only a slow or medium flash rate must be
used.
68

CWA 16926-10:2020 (E)
6. Events
6.1 WFS_SRVE_SIU_PORT_STATUS
Description This event id is used to specify that a port has changed its state, due to the result of a command or
to some external condition. Reporting of this event is controlled by the
WFS_CMD_SIU_ENABLE_EVENTS command. Event reporting is disabled as a default
situation.
Event Param LPWFSSIUPORTEVENT lpPortEvent;
typedef struct _wfs_siu_port_event
{
WORD wPortType;
WORD wPortIndex;
WORD wPortStatus;
LPSTR lpszExtra;
DWORD dwPortStatus;
} WFSSIUPORTEVENT, *LPWFSSIUPORTEVENT;
wPortType
Specifies the type of sensors and indicators that has changed state by one of the following flags:
Value Meaning
WFS_SIU_SENSORS A port in the input sensors has changed state.
WFS_SIU_DOORS A port in the door sensors has changed state.
WFS_SIU_INDICATORS A port in the indicators has changed state.
WFS_SIU_AUXILIARIES A port in the auxiliary indicators has
changed state.
WFS_SIU_GUIDLIGHTS A port in the guidance lights has changed
state.
wPortIndex
Specifies the index of the port that has changed state by one of the following values:
Value Meaning
WFS_SIU_OPERATORSWITCH The Operator Switch has changed its state.
WFS_SIU_TAMPER The Tamper Sensor has changed its state.
WFS_SIU_INTTAMPER The Internal Tamper Sensor has changed its
state.
WFS_SIU_SEISMIC The Seismic Sensor has changed its state.
WFS_SIU_HEAT The Heat Sensor has changed its state.
WFS_SIU_PROXIMITY The Proximity Sensor has changed its state.
WFS_SIU_AMBLIGHT The Ambient Light Sensor has changed its
state.
WFS_SIU_ENHANCEDAUDIO The Audio Jack has changed its state (a
headset has been plugged-in or removed).
WFS_SIU_BOOT_SWITCH The Boot Switch Sensor has changed its
state.
WFS_SIU_CONSUMER_DISPLAY The Consumer Display Sensor has changed
its state.
WFS_SIU_OPERATOR_CALL_BUTTON The Operator Call Button has changed its
state.
WFS_SIU_HANDSETSENSOR The Handset Sensor has changed its state.
WFS_SIU_GENERALINPUTPORT At least one of the General-Purpose Input
Ports has changed its state. The status should
be checked to determine which General-
Purpose Input Port has changed its state.
WFS_SIU_HEADSETMICROPHONE The Microphone Jack has changed its state
(a headset microphone has been plugged-in
or removed).
WFS_SIU_CABINET The Cabinet Doors have changed their state.
69

CWA 16926-10:2020 (E)
WFS_SIU_SAFE The Safe Doors have changed their state.
WFS_SIU_VANDALSHIELD The Vandal Shield has changed its state.
WFS_SIU_CABINET_FRONT The Front Cabinet Doors have changed their
state.
WFS_SIU_CABINET_REAR The Rear Cabinet Doors have changed their
state.
WFS_SIU_CABINET_LEFT The Left Cabinet Doors have changed their
state.
WFS_SIU_CABINET_RIGHT The Right Cabinet Doors have changed their
state.
WFS_SIU_OPENCLOSE The Open/Close Indicator state has changed.
WFS_SIU_FASCIALIGHT The Fascia Light state has changed.
WFS_SIU_AUDIO The Audio Indicator state has changed.
WFS_SIU_HEATING The Heating device state has changed.
WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT
The Consumer Display Backlight state has
changed.
WFS_SIU_SIGNAGEDISPLAY The Signage Display device state has
changed.
WFS_SIU_TRANSINDICATOR At least one of the Transaction Indicators has
changed its value. Use the
WFS_INF_SIU_STATUS command to
determine which Transaction Indicators have
changed.
WFS_SIU_GENERALOUTPUTPORT At least one of the General-Purpose Output
Ports has changed its state. Use the
WFS_INF_SIU_STATUS command to
determine which General-Purpose Output
Ports have changed.
WFS_SIU_VOLUME The Volume Control device has changed its
value.
WFS_SIU_UPS The UPS device state has changed.
WFS_SIU_REMOTE_STATUS_MONITOR The Remote Status Monitor device state has
changed.
WFS_SIU_AUDIBLE_ALARM The Audible Alarm device state has changed.
WFS_SIU_ENHANCEDAUDIOCONTROL The Enhanced Audio Controller has changed
state.
WFS_SIU_ENHANCEDMICROPHONECONTROL The Enhanced Microphone
Controller has changed state.
WFS_SIU_MICROPHONEVOLUME The Microphone Volume Control device has
changed its value.
WFS_SIU_CARDUNIT The Guidance Light Indicator state for the
card unit has changed.
WFS_SIU_PINPAD The Guidance Light Indicator state for the
PIN pad unit has changed.
WFS_SIU_NOTESDISPENSER The Guidance Light Indicator state for the
note dispenser unit has changed.
WFS_SIU_COINDISPENSER The Guidance Light Indicator state for the
coin dispenser unit has changed.
WFS_SIU_RECEIPTPRINTER The Guidance Light Indicator state for the
receipt printer unit has changed.
WFS_SIU_PASSBOOKPRINTER The Guidance Light Indicator state for the
passbook printer unit has changed.
WFS_SIU_ENVDEPOSITORY The Guidance Light Indicator state for the
envelope depository unit has changed.
WFS_SIU_CHEQUEUNIT The Guidance Light Indicator state for the
cheque unit has changed.
WFS_SIU_BILLACCEPTOR The Guidance Light Indicator state for the
bill acceptor unit has changed.
70

CWA 16926-10:2020 (E)
WFS_SIU_ENVDISPENSER The Guidance Light Indicator state for the
envelope dispenser unit has changed.
WFS_SIU_DOCUMENTPRINTER The Guidance Light Indicator state for the
Document Printer unit has changed.
WFS_SIU_COINACCEPTOR The Guidance Light Indicator state for the
coin acceptor has changed.
WFS_SIU_SCANNER The Guidance Light Indicator state for the
scanner has changed.
WFS_SIU_CONTACTLESS The Guidance Light Indicator state for the
Contactless Reader unit has changed.
WFS_SIU_CARDUNIT2 The Guidance Light Indicator state for the
second card unit has changed.
WFS_SIU_NOTESDISPENSER2 The Guidance Light Indicator state for the
second notes dispenser has changed.
WFS_SIU_BILLACCEPTOR2 The Guidance Light Indicator state for the second bill acceptor
has changed.wPortStatus
Specifies the new state of the port indicated by wPortType and wPortIndex. See the
WFS_INF_SIU_STATUS information command for the possible values.
lpszExtra
Pointer to a list of vendor-specific, or any other extended, information. The information is
returned as a series of “key=value” strings so that it is easily extensible by Service Providers.
Each string is null-terminated, with the final string terminating with two null characters. An empty
list may be indicated by either a NULL pointer or a pointer to two consecutive null characters.
dwPortStatus
Specifies the new state of the port indicated by wPortType and wPortIndex. See the
WFS_INF_SIU_STATUS information command for the possible values. This field allows
additional values/flags to be reported which cannot be reported by wPortStatus as its type is
WORD. Applications are recommended to use this field.
Comments None.
71

CWA 16926-10:2020 (E)
6.2 WFS_EXEE_SIU_PORT_ERROR
Description This event is used to specify that a port has detected an error.
Event Param LPWFSSIUPORTERROR lpPortError;
typedef struct _wfs_siu_port_error
{
WORD wPortType;
WORD wPortIndex;
HRESULT PortError;
WORD wPortStatus;
LPSTR lpszExtra;
DWORD dwPortStatus;
} WFSSIUPORTERROR, *LPWFSSIUPORTERROR;
wPortType
Specifies the type of sensors and indicators that has detected an error by one of the following
flags:
Value Meaning
WFS_SIU_SENSORS A port in the input sensors has detected an
error.
WFS_SIU_DOORS A port in the door sensors has detected an
error.
WFS_SIU_INDICATORS A port in the indicators has detected an error.
WFS_SIU_AUXILIARIES A port in the auxiliary Indicators has
detected an error.
WFS_SIU_GUIDLIGHTS A port in the guidance lights has detected an
error.
wPortIndex
Specifies the index of the port that has detected an error by one of the following values:
Value Meaning
WFS_SIU_OPERATORSWITCH The Operator Switch has detected an error.
WFS_SIU_TAMPER The Tamper Sensor has detected an error.
WFS_SIU_INTTAMPER The internal Tamper Sensor has detected an
error.
WFS_SIU_SEISMIC The Seismic Sensor has detected an error.
WFS_SIU_HEAT The Heat Sensor has detected an error.
WFS_SIU_PROXIMITY The Proximity Sensor has detected an error.
WFS_SIU_AMBLIGHT The Ambient Light Sensor has detected an
error.
WFS_SIU_ENHANCEDAUDIO The Audio Jack Sensor has detected an error.
WFS_SIU_BOOT_SWITCH The Boot Switch Sensor has detected an
error.
WFS_SIU_CONSUMER_DISPLAY The Consumer Display has detected an error.
WFS_SIU_OPERATOR_CALL_BUTTON The Operator Call Button has detected an
error.
WFS_SIU_HANDSETSENSOR The Handset Sensor has detected an error.
WFS_SIU_GENERALINPUTPORT The General-Purpose Input Port has detected
an error.
WFS_SIU_HEADSETMICROPHONE The Microphone Jack Sensor has detected an
error.
WFS_SIU_CABINET The Cabinet Doors have detected an error.
WFS_SIU_SAFE The Safe Doors have detected an error.
WFS_SIU_VANDALSHIELD The Vandal Shield has detected an error.
WFS_SIU_CABINET_FRONT The Front Cabinet Doors have detected an
error.
WFS_SIU_CABINET_REAR The Rear Cabinet Doors have detected an
error.
72

CWA 16926-10:2020 (E)
WFS_SIU_CABINET_LEFT The Left Cabinet Doors have detected an
error.
WFS_SIU_CABINET_RIGHT The Right Cabinet Doors have detected an
error.
WFS_SIU_OPENCLOSE The Open/Close Indicator has detected an
error.
WFS_SIU_FASCIALIGHT The Fascia Light state has detected an error.
WFS_SIU_AUDIO The Audio Indicator state has detected an
error.
WFS_SIU_HEATING The Heating device state has detected an
error.
WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT
The Consumer Display Backlight state has
detected an error.
WFS_SIU_SIGNAGEDISPLAY The Signage Display device state has
detected an error.
WFS_SIU_TRANSINDICATOR The Transaction Indicator state has detected
an error.
WFS_SIU_GENERALOUTPUTPORT The General-Purpose Output Port has
detected an error.
WFS_SIU_VOLUME The Volume Control device has detected an
error.
WFS_SIU_UPS The UPS device has detected an error.
WFS_SIU_REMOTE_STATUS_MONITOR The Remote Status Monitor device has
detected an error.
WFS_SIU_AUDIBLE_ALARM The Audible Alarm device has detected an
error.
WFS_SIU_ENHANCEDAUDIOCONTROL The Enhanced Audio Controller has detected
an error.
WFS_SIU_ENHANCEDMICROPHONECONTROL The Enhanced Microphone
Controller has detected an error.
WFS_SIU_MICROPHONEVOLUME The Microphone Volume Control device has
detected an error.
WFS_SIU_CARDUNIT The Guidance Light Indicator for the card
unit has detected an error.
WFS_SIU_PINPAD The Guidance Light Indicator for the PIN
pad unit has detected an error.
WFS_SIU_NOTESDISPENSER The Guidance Light Indicator for the note
dispenser unit has detected an error.
WFS_SIU_COINDISPENSER The Guidance Light Indicator for the coin
dispenser unit has detected an error.
WFS_SIU_RECEIPTPRINTER The Guidance Light Indicator for the receipt
printer unit has detected an error.
WFS_SIU_PASSBOOKPRINTER The Guidance Light Indicator for the
passbook printer unit has detected an error.
WFS_SIU_ENVDEPOSITORY The Guidance Light Indicator for the
envelope depository unit has detected an
error.
WFS_SIU_CHEQUEUNIT The Guidance Light Indicator for the cheque
unit has detected an error.
WFS_SIU_BILLACCEPTOR The Guidance Light Indicator for the bill
acceptor unit has detected an error.
WFS_SIU_ENVDISPENSER The Guidance Light Indicator for the
envelope dispenser unit has detected an
error.
WFS_SIU_DOCUMENTPRINTER The Guidance Light Indicator for the
document printer has detected an error.
73

CWA 16926-10:2020 (E)
WFS_SIU_COINACCEPTOR The Guidance Light Indicator for the coin
acceptor has detected an error.
WFS_SIU_SCANNER The Guidance Light Indicator for the scanner
has detected an error.
WFS_SIU_CONTACTLESS The Guidance Light Indicator state for the
Contactless Reader unit has changed.
WFS_SIU_CARDUNIT2 The Guidance Light Indicator state for the
second card unit has changed.
WFS_SIU_NOTESDISPENSER2 The Guidance Light Indicator state for the
second notes dispenser has changed.
WFS_SIU_BILLACCEPTOR2 The Guidance Light Indicator state for the
second bill acceptor has changed.
PortError
Specifies the error of the port indicated in the wPortType and wPortIndex by one of the following
values:
Value Meaning
WFS_ERR_SIU_INVALID_PORT An attempt to enable or disable events to a
port was invalid because the port does not
exist.
WFS_ERR_SIU_SYNTAX Syntax error in the input parameters. E.g. an
attempt to both enable and disable events to
the same port was made.
WFS_ERR_SIU_PORT_ERROR A hardware error occurred while executing a
command.
wPortStatus
Specifies the new state of the port indicated by wPortType and wPortIndex. See the
WFS_INF_SIU_STATUS information command for the possible values.
lpszExtra
Pointer to a list of vendor-specific, or any other extended, information. The information is
returned as a series of “key=value” strings so that it is easily extensible by Service Providers.
Each string is null-terminated, with the final string terminating with two null characters. An empty
list may be indicated by either a NULL pointer or a pointer to two consecutive null characters.
dwPortStatus
Specifies the new state of the port indicated in the wPortType and wPortIndex. See the
WFS_INF_SIU_STATUS information command for the possible values. This field allows
additional values/flags to be reported which cannot be reported by wPortStatus as its type is
WORD. Applications are recommended to use this field.
Comments None.
74

CWA 16926-10:2020 (E)
6.3 WFS_SRVE_SIU_POWER_SAVE_CHANGE
Description This service event specifies that the power save recovery time has changed.
Event Param LPWFSSIUPOWERSAVECHANGE lpPowerSaveChange;
typedef struct _wfs_siu_power_save_change
{
USHORT usPowerSaveRecoveryTime;
} WFSSIUPOWERSAVECHANGE, *LPWFSSIUPOWERSAVECHANGE;
usPowerSaveRecoveryTime
Specifies the actual number of seconds required by the device to resume its normal operational
state. This value is zero if the device exited the power saving mode.
Comments None.
75

CWA 16926-10:2020 (E)
7. C - Header file
/******************************************************************************
* *
* xfssiu.h XFS - Sensors and Indicators Unit (SIU) definitions *
* *
* Version 3.40 (December 6 2019) *
* *
******************************************************************************/
#ifndef __INC_XFSSIU__H
#define __INC_XFSSIU__H
#ifdef __cplusplus
extern "C" {
#endif
#include <xfsapi.h>
/* be aware of alignment */
#pragma pack (push, 1)
/* values of WFSSIUCAPS.wClass */
#define WFS_SERVICE_CLASS_SIU (8)
#define WFS_SERVICE_CLASS_NAME_SIU "SIU"
#define WFS_SERVICE_CLASS_VERSION_SIU (0x2803) /* Version 3.40 */
#define SIU_SERVICE_OFFSET (WFS_SERVICE_CLASS_SIU * 100)
/* SIU Info Commands */
#define WFS_INF_SIU_STATUS (SIU_SERVICE_OFFSET + 1)
#define WFS_INF_SIU_CAPABILITIES (SIU_SERVICE_OFFSET + 2)
#define WFS_INF_SIU_GET_AUTOSTARTUP_TIME (SIU_SERVICE_OFFSET + 3)
/* SIU Command Verbs */
#define WFS_CMD_SIU_ENABLE_EVENTS (SIU_SERVICE_OFFSET + 1)
#define WFS_CMD_SIU_SET_PORTS (SIU_SERVICE_OFFSET + 2)
#define WFS_CMD_SIU_SET_DOOR (SIU_SERVICE_OFFSET + 3)
#define WFS_CMD_SIU_SET_INDICATOR (SIU_SERVICE_OFFSET + 4)
#define WFS_CMD_SIU_SET_AUXILIARY (SIU_SERVICE_OFFSET + 5)
#define WFS_CMD_SIU_SET_GUIDLIGHT (SIU_SERVICE_OFFSET + 6)
#define WFS_CMD_SIU_RESET (SIU_SERVICE_OFFSET + 7)
#define WFS_CMD_SIU_POWER_SAVE_CONTROL (SIU_SERVICE_OFFSET + 8)
#define WFS_CMD_SIU_SET_AUTOSTARTUP_TIME (SIU_SERVICE_OFFSET + 9)
#define WFS_CMD_SIU_SYNCHRONIZE_COMMAND (SIU_SERVICE_OFFSET + 10)
/* SIU Messages */
#define WFS_SRVE_SIU_PORT_STATUS (SIU_SERVICE_OFFSET + 1)
#define WFS_EXEE_SIU_PORT_ERROR (SIU_SERVICE_OFFSET + 2)
#define WFS_SRVE_SIU_POWER_SAVE_CHANGE (SIU_SERVICE_OFFSET + 3)
/* Values of WFSSIUSTATUS.fwDevice */
#define WFS_SIU_DEVONLINE WFS_STAT_DEVONLINE
#define WFS_SIU_DEVOFFLINE WFS_STAT_DEVOFFLINE
#define WFS_SIU_DEVPOWEROFF WFS_STAT_DEVPOWEROFF
#define WFS_SIU_DEVNODEVICE WFS_STAT_DEVNODEVICE
#define WFS_SIU_DEVHWERROR WFS_STAT_DEVHWERROR
#define WFS_SIU_DEVUSERERROR WFS_STAT_DEVUSERERROR
#define WFS_SIU_DEVBUSY WFS_STAT_DEVBUSY
#define WFS_SIU_DEVFRAUDATTEMPT WFS_STAT_DEVFRAUDATTEMPT
#define WFS_SIU_DEVPOTENTIALFRAUD WFS_STAT_DEVPOTENTIALFRAUD
76

CWA 16926-10:2020 (E)
/* Size and max index of fwSensors array */
#define WFS_SIU_SENSORS_SIZE (32)
#define WFS_SIU_SENSORS_MAX (WFS_SIU_SENSORS_SIZE - 1)
/* Size and max index of fwDoors array */
#define WFS_SIU_DOORS_SIZE (16)
#define WFS_SIU_DOORS_MAX (WFS_SIU_DOORS_SIZE - 1)
/* Size and max index of fwIndicators array */
#define WFS_SIU_INDICATORS_SIZE (16)
#define WFS_SIU_INDICATORS_MAX (WFS_SIU_INDICATORS_SIZE - 1)
/* Size max index of fwAuxiliaries array */
#define WFS_SIU_AUXILIARIES_SIZE (16)
#define WFS_SIU_AUXILIARIES_MAX (WFS_SIU_AUXILIARIES_SIZE - 1)
/* Size and max index of fwGuidLights array */
#define WFS_SIU_GUIDLIGHTS_SIZE (16)
#define WFS_SIU_GUIDLIGHTS_MAX (WFS_SIU_GUIDLIGHTS_SIZE - 1)
/* Size and max index of fwGuidLightsEx array */
#define WFS_SIU_GUIDLIGHTS_SIZE_EX (32)
#define WFS_SIU_GUIDLIGHTS_MAX_EX (WFS_SIU_GUIDLIGHTS_SIZE_EX - 1)
/* Indices of WFSSIUSTATUS.fwSensors [...]
WFSSIUCAPS.fwSensors [...]
WFSSIUENABLE.fwSensors [...]
WFSSIUPORTEVENT.wPortIndex
WFSSIUPORTERROR.wPortIndex */
#define WFS_SIU_OPERATORSWITCH (0)
#define WFS_SIU_TAMPER (1)
#define WFS_SIU_INTTAMPER (2)
#define WFS_SIU_SEISMIC (3)
#define WFS_SIU_HEAT (4)
#define WFS_SIU_PROXIMITY (5)
#define WFS_SIU_AMBLIGHT (6)
#define WFS_SIU_ENHANCEDAUDIO (7)
#define WFS_SIU_BOOT_SWITCH (8)
#define WFS_SIU_CONSUMER_DISPLAY (9)
#define WFS_SIU_OPERATOR_CALL_BUTTON (10)
#define WFS_SIU_HANDSETSENSOR (11)
#define WFS_SIU_GENERALINPUTPORT (12)
#define WFS_SIU_HEADSETMICROPHONE (13)
#define WFS_SIU_FASCIAMICROPHONE (14)
/* Indices of WFSSIUSTATUS.fwDoors [...]
WFSSIUCAPS.fwDoors [...]
WFSSIUENABLE.fwDoors [...]
WFSSIUSETPORTS.fwDoors [...]
WFSSIUSETDOOR.wDoor
WFSSIUPORTEVENT.wPortIndex
WFSSIUPORTERROR.wPortIndex */
#define WFS_SIU_CABINET (0)
#define WFS_SIU_SAFE (1)
#define WFS_SIU_VANDALSHIELD (2)
#define WFS_SIU_CABINET_FRONT (3)
#define WFS_SIU_CABINET_REAR (4)
#define WFS_SIU_CABINET_LEFT (5)
#define WFS_SIU_CABINET_RIGHT (6)
77

CWA 16926-10:2020 (E)
/* Indices of WFSSIUSTATUS.fwIndicators [...]
WFSSIUCAPS.fwIndicators [...]
WFSSIUENABLE.fwIndicators [...]
WFSSIUSETPORTS.wIndicators [...]
WFSSIUSETINDICATOR.wIndicator
WFSSIUPORTEVENT.wPortIndex
WFSSIUPORTERROR.wPortIndex */
#define WFS_SIU_OPENCLOSE (0)
#define WFS_SIU_FASCIALIGHT (1)
#define WFS_SIU_AUDIO (2)
#define WFS_SIU_HEATING (3)
#define WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT (4)
#define WFS_SIU_SIGNAGEDISPLAY (5)
#define WFS_SIU_TRANSINDICATOR (6)
#define WFS_SIU_GENERALOUTPUTPORT (7)
/* Indices of WFSSIUSTATUS.fwAuxiliaries [...]
WFSSIUCAPS.fwAuxiliaries [...]
WFSSIUENABLE.fwAuxiliaries [...]
WFSSIUSETPORTS.wAuxiliaries [...]
WFSSIUSETAUXILIARY.wAuxiliary
WFSSIUPORTEVENT.wPortIndex
WFSSIUPORTERROR.wPortIndex */
#define WFS_SIU_VOLUME (0)
#define WFS_SIU_UPS (1)
#define WFS_SIU_REMOTE_STATUS_MONITOR (2)
#define WFS_SIU_AUDIBLE_ALARM (3)
#define WFS_SIU_ENHANCEDAUDIOCONTROL (4)
#define WFS_SIU_ENHANCEDMICROPHONECONTROL (5)
#define WFS_SIU_MICROPHONEVOLUME (6)
/* Indices of WFSSIUSTATUS.fwGuidLights [...]
WFSSIUCAPS.fwGuidLights [...]
WFSSIUENABLE.fwGuidLights [...]
WFSSIUSETPORTS.wGuidLights [...]
WFSSIUSETGUIDLIGHT.wGuidLight
WFSSIUPORTEVENT.wPortIndex
WFSSIUPORTERROR.wPortIndex */
#define WFS_SIU_CARDUNIT (0)
#define WFS_SIU_PINPAD (1)
#define WFS_SIU_NOTESDISPENSER (2)
#define WFS_SIU_COINDISPENSER (3)
#define WFS_SIU_RECEIPTPRINTER (4)
#define WFS_SIU_PASSBOOKPRINTER (5)
#define WFS_SIU_ENVDEPOSITORY (6)
#define WFS_SIU_CHEQUEUNIT (7)
#define WFS_SIU_BILLACCEPTOR (8)
#define WFS_SIU_ENVDISPENSER (9)
#define WFS_SIU_DOCUMENTPRINTER (10)
#define WFS_SIU_COINACCEPTOR (11)
#define WFS_SIU_SCANNER (12)
#define WFS_SIU_CONTACTLESS (16)
#define WFS_SIU_CARDUNIT2 (17)
#define WFS_SIU_NOTESDISPENSER2 (18)
#define WFS_SIU_BILLACCEPTOR2 (19)
/* Values of WFSSIUSTATUS.fwSensors [...]
WFSSIUSTATUS.fwDoors [...]
WFSSIUSTATUS.fwIndicators [...]
WFSSIUSTATUS.fwAuxiliaries [...]
WFSSIUSTATUS.fwGuidLights [...]
WFSSIUCAPS.fwSensors [...]
WFSSIUCAPS.fwDoors [...]
WFSSIUCAPS.fwIndicators [...]
WFSSIUCAPS.fwAuxiliaries [...]
WFSSIUCAPS.fwGuidLights [...] */
#define WFS_SIU_NOT_AVAILABLE (0x0000)
78

CWA 16926-10:2020 (E)
#define WFS_SIU_AVAILABLE (0x0001)
/* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_OPERATORSWITCH]
WFSSIUCAPS.fwSensors [WFS_SIU_OPERATORSWITCH]
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_RUN (0x0001)
#define WFS_SIU_MAINTENANCE (0x0002)
#define WFS_SIU_SUPERVISOR (0x0004)
/* Values of WFSSIUSTATUS.fwDoors [...]
WFSSIUSTATUS.fwIndicators [WFS_SIU_OPENCLOSE]
WFSSIUCAPS.fwDoors [...]
WFSSIUCAPS.fwIndicators [WFS_SIU_OPENCLOSE]
WFSSIUSETPORTS.fwDoors [...]
WFSSIUSETPORTS.fwIndicators [WFS_SIU_OPENCLOSE]
WFSSIUSETDOOR.wDoor
WFSSIUSETINDICATOR.fwCommand
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_CLOSED (0x0001)
#define WFS_SIU_OPEN (0x0002)
#define WFS_SIU_LOCKED (0x0004)
#define WFS_SIU_BOLTED (0x0008)
#define WFS_SIU_SERVICE (0x0010)
#define WFS_SIU_KEYBOARD (0x0020)
#define WFS_SIU_AJAR (0x0040)
#define WFS_SIU_JAMMED (0x0080)
#define WFS_SIU_TAMPERED (0x0100)
/* Values of WFSSIUSTATUS.fwIndicators [WFS_SIU_AUDIO]
WFSSIUSETPORTS.fwIndicators [WFS_SIU_AUDIO]
WFSSIUSETINDICATOR.fwCommand
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_KEYPRESS (0x0002)
#define WFS_SIU_EXCLAMATION (0x0004)
#define WFS_SIU_WARNING (0x0008)
#define WFS_SIU_ERROR (0x0010)
#define WFS_SIU_CRITICAL (0x0020)
/* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_CONSUMER_DISPLAY]
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_DISPLAY_ERROR (0x0004)
/* Flags for WFSSIUSTATUS.fwIndicators [WFS_SIU_TRANSINDICATOR]
WFSSIUSETPORTS.fwIndicators [WFS_SIU_TRANSINDICATOR]
WFSSIUSETINDICATOR.fwCommand
WFSSIUPORTEVENT.wPortStatus[WFS_SIU_TRANSINDICATOR]
WFSSIUPORTERROR.wPortStatus[WFS_SIU_TRANSINDICATOR] */
#define WFS_SIU_LAMP1 (0x0001)
#define WFS_SIU_LAMP2 (0x0002)
#define WFS_SIU_LAMP3 (0x0004)
#define WFS_SIU_LAMP4 (0x0008)
#define WFS_SIU_LAMP5 (0x0010)
#define WFS_SIU_LAMP6 (0x0020)
#define WFS_SIU_LAMP7 (0x0040)
#define WFS_SIU_LAMP8 (0x0080)
#define WFS_SIU_LAMP9 (0x0100)
#define WFS_SIU_LAMP10 (0x0200)
#define WFS_SIU_LAMP11 (0x0400)
#define WFS_SIU_LAMP12 (0x0800)
#define WFS_SIU_LAMP13 (0x1000)
#define WFS_SIU_LAMP14 (0x2000)
#define WFS_SIU_LAMP15 (0x4000)
79

CWA 16926-10:2020 (E)
#define WFS_SIU_LAMP16 (0x8000)
/* Values of WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_REMOTE_STATUS_MONITOR]
WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_REMOTE_STATUS_MONITOR]
WFSSIUSETAUXILIARY.fwCommand
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_GREEN_LED_ON (0x0001)
#define WFS_SIU_GREEN_LED_OFF (0x0002)
#define WFS_SIU_AMBER_LED_ON (0x0004)
#define WFS_SIU_AMBER_LED_OFF (0x0008)
#define WFS_SIU_RED_LED_ON (0x0010)
#define WFS_SIU_RED_LED_OFF (0x0020)
/* Values of WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
WFSSIUSETAUXILIARY.fwCommand
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_PUBLICAUDIO_MANUAL (0x0001)
#define WFS_SIU_PUBLICAUDIO_AUTO (0x0002)
#define WFS_SIU_PUBLICAUDIO_SEMI_AUTO (0x0004)
#define WFS_SIU_PRIVATEAUDIO_MANUAL (0x0008)
#define WFS_SIU_PRIVATEAUDIO_AUTO (0x0010)
#define WFS_SIU_PRIVATEAUDIO_SEMI_AUTO (0x0020)
/* Values of WFSSIUSTATUS.fwSensors [...]
WFSSIUSTATUS.fwIndicators [...]
WFSSIUSTATUS.fwAuxiliaries [...]
WFSSIUSTATUS.fwGuidLights [...]
WFSSIUCAPS.fwSensors [...]
WFSSIUCAPS.fwIndicators [...]
WFSSIUCAPS.fwGuidLights [...]
WFSSIUSETPORTS.fwIndicators [...]
WFSSIUSETPORTS.fwAuxiliaries [...]
WFSSIUSETPORTS.fwGuidLights [...]
WFSSIUSETINDICATOR.fwCommand [...]
WFSSIUSETAUXILIARY.fwCommand [...]
WFSSIUSETGUIDLIGHT.fwCommand [...]
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_OFF (0x0001)
#define WFS_SIU_ON (0x0002)
#define WFS_SIU_SLOW_FLASH (0x0004)
#define WFS_SIU_MEDIUM_FLASH (0x0008)
#define WFS_SIU_QUICK_FLASH (0x0010)
#define WFS_SIU_CONTINUOUS (0x0080)
#define WFS_SIU_GUIDANCE_RED (0x00000100)
#define WFS_SIU_GUIDANCE_GREEN (0x00000200)
#define WFS_SIU_GUIDANCE_YELLOW (0x00000400)
#define WFS_SIU_GUIDANCE_BLUE (0x00000800)
#define WFS_SIU_GUIDANCE_CYAN (0x00001000)
#define WFS_SIU_GUIDANCE_MAGENTA (0x00002000)
#define WFS_SIU_GUIDANCE_WHITE (0x00004000)
#define WFS_SIU_GUIDANCE_ENTRY (0x00100000)
#define WFS_SIU_GUIDANCE_EXIT (0x00200000)
/* Flags for WFSSIUSTATUS.fwSensors [WFS_SIU_GENERALINPUTPORT]
WFSSIUSTATUS.fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
WFSSIUSETPORTS.fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
WFSSIUSETINDICATOR.fwCommand
WFSSIUPORTEVENT.wPortStatus[WFS_SIU_GENERALINPUTPORT]
WFSSIUPORTEVENT.wPortStatus[WFS_SIU_GENERALOUTPUTPORT]
WFSSIUPORTERROR.wPortStatus[WFS_SIU_GENERALINPUTPORT]
WFSSIUPORTERROR.wPortStatus[WFS_SIU_GENERALOUTPUTPORT] */
80

CWA 16926-10:2020 (E)
#define WFS_SIU_GPP1 (0x0001)
#define WFS_SIU_GPP2 (0x0002)
#define WFS_SIU_GPP3 (0x0004)
#define WFS_SIU_GPP4 (0x0008)
#define WFS_SIU_GPP5 (0x0010)
#define WFS_SIU_GPP6 (0x0020)
#define WFS_SIU_GPP7 (0x0040)
#define WFS_SIU_GPP8 (0x0080)
#define WFS_SIU_GPP9 (0x0100)
#define WFS_SIU_GPP10 (0x0200)
#define WFS_SIU_GPP11 (0x0400)
#define WFS_SIU_GPP12 (0x0800)
#define WFS_SIU_GPP13 (0x1000)
#define WFS_SIU_GPP14 (0x2000)
#define WFS_SIU_GPP15 (0x4000)
#define WFS_SIU_GPP16 (0x8000)
/* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_PROXIMITY]
WFSSIUSTATUS.fwSensors [WFS_SIU_ENHANCEDAUDIO]
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_PRESENT (0x0001)
#define WFS_SIU_NOT_PRESENT (0x0002)
/* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_HANDSETSENSOR] */
#define WFS_SIU_OFF_THE_HOOK (0x0001)
#define WFS_SIU_ON_THE_HOOK (0x0002)
/* Values of WFSSIUCAPS.fwSensors [WFS_SIU_ENHANCEDAUDIO]
WFSSIUCAPS.fwSensors [WFS_SIU_HANDSETSENSOR] */
#define WFS_SIU_MANUAL (0x0001)
#define WFS_SIU_AUTO (0x0002)
#define WFS_SIU_SEMI_AUTO (0X0004)
/* Values of WFSSIUCAPS.fwSensors [WFS_SIU_HANDSETSENSOR] */
#define WFS_SIU_MICROPHONE (0x0010)
/* Values of WFSSIUCAPS.fwSensors [WFS_SIU_ENHANCEDAUDIO] */
#define WFS_SIU_BIDIRECTIONAL (0x0020)
/* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_AMBLIGHT]
WFSSIUCAPS.fwSensors [WFS_SIU_AMBLIGHT]
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_VERY_DARK (0x0001)
#define WFS_SIU_DARK (0x0002)
#define WFS_SIU_MEDIUM_LIGHT (0x0004)
#define WFS_SIU_LIGHT (0x0008)
#define WFS_SIU_VERY_LIGHT (0x0010)
/* Values of WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_UPS]
WFSSIUCAPS.fwAuxiliaries [WFS_SIU_UPS]
WFSSIUPORTEVENT.wPortStatus
WFSSIUPORTERROR.wPortStatus */
#define WFS_SIU_LOW (0x0002)
#define WFS_SIU_ENGAGED (0x0004)
#define WFS_SIU_POWERING (0x0008)
#define WFS_SIU_RECOVERED (0x0010)
/* Values of WFSSIUCAPS.fwType */
81

CWA 16926-10:2020 (E)
#define WFS_SIU_SENSORS (0x0001)
#define WFS_SIU_DOORS (0x0002)
#define WFS_SIU_INDICATORS (0x0004)
#define WFS_SIU_AUXILIARIES (0x0008)
#define WFS_SIU_GUIDLIGHTS (0x0010)
/* Values of WFSSIUCAPS.fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
WFSSIUCAPS.fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL] */
#define WFS_SIU_HEADSET_DETECTION (0x0001)
#define WFS_SIU_MODE_CONTROLLABLE (0x0002)
/* Values of WFSSIUENABLE.fwSensors [...]
WFSSIUENABLE.fwDoors [...]
WFSSIUENABLE.fwIndicators [...]
WFSSIUENABLE.fwAuxiliaries [...]
WFSSIUENABLE.fwGuidLights [...]
WFSSIUSETPORTS.fwDoors [...]
WFSSIUSETPORTS.fwIndicators [...]
WFSSIUSETPORTS.fwAuxiliaries [...]
WFSSIUSETPORTS.fwGuidLights [...] */
#define WFS_SIU_NO_CHANGE (0x0000)
#define WFS_SIU_ENABLE_EVENT (0x0001)
#define WFS_SIU_DISABLE_EVENT (0x0002)
/* Values of WFSSIUSETPORTS.fwDoors [...]
WFSSIUSETDOOR.fwCommand [...] */
#define WFS_SIU_BOLT (0x0001)
#define WFS_SIU_UNBOLT (0x0002)
/* Values of WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_UPS]
WFSSIUSETAUXILIARY.wAuxiliary [WFS_SIU_UPS] */
#define WFS_SIU_ENGAGE (0x0001)
#define WFS_SIU_DISENGAGE (0x0002)
/* Values of WFSSIUCAPS.fwAutoStartupMode
WFSSIUSETSTARTUPTIME.wMode
WFSSIUGETSTARTUPTIME.wMode */
#define WFS_SIU_AUTOSTARTUP_CLEAR (0x0001)
#define WFS_SIU_AUTOSTARTUP_SPECIFIC (0x0002)
#define WFS_SIU_AUTOSTARTUP_DAILY (0x0004)
#define WFS_SIU_AUTOSTARTUP_WEEKLY (0x0008)
/* Values of WFSSIUSTATUS.wAntiFraudModule */
#define WFS_SIU_AFMNOTSUPP (0)
#define WFS_SIU_AFMOK (1)
#define WFS_SIU_AFMINOP (2)
#define WFS_SIU_AFMDEVICEDETECTED (3)
#define WFS_SIU_AFMUNKNOWN (4)
/* XFS SIU Errors */
#define WFS_ERR_SIU_INVALID_PORT (-(SIU_SERVICE_OFFSET + 1))
#define WFS_ERR_SIU_SYNTAX (-(SIU_SERVICE_OFFSET + 2))
#define WFS_ERR_SIU_PORT_ERROR (-(SIU_SERVICE_OFFSET + 3))
#define WFS_ERR_SIU_POWERSAVETOOSHORT (-(SIU_SERVICE_OFFSET + 4))
#define WFS_ERR_SIU_COMMANDUNSUPP (-(SIU_SERVICE_OFFSET + 5))
#define WFS_ERR_SIU_SYNCHRONIZEUNSUPP (-(SIU_SERVICE_OFFSET + 6))
/*=================================================================*/
/* SIU Info Command Structures and variables */
/*=================================================================*/
typedef struct _wfs_siu_status
{
WORD fwDevice;
82

CWA 16926-10:2020 (E)
WORD fwSensors [WFS_SIU_SENSORS_SIZE];
WORD fwDoors [WFS_SIU_DOORS_SIZE];
WORD fwIndicators [WFS_SIU_INDICATORS_SIZE];
WORD fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
WORD fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
LPSTR lpszExtra;
USHORT usPowerSaveRecoveryTime;
WORD wAntiFraudModule;
DWORD fwGuidLightsEx [WFS_SIU_GUIDLIGHTS_SIZE_EX];
} WFSSIUSTATUS, *LPWFSSIUSTATUS;
typedef struct _wfs_siu_trm_info
{
LPWSTR lpszModelID;
LPWSTR lpszSerialNumber;
} WFSSIUTRMINFO, *LPWFSSIUTRMINFO;
typedef struct _wfs_siu_caps
{
WORD wClass;
WORD fwType;
WORD fwSensors [WFS_SIU_SENSORS_SIZE];
WORD fwDoors [WFS_SIU_DOORS_SIZE];
WORD fwIndicators [WFS_SIU_INDICATORS_SIZE];
WORD fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
WORD fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
LPSTR lpszExtra;
BOOL bPowerSaveControl;
WORD fwAutoStartupMode;
BOOL bAntiFraudModule;
LPDWORD lpdwSynchronizableCommands;
LPWFSSIUTRMINFO lpTerminalInformation;
DWORD fwGuidLightsEx [WFS_SIU_GUIDLIGHTS_SIZE_EX];
} WFSSIUCAPS, *LPWFSSIUCAPS;
typedef struct wfs_siu_get_startup_time
{
WORD wMode;
LPSYSTEMTIME lpStartTime;
} WFSSIUGETSTARTUPTIME, *LPWFSSIUGETSTARTUPTIME;
/*=================================================================*/
/* SIU Execute Command Structures */
/*=================================================================*/
typedef struct _wfs_siu_enable
{
WORD fwSensors [WFS_SIU_SENSORS_SIZE];
WORD fwDoors [WFS_SIU_DOORS_SIZE];
WORD fwIndicators [WFS_SIU_INDICATORS_SIZE];
WORD fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
WORD fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
LPSTR lpszExtra;
} WFSSIUENABLE, *LPWFSSIUENABLE;
typedef struct _wfs_siu_set_ports
{
WORD fwDoors [WFS_SIU_DOORS_SIZE];
WORD fwIndicators [WFS_SIU_INDICATORS_SIZE];
WORD fwAuxiliaries [WFS_SIU_AUXILIARIES_SIZE];
WORD fwGuidLights [WFS_SIU_GUIDLIGHTS_SIZE];
LPSTR lpszExtra;
} WFSSIUSETPORTS, *LPWFSSIUSETPORTS;
typedef struct _wfs_siu_set_door
{
WORD wDoor;
WORD fwCommand;
} WFSSIUSETDOOR, *LPWFSSIUSETDOOR;
83

CWA 16926-10:2020 (E)
typedef struct _wfs_siu_set_indicator
{
WORD wIndicator;
WORD fwCommand;
} WFSSIUSETINDICATOR, *LPWFSSIUSETINDICATOR;
typedef struct _wfs_siu_set_auxiliary
{
WORD wAuxiliary;
WORD fwCommand;
} WFSSIUSETAUXILIARY, *LPWFSSIUSETAUXILIARY;
typedef struct _wfs_siu_set_guidlight
{
WORD wGuidLight;
WORD fwCommand;
} WFSSIUSETGUIDLIGHT, *LPWFSSIUSETGUIDLIGHT;
typedef struct _wfs_siu_set_guidlight_ex
{
WORD wGuidLight;
DWORD fwCommand;
} WFSSIUSETGUIDLIGHTEX, *LPWFSSIUSETGUIDLIGHTEX;
typedef struct _wfs_siu_power_save_control
{
USHORT usMaxPowerSaveRecoveryTime;
} WFSSIUPOWERSAVECONTROL, *LPWFSSIUPOWERSAVECONTROL;
typedef struct wfs_siu_set_startup_time
{
WORD wMode;
LPSYSTEMTIME lpStartTime;
} WFSSIUSETSTARTUPTIME, *LPWFSSIUSETSTARTUPTIME;
/*=================================================================*/
/* SIU Message Structures */
/*=================================================================*/
typedef struct _wfs_siu_port_event
{
WORD wPortType;
WORD wPortIndex;
WORD wPortStatus;
LPSTR lpszExtra;
DWORD dwPortStatus;
} WFSSIUPORTEVENT, *LPWFSSIUPORTEVENT;
typedef struct _wfs_siu_port_error
{
WORD wPortType;
WORD wPortIndex;
HRESULT PortError;
WORD wPortStatus;
LPSTR lpszExtra;
DWORD dwPortStatus;
} WFSSIUPORTERROR, *LPWFSSIUPORTERROR;
typedef struct _wfs_siu_power_save_change
{
USHORT usPowerSaveRecoveryTime;
} WFSSIUPOWERSAVECHANGE, *LPWFSSIUPOWERSAVECHANGE;
typedef struct _wfs_siu_synchronize_command
{
DWORD dwCommand;
LPVOID lpCmdData;
} WFSSIUSYNCHRONIZECOMMAND, *LPWFSSIUSYNCHRONIZECOMMAND;
84

CWA 16926-10:2020 (E)
/* restore alignment */
#pragma pack (pop)
#ifdef __cplusplus
} /*extern "C"*/
#endif
#endif /* __INC_XFSSIU__H */
85