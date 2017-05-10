using MapKit;
using SloperMobile.CustomControls;
using Xamarin.Forms.Maps;

namespace SloperMobile.iOS.MapRenderers
{
    public class CustomMKPinAnnotationView : MKPinAnnotationView
    {
        public string Id { get; set; }

        //public string Url { get; set; }
        public iCustomPin MapCustomPin { get; set; }

        public CustomMKPinAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
        {
        }

        public CustomMKPinAnnotationView(iCustomPin customPin, IMKAnnotation annotation, string id) : base(annotation, id)
        {
            MapCustomPin = customPin;
        }

    }
}