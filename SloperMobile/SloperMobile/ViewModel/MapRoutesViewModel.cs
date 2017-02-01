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

namespace SloperMobile.ViewModel
{
    public class MapRoutesViewModel:BaseViewModel
    {
        private ImageSource currentImage;

        public ImageSource CurrentSectorImage
        {
            get { return currentImage; }
            set { currentImage = value; OnPropertyChanged(); }
        }
        public MapRoutesViewModel(MapListModel CurrentSector)
        {
            SendCommand = new DelegateCommand(ExecuteOnSends);
            try
            {
                PageHeaderText = CurrentSector.SectorName;
                CurrentSectorImage = CurrentSector.SectorImage;
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

        public DelegateCommand SendCommand { get; set; }
    }
}
