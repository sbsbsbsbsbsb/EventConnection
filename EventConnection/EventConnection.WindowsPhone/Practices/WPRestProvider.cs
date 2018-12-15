using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Popups;
using JetBrains.Annotations;
using Microsoft.Practices.Unity;
using Model.DTO;
using Model.Runtime.Interfaces;
using IServiceProvider = Model.Runtime.Interfaces.IServiceProvider;

namespace EventConnection.Practices
{
    //TODO: refactor class
    public class WPRestProvider : IBusinessDataProvider
    {
        private readonly IServiceProvider _service;
        private readonly IDataProvider _dataProvider;

        [InjectionConstructor]
        public WPRestProvider(IServiceProvider service, IDataProvider dataProvider)
        {
            _service = service;
            _dataProvider = dataProvider;
        }

        public async Task<ConferenceModel> GetConference(int conferenceId)
        {
            var result = await _service.GetConference(conferenceId);

            if (!result.HasErrors) return result.FirstResponce;
            
            await HandleErrors(result);
            return null;
        }

        public async Task<List<UserModel>> GetConferenceUsers(int conferenceId)
        {
            var result = await _service.GetConferenceUsers(conferenceId);

            if (!result.HasErrors) return result.Response;

            await HandleErrors(result);
            return null;
        }

        public async Task<EventModel> GetEvent(int eventId)
        {
            var result = await _service.GetEvent(eventId);

            if (!result.HasErrors) return result.FirstResponce;

            await HandleErrors(result);
            return null;
        }

        public async Task<List<EventModel>> GetEvents()
        {
            return await _service.GetEvents();
        }

        public async Task<bool> SetRate(int rating, int eventId)
        {
            var result = await _service.SetRate(new RatingModel { Rating = rating, UserId = _dataProvider.CurrentUser.UserId.Value }, eventId);

            if (!result.HasErrors) return true;

            await HandleErrors(result);
            return false;
        }

        public async Task<bool> SetStaffRate(int rating, int eventId, int staffId)
        {
            var result = await _service.SetStaffRate(new RatingModel { Rating = rating, UserId = _dataProvider.CurrentUser.UserId.Value }, eventId, staffId);

            if (!result.HasErrors) return true;

            await HandleErrors(result);
            return false;
        }

        public async Task<StaffModel> GetStaff(int staffId)
        {
            var result = await _service.GetStaff(staffId);

            if (!result.HasErrors) return result.FirstResponce;

            await HandleErrors(result);
            return null;
        }

        public async Task<StaffModel> GetStaff(int eventId, int staffId)
        {
            var result = await _service.GetStaff(eventId, staffId);

            if (!result.HasErrors) return result.FirstResponce;

            await HandleErrors(result);
            return null;
        }

        public async Task<UserModel> GetUser(int userId)
        {
            var result = await _service.GetUser(userId);

            if (!result.HasErrors) return result.FirstResponce;

            await HandleErrors(result);
            return null;
        }

        public async Task<UserModel> GetCurrentUser(int userId)
        {
            var result = await _service.GetCurrentUser(userId);

            if (!result.HasErrors) return result.FirstResponce;

            await HandleErrors(result);
            return null;
        }

        public async Task<bool> SetUser([NotNull] UserModel user, int id)
        {
            var result = await _service.SetUser(user, id);

            if (!result.HasErrors) return true;

            await HandleErrors(result);
            return false;
        }

        public async Task<int> AddUser([NotNull] UserModel user)
        {
            var result = await _service.AddUser(user);

            if (!result.HasErrors) return result.FirstResponce.Id ?? -1;

            await HandleErrors(result);
            return -1;
        }

        public async Task<int> AddReview(int userId, [NotNull] QuestionnaireModel review)
        {
            var result = await _service.AddReview(userId, review);

            if (!result.HasErrors) return result.FirstResponce.Id ?? -1;

            await HandleErrors(result);
            return -1;
        }

        public async Task<List<MessageModel>> GetMessages(string token)
        {
            var result = await _service.GetMessages(token);

            if (!result.HasErrors) return result.Response;

            await HandleErrors(result);
            return null;
        }

        public async Task<int> SendMessage(string token, [NotNull] MessageModel msg)
        {
            var result = await _service.SendMessage(token, msg);

            if (!result.HasErrors) return result.FirstResponce.Id?? -1;

            await HandleErrors(result);
            return -1;
        }

        public async Task<int> CheckUser(string socialId)
        {
            var result = await _service.CheckUser(socialId);

            if (!result.HasErrors) return result.FirstResponce.Id ?? -1;

            await HandleErrors(result);
            return -1;
        }

        public async Task<bool> AddUserToConf(string token, int conferenceId, UserType type)
        {
            var result = await _service.AddUserToConf(token, conferenceId, type);

            if (!result.HasErrors) return true;

            await HandleErrors(result);
            return false;
        }

        private static Task HandleErrors<T>([NotNull] ResponseModel<T> result) where T : class
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ThreadPool.RunAsync(operation => UIDispatcher.Execute(async () =>
            {
                try
                {
                    MessageDialog mDialog = new MessageDialog(result.ErrorTrace);
                    mDialog.Commands.Add(new UICommand("ОК"));
                    /*mDialog.Commands.Add(new UICommand("Повтор", delegate
                    {
                        goto Start_GetConference; //TODO: implement repeat logic
                    }));*/
                    await mDialog.ShowAsync();
                }
                catch
                {
                    //ignore
                }
            }));
            return Task.FromResult<object>(null);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}
