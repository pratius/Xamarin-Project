using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using SloperMobile.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using SloperMobile.ViewModel;

[assembly: ResolutionGroupName("SloperSoftware")]
[assembly: ExportEffect(typeof(TapGestureWithPointEffect), "TapGestureWithPointEffect")]

namespace SloperMobile.iOS
{
    internal class TapGestureWithPointEffect : PlatformEffect
    {
        private readonly UITapGestureRecognizer tapDetector;
        private Command<Point> tapWithPositionCommand;

        public TapGestureWithPointEffect()
        {
            tapDetector = CreateTapRecognizer(() => tapWithPositionCommand);
            ;
        }

        private UITapGestureRecognizer CreateTapRecognizer(Func<Command<Point>> getCommand)
        {
            return new UITapGestureRecognizer(() =>
            {
                var handler = getCommand();
                if (handler != null)
                {
                    var control = Control ?? Container;
                    var tapPoint = tapDetector.LocationInView(control);
                    var point = new Point(tapPoint.X, tapPoint.Y);

                    if (handler.CanExecute(point) == true)
                        handler.Execute(point);
                }
            })
            {
                Enabled = false,
                ShouldRecognizeSimultaneously = (recognizer, gestureRecognizer) => true,
            };
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            tapWithPositionCommand = Gesture.GetCommand(Element);
        }

        protected override void OnAttached()
        {
            var control = Control ?? Container;

            control.AddGestureRecognizer(tapDetector);
            tapDetector.Enabled = true;

            OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
        }

        protected override void OnDetached()
        {
            var control = Control ?? Container;
            tapDetector.Enabled = false;
            control.RemoveGestureRecognizer(tapDetector);
        }
    }
}