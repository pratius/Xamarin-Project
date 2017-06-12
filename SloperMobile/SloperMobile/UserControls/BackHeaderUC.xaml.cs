using SloperMobile.Common.Constants;
using SloperMobile.Model;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class BackHeaderUC : ContentView
    {
        private MapListModel _selectedSector;
        public BackHeaderUC()
        {
            InitializeComponent();
            displayBackButton = true;
        }

        private async void OnBackTapped(object sender, EventArgs e)
        {
            _selectedSector = Cache.SelctedCurrentSector;
            if (Cache.SendBackArrowCount == 3)
            {
                await Navigation.PushAsync(new Views.MapDetailPage(_selectedSector));
                Cache.SendBackArrowCount = 2;
            }
            else if (Cache.SendBackArrowCount == 2)
            {
                await Navigation.PushAsync(new MapPage());
                Cache.SendBackArrowCount = 0;
            }
            else
            {
                await Navigation.PopAsync().ConfigureAwait(false);
            }

        }
        private async void OnSearch(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.SearchPage());
        }

        private bool displayBackButton;

        public bool DisplayBackButton
        {
            get { return displayBackButton; }
            set { displayBackButton = value; OnPropertyChanged(); }
        }

    }
}
