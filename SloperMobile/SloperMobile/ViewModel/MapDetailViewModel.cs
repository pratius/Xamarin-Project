using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    
    public class MapDetailViewModel:BaseViewModel
    {
        private ImageSource sectorImage;
        private List<SectorData> secdata;
        public List<SectorData> SecData
        {
            get { return secdata; }
            set { secdata = value; OnPropertyChanged(); }
        }
        public ImageSource SectorImage
        {
            get { return sectorImage; }
            set { sectorImage = value;OnPropertyChanged(); }
        }
        public MapDetailViewModel(MapListModel SelectedSector)
        {
            try
            {
                PageHeaderText = SelectedSector.SectorName;
                SectorImage = SelectedSector.SectorImage;
                SecData = new List<SectorData>();
                SecData.Add(new SectorData { TitleText = "03 TIME TO FLY", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "04 TOMORROW LAND", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "05 GOOD TIMES CHARLIE", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "06 NEVER NEVER MIND", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "07 GREAT EXPERIENCE", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "08 WORTHLESS CLIMBING", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "09 IT'S TIME TO CHEER UP", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "10 GREAT GREAT GREAT", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "11 WOHA!! COMPLETED FIRST CLIMBING", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
                SecData.Add(new SectorData { TitleText = "12 TIME TO FLY AGAIN", SubText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", ImagePath = "listimage" });
            }
            catch
            {
            }
        }
    }

    public class SectorData
    {
        public string TitleText { get; set; }
        public string SubText { get; set; }
        public string ImagePath { get; set; }
    }
}
