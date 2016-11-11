using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class MenuListPage : ContentPage
    {
        public ListView ListView { get { return listView; } }
        MenuViewModel MenuVM;
        public MenuListPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MenuVM = new MenuViewModel();
            this.BindingContext = MenuVM;
        }
    }
}
