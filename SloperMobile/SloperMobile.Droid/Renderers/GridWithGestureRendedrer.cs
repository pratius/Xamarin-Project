using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using SloperMobile.Renderers;
using SloperMobile.Listeners;

[assembly: ExportRenderer (typeof(SloperMobile.ViewModel.GridWithGesture), typeof(GridWithGestureRendedrer))]

namespace SloperMobile.Renderers
{
	public class GridWithGestureRendedrer : ViewRenderer<SloperMobile.ViewModel.GridWithGesture, Android.Views.View>, ScaleGestureDetector.IOnScaleGestureListener
	{
		private readonly FancyGestureListener _listener;
		private readonly GestureDetector _detector;
		private ScaleGestureDetector scaleGestureDetector;

		public GridWithGestureRendedrer()
		{
			_listener = new FancyGestureListener();
			_detector = new GestureDetector(_listener);
			scaleGestureDetector = new ScaleGestureDetector(this.Context, this);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<SloperMobile.ViewModel.GridWithGesture> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
				this.GenericMotion -= HandleGenericMotion;
				this.Touch -= HandleTouch;
			}

			if (e.OldElement == null)
			{
				this.GenericMotion += HandleGenericMotion;
				this.Touch += HandleTouch;
			}
		}

		void HandleTouch(object sender, TouchEventArgs e)
		{
			_detector.OnTouchEvent(e.Event);
			scaleGestureDetector.OnTouchEvent(e.Event);
		}

		void HandleGenericMotion(object sender, GenericMotionEventArgs e)
		{
			_detector.OnTouchEvent(e.Event);
			scaleGestureDetector.OnTouchEvent(e.Event);
		}

		public bool OnScale(ScaleGestureDetector detector)
		{
			Element.OnPinchAction(Element, new PinchGestureUpdatedEventArgs(GestureStatus.Running, detector.ScaleFactor, new Point(detector.FocusX, detector.FocusY)));
			return true;
		}

		public bool OnScaleBegin(ScaleGestureDetector detector)
		{
			Element.OnPinchAction(Element, new PinchGestureUpdatedEventArgs(GestureStatus.Started, detector.ScaleFactor, new Point(detector.FocusX, detector.FocusY)));
			return true;
		}

		public void OnScaleEnd(ScaleGestureDetector detector)
		{
			Element.OnPinchAction(Element, new PinchGestureUpdatedEventArgs(GestureStatus.Completed, detector.ScaleFactor, new Point(detector.FocusX, detector.FocusY)));
		}
	}
}