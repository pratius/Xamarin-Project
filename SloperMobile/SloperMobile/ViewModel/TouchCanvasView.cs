using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using UIKit;

namespace SloperMobile.ViewModel
{
    public class TouchCanvasView : SKCanvasView, ITouchCanvasViewController
    {
        public event Action<SKPoint> Touched;
        public event Action<UIPinchGestureRecognizer> Pinch;

        public virtual void OnTouch(SKPoint point)
        {
            Touched?.Invoke(point);
        }
        public virtual void OnPinchUpdated(UIPinchGestureRecognizer recognizer)
        {
            Pinch?.Invoke(recognizer);
        }
    }
    public interface ITouchCanvasViewController : IViewController
    {
        void OnTouch(SKPoint point);
        void OnPinchUpdated(UIPinchGestureRecognizer recognizer);
    }
}
