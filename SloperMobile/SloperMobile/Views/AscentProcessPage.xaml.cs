using Acr.UserDialogs;
using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Common.Interfaces;
using SloperMobile.DataBase;
using SloperMobile.Model;
using SloperMobile.ViewModel;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Platform.Device;

namespace SloperMobile.Views
{
    public partial class AscentProcessPage : CarouselPage
    {
        SQLiteConnection dbConn;
        private string _routeid, staticAnnotationData;
        public List<Tuple<string, string>> _bucket = new List<Tuple<string, string>>();
        public MapListModel _CurrentSector { get; set; }
        private AscentProcessViewModel AscentProcessVM;
        public AscentProcessPage(string routeid, MapListModel CurrentSector)
        {
            dbConn = DependencyService.Get<ISQLite>().GetConnection();
            var tapImageNext = new TapGestureRecognizer();
            tapImageNext.Tapped += tapImageNext_Tapped;
            var tapImagePrev = new TapGestureRecognizer();
            tapImagePrev.Tapped += tapImagePrev_Tapped;

            _routeid = routeid;
            _CurrentSector = CurrentSector;
            InitializeComponent();

            ascType.ObjImgAcentTypeNxt.GestureRecognizers.Add(tapImageNext);
            ascDate.ObjImgAcentDateNxt.GestureRecognizers.Add(tapImageNext);
            ascDate.ObjImgAcentDatePrv.GestureRecognizers.Add(tapImagePrev);

            ascRating.ObjImgAcentRatingNxt.GestureRecognizers.Add(tapImageNext);
            ascRating.ObjImgAcentRatingPrv.GestureRecognizers.Add(tapImagePrev);
            ascClimbingAngle.ObjImgClmAngleNxt.GestureRecognizers.Add(tapImageNext);
            ascClimbingAngle.ObjImgClmAnglePrv.GestureRecognizers.Add(tapImagePrev);

            ascHoldType.ObjAscentHoldTypeNxt.GestureRecognizers.Add(tapImageNext);
            ascHoldType.ObjAscentHoldTypePrv.GestureRecognizers.Add(tapImagePrev);
            ascRouteStyle.ObjAscentRouteStyleNxt.GestureRecognizers.Add(tapImageNext);
            ascRouteStyle.ObjAscentRouteStylePrv.GestureRecognizers.Add(tapImagePrev);


            AscentProcessVM = new AscentProcessViewModel(Navigation, routeid);
            BindingContext = AscentProcessVM;
            //Title = AscentProcessVM.PageHeaderText;
            NavigationPage.SetHasNavigationBar(this, false);
            if (AppSetting.APP_TYPE == "indoor")
            {
                Children.RemoveAt(3);
            }

        }
        void tapImageNext_Tapped(object sender, EventArgs e)
        {
            var index = Children.IndexOf(CurrentPage);
            if (index < Children.Count)
                SelectedItem = Children[index + 1];

            //DisplayAlert("Alert", "This is an image button", "OK");
        }

