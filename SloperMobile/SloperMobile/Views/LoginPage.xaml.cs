using System;
using SloperMobile.Common.Helpers;
using SloperMobile.ViewModel;
using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class LoginPage : ContentPage
    {
        private UserViewModel loginViewModel;
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            loginViewModel = new UserViewModel(Navigation);
            BindingContext = loginViewModel;
            loginViewModel.OnPageNavigation = LoginViewModel_OnLoginClick;
            var vDB = App.DAUtil;
            if (string.IsNullOrEmpty(Settings.SelectedCragSettings))
            {
                Settings.SelectedCragSettings = App.SelectedCrag;
            }
        }

        private void LoginViewModel_OnLoginClick()
        {
            Device.BeginInvokeOnMainThread(() =>
                                           App.SetMainPage(new MenuNavigationPage()));
        }

        private void OnSignUP(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
                                           App.SetMainPage(new RegistrationPage()));
        }

        private void OnForgotPasswordClick(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
                                           App.SetMainPage(new ResetPasswordPage()));
        }
    }
}
