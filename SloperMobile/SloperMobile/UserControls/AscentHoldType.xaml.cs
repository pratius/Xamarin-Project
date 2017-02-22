using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class AscentHoldType : StackLayout
    {
        public AscentHoldType()
        {
            InitializeComponent();
        }
        public void SetFrameColor(object sender, EventArgs e)
        {
            var holdframe = (Frame)sender;
            if (holdframe.BackgroundColor == Color.Black)
            {
                holdframe.BackgroundColor = Color.FromHex("#FF8E2D");
            }
            else
            {
                holdframe.BackgroundColor = Color.Black;
            }
        }
    }
}
