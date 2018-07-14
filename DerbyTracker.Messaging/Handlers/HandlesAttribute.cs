using System;

namespace DerbyTracker.Messaging.Handlers
{
    public class HandlesAttribute : Attribute
    {
        public string CommandType { get; set; }

        public HandlesAttribute(string mtype) { CommandType = mtype; }

        public static HandlesAttribute Get(object o)
        {
            var t = o.GetType();
            return (HandlesAttribute)GetCustomAttribute(t, typeof(HandlesAttribute));
        }
    }
}
