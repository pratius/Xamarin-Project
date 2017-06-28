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
        public const string APP_ID = "33";
		public const string APP_TYPE = "indoor";
		public const string APP_DBNAME = "sloper" + APP_ID + ".db3";

		//HockeyApp
		public const string HockeyAppId_Droid = "6d8aec25c7564af2a8e643de45b07999";
		public const string HockeyAppId_iOS = "d8fd7e5fcbc24806af65b7e3ee1d0392";

		//Google Map API Key Steve
		public const string GoogleApiKey_Droid = "AIzaSyC45C1WQGYjmePYECElEWl4k-Zr3YDQQdU";
		public const string GoogleApiKey_iOS = "AIzaSyDFYT8I-UVDNHYru4Cvold-bZJE3M9UWas";

		//Company
		public const string APP_TITLE = "ASCEND";
		public const string APP_COMPANY = "Ascend";
		public const string APP_LABEL_DROID = "Ascend";

		//Guest DNN Login
		public const string Guest_UserId = "ascend.guest@sloperclimbing.com";
		public const string Guest_UserPassword = ",>`6h5K7am-GaZ~5";
	}
}
