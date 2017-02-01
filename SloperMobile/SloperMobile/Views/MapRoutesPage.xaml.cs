using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;
using Xamarin.Forms;
using SloperMobile.Common.Constants;

namespace SloperMobile.Views
{
    public partial class MapRoutesPage : ContentPage
    {
        public MapListModel _CurrentSector { get; set; }
        private ViewModel.MapRoutesViewModel MapRouteVM;
        public MapRoutesPage(MapListModel CurrentSector)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MapRouteVM = new ViewModel.MapRoutesViewModel(CurrentSector);
            BindingContext = MapRouteVM;
            MapRouteVM.OnPageNavigation = OnPageNavigation;
            _CurrentSector = CurrentSector;
        }       
        private async void OnPageNavigation()
        {
            await Navigation.PushAsync(new AscentTypePage());
        }

        protected override void OnAppearing()
        {
            this.webView.LoadFinished += OnLoadFinished;
            this.webView.LoadFromContent("HTML/Index.html");
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            webView.LoadFinished -= OnLoadFinished;
        }

        private void OnLoadFinished(object sender, EventArgs args)
        {
            var listData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
            int count = 0;
            // hybrid.CallJsFunction("bindImage", "");            
            if (listData.points.Count > 0)
            {
                foreach(var line in listData.points)
                {
                    count++;
                    //drawLine(80, 55, 75, 120, 1);//1 =button value
                    webView.CallJsFunction("drawLine", line.x, "0", "0", line.y, count);
                }
            }
        }
    }
}
