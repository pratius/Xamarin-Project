using SloperMobile.Common.Enumerators;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class FooterUC : ContentView
    {
        public FooterUC()
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
                    case ApplicationActivity.NewsPage:
                        await Navigation.PushAsync(new NewsPage());
                        break;
                    case ApplicationActivity.ProfilePage:
                        await Navigation.PushAsync(new SendsPage("SENDS"));
                        break;
                    case ApplicationActivity.HomePage:
                        await Navigation.PushAsync(new HomePage());
                        break;
                    case ApplicationActivity.MapPage:
                        await Navigation.PushAsync(new MapPage());
                        break;
                    case ApplicationActivity.SettingsPage:
                        await Navigation.PushAsync(new SettingsPage());
                        break;
                }
            }
            catch (Exception ex)
            {
            
            }


        }
    }
}
