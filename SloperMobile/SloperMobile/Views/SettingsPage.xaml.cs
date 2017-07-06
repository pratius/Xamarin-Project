using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class SettingsPage : ContentPage
    {
        private SettingViewModel settingVM;
        public SettingsPage()
        {
            InitializeComponent();
            settingVM = new SettingViewModel();
            BindingContext = settingVM;
            settingVM.OnPageNavigation = SettingViewModel_OnLogoutClick;
        }

        private async void SettingViewModel_OnLogoutClick()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }
    }
}
