using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NetworkErrorPage : ContentPage
    {
        public NetworkErrorPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OnContinue(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }
    }
}