        void tapImagePrev_Tapped(object sender, EventArgs e)
        {
            var index = Children.IndexOf(CurrentPage);
            if (index > Children.Count)
            {
                SelectedItem = Children[index + 1];
            }
            if (index < Children.Count)
                SelectedItem = Children[index - 1];

            //DisplayAlert("Alert", "This is an image button", "OK");
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
            var topolistData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
            var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
            var topoimg = JsonConvert.SerializeObject(topoimgages[Cache.SelectedTopoIndex]);
            bool IsRouteClicked = false;
            staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";

            // Added by sandeep            

            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            if (!IsRouteClicked)
            {
                _bucket.Clear();
                if (topoimg != null)
                {
                    for (int i = 0; i < topoimgages[0].drawing.Count; i++)
                    {
                        if (_routeid == topoimgages[0].drawing[i].id)
                        {
                            _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimgages[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimgages[0].drawing[i].gradeBucket), topoimgages[0].drawing[i].gradeBucket));
                        }
                        else
                        {
                            _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimgages[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimgages[0].drawing[i].gradeBucket), topoimgages[0].drawing[i].gradeBucket));
                        }
                    }
                }
                webView.CallJsFunction("initDrawing", staticAnnotationData, "[" + topoimg + "]", webView.HeightRequest, _bucket);
                if (Convert.ToInt32(_routeid) > 0)
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        webView.CallJsFunction("initAscentReDrawing", staticAnnotationData, "[" + topoimg + "]", (webView.HeightRequest), Convert.ToInt32(_routeid), false, true, _bucket);
                    }
                    else
                    {
                        webView.CallJsFunction("initAscentReDrawing", staticAnnotationData, "[" + topoimg + "]", (device.Display.Height), Convert.ToInt32(_routeid), false, true, _bucket);
                    }
                }
                var ratio = webView.HeightRequest / Convert.ToInt32((topoimgages[Cache.SelectedTopoIndex].image.height));
                var newWidth = Convert.ToInt32((topoimgages[Cache.SelectedTopoIndex].image.width)) * ratio;
                webView.WidthRequest = newWidth;
                UserDialogs.Instance.HideLoading();
            }
        }
        protected override void OnCurrentPageChanged()
        {
            int isRouteIdFound = -1;
            List<int> topoElement = new List<int>();
            base.OnCurrentPageChanged();
            var index = Children.IndexOf(CurrentPage);
            SelectedItem = Children[index];
            if (index == 6 || AppSetting.APP_TYPE == "indoor" && index==5)
            {
                
                if (AscentProcessVM.IsDisplaySummaryWeb)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    var staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";
                    var topolistData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
                    var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
                    //get topoimagedata using match routeid
                    for (int i = 0; i < topoimgages.Count; i++)
                    {
                        if (topoElement.Count == 0)
                        {
                            for (int j = 0; j < topoimgages[i].drawing.Count; j++)
                            {
                                if (_routeid == topoimgages[i].drawing[j].id && topoimgages[i].drawing[j].line.points.Count > 0)
                                {
                                    isRouteIdFound = i;
                                    Cache.SelectedTopoIndex = i;
                                    topoElement.Add(i);
                                }
                            }
                        }
                    }
                    var device = XLabs.Ioc.Resolver.Resolve<IDevice>(); _bucket.Clear();
                    if (isRouteIdFound != -1)
                    {
                        var topoimg = JsonConvert.SerializeObject(topoimgages[Cache.SelectedTopoIndex]);                        
                        if (topoimg != null)
                        {
                            for (int i = 0; i < topoimgages[0].drawing.Count; i++)
                            {
                                if (_routeid == topoimgages[0].drawing[i].id)
                                {
                                    _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimgages[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimgages[0].drawing[i].gradeBucket), topoimgages[0].drawing[i].gradeBucket));
                                }
                            }
                        }
                        webView.CallJsFunction("initAscentReDrawing", staticAnnotationData, "[" + topoimg + "]", (device.Display.Height), Convert.ToInt32(_routeid), false, true, _bucket);
                        this.webView.LoadFromContent("HTML/TopoResizeImage.html");
                    }
                    else
                    {
                        //if no topo image then load default sector image
                        var item = dbConn.Table<TCRAG_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == Settings.SelectedCragSettings);
                        if (topoimgages.Count > 0)
                        {
                            if (topoimgages[0].image.name == "No_Image.jpg")
                            {
                                if (item != null)
                                {
                                    string _strimg64 = item.crag_portrait_image.Split(',')[1];
                                    if (!string.IsNullOrEmpty(_strimg64))
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(_strimg64);
                                        _Image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                                        _Image.IsVisible = true;
                                        webView.IsVisible = false;
                                    }
                                }
                                else
                                {
                                    //other wise show default                                
                                    if (AppSetting.APP_TYPE == "indoor")
                                    {
                                        _Image.Source = ImageSource.FromFile("default_sloper_portrait_image_indoor");
                                    }
                                    else { _Image.Source = ImageSource.FromFile("default_sloper_portrait_image_outdoor"); }
                                    _Image.IsVisible = true;
                                    webView.IsVisible = false;
                                }
                            }
                            //load image without route
                            var topoimg = JsonConvert.SerializeObject(topoimgages[0]);
                            webView.CallJsFunction("initAscentReDrawing", staticAnnotationData, "[" + topoimg + "]", (device.Display.Height), Convert.ToInt32(_routeid), false, true, _bucket);
                            this.webView.LoadFromContent("HTML/TopoResizeImage.html");
                        }
                        else if (item != null) //if no topo , no sector image then load Crag Scenic Action Portrait Shot (spceific to gym)                                                  
                        {
                            string strimg64 = item.crag_portrait_image.Split(',')[1];
                            if (!string.IsNullOrEmpty(strimg64))
                            {
                                byte[] imageBytes = Convert.FromBase64String(strimg64);
                                _Image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                                _Image.IsVisible = true;
                                webView.IsVisible = false;
                            }
                        }
                        else
                        {
                            //other wise show default
                            webView.IsVisible = false;
                            if (AppSetting.APP_TYPE == "indoor") { _Image.Source = "default_sloper_portrait_image_indoor"; }
                            else { _Image.Source = "default_sloper_portrait_image_outdoor"; }
                            _Image.IsVisible = true;
                        }
                    }
                }
                comment_text.Text = AscentProcessVM.CommentText;
                summary_icons.Children?.Clear();
                /*summary_icons.RowDefinitions?.Clear();   //=========For Grid UI========
                summary_icons.ColumnDefinitions?.Clear();*/
                int gridrowcount = 0;
                List<string> iconsource = new List<string>();
                var a = AscentProcessVM.SendClimbingStyle;
                var b = AscentProcessVM.SendHoldType;
                var c = AscentProcessVM.SendRouteCharacteristics;
                if (!string.IsNullOrEmpty(a))
                {
                    string[] arr1 = a.Split(',');
                    foreach (string sa in arr1)
                    {
                        iconsource.Add(AscentProcessVM.GetSummarySteepnessResourceName(sa));
                    }
                    gridrowcount += arr1.Count();
                }
                if (!string.IsNullOrEmpty(b))
                {
                    string[] arr2 = b.Split(',');
                    foreach (string sb in arr2)
                    {
                        iconsource.Add(AscentProcessVM.GetSummaryHoldTypeResourceName(sb));
                    }
                    gridrowcount += arr2.Count();
                }
                if (!string.IsNullOrEmpty(c))
                {
                    string[] arr3 = c.Split(',');
                    foreach (string sc in arr3)
                    {
                        iconsource.Add(AscentProcessVM.GetSummaryRouteStyleResourceName(sc));
                    }
                    gridrowcount += arr3.Count();
                }
                #region Grid based UI
                /*   ================================== 
                if (gridrowcount % 4 == 0)
                {
                    gridrowcount = gridrowcount / 4;
                }
                else
                {
                    gridrowcount = (gridrowcount / 4) + 1;
                }

                for (var i = 0; i < gridrowcount; i++)
                {
                    summary_icons.RowDefinitions?.Add(new RowDefinition { Height = 50 });
                }
                for (var i = 0; i < 4; i++)
                {
                    summary_icons.ColumnDefinitions?.Add(new ColumnDefinition { Width = 50 });
                }

                var iconcount = 0;
                for (var gr = 0; gr < gridrowcount; gr++)
                {
                    for (var gc = 0; gc < 4; gc++)
                    {
                        if (iconcount < iconsource.Count)
                        {
                            summary_icons.Children.Add(new Image { Source = ImageSource.FromFile(iconsource[iconcount]) }, gc, gr);
                            iconcount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                */
                #endregion

                foreach (string iconstr in iconsource)
                {
                    summary_icons.Children.Add(new Image { Source = ImageSource.FromFile(iconstr), HeightRequest = 50, WidthRequest = 50 });
                }
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
