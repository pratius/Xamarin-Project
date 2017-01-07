using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            var splahVM = new SplashViewModel(); ;
            BindingContext = splahVM;
            splahVM.OnConditionNavigation = OnPageNavigation;

        }

        private async void OnPageNavigation(object obj)
        {
            if (Convert.ToString(obj) == "Procced")
                await Navigation.PushAsync(new LoginPage());
            else if (Convert.ToString(obj) == "Cancel")
                await Navigation.PopAsync();


        }
    }
}
