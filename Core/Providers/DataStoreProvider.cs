using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using JetBrains.Annotations;
using Model.DataModel;
using Model.Runtime;
using Model.Runtime.Interfaces;
using Newtonsoft.Json;

namespace Core.Providers
{
    public class DataStoreProvider : IDataProvider //TODO: use session provider for suspend control
    {
        private readonly ApplicationDataContainer _appSettings;

        private const string CurrentUserKey = "EC.User";
        private const string ReviewKey = "EC.Reviews";
        private const string StaffVotesKey = "EC.StaffVotes";
        private const string EventVotesKey = "EC.EventVotes";

        private UserInfo _currentUser;
        private Dictionary<int, ReviewCollector> _reviewCollectors;
        private Dictionary<int, int> _staffVotesDictionary;
        private Dictionary<int, int> _eventsVotesDictionary;

        public UserInfo CurrentUser => _currentUser;

        public ReviewCollector Review
        {
            get
            {
                var confId = ConferenceManager.GetConferenceId();
                EnsureCurrentReview(confId);
                return _reviewCollectors[confId];
            }
            set
            {
                var confId = ConferenceManager.GetConferenceId();
                EnsureCurrentReview(confId);
                _reviewCollectors[confId] = value;
                //SetStoredValue(ReviewKey, _reviewCollectors);
            }
        }

        private void EnsureCurrentReview(int confId)
        {
            //GetStoredValue(CurrentUserKey, out _currentUser); //TODO: remove hack for multiply instance
            //GetStoredValue(ReviewKey, out _reviewCollectors); //TODO: remove hack for multiply instance
            //CreateInitialData(); //TODO: remove hack for multiply instance

            if (!_reviewCollectors.Any() || !_reviewCollectors.ContainsKey(confId))
            {
                _reviewCollectors.Add(confId, new ReviewCollector { ConferenceId = confId, UserId = _currentUser.UserId.Value });
            }
        }

        public DataStoreProvider()
        {
            _appSettings = ApplicationData.Current.LocalSettings;

            GetStoredValue(CurrentUserKey, out _currentUser);
            GetStoredValue(ReviewKey, out _reviewCollectors);

            GetStoredValue(StaffVotesKey, out _staffVotesDictionary);
            GetStoredValue(EventVotesKey, out _eventsVotesDictionary);

            CreateInitialData();
        }

        public bool UpdateUserInfo([NotNull] UserInfo user)
        {
            try
            {
                SetStoredValue(CurrentUserKey, user);
                _currentUser = user;
            }
            catch
            {
                return false;
            }
            return true;
        }
        /*
        public bool UpdateReview([NotNull] ReviewCollector review)
        {
            try
            {
                Review = review;
                SetStoredValue(ReviewKey, _reviewCollectors);
            }
            catch
            {
                return false;
            }
            return true;
        }*/

        public bool VoteEvent(int id, int voting)
        {
            try
            {
                if (!_eventsVotesDictionary.ContainsKey(id))
                {
                    _eventsVotesDictionary.Add(id, voting);
                }
                else
                {
                    return false;
                    //_eventsVotesDictionary[id] = voting;
                }
                SetStoredValue(EventVotesKey, _eventsVotesDictionary);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int GetVoteForEvent(int id)
        {
            GetStoredValue(EventVotesKey, out _eventsVotesDictionary); //TODO: remove hack for multiply instance
            CreateInitialData(); //TODO: remove hack for multiply instance
            return !_eventsVotesDictionary.ContainsKey(id) ? -1 : _eventsVotesDictionary[id];
        }

        public bool VoteStaff(int id, int voting)
        {
            try
            {
                if (!_staffVotesDictionary.ContainsKey(id))
                {
                    _staffVotesDictionary.Add(id, voting);
                }
                else
                {
                    return false;
                    //_staffVotesDictionary[id] = voting;
                }
                SetStoredValue(StaffVotesKey, _staffVotesDictionary);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int GetVoteForStaff(int id)
        {
            GetStoredValue(StaffVotesKey, out _staffVotesDictionary); //TODO: remove hack for multiply instance
            CreateInitialData(); //TODO: remove hack for multiply instance
            return !_staffVotesDictionary.ContainsKey(id) ? -1 : _staffVotesDictionary[id];
        }

        public void UserUpdatedHandler([NotNull] object sender, [NotNull] UserUpdatedEventArgs e)
        {
            UpdateUserInfo((UserInfo)e.User);
        }

        public void StaffVotedHandler(object sender, StaffVotedEventArgs e)
        {
            VoteStaff(e.StaffId, e.Vote);
        }

        public void EventVotedHandler(object sender, EventVotedEventArgs e)
        {
            VoteEvent(e.EventId, e.Vote);
        }

        private void CreateInitialData()
        {
            if (_reviewCollectors == null)
                _reviewCollectors = new Dictionary<int, ReviewCollector>();
            if (_staffVotesDictionary == null)
                _staffVotesDictionary = new Dictionary<int, int>();
            if (_eventsVotesDictionary == null)
                _eventsVotesDictionary = new Dictionary<int, int>();

#if DEBUG
            /*
            _currentUser = new UserInfo
            {
                FirstName = "Alex",
                Surname = "Bel",
                SocialId = "FB.10206171803186682",
                UserId = 10,
                UserToken = @"CAAX3Hlp2T38BAOylGbdWZA3EC9wwYLILgyuodWXMospyZCAyhjFk3ugZBD8lR70BaerVM6XhUSmXuQYmDkposjTov2lJOdo0gb8y8lwtowCDNsLAk6XsQGgneuRgWjzoa8ToOZBkZBzOYlZADgNqCxMrtOlhv0Efk0gXfxItfNwhenljnvYZCZBKfPdogbuUEX5BMseyrLjxyZBHgr9zgEPvgz5cZC11OZCyYAZD"
            };*/
#endif
        }

        private void SetStoredValue([NotNull]string key, [NotNull] object value)
        {
            if (_appSettings.Values.ContainsKey(key))
            {
                _appSettings.Values.Remove(key);
            }

            _appSettings.Values.Add(key, JsonConvert.SerializeObject(value));
        }

        private void GetStoredValue<T>([NotNull] string key, out T result) //out for autoresolve generic recognition by signature af arguments
        {
            result = default(T);
            object obj;
            if (_appSettings.Values.TryGetValue(key, out obj))
            {
                result = JsonConvert.DeserializeObject<T>((string)obj);
            }
        }
    }
}
