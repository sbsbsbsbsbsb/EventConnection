using System;

namespace Model.Runtime
{
    public class StaffVotedEventArgs : EventArgs
    {
        public int Vote { get; set; }
        public int StaffId { get; set; }
    }
}
