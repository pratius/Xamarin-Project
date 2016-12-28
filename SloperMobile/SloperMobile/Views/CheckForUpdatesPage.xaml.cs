
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class CheckForUpdatesPage : ContentPage
    {
        ViewModel.CheckForUpdatesViewModel checkForUpdateVm;
        public CheckForUpdatesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            checkForUpdateVm = new ViewModel.CheckForUpdatesViewModel();
            BindingContext = checkForUpdateVm;
            checkForUpdateVm.OnPageAppearing();
        }

        //protected override void OnAppearing()
        //{
        //    checkForUpdateVm.OnPageAppearing();
        //    base.OnAppearing();
        //}
    }
}
