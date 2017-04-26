using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Common.Constants
{
    public class AppSetting
    {
        //API Base URL
        public const string Base_Url = "http://www.sloperclimbing.com/DesktopModules/";
        public const string APP_ID = "26";

        //HockeyApp
        public const string HockeyAppId_Droid = "dba8f25f7a6e46fbbb417550aad61431";
        public const string HockeyAppId_iOS = "091478a6166c454f9e59853563e9ab7e";

        //Company
        public const string APP_TITLE = "GRAND VALLEY CLIMBING";         public const string APP_COMPANY = "Grand Valley Climbing";         public const string APP_DBNAME = "slopergvcdb.db3";
        public const string APP_LABEL_DROID = "Grand Valley";

        //Guest DNN Login
        public const string Guest_UserId = "gvc.anonymous@sloperclimbing.com";         public const string Guest_UserPassword = "s.s2017!";
    }
}