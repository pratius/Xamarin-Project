using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Common.Command;
using SloperMobile.Model;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{

    public class MapDetailViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private ImageSource sectorImage;
        private List<SectorData> secdata;
        private MapListModel currentsec;
        public List<SectorData> SecData
        {
            get { return secdata; }
            set { secdata = value; OnPropertyChanged(); }
        }
        public ImageSource SectorImage
        {
            get { return sectorImage; }
            set { sectorImage = value; OnPropertyChanged(); }
        }

        public MapListModel CurrentSector
        {
            get { return currentsec; }
            set { currentsec = value;OnPropertyChanged();}
        }

        #region DelegateCommand

        public DelegateCommand TapSectorCommand { get; set; }
        #endregion
        public MapDetailViewModel(MapListModel SelectedSector, INavigation navigaton)
        {
            try
            {
                _navigation = navigaton;
                CurrentSector = SelectedSector;
                PageHeaderText = SelectedSector.SectorName;
                SectorImage = SelectedSector.SectorImage;
                TapSectorCommand = new DelegateCommand(TapOnSectorImage);
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

    public class SectorData
    {
        public string TitleText { get; set; }
        public string SubText { get; set; }
        public string ImagePath { get; set; }
    }
}
