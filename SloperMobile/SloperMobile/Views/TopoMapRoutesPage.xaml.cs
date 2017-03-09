using SloperMobile.Model;
using System;
using Xamarin.Forms;
using XLabs.Platform.Device;
using System.IO;
using System.Net.Http;
using SloperMobile.Common.Constants;

namespace SloperMobile.Views
{
    public partial class TopoMapRoutesPage : ContentPage
    {
        public MapListModel _CurrentSector { get; set; }
        private ViewModel.TopoMapRoutesViewModel TopoMapRouteVM;
        public string staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";
        string listData = string.Empty;
        int _routeId = 0;
        public TopoMapRoutesPage(MapListModel CurrentSector, string _lstData, int routeId)
        {
            try
            {
                InitializeComponent();
                _CurrentSector = CurrentSector;
                listData = _lstData;
                _routeId = routeId;
                NavigationPage.SetHasNavigationBar(this, false);
                Title = CurrentSector.SectorName;
                TopoMapRouteVM = new ViewModel.TopoMapRoutesViewModel(CurrentSector, Navigation);

                BindingContext = TopoMapRouteVM;
                if (listData == string.Empty)
                {
                    TopoMapRouteVM.LoadRouteData(routeId, listData);
                    TopoMapRouteVM.IsPopupShow = true;
                    // load the scenic shot if there are no topos available
                    webView.IsVisible = false;
                    TopoMapRouteVM.IsHideSwipeUp = false;
                    this.BackgroundImage = "scenic_shot_portrait";
                }
                TopoMapRouteVM.OnConditionNavigation = OnPageNavigation;
                TopoMapRouteVM.IsRunningTasks = true;
                this.webView.RegisterCallback("dataCallback", t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    TopoMapRouteVM.LoadRouteData(t, listData);
                    TopoMapRouteVM.IsPopupHide = true;
                    var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
                    webView.CallJsFunction("initReDrawing", staticAnnotationData, listData, Cache.CurrentScreenHeight, Convert.ToInt32(t), true);
                }));
            }
            catch (Exception ex)
            {
                TopoMapRouteVM.IsRunningTasks = false;
                throw ex;
            }
            TopoMapRouteVM.IsRunningTasks = false;
        }
        private async void OnPageNavigation(object obj)
        {
            await Navigation.PushAsync(new AscentProcessPage(Convert.ToString(obj), _CurrentSector));
        }

        protected override void OnAppearing()
        {
            try
            {
                this.webView.LoadFinished += OnLoadFinished;
                this.webView.LoadFromContent("HTML/TopoResizeImage.html");
                base.OnAppearing();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            webView.LoadFinished -= OnLoadFinished;
        }
        private void OnLoadFinished(object sender, EventArgs args)
        {
            bool IsRouteClicked = false;
            TopoMapRouteVM.IsRunningTasks = true;
            staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";

            // Added by sandeep
            // To load slider we need to pass all topo image data, I am using static which we can convert in dynamic 
            // if image topo data will be single slider will not show and direct route page load with line drawing

            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            if (!IsRouteClicked)
            {
                //webView.CallJsFunction("initDrawing", staticAnnotationData, listData, (device.Display.Height));
                webView.CallJsFunction("initDrawing", staticAnnotationData, listData, Cache.CurrentScreenHeight);
                if (_routeId > 0)
                {
                    TopoMapRouteVM.LoadRouteData(_routeId, listData);
                    if (listData != string.Empty)
                        TopoMapRouteVM.IsPopupHide = true;

                    webView.CallJsFunction("initReDrawing", staticAnnotationData, listData, (Cache.CurrentScreenHeight), _routeId, true);
                }
            }
            else
            {
                webView.CallJsFunction("initRouteDrawing", staticAnnotationData, listData, _routeId, (Cache.CurrentScreenHeight));
            }
            TopoMapRouteVM.IsRunningTasks = false;
        }


        private void OnSwipeUpPopup(object sender, EventArgs e)
        {
            TopoMapRouteVM.HidePopupCommand.Execute(null);
        }

        private void OnSwipeDown(object sender, EventArgs e)
        {
            TopoMapRouteVM.ShowPopupCommand.Execute(null);
        }

        private void OnSwipeUpHidePopup(object sender, EventArgs e)
        {
            TopoMapRouteVM.IsPopupHide = false;
            if (_routeId <= 0)
                webView.CallJsFunction("initDrawing", staticAnnotationData, listData, Cache.CurrentScreenHeight);
        }
    }
}
