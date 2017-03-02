using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;
using SloperMobile.Common.Command;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using SloperMobile.Common.Constants;
using XLabs.Platform.Device;
using SloperMobile.Views;
using SloperMobile.Common.Helpers;

namespace SloperMobile.ViewModel
{
    public class TopoMapRoutesViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private ImageSource currentImage;
        private ImageSource topangle=null;
        private ImageSource tophold = null;
        private ImageSource toproutestyle = null;        
        public string ImageData = string.Empty;
        public string staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";
        public ImageSource CurrentSectorImage
        {
            get { return currentImage; }
            set { currentImage = value; OnPropertyChanged(); }
        }

        private bool isPopupHide;

        public bool IsPopupHide
        {
            get { return isPopupHide; }
            set { isPopupHide = value; OnPropertyChanged(); }
        }

        private bool isPopupShow;

        public bool IsPopupShow
        {
            get { return isPopupShow; }
            set { isPopupShow = value; OnPropertyChanged(); }
        }

        private string _sectorName;

        public string SectorName
        {
            get { return _sectorName; }
            set { _sectorName = value; OnPropertyChanged(); }
        }

        private string cragName;

        public string CragName
        {
            get { return cragName; }
            set { cragName = value; OnPropertyChanged(); }
        }

        private string stateName;

        public string StateName
        {
            get { return stateName; }
            set { stateName = value; OnPropertyChanged(); }
        }

        private string routeName="";

        public string RouteName
        {
            get { return routeName; }
            set { routeName = value; OnPropertyChanged(); }
        }

        private string techGrade;

        public string TechGrade
        {
            get { return techGrade; }
            set { techGrade = value; OnPropertyChanged(); }
        }

        private string routeInfo="";

        public string RouteInfo
        {
            get { return routeInfo; }
            set { routeInfo = value; OnPropertyChanged(); }
        }

        private string rating="";

        public string Rating
        {
            get { return rating; }
            set { rating = value; OnPropertyChanged(); }
        }

        private string routeTypeTop;

        public string RouteTypeTop
        {
            get { return routeTypeTop; }
            set { routeTypeTop = value; OnPropertyChanged(); }
        }

        private string curr_routeid="";

        public string CurrentRouteID
        {
            get { return curr_routeid; }
            set { curr_routeid = value; OnPropertyChanged(); }
        }
        private ObservableCollection<MapRouteImageModel> routeImageList;

        public ObservableCollection<MapRouteImageModel> RouteImageList
        {
            get { return routeImageList; }
            set { routeImageList = value; OnPropertyChanged(); }
        }

        public ImageSource TopAngle_20
        {
            get { return topangle; }
            set { topangle = value; OnPropertyChanged(); }
        }

        public ImageSource TopAngle_54
        {
            get { return topangle; }
            set { topangle = value; OnPropertyChanged(); }
        }

        public ImageSource TopHold_20
        {
            get { return tophold; }
            set { tophold = value; OnPropertyChanged(); }
        }

        public ImageSource TopHold_54
        {
            get { return tophold; }
            set { tophold = value; OnPropertyChanged(); }
        }

        public ImageSource TopRouteStyle_20
        {
            get { return toproutestyle; }
            set { toproutestyle = value; OnPropertyChanged(); }
        }

        public ImageSource TopRouteStyle_54
        {
            get { return toproutestyle; }
            set { toproutestyle = value; OnPropertyChanged(); }
        }

        public MapListModel _CurrentSector { get; set; }

        public TopoMapRoutesViewModel(MapListModel CurrentSector, INavigation navigation)
        {
            _navigation = navigation;
            TickListCommand = new DelegateCommand(ExecuteOnTickList);
            SendCommand = new DelegateCommand(ExecuteOnSends);
            HidePopupCommand = new DelegateCommand(ExecuteOnHidePopup);
            ShowPopupCommand = new DelegateCommand(ExecuteOnShowPopup);
            IsPopupHide = false;
            try
            {
                PageHeaderText = CurrentSector.SectorName;
                CurrentSectorImage = CurrentSector.SectorImage;
                LoadRouteData(CurrentRouteID,string.Empty);
            }
            catch
            {
            }
        }


