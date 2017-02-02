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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using SloperMobile.Droid;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ProgressBar), typeof(CustomProgressBar))]
namespace SloperMobile.Droid
{
    public class CustomProgressBar : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            Control.ProgressDrawable.SetColorFilter(Android.Graphics.Color.Rgb(255, 244, 0), Android.Graphics.PorterDuff.Mode.SrcIn);
            Control.ScaleY = 2; //Change the height of progressbar
        }
    }
}