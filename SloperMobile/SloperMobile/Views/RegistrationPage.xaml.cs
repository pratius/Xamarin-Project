using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class RegistrationPage : ContentPage
    {
        UserViewModel usrVM;
        public RegistrationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            usrVM = new UserViewModel();
            BindingContext = usrVM;
            usrVM.PageNavigation = LoginViewModel_OnLoginClick;
        }

        private async void LoginViewModel_OnLoginClick()
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
