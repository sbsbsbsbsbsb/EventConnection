using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Common;
using JetBrains.Annotations;
using Model.DTO;
using Model.Runtime;
using Model.Runtime.Interfaces;
using Newtonsoft.Json;
using Tools.ResourcesSupport;
using Wukker.Tools;
using IServiceProvider = Model.Runtime.Interfaces.IServiceProvider;

namespace Core.Providers
{
    public class RestServiceProvider : IServiceProvider
    {
        private readonly string _mainPath = ConfigManager.GetMainServiceUrl();
        private readonly TimeSpan _delay = new TimeSpan(0, 0, 0, 30);
        private readonly CacheProvider _cache = new CacheProvider();
        
        public RestServiceProvider(IDataProvider dataProvider) //TODO: remove this "hack" //error occures because it created twice when init
        {
            UserUpdated += dataProvider.UserUpdatedHandler;
            EventVoted += dataProvider.EventVotedHandler;
            StaffVoted += dataProvider.StaffVotedHandler;

        }

        public async Task<T1> UniversalMethod<T, T1>([CanBeNull] T obj, [NotNull] string methodName,
            [NotNull] string resourceError, [CanBeNull] object[] @params) where T1 : class
        {
            return await UniversalMethod<T, T1>(obj, methodName, resourceError, @params, false);
        }


        //TODO: Make request-bulder and layer for error handling
        public async Task<T1> UniversalMethod<T, T1>([CanBeNull] T obj, [NotNull] string methodName,
            [NotNull] string resourceError, [CanBeNull] object[] @params, bool asQueryString, string contentType = "application/json") where T1 : class
        {
            using (var client = new HttpClient())
            {
                var methodMask = ConfigManager.GetMainServiceMethods()
                    .First(x => x.Name == methodName);

                var url = @params == null
                    ? _mainPath + methodMask.PathMask
                    : string.Format(string.Concat(_mainPath, methodMask.PathMask), @params);

                var uri = new Uri(url);
                client.Timeout = _delay;

                HttpContent contentPost;
                if (methodMask.Protocol == HttpMethod.Get)
                    contentPost = null;
                else
                {
                    string param = null;

                    if (!asQueryString)
                        param = obj != null ? JsonConvert.SerializeObject(obj) : string.Empty;

                    if (asQueryString && obj != null)
                    {
                        var objDict = obj as Dictionary<string, string>;

                        if (objDict != null)
                        {
                            var parameters = new List<string>(objDict.Count);
                            parameters.AddRange(objDict.Select(kvp => $"{kvp.Key}={kvp.Value}"));

                            param = string.Join("&", parameters);
                        } //TODO: refactor
                    }

                    contentPost = new StringContent(param, Encoding.UTF8, contentType);
                }

                var reqMsg = new HttpRequestMessage(methodMask.Protocol, uri) { Content = contentPost };//TODO: remove
                if (string.IsNullOrEmpty(reqMsg.RequestUri.ToString()))
                {
                    reqMsg = new HttpRequestMessage(methodMask.Protocol, uri) { Content = contentPost };
                }

                //var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes("user:12QWasZX"));
                //reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
                var responseSend = client.SendAsync(reqMsg).ConfigureAwait(false);
                var response = await responseSend;
                object result;
                try
                {

                    string responseBody = await response.Content.ReadAsStringAsync();

                    //response.EnsureSuccessStatusCode(); //TODO: restore logic on server side fixes

                    //if (!response.IsSuccessStatusCode) throw new WebException(resourceError); //TODO: restore logic on server side fixes


                    result = typeof(T1).GetTypeInfo().IsAssignableFrom(typeof(string).GetTypeInfo())
                        ? responseBody
                        : (object)JsonConvert.DeserializeObject<T1>(responseBody);
                    //hack for T1 error convertation to string
                }
                catch (HttpRequestException)
                {
                    //TODO: Implement behavior for 405 and other errors
                    //throw new WebException(AppResources.GetResource("ErrorSessionExpired"));
                    return null;
                }
                catch (Exception)
                {
                    throw new WebException(resourceError);
                }

                return (T1)result;
            }
        }

        public async Task<ResponseModel<ConferenceModel>> GetConference(int conferenceId)
        {
            var result = await UniversalMethod<object, ResponseModel<ConferenceModel>>(null, ProjectConsts.GetConferenceMethodName,
                AppResources.GetResource("Error"), new object[] { conferenceId });

            _cache.Conference = result.FirstResponce;

            return result;
        }

        public async Task<ResponseModel<UserModel>> GetConferenceUsers(int conferenceId)
        {
            return await UniversalMethod<object, ResponseModel<UserModel>>(null, ProjectConsts.GetConferenceUsersMethodName,
                AppResources.GetResource("Error"), new object[] { conferenceId });
        }

        public async Task<List<EventModel>> GetEvents()
        {
            var cachedList = _cache.GetEventsList();
            if (cachedList != null) return cachedList;

            if (_cache.Conference?.Id == null)
            {
                return new List<EventModel>();
            }

            var result = await GetConference(_cache.Conference.Id.Value);

            _cache.Conference = result.FirstResponce;

            return _cache.Conference.Events;
        }

        public async Task<ResponseModel<EventModel>> GetEvent(int eventId)
        {
            var cachedEvent = _cache.GetEvent(eventId);
            if (cachedEvent != null) return new ResponseModel<EventModel> { HasErrors = false, FirstResponce = cachedEvent };

            var result =
                await UniversalMethod<object, ResponseModel<EventModel>>(null, ProjectConsts.GetEventMethodName,
                    AppResources.GetResource("Error"), new object[] { eventId });

            _cache.EventsList.Add(result.FirstResponce);

            return result;
        }

