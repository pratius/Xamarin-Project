
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
            CheckUpdatedDataFromLocalDB();

            string strLastUpdated = App.DAUtil.GetLastUpdate();

            if (strLastUpdated.Length == 8)
            {
                strLastUpdated = strLastUpdated.Substring(4, 2) + "/" + strLastUpdated.Substring(6, 2) + "/" + strLastUpdated.Substring(0, 4);
            }
            else
            {
                strLastUpdated = strLastUpdated.Substring(4, 2) + "/" + strLastUpdated.Substring(6, 2) + "/" + strLastUpdated.Substring(0, 4) + " " + strLastUpdated.Substring(8, 2) + ":" + strLastUpdated.Substring(10, 2) + ":" + strLastUpdated.Substring(12, 2);
            }

            lblLastUpdated.Text = "Last Updated: " + Convert.ToDateTime(strLastUpdated);
        }

        private async void CheckUpdatedDataFromLocalDB()
        {
          await  checkForUpdateVm.OnPageAppearing();
        }
        //protected override void OnAppearing()
        //{
        //    checkForUpdateVm.OnPageAppearing();
        //    base.OnAppearing();
        //}
    }
}
