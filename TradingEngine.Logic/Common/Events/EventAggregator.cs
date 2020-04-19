using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace TradingEngine.Logic.Common.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<WeakReference>> _eventSubscriberList =
            new Dictionary<Type, List<WeakReference>>();

        private readonly object _lock = new object();

        public void Raise<TEvent>(TEvent eventToPublish)
        {
            var subscriberType = typeof(ISubscriber<>).MakeGenericType(typeof(TEvent));
            var subscribers = GetSubscribers(subscriberType);
            List<WeakReference> subscribersToRemove = new List<WeakReference>();

            foreach (var weakSubsriber in subscribers)
            {
                if (weakSubsriber.IsAlive)
                {
                    var subsriber = (ISubscriber<TEvent>)weakSubsriber.Target;
                    var syncContext = SynchronizationContext.Current;
                    if (syncContext == null)
                        syncContext = new SynchronizationContext();

                    syncContext.Post(s => subsriber.Handle(eventToPublish), null);
                }
                else
                {
                    subscribersToRemove.Add(weakSubsriber);
                }
            }

            if (subscribersToRemove.Any())
            {
                lock (_lock)
                {
                    foreach (var item in subscribersToRemove)
                    {
                        subscribers.Remove(item);
                    }
                }
            }
        }

        public void Register(object subscriber)
        {
            lock (_lock)
            {
                var subscriberTypes =
                    subscriber.GetType().GetInterfaces().Where(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

                var weakReference = new WeakReference(subscriber);
                foreach (var subscriberType in subscriberTypes)
                {
                    var subscribers = GetSubscribers(subscriberType);
                    subscribers.Add(weakReference);
                }
            }
        }

        private List<WeakReference> GetSubscribers(Type subscriberType)
        {
            List<WeakReference> subscribers;
            lock (_lock)
            {
                var found = _eventSubscriberList.TryGetValue(subscriberType, out subscribers);
                if (!found)
                {
                    subscribers = new List<WeakReference>();
                    _eventSubscriberList.Add(subscriberType, subscribers);
                }
            }
            return subscribers;
        }
    }
}