        public async Task<ResponseModel<RatingModel>> SetRate([NotNull] RatingModel rating, int eventId)
        {
            var result = await UniversalMethod<RatingModel, ResponseModel<RatingModel>>(rating, ProjectConsts.SetRateMethodName,
                AppResources.GetResource("Error"), new object[] { eventId });

            OnEventVoted(new EventVotedEventArgs { EventId = eventId, Vote = rating.Rating });

            return result;
        }

        public async Task<ResponseModel<RatingModel>> SetStaffRate([NotNull] RatingModel rating, int eventId, int staffId)
        {
            var result = await UniversalMethod<RatingModel, ResponseModel<RatingModel>>(rating, ProjectConsts.SetStaffRateMethodName,
            AppResources.GetResource("Error"), new object[] { eventId, staffId });

            OnStaffVoted(new StaffVotedEventArgs { StaffId = staffId, Vote = rating.Rating });

            return result;
        }

        public async Task<ResponseModel<StaffModel>> GetStaff(int eventId, int staffId)
        {
            await GetEvent(eventId); //ensure that current event in cache


            var staff = _cache.GetStaffFromEvent(eventId, staffId);
            if (staff == null)
            {
                throw new ArgumentException($"There is no person with ID {eventId} in event with ID {staffId}", nameof(staffId));
                //TODO: ensure this point
            }
            return new ResponseModel<StaffModel> { HasErrors = false, FirstResponce = staff };
        }

        public async Task<ResponseModel<StaffModel>> GetStaff(int staffId)
        {
            var cachedStuff = _cache.GetStaff(staffId);
            if (cachedStuff != null) return new ResponseModel<StaffModel> { HasErrors = false, FirstResponce = cachedStuff };

            var result =
                await UniversalMethod<object, ResponseModel<StaffModel>>(null, ProjectConsts.GetEventMethodName,
                    AppResources.GetResource("Error"), new object[] { staffId });

            _cache.StaffList.Add(result.FirstResponce);

            return result;
        }

        public async Task<ResponseModel<UserModel>> GetUser(int userId)
        {
            return await UniversalMethod<object, ResponseModel<UserModel>>(null, ProjectConsts.GetUserMethodName,
                AppResources.GetResource("Error"), new object[] { userId });
        }

        public async Task<ResponseModel<UserModel>> GetCurrentUser(int userId)
        {
            var result = await GetUser(userId);
            OnUserChanged(new UserUpdatedEventArgs { User = result.FirstResponce });

            return result;
        }

        public async Task<ResponseModel<UserModel>> SetUser([NotNull] UserModel user, int id)
        {
            var result =
                await UniversalMethod<UserModel, ResponseModel<UserModel>>(user, ProjectConsts.SetUserMethodName,
                    AppResources.GetResource("Error"), new object[] {id});

            OnUserChanged(new UserUpdatedEventArgs { User = result.FirstResponce });

            return result;
        }

        public async Task<ResponseModel<UserModel>> AddUser([NotNull] UserModel user)
        {
            var result = await UniversalMethod<UserModel, ResponseModel<UserModel>>(user, ProjectConsts.AddUserMethodName,
                AppResources.GetResource("Error"), null);

            OnUserChanged(new UserUpdatedEventArgs { User = result.FirstResponce });

            return result;
        }

        public async Task<ResponseModel<QuestionnaireModel>> AddReview(int userId, [NotNull] QuestionnaireModel review)
        {
            return await UniversalMethod<QuestionnaireModel, ResponseModel<QuestionnaireModel>>(review, ProjectConsts.AddReviewMethodName,
                AppResources.GetResource("Error"), new object[] { userId });
        }

        public async Task<ResponseModel<MessageModel>> GetMessages(string token)
        {
            var result = await UniversalMethod<object, ResponseModel<MessageModel>>(null, ProjectConsts.GetMessagesMethodName,
                AppResources.GetResource("Error"), new object[] { token });

            return result;
        }

        public async Task<ResponseModel<MessageModel>> SendMessage(string token, [NotNull] MessageModel msg)
        {
            var result = await UniversalMethod<MessageModel, ResponseModel<MessageModel>>(msg, ProjectConsts.SendMessageMethodName,
                AppResources.GetResource("Error"), new object[] { token });

            return result;
        }

        public async Task<ResponseModel<UserModel>> CheckUser(string socialId)
        {
            var result = await UniversalMethod<object, ResponseModel<UserModel>>(null, ProjectConsts.CheckUserMethodName,
                AppResources.GetResource("Error"), new object[] { socialId });

            return result;
        }

        public async Task<ResponseModel<UserModel>> AddUserToConf(string token, int conferenceId, UserType type)
        {
            var result = await UniversalMethod<Dictionary<string, string>, ResponseModel<UserModel>>(new Dictionary<string, string>(1) { { "type", type.GetLocalisedDescription() } }, ProjectConsts.AddUserToConfMethodName, //TODO: move to enums
                AppResources.GetResource("Error"), new object[] { token, conferenceId },
                true, "application/x-www-form-urlencoded");

            return result;
        }

        private void OnUserChanged([NotNull] UserUpdatedEventArgs e)
        {
            var handler = UserUpdated;
            handler?.Invoke(this, e);
        }

        private void OnEventVoted([NotNull] EventVotedEventArgs e)
        {
            var handler = EventVoted;
            handler?.Invoke(this, e);
        }

        private void OnStaffVoted([NotNull] StaffVotedEventArgs e)
        {
            var handler = StaffVoted;
            handler?.Invoke(this, e);
        }


        public event EventHandler<UserUpdatedEventArgs> UserUpdated;
        public event EventHandler<StaffVotedEventArgs> StaffVoted;
        public event EventHandler<EventVotedEventArgs> EventVoted;
    }
}
