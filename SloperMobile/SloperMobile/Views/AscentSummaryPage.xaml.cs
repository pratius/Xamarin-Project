﻿using Acr.UserDialogs;
using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Model;
using SloperMobile.UserControls;
using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Platform.Device;

namespace SloperMobile.Views
{
    public partial class AscentSummaryPage : ContentPage
    {
        private string _routeid, staticAnnotationData;
        private Send _send = null;
        public MapListModel _CurrentSector { get; set; }
        private AscentSummaryModel AscentProcessVM;

        public AscentSummaryPage(string routeid, MapListModel CurrentSector, Send send)
        {
            _send = send;
            _routeid = routeid;
            _CurrentSector = CurrentSector;
            InitializeComponent();

            AscentProcessVM = new AscentSummaryModel(Navigation, routeid, send);
            BindingContext = AscentProcessVM;
            List<int> _topoElement = new List<int>();
            Title = AscentProcessVM.PageHeaderText;
            UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
            var staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";
            var topolistData = App.DAUtil.GetSectorLines(_send.sector_id.ToString());
            var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
            //get topoimagedata using match routeid
            for (int i = 0; i < topoimgages.Count; i++)
            {
                if (_topoElement.Count == 0)
                {
                    for (int j = 0; j < topoimgages[i].drawing.Count; j++)
                    {
                        if (_routeid == topoimgages[i].drawing[j].id)
                        {
                            Cache.SelectedTopoIndex = i;
                            _topoElement.Add(i);
                        }
                    }
                }
            }
            var topoimg = JsonConvert.SerializeObject(topoimgages[Cache.SelectedTopoIndex]);
            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            webView.CallJsFunction("initAscentReDrawing", staticAnnotationData, "[" + topoimg + "]", (device.Display.Height), Convert.ToInt32(_routeid), false, true);
            this.webView.LoadFromContent("HTML/TopoResizeImage.html");
            comment_text.Text = _send.Comment;
            summary_icons.Children?.Clear();

            int gridrowcount = 0;
            List<string> iconsource = new List<string>();
            var a = getCommaSepratedValue(_send.Climbing_Angle.ToString());
            var b = getCommaSepratedValue(_send.Hold_Type.ToString());
            var c = getCommaSepratedValue(_send.Route_Style.ToString());
            if (!string.IsNullOrEmpty(a))
            {
                string[] arr1 = a.Split(',');
                foreach (string sa in arr1)
                {
                    iconsource.Add(GetSummarySteepnessResourceName(sa));
                }
                gridrowcount += arr1.Count();
            }
            if (!string.IsNullOrEmpty(b))
            {
                string[] arr2 = b.Split(',');
                foreach (string sb in arr2)
                {
                    iconsource.Add(GetSummaryHoldTypeResourceName(sb));
                }
                gridrowcount += arr2.Count();
            }
            if (!string.IsNullOrEmpty(c))
            {
                string[] arr3 = c.Split(',');
                foreach (string sc in arr3)
                {
                    iconsource.Add(GetSummaryRouteStyleResourceName(sc));
                }
                gridrowcount += arr3.Count();
            }

            foreach (string iconstr in iconsource)
            {
                summary_icons.Children.Add(new Image { Source = ImageSource.FromFile(iconstr), HeightRequest = 50, WidthRequest = 50 });
            }
            if (Device.OS == TargetPlatform.iOS)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public string getCommaSepratedValue(string value)
        {
            var steepnessString = string.Empty;
            var steepnessInt = Convert.ToInt32(value);

            for (var i = 0; i <= steepnessInt; i++)
            {
                if (steepnessInt >= 32)
                {
                    steepnessString += steepnessString.Length > 0 ? ",32" : "32";
                    steepnessInt -= 32;
                }
                else if (steepnessInt >= 16)
                {
                    steepnessString += steepnessString.Length > 0 ? ",16" : "16"; ;
                    steepnessInt -= 16;
                }
                else if (steepnessInt >= 8)
                {
                    steepnessString += steepnessString.Length > 0 ? ",8" : "8";
                    steepnessInt -= 8;
                }
                else if (steepnessInt >= 4)
                {
                    steepnessString += steepnessString.Length > 0 ? ",4" : "4";
                    steepnessInt -= 4;
                }
                else if (steepnessInt >= 2)
                {
                    steepnessString += steepnessString.Length > 0 ? ",2" : "2";
                    steepnessInt -= 2;
                }
                else if (steepnessInt >= 1)
                {
                    steepnessString += steepnessString.Length > 0 ? ",1" : "1";
                    steepnessInt -= 1;
                }
            }
            return steepnessString;
        }
        public string GetSummarySteepnessResourceName(string steep)
        {
            string resource = "icon_steepness_1_slab_border_80x80";
            switch (steep)
            {
                case "1":
                    resource = "icon_steepness_1_slab_border_80x80";
                    break;
                case "2":
                    resource = "icon_steepness_2_vertical_border_80x80";
                    break;
                case "4":
                    resource = "icon_steepness_4_overhanging_border_80x80";
                    break;
                case "8":
                    resource = "icon_steepness_8_roof_border_80x80";
                    break;
            }
            return resource;
        }

        public string GetSummaryHoldTypeResourceName(string hold)
        {
            string resource = "icon_hold_type_1_slopers_border_80x80";
            switch (hold)
            {
                case "1":
                    resource = "icon_hold_type_1_slopers_border_80x80";
                    break;
                case "2":
                    resource = "icon_hold_type_2_crimps_border_80x80";
                    break;
                case "4":
                    resource = "icon_hold_type_4_jugs_border_80x80";
                    break;
                case "8":
                    resource = "icon_hold_type_8_pockets_border_80x80";
                    break;
            }
            return resource;
        }

        public string GetSummaryRouteStyleResourceName(string route)
        {
            string resource = "icon_route_style_1_technical_border_80x80";
            switch (route)
            {
                case "1":
                    resource = "icon_route_style_1_technical_border_80x80";
                    break;
                case "2":
                    resource = "icon_route_style_2_sequential_border_80x80";
                    break;
                case "4":
                    resource = "icon_route_style_4_powerful_border_80x80";
                    break;
                case "8":
                    resource = "icon_route_style_8_sustained_border_80x80";
                    break;
                case "16":
                    resource = "icon_route_style_16_one_move_border_80x80";
                    break;
                case "all":
                    resource = "icon_route_style_32_everything_border_80x80";
                    break;
            }
            return resource;
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
            List<int> _topoElement = new List<int>();
            var topolistData = App.DAUtil.GetSectorLines(_send.sector_id.ToString());
            var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
            //get topoimagedata using match routeid
            for (int i = 0; i < topoimgages.Count; i++)
            {
                if (_topoElement.Count == 0)
                {
                    for (int j = 0; j < topoimgages[i].drawing.Count; j++)
                    {
                        if (_routeid == topoimgages[i].drawing[j].id)
                        {
                            Cache.SelectedTopoIndex = i;
                            _topoElement.Add(i);
                        }
                    }
                }
            }
            var topoimg = JsonConvert.SerializeObject(topoimgages[Cache.SelectedTopoIndex]);
            bool IsRouteClicked = false;
            staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";
           
            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            if (!IsRouteClicked)
            {
                webView.CallJsFunction("initDrawing", staticAnnotationData, "[" + topoimg + "]", webView.HeightRequest);
                if (Convert.ToInt32(_routeid) > 0)
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        webView.CallJsFunction("initAscentReDrawing", staticAnnotationData, "[" + topoimg + "]", (webView.HeightRequest), Convert.ToInt32(_routeid), false, true);
                    }
                    else
                    {
                        webView.CallJsFunction("initAscentReDrawing", staticAnnotationData, "[" + topoimg + "]", (device.Display.Height), Convert.ToInt32(_routeid), false, true);
                    }
                }
                
                 var ratio = webView.HeightRequest / Convert.ToInt32((topoimgages[Cache.SelectedTopoIndex].image.height));
                 var newWidth = Convert.ToInt32((topoimgages[Cache.SelectedTopoIndex].image.width)) * ratio;
                 webView.WidthRequest = newWidth;
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
