using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class AscentRouteStyle : StackLayout
    {
        public AscentRouteStyle()
        {
            InitializeComponent();
        }
        public void SetFrameColor(object sender, EventArgs e)
        {
            var angleframe = (Frame)sender;
            if (angleframe.AutomationId == "technical")
            {
                if (angleframe.BackgroundColor == Color.Black)
                {
                    angleframe.BackgroundColor = Color.FromHex("#FF9933");
                }
                else
                {
                    angleframe.BackgroundColor = Color.Black;
                }
            }


            if (angleframe.AutomationId == "sequential")
            {
                if (angleframe.BackgroundColor == Color.Black)
                {
                    angleframe.BackgroundColor = Color.FromHex("#FF9933");
                }
                else
                {
                    angleframe.BackgroundColor = Color.Black;
                }
            }


            if (angleframe.AutomationId == "powerful")
            {
                if (angleframe.BackgroundColor == Color.Black)
                {
                    angleframe.BackgroundColor = Color.FromHex("#FF9933");
                }
                else
                {
                    angleframe.BackgroundColor = Color.Black;
                }
            }


            if (angleframe.AutomationId == "sustained")
            {
                if (angleframe.BackgroundColor == Color.Black)
                {
                    angleframe.BackgroundColor = Color.FromHex("#FF9933");
                }
                else
                {
                    angleframe.BackgroundColor = Color.Black;
                }
            }



            if (angleframe.AutomationId == "onemove")
            {
                if (angleframe.BackgroundColor == Color.Black)
                {
                    angleframe.BackgroundColor = Color.FromHex("#FF9933");
                }
                else
                {
                    angleframe.BackgroundColor = Color.Black;
                }
            }


            if (angleframe.AutomationId == "everything")
            {
                if (angleframe.BackgroundColor == Color.Black)
                {
                    Technical.BackgroundColor = Color.FromHex("#FF9933");
                    Sequential.BackgroundColor = Color.FromHex("#FF9933");
                    Powerful.BackgroundColor = Color.FromHex("#FF9933");
                    Sustained.BackgroundColor = Color.FromHex("#FF9933");
                    Onemove.BackgroundColor = Color.FromHex("#FF9933");
                    Everything.BackgroundColor = Color.FromHex("#FF9933");
                }
                else
                {
                    Technical.BackgroundColor = Color.Black;
                    Sequential.BackgroundColor = Color.Black;
                    Powerful.BackgroundColor = Color.Black;
                    Sustained.BackgroundColor = Color.Black;
                    Onemove.BackgroundColor = Color.Black;
                    Everything.BackgroundColor = Color.Black;
                }
            }
        }
    }
}
