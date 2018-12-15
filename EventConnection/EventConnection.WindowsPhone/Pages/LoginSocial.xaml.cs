using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Facebook;
using Facebook.Client;
using Facebook.Client.Controls;
using JetBrains.Annotations;
using Microsoft.Practices.Prism.StoreApps;
using Model.DataModel;
using Tools.Extentions;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EventConnection.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginSocial : VisualStateAwarePage
    {
        private ViewModels.Context.LoginSocial _vm;

        public LoginSocial()
        {
            InitializeComponent();

            _vm = (ViewModels.Context.LoginSocial) DataContext;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                LoadBar.SetProgressIndicator(true);
                if (LoginButton.CurrentUser != null)
                {
                    FetchUser(LoginButton.CurrentUser);
                }
                else if(Session.ActiveSession != null && Session.ActiveSession.CurrentAccessTokenData.State != null)
                {
                    await RetriveUserInfo();
                }
            }
            finally
            {
                LoadBar.SetProgressIndicator(false);
            }
        }

        private void LoginButton_OnUserInfoChanged(object sender, UserInfoChangedEventArgs e)
        {
            FetchUser(e.User);
        }

        private async Task RetriveUserInfo()
        {
            var client = new FacebookClient(Session.ActiveSession.CurrentAccessTokenData.AccessToken);

            dynamic result = await client.GetTaskAsync("me"); //TODO: me?fields=picture p-o-c
            var currentUser = new GraphUser(result);

            FetchUser(currentUser);
        }

        private async void FetchUser([NotNull] GraphUser user)
        {
            var socId = "FB." + user.Id; //TODO: move to consts
            var userExist = false; //await _vm.GetIdIfExist(socId); //TODO: return

            if (userExist)
            {
                Frame.Navigate(typeof(EventsList));//TODO: Use PRISM navigation service
                return;
            }

            var userModel = new UserInfo //TODO: it comes empty
            {
                FirstName = user.FirstName,
                Surname = user.LastName,
                Patronymic = user.MiddleName,
                UserToken = Session.ActiveSession.CurrentAccessTokenData.AccessToken,
                UserTokenExpiration = Session.ActiveSession.CurrentAccessTokenData.Expires.GetUnixTimeStamp(),
                SocialId = socId,
                Email = (string) user["email"]//,
                //Photo = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", user.Id, "square", Session.ActiveSession.CurrentAccessTokenData.AccessToken) //TODO
        };
            Frame.Navigate(typeof (LoginPageForm), userModel);//TODO: Use PRISM navigation service
        }

        private async void LoginButton_OnSessionStateChanged(object sender, SessionStateChangedEventArgs e)
        {
             if (e.SessionState == FacebookSessionState.Opened)
            {
                await RetriveUserInfo();
            }
        }
    }
}
