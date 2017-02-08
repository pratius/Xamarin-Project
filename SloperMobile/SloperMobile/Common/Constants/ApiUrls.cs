
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Common.Constants
{
    public class ApiUrls
    {
        public const string Base_Url = "http://sloper.slicksystems.ca:8080/DesktopModules/";
        public const string Url_Login = Base_Url + "JwtAuth/API/mobile/login";
        public const string Url_Login_Extend = Base_Url + "JwtAuth/API/mobile/extendtoken";
        public const string Url_User_Register = Base_Url + "SloperPlatform/API/v2/SloperUser/Register";
        public const string Url_CheckUpdate_AppData = Base_Url + "SloperPlatform/API/v2/M/AvailableUpdate";
        public const string Url_GetUpdate_AppData = Base_Url + "SloperPlatform/API/v2/M/GetUpdatesByType?appid={0}&since={1}&type={2}&initialize=true";
        public const string Url_GetUpdate_TopoData = Base_Url + "SloperPlatform/API/v2/TopoImageServer/Get";
        public const string Url_GetAscent_AppData = Base_Url + "SloperPlatform/API/v2/M/GetAscents?start_date={0}&end_date={1}";
        public const string Url_SendAscent_Process = Base_Url + "SloperPlatform/API/v2/Ascent/CreateAscent";

    }
}
