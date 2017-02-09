using Newtonsoft.Json;
using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel()
        {
            LoginCommand = new DelegateCommand(ExecuteOnLogin);
            RegistrationCommand = new DelegateCommand(ExecuteOnRegistration);
            LoginReq = new LoginReq();
            RegistrationReq = new RegistrationReq();
        }


        #region Properties
        private LoginReq loginReq;
        /// <summary>
        /// /Get or set the login required 
        /// </summary>
        public LoginReq LoginReq
        {
            get { return loginReq; }
            set { loginReq = value; OnPropertyChanged(); }
        }

        private RegistrationReq registrationReq;
        /// <summary>
        /// Get or set the Registration Required 
        /// </summary>
        public RegistrationReq RegistrationReq
        {
            get { return registrationReq; }
            set { registrationReq = value; OnPropertyChanged(); }
        }

        private string confirmPassword;
        /// <summary>
        /// Get or set the confirm Password
        /// </summary>
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Returns app's last updated date.
        /// </summary>
        public string AppLastUpdateDate
        {
            get
            {
                string lastupdate = App.DAUtil.GetLastUpdate();
                if (string.IsNullOrEmpty(lastupdate))
                {
                    lastupdate = "20160101";
                }
                return lastupdate;
            }
        }

        #endregion

        #region DelegateCommand
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand RegistrationCommand { get; set; }
        #endregion


        #region Methods/Functions 
        private async void ExecuteOnLogin(object parma)
        {
            if (Convert.ToString(parma) == "Guest")
            {
                LoginReq.u = AppConstant.Guest_UserId;
                LoginReq.p = AppConstant.Guest_UserPassword;
            }
            if (string.IsNullOrWhiteSpace(LoginReq.u) || string.IsNullOrWhiteSpace(LoginReq.p))
            {
                await Application.Current.MainPage.DisplayAlert("Login", "Please enter the Email Id and Password", "OK");
                return;
            }
            if (Convert.ToString(parma) != "Guest")
            {
                if (!Helper.IsEmailValid(loginReq.u))
                {
                    await Application.Current.MainPage.DisplayAlert("Login", "Invalid Email Id", "OK");
                    return;
                }

            }

            if (!IsRunningTasks)
            {
                IsRunningTasks = true;
                HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_Login, string.Empty);
                var loginjson = JsonConvert.SerializeObject(LoginReq);
                var response = await apicall.Post<LoginResponse>(loginjson);
                if (response != null)
                {
                    if (response.accessToken != null && response.renewalToken != null)
                    {
                        Settings.AccessTokenSettings= response.accessToken;
                        Settings.RenewalTokenSettings = response.renewalToken;
                        Settings.DisplayNameSettings = response.displayName;
                        var climbdays=await HttpGetClimbdays();
                        if (climbdays != null)
                        {
                            Settings.ClimbingDaysSettings = Convert.ToInt32(climbdays[0].climbing_days);
                        }
                        OnPageNavigation?.Invoke();
                        DisposeObject();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                }
            }
            IsRunningTasks = false;
        }

        private async void ExecuteOnRegistration(object obj)
        {
            var isValidate = await IsRegistrationValidation();

            if (!IsRunningTasks && isValidate)
            {
                IsRunningTasks = true;
                HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_User_Register, string.Empty);
                RegistrationReq.Email = RegistrationReq.UserName;
                RegistrationReq.DisplayName = RegistrationReq.FirstName + " " + RegistrationReq.LastName;
                var regjson = JsonConvert.SerializeObject(RegistrationReq);
                var response = await apicall.Post<RgistrationResponse>(regjson);
                if (response != null)
                {
                    if (!string.IsNullOrEmpty(response.successful) && response.successful == "true")
                    {

                        await Application.Current.MainPage.DisplayAlert("Registration", response.message, "OK");
                        DisposeObject();
                        OnPageNavigation?.Invoke();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Registration", response.message, "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Registration", AppConstant.REGISTRATION_FAILURE, "OK");
                }
            }
            IsRunningTasks = false;
        }

        private async Task<bool> IsRegistrationValidation()
        {

            if (string.IsNullOrWhiteSpace(RegistrationReq.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration", "Please enter the First Name ", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(RegistrationReq.LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration", "Please enter Last Name", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(RegistrationReq.UserName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration", "Please enter the Email Id", "OK");
                return false;
            }
            else if (!Helper.IsEmailValid(RegistrationReq.UserName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration", "Invalid Email Id", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(RegistrationReq.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Registration", "Please enter the Password", "OK");
                return false;
            }
            else if (RegistrationReq.Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Registration", "Enter Password is not Matching", "OK");
                return false;
            }
            return true;
        }

        private void DisposeObject()
        {
            RegistrationReq = new RegistrationReq();
            LoginReq = new LoginReq();
            ConfirmPassword = string.Empty;
        }
        #endregion
        #region Service
        private async Task<List<ClimbingDaysModel>> HttpGetClimbdays()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppConstant.APP_ID, AppLastUpdateDate, "ascent"), Settings.AccessTokenSettings);
            var area_response = await apicall.Get<ClimbingDaysModel>();
            return area_response;
        }
        #endregion
    }
}
