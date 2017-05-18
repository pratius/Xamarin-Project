using SloperMobile.ViewModel;
using SloperMobile.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.DataBase;
using SloperMobile.Common.Enumerators;
using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class SendsPage : ContentPage
    {
        #region Properties

        public new event PropertyChangedEventHandler PropertyChanged;

        #region PointerValue

        private double pointerValue = 60;

        public double PointerValue
        {
            get { return pointerValue; }
            set
            {
                pointerValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("PointerValue"));
                }
            }
        }

        #endregion PointerValue

        #endregion Properties
        public SendsPage(string TabName)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new SendsViewModel(TabName, Navigation);
            if (TabName == "SENDS")
            {
                SendsScrollViewer.IsVisible = true;
                TickListsScrollViewer.IsVisible = false;
                PointScrollViewer.IsVisible = false;
                main_layout.IsVisible = true;
                Ticklists.IsVisible = false;
                Sends.IsVisible = true;
                Points.IsVisible = false;
            }
            else if (TabName == "TICKLIST")
            {
                SendsScrollViewer.IsVisible = false;
                TickListsScrollViewer.IsVisible = true;
                PointScrollViewer.IsVisible = false;
                main_layout.IsVisible = false;
                Ticklists.IsVisible = true;
                Sends.IsVisible = false;
                Points.IsVisible = false;
            }
            else if (TabName == "POINTS")
            {
                SendsScrollViewer.IsVisible = false;
                TickListsScrollViewer.IsVisible = false;
                PointScrollViewer.IsVisible = true;
                main_layout.IsVisible = false;
                Ticklists.IsVisible = false;
                Sends.IsVisible = false;
                Points.IsVisible = true;
            }
        }


        private void Handle_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //var data = sender as SegmentedControl.FormsPlugin.Abstractions.SegmentedControl;

                //SendVM.ExecuteModeChange(data.SelectedText);
            }

            catch
            {

            }
        }
    }
}
