using System;
using Model.DataModel;

namespace Model.Runtime.Interfaces
{
    public interface IDataProvider
    {
        UserInfo CurrentUser { get; }
        ReviewCollector Review { get; set; }

        bool UpdateUserInfo(UserInfo user);
        //bool UpdateReview(ReviewCollector review);

        bool VoteEvent(int id, int voting);
        int GetVoteForEvent(int id);

        bool VoteStaff(int id, int voting);
        int GetVoteForStaff(int id);

        void UserUpdatedHandler(object sender, UserUpdatedEventArgs e);
        void StaffVotedHandler(object sender, StaffVotedEventArgs e);
        void EventVotedHandler(object sender, EventVotedEventArgs e);
    }
}
