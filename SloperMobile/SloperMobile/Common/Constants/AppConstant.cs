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
        public const string APP_TITLE = "CCC Hanger";
        public const string APP_ID = "1";
        public const string APP_DBNAME = "ccchangerdb.db3";

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

        /// <summary>
        /// The email address
        /// </summary>
        public const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

    }
}