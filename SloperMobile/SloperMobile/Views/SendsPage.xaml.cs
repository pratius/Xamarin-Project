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
