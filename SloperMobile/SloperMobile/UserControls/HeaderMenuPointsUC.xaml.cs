using SloperMobile.Common.Enumerators;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SloperMobile.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderMenuPointsUC : ContentView
    {
        public HeaderMenuPointsUC()
        {
            InitializeComponent();
        }
        private async void OnNavigation(object sender, EventArgs e)
        {
            try
            {
                Label btn = (Label)sender;
                var tabtxt = btn.Text.Trim();
                var arg = e as TappedEventArgs;
                var param = arg.Parameter as string;
                if (string.IsNullOrEmpty(param))
                    return;
                var pageType = (ApplicationActivity)Enum.Parse(typeof(ApplicationActivity), param);
                await PageNavigation(pageType, tabtxt);
            }
            catch (Exception ex)
            {


            }
        }

        private async Task PageNavigation(ApplicationActivity page, string tabtxt)
        {
            try
            {
               
                switch (tabtxt)
                {
                    case "SENDS":
                        await Navigation.PushAsync(new SendsPage("SENDS"));
                        break;
                    case "TICK LIST":
                        await Navigation.PushAsync(new TickListPage());
                        break;
                    case "CALENDAR":
                        await Navigation.PushAsync(new CalendarPage());
                        break;
                    case "RANKING":
                        await Navigation.PushAsync(new RankingPage());
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}