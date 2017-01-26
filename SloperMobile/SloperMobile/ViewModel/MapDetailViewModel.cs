using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Common.Command;
using SloperMobile.Model;
using Xamarin.Forms;
using SloperMobile.Common.Constants;
using SloperMobile.DataBase;

namespace SloperMobile.ViewModel
{

    public class MapDetailViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private ImageSource sectorImage;
        private List<RouteData> routedata;
        private MapListModel currentsec;
        private T_CRAG currentCrag;
        public List<RouteData> RoutesData
        {
            get { return routedata; }
            set { routedata = value; OnPropertyChanged(); }
        }
        public ImageSource SectorImage
        {
            get { return sectorImage; }
            set { sectorImage = value; OnPropertyChanged(); }
        }

        public MapListModel CurrentSector
        {
            get { return currentsec; }
            set { currentsec = value; OnPropertyChanged(); }
        }

        #region DelegateCommand

        public DelegateCommand TapSectorCommand { get; set; }
        #endregion
        public MapDetailViewModel(MapListModel SelectedSector, INavigation navigaton)
        {
            try
            {
                currentCrag = App.DAUtil.GetSelectedCragData();
                _navigation = navigaton;
                CurrentSector = SelectedSector;
                Cache.SelctedCurrentSector = SelectedSector;
                PageHeaderText = SelectedSector.SectorName;
                PageSubHeaderText = currentCrag.crag_name;
                SectorImage = SelectedSector.SectorImage;
                TapSectorCommand = new DelegateCommand(TapOnSectorImage);
                var routes = App.DAUtil.GetRoutesBySectorId(SelectedSector.SectorId);
                RoutesData = new List<RouteData>();
                int i = 1;
                foreach (T_ROUTE route in routes)
                {
                    RouteData routeobj = new RouteData();
                    if (i < 10)
                    {
                        routeobj.RouteIndex = "0" + i.ToString();
                    }
                    else
                    {
                        routeobj.RouteIndex = i.ToString();
                    }
                    routeobj.TitleText = route.route_name;
                    routeobj.SubText = route.route_info;
                    RoutesData.Add(routeobj);
                    i++;
                }
            }
            catch
            {
            }
        }
        private async void TapOnSectorImage(object obj)
        {
            try
            {
                await _navigation.PushAsync(new Views.MapRoutesPage(CurrentSector));
            }
            catch
            {
            }
        }
    }

    public class RouteData
    {
        public string RouteIndex { get; set; }
        public string TitleText { get; set; }
        public string SubText { get; set; }
    }
}
