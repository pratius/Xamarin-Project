using System.Linq;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using SkiaSharp.Views.Forms;
using SloperMobile.Droid;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using SloperMobile.ViewModel;

[assembly: ExportRenderer(typeof(ZoomableScrollView), typeof(ZoomableScrollViewRenderer))]
namespace SloperMobile.Droid
{
    public class ZoomableScrollViewRenderer : ScrollViewRenderer, ScaleGestureDetector.IOnScaleGestureListener
    {
        float originalDistanceX, currentdistanceX, originalDistanceY, currentdistanceY; private bool _isScaleProcess = false;
        bool IsPinching = false;
        double currentScale;
        private SKCanvasView canvas;
        ScrollView svMain, svSub;
        private StackLayout absoluteLayout;
        private DisplayMetrics displayMetrics;
        private ScaleGestureDetector _scaleDetector;
        private float mScale;
        private Command<Point> tapWithPositionCommand;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            _scaleDetector = new ScaleGestureDetector(Context, this);
            displayMetrics = Context.Resources.DisplayMetrics;
            svMain = ((ScrollView)e.NewElement);
            absoluteLayout = svMain.Content as StackLayout;
            canvas = absoluteLayout.Children.FirstOrDefault() as SKCanvasView;
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
            float scale = 1 - detector.ScaleFactor;

            float prevScale = mScale;
            mScale += scale;

            if (mScale < 0.1f) // Minimum scale condition:
                mScale = 0.1f;

            if (mScale > 10f) // Maximum scale condition:
                mScale = 10f;

            var fromX = 1f / prevScale;
            var toX = 1f / mScale;
            var fromY = 1f / prevScale;
            var toY = 1f / mScale;
            var pivotX = detector.FocusX;
            var pivotY = detector.FocusY;

            if (toX < 1 || toY < 1)
            {
                return true;
            }

            var scaleAnimation = new ScaleAnimation(fromX, toX, fromY, toY, pivotX, pivotY);
            scaleAnimation.Duration = 0;
            scaleAnimation.FillAfter = true;
            StartAnimation(scaleAnimation);

            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {
        }
    }
}