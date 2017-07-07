using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;
using SloperMobile.ViewModel;

namespace SloperMobile.Views
{
    public partial class MapDetailPage : ContentPage
    {
        private ViewModel.MapDetailViewModel SectorDetailVM;
        public MapListModel _CurrentSector { get; set; }
        public MapDetailPage(MapListModel SelectedSector)
        {
            _CurrentSector = SelectedSector;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            SectorDetailVM = new ViewModel.MapDetailViewModel(SelectedSector,Navigation);
            BindingContext = SectorDetailVM;
            lstView.ItemTapped += this.OnItemTapped;
        }
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var dataItem = e.Item as RouteData;
                Navigation.PushAsync(new TopoSectorPage(_CurrentSector, dataItem.RouteId, true));
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }
    }
    
}
