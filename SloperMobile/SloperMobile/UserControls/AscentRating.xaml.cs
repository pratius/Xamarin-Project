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
        public StackLayout ObjImgAcentRatingPrv { get { return ImgAscRatingPrv; } }
        public StackLayout ObjImgAcentRatingNxt { get { return ImgAscRatingNxt; } }
        public AscentRating()
        {
            InitializeComponent();
        }
    }
}
