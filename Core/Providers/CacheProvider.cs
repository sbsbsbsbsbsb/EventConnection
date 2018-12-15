using System.Collections.Generic;
using System.Linq;
using Model.DTO;
using Model.Runtime;

namespace Core.Providers
{
    public class CacheProvider //TODO: use session provider for suspend control
    {
        public List<EventModel> GetEventsList()
        {
            if (Conference?.Events != null && Conference.Events.Any())
            {
                return Conference.Events;
            }
            return null;
        }

        public EventModel GetEvent(int eventId)
        {
            return !EventsList.Any() ? null : EventsList.FirstOrDefault(x => x.Id == eventId);
        }

        public StaffModel GetStaff(int staffId)
        {
            return !StaffList.Any() ? null : StaffList.FirstOrDefault(x => x.Id == staffId);
        }

        public StaffModel GetStaffFromEvent(int eventId, int staffId)
        {
            var @event = GetEvent(eventId);

            return @event?.Staffs.FirstOrDefault(x => x.Id == staffId);
        }

        private CachedObject<ConferenceModel> _conference;
        private CachedObject<List<StaffModel>> _staffList;
        private CachedObject<List<EventModel>> _eventsList;

        public ConferenceModel Conference
        {
            get
            {
                if (_conference?.Object == null)
                {
                    _conference = new CachedObject<ConferenceModel>(new ConferenceModel { Id = ConferenceManager.GetConferenceId() });
                }
                    return _conference.Object;
            }
            set { _conference = new CachedObject<ConferenceModel>(value); }
        }

        public List<StaffModel> StaffList
        {
            get
            {
                if (_staffList?.Object == null)
                {
                    _staffList = new CachedObject<List<StaffModel>>(new List<StaffModel>());
                }
                return _staffList.Object;
            }
            set { _staffList = new CachedObject<List<StaffModel>>(value); }
        }

        public List<EventModel> EventsList
        {
            get
            {
                if (_eventsList?.Object == null)
                {
                    _eventsList = new CachedObject<List<EventModel>>(new List<EventModel>());
                }
                return _eventsList.Object;
            }
            set { _eventsList = new CachedObject<List<EventModel>>(value); }
        } //because conference stores only name and id of event
    }
}