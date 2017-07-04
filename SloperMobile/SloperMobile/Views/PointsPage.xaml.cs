using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SloperMobile.ViewModel;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PointsPage : ContentPage
    {
        PointsViewModel pointsVM;
        public PointsPage(string date)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...",Acr.UserDialogs.MaskType.Black);
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            pointsVM = new PointsViewModel(date);
            //this.BindingContext = pointsVM;
            //UserControls.PointsUC uc = new UserControls.PointsUC(date);
            //uc.Padding = new Thickness(0, -125, 0, 0);
            //MainGrid.Children.Add(uc, 1, 3);
        }
        protected override void OnAppearing()
        {
            pointsVM.OnPagePrepration();
            base.OnAppearing();
           
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }

        private void lstViewPoint_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.Item == null)
            {
                return;
            }
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null;
            // de-select the row after ripple effect
        }
    }
}