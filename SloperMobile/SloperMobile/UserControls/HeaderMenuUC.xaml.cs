using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Common.Enumerators;
using SloperMobile.Views;
using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class HeaderMenuUC : ContentView
    {
        public HeaderMenuUC()
        {
            InitializeComponent();
        }

        private async void OnNavigation(object sender, EventArgs e)
        {
            try
            {
                var arg = e as TappedEventArgs;
                var param = arg.Parameter as string;
                if (string.IsNullOrEmpty(param))
                    return;
                var pageType = (ApplicationActivity)Enum.Parse(typeof(ApplicationActivity), param);
                await PageNavigation(pageType);
            }
            catch (Exception ex)
            {


            }
        }

        private async Task PageNavigation(ApplicationActivity page)
        {
            try
            {
                switch (page)
                {
                    case ApplicationActivity.ProfilePage:
                        await Navigation.PushAsync(new SendsPage("TICKLIST"));
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
