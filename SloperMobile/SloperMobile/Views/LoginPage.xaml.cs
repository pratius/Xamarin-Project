﻿using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            loginViewModel = new UserViewModel();
            BindingContext = loginViewModel;
            loginViewModel.OnPageNavigation = LoginViewModel_OnLoginClick;
            var vDB = App.DAUtil;
        }

        private async void LoginViewModel_OnLoginClick()
        {
            await Navigation.PushAsync(new MenuNavigationPage());
        }

        private async void OnSignUP(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }
    }
}
