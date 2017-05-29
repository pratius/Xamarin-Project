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
using System.Threading;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CragMapPage : ContentPage
    {
        string user_loc_img_name = "";
        Assembly appassembly;
        System.IO.Stream user_icon_stream;
        public CragMapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            appassembly = typeof(App).GetTypeInfo().Assembly;
            user_loc_img_name = "icon_pin_device_location.png";
            user_icon_stream = appassembly.GetManifestResourceStream($"SloperMobile.CustomControls.MapRoot.Pins.{user_loc_img_name}");
        }

        protected override async void OnAppearing()
        {
            var crags = App.DAUtil.GetCragList();
            foreach (T_CRAG tcrag in crags)
            {
                if (!string.IsNullOrEmpty(tcrag.crag_latitude) && !string.IsNullOrEmpty(tcrag.crag_longitude))
                {
                    BitmapDescriptor pinIcon;
                    string filename = "";
                    if (!string.IsNullOrEmpty(Settings.SelectedCragSettings) && Settings.SelectedCragSettings == tcrag.crag_id)
                    {
                        var assembly = typeof(App).GetTypeInfo().Assembly; //typeof(CragMapPage).GetTypeInfo().Assembly;
                        filename = "icon_pin_crag_current.png";
                        var stream = assembly.GetManifestResourceStream($"SloperMobile.CustomControls.MapRoot.Pins.{filename}");
                        pinIcon = BitmapDescriptorFactory.FromStream(stream);
                    }
                    else
                    {
                        var assembly = typeof(App).GetTypeInfo().Assembly;
                        filename = "icon_pin_crag.png";
                        var stream = assembly.GetManifestResourceStream($"SloperMobile.CustomControls.MapRoot.Pins.{filename}");
                        pinIcon = BitmapDescriptorFactory.FromStream(stream);
                    }
                    map.Pins.Add(new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(Convert.ToDouble(tcrag.crag_latitude), Convert.ToDouble(tcrag.crag_longitude)),
                        Label = tcrag.crag_name,
                        Address = (AppSetting.APP_TYPE == "indoor") ? tcrag.crag_parking_info : tcrag.area_name,
                        Tag = tcrag.crag_id,
                        //Icon = (Settings.SelectedCragSettings == tcrag.crag_id)?BitmapDescriptorFactory.DefaultMarker(Color.SkyBlue): BitmapDescriptorFactory.DefaultMarker(Color.Gray)
                        Icon = pinIcon
                    });
                }
            }
            var moveto = map.Pins.Where(p => p.Tag.ToString() == Settings.SelectedCragSettings).FirstOrDefault().Position;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(moveto.Latitude, moveto.Longitude), Distance.FromMiles(0.3)), true);
            map.InfoWindowClicked += Map_InfoWindowClicked;

            var userloc = await GetGurrentLocation();
            if (userloc != null)
            {
                //var assembly = typeof(App).GetTypeInfo().Assembly;
                //user_loc_img_name = "icon_pin_device_location.png";
                //var stream = appassembly.GetManifestResourceStream($"SloperMobile.CustomControls.MapRoot.Pins.{user_loc_img_name}");
                map.Pins.Add(new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(userloc.Latitude, userloc.Longitude),
                    Label = "",
                    Address = "",
                    Tag = "",
                    Icon = BitmapDescriptorFactory.FromStream(user_icon_stream)
                });
                //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(userloc.Latitude, userloc.Longitude), Distance.FromMiles(0.3)), true);
            }
            base.OnAppearing();
        }

        private async void Map_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            var cragid = e.Pin.Tag;
            if (!string.IsNullOrEmpty(cragid.ToString()))
            {
                Settings.SelectedCragSettings = cragid.ToString();
                if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != new CragDetailsPage().GetType())
                {
                    await Navigation.PushAsync(new CragDetailsPage());
                }
            }
        }

        private async Task<Plugin.Geolocator.Abstractions.Position> GetGurrentLocation()
        {
            try
            {
                CancellationTokenSource ctsrc = new CancellationTokenSource(2000);
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                //var position = await locator.GetPositionAsync(timeoutMilliseconds: 5000);
                var position = await locator.GetPositionAsync(2000, ctsrc.Token);
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
