using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.DTO;

namespace Model.Runtime.Interfaces
{
    public interface IServiceProvider
    {
        Task<T1> UniversalMethod<T, T1>(T obj, string methodName, string resourceError, object[] @params) where T1 : class;
        Task<ResponseModel<ConferenceModel>> GetConference(int conferenceId);
        Task<ResponseModel<UserModel>> GetConferenceUsers(int conferenceId);
        Task<ResponseModel<EventModel>> GetEvent(int eventId);
        Task<List<EventModel>> GetEvents();
        Task<ResponseModel<RatingModel>> SetRate(RatingModel rating, int eventId);
        Task<ResponseModel<RatingModel>> SetStaffRate(RatingModel rating, int eventId, int staffId);
        Task<ResponseModel<StaffModel>> GetStaff(int staffId);
        Task<ResponseModel<StaffModel>> GetStaff(int eventId, int staffId);
        Task<ResponseModel<UserModel>> GetUser(int userId);
        Task<ResponseModel<UserModel>> GetCurrentUser(int userId);
        Task<ResponseModel<UserModel>> SetUser(UserModel user, int id);
        Task<ResponseModel<UserModel>> AddUser(UserModel user);
        Task<ResponseModel<QuestionnaireModel>> AddReview(int userId, QuestionnaireModel review);
        Task<ResponseModel<MessageModel>> GetMessages(string token);
        Task<ResponseModel<MessageModel>> SendMessage(string token, MessageModel msg);
        Task<ResponseModel<UserModel>> CheckUser(string socialId);
        Task<ResponseModel<UserModel>> AddUserToConf(string token, int conferenceId, UserType type);

        event EventHandler<UserUpdatedEventArgs> UserUpdated;
        event EventHandler<StaffVotedEventArgs> StaffVoted;
        event EventHandler<EventVotedEventArgs> EventVoted;
    }
}
