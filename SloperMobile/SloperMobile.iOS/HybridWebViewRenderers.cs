using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using XLabs.Forms.Controls;

namespace SloperMobile.iOS
{
    public class HybridWebViewRenderers : HybridWebViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control != null)
            {
                var web = (UIWebView)NativeView;
                web.ScrollView.ScrollEnabled = true;
                //web.ScalesPageToFit = true;
                web.ScalesPageToFit = true;
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}