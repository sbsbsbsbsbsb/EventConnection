using Model.DTO;

namespace Model.Runtime
{
    public class EventStaffNavigationArgs
    {
        public StaffModel Staff { get; set; }
        public int? EventId { get; set; }
    }
}
