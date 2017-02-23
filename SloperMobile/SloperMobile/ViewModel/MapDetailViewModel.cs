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
using SloperMobile.Common.Enumerators;

namespace SloperMobile.ViewModel
{

    public class MapDetailViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private ImageSource sectorImage;
        private List<RouteData> routedata;
        private MapListModel currentsec;
        private T_CRAG currentCrag;
        private string[] gradehex = { "#036177", "#1f8a70", "#91a537", "#b49800", "#fd7400" };
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
                int j = 0;
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
                    routeobj.RouteId = route.route_id;

                    if (!string.IsNullOrEmpty(route.angles_top_2) && route.angles_top_2.Contains(","))
                    {
                        string[] steeps = route.angles_top_2.Split(',');
                        routeobj.Steepness1 = ImageSource.FromFile(GetSteepnessResourceName(Convert.ToInt32(steeps[0])));
                        routeobj.Steepness2 = ImageSource.FromFile(GetSteepnessResourceName(Convert.ToInt32(steeps[1])));
                    }
                    else
                    {
                        routeobj.Steepness1 = ImageSource.FromFile(GetSteepnessResourceName(1));
                        routeobj.Steepness2 = ImageSource.FromFile(GetSteepnessResourceName(2));
                    }
                    routeobj.RouteTechGrade = route.tech_grade;
                    if (j > 4)
                    { j = 0; }
                    routeobj.RouteGradeColor = gradehex[j];
                    RoutesData.Add(routeobj);
                    i++;
                    j++;
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
                await _navigation.PushAsync(new Views.TopoSectorPage(CurrentSector, "0"));
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }


        private string GetSteepnessResourceName(int steep)
        {
            string resource = "";
            AppSteepness steepvalue;
            if (steep == (int)AppSteepness.Slab)
            {
                steepvalue = AppSteepness.Slab;
            }
            else if (steep == (int)AppSteepness.Vertical)
            {
                steepvalue = AppSteepness.Vertical;
            }
            else if (steep == (int)AppSteepness.Overhanging)
            {
                steepvalue = AppSteepness.Overhanging;
            }
            else
            {
                steepvalue = AppSteepness.Roof;
            }
            switch (steepvalue)
            {
                case AppSteepness.Slab:
                    resource = "steepSlab.png";
                    break;
                case AppSteepness.Vertical:
                    resource = "steepVertical.png";
                    break;
                case AppSteepness.Overhanging:
                    resource = "steepOverhanging.png";
                    break;
                case AppSteepness.Roof:
                    resource = "steepRoof.png";
                    break;
                default:
                    resource = "steepSlab.png";
                    break;
            }
            return resource;
        }
    }

    public class RouteData
    {
        public string RouteIndex { get; set; }
        public string TitleText { get; set; }
        public string SubText { get; set; }
        public ImageSource Steepness1 { get; set; }
        public ImageSource Steepness2 { get; set; }
        public string RouteTechGrade { get; set; }
        public string RouteGradeColor { get; set; }
        public string RouteId { get; set; }
        
    }
}
