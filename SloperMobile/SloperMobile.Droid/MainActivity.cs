using Android.App;
using Android.Content.PM;
using Android.OS;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using ImageCircle.Forms.Plugin.Droid;
using SloperMobile.Common.Constants;
using XLabs.Ioc;
using XLabs.Serialization;
using XLabs.Serialization.JsonNET;
using XLabs.Forms.Controls;
using XLabs.Platform.Device;
using Acr.UserDialogs;
using Xamarin.Forms;
using SloperMobile.MessagingTask;
using Android.Content;
using SloperMobile.Droid.Services;
using System;
using Plugin.Permissions;

namespace SloperMobile.Droid
{
    [Activity(Label = AppSetting.APP_LABEL_DROID, Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //protected override void OnDestroy()
        //{
        //    try
        //    {
        //        base.OnDestroy();
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //}

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //take screen height -40 for top nav and 40 for bottom nav
            Cache.CurrentScreenHeight = (int)((Resources.DisplayMetrics.HeightPixels) / Resources.DisplayMetrics.Density) - 80;
            var container = new SimpleContainer();

            container.Register<IJsonSerializer, JsonSerializer>();
            container.Register<IDevice>(AndroidDevice.CurrentDevice);
            Resolver.ResetResolver();
            Resolver.SetResolver(container.GetResolver());

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle); // initialize for Xamarin.Forms.GoogleMaps
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);

            LoadApplication(new App());

            //Add HockeyApp Reporting
            CrashManager.Register(this, AppSetting.HockeyAppId_Droid, new CrashManagerListenerImp());
            MetricsManager.Register(this, Application, AppSetting.HockeyAppId_Droid);

            WireUpCheckUpdateRunningTask();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void WireUpCheckUpdateRunningTask()
        {
            MessagingCenter.Subscribe<StartCheckForUpdatesTask>(this, "StartCheckForUpdatesTaskMessage", message =>
            {
                var intent = new Intent(this, typeof(CheckUpdatesTaskService));
                StartService(intent);
            });
            MessagingCenter.Subscribe<StopCheckForUpdatesTask>(this, "StopCheckForUpdatesTaskMessage", message =>
            {
                var intent = new Intent(this, typeof(CheckUpdatesTaskService));
                StopService(intent);
            });
        }
    }

    public class CrashManagerListenerImp : CrashManagerListener
    {
        public override bool ShouldAutoUploadCrashes()
        {
            base.ShouldAutoUploadCrashes();
            return true;
        }
    }

}