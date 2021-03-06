using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;
using SloperMobile.Common.Constants;
using XLabs.Platform.Device;

namespace SloperMobile.Views
{
    public partial class MapRoutesPage : ContentPage
    {
        public MapListModel _CurrentSector { get; set; }
        private ViewModel.MapRoutesViewModel MapRouteVM;
        public MapRoutesPage(MapListModel CurrentSector)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, true);
                Title = CurrentSector.SectorName;
                MapRouteVM = new ViewModel.MapRoutesViewModel(CurrentSector);
                BindingContext = MapRouteVM;
                MapRouteVM.OnConditionNavigation = OnPageNavigation;
                _CurrentSector = CurrentSector;
                this.webView.RegisterCallback("dataCallback", t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    MapRouteVM.LoadRouteData(t);
                    MapRouteVM.DisplayRoutePopupSm = true;
                })
            );

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async void OnPageNavigation(object obj)
        {
            await Navigation.PushAsync(new AscentProcessPage(Convert.ToString(obj), _CurrentSector));
        }

        protected override void OnAppearing()
        {
            this.webView.LoadFinished += OnLoadFinished;
            this.webView.LoadFromContent("HTML/ResizeImage.html");
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            webView.LoadFinished -= OnLoadFinished;
        }

        private void OnLoadFinished(object sender, EventArgs args)
        {
            bool IsRouteClicked = false;

            string staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";

            // Added by sandeep
            // To load slider we need to pass all topo image data, I am using static which we can convert in dynamic 
            // if image topo data will be single slider will not show and direct route page load with line drawing

            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            var listData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
            if (!IsRouteClicked)
            {
                //webView.Eva(string.Format("InitDrawing({0})", staticData));
                webView.CallJsFunction("initDrawing", staticAnnotationData, listData, (device.Display.Height - 250));
                //int count = 0;
            }
            else
            {
                webView.CallJsFunction("initRouteDrawing", staticAnnotationData, listData, "11840", (device.Display.Height - 250));
            }

        }

        private void BindAndDisPlayPopup(string routeid)
        {
            Application.Current.MainPage.DisplayAlert("Route ID", routeid, "OK");
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("test", "Click Down", "Ok");
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await DisplayAlert("test", "Click Up", "Ok");
        }

        private void SwipeableImage_SwipedDown(object sender, EventArgs e)
        {
            MapRouteVM.ShowRoutePopupLgCommand.Execute(null);
        }

        private void SwipeableImage_SwipedUp(object sender, EventArgs e)
        {
            MapRouteVM.HideRoutePopupLgCommand.Execute(null);

        }
        //private void OnMenuTapped(object sender, EventArgs e)
        //{
        //    Cache.MasterPage.IsPresented = true;
        //}

        //private void OnSearch(object sender, EventArgs e)
        //{

        //}
    }
}
