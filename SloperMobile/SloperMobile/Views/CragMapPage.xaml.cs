using Plugin.Geolocator;
using SloperMobile.Common.Helpers;
using SloperMobile.CustomControls;
using SloperMobile.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CragMapPage : ContentPage
    {
        Map theMap;
        public CragMapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            theMap = new Map()
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MapType = MapType.Street
            };

            var crags = App.DAUtil.GetCragList();
            foreach (T_CRAG tcrag in crags)
            {
                if (!string.IsNullOrEmpty(tcrag.crag_latitude) && !string.IsNullOrEmpty(tcrag.crag_longitude))
                {
                    theMap.Pins.Add(new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(Convert.ToDouble(tcrag.crag_latitude), Convert.ToDouble(tcrag.crag_longitude)),
                        Label = tcrag.crag_name,
                        Address = tcrag.crag_general_info
                    });
                }
            }
            var userloc = await GetGurrentLocation();
            if (userloc != null)
            {
                theMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(userloc.Latitude, userloc.Longitude), Distance.FromMiles(0.3)));
            }
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(theMap);
            Content = stack;
            base.OnAppearing();
        }

        private async Task<Plugin.Geolocator.Abstractions.Position> GetGurrentLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                if (position != null)
                    return position;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
