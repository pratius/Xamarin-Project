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

        private string routeName;

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

        private string routeInfo;

        public string RouteInfo
        {
            get { return routeInfo; }
            set { routeInfo = value; OnPropertyChanged(); }
        }

        private string rating;

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

        private ObservableCollection<MapRouteImageModel> routeImageList;

        public ObservableCollection<MapRouteImageModel> RouteImageList
        {
            get { return routeImageList; }
            set { routeImageList = value; OnPropertyChanged(); }
        }


        public MapRoutesViewModel(MapListModel CurrentSector)
        {
            SendCommand = new DelegateCommand(ExecuteOnSends);
            HidePopupCommand = new DelegateCommand(ExecuteOnHidePopup);
            ShowPopupCommand = new DelegateCommand(ExecuteOnShowPopup);
            IsPopupHide = true;
            try
            {
                PageHeaderText = CurrentSector.SectorName;
                CurrentSectorImage = CurrentSector.SectorImage;
                LoadRouteDate();
            }
            catch
            {
            }
        }



        private void ExecuteOnSends(object obj)
        {
            OnPageNavigation?.Invoke();
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

        private void LoadRouteDate()
        {
            var routeData = App.DAUtil.GetRouteDataByRouteID("22044");
            if (routeData != null)
            {
                SectorName = routeData.route_name;
                RouteName = routeData.route_name;
                CragName = routeData.crag_id;
                RouteInfo = routeData.route_info;
                Rating = routeData.rating;
                TechGrade = routeData.tech_grade;
                LoadTheImageBaseOnRouteType(routeData.route_type);
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

        private void LoadTheImageBaseOnRouteType(string routeType)
        {
            RouteImageList = new ObservableCollection<MapRouteImageModel>();
            if (string.IsNullOrWhiteSpace(routeType))
                return;
            if (routeType == AppConstant.RouteType_climbing_Angle)
            {
                RouteImageList.Add(new MapRouteImageModel { Id = 1, ImagePath = AppConstant.RouteType_Angle_Slab_1 });
                RouteImageList.Add(new MapRouteImageModel { Id = 2, ImagePath = AppConstant.RouteType_Angle_Vertical_2 });
                RouteImageList.Add(new MapRouteImageModel { Id = 3, ImagePath = AppConstant.RouteType_Angle_Overhanging_4 });
                RouteImageList.Add(new MapRouteImageModel { Id = 4, ImagePath = AppConstant.RouteType_Angle_Roof_8 });
            }
            else if (routeType == AppConstant.RouteType_hold_Type)
            {
                RouteImageList.Add(new MapRouteImageModel { Id = 1, ImagePath = AppConstant.RouteType_Hold_Slopers_1 });
                RouteImageList.Add(new MapRouteImageModel { Id = 2, ImagePath = AppConstant.RouteType_Hold_Crimp_2 });
                RouteImageList.Add(new MapRouteImageModel { Id = 3, ImagePath = AppConstant.RouteType_Hold_Jungs_4 });
                RouteImageList.Add(new MapRouteImageModel { Id = 4, ImagePath = AppConstant.RouteType_Hold_Pockets_8 });
            }
            else if (routeType == AppConstant.RouteType_Route_Style)
            {
                RouteImageList.Add(new MapRouteImageModel { Id = 1, ImagePath = AppConstant.RouteType_Route_Style_Technical_1 });
                RouteImageList.Add(new MapRouteImageModel { Id = 2, ImagePath = AppConstant.RouteType_Route_Style_Sequential_2 });
                RouteImageList.Add(new MapRouteImageModel { Id = 3, ImagePath = AppConstant.RouteType_Route_Style_Powerful_4 });
                RouteImageList.Add(new MapRouteImageModel { Id = 4, ImagePath = AppConstant.RouteType_Route_Style_Sustained_8 });
                RouteImageList.Add(new MapRouteImageModel { Id = 5, ImagePath = AppConstant.RouteType_Route_Style_One_Move_16 });
            }
        }

        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand HidePopupCommand { get; set; }
        public DelegateCommand ShowPopupCommand { get; set; }
    }
}
