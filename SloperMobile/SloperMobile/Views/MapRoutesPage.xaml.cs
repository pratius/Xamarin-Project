using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class MapRoutesPage : ContentPage
    {
        private ViewModel.MapRoutesViewModel MapRouteVM;
        public MapRoutesPage(MapListModel CurrentSector)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MapRouteVM = new ViewModel.MapRoutesViewModel(CurrentSector);
            BindingContext = MapRouteVM;
        }
    }
}
