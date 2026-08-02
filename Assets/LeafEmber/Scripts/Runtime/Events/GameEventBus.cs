using System;
using System.Collections.Generic;

namespace LeafEmber.Events
{

public sealed class GameEventBus : IEventBus
{
    private readonly Dictionary<Type, Delegate> handlers = new();

    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }
        Type eventType = typeof(TEvent);
        handlers.TryGetValue(eventType, out Delegate current);
        handlers[eventType] = Delegate.Combine(current, handler);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }
        Type eventType = typeof(TEvent);

        if (!handlers.TryGetValue(eventType, out Delegate current))
        {
            return;
        }

        Delegate remaining = Delegate.Remove(current, handler);
        if (remaining == null)
        {
            handlers.Remove(eventType);
            return;
        }

        handlers[eventType] = remaining;
    }

    public void Publish<TEvent>(TEvent eventData)
    {
        if (handlers.TryGetValue(typeof(TEvent), out Delegate current))
        {
            ((Action<TEvent>)current).Invoke(eventData);
        }
    }
}
}
