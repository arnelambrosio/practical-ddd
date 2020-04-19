using System;
using System.Collections.Generic;
using System.Text;
using TradingEngine.Logic.Common;

namespace TradingEngine.Logic.Domain
{
    public class Currency : AggregateRoot
    {
        public virtual string Name { get; protected set; }
        public virtual decimal Ratio { get; protected set; }

        public Currency(string name, decimal ratio):this()
        {
            Name = name;
            Ratio = ratio;
        }

        private Currency()
        {

        }

        public override string ToString()
        {
            return $"Curreny(name= ){Name}, ratio = {Ratio}";
        }
    }
}
