using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
   
    public partial class AscentRating : StackLayout
    {
        public Image ObjImgAcentRatingPrv { get { return ImgAscRatingPrv; } }
        public Image ObjImgAcentRatingNxt { get { return ImgAscRatingNxt; } }
        public AscentRating()
        {
            InitializeComponent();
        }
    }
}
