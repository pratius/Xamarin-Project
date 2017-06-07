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
        public PointsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
            BindingContext = new PointsViewModel();
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }
    }
}