using System.Text.Json;

namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>Handles one message type received from the Service Provider DLL.</summary>
public interface IMessageHandler
{
    string MessageType { get; }

    Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken);
}

public interface IMessageDispatcher
{
    Task DispatchAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken);
}
