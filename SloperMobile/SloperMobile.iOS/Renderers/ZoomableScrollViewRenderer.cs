using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
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

        protected async override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            if (e.OldElement == null)
            {
                var zsv = Element as ZoomableScrollView;
                this.MinimumZoomScale = 1;
                this.MaximumZoomScale = zsv.MaximumZoomScale;
                this.ViewForZoomingInScrollView +=
                    (UIScrollView sv) =>
                    {
                        var view = this.Subviews[0];//.Subviews[0];
                        
                        return view;
                    };

                this.ZoomingEnded += (sender, eventArgs) => {
                    zsv.ScaleFactor = (float)eventArgs.AtScale;
                    zsv.IsScalingUp = true;
                    zsv.IsScalingDown = true;
                    zsv.RescaleOniOS();
                };

                await Task.Delay(1000);
                zsv.IsScalingUp = true;
                zsv.IsScalingDown = true;
                zsv.RescaleOniOS();

            }
        }
    }
}