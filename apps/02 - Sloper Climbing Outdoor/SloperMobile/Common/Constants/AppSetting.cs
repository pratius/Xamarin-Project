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
		public const string APP_ID = "2";
		public const string APP_TYPE = "outdoor";
		public const string APP_DBNAME = "sloper" + APP_ID + ".db3";

		//HockeyApp
		public const string HockeyAppId_Droid = "7f4a57b534554e3abf9d0a7d45e7d9e0";
		public const string HockeyAppId_iOS = "51f531c5ab89457aa5b731a9edeca878";

		//Google Map API Key Steve
        public const string GoogleApiKey_Droid = "AIzaSyC45C1WQGYjmePYECElEWl4k-Zr3YDQQdU";
		public const string GoogleApiKey_iOS = "AIzaSyDFYT8I-UVDNHYru4Cvold-bZJE3M9UWas";

		//Company
		public const string APP_TITLE = "SLOPER CLIMBING";
		public const string APP_COMPANY = "Sloper Climbing";
		public const string APP_LABEL_DROID = "Sloper Climbing";

		//Guest DNN Login
		public const string Guest_UserId = "sloper.guest@sloperclimbing.com";
		public const string Guest_UserPassword = "V($@'UEa7ZM4x]/<";
	}
}