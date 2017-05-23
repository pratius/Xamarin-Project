//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Foundation;
//using UIKit;
//using Xamarin.Forms;
//using Xamarin.Forms.Maps.iOS;
//using MapKit;
//using Xamarin.Forms.Maps;
//using CoreGraphics;
//using Xamarin.Forms.Platform.iOS;
//using SloperMobile.CustomControls;
//using SloperMobile.iOS.MapRenderers;

//[assembly: ExportRenderer(typeof(iCustomMap), typeof(iCustomMapRenderer))]
//namespace SloperMobile.iOS.MapRenderers
//{
//    public partial class iCustomMapRenderer : MapRenderer
//    {
//        UIView customPinView;
//        UIView placeImageView;
//        IList<Pin> _pins;

//        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
//        {
//            base.OnElementChanged(e);

//            if (e.OldElement != null)
//            {
//                var nativeMap = Control as MKMapView;

//                nativeMap.GetViewForAnnotation = null;
//                nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
//                nativeMap.DidSelectAnnotationView -= OnAfterSelectAnnotationView;
//                nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
//            }

//            if (e.NewElement != null)
//            {
//                var formsMap = e.NewElement as iCustomMap;
//                var nativeMap = Control as MKMapView;

//                _pins = formsMap.Pins;

//                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
//                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
//                nativeMap.DidSelectAnnotationView += OnAfterSelectAnnotationView;
//                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
//            }
//        }

//        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
//        {
//            MKAnnotationView annotationView = null;

//            if (annotation is MKUserLocation)
//                return null;

//            var anno = annotation as MKPointAnnotation;
//            var customPin = GetCustomPin(anno);
//            SamplePlace place = customPin == null ? null : customPin.DataObject as SamplePlace;

//            if (customPin == null || place == null)
//            {
//                return null;
//                // throw new Exception ("Custom pin not found");
//            }

//            string annotationId = customPin.MapPinPosition.Latitude.ToString() + "," + customPin.MapPinPosition.Longitude.ToString();
//            annotationView = mapView.DequeueReusableAnnotation(annotationId);

//            if (annotationView == null)
//            {
//                annotationView = new CustomMKPinAnnotationView(customPin, annotation, annotationId);


//                if (place.IsPink)
//                    annotationView.Image = UIImage.FromFile("pin32pink.png");
//                else
//                    annotationView.Image = UIImage.FromFile("placePin.png");

//                annotationView.CalloutOffset = new CGPoint(0, 0);
//                //annotationView.LeftCalloutAccessoryView		= new UIImageView (UIImage.FromFile ("monkey.png"));
//                annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
//            }

//            annotationView.CanShowCallout = true;

//            return annotationView;
//        }

//        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
//        {
//            RaiseSelectEvent(e.View);
//        }

//        void RaiseSelectEvent(MKAnnotationView view)
//        {
//            var customView = view as CustomMKPinAnnotationView;
//            iCustomPin customPin = customView == null ? null : customView.MapCustomPin;

//            if (customPin != null)
//                customPin.RaiseClickEvent();
//        }


//        void OnAfterSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
//        {
//            var customView = e.View as CustomMKPinAnnotationView;
//            var customPin = this.Element as iCustomMap;
//            var place = customView == null || customView.MapCustomPin == null ? null : customView.MapCustomPin.DataObject as SamplePlace;

//            if (place == null)
//                return;

//            customPinView = new UIView();
//            /*
//            placeImageView = new UIView();
//            UIImage img = new UIImage(place.ImageFile);
//            UIImageView imgCtrl = new UIImageView(new CGRect(40, 10, 84, 84));

//            imgCtrl.Image = img;

//            placeImageView.AddSubview(imgCtrl);
//            customPinView.AddSubview(placeImageView);
//            */
//            customPinView.Frame = new CGRect(0, 0, 200, 84);
//            customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 76)); // 75));

//            e.View.AddSubview(customPinView);
//        }

//        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
//        {
//            CustomMKPinAnnotationView view = e == null ? null : e.View as CustomMKPinAnnotationView;

//            if (customPinView == null)
//                return;

//            if (!view.Selected)
//            {
//                customPinView.RemoveFromSuperview();
//                customPinView.Dispose();
//                customPinView = null;
//            }
//        }

//        iCustomPin GetCustomPin(MKPointAnnotation annotation)
//        {
//            var formsMap = this.Element as iCustomMap;
//            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);

//            if (formsMap == null)
//                return null;

//            return formsMap.GetPinAtPosition(position);
//        }
//    }
//}