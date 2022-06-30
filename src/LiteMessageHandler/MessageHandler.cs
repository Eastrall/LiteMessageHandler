using MessageHandler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteMessageHandler;

public abstract class MessageHandler<TMessage>
{
    public abstract void Execute(TMessage message);
}

public class MessageHandlerAction
{
    public Type HandlerType { get; }

    public Type? HandlerParameterType { get; }

    public MessageHandlerAction(Type? handlerType)
    {
        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));

        var genericType = handlerType.FindGenericType(typeof(MessageHandler<>));

        if (genericType != null && genericType.IsGenericType)
        {
            HandlerParameterType = genericType.GetGenericArguments().Single();
        }
    }
}


public static class MessageHandlerDispatcher
{
    private static readonly Dictionary<Type, MessageHandlerAction> _handlers = new();

    public static void Load()
    {
        var handlers = from x in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                       where x.IsClass && !x.IsAbstract && x.ImplementsGenericType(typeof(MessageHandler<>))
                       select new MessageHandlerAction(x);

        foreach (var handler in handlers)
        {
            _handlers.Add(handler.HandlerParameterType, handler);
        }
    }

    public static void Dispatch<TMessage>(TMessage message)
    {
        if (_handlers.TryGetValue(typeof(TMessage), out var handler))
        {
            var h = Activator.CreateInstance(handler.HandlerType) as MessageHandler<TMessage>;

            if (h != null)
            {
                h.Execute(message);
            }
        }
    }
}
