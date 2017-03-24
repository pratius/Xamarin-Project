using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class AscentClimbingAngle : StackLayout
    {
        public AscentClimbingAngle()
        {
            InitializeComponent();
        }
        public void SetFrameColor(object sender, EventArgs e)
        {
            var angleframe = (Frame)sender;
            if (angleframe.BackgroundColor == Color.Black)
            {
                angleframe.BackgroundColor = Color.FromHex("#FF8E2D");
            }
            else
            {
                angleframe.BackgroundColor = Color.Black;
            }
        }
    }
}
