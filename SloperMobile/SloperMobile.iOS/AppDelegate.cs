using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using HockeyApp.iOS;
using SloperMobile.Common.Constants;
using Syncfusion.SfGauge.XForms.iOS;
using ImageCircle.Forms.Plugin.iOS;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Serialization;
using XLabs.Platform.Device;
using Syncfusion.SfRating.XForms.iOS;
using XLabs.Serialization.JsonNET;
using Xamarin.Forms;
using SloperMobile.MessagingTask;
using SloperMobile.iOS.Services;
using Syncfusion.SfChart.XForms.iOS.Renderers;

namespace SloperMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        iOSCheckUpdatesTaskService updateRunningTask;
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            HybridWebViewRenderer.CopyBundleDirectory("HTML");
            var container = new SimpleContainer();

            container.Register<IJsonSerializer, JsonSerializer>();
            container.Register<IDevice>(AppleDevice.CurrentDevice);

            Resolver.SetResolver(container.GetResolver());
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsGoogleMaps.Init(AppSetting.GoogleApiKey_iOS); // initialize for Xamarin.Forms.GoogleMaps
            ImageCircleRenderer.Init();
            Cache.CurrentScreenHeight = (int)((int)(UIScreen.MainScreen.Bounds.Height * (int)UIScreen.MainScreen.Scale) * 2);
            LoadApplication(new App());

            //Add HockeyApp Reporting
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure(AppSetting.HockeyAppId_iOS);
            manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation();
            new SfChartRenderer();
            new SfGaugeRenderer();
            new SfRatingRenderer();
            WireUpCheckUpdateRunningTask();
            return base.FinishedLaunching(app, options);
        }

        void WireUpCheckUpdateRunningTask()
        {
            MessagingCenter.Subscribe<StartCheckForUpdatesTask>(this, "StartCheckForUpdatesTaskMessage", async message =>
            {
                updateRunningTask = new iOSCheckUpdatesTaskService();
                await updateRunningTask.Start();
            });

            MessagingCenter.Subscribe<StopCheckForUpdatesTask>(this, "StopCheckForUpdatesTaskMessage", message =>
            {
                updateRunningTask.Stop();
            });
        }
    }
}
