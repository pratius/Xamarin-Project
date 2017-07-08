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
    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public ChangePasswordViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ChangePasswordCommand = new DelegateCommand(ExecuteOnChangePassword);
            ChangePasswordReq = new ChangePasswordModel();
        }
        #endregion

        #region Properties
        private ChangePasswordModel _changePasswordReq;
        public ChangePasswordModel ChangePasswordReq
        {
            get { return _changePasswordReq; }
            set { _changePasswordReq = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delegate Command
        public DelegateCommand ChangePasswordCommand { get; set; }
        #endregion

        #region Methods/Functions
        private async void ExecuteOnChangePassword(object obj)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var isValidate = await IsRegistrationValidation();
                if (isValidate)
                {
                    UserDialogs.Instance.ShowLoading("Please Wait...", MaskType.Black);
                    HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_User_ChangePassword, Settings.AccessTokenSettings);
                    var changepasswordjson = JsonConvert.SerializeObject(ChangePasswordReq);
                    var response = await apicall.Post<ChangePasswordResponse>(changepasswordjson);
                    if (response != null)
                    {
                        if (response.status != null && response.status == "true")
                        {
                            UserDialogs.Instance.Loading().Hide();
                            await Application.Current.MainPage.DisplayAlert(response.message, response.data, "OK");
                            DisposeObject();
                            OnPageNavigation?.Invoke();
                            UserDialogs.Instance.HideLoading();
                        }
                        else if (response.status != null && response.status == "false")
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Error", response.message, "OK"); ;
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Error", AppConstant.CHANGEPASSWORD_FAILURE, "OK");
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await Application.Current.MainPage.DisplayAlert("Change Password", AppConstant.CHANGEPASSWORD_FAILURE, "OK");
                        return;
                    }
                }
            }
            else
                await _navigation.PushAsync(new NetworkErrorPage());
        }
        private async Task<bool> IsRegistrationValidation()
        {
            if (string.IsNullOrWhiteSpace(ChangePasswordReq.currentpassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Current Password required, try again.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(ChangePasswordReq.newpassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "New Password required, try again.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(ChangePasswordReq.confirmpassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Confirm Password required, try again.", "OK");
                return false;
            }
            else if (!ChangePasswordReq.newpassword.Equals(ChangePasswordReq.confirmpassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "New Password and Confirm Password must same, try again.", "OK");
                return false;
            }
            return true;
        }
        private void DisposeObject()
        {
            ChangePasswordReq = new ChangePasswordModel();
        }
        #endregion
    }
}
