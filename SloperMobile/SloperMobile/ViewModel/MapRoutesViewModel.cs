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

namespace SloperMobile.ViewModel
{
    public class MapRoutesViewModel : BaseViewModel
    {
        private ImageSource currentImage;
        private ImageSource topangle=null;
        private ImageSource tophold = null;
        private ImageSource toproutestyle = null;
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
        bool hideSwipeUp=true;
        public bool IsHideSwipeUp
        {
            get { return hideSwipeUp; }
            set { hideSwipeUp = value; OnPropertyChanged(); }
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

        public ImageSource TopAngle
        {
            get { return topangle; }
            set { topangle = value; OnPropertyChanged(); }
        }

        public ImageSource TopHold
        {
            get { return tophold; }
            set { tophold = value; OnPropertyChanged(); }
        }

        public ImageSource TopRouteStyle
        {
            get { return toproutestyle; }
            set { toproutestyle = value; OnPropertyChanged(); }
        }

        public MapRoutesViewModel(MapListModel CurrentSector)
        {
            SendCommand = new DelegateCommand(ExecuteOnSends);
            HidePopupCommand = new DelegateCommand(ExecuteOnHidePopup);
            ShowPopupCommand = new DelegateCommand(ExecuteOnShowPopup);
            IsPopupHide = false;
            try
            {
                PageHeaderText = CurrentSector.SectorName;
                CurrentSectorImage = CurrentSector.SectorImage;
                LoadRouteData(CurrentRouteID);
            }
            catch
            {
            }
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

        public void LoadRouteData(object obj)
        {
            CurrentRouteID = Convert.ToString(obj);
            var routeData = App.DAUtil.GetRouteDataByRouteID(CurrentRouteID);
            if (routeData != null)
            {
                SectorName = routeData.route_name;
                RouteName = routeData.route_name;
                CragName = routeData.crag_id;
                RouteInfo = routeData.route_info;
                Rating = routeData.rating;
                TechGrade = routeData.tech_grade;
                TopAngle = ImageSource.FromFile(GetTopAngleResourceName(routeData.angles_top_1));
                TopHold = ImageSource.FromFile(GetTopHoldResourceName(routeData.hold_type_top_1));
                TopRouteStyle = ImageSource.FromFile(GetTopRouteStyleResourceName(routeData.route_style_top_1));
            }
        }

        private void ExecuteOnHidePopup(object obj)
        {
            IsPopupShow = false;
            IsPopupHide = true;
        }

        private void ExecuteOnShowPopup(object obj)
        {
            IsPopupShow = true;
            IsPopupHide = false;
        }


        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand HidePopupCommand { get; set; }
        public DelegateCommand ShowPopupCommand { get; set; }

        private string GetTopAngleResourceName(string angle)
        {
            string resource = "steepSlab.png";
            switch (angle)
            {
                case "1":
                    resource = "steepSlab.png";
                    break;
                case "2":
                    resource = "steepVertical.png";
                    break;
                case "4":
                    resource = "steepOverhanging.png";
                    break;
                case "8":
                    resource = "steepRoof.png";
                    break;
            }
            return resource;
        }

        private string GetTopHoldResourceName(string hold)
        {
            string resource = "hold_type_1_slopers.png";
            switch (hold)
            {
                case "1":
                    resource = "hold_type_1_slopers.png";
                    break;
                case "2":
                    resource = "hold_type_2_crimps.png";
                    break;
                case "4":
                    resource = "hold_type_4_jugs.png";
                    break;
                case "8":
                    resource = "hold_type_4_jugs.png";
                    break;
            }
            return resource;
        }

        private string GetTopRouteStyleResourceName(string route)
        {
            string resource = "route_style_1_technical.png";
            switch (route)
            {
                case "1":
                    resource = "route_style_1_technical.png";
                    break;
                case "2":
                    resource = "route_style_2_sequential.png";
                    break;
                case "4":
                    resource = "route_style_4_powerful.png";
                    break;
                case "8":
                    resource = "route_style_8_sustained.png";
                    break;
                case "16":
                    resource = "route_style_16_one_move.png";
                    break;
                case "all":
                    resource = "route_style_everything.png";
                    break;
            }
            return resource;
        }
    }
}
