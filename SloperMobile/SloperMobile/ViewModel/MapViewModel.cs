using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SloperMobile.Common.Command;
using SloperMobile.Model;
using Xamarin.Forms;
using SloperMobile.DataBase;
using SloperMobile.Common.Enumerators;
using SloperMobile.Common.Helpers;
using SloperMobile.Common.Constants;
using SQLite.Net;
using SloperMobile.Common.Interfaces;
using Acr.UserDialogs;

namespace SloperMobile.ViewModel
{
    public class MapViewModel : BaseViewModel
    {
        SQLiteConnection dbConn;
        private readonly INavigation _navigation;
        private MapListModel _selectedSector;
        private T_CRAG currentCrag;
        public MapViewModel(INavigation navigation)
        {
            UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
            dbConn = DependencyService.Get<ISQLite>().GetConnection();
            currentCrag = App.DAUtil.GetSelectedCragData();
            _navigation = navigation;
            PageHeaderText = currentCrag.crag_name;
            PageSubHeaderText = currentCrag.area_name;
            LoadLegendsBucket();
            LoadMoreSector = new DelegateCommand(LoadSectorImages);
        }

        #region Properties
        private ObservableCollection<MapListModel> _sectorimageList;

        public ObservableCollection<MapListModel> SectorImageList
        {
            //get { return _sectorimageList; }
            get { return _sectorimageList ?? (_sectorimageList = new ObservableCollection<MapListModel>()); }
            set { _sectorimageList = value; OnPropertyChanged(); }
        }

        public MapListModel SelectedSector
        {
            get { return _selectedSector; }
            set
            {
                _selectedSector = value;
                GoToSeletedSector();
                OnPropertyChanged();
            }
        }

        private int legendheight = 0;
        public int LegendsHeight
        {
            get { return legendheight; }
            set { legendheight = value; OnPropertyChanged(); }
        }

        private DataTemplate legendsdata;
        public DataTemplate LegendsDataTemplate
        {
            get { return legendsdata; }
            set { legendsdata = value; OnPropertyChanged(); }
        }

        #endregion

        #region DelegateCommand

        public DelegateCommand LoadMoreSector { get; set; }
        #endregion

