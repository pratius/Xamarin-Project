using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class AscentProcessPage : CarouselPage
    {
        private AscentProcessViewModel AscentProcessVM;
        public AscentProcessPage(string routeid)
        {
            InitializeComponent();
            AscentProcessVM = new AscentProcessViewModel(Navigation, routeid);
            BindingContext = AscentProcessVM;
            Title = AscentProcessVM.PageHeaderText;
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var index = Children.IndexOf(CurrentPage);
            SelectedItem = Children[index];
        }
    }
}
