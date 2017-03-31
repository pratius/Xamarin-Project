
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Common.Constants
{
    public class ApiUrls
    {
        //public const string Base_Url = "http://sloper.slicksystems.ca:8080/DesktopModules/";
        public const string Base_Url = "http://www.sloperclimbing.com/DesktopModules/";

        public const string Url_Login = Base_Url + "JwtAuth/API/mobile/login";
        public const string Url_Login_Extend = Base_Url + "JwtAuth/API/mobile/extendtoken";
        public const string Url_User_Register = Base_Url + "SloperPlatform/API/v2/SloperUser/Register";
        public const string Url_CheckUpdate_AppData = Base_Url + "SloperPlatform/API/v2/M/AvailableUpdate";
        public const string Url_GetUpdate_AppData = Base_Url + "SloperPlatform/API/v2/M/GetUpdatesByType?appid={0}&since={1}&type={2}&initialize={3}";
        public const string Url_GetUpdate_TopoData = Base_Url + "SloperPlatform/API/v2/TopoImagesServer/Get";
        public const string Url_GetAscent_AppData = Base_Url + "SloperPlatform/API/v2/M/GetAscents";
        public const string Url_SendAscent_Process = Base_Url + "SloperPlatform/API/v2/Ascent/CreateAscent";
        public const string Url_Tick_List = Base_Url + "SloperPlatform/API/v2/TickList/AddTickList?routeId={0}";
        public const string Url_GetTick_ListData = Base_Url + "SloperPlatform/API/v2/TickList/GetTickList?cragId={0}";
        public const string Url_Isticklist_Route_Present = Base_Url + "SloperPlatform/API/v2/TickList/IsRoutePresent?routeId={0}";
    }
}
