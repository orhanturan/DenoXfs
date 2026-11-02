using System;
using DenoXfs.Api;
using DenoXfs.Core.Abstractions;

namespace DenoXfs.Core.Services;

public class XfsResultReleaser : IXfsResultReleaser
{
    public int Free(IntPtr resultHandle)
    {
        return resultHandle == IntPtr.Zero ? 0 : XfsApi.FreeResult(resultHandle);
    }
}
