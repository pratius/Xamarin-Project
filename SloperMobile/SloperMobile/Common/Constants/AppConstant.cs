using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SloperMobile.Common.Constants
{
    public class AppConstant
    {
        public const string APP_TITLE = "Sloper Mobile";
        public const string APP_ID = "1";
        public const string APP_DBNAME = "sloperdb.db3";
        public const string APP_COMPANY = "Sloper Inc.";

        public const string EMPTY_STRING = "";
        public const string NETWORK_FAILURE = "No Network Connection found! Please try again.";
        public const string LOGIN_FAILURE = "Login Failed! Please try again.";
        public const string REGISTRATION_FAILURE = "Registration Failed! Please try again.";

        public const string UPDATE_FAILURE_MSG = "Your app data is up to date.";

        public const string SPF_USER_DISPLAYNAME = "displayName";
        public const string SPF_ACCESSTOKEN = "accessToken";
        public const string SPF_RENEWALTOKEN = "renewalToken";

        public const string Guest_UserId = "sloper.anonymous";
        public const string Guest_UserPassword = "s.s2016!";

        //MapRoutePage
        public const string RouteType_climbing_Angle = "climbing angle";
        public const string RouteType_hold_Type = "hold type";
        public const string RouteType_Route_Style = "route style";

        //Images For RouteType Angle
        public const string RouteType_Angle_Slab_1 = "angle_1_slab";
        public const string RouteType_Angle_Vertical_2 = "angle_2_vertical";
        public const string RouteType_Angle_Overhanging_4 = "angle_4_overhanging";
        public const string RouteType_Angle_Roof_8 = "angle_8_roof";

        //Images for Hold Type 
        public const string RouteType_Hold_Slopers_1 = "hold_type_1_slopers";
        public const string RouteType_Hold_Crimp_2 = "hold_type_2_crimps";
        public const string RouteType_Hold_Jungs_4 = "hold_type_4_jugs";
        public const string RouteType_Hold_Pockets_8 = "hold_type_8_pockets";

        //Images for Route Style
        public const string RouteType_Route_Style_Technical_1 = "route_style_1_technical";
        public const string RouteType_Route_Style_Sequential_2 = "route_style_2_sequential";
        public const string RouteType_Route_Style_Powerful_4 = "route_style_4_powerful";
        public const string RouteType_Route_Style_Sustained_8= "route_style_8_sustained";
        public const string RouteType_Route_Style_One_Move_16 = "route_style_16_one_move";

        /// <summary>
        /// The email address
        /// </summary>
        public const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

    }
}