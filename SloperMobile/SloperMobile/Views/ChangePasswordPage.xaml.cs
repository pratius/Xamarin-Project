using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        private ChangePasswordViewModel _changePasswordVM;
        public ChangePasswordPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _changePasswordVM = new ChangePasswordViewModel(Navigation);
            BindingContext = _changePasswordVM;
            _changePasswordVM.OnPageNavigation = ChangePasswordVM_OnLogout;
        }
        private async void ChangePasswordVM_OnLogout()
        {
            await Navigation.PushAsync(new MenuNavigationPage());
        }

        private async void OnBackTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}