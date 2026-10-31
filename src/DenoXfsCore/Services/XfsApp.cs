using System;
using DenoXfs.Core.Abstractions;
using DenoXfs.Api;

namespace DenoXfs.Core.Services;

public class XfsApp : IXfsApp, IDisposable
{
    private readonly object lockObject = new object();
    private IntPtr handle = IntPtr.Zero;
    private bool disposed = false;

    public string Name { get; }

    public IntPtr Handle => handle;

    public XfsApp(string name)
    {
        Name = name ?? string.Empty;
    }

    public IntPtr Create()
    {
        lock (lockObject)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(XfsApp));

            if (handle != IntPtr.Zero)
                return handle;

            int result = XfsApi.CreateAppHandle(out IntPtr hApp);
            if (result == 0) // WFS_SUCCESS
            {
                handle = hApp;
            }

            return handle;
        }
    }

    public void Destroy()
    {
        lock (lockObject)
        {
            if (handle != IntPtr.Zero)
            {
                XfsApi.DestroyAppHandle(handle);
                handle = IntPtr.Zero;
            }
        }
    }

    public void Dispose()
    {
        if (disposed)
            return;

        lock (lockObject)
        {
            if (!disposed)
            {
                Destroy();
                disposed = true;
            }
        }
    }
}






