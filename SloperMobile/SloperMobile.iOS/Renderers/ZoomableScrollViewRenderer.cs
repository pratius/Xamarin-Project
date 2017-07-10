using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using SloperMobile.iOS.Renderers;
using Xamarin.Forms;
using SloperMobile.ViewModel;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ZoomableScrollView), typeof(ZoomableScrollViewRenderer))]

namespace SloperMobile.iOS.Renderers
{
    public class ZoomableScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            if (e.OldElement == null)
            {
                var zsv = Element as ZoomableScrollView;
                this.MinimumZoomScale = zsv.MinimumZoomScale;
                this.MaximumZoomScale = zsv.MaximumZoomScale;
                this.ViewForZoomingInScrollView += (UIScrollView sv) => this.Subviews[0];
            }
        }
    }
}