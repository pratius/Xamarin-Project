using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class MapPage : ContentPage
    {
        private ViewModel.MapViewModel SectorListvm;
        public MapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            SectorListvm = new ViewModel.MapViewModel(Navigation);
            BindingContext = SectorListvm;
        }
        protected override void OnAppearing()
        {
            SectorListvm.LoadMoreSector.Execute(null);
            base.OnAppearing();
        }
    }
}
