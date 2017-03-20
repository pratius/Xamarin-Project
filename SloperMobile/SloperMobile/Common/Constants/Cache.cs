using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SloperMobile.DataBase;
using SloperMobile.Model;

namespace SloperMobile.Common.Constants
{
    public class Cache
    {
        public static string AccessToken { get; set; }
        public static MasterDetailPage MasterPage { get; set; }
        public static T_CRAG Selected_CRAG { get; set; }
        public static MapListModel SelctedCurrentSector { get; set; }
        public static int SelectedTopoIndex { get; set; }
        public static int BackArrowCount { get; set; }
        public static int CurrentScreenHeight;
    }
}