        private void LoadSectorImages(object obj)
        {
            try
            {
                Cache.IsCragOrDefaultImageCount = 0;
                var sector_images = App.DAUtil.GetSectorImages(SectorImageList.Count(), 10);
                foreach (var sector in sector_images)
                {
                    string strimg64 = string.Empty;
                    MapListModel objSec = new MapListModel();
                    if (!(sector.topo_json == "[]"))
                    {
                        var topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(sector.topo_json);
                        if (topoimg != null && topoimg.Count > 0)
                        {
                            for (int i = 0; i < topoimg.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(topoimg[0].image.data))
                                {
                                    if (topoimg[0].image.name == "No_Image.jpg")
                                    {
                                        objSec.SectorImage = LoadCragAndDefaultImage();                                        
                                    }
                                    else
                                    {
                                        strimg64 = topoimg[0].image.data.Split(',')[1];
                                        if (!string.IsNullOrEmpty(strimg64))
                                        {
                                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                                            objSec.SectorImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));                                            
                                        }
                                    }
                                }
                                else
                                {
                                    if (topoimg.Count == 2)
                                    {
                                        if (!string.IsNullOrEmpty(topoimg[1].image.data))
                                        {
                                            if (topoimg[1].image.name == "No_Image.jpg")
                                            {
                                                objSec.SectorImage = LoadCragAndDefaultImage();                                                
                                            }
                                            else
                                            {
                                                strimg64 = topoimg[1].image.data.Split(',')[1];
                                                if (!string.IsNullOrEmpty(strimg64))
                                                {
                                                    byte[] imageBytes = Convert.FromBase64String(strimg64);
                                                    objSec.SectorImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));                                                    
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        objSec.SectorImage = LoadCragAndDefaultImage();                                        
                                    }
                                }
                            }                            
                        }
                        else
                        {                            
                            objSec.SectorImage = LoadCragAndDefaultImage();
                        }
                    }
                    else
                    {                        
                        objSec.SectorImage = LoadCragAndDefaultImage();
                    }
                    objSec.SectorId = sector.sector_id;
                    T_SECTOR tsec = App.DAUtil.GetSectorDataBySectorID(sector.sector_id);
                    objSec.SectorName = tsec.sector_name;
                    string latlong = "";
                    if (!string.IsNullOrEmpty(tsec.latitude) && !string.IsNullOrEmpty(tsec.longitude))
                    {
                        latlong = tsec.latitude + " / " + tsec.longitude;
                    }
                    objSec.SectorLatLong = latlong;
                    objSec.SectorShortInfo = tsec.sector_info_short;
                    if (!string.IsNullOrEmpty(tsec.angles_top_2) && tsec.angles_top_2.Contains(","))
                    {
                        string[] steeps = tsec.angles_top_2.Split(',');
                        objSec.Steepness1 = ImageSource.FromFile(GetSteepnessResourceName(Convert.ToInt32(steeps[0])));
                        objSec.Steepness2 = ImageSource.FromFile(GetSteepnessResourceName(Convert.ToInt32(steeps[1])));
                    }
                    else
                    {
                        objSec.Steepness1 = ImageSource.FromFile(GetSteepnessResourceName(2));
                        objSec.Steepness2 = ImageSource.FromFile(GetSteepnessResourceName(4));
                    }
                    int totalbuckets = App.DAUtil.GetTotalBucketForApp();
                    //var tgrades = App.DAUtil.GetBucketCountsBySectorId(tsec.sector_id);
                    if (totalbuckets != 0)
                    {
                        objSec.BucketCountTemplate = new DataTemplate(() =>
                        {
                            StackLayout slBucketFrame = new StackLayout();
                            slBucketFrame.Orientation = StackOrientation.Horizontal;
                            slBucketFrame.HorizontalOptions = LayoutOptions.EndAndExpand;
                            slBucketFrame.VerticalOptions = LayoutOptions.Start;
                        //foreach (T_GRADE tgrd in tgrades)
                        for (int i = 1; i <= totalbuckets; i++)
                            {
                                Frame countframe = new Frame();
                                countframe.HasShadow = false;
                                countframe.Padding = 0; countframe.WidthRequest = 25; countframe.HeightRequest = 20;
                            //countframe.BackgroundColor = Color.FromHex(GetHexColorCodeByGradeBucketId(i));
                            countframe.BackgroundColor = Color.FromHex(App.DAUtil.GetBucketHexColorByGradeBucketId(i.ToString()) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(i.ToString()));
                                Label lblcount = new Label();
                                if (App.DAUtil.GetBucketCountBySectorIdAndGradeBucketId(tsec.sector_id, i.ToString()) != null)
                                {
                                    lblcount.Text = App.DAUtil.GetBucketCountBySectorIdAndGradeBucketId(tsec.sector_id, i.ToString());
                                }
                                else
                                {
                                    lblcount.Text = "0";
                                }
                                lblcount.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                lblcount.VerticalOptions = LayoutOptions.CenterAndExpand;
                                lblcount.TextColor = Color.White; lblcount.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                                countframe.Content = lblcount;
                                slBucketFrame.Children.Add(countframe);
                            }
                            return slBucketFrame;
                        });
                        //int loopvar = 1;
                        //foreach (T_GRADE tgrd in tgrades)
                        //{
                        //    if (loopvar == 1)
                        //    { objSec.BucketCount1 = tgrd.grade_bucket_id_count.ToString(); }
                        //    if (loopvar == 2)
                        //    { objSec.BucketCount2 = tgrd.grade_bucket_id_count.ToString(); }
                        //    if (loopvar == 3)
                        //    { objSec.BucketCount3 = tgrd.grade_bucket_id_count.ToString(); }
                        //    if (loopvar == 4)
                        //    { objSec.BucketCount4 = tgrd.grade_bucket_id_count.ToString(); }
                        //    if (loopvar == 5)
                        //    { objSec.BucketCount5 = tgrd.grade_bucket_id_count.ToString(); }
                        //    loopvar++;
                        //}
                    }
                    SectorImageList.Add(objSec);
                    // }
                    //  }
                    //====
                }
            }
            catch (Exception ex)
            {
                string strerr = ex.Message;
            }

