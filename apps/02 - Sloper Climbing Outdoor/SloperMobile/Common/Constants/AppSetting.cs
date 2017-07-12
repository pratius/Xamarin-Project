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
		public const string API_VERSION = "v170428";
		public const string Base_Url = "http://www.sloperclimbing.com/DesktopModules/";
		public const string APP_ID = "2";
		public const string APP_TYPE = "outdoor";

		//HockeyApp
		public const string HockeyAppId_Droid = "7f4a57b534554e3abf9d0a7d45e7d9e0";
		public const string HockeyAppId_iOS = "51f531c5ab89457aa5b731a9edeca878";

		//Google Map API Key Ravi
		//public const string GoogleApiKey_Droid = "AIzaSyDlezhB_TB6imlElBbrajrIEnvmdtAhfF8";
		public const string GoogleApiKey_Droid = "AIzaSyASierV3-kHa3WE6R6KGJg556J0CAFcoT8";
		public const string GoogleApiKey_iOS = "AIzaSyDArJKC42cFUb_LThCM-UmY5BhO06BVOD4";

		//Google Map API Key Steve
		//public const string GoogleApiKey_Droid = "AIzaSyDJNaoMh4e5cQyQrqeFHlP3r_Avco1AOQ4";
		//public const string GoogleApiKey_iOS = "AIzaSyBom0uG454iMScJX3v9yQdXM47524qjdIU";

		//Company
		public const string APP_TITLE = "SLOPER CLIMBING";
		public const string APP_COMPANY = "Sloper Climbing";
		public const string APP_DBNAME = "sloper" + APP_ID + ".db3";
		public const string APP_LABEL_DROID = "Sloper Climbing";

		//Guest DNN Login
		//public const string Guest_UserId = "bull.guest@sloperclimbing.com";
		//public const string Guest_UserPassword = "G'7/[LdYbcC4K^Jr";

		//Guest DNN Login
		public const string Guest_UserId = "sloper.anonymous";
		public const string Guest_UserPassword = "s.s2016!";
	}
}