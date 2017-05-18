using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using SloperMobile.ViewModel;
using SkiaSharp.Views.iOS;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(SloperMobile.ViewModel.TouchCanvasView), typeof(SloperMobile.iOS.TouchCanvasViewRenderer))]
namespace SloperMobile.iOS
{
    public class TouchCanvasViewRenderer : SKCanvasViewRenderer
    {       
        private readonly UITapGestureRecognizer tapGestureRecognizer;
        private readonly UIPinchGestureRecognizer pinchGestureRecognizer;

        public TouchCanvasViewRenderer()
        {
            tapGestureRecognizer = new UITapGestureRecognizer(OnTapped);
            pinchGestureRecognizer = new UIPinchGestureRecognizer(OnPinchUpdated);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SkiaSharp.Views.Forms.SKCanvasView> e)
        {
            // clean up old native control
            if (Control != null)
            {
                Control.RemoveGestureRecognizer(tapGestureRecognizer);
                Control.RemoveGestureRecognizer(pinchGestureRecognizer);
            }

            // do clean up old element
            if (Element != null)
            {
                var oldTouchCanvas = (TouchCanvasView)Element;
                var oldTouchController = (ITouchCanvasViewController)Element;

                // ...
            }

            base.OnElementChanged(e);

            // set up new native control
            if (Control != null)
            {
                Control.UserInteractionEnabled = true;
                Control.AddGestureRecognizer(tapGestureRecognizer);
                Control.AddGestureRecognizer(pinchGestureRecognizer);
            }

            // set up new element
            if (e.NewElement != null)
            {
                var newTouchCanvas = (TouchCanvasView)Element;
                var newTouchController = (ITouchCanvasViewController)Element;

                // ...
            }
        }

        private void OnTapped(UITapGestureRecognizer recognizer)
        {
            var touchController = Element as ITouchCanvasViewController;
            if (touchController != null)
            {
                touchController.OnTouch(recognizer.LocationInView(Control).ToSKPoint());
            }
        }
        private void OnPinchUpdated(UIPinchGestureRecognizer recognizer)
        {           
            var touchController = Element as ITouchCanvasViewController;            
            if (touchController != null)
            {
                touchController.OnPinchUpdated(recognizer);
            }
        }        
    }
}