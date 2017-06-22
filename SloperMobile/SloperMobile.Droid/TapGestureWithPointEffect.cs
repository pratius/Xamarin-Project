using SloperMobile.Droid;
using System;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Support.V4.View;
using Android.Util;
using System.ComponentModel;
using View = Android.Views.View;
using SloperMobile.ViewModel;

[assembly: ResolutionGroupName("AvalonSoftware")]
[assembly: ExportEffect(typeof(TapGestureWithPointEffect), "TapGestureWithPointEffect")]

namespace SloperMobile.Droid
{
    public class TapGestureWithPointEffect : PlatformEffect
    {
        private ScaleGestureDetector scaleGestureDetector;
        private GestureDetectorCompat gestureRecognizer;
        private readonly InternalGestureDetector tapDetector;
        private Command<Point> tapWithPositionCommand;
        private DisplayMetrics displayMetrics;

        public TapGestureWithPointEffect()
        {
            tapDetector = new InternalGestureDetector
            {
                TapAction = motionEvent =>
                {
                    var tap = tapWithPositionCommand;
                    if (tap != null)
                    {
                        var x = motionEvent.GetX();
                        var y = motionEvent.GetY();

                        var point = PxToDp(new Point(x, y));
                        Log.WriteLine(LogPriority.Debug, "gesture", $"Tap detected at {x} x {y} in forms: {point.X} x {point.Y}");
                        if (tap.CanExecute(point))
                            tap.Execute(point);
                    }
                }
            };
        }

        private Point PxToDp(Point point)
        {
            point.X = point.X / displayMetrics.Density;
            point.Y = point.Y / displayMetrics.Density;
            return point;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            tapWithPositionCommand = Gesture.GetCommand(Element);
        }

        private Element element;

        protected override void OnAttached()
        {
            var control = Control ?? Container;

            var context = control.Context;
            displayMetrics = context.Resources.DisplayMetrics;
            tapDetector.Density = displayMetrics.Density;

            if (gestureRecognizer == null)
                gestureRecognizer = new GestureDetectorCompat(context, tapDetector);

            control.Touch += ControlOnTouch;
            OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
        }

        private void ControlOnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            gestureRecognizer?.OnTouchEvent(touchEventArgs.Event);
        }

        protected override void OnDetached()
        {
            var control = Control ?? Container;
            control.Touch -= ControlOnTouch;
        }

        sealed class InternalGestureDetector : GestureDetector.SimpleOnGestureListener
        {
            public Action<MotionEvent> TapAction { get; set; }

            public float Density { get; set; }

            public override bool OnSingleTapUp(MotionEvent e)
            {
                TapAction?.Invoke(e);
                return true;
            }
        }
    }
}