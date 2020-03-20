using System;

namespace CMA.ISMAI.Core.Events
{
    public class Event : Message
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
