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
using Xamarin.Forms;
using SkiaSharp.Views.Android;
using Xamarin.Forms.Platform.Android;
using SloperMobile.ViewModel;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using UIKit;

[assembly: ExportRenderer(typeof(SloperMobile.ViewModel.TouchCanvasView), typeof(SloperMobile.Droid.TouchCanvasViewRenderer))]
namespace SloperMobile.Droid
{
    public class TouchCanvasViewRenderer : SKCanvasViewRenderer
    {
        private readonly UIPinchGestureRecognizer pinchGestureRecognizer;
        protected override void OnElementChanged(ElementChangedEventArgs<SkiaSharp.Views.Forms.SKCanvasView> e)
        {            
            // clean up old native control
            if (Control != null)
            {
                Control.Touch -= OnTouch;                                           
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
                Control.Touch += OnTouch;
            }

            // set up new element
            if (e.NewElement != null)
            {
                var newTouchCanvas = (TouchCanvasView)Element;
                var newTouchController = (ITouchCanvasViewController)Element;

                // ...
            }
        }

        private void OnTouch(object sender, TouchEventArgs e)
        {
            var touchController = Element as ITouchCanvasViewController;
            if (touchController != null)
            {
                var wasTap = e.Event.ActionMasked == MotionEventActions.Up || e.Event.ActionMasked == MotionEventActions.Cancel;
                if (wasTap)
                {
                    var scale = Control.Resources.DisplayMetrics.Density;
                    touchController.OnTouch(new SKPoint(e.Event.GetX() / scale, e.Event.GetY() / scale));
                }
            }
        }
    }
}