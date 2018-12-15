using System;
using System.Net;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Model.DataModel;
using Model.DTO;
using Tools.Extentions;
using Tools.ResourcesSupport;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EventConnection.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPageForm : VisualStateAwarePage
    {
        private ViewModels.Context.LoginPageForm _vm;

        public LoginPageForm()
        {
            InitializeComponent();

            _vm = (ViewModels.Context.LoginPageForm)DataContext;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = e.Parameter as UserInfo;
            if (param != null)
            {
                _vm.Model.GetInfoFromDataModel(param);//todo ensure user is logon in current conferencion
            }
        }

        //TODO: update options of select to
        //case OTHER = "Прочее"
        //case IT_COMPANY = "ИТ-Компания"
        //case PARTICIPANT = "Участник"
        //case MEDIA = "СМИ"

        //TODO: register user in conferecion

        private async void OnSubmit(object sender, RoutedEventArgs e)
        {
            _vm.Model.ValidateAll();
            if (_vm.Model.HasValidationMessages())
                return;

            try
            {
                LoadBar.SetProgressIndicator(true);
                await _vm.CreateUser();
            }
            finally
            {
                LoadBar.SetProgressIndicator(false);
            }
        }
    }
}
