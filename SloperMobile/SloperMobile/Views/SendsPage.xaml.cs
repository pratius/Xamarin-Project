using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SendsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new SendsViewModel();
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
