using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{

    
    public partial class AscentDate : StackLayout
    {
        public StackLayout ObjImgAcentDatePrv { get { return ImgAcentDatePrv; } }
        public StackLayout ObjImgAcentDateNxt { get { return ImgAcentDateNxt; } }
        public AscentDate()
        {
            InitializeComponent();
           
        }



    }
}
