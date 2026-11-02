using DenoXfs.Core.Abstractions;
using DenoXfs.Api;
using DenoXfs.Api.Common;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Services;

public class XfsSession : IXfsSession
{
    private readonly object lockObject = new object();
    private bool isStarted = false;
    private bool disposed = false;

    public StartUpResponse Startup(StartUpRequest request)
    {
        lock (lockObject)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(XfsSession));

            if (isStarted)
                return new StartUpResponse { Result = 0, XfsVersion = request.XfsVersion };

            var version = new XfsVersion
            {
                Description = new byte[Xfs.WfsddescriptionLen + 1],
                SystemStatus = new byte[Xfs.WfsdsysstatusLen + 1]
            };
            int result = XfsApi.StartUp(request.VersionsRequired, ref version);
            if (result == 0) // WFS_SUCCESS
            {
                isStarted = true;
            }

            return new StartUpResponse { Result = result, XfsVersion = version };
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
