using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankingPage : ContentPage
    {
        public RankingPage()
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...", Acr.UserDialogs.MaskType.Black);
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = new RankingViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new RankingViewModel();
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}