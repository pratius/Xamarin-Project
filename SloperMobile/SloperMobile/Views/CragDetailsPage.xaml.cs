using Acr.UserDialogs;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.DataBase;
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
        private List<T_ROUTE> routeObj;
        private List<T_SECTOR> sectorObj;

        public CragDetailsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            cragdetailsVM = new CragDetailsViewModel();
            BindingContext = cragdetailsVM;
            if (Device.RuntimePlatform == Device.iOS)
            {
                sf_legendGraph.Margin = new Thickness(-12, 0, -12, -12);
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                sf_legendGraph.Margin = new Thickness(-5, 0, -5, -5);
            }
        }
        #region Properties
        public List<T_ROUTE> RouteObj
        {
            get { return routeObj; }
            set { routeObj = value; OnPropertyChanged(); }
        }
        public List<T_SECTOR> SectorObj
        {
            get { return sectorObj; }
            set { sectorObj = value; OnPropertyChanged(); }
        }
        #endregion
        protected override void OnAppearing()
        {
            base.OnAppearing();

            cragdetailsVM.IsDisplayOverlayFrame = false;
            cragdetailsVM.IsDisplayRemoveCragFrame = false;
            cragdetailsVM.IsDisplayGraphFrame = true;
            cragdetailsVM.IsDisplayFadeImage = true;

            cragdetailsVM.IsShowConfirmation = false;
            cragdetailsVM.IsShowDownloading = false;
            cragdetailsVM.IsShowRemoveConfirmation = false;
            cragdetailsVM.IsShowRemoving = false;

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
            if (!App.DAUtil.CheckCragDataExistsByCragID(Settings.SelectedCragSettings))
            {
                newroutes.Add("DOWNLOAD");
            }
            else
            {
                cragdetailsVM.IsDisplaySectorButton = true;
                newroutes.Add("REMOVE");
            }
            foreach (string iconstr in newroutes)
            {
                if (!string.IsNullOrEmpty(GetIconNameByRouteTypeName(iconstr)))
                {
                    StackLayout innerstack = new StackLayout();
                    if (iconstr == "DOWNLOAD")
                    {
                        innerstack.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() =>
                            {
                                cragdetailsVM.IsDisplayFadeImage = false;
                                cragdetailsVM.IsDisplayGraphFrame = false;
                                cragdetailsVM.IsDisplayRemoveCragFrame = false;
                                cragdetailsVM.IsDisplayOverlayFrame = true;

                                cragdetailsVM.IsShowConfirmation = true;
                                cragdetailsVM.IsShowDownloading = false;
                            }),
                        });
                    }

                    if (iconstr == "REMOVE")
                    {
                        innerstack.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() =>
                            {
                                cragdetailsVM.IsDisplayFadeImage = false;
                                cragdetailsVM.IsDisplayGraphFrame = false;
                                cragdetailsVM.IsDisplayOverlayFrame = false;
                                cragdetailsVM.IsDisplayRemoveCragFrame = true;
                                cragdetailsVM.IsShowRemoveConfirmation = true;
                                cragdetailsVM.IsShowRemoving = false;
                            }),
                        });
                    }

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
            if (routetypename == "DOWNLOAD")
            {
                iconname = "icon_crag_download.png";
            }
            if (routetypename == "REMOVE")
            {
                iconname = "icon_crag_download.png";
            }
            
            return iconname;
        }

        private async void Sector_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.MapPage());
        }
        private void OnCancelDownload(object sender,EventArgs e)
        {
            cragdetailsVM.IsDisplayGraphFrame = true;
            cragdetailsVM.IsDisplayFadeImage = true;
            cragdetailsVM.IsDisplayOverlayFrame = false;
            cragdetailsVM.IsDisplayRemoveCragFrame = false;
        }

        private void OnCancelRemove(object sender, EventArgs e)
        {
            cragdetailsVM.IsDisplayGraphFrame = true;
            cragdetailsVM.IsDisplayFadeImage = true;
            cragdetailsVM.IsDisplayOverlayFrame = false;
            cragdetailsVM.IsDisplayRemoveCragFrame = false;
        }

        private async void OnDownload(object sender, EventArgs e)
        {
            cragdetailsVM.IsShowConfirmation = false;
            cragdetailsVM.IsShowDownloading = true;
            SectorObj = await HttpGetSectorUpdates();
            foreach (T_SECTOR sector in SectorObj)
            {
                App.DAUtil.SaveSector(sector);
                Dictionary<string, string> topodict = new Dictionary<string, string>();
                topodict.Add("sectorID", sector.sector_id);
                HttpClientHelper apicall = new ApiHandler(ApiUrls.Url_GetUpdate_TopoData, string.Empty);
                var topo_response = await apicall.GetJsonString<string>(topodict);
                T_TOPO topo = new T_TOPO();
                topo.sector_id = sector.sector_id;
                topo.topo_json = topo_response;
                topo.upload_date = Helper.GetCurrentDate("yyyyMMdd");
                App.DAUtil.SaveTopo(topo);
            }
            RouteObj = await HttpGetRouteUpdates();
            foreach (T_ROUTE route in RouteObj)
            {
                App.DAUtil.SaveRoute(route);
            }
            progressbar.IsRunning = false;
            OnAppearing();
        }

        private void OnRemove(object sender, EventArgs e)
        {
            cragdetailsVM.IsShowRemoveConfirmation = false;
            cragdetailsVM.IsShowRemoving = true;
            cragdetailsVM.IsDisplaySectorButton = false;
            App.DAUtil.RemoveCragData(Settings.SelectedCragSettings);
            progressbar_remove.IsRunning = false;
            OnAppearing();
        }

        private async void OnBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync().ConfigureAwait(false);
            await Navigation.PushAsync(new CragMapPage());
        }

        #region Services
        private async Task<List<T_ROUTE>> HttpGetRouteUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppSetting.APP_ID, Settings.SelectedCragSettings, "20160101", "route", true), Cache.AccessToken);
            var route_response = await apicall.Get<T_ROUTE>();
            return route_response;
        }
        private async Task<List<T_SECTOR>> HttpGetSectorUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppSetting.APP_ID, Settings.SelectedCragSettings, "20160101", "sector", true), Cache.AccessToken);
            var sector_response = await apicall.Get<T_SECTOR>();
            return sector_response;
        }
        #endregion
    }
}
