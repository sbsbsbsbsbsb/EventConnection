using System.Collections.Generic;
using System.Threading.Tasks;
using Model.DTO;

namespace Model.Runtime.Interfaces
{
    public interface IBusinessDataProvider
    {
        Task<ConferenceModel> GetConference(int conferenceId);
        Task<List<UserModel>> GetConferenceUsers(int conferenceId);
        Task<EventModel> GetEvent(int eventId);
        Task<List<EventModel>> GetEvents();
        Task<bool> SetRate(int rating, int eventId);//success
        Task<bool> SetStaffRate(int rating, int eventId, int staffId);//success
        Task<StaffModel> GetStaff(int staffId);
        Task<StaffModel> GetStaff(int eventId, int staffId);
        Task<UserModel> GetUser(int userId);
        Task<UserModel> GetCurrentUser(int userId);
        Task<bool> SetUser(UserModel user, int id); //success
        Task<int> AddUser(UserModel user); //id
        Task<int> AddReview(int userId, QuestionnaireModel review); //id
        Task<List<MessageModel>> GetMessages(string token);
        Task<int> SendMessage(string token, MessageModel msg);//id
        Task<int> CheckUser(string socialId);//id
        Task<bool> AddUserToConf(string token, int conferenceId, UserType type); //success
    }
}