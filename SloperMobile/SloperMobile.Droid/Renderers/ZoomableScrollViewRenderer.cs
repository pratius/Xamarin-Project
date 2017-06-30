using Android.Util;
using Android.Views;
using Android.Views.Animations;
using SkiaSharp.Views.Forms;
using SloperMobile.Droid;
using SloperMobile.ViewModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ZoomableScrollView), typeof(ZoomableScrollViewRenderer))]

namespace SloperMobile.Droid
{
    public class ZoomableScrollViewRenderer : ScrollViewRenderer, ScaleGestureDetector.IOnScaleGestureListener
    {
        private SKCanvasView canvas;
        ZoomableScrollView svMain;
        private AbsoluteLayout absoluteLayout;
        private DisplayMetrics displayMetrics;
        private ScaleGestureDetector _scaleDetector;
        private float mScale;
        private bool _isScaleProcess;

        public ZoomableScrollViewRenderer()
        {
            _scaleDetector = new ScaleGestureDetector(Context, this);
            displayMetrics = Context.Resources.DisplayMetrics;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            try
            {
                this.VerticalScrollBarEnabled = true;
                this.HorizontalScrollBarEnabled = true;
                svMain = ((ZoomableScrollView)e.NewElement);
                absoluteLayout = svMain.Content as AbsoluteLayout;
                canvas = absoluteLayout.Children.FirstOrDefault() as SKCanvasView;
            }
            catch (System.Exception)
            {
            }
        }

        public override bool DispatchTouchEvent(Android.Views.MotionEvent e)
        {
            if (e.PointerCount == 2)
            {
                return _scaleDetector.OnTouchEvent(e);
            }
            else if (_isScaleProcess)
            {
                //HACK:
                //Prevent letting any touch events from moving the scroll view until all fingers are up from zooming...This prevents the jumping and skipping around after user zooms.
                if (e.Action == MotionEventActions.Up)
                {
                    _isScaleProcess = false;
                }

                if (e.Action == MotionEventActions.Down)
                {
                    _isScaleProcess = false;
                }

                return false;
            }

            return base.DispatchTouchEvent(e);
        }

        public bool OnScale(ScaleGestureDetector detector)
        {
            float prevScale = mScale;

            if (detector.ScaleFactor == 1)
            {
                return true;
            }
            else if (detector.ScaleFactor < 1)
            {
                svMain.IsScalingDown = true;
                svMain.IsScalingUp = false;
                mScale = detector.ScaleFactor;
            }
            else if (detector.ScaleFactor > 1)
            {
                svMain.IsScalingDown = false;
                svMain.IsScalingUp = true;
                mScale = detector.ScaleFactor;
            }

            var pivotX = detector.FocusX;
            var pivotY = detector.FocusY;

            var scaleAnimation = new ScaleAnimation(prevScale, mScale, prevScale, mScale, pivotX, pivotY);
            scaleAnimation.Duration = 2;
            scaleAnimation.FillAfter = true;
            StartAnimation(scaleAnimation);
           
            svMain.ScaleFactor = detector.ScaleFactor;
           
            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {
            svMain.ScaleFactor = 1;
            svMain.IsScalingDown = true;
            svMain.IsScalingUp = false;
        }
    }
}