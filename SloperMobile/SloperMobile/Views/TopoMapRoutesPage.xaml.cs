using SloperMobile.Model;
using System;
using Xamarin.Forms;
using XLabs.Platform.Device;
using System.IO;
using System.Net.Http;
using SloperMobile.Common.Constants;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite.Net;
using SloperMobile.Common.Interfaces;
using SloperMobile.DataBase;
using System.Linq;
using SloperMobile.Common.Helpers;
//using UIKit;
//using Foundation;

namespace SloperMobile.Views
{
    public partial class TopoMapRoutesPage : ContentPage
    {
        SQLiteConnection dbConn;
        public MapListModel _CurrentSector { get; set; }
        public List<TopoImageResponse> topoimg = null;
        public List<Tuple<string, string>> _bucket = new List<Tuple<string, string>>();
        private ViewModel.TopoMapRoutesViewModel TopoMapRouteVM;
        public string staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";
        string listData = string.Empty;
        Double height, newHeight;
        int _routeId = 0;
        public TopoMapRoutesPage(MapListModel CurrentSector, string _lstData, int routeId)
        {
            try
            {
                InitializeComponent();
                Cache.SendBackArrowCount = 3;
                dbConn = DependencyService.Get<ISQLite>().GetConnection();
                _CurrentSector = CurrentSector;
                listData = _lstData;
                _routeId = routeId;
                topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(listData);
                NavigationPage.SetHasNavigationBar(this, false);
                Title = CurrentSector.SectorName;
                TopoMapRouteVM = new ViewModel.TopoMapRoutesViewModel(CurrentSector, Navigation);

                BindingContext = TopoMapRouteVM;
                Cache.GlobalBase64String = null;
                // load the scenic shot if there are no topos available
                if (listData == string.Empty || listData == "[]")
                {
                    TopoMapRouteVM.OnConditionNavigation = OnPageNavigation;
                    TopoMapRouteVM.LoadRouteData(routeId, listData);
                    if (routeId > 0 && (listData == "" || listData == "[]"))
                    {
                        TopoMapRouteVM.DisplayRoutePopupLg = true;
                        TopoMapRouteVM.IsHideSwipeUp = false;
                    }
                    webView.IsVisible = false;
                    var item = dbConn.Table<TCRAG_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == Settings.SelectedCragSettings);
                    if (CurrentSector != null && listData == "[]")
                    {
                        if (Cache.IsCragOrDefaultImageCount != 1)
                        {
                            _Image.Source = CurrentSector.SectorImage;
                            Cache.GlobalBase64String = CurrentSector.SectorImage;
                            _Image.IsVisible = true;
                        }
                    }
                    else if (item != null)
                    {
                        string strimg64 = item.crag_portrait_image.Split(',')[1];
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            _Image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                            _Image.IsVisible = true;                            
                        }
                    }
                    else
                    {
                        if (AppSetting.APP_TYPE == "indoor")
                        {
                            //this.BackgroundImage = "default_sloper_indoor_portrait";
                            _Image.Source = ImageSource.FromFile("default_sloper_indoor_portrait");
                        }
                        else
                        {
                            //this.BackgroundImage = "default_sloper_outdoor_portrait";
                            _Image.Source = ImageSource.FromFile("default_sloper_outdoor_portrait");
                        }
                        _Image.IsVisible = true;
                    }
                }
                // otherwise load the topos
                else
                {
                    TopoMapRouteVM.OnConditionNavigation = OnPageNavigation;
                    TopoMapRouteVM.IsRunningTasks = true;
                    this.webView.RegisterCallback("dataCallback", t =>
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        TopoMapRouteVM.LoadRouteData(t, listData);
                        TopoMapRouteVM.DisplayRoutePopupSm = true;
                        var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
                        height = device.Display.Height;
                        newHeight = GetHeight(height); _bucket.Clear();
                        if (topoimg != null && topoimg.Count > 0)
                        {
                            for (int i = 0; i < topoimg[0].drawing.Count; i++)
                            {
                                if (t == topoimg[0].drawing[i].id)
                                {
                                    _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket), topoimg[0].drawing[i].gradeBucket));
                                }
                            }
                        }
                        if (topoimg != null && topoimg.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(topoimg[0].image.data))
                            {
                                if (topoimg[0].image.name == "No_Image.jpg")
                                {
                                    LoadCragAndDefaultImage();
                                    webView.IsVisible = false;
                                    // _Image.IsVisible = false;
                                }
                                else
                                {
                                    webView.CallJsFunction("initReDrawing", staticAnnotationData, listData, newHeight, Convert.ToInt32(t), true, false, _bucket);
                                }
                            }
                            else
                            {
                                if (_routeId > 0)
                                {
                                    TopoMapRouteVM.DisplayRoutePopupLg = true;
                                    TopoMapRouteVM.IsHideSwipeUp = false;
                                }
                                LoadCragAndDefaultImage();
                                webView.IsVisible = false;
                            }
                        }
                        else
                        {
                            LoadCragAndDefaultImage();
                            webView.IsVisible = false;
                        }
                    }));
                }
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

        public double GetHeight(Double _height)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                switch (Convert.ToInt32(_height))
                {
                    case 1334:
                        _height = _height + 200; //iphone 6 and iphone 7 and iphone 6s
                        break;
                    case 2208:
                        _height = _height - 580; //iPhone6Plus and iPhone7Plus and iphone 6s plus
                        break;
                    case 1136:
                        _height = _height + 350; //iPhone5 and iPhone5s and iPhone SE
                        break;
                    case 2048:
                        _height = _height - 840; //iPas Air 2 and iPad Air 
                        break;
                }
            }
            else
            {
                var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
                _height = (_height / device.Display.Scale) - 110;
            }
            return _height;
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
            TopoMapRouteVM.IsRunningTasks = true;
            staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";

            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            height = device.Display.Height;
            newHeight = GetHeight(height);
            if (topoimg != null && topoimg.Count > 0)
            {
                for (int i = 0; i < topoimg[0].drawing.Count; i++)
                {
                    _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket), topoimg[0].drawing[i].gradeBucket));
                }
            }
            if (topoimg != null && topoimg.Count > 0)
            {
                if (!string.IsNullOrEmpty(topoimg[0].image.data))
                {
                    if (topoimg[0].image.name == "No_Image.jpg")
                    {
                        LoadCragAndDefaultImage();
                        webView.IsVisible = false;
                    }
                    else
                    {
                        webView.CallJsFunction("initDrawing", staticAnnotationData, listData, newHeight, _bucket);
                    }
                }
                else
                {
                    if (_routeId > 0)
                    {
                        TopoMapRouteVM.DisplayRoutePopupLg = true;
                        TopoMapRouteVM.IsHideSwipeUp = false;
                        TopoMapRouteVM.DisplayRoutePopupSm = false;
                    }
                    LoadCragAndDefaultImage();
                    webView.IsVisible = false;
                }
            }
            else
            {
                if (listData != "[]")
                {
                    LoadCragAndDefaultImage();
                    webView.IsVisible = false;
                }
            }

            // if a route was clicked from the list
            if (_routeId > 0)
            {
                List<int> _topoElement = new List<int>();
                var topolistData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
                var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
                //get topoimagedata using match routeid
                for (int i = 0; i < topoimgages.Count; i++)
                {
                    if (_topoElement.Count == 0)
                    {
                        for (int j = 0; j < topoimgages[i].drawing.Count; j++)
                        {
                            if (_routeId.ToString() == topoimgages[i].drawing[j].id)
                            {
                                Cache.SelectedTopoIndex = i;
                                _topoElement.Add(i);
                            }
                        }
                    }
                }
                TopoMapRouteVM.LoadRouteData(_routeId, listData);
                //if the route clicked has a topo, display the small popup
                if (listData != string.Empty && listData != "[]")
                {
                    if (topoimgages.Count > 0)
                    {
                        if (topoimgages[0].image.data != "")
                            TopoMapRouteVM.DisplayRoutePopupSm = true;

                    }
                }
                _bucket.Clear();
                if (topoimg != null && topoimg.Count > 0)
                {
                    for (int i = 0; i < topoimg[0].drawing.Count; i++)
                    {
                        if (_routeId.ToString() == topoimg[0].drawing[i].id)
                        {
                            _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket), topoimg[0].drawing[i].gradeBucket));
                        }
                    }
                }
                if (topoimgages.Count > 0 && topoimgages != null)
                {
                    if (topoimg != null && topoimg.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(topoimg[0].image.data))
                        {
                            if (topoimgages[0].image.name == "No_Image.jpg")
                            {
                                LoadCragAndDefaultImage();
                                webView.IsVisible = false;
                            }
                            else
                            {
                                webView.CallJsFunction("initReDrawing", staticAnnotationData, listData, (newHeight), _routeId, true, false, _bucket);
                            }
                        }
                        else
                        {
                            LoadCragAndDefaultImage();
                            webView.IsVisible = false;
                        }
                    }
                    else
                    {
                        if (listData != "[]")
                        {
                            LoadCragAndDefaultImage();
                            webView.IsVisible = false;
                        }
                    }
                }
                else
                {
                    LoadCragAndDefaultImage();
                    webView.IsVisible = false;
                }
            }
            TopoMapRouteVM.IsRunningTasks = false;
        }

        private void LoadCragAndDefaultImage()
        {
            var item = dbConn.Table<TCRAG_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == Settings.SelectedCragSettings);
            if (item != null)
            {
                string _strimg64 = item.crag_portrait_image.Split(',')[1];
                if (!string.IsNullOrEmpty(_strimg64))
                {
                    byte[] imageBytes = Convert.FromBase64String(_strimg64);
                    _Image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    _Image.IsVisible = true;
                }
            }
            else
            {
                if (AppSetting.APP_TYPE == "indoor")
                {
                    //this.BackgroundImage = "default_sloper_indoor_portrait";
                    _Image.Source = ImageSource.FromFile("default_sloper_indoor_portrait");
                }
                else
                {
                    // this.BackgroundImage = "default_sloper_outdoor_portrait";
                    _Image.Source = ImageSource.FromFile("default_sloper_outdoor_portrait");
                }
                _Image.IsVisible = true;
            }
        }
        private void OnSwipeTopRoutePopupLg(object sender, EventArgs e)
        {
            if (listData != string.Empty)
            {
                TopoMapRouteVM.HideRoutePopupLgCommand.Execute(null);
            }
        }

        private void OnSwipeDownRoutePopupSm(object sender, EventArgs e)
        {
            TopoMapRouteVM.ShowRoutePopupLgCommand.Execute(null);
        }

        private void OnSwipeTopRoutePopupSm(object sender, EventArgs e)
        {
            TopoMapRouteVM.DisplayRoutePopupSm = false;
            _bucket.Clear();
            topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(listData);
            if (topoimg != null)
            {
                for (int i = 0; i < topoimg[0].drawing.Count; i++)
                {
                    _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket), topoimg[0].drawing[i].gradeBucket));
                }
            }
            // if we got to the topo via the map and not the list, redraw all the routes
            if (_routeId <= 0)
            {
                webView.CallJsFunction("initDrawing", staticAnnotationData, listData, newHeight, _bucket);
            }
        }
    }
}
