using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TradingEngine.Logic.Common
{
    public  abstract class Entity
    {
        [Description("ignore")]
        public virtual int Id { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a.Equals(b));
        }

        public override int GetHashCode()
        {
            return (this.GetType().ToString() + Id).GetHashCode();
        }

        private Type GetRealType()
        {
            return this.GetType(); //To Do, need to implement based on ORM handling of proxy classes

            //NHibernate
            //return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);

        }
    }
}
