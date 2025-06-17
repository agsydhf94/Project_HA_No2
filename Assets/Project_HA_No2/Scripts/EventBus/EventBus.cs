using System;
using System.Collections.Generic;

namespace HA
{
    /// <summary>
    /// A global event bus system implementing a publish-subscribe model.
    /// Allows loose coupling between systems by broadcasting messages of type IEventMessage.
    /// </summary>
    public class EventBus : SingletonBase<EventBus>
    {
        /// <summary>
        /// Stores event type to list of delegates (subscribers) mapping.
        /// </summary>
        private readonly Dictionary<Type, List<Delegate>> listeners = new();


        /// <summary>
        /// Subscribes a listener to a specific event type.
        /// </summary>
        /// <typeparam name="T">Type of event message implementing IEventMessage.</typeparam>
        /// <param name="callback">Callback to be invoked when the event is published.</param>
        public void Subscribe<T>(Action<T> callback) where T : IEventMessage
        {
            var type = typeof(T);
            if (!listeners.ContainsKey(type))
                listeners[type] = new List<Delegate>();

            listeners[type].Add(callback);
        }


        /// <summary>
        /// Unsubscribes a listener from a specific event type.
        /// </summary>
        /// <typeparam name="T">Type of event message implementing IEventMessage.</typeparam>
        /// <param name="callback">Previously registered callback to remove.</param>
        public void Unsubscribe<T>(Action<T> callback) where T : IEventMessage
        {
            var type = typeof(T);
            if (listeners.ContainsKey(type))
                listeners[type].Remove(callback);
        }


        /// <summary>
        /// Publishes an event of the specified type to all registered listeners.
        /// </summary>
        /// <typeparam name="T">Type of event message implementing IEventMessage.</typeparam>
        /// <param name="eventData">The event data to be delivered.</param>
        public void Publish<T>(T eventData) where T : IEventMessage
        {
            var type = typeof(T);
            if (listeners.TryGetValue(type, out var delList))
            {
                foreach (var del in delList)
                    ((Action<T>)del)?.Invoke(eventData);
            }
        }
    }
}
