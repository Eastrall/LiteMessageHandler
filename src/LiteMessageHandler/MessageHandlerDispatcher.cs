using LiteMessageHandler.Internal;
using MessageHandler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteMessageHandler;

public class MessageHandlerDispatcher : IMessageHandlerDispatcher
{
    private readonly IServiceProvider? _serviceProvider;
    private MessageHandlerActionCache? _handlerCache;

    public MessageHandlerDispatcher(IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;
    }

    public void Load(params Assembly[] assemblies)
    {
        Assembly[]? assembliesToLoad = assemblies?.Length > 0 ? assemblies : AppDomain.CurrentDomain.GetAssemblies();

        Dictionary<Type, MessageHandlerAction>? handlers = assembliesToLoad.SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.ImplementsInterface(typeof(IMessageHandler<>)))
            .Select(x => new MessageHandlerAction(x))
            .ToDictionary(keySelector: x => x.HandlerParameterType, elementSelector: x => x);

        _handlerCache = new MessageHandlerActionCache(handlers);
    }

    public MessageHandler? GetHandler(Type? handlerType)
    {
        if (_handlerCache == null)
        {
            throw new ArgumentException("Cannot get message handler from cache.");
        }

        MessageHandlerAction? handler = _handlerCache.GetMessageHandler(handlerType);

        if (handler == null)
        {
            return null;
        }

        return new MessageHandler(handler.CreateInstance(_serviceProvider), handler.Executor);
    }

    public MessageHandler? GetHandler<TMessage>()
    {
        return GetHandler(typeof(TMessage));
    }

    public void Dispatch<TMessage>(TMessage message)
    {
        MessageHandler? handler = GetHandler(typeof(TMessage));

        if (handler == null)
        {
            throw new InvalidOperationException($"Cannot find handler for type '{typeof(TMessage).FullName}'.");
        }

        handler.Execute(message!);
    }

    public void Dispose()
    {
        _handlerCache?.Dispose();
    }
}
