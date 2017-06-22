using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class TouchCanvasView : SKCanvasView, ITouchCanvasViewController
    {
        public event Action<SKPoint> Touched;
        public event Action<UIPinchGestureRecognizer> Pinch;
        //public event Action<MonoTouch.UIKit.UIPinchGestureRecognizer> AndroidPinch;

        public virtual void OnTouch(SKPoint point)
        {
            Touched?.Invoke(point);
        }
        public virtual void OnPinchUpdated(UIPinchGestureRecognizer recognizer)
        {
            Pinch?.Invoke(recognizer);
        }
        //public virtual void OnAndroidPinchUpdated(MonoTouch.UIKit.UIPinchGestureRecognizer _recognizer)
        //{
        //    AndroidPinch?.Invoke(_recognizer);
        //}
    }
    public interface ITouchCanvasViewController : IViewController
    {
        void OnTouch(SKPoint point);
        void OnPinchUpdated(UIPinchGestureRecognizer recognizer);
        //void OnAndroidPinchUpdated(MonoTouch.UIKit.UIPinchGestureRecognizer recognizer);
    }
}
