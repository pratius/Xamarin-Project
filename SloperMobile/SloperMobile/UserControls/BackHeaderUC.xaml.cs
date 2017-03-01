using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class BackHeaderUC : ContentView
    {
        public BackHeaderUC()
        {
            InitializeComponent();
        }
        private async void OnBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync().ConfigureAwait(false);
        }
        private async void OnSearch(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.SearchPage());
        }
    }
}
