---
source: XfsApi.pdf
document: XfsApi
device_class: API/SPI
part: 1
section_id: "17"
title: "Appendix D – Win64 Migration Considerations"
type: appendix
source_lines: [5491, 5525]
---

<!-- 17 Appendix D – Win64 Migration Considerations -->

17 Appendix D – Win64 Migration Considerations
Users must ensure that when porting their XFS applications to the Win64 environment that care is taken to
update their existing code correctly in order to avoid issues. Microsoft state that porting 32-bit applications
to 64-bit will be easier than it was porting 16-bit applications to 32-bit Windows, but care must still be
taken in certain areas.
On 64-bit operating systems it is possible to run either a complete 32-bit XFS software stack, or a
complete 64-bit software stack. Where a native 64-bit application is being run a 64-bit XFS Manager must
be used. A sample XFS Manager is supplied with the XFS SDK, however this is a 32-bit XFS Manager
only.
By far the biggest change when migrating C code is the change in pointer size from 32 to 64 bits. As the
XFS architecture makes extensive use of pointers this change may have a significant impact on native XFS
applications that currently run on Win32 environments. The following are some considerations for
developers with regard to the XFS architecture:
1. As it is impossible for a 64-bit process to load a 32-bit DLL directly it is recommended that the
entire software stack from the application to the Service Providers should be native 64-bit where
possible. While this is the most ideal solution it is not mandatory but a recommendation, as feasibly
some dependencies could run out of process to the application and/or the Service Providers. The XFS
Manager that is used will always need to be 64-bit for a 64-bit application.
2. All declarations, use and storage of pointers should be checked. In C code memory addresses are
often stored as a ULONG value, because on 32-bit Windows an address, a pointer, and a ULONG are
all 32 bits. On 64-bit Windows a ULONG is also 32 bits long, but all pointers are 64-bit values.
Functions such as the C sizeof() method will also need to be checked as the value that they return for
the size of a pointer will be 8 bytes rather than 4 bytes. C style casts will also need to be scrutinized as
potentially they may cast a pointer to a 32-bit value.
3. In order to prepare for the possibility of future porting to 64-bit code, developers should consider
using the latest Windows header files that contain the portable pointer precision type ULONG_PTR.
This data type can be used in current 32-bit code to store pointer values instead of a ULONG. The
ULONG_PTR data type is a portable value that is 32 bits when compiled with a 32-bit compiler and
64 bits when compiled with a 64-bit compiler, thus ensuring good compatibility when compiled in
either environment.
4. If running a 32-bit application on a 64 bit operating system then the operating system may manage
the precise location of the XFS registry locations in 32-bit compatible areas of the registry.
160

CWA 16926-1:2025 (E)
