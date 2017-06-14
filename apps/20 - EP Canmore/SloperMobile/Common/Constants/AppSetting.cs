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
		public const string APP_ID = "20";
		public const string APP_TYPE = "indoor";

		//HockeyApp
		public const string HockeyAppId_Droid = "3d57b8a3ebcd41d9b28c07fe8577daef";
		public const string HockeyAppId_iOS = "747f9835300a4aed9d10818c885a1e04";

		//Google Map API Key Ravi
		//public const string GoogleApiKey_Droid = "AIzaSyDlezhB_TB6imlElBbrajrIEnvmdtAhfF8";
		public const string GoogleApiKey_Droid = "AIzaSyASierV3-kHa3WE6R6KGJg556J0CAFcoT8";
		public const string GoogleApiKey_iOS = "AIzaSyDArJKC42cFUb_LThCM-UmY5BhO06BVOD4";

		//Google Map API Key Steve
		//public const string GoogleApiKey_Droid = "AIzaSyDJNaoMh4e5cQyQrqeFHlP3r_Avco1AOQ4";
		//public const string GoogleApiKey_iOS = "AIzaSyBom0uG454iMScJX3v9yQdXM47524qjdIU";

		//Company
		public const string APP_TITLE = "ELEVATION PLACE";
		public const string APP_COMPANY = "Elevation Place";
		public const string APP_DBNAME = "sloper" + APP_ID + ".db3";
		public const string APP_LABEL_DROID = "Elevation Place";

		//Guest DNN Login
		public const string Guest_UserId = "elevationplace.guest@sloperclimbing.com";
		public const string Guest_UserPassword = "6';_>UQqgE3pXLw-";
	}
}
