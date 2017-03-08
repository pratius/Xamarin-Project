using SloperMobile.Common.Constants;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using SloperMobile.Common.Helpers;

namespace SloperMobile.Views
{
    public partial class MenuNavigationPage : MasterDetailPage
    {
        public MenuNavigationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Detail = new NavigationPage(new HomePage());
            masterMenuPage.ListView.ItemSelected += OnItemSelected;
            Cache.MasterPage = this;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                var item = e.SelectedItem as MasterPageItem;
                if (item != null)
                {
                    if (!string.IsNullOrEmpty(item.ItemId))
                    {
                        Settings.SelectedCragSettings= item.ItemId;
                    }
                    if(item.TargetType==typeof(HomePage))
                    {
                        Device.OpenUri(new Uri("http://www.sloperclimbing.com"));
                    }

                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    masterMenuPage.ListView.SelectedItem = null;
                    IsPresented = false;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
