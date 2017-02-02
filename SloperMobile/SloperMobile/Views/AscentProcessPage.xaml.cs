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
        public AscentProcessPage()
        {
            InitializeComponent();
            Title = "Ascent Process";
            BindingContext = new AscentProcessViewModel(Navigation);
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var index = Children.IndexOf(CurrentPage);
            SelectedItem = Children[index];
        }
    }
}
