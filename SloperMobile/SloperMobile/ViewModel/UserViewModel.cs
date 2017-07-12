using Acr.UserDialogs;
using Newtonsoft.Json;
using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Connectivity;
namespace SloperMobile.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        private INavigation _navigation;
        public UserViewModel(INavigation navigation)
        {
            _navigation = navigation;
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
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (Convert.ToString(parma) == "Guest")
                    {
                        LoginReq.u = AppSetting.Guest_UserId;
                        LoginReq.p = AppSetting.Guest_UserPassword;
                    }
                    if (string.IsNullOrWhiteSpace(LoginReq.u) || string.IsNullOrWhiteSpace(LoginReq.p))
                    {
                        await Application.Current.MainPage.DisplayAlert("Login Error", "Enter both Email and Password.", "OK");
                        return;
                    }
                    if (Convert.ToString(parma) != "Guest")
                    {
                        //check lowercase username, as they are stored in the database all lowercase.
                        if (!Helper.IsEmailValid(loginReq.u.ToLower()))
                        {
                            await Application.Current.MainPage.DisplayAlert("Login Error", "Account not found, try again.", "OK");
                            return;
                        }
                    }

                    //IsRunningTasks = true;
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_Login, string.Empty);
                    var loginjson = JsonConvert.SerializeObject(LoginReq);
                    var response = await apicall.Post<LoginResponse>(loginjson);
                    if (response != null)
                    {
                        if (response.accessToken != null && response.renewalToken != null)
                        {
                            Settings.AccessTokenSettings = response.accessToken;
                            Settings.RenewalTokenSettings = response.renewalToken;
                            Settings.DisplayNameSettings = response.displayName;
                            var climbdays = await HttpGetClimbdays();
                            if (climbdays != null)
                            {
                                Settings.ClimbingDaysSettings = Convert.ToInt32(climbdays[0].climbing_days);
                            }
                            OnPageNavigation?.Invoke();
                            DisposeObject();
                            UserDialogs.Instance.HideLoading();
                            return;
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                            return;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                        return;
                    }

                    //IsRunningTasks = false;
                }
                else
                    await _navigation.PushAsync(new Views.NetworkErrorPage());
            }
            catch (Exception ex)
            {
                //IsRunningTasks = false;
                UserDialogs.Instance.HideLoading();
                //await _navigation.PushAsync(new Views.NetworkErrorPage());
                await Application.Current.MainPage.DisplayAlert("Login Failure", "Incorrect username/password. Please try again.", "OK");
                return;
            }
        }

        private async void ExecuteOnRegistration(object obj)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var isValidate = await IsRegistrationValidation();

                //if (!IsRunningTasks && isValidate)
                if (isValidate)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    //IsRunningTasks = true;
                    HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_User_Register, string.Empty);
                    RegistrationReq.Email = RegistrationReq.UserName;
                    RegistrationReq.DisplayName = RegistrationReq.FirstName + " " + RegistrationReq.LastName;
                    var regjson = JsonConvert.SerializeObject(RegistrationReq);
                    var response = await apicall.Post<RgistrationResponse>(regjson);
                    if (response != null)
                    {
                        if (!string.IsNullOrEmpty(response.successful) && response.successful == "true")
                        {

                            //await Application.Current.MainPage.DisplayAlert("Registration", response.message, "OK");

                            HttpClientHelper apilogin = new HttpClientHelper(ApiUrls.Url_Login, string.Empty);
                            LoginReq.u = RegistrationReq.UserName;
                            LoginReq.p = RegistrationReq.Password;
                            var loginjson = JsonConvert.SerializeObject(LoginReq);
                            var logresponse = await apilogin.Post<LoginResponse>(loginjson);
                            if (logresponse != null)
                            {
                                if (logresponse.accessToken != null && logresponse.renewalToken != null)
                                {
                                    Settings.AccessTokenSettings = logresponse.accessToken;
                                    Settings.RenewalTokenSettings = logresponse.renewalToken;
                                    Settings.DisplayNameSettings = logresponse.displayName;
                                    var climbdays = await HttpGetClimbdays();
                                    if (climbdays != null)
                                    {
                                        Settings.ClimbingDaysSettings = Convert.ToInt32(climbdays[0].climbing_days);
                                    }
                                    OnPageNavigation?.Invoke();
                                    DisposeObject();
                                    UserDialogs.Instance.HideLoading();
                                    return;
                                }
                                else
                                {
                                    UserDialogs.Instance.HideLoading();
                                    await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                                    return;
                                }
                            }

                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Registration", response.message, "OK");
                            return;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await Application.Current.MainPage.DisplayAlert("Registration", AppConstant.REGISTRATION_FAILURE, "OK");
                        return;
                    }
                }

                //IsRunningTasks = false;
            }
            else
                await _navigation.PushAsync(new Views.NetworkErrorPage());
        }

        private async Task<bool> IsRegistrationValidation()
        {

            if (string.IsNullOrWhiteSpace(RegistrationReq.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "First Name required, try again.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(RegistrationReq.LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Last Name required, try again.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(RegistrationReq.UserName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Email Address required, try again.", "OK");
                return false;
            }
            else if (!Helper.IsEmailValid(RegistrationReq.UserName.ToLower()))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Email Address already taken.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(RegistrationReq.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Password required, try again.", "OK");
                return false;
            }
            else if (RegistrationReq.Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Passwords do not match, try again.", "OK");
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
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetInitialUpdate_AppData, AppSetting.APP_ID, AppLastUpdateDate, "ascent", true), Settings.AccessTokenSettings);
            var area_response = await apicall.Get<ClimbingDaysModel>();
            return area_response;
        }
        #endregion
    }
}
