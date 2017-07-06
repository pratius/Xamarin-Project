using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Connectivity;
using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class ResetPasswordViewModel : BaseViewModel 
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public ResetPasswordViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ResetPasswordCommand = new DelegateCommand(OnResetPasswordAsync);
        }
        #endregion

        #region Properties
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value;OnPropertyChanged(); }
        }
        #endregion

        #region Delegate Commands
        public DelegateCommand ResetPasswordCommand { get; set; }
        #endregion

        #region Methods 
        private async void OnResetPasswordAsync(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (string.IsNullOrWhiteSpace(Email))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Enter Email.", "OK");
                        return;
                    }
                    UserDialogs.Instance.ShowLoading("Please Wait...", MaskType.Black);
                    HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_User_ResetPassword, Email),String.Empty);
                    var response = apicall.GetResponse<ResetPasswordModel>();
                    if (response != null)
                    {
                        if(response.Result.message.Equals("success"))
                        {
                            UserDialogs.Instance.Loading().Hide();
                            await Application.Current.MainPage.DisplayAlert(response.Result.message, response.Result.data, "OK");
                            await _navigation.PushAsync(new LoginPage());
                        }
                        else
                        {
                            Email = String.Empty;
                            UserDialogs.Instance.Loading().Hide();
                            await Application.Current.MainPage.DisplayAlert(response.Result.message, response.Result.data, "OK");
                            return;
                        }
                    }
                    UserDialogs.Instance.Loading().Hide();
                }
                else
                {
                    UserDialogs.Instance.Loading().Hide();
                    await Application.Current.MainPage.DisplayAlert("Connection Error", "Internet Connection Lost!!", "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Loading().Hide();
                await Application.Current.MainPage.DisplayAlert("Exception", "Please Enter Valid Email.", "OK");
                return;
            }
        }
        #endregion
    }
}
