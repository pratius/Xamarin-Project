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
namespace SloperMobile.Droid
{
    [Activity(Label = "SloperMobile", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Cache.CurrentScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            var container = new SimpleContainer();

            container.Register<IJsonSerializer, JsonSerializer>();
            container.Register<IDevice>(AndroidDevice.CurrentDevice);

            Resolver.SetResolver(container.GetResolver());

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();


            LoadApplication(new App());
            CrashManager.Register(this, AppSetting.HockeyAppId);
            //// in your main activity OnCreate-method add:
            MetricsManager.Register(this, Application, AppSetting.HockeyAppId);
        }
    }
}

