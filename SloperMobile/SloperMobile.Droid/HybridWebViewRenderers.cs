using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XLabs.Forms.Controls;
using Xamarin.Forms;
using SloperMobile.CustomControls;
using SloperMobile.Droid;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(HybridWebViewCustom), typeof(HybridWebViewRenderers))]
namespace SloperMobile.Droid
{
    public class HybridWebViewRenderers :HybridWebViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control != null)
            {
                //Control.Settings.UseWideViewPort = true;
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = true;
            }
            base.OnElementPropertyChanged(sender, e);
        }

     
    }
}