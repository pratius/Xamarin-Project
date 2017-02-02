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
    }
}
