using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
using SloperMobile.ViewModel;
using SloperMobile.Listeners;

[assembly: ResolutionGroupName("new")]
[assembly: ExportEffect(typeof(TestApp.Droid.TouchEffect), "new1")]
namespace TestApp.Droid
{
    public class TouchEffect : PlatformEffect
    {
        private GestureDetector _detector;
        Android.Views.View view;
        Element formsElement;
        SloperMobile.ViewModel.TouchEffect pclTouchEffect;
        bool capture;
        Func<double, double> fromPixels;
        int[] twoIntArray = new int[2];
        private PinchGestureUpdatedEventArgs pinchGestureUpdatedEventArgs;

        static Dictionary<Android.Views.View, TouchEffect> viewDictionary =
            new Dictionary<Android.Views.View, TouchEffect>();

        static Dictionary<int, TouchEffect> idToEffectDictionary =
            new Dictionary<int, TouchEffect>();

        private FancyGestureListener _listener;

        public float Distance(MotionEvent eventArgs, int first, int second)
        {
            if (eventArgs.PointerCount == 2)
            {
                float x = eventArgs.GetX(first) - eventArgs.GetX(second);
                float y = eventArgs.GetY(first) - eventArgs.GetY(second);

                return (float)Math.Sqrt(x * x + y * y);
            }
            else
            {
                return 0;
            }
        }
        private float mPrimStartTouchEventX = -1;
        private float mPrimStartTouchEventY = -1;
        private float mSecStartTouchEventX = -1;
        private float mSecStartTouchEventY = -1;
        private float mPrimSecStartTouchDistance = 0;
        private double mViewScaledTouchSlop;


        private bool IsPinchGesture(MotionEvent eventArgs)
        {
            if (eventArgs.PointerCount == 2)
            {
                float distanceCurrent = Distance(eventArgs, 0, 1);
                float diffPrimX = mPrimStartTouchEventX - eventArgs.GetX(0);
                float diffPrimY = mPrimStartTouchEventY - eventArgs.GetY(0);
                float diffSecX = mSecStartTouchEventX - eventArgs.GetX(1);
                float diffSecY = mSecStartTouchEventY - eventArgs.GetY(1);

                if (// if the distance between the two fingers has increased past
                    // our threshold
                    Math.Abs(distanceCurrent - mPrimSecStartTouchDistance) > mViewScaledTouchSlop
                    // and the fingers are moving in opposing directions
                    && (diffPrimY * diffSecY) <= 0
                    && (diffPrimX * diffSecX) <= 0)
                {
                    return true;
                }
            }

            return false;
        }
        private bool isScaling;
        private int PrevMoveX;
        private int PrevMoveY;
        private float PrevDistance;

        private float totalPinchScale = 1f;

        double startPinchAnchorX = 0;
        double startPinchAnchorY = 0;

        private SKMatrix _m = SKMatrix.MakeIdentity();
        private SKMatrix _currentTransformM = SKMatrix.MakeIdentity();
        private SKMatrix _startPanM = SKMatrix.MakeIdentity();
        private SKMatrix _startPinchM = SKMatrix.MakeIdentity();


