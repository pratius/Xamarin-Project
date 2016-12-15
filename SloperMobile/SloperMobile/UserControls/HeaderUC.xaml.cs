using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class HeaderUC : ContentView
    {
        public HeaderUC()
        {
            InitializeComponent();
        }


        private void OnMenuTapped(object sender, EventArgs e)
        {

        }

        private async void OnSearch(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.SearchPage());
        }




    }
}
