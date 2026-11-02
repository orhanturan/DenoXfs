using DenoXfs.Core.Models;
using DenoXfs.TestTool.Operations;
using Microsoft.Extensions.Logging;

namespace DenoXfs.TestTool.Logging;

public class XfsTrafficLog : IXfsTrafficLog
{
    private const string DumpIndent = "                 ";
    private readonly IUiLogFeed _feed;
    private readonly IXfsCodeNames _names;
    private readonly ILogger<XfsTrafficLog> _logger;

    public XfsTrafficLog(IUiLogFeed feed, IXfsCodeNames names, ILogger<XfsTrafficLog> logger)
    {
        _feed = feed;
        _names = names;
        _logger = logger;
    }

    public void Request(XfsCodeKind kind, uint code, ushort hService, object? input)
    {
        var name = _names.Describe(kind, code);
        _logger.LogInformation("xfs request call={Call} code={Code} hService={HService}", Call(kind), name, hService);
        Write($"→ {Call(kind)} {name} hService={hService}", input);
    }

    public void Completion(XfsCodeKind kind, uint code, uint requestId, int result, object? output)
    {
        var name = _names.Describe(kind, code);
        var resultName = ResultNames.Describe(result);
        _logger.LogInformation("xfs completion call={Call} code={Code} reqId={ReqId} result={Result}", Call(kind), name, requestId, resultName);
        Write($"← {Call(kind)} {name} reqId={requestId} result={resultName}", output ?? "(no result data)");
    }

    public void Failure(XfsCodeKind kind, uint code, string message)
    {
        var name = _names.Describe(kind, code);
        _logger.LogWarning("xfs failure call={Call} code={Code} message={Message}", Call(kind), name, message);
        Write($"✖ {Call(kind)} {name} {message}", null);
    }

    public void CancelSent(XfsCodeKind kind, uint code, string call, int result)
    {
        var name = _names.Describe(kind, code);
        var resultName = ResultNames.Describe(result);
        _logger.LogInformation("xfs cancel call={Call} code={Code} api={Api} result={Result}", Call(kind), name, call, resultName);
        Write($"✖ CANCEL {Call(kind)} {name} {call} result={resultName}", null);
    }

    public void Event(XfsEventMessage message, object? payload)
    {
        var kind = message.Kind switch
        {
            XfsEventKind.Execute => XfsCodeKind.ExecuteEvent,
            XfsEventKind.Service => XfsCodeKind.ServiceEvent,
            XfsEventKind.User => XfsCodeKind.UserEvent,
            _ => XfsCodeKind.SystemEvent
        };
        var name = _names.Describe(kind, message.EventId);
        var body = payload ?? (message.Buffer == IntPtr.Zero ? null : "(payload not decoded)");
        Write($"◆ {Call(kind)} {name} hService={message.HService} reqId={message.RequestId} result={ResultNames.Describe(message.Result)}", body);
    }

    public void Session(string text) => Write($"■ {text}", null);

    private static string Call(XfsCodeKind kind) => kind switch
    {
        XfsCodeKind.Info => "INFO",
        XfsCodeKind.Command => "EXECUTE",
        XfsCodeKind.ExecuteEvent => "EXEE",
        XfsCodeKind.ServiceEvent => "SRVE",
        XfsCodeKind.UserEvent => "USRE",
        _ => "SYSE"
    };

    private void Write(string header, object? body)
    {
        var line = $"{DateTime.Now:HH:mm:ss.fff} {header}";
        var dump = body == null ? string.Empty : ObjectDumper.Dump(body);
        if (dump.Length > 0)
        {
            _logger.LogDebug("xfs dump {Header}{NewLine}{Dump}", header, Environment.NewLine, dump);
            var indented = string.Join(Environment.NewLine, dump.Split(Environment.NewLine).Select(l => DumpIndent + l));
            line += Environment.NewLine + indented;
        }
        _feed.Write(line);
    }
}
