using System;
using System.Reflection;

namespace LiteMessageHandler;

/// <summary>
/// Provides a mechanism to manage the message handlers.
/// </summary>
public interface IMessageHandlerDispatcher : IDisposable
{
    /// <summary>
    /// Loads the message handlers based on the current domain
    /// </summary>
    void Load(params Assembly[] assemblies);

    MessageHandler? GetHandler(Type? type);

    MessageHandler? GetHandler<TMessage>();

    void Dispatch<TMessage>(TMessage message);
}
