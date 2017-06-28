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
        public const string APP_ID = "32";
		public const string APP_TYPE = "indoor";
		public const string APP_DBNAME = "sloper" + APP_ID + ".db3";

        //HockeyApp
        public const string HockeyAppId_Droid = "5938f69b0fb243bcaf30f9df53975c7d";
        public const string HockeyAppId_iOS = "6cbf84c91c994e4fa5ff9277dbde61d3";

		//Google Map API Key Steve
		public const string GoogleApiKey_Droid = "AIzaSyC45C1WQGYjmePYECElEWl4k-Zr3YDQQdU";
		public const string GoogleApiKey_iOS = "AIzaSyDFYT8I-UVDNHYru4Cvold-bZJE3M9UWas";

        //Company
        public const string APP_TITLE = "URBAN CLIMB";
        public const string APP_COMPANY = "Urban Climb";
        public const string APP_LABEL_DROID = "Urban Climb";

        //Guest DNN Login
        public const string Guest_UserId = "urbanclimb.guest@sloperclimbing.com";
        public const string Guest_UserPassword = "$KG!je,L4uJ8U8}!";
    }
}
