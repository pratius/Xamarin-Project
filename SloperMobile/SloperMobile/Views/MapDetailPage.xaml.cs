using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class MapDetailPage : ContentPage
    {
        private ViewModel.MapDetailViewModel SectorDetailVM;
        public MapDetailPage(MapListModel SelectedSector)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            SectorDetailVM = new ViewModel.MapDetailViewModel(SelectedSector,Navigation);
            BindingContext = SectorDetailVM;
        }
    }
    
}
