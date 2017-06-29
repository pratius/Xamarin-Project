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
        public PointsPage(string date)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...",Acr.UserDialogs.MaskType.Black);
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            UserControls.PointsUC uc = new UserControls.PointsUC(date);
            uc.Padding = new Thickness(0, -125, 0, 0);
            MainGrid.Children.Add(uc, 1, 3);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new PointsViewModel();
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }
    }
}