        public bool OnTouchEvent(MotionEvent e, int pointerIndex, View senderView, int id, Point screenPointerCoords)
        {
            int iTouchCount = e.PointerCount;

            if (e.Action == MotionEventActions.Down || e.Action == MotionEventActions.Pointer1Down ||
                e.Action == MotionEventActions.Pointer2Down)
            {
                if (iTouchCount >= 2)
                {
                    float Max_X = Math.Max(e.GetX(0), e.GetX(1));
                    float Min_X = Math.Min(e.GetX(0), e.GetX(1));
                    float Max_Y = Math.Max(e.GetY(0), e.GetY(1));
                    float Min_Y = Math.Min(e.GetY(0), e.GetY(1));
                    startPinchAnchorX = Min_X + (Max_X - Min_X);
                    startPinchAnchorY = Min_Y + (Max_Y - Min_Y);

                    _startPinchM = _m;
                    totalPinchScale = 1f;

                    float distance = Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                    PrevDistance = distance;

                    senderView.GetLocationOnScreen(twoIntArray);

                    screenPointerCoords = new Point(twoIntArray[0] + e.GetX(pointerIndex),
                        twoIntArray[1] + e.GetY(pointerIndex));
                    pinchGestureUpdatedEventArgs = new PinchGestureUpdatedEventArgs(GestureStatus.Started, distance, new Point(Max_Y, Max_X));

                    FireEventForPinch(this, id, TouchActionType.Moved, screenPointerCoords, true);
                    isScaling = true;
                    return true;
                }
                else
                {
                    _startPanM = _m;
                    PrevMoveX = (int)e.GetX();
                    PrevMoveY = (int)e.GetY();
                }
            }
            else if (e.Action == MotionEventActions.Move)
            {
                if (isScaling)
                {
                    try
                    {
                        float Max_X = Math.Max(e.GetX(0), e.GetX(1));
                        float Min_X = Math.Min(e.GetX(0), e.GetX(1));
                        float Max_Y = Math.Max(e.GetY(0), e.GetY(1));
                        float Min_Y = Math.Min(e.GetY(0), e.GetY(1));
                        startPinchAnchorX = Min_X + (Max_X - Min_X);
                        startPinchAnchorY = Min_Y + (Max_Y - Min_Y);
                        float dist = Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                        float scale = dist / PrevDistance;
                        PrevDistance = dist;

                        totalPinchScale *= (float)scale;


                        senderView.GetLocationOnScreen(twoIntArray);

                        screenPointerCoords = new Point(twoIntArray[0] + e.GetX(pointerIndex),
                            twoIntArray[1] + e.GetY(pointerIndex));
                        pinchGestureUpdatedEventArgs = new PinchGestureUpdatedEventArgs(GestureStatus.Running, totalPinchScale,
                            new Point(startPinchAnchorX, startPinchAnchorY));
                        FireEventForPinch(this, id, TouchActionType.Moved, screenPointerCoords, true);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
                else
                {
                    float canvasTotalX = (float)-(PrevMoveX - (int)e.GetX());
                    float canvasTotalY = (float)-(PrevMoveY - (int)e.GetY());

                    //pinchGestureUpdatedEventArgs = new PinchGestureUpdatedEventArgs(GestureStatus.Running, totalPinchScale,
                    //	new Point(canvasTotalX, canvasTotalY));
                }

                if (iTouchCount >= 2)
                {
                    return true;
                }
            }
            else if (e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Pointer1Up ||
                     e.Action == MotionEventActions.Pointer2Up)
            {
                if (isScaling)
                {
                    _startPinchM = SKMatrix.MakeIdentity();
                    startPinchAnchorX = 0;
                    startPinchAnchorY = 0;
                    totalPinchScale = 1f;
                    try
                    {

                        float Max_X = Math.Max(e.GetX(0), e.GetX(1));
                        float Min_X = Math.Min(e.GetX(0), e.GetX(1));
                        float Max_Y = Math.Max(e.GetY(0), e.GetY(1));
                        float Min_Y = Math.Min(e.GetY(0), e.GetY(1));
                        float dist = Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                        float scale = dist / PrevDistance;
                        PrevDistance = dist;

                        senderView.GetLocationOnScreen(twoIntArray);

                        screenPointerCoords = new Point(twoIntArray[0] + e.GetX(pointerIndex),
                            twoIntArray[1] + e.GetY(pointerIndex));
                        pinchGestureUpdatedEventArgs =
                            new PinchGestureUpdatedEventArgs(GestureStatus.Completed, dist, new Point((Min_X), (Max_Y)));

                        FireEventForPinch(this, id, TouchActionType.Moved, screenPointerCoords, true);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        senderView.GetLocationOnScreen(twoIntArray);

                        screenPointerCoords = new Point(twoIntArray[0] + e.GetX(pointerIndex),
                            twoIntArray[1] + e.GetY(pointerIndex));
                        pinchGestureUpdatedEventArgs =
                            new PinchGestureUpdatedEventArgs(GestureStatus.Completed);

                        FireEventForPinch(this, id, TouchActionType.Moved, screenPointerCoords, true);
                    }
                    return true;
                }
                else
                {
                    _startPanM = SKMatrix.MakeIdentity();
                }

                isScaling = false;
                if (iTouchCount >= 2)
                {
                    return true;
                }
            }

            return false;
        }

        private float Distance(float fX0, float fX1, float fY0, float fY1)
        {
            float x = fX0 - fX1;
            float y = fY0 - fY1;
            return FloatMath.Sqrt(x * x + y * y);
        }

        protected override void OnAttached()
        {
            // Get the Android View corresponding to the Element that the effect is attached to
            view = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the PCL
            SloperMobile.ViewModel.TouchEffect touchEffect =
                (SloperMobile.ViewModel.TouchEffect)Element.Effects.
                    FirstOrDefault(e => e is SloperMobile.ViewModel.TouchEffect);
            try
            {

                if (touchEffect != null && view != null)
                {
                    viewDictionary.Add(view, this);

                    formsElement = Element;

                    pclTouchEffect = touchEffect;

                    // Save fromPixels function
                    fromPixels = view.Context.FromPixels;

                    // Set event handler on View
                    //_listener = new FancyGestureListener();
                    //_detector = new GestureDetector(_listener);
                    //view.ContextClick += views;
                    //view.Click += View_Click;
                    view.Touch += OnTouch;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void View_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void views(object sender, View.ContextClickEventArgs contextClickEventArgs)
        {
            throw new NotImplementedException();
        }

        private void GestureDetectorOnContextClick(object sender, GestureDetector.ContextClickEventArgs contextClickEventArgs)
        {
            throw new NotImplementedException();
        }

        protected override void OnDetached()
        {
            //if (viewDictionary.ContainsKey(view))
            //{
            //    viewDictionary.Remove(view);
            //    view.Touch -= OnTouch;
            //}
        }

        void OnTouch(object sender, Android.Views.View.TouchEventArgs args)
        {
            ViewConfiguration viewConfig = ViewConfiguration.Get(view.Context);
            mViewScaledTouchSlop = viewConfig.ScaledTouchSlop;
            // Two object common to all the events
            Android.Views.View senderView = sender as Android.Views.View;
            MotionEvent motionEvent = args.Event;

            // Get the pointer index
            int pointerIndex = motionEvent.ActionIndex;

            // Get the id that identifies a finger over the course of its progress
            int id = motionEvent.GetPointerId(pointerIndex);


            senderView.GetLocationOnScreen(twoIntArray);
            Point screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                  twoIntArray[1] + motionEvent.GetY(pointerIndex));

            if (OnTouchEvent(motionEvent, pointerIndex, senderView, id, screenPointerCoords))
            {
                return;
            }

            // Use ActionMasked here rather than Action to reduce the number of possibilities
            switch (args.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    FireEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);
                    try
                    {

                        idToEffectDictionary.Add(id, this);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    capture = pclTouchEffect.Capture;
                    break;

                case MotionEventActions.Move:
                    // Multiple Move events are bundled, so handle them in a loop
                    for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
                    {
                        id = motionEvent.GetPointerId(pointerIndex);

                        if (capture)
                        {
                            senderView.GetLocationOnScreen(twoIntArray);

                            screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                            twoIntArray[1] + motionEvent.GetY(pointerIndex));

                            FireEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
                        }
                        else
                        {
                            CheckForBoundaryHop(id, screenPointerCoords);

                            if (idToEffectDictionary[id] != null)
                            {
                                FireEvent(idToEffectDictionary[id], id, TouchActionType.Moved, screenPointerCoords, true);
                            }
                        }
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
                    }
                    else
                    {
                        CheckForBoundaryHop(id, screenPointerCoords);

                        if (idToEffectDictionary[id] != null)
                        {
                            FireEvent(idToEffectDictionary[id], id, TouchActionType.Released, screenPointerCoords, false);
                        }
                    }
                    idToEffectDictionary.Remove(id);
                    break;

                case MotionEventActions.Cancel:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
                    }
                    else
                    {
                        if (idToEffectDictionary[id] != null)
                        {
                            FireEvent(idToEffectDictionary[id], id, TouchActionType.Cancelled, screenPointerCoords, false);
                        }
                    }
                    idToEffectDictionary.Remove(id);
                    break;
            }
        }

        void CheckForBoundaryHop(int id, Point pointerLocation)
        {
            TouchEffect touchEffectHit = null;

            foreach (Android.Views.View view in viewDictionary.Keys)
            {
                // Get the view rectangle
                try
                {
                    view.GetLocationOnScreen(twoIntArray);
                }
                catch // System.ObjectDisposedException: Cannot access a disposed object.
                {
                    continue;
                }
                Rectangle viewRect = new Rectangle(twoIntArray[0], twoIntArray[1], view.Width, view.Height);

                if (viewRect.Contains(pointerLocation))
                {
                    touchEffectHit = viewDictionary[view];
                }
            }

            if (touchEffectHit != idToEffectDictionary[id])
            {
                if (idToEffectDictionary[id] != null)
                {
                    FireEvent(idToEffectDictionary[id], id, TouchActionType.Exited, pointerLocation, true);
                }
                if (touchEffectHit != null)
                {
                    FireEvent(touchEffectHit, id, TouchActionType.Entered, pointerLocation, true);
                }
                idToEffectDictionary[id] = touchEffectHit;
            }
        }

        void FireEvent(TouchEffect touchEffect, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
        {
            // Get the method to call for firing events
            Action<Element, TouchActionEventArgs> onTouchAction = touchEffect.pclTouchEffect.OnTouchAction;

            // Get the location of the pointer within the view
            touchEffect.view.GetLocationOnScreen(twoIntArray);
            double x = pointerLocation.X - twoIntArray[0];
            double y = pointerLocation.Y - twoIntArray[1];
            Point point = new Point(fromPixels(x), fromPixels(y));

            // Call the method
            onTouchAction(touchEffect.formsElement,
                new TouchActionEventArgs(id, actionType, point, isInContact));
        }

        void FireEventForPinch(TouchEffect touchEffect, int id, TouchActionType actionType, Point pointerLocation,
            bool isInContact)
        {

            Action<Element, PinchGestureUpdatedEventArgs> onTouchAction = pclTouchEffect.OnPinchAction;
            touchEffect.view.GetLocationOnScreen(twoIntArray);
            double x = pointerLocation.X - twoIntArray[0];
            double y = pointerLocation.Y - twoIntArray[1];
            Point point = new Point(fromPixels(x), fromPixels(y));
            // Call the method
            onTouchAction(formsElement, pinchGestureUpdatedEventArgs);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IntPtr Handle { get; }
        public bool OnDown(MotionEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            throw new NotImplementedException();
        }

        public void OnLongPress(MotionEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            throw new NotImplementedException();
        }

        public void OnShowPress(MotionEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            throw new NotImplementedException();
        }
    }

    sealed class InternalGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        private const int SwipeThresholdInPoints = 40;

        public Action<MotionEvent> TapAction { get; set; }
        public Action<MotionEvent> SwipeLeftAction { get; set; }
        public Action<MotionEvent> SwipeRightAction { get; set; }
        public Action<MotionEvent> SwipeTopAction { get; set; }
        public Action<MotionEvent> SwipeBottomAction { get; set; }

        public float Density { get; set; }

        public override bool OnSingleTapUp(MotionEvent e)
        {
            TapAction?.Invoke(e);
            return true;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            var dx = e2.RawX - e1.RawX;
            var dy = e2.RawY - e1.RawY;
            if (Math.Abs(dx) > SwipeThresholdInPoints * Density)
            {
                if (dx > 0)
                    SwipeRightAction?.Invoke(e2);
                else
                    SwipeLeftAction?.Invoke(e2);
            }
            else if (Math.Abs(dy) > SwipeThresholdInPoints * Density)
            {
                if (dy > 0)
                    SwipeBottomAction?.Invoke(e2);
                else
                    SwipeTopAction?.Invoke(e2);
            }
            return true;
        }
    }
}