            UserDialogs.Instance.HideLoading();
        }

        private ImageSource LoadCragAndDefaultImage()
        {
            Cache.IsCragOrDefaultImageCount = 1;
            string strimg64 = string.Empty;
            ImageSource SectorImage = null;
            //load Crag Scenic Action Portrait Shot (specific to Gym)
            var item = dbConn.Table<TCRAG_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == Settings.SelectedCragSettings);
            if (item != null)
            {
                strimg64 = item.crag_landscape_image.Split(',')[1];
                if (!string.IsNullOrEmpty(strimg64))
                {
                    byte[] imageBytes = Convert.FromBase64String(strimg64);
                    SectorImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
            }
            else
            {
                //other wise show default                                
                if (AppSetting.APP_TYPE == "indoor")
                {
                    SectorImage = ImageSource.FromFile("default_sloper_indoor_landscape");
                }
                else { SectorImage = ImageSource.FromFile("default_sloper_outdoor_landscape"); }
            }
            return SectorImage;
        }

        private async void GoToSeletedSector()
        {
            if (SelectedSector == null)
                return;
            await _navigation.PushAsync(new Views.MapDetailPage(SelectedSector));
            Cache.SendBackArrowCount = 2;
        }


        private void LoadLegendsBucket()
        {
            try
            {
                var leg_buckets = App.DAUtil.GetBucketsByCragID(Settings.SelectedCragSettings);
                if (leg_buckets != null)
                {
                    int gc = App.DAUtil.GetTotalBucketForApp();
                    int gr = leg_buckets.Count / gc;
                    Grid grdLegend = new Grid();
                    for (var i = 0; i < gr; i++)
                    {
                        grdLegend.RowDefinitions?.Add(new RowDefinition { Height = GridLength.Auto });
                    }

                    for (var i = 0; i < gc; i++)
                    {
                        grdLegend.ColumnDefinitions?.Add(new ColumnDefinition { Width = GridLength.Star });
                    }

                    var batches = leg_buckets.Select((x, i) => new { x, i }).GroupBy(p => (p.i / gc), p => p.x);

                    int r = 0;
                    foreach (var row in batches)
                    {
                        int c = 0;
                        foreach (var item in row)
                        {
                            grdLegend.Children.Add(new Label { Text = item.BucketName, HorizontalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.FromHex(item.HexColor) }, c, r);
                            c++;
                        }
                        r++;
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
                    resource = "icon_steepness_1_slab_border_20x20.png";
                    break;
                case AppSteepness.Vertical:
                    resource = "icon_steepness_2_vertical_border_20x20.png";
                    break;
                case AppSteepness.Overhanging:
                    resource = "icon_steepness_4_overhanging_border_20x20.png";
                    break;
                case AppSteepness.Roof:
                    resource = "icon_steepness_8_roof_border_20x20.png";
                    break;
            }
            return resource;
        }

        //private string GetHexColorCodeByGradeBucketId(int id)
        //{
        //    string hexcode;
        //    switch (id)
        //    {
        //        //case 1:
        //        //    hexcode = "#036177";
        //        //    break;
        //        //case 2:
        //        //    hexcode = "#1f8a70";
        //        //    break;
        //        //case 3:
        //        //    hexcode = "#91A537";
        //        //    break;
        //        //case 4:
        //        //    hexcode = "#B49800";
        //        //    break;
        //        //case 5:
        //        //    hexcode = "#FD7400";
        //        //    break;
        //        //default:
        //        //    hexcode = "#B9BABD";
        //        //    break;
        //        case 1:
        //            hexcode = "#FFFF00";
        //            break;
        //        case 2:
        //            hexcode = "#0000FF";
        //            break;
        //        case 3:
        //            hexcode = "#800080";
        //            break;
        //        case 4:
        //            hexcode = "#008000";
        //            break;
        //        case 5:
        //            hexcode = "#FF0000";
        //            break;
        //        case 6:
        //            hexcode = "#708090";
        //            break;
        //        case 7:
        //            hexcode = "#D3D3D3";
        //            break;
        //        case 8:
        //            hexcode = "#FFA500";
        //            break;
        //        default:
        //            hexcode = "#B9BABD";
        //            break;
        //    }
        //    return hexcode;
        //}
    }
}
