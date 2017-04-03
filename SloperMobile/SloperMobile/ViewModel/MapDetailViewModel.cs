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
using SloperMobile.Common.Helpers;

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

        //private List<BucketLegends> legendsdata;
        //public List<BucketLegends> LegendsData
        //{
        //    get { return legendsdata; }
        //    set { legendsdata = value; OnPropertyChanged(); }
        //}
        private DataTemplate legendsdata;
        public DataTemplate LegendsDataTemplate
        {
            get { return legendsdata; }
            set { legendsdata = value; OnPropertyChanged(); }
        }

        private int legendheight = 0;
        public int LegendsHeight
        {
            get { return legendheight; }
            set { legendheight = value; OnPropertyChanged(); }
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
                LoadLegendsBucket();

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
                    routeobj.Rating = Math.Round(Convert.ToDecimal(route.rating)).ToString();
                    routeobj.StarImage = ImageSource.FromFile(GetStarImage(Math.Round(Convert.ToDecimal(route.rating)).ToString()));
                    //if (!string.IsNullOrEmpty(route.angles_top_2) && route.angles_top_2.Contains(","))
                    //{
                    //    string[] steeps = route.angles_top_2.Split(',');
                    //    routeobj.Steepness1 = ImageSource.FromFile(GetSteepnessResourceName(Convert.ToInt32(steeps[0])));
                    //    routeobj.Steepness2 = ImageSource.FromFile(GetSteepnessResourceName(Convert.ToInt32(steeps[1])));
                    //}
                    //else
                    //{
                    //    routeobj.Steepness1 = ImageSource.FromFile(GetSteepnessResourceName(32));
                    //    routeobj.Steepness2 = ImageSource.FromFile(GetSteepnessResourceName(16));
                    //    routeobj.Steepness3 = ImageSource.FromFile(GetSteepnessResourceName(2));
                    //}
                    if (!string.IsNullOrEmpty(route.angles_top_1) && Convert.ToInt32(route.angles_top_1) > 0)
                    {
                        routeobj.Steepness1 = ImageSource.FromFile(GetTopAngleResourceName(route.angles_top_1) + "_20x20");
                    }
                    else
                    {
                        routeobj.Steepness1 = ImageSource.FromFile(GetTopAngleResourceName("2") + "_20x20");
                    }
                    if (!string.IsNullOrEmpty(route.hold_type_top_1) && Convert.ToInt32(route.hold_type_top_1) > 0)
                    {
                        routeobj.Steepness2 = ImageSource.FromFile(GetTopHoldResourceName(route.hold_type_top_1) + "_20x20");
                    }
                    else
                    {
                        routeobj.Steepness2 = ImageSource.FromFile(GetTopHoldResourceName("4") + "_20x20");
                    }
                    if (!string.IsNullOrEmpty(route.route_style_top_1) && Convert.ToInt32(route.route_style_top_1) > 0)
                    {
                        routeobj.Steepness3 = ImageSource.FromFile(GetTopRouteStyleResourceName(route.route_style_top_1) + "_20x20");
                    }
                    else
                    {
                        routeobj.Steepness3 = ImageSource.FromFile(GetTopRouteStyleResourceName("2") + "_20x20");
                    }
                    routeobj.RouteTechGrade = route.tech_grade;
                    if (j > 4)
                    { j = 0; }
                    routeobj.RouteGradeColor = GetGradeBucketHex(route.grade_bucket_id);
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
                //var nav = new NavigationPage(new Views.TopoSectorPage(CurrentSector, "0"));
                //nav.BarBackgroundColor = Color.Transparent;
                //nav.HeightRequest = 30;
                //await _navigation.PushAsync(nav);
                await _navigation.PushAsync(new Views.TopoSectorPage(CurrentSector, "0"));
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }

        private string GetGradeBucketHex(string grade_bucket_id)
        {
            switch (grade_bucket_id)
            {
                case "1":
                    return "#036177";
                case "2":
                    return "#1f8a70";
                case "3":
                    return "#91a537";
                case "4":
                    return "#b49800";
                case "5":
                    return "#fd7400";
                default:
                    return "#B9BABD";
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
                    resource = "icon_steepness_1_slab_border_20x20";
                    break;
                case AppSteepness.Vertical:
                    resource = "icon_steepness_2_vertical_border_20x20";
                    break;
                case AppSteepness.Overhanging:
                    resource = "icon_steepness_4_overhanging_border_20x20";
                    break;
                case AppSteepness.Roof:
                    resource = "icon_steepness_8_roof_border_20x20";
                    break;               
                default:
                    resource = "icon_steepness_1_slab_border_20x20";
                    break;
            }
            return resource;
        }

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

        private string GetStarImage(string star)
        {
            string resource = "";
            switch (star)
            {
                case "0":
                    resource = "star0";
                    break;
                case "1":
                    resource = "star1";
                    break;
                case "2":
                    resource = "star2";
                    break;
                case "3":
                    resource = "star3";
                    break;
                case "4":
                    resource = "star4";
                    break;
                case "5":
                    resource = "star5";
                    break;               
            }            
                  
            return resource;
        }

        //private List<BucketLegends> LoadLegendsBucket()
        //{
        //    try
        //    {
        //        List<BucketLegends> bucketlist = new List<BucketLegends>();
        //        List<GradeId> gradetyp_id = new List<GradeId>();
        //        List<string> bucketname = new List<string>();
        //        gradetyp_id = App.DAUtil.GetGradeTypeIdBySectorId(CurrentSector.SectorId);
        //        if (gradetyp_id == null) return bucketlist;
        //        foreach (GradeId grdtypid in gradetyp_id)
        //        {
        //            bucketname = App.DAUtil.GetBucketNameByGradeTypeId(grdtypid.grade_type_id);
        //            if (bucketname != null && bucketname.Count == 5)
        //            {
        //                BucketLegends bktObj = new BucketLegends();
        //                bktObj.BucketName1 = bucketname[0];
        //                bktObj.BucketName2 = bucketname[1];
        //                bktObj.BucketName3 = bucketname[2];
        //                bktObj.BucketName4 = bucketname[3];
        //                bktObj.BucketName5 = bucketname[4];
        //                bucketlist.Add(bktObj);
        //                LegendsHeight += 30;
        //            }
        //        }
        //        return bucketlist.Distinct(new BucketLegends.Comparer()).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        private void LoadLegendsBucket()
        {
            try
            {
                List<GradeId> gradetyp_id = new List<GradeId>();
                List<string> bucketname = new List<string>();
                gradetyp_id = App.DAUtil.GetGradeTypeIdByCragId(Settings.SelectedCragSettings);
                if (gradetyp_id != null)
                {
                    int gr = gradetyp_id.Count;
                    int gc = App.DAUtil.GetTotalBucketForApp();
                    Grid grdLegend = new Grid();
                    for (var i = 0; i < gr; i++)
                    {
                        grdLegend.RowDefinitions?.Add(new RowDefinition { Height = GridLength.Auto });
                    }

                    for (var i = 0; i < gc; i++)
                    {
                        grdLegend.ColumnDefinitions?.Add(new ColumnDefinition { Width = GridLength.Star });
                    }
                    for (var r = 0; r < gr; r++)
                    {
                        bucketname = App.DAUtil.GetBucketNameByGradeTypeId(gradetyp_id[r].grade_type_id);
                        if (bucketname != null && bucketname.Count == gc)
                        {
                            for (int c = 0; c < gc; c++)
                            {
                                grdLegend.Children.Add(new Label { Text = bucketname[c], HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.FromHex(GetGradeBucketHex((c + 1).ToString())) }, c, r);
                            }
                        }
                    }
                    LegendsDataTemplate = new DataTemplate(() =>
                    {
                        return grdLegend;
                    });

                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class RouteData
    {
        public string RouteIndex { get; set; }
        public string TitleText { get; set; }
        public string SubText { get; set; }
        public string Rating { get; set; }
        public ImageSource Steepness1 { get; set; }
        public ImageSource Steepness2 { get; set; }
        public ImageSource Steepness3 { get; set; }
        public ImageSource StarImage { get; set; }
        
        public string RouteTechGrade { get; set; }
        public string RouteGradeColor { get; set; }
        public string RouteId { get; set; }
        
    }
}
