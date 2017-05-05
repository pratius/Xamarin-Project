using Plugin.Geolocator;
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
        Map map;
        public CragMapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            
            map = new Map(MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(0.3)))
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
                    map.Pins.Add(new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(Convert.ToDouble(tcrag.crag_latitude), Convert.ToDouble(tcrag.crag_longitude)),
                        Label = tcrag.crag_name,
                        Address = tcrag.crag_general_info
                    });
                }
            }
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var userloc = await GetGurrentLocation();
            if (userloc != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(userloc.Latitude, userloc.Longitude), Distance.FromMiles(0.3)));
            }
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
