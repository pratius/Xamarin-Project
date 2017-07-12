
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Common.Constants
{
    public class ApiUrls
    {
        public const string Url_Login = AppSetting.Base_Url + "JwtAuth/API/mobile/login";
        public const string Url_Login_Extend = AppSetting.Base_Url + "JwtAuth/API/mobile/extendtoken";
        public const string Url_User_Register = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/SloperUser/Register";
        public const string Url_CheckUpdate_AppData = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/M/AvailableUpdate";
        public const string Url_GetInitialUpdate_AppData = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/M/GetInitialUpdatesByType?appid={0}&since={1}&type={2}&initialize={3}";
        public const string Url_GetUpdate_AppData = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/M/GetUpdatesByType?appid={0}&cragid={1}&since={2}&type={3}&initialize={4}";
        public const string Url_GetUpdate_TopoData = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/TopoImagesServer/Get";
        public const string Url_GetAscent_AppData = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/M/GetAscents";
        public const string Url_SendAscent_Process = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/Ascent/CreateAscent";
        public const string Url_Tick_List = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/TickList/AddTickList?routeId={0}";
        public const string Url_GetTick_ListData = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/TickList/GetTickList?cragId={0}";
        public const string Url_Isticklist_Route_Present = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/TickList/IsRoutePresent?routeId={0}";
        public const string Url_GetGradeBuckets = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/M/GetGradesByAppId?appid={0}";
        public const string Url_GetTTechGrades = AppSetting.Base_Url + "SloperPlatform/API/" + AppSetting.API_VERSION + "/M/GetTTechGrades?appid={0}";
        public const string Url_GetUserPoints = AppSetting.Base_Url + "/SloperPlatform/API/" + AppSetting.API_VERSION + "/M/GetPoints?appid={0}";
    }
}
