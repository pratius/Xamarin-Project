using SloperMobile.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.Common.Constants
{
    public class Cache
    {
        public static string AccessToken { get; set; }
        public static MasterDetailPage MasterPage { get; set; }
        public static T_CRAG Selected_CRAG { get; set; }
    }
}
