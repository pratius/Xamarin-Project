using System;
using SloperMobile.ViewModel;
using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            var splahVM = new SplashViewModel(Navigation); ;
            BindingContext = splahVM;
            splahVM.OnConditionNavigation = OnPageNavigation;

        }

        private async void OnPageNavigation(object obj)
        {
            if (Convert.ToString(obj) == "Procced")
                 Device.BeginInvokeOnMainThread(() =>
                                           App.SetMainPage(new LoginPage()));
            else if (Convert.ToString(obj) == "CANCEL")
                Device.BeginInvokeOnMainThread(() =>
                                               App.SetMainPage(new SplashPage()));
        }
    }
}
