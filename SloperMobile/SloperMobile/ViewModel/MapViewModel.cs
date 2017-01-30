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

namespace SloperMobile.ViewModel
{
    public class MapViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private MapListModel _selectedSector;
        private T_CRAG currentCrag;
        public MapViewModel(INavigation navigation)
        {
            currentCrag = App.DAUtil.GetSelectedCragData();
            _navigation = navigation;
            PageHeaderText = currentCrag.crag_name;
            PageSubHeaderText = currentCrag.area_name;
            LoadMoreSector = new DelegateCommand(LoadSectorImages);
        }
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

        #region DelegateCommand

        public DelegateCommand LoadMoreSector { get; set; }
        #endregion

        private void LoadSectorImages(object obj)
        {
            try
            {
                var sector_images = App.DAUtil.GetSectorImages(SectorImageList.Count(), 10);
                foreach (var sector in sector_images)
                {
                    var topoimg = JsonConvert.DeserializeObject<TopoImageResponse>(sector.topo_json);
                    if (!string.IsNullOrEmpty(topoimg.image.data))
                    {
                        string strimg64 = topoimg.image.data.Split(',')[1];
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            MapListModel objSec = new MapListModel();
                            objSec.SectorId = sector.sector_id;
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            objSec.SectorImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
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
                                objSec.Steepness1 = ImageSource.FromFile(GetSteepnessResourceName(1));
                                objSec.Steepness2 = ImageSource.FromFile(GetSteepnessResourceName(2));
                            }
                            var tgrades = App.DAUtil.GetBucketCountsBySectorId(tsec.sector_id);
                            if (tgrades != null)
                            {
                                int loopvar = 1;
                                foreach (T_GRADE tgrd in tgrades)
                                {
                                    if (loopvar == 1)
                                    { objSec.BucketCount1 = tgrd.grade_bucket_id_count.ToString(); }
                                    if (loopvar == 2)
                                    { objSec.BucketCount2 = tgrd.grade_bucket_id_count.ToString(); }
                                    if (loopvar == 3)
                                    { objSec.BucketCount3 = tgrd.grade_bucket_id_count.ToString(); }
                                    if (loopvar == 4)
                                    { objSec.BucketCount4 = tgrd.grade_bucket_id_count.ToString(); }
                                    if (loopvar == 5)
                                    { objSec.BucketCount5 = tgrd.grade_bucket_id_count.ToString(); }
                                    loopvar++;
                                }
                            }
                            SectorImageList.Add(objSec);
                        }
                    }
                    //====
                }
            }
            catch (Exception ex)
            {
                string strerr = ex.Message;
            }
        }

        private async void GoToSeletedSector()
        {
            if (SelectedSector == null)
                return;
            await _navigation.PushAsync(new Views.MapDetailPage(SelectedSector));
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
            }
            return resource;
        }
    }
}
