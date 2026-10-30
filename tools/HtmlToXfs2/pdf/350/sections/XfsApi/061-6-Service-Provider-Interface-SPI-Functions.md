---
source: XfsApi.pdf
document: XfsApi
device_class: API/SPI
part: 1
section_id: "6"
title: "Service Provider Interface (SPI) Functions"
type: section
source_lines: [2917, 2928]
---

<!-- 6 Service Provider Interface (SPI) Functions -->

6  Service Provider Interface (SPI) Functions
The Service Provider functions are described in the following sections, in alphabetical order. The table below
shows the SPI functions, the sections in which they are defined, their modes, and the API functions they implement.
The asynchronous SPI functions behavior is influenced by whether the function is Deferred or Non-deferred [see
section 4.8 Exclusive Service and Device Access]. An asynchronous non-deferred function (for example
WFPRegister) can be processed completely by the service as soon as it is received. An asynchronous deferred
function (for example WFPExecute) cannot be processed completely as soon as it arrives, because it may require
hardware and/or operator interaction.

| Sectio XFS SPI  | Mode  | XFS API  | Mode  |
| --------------- | ----- | -------- | ----- |
n
