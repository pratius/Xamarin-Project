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
        public const string API_VERSION = "v170428";
        public const string APP_ID = "24";
	    public const string APP_TYPE = "indoor";
		public const string APP_DBNAME = "sloper" + APP_ID + ".db3";

        //HockeyApp
        public const string HockeyAppId_Droid = "25c2e7a936874599ace739cbe1505963";
        public const string HockeyAppId_iOS = "412a08835dd34140a4b54af501d2d45c";

		//Google Map API Key Steve
		public const string GoogleApiKey_Droid = "AIzaSyC45C1WQGYjmePYECElEWl4k-Zr3YDQQdU";
		public const string GoogleApiKey_iOS = "AIzaSyDFYT8I-UVDNHYru4Cvold-bZJE3M9UWas";

        //Company
        public const string APP_TITLE = "STONE AGE";
        public const string APP_COMPANY = "Stone Age";
        public const string APP_LABEL_DROID = "Stone Age";

        //Guest DNN Login
        public const string Guest_UserId = "stoneage.guest@sloperclimbing.com";
        public const string Guest_UserPassword = "U=xx4b_8$WkTF_SH";
    }
}
