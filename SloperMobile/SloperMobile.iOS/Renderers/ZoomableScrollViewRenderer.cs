using SloperMobile.iOS.Renderers;
using SloperMobile.ViewModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ZoomableScrollView), typeof(ZoomableScrollViewRenderer))]

namespace SloperMobile.iOS.Renderers
{
    public class ZoomableScrollViewRenderer : ScrollViewRenderer
    {
        private ZoomableScrollViewRenderer This {
            get{
                return this;
            }
        }

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
                this.ViewForZoomingInScrollView +=
                    (UIScrollView sv) =>
                    {
                        var view = this.Subviews[0];
                        
                        return view;
                    };

            }
        }
    }
}