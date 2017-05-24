using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.UserControls;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class AscentType : StackLayout
    {
        public StackLayout ObjImgAcentTypeNxt { get { return ImgAcentTypeNxt; } }
        public AscentType()
        {
            InitializeComponent();
            BackHeaderUC myBackHeader = new BackHeaderUC();
            myBackHeader.DisplayBackButton = true;
        }
        public void SetFrameColor(object sender, EventArgs e)
        {
            Onsight.BackgroundColor = Color.Black;
            Flash.BackgroundColor = Color.Black;
            Redpoint.BackgroundColor = Color.Black;
            Repeat.BackgroundColor = Color.Black;
            ProjectBurn.BackgroundColor = Color.Black;
            OneHang.BackgroundColor = Color.Black;
            var ascframe = (Frame)sender;
            ascframe.BackgroundColor = Color.FromHex("#FF8E2D");
        }
    }
}
