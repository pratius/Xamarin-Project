﻿using SloperMobile.Common.Constants;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class HeaderNoButtonUC : ContentView
    {
        private MapListModel _selectedSector;
        public HeaderNoButtonUC()
        {
            InitializeComponent();
            displayBackButton = false;
        }

        private async void OnBackTapped(object sender, EventArgs e)
        {
            _selectedSector = Cache.SelctedCurrentSector;
            if (Cache.SendBackArrowCount == 1)
            {
                await Navigation.PushAsync(new Views.MapDetailPage(_selectedSector));
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
