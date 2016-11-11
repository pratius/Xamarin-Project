﻿
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
        public const string Url_Login_Extend = Base_Url + "/JwtAuth/API/mobile/extendtoken";
        public const string Url_User_Register = Base_Url + "SloperPlatform/API/v2/SloperUser/Register";
        public const string Url_CheckUpdate_AppData = Base_Url + "SloperPlatform/API/v2/DataExporterTest/AreUpdatesAvailableWithFormat";
        public const string Url_GetUpdate_AppData = Base_Url + "SloperPlatform/API/v2/DataExporterTest/GetCodeTemplate?appid={0}&format={1}&type={2}&since={3}";
        public const string Url_GetUpdate_TopoData = Base_Url + "SloperPlatform/API/v2/TopoImageServer/Get";
    }
}
