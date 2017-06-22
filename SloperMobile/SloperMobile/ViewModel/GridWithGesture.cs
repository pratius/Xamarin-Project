using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class GridWithGesture : Grid
    {
        public event EventHandler<PinchGestureUpdatedEventArgs> PinchAction;

        public void OnPinchAction(Element element, PinchGestureUpdatedEventArgs args)
        {
            PinchAction?.Invoke(element, args);
        }
    }
}
