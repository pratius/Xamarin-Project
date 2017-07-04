﻿using Plugin.Connectivity;
using SloperMobile.Common.Enumerators;
using SloperMobile.Common.Helpers;
using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class HomePage : ContentPage
    {
        private HomeViewModel HomeVM;

        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            HomeVM = new HomeViewModel();
            BindingContext = HomeVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HomeVM.ClimbDaysCount = Settings.ClimbingDaysSettings.ToString();
        }

        private async void OnClickSend(object sender, EventArgs e)
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
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task PageNavigation(ApplicationActivity page)
        {
            try
            {
                switch (page)
                {
                    case ApplicationActivity.CragMapPage:
                        //await Navigation.PushAsync(new MapPage());
                        //await Navigation.PopToRootAsync();
                        await Navigation.PushAsync(new CragMapPage());
                        break;
                    case ApplicationActivity.ProfilePage:
                        await Navigation.PushAsync(new RankingPage());
                        // await Navigation.PushAsync(new ProfilePage());
                        // await Navigation.PushAsync(new SendsPage("POINTS"));
                        //await Navigation.PushAsync(new PointsPage(String.Empty));
                        break;
                    case ApplicationActivity.PyramidPage:
                        await Navigation.PushAsync(new PyramidPage());
                        break;
                    case ApplicationActivity.SendsPage:
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            await Navigation.PushAsync(new SendsPage("SENDS"));
                            break;
                        }
                        else
                        {
                            await Navigation.PushAsync(new NetworkErrorPage());
                            break;
                        }
                    case ApplicationActivity.CalendarPage:
                        await Navigation.PushAsync(new CalendarPage());
                        //throw new Exception("Exception Raised by Ravi Explicitly !!!!!!");
                        break;
                    case ApplicationActivity.MapPage:
                        //await Navigation.PushAsync(new MapPage());
                        await Navigation.PushAsync(new MapPage());
                        break;
                    case ApplicationActivity.NewsPage:
                        await Navigation.PushAsync(new NewsPage());
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
                throw;
            }
        }

    }
}
