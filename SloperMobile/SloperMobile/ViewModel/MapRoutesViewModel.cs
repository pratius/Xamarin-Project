using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;

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
            try
            {
                PageHeaderText = CurrentSector.SectorName;
                CurrentSectorImage = CurrentSector.SectorImage;
            }
            catch
            {
            }
        }
    }
}
