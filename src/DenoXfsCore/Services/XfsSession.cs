using DenoXfs.Core.Abstractions;
using DenoXfs.Api;
using DenoXfs.Core.Models.Requests;

namespace DenoXfs.Core.Services;

public class XfsSession : IXfsSession
{
    private readonly object lockObject = new object();
    private bool isStarted = false;
    private bool disposed = false;

    public void Startup(StartUpRequest request)
    {
        lock (lockObject)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(XfsSession));

            if (isStarted)
                return;

            var version = request.XfsVersion;
            int result = XfsApi.StartUp(request.VersionsRequired, ref version);
            if (result == 0) // WFS_SUCCESS
            {
                isStarted = true;
            }
        }
    }

    public void Stop()
    {
        lock (lockObject)
        {
            if (!isStarted)
                return;

            XfsApi.CleanUp();
            isStarted = false;
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
                Stop();
                disposed = true;
            }
        }
    }
}






