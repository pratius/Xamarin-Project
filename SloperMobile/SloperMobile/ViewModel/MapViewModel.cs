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

namespace SloperMobile.ViewModel
{
    public class MapViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private MapListModel _selectedSector;
        public MapViewModel(INavigation navigation)
        {
            _navigation = navigation;
            PageHeaderText = "MAPS";
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
                    MapListModel objSec = new MapListModel();
                    objSec.SectorId = sector.sector_id;
                    var topoimg = JsonConvert.DeserializeObject<TopoImageResponse>(sector.topo_json);
                    string strimg64 = topoimg.image.data.Split(',')[1];
                    byte[] imageBytes = Convert.FromBase64String(strimg64);
                    objSec.SectorImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    objSec.SectorName = App.DAUtil.GetSectorNameBySectorID(sector.sector_id);

                    SectorImageList.Add(objSec);
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
    }
}