        private async void ExecuteOnTickList(object obj)
        {
            //first check if route already present in db or not
            HttpClientHelper _apicall = new ApiHandler(string.Format(ApiUrls.Url_Isticklist_Route_Present, Convert.ToInt64(CurrentRouteID)), Settings.AccessTokenSettings);
            var isRoutePresent_Response = await _apicall.Get<IsRoutePresent>();
            if (isRoutePresent_Response.Count == 0 || isRoutePresent_Response[0].isRoutePresent == false)
            {
                HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_Tick_List, Convert.ToInt64(CurrentRouteID)), Settings.AccessTokenSettings);
                var tickList_response = await apicall.Get<string>();
            }
            await _navigation.PushAsync(new SendsPage("TICKLIST"));
        }        
        private void ExecuteOnSends(object obj)
        {
            //OnPageNavigation?.Invoke();
            OnConditionNavigation?.Invoke(CurrentRouteID);
        }

        private void LoadSectorImages()
        {
            try
            {
                var sector_images = App.DAUtil.GetSectorImages(1, 10);
                foreach (var sector in sector_images)
                {
                    var topoimg = JsonConvert.DeserializeObject<TopoImageResponse>(sector.topo_json);
                    string strimg64 = topoimg.image.data.Split(',')[1];
                    if (!string.IsNullOrEmpty(strimg64))
                    {
                        MapListModel objSec = new MapListModel();
                        objSec.SectorId = sector.sector_id;
                        byte[] imageBytes = Convert.FromBase64String(strimg64);
                        objSec.SectorImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                    }
                }
            }
            catch (Exception ex)
            {
                string strerr = ex.Message;
            }
        }

        public void LoadRouteData(object obj, string listData)
        {
            ImageData = listData;
            CurrentRouteID = Convert.ToString(obj);
            var routeData = App.DAUtil.GetRouteDataByRouteID(CurrentRouteID);
            if (routeData != null)
            {
                SectorName = routeData.route_name;
                TechGrade = routeData.tech_grade;
                RouteName = (routeData.route_name).ToUpper() + " " + TechGrade;
                CragName = routeData.crag_id;
                RouteInfo = routeData.route_info;
                Rating = routeData.rating;
                TopAngle_20 = ImageSource.FromFile(GetTopAngleResourceName(routeData.angles_top_1) + "_20x20");
                TopAngle_54 = ImageSource.FromFile(GetTopAngleResourceName(routeData.angles_top_1) + "_54x54");
                TopHold_20 = ImageSource.FromFile(GetTopHoldResourceName(routeData.hold_type_top_1) + "_20x20");
                TopHold_54 = ImageSource.FromFile(GetTopHoldResourceName(routeData.hold_type_top_1) + "_54x54");
                TopRouteStyle_20 = ImageSource.FromFile(GetTopRouteStyleResourceName(routeData.route_style_top_1) + "_20x20");
                TopRouteStyle_54 = ImageSource.FromFile(GetTopRouteStyleResourceName(routeData.route_style_top_1) + "_54x54");
            }
        }

        private void ExecuteOnHidePopup(object obj)
        {           
            IsPopupShow = false;
            IsPopupHide = true;
        }

        private void ExecuteOnShowPopup(object obj)
        {            
            var routId = Convert.ToInt32(CurrentRouteID);
            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();            
            //TopoMapRoutesPage _topoMapPage = new TopoMapRoutesPage(_CurrentSector, string.Empty,0);            
           // webView.CallJsFunction("initReDrawing", staticAnnotationData, ImageData, (device.Display.Height), Convert.ToInt32(CurrentRouteID), false);
            IsPopupShow = true;
            IsPopupHide = false;
        }

        public DelegateCommand TickListCommand { get; set; }
        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand HidePopupCommand { get; set; }
        public DelegateCommand ShowPopupCommand { get; set; }

        private string GetTopAngleResourceName(string angle)
        {
            string resource = "icon_steepness_1_slab_border";
            switch (angle)
            {
                case "1":
                    resource = "icon_steepness_1_slab_border";
                    break;
                case "2":
                    resource = "icon_steepness_2_vertical_border";
                    break;
                case "4":
                    resource = "icon_steepness_4_overhanging_border";
                    break;
                case "8":
                    resource = "icon_steepness_8_roof_border";
                    break;
            }
            return resource;
        }

        private string GetTopHoldResourceName(string hold)
        {
            string resource = "icon_hold_type_1_slopers_border";
            switch (hold)
            {
                case "1":
                    resource = "icon_hold_type_1_slopers_border";
                    break;
                case "2":
                    resource = "icon_hold_type_2_crimps_border";
                    break;
                case "4":
                    resource = "icon_hold_type_4_jugs_border";
                    break;
                case "8":
                    resource = "icon_hold_type_8_pockets_border";
                    break;
            }
            return resource;
        }

        private string GetTopRouteStyleResourceName(string route)
        {
            string resource = "icon_route_style_1_technical_border";
            switch (route)
            {
                case "1":
                    resource = "icon_route_style_1_technical_border";
                    break;
                case "2":
                    resource = "icon_route_style_2_sequential_border";
                    break;
                case "4":
                    resource = "icon_route_style_4_powerful_border";
                    break;
                case "8":
                    resource = "icon_route_style_8_sustained_border";
                    break;
                case "16":
                    resource = "icon_route_style_16_one_move_border";
                    break;
                case "all":
                    resource = "icon_route_style_32_everything_border";
                    break;
            }
            return resource;
        }
    }
}
