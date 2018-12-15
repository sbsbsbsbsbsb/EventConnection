using System;

namespace Model.Runtime
{
    public class EventVotedEventArgs : EventArgs
    {
        public int Vote { get; set; }
        public int EventId { get; set; }
    }
}
