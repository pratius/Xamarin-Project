using System;
using Android.Views;

namespace SloperMobile.Listeners
{
	public class FancyGestureListener : Java.Lang.Object, ScaleGestureDetector.IOnScaleGestureListener, GestureDetector.IOnGestureListener
	{
		public bool OnScale(ScaleGestureDetector detector)
		{
			return true;
			////throw new NotImplementedException();
		}

		public bool OnScaleBegin(ScaleGestureDetector detector)
		{
			return true;
			//throw new NotImplementedException();
		}

		public void OnScaleEnd(ScaleGestureDetector detector)
		{
			//throw new NotImplementedException();
		}

		public bool OnDown(MotionEvent e)
		{
			return true;
			//throw new NotImplementedException();
		}

		public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			return true;
			//throw new NotImplementedException();
		}

		public void OnLongPress(MotionEvent e)
		{
			//throw new NotImplementedException();
		}

		public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			return true;
			//throw new NotImplementedException();
		}

		public void OnShowPress(MotionEvent e)
		{
			//throw new NotImplementedException();
		}

		public bool OnSingleTapUp(MotionEvent e)
		{
			return true;
			//throw new NotImplementedException();
		}
	}
}