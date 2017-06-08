using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
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
    public partial class CragDetailsPage : ContentPage
    {
        CragDetailsViewModel cragdetailsVM;
        public CragDetailsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            cragdetailsVM = new CragDetailsViewModel();
            BindingContext = cragdetailsVM;
            if(Device.RuntimePlatform==Device.iOS)
            {
                sf_legendGraph.Margin = new Thickness(-12, 0, -12, -12);
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                sf_legendGraph.Margin = new Thickness(0, 0, 0, 0);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            crag_icons.Children.Clear();
            var routes = App.DAUtil.GetRouteTypesByCragID(Settings.SelectedCragSettings);
            List<string> newroutes = new List<string>();
            if (AppSetting.APP_TYPE != "indoor")
            {
                newroutes.Add("season");
            }
            foreach (string iconstr in routes)
            {
                newroutes.Add(iconstr);
            }
            foreach (string iconstr in newroutes)
            {
                if (!string.IsNullOrEmpty(GetIconNameByRouteTypeName(iconstr)))
                {
                    StackLayout innerstack = new StackLayout();
                    innerstack.Orientation = StackOrientation.Vertical;
                    innerstack.HorizontalOptions = LayoutOptions.Center;
                    innerstack.VerticalOptions = LayoutOptions.EndAndExpand;
                    innerstack.Children.Add(new Image { Source = ImageSource.FromFile(GetIconNameByRouteTypeName(iconstr)), HeightRequest = 50, WidthRequest = 50 });
                    innerstack.Children.Add(new Label { Text = (iconstr == "season" ? App.DAUtil.GetSelectedCragData().season : iconstr), TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, FontSize = 10 });
                    crag_icons.Children.Add(innerstack);
                }
            }
        }

        private string GetIconNameByRouteTypeName(string routetypename)
        {
            string iconname = "";
            if (routetypename == "season")
            {
                iconname = "icon_season.png";
            }
            if (routetypename == "Bouldering")
            {
                iconname = "icon_route_type_bouldering.png";
            }
            if (routetypename == "Sport")
            {
                iconname = "icon_route_type_sport.png";
            }
            if (routetypename == "Traditional")
            {
                iconname = "icon_route_type_traditional.png";
            }
            return iconname;
        }

        private async void Sector_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.MapPage());
        }
        private async void OnBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync().ConfigureAwait(false);
            await Navigation.PushAsync(new CragMapPage());
        }
    }
}
