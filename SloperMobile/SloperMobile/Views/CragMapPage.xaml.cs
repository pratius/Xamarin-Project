using Plugin.Geolocator;
using SloperMobile.Common.Helpers;
using SloperMobile.CustomControls;
using SloperMobile.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SloperMobile.Common.Constants;
using System.Reflection;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CragMapPage : ContentPage
    {
        public CragMapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {

            var crags = App.DAUtil.GetCragList();
            foreach (T_CRAG tcrag in crags)
            {
                if (!string.IsNullOrEmpty(tcrag.crag_latitude) && !string.IsNullOrEmpty(tcrag.crag_longitude))
                {
                    //BitmapDescriptor pinIcon;
                    //if (!string.IsNullOrEmpty(Settings.SelectedCragSettings)&& Settings.SelectedCragSettings== tcrag.crag_id)
                    //{
                    //    var assembly = typeof(CragMapPage).GetTypeInfo().Assembly;
                    //    var file = "icon_pin_crag_current";
                    //    var stream = assembly.GetManifestResourceStream($"SloperMobile.{file}");
                    //    pinIcon = BitmapDescriptorFactory.FromStream(stream);
                    //}
                    //else
                    //{
                    //    var assembly = typeof(CragMapPage).GetTypeInfo().Assembly;
                    //    var file = "icon_pin_crag";
                    //    var stream = assembly.GetManifestResourceStream($"SloperMobile.{file}");
                    //    pinIcon = BitmapDescriptorFactory.FromStream(stream);
                    //}
                    map.Pins.Add(new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(Convert.ToDouble(tcrag.crag_latitude), Convert.ToDouble(tcrag.crag_longitude)),
                        Label = tcrag.crag_name,
                        Address =(AppSetting.APP_TYPE=="indoor")? tcrag.crag_parking_info : tcrag.area_name,
                        Tag = tcrag.crag_id
                    });
                }
            }
            //var userloc = await GetGurrentLocation();
            //if (userloc != null)
            //{
            //    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(userloc.Latitude, userloc.Longitude), Distance.FromMiles(0.3)), true);
            //}
            map.InfoWindowClicked += Map_InfoWindowClicked;
            base.OnAppearing();
        }

        private async void Map_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            var cragid = e.Pin.Tag;
            if(!string.IsNullOrEmpty(cragid.ToString()))
            {
                Settings.SelectedCragSettings = cragid.ToString();
            }
            await Navigation.PushAsync(new CragDetailsPage());
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
