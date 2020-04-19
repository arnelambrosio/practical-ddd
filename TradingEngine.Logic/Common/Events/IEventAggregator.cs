using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.Common.Events
{
    public interface IEventAggregator
    {
        void Register(object subscriber);
        void Raise<TEvent>(TEvent eventToPublish);
    }
}
