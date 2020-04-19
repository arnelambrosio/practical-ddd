using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.Common.Events
{
    interface ISubscriber<T>
    {
        void Handle(T e);
    }
}
