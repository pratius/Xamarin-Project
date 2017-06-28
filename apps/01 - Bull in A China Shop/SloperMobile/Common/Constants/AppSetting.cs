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
		public const string Base_Url = "http://sloper.slicksystems.ca:8080/DesktopModules/";
        public const string API_VERSION = "v170428";
		public const string APP_ID = "1";
		public const string APP_TYPE = "outdoor";
		public const string APP_DBNAME = "sloper" + APP_ID + ".db3";

		//HockeyApp
		public const string HockeyAppId_Droid = "c528525a7d654946a5692d82f19a8228";
		public const string HockeyAppId_iOS = "12da4aa9ca2044228df1cca19fd7675d";

		//Google Map API Key Steve
		public const string GoogleApiKey_Droid = "AIzaSyC45C1WQGYjmePYECElEWl4k-Zr3YDQQdU";
		public const string GoogleApiKey_iOS = "AIzaSyDFYT8I-UVDNHYru4Cvold-bZJE3M9UWas";

		//Company
		public const string APP_TITLE = "BULL IN A CHINA SHOP";
		public const string APP_COMPANY = "Bull in a China Shop";
		public const string APP_LABEL_DROID = "Bull App";

		//Guest DNN Login
		public const string Guest_UserId = "bull.guest@sloperclimbing.com";
		public const string Guest_UserPassword = "G'7/[LdYbcC4K^Jr";
	}
}
