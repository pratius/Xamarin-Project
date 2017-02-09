using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SloperMobile.Common.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string DisplayName = "display_name";
        private static readonly string DefaultDisplayName = string.Empty;

        private const string AccessToken = "access_token";
        private static readonly string DefaultAccessToken = string.Empty;

        private const string RenewalToken = "renewal_token";
        private static readonly string DefaultRenewalToken = string.Empty;

        private const string SelectedCrag = "selectedcrag_id";
        private static readonly string DefaultSelectedCrag = string.Empty;

        private const string ClimbingDays = "climb_days";
        private static readonly int DefaultClimbingDays = 0;


        #endregion

        #region Getter and Setter
        public static string DisplayNameSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(DisplayName, DefaultDisplayName);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(DisplayName, value);
            }
        }
        public static string AccessTokenSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(AccessToken, DefaultAccessToken);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(AccessToken, value);
            }
        }
        public static string RenewalTokenSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(RenewalToken, DefaultRenewalToken);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(RenewalToken, value);
            }
        }
        public static string SelectedCragSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SelectedCrag, DefaultSelectedCrag);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SelectedCrag, value);
            }
        }

        public static int ClimbingDaysSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<int>(ClimbingDays, DefaultClimbingDays);
            }
            set
            {
                AppSettings.AddOrUpdateValue<int>(ClimbingDays, value);
            }
        }
        #endregion

    }
}
