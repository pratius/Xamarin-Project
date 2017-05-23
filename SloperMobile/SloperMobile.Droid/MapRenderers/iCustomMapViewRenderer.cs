//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.ComponentModel;
//using Android.Content;
//using Android.Gms.Maps;
//using Android.Gms.Maps.Model;
//using Android.Widget;
//using Xamarin.Forms;
//using Xamarin.Forms.Maps;
//using Xamarin.Forms.Maps.Android;
//using Java.IO;
//using Java.Net;
//using System.IO;
//using System.Threading.Tasks;
//using Android.Views;
//using Xamarin.Forms.Platform.Android;
//using Android.Graphics;
//using SloperMobile.UserControls.CustomMap;
//using SloperMobile.CustomControls;

//[assembly: ExportRenderer(typeof(SloperMobile.CustomControls.iCustomMap), typeof(SloperMobile.Droid.MapRenderers.iCustomMapViewRenderer))]

//namespace SloperMobile.Droid.MapRenderers
//{
//    public class iCustomMapViewRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
//    {
//        bool _isDrawnDone;
//        ImageView _pinImage = null;
//        iMapPinInfoCtrl xamInfoPanel = new iMapPinInfoCtrl();
//        GoogleMap _map;
//        IList<Pin> _mapPins;


//        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
//        {
//            base.OnElementChanged(e);
//            if (e.OldElement != null)
//            {
//                _map.InfoWindowClick -= Map_InfoWindowClick;
//            }

//            if (e.NewElement != null)
//            {
//                var formsMap = e.NewElement as iCustomMap;

//                _mapPins = formsMap.Pins;
//                ((MapView)Control).GetMapAsync(this);
//            }
//        }

//        void IOnMapReadyCallback.OnMapReady(GoogleMap googleMap)
//        {
//            InvokeOnMapReadyBaseClassHack(googleMap);
//            _map = googleMap;
//            _map.InfoWindowClick += Map_InfoWindowClick;

//            // required if you wish to handle info window and its content
//            _map.SetInfoWindowAdapter(this);
//        }


//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);

//            if (_isDrawnDone)
//                return;

//            if (e.PropertyName.Equals("VisibleRegion"))
//            {
//                _map.Clear();
//                _map.MarkerClick += HandleMarkerClick;
//                _map.InfoWindowClick += Map_InfoWindowClick;

//                foreach (var pin in _mapPins)
//                {
//                    var marker = new MarkerOptions();
//                    iCustomPin customPin = pin.BindingContext as iCustomPin;

//                    marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
//                    marker.SetTitle(pin.Label);

//                    if (!string.IsNullOrEmpty(pin.Address))
//                        marker.SetSnippet(pin.Address);
//                    else
//                        marker.SetSnippet("");

//                    //marker.InfoWindowAnchor(10f, 5f);

//                    BitmapDescriptor bmp = null;

//                    if (customPin != null && !string.IsNullOrEmpty(customPin.IconResource))
//                    {
//                        string resourceName = System.IO.Path.GetFileNameWithoutExtension(customPin.IconResource);
//                        int resourceId = Context.Resources.GetIdentifier(resourceName, "drawable", Context.PackageName);

//                        try
//                        {
//                            bmp = BitmapDescriptorFactory.FromResource(resourceId);
//                        }
//                        catch (Exception)
//                        {
//                            bmp = null;
//                        }

//                        if (bmp == null)
//                        {
//                            try
//                            {
//                                bmp = BitmapDescriptorFactory.FromResource(Resource.Drawable.placepin);
//                            }
//                            catch (Exception)
//                            {
//                                bmp = null;
//                            }
//                        }
//                    }

//                    if (bmp != null)
//                        marker.SetIcon(bmp);
//                    else
//                        marker.SetIcon(BitmapDescriptorFactory.DefaultMarker());

//                    _map.AddMarker(marker);
//                }

//                _isDrawnDone = true;
//            }
//        }



//        private void Map_InfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
//        {
//            var marker = e == null ? null : e.Marker;
//            iCustomMap myMap = this.Element as iCustomMap;

//            if (myMap == null || marker == null)
//                return;

//            iCustomPin customPin = myMap.GetPinAtPosition(new Position(marker.Position.Latitude, marker.Position.Longitude));

//            if (customPin != null)
//            {
//                marker.HideInfoWindow();
//                customPin.RaiseClickEvent();
//            }
//        }

//        void HandleMarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
//        {
//            var marker = e == null ? null : e.Marker;       //.P0;

//            if (marker == null || marker.IsInfoWindowShown)
//                return;

//            marker.ShowInfoWindow();

//            iCustomMap myMap = this.Element as iCustomMap;

//            if (myMap == null)
//                return;

//            Position currentPos = new Position(marker.Position.Latitude, marker.Position.Longitude);
//            Distance currentRadius = myMap.VisibleRegion.Radius;

//            // center the map on the clicked pushpin
//            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(currentPos, currentRadius));
//        }

//        protected override void OnLayout(bool changed, int l, int t, int r, int b)
//        {
//            base.OnLayout(changed, l, t, r, b);

//            //NOTIFY CHANGE
//            if (changed)
//            {
//                _isDrawnDone = false;
//            }
//        }

//        public Android.Views.View GetInfoContents(Marker marker)
//        {
//            // note: return null if you used the GetInfoWindow to return the entire info window

//            iCustomMap myMap = this.Element as iCustomMap;

//            if (myMap == null || marker == null)
//                return null;

//            iCustomPin customPin = myMap.GetPinAtPosition(new Position(marker.Position.Latitude, marker.Position.Longitude));

//            if (customPin == null)
//                return null;

//            double density = Resources.DisplayMetrics.Density;
//            double infoWidth = this.Control.Width * 0.70,
//                            infoHeight = 380.0 / 2.8 * density;
//            SamplePlace place = customPin.DataObject as SamplePlace;
//            int nTextLines = 1;
//            int wrapLength = (int)(infoWidth / density / 6.0);  // estimated max chars before text wraps

//            // count lines in the place's info text

//            if (place != null && !string.IsNullOrEmpty(place.MapPinText))
//            {
//                var linesArray = place.MapPinText.Split(new char[] { '\n' });
//                List<string> lines = linesArray == null ? null : linesArray.ToList(); ;

//                if (lines != null && lines.Count > 1)
//                {
//                    nTextLines = lines.Count;

//                    foreach (string s in lines)
//                    {
//                        // empiric: lines are mostly wrapped at 38 with our dimesnions
//                        if (s.Length >= wrapLength)
//                            nTextLines++;
//                    }
//                }
//                else if (place.MapPinText.Length >= wrapLength)
//                    nTextLines++;

//                //nTextLines	= place.MapPinText.Count(c => c == '\n');
//            }

//            infoHeight += nTextLines * 12 * density;

//            xamInfoPanel.BindingContext = place;

//            ViewGroup viewGrp = DroidXFUtilities.ConvertFormsToNative(xamInfoPanel.Content, new Rectangle(0, 0, infoWidth, infoHeight));

//            // transform the native control into a bitmap
//            Bitmap bmp = DroidXFUtilities.ViewGroupToBitmap(viewGrp, this.Context, (int)infoWidth, (int)infoHeight, density, true);

//            if (_pinImage == null)
//                _pinImage = new ImageView(this.Context);

//            _pinImage.SetImageBitmap(bmp);

//            return _pinImage;
//        }


//        public Android.Views.View GetInfoWindow(Marker marker)
//        {
//            // return the entire info window or:
//            // return null for the GetInfoContents method to be used
//            return null;
//        }
//        private void InvokeOnMapReadyBaseClassHack(GoogleMap googleMap)
//        {
//            System.Reflection.MethodInfo onMapReadyMethodInfo = null;

//            Type baseType = typeof(MapRenderer);
//            foreach (var currentMethod in baseType.GetMethods(System.Reflection.BindingFlags.NonPublic |
//                                                             System.Reflection.BindingFlags.Instance |
//                                                              System.Reflection.BindingFlags.DeclaredOnly))
//            {

//                if (currentMethod.IsFinal && currentMethod.IsPrivate)
//                {
//                    if (string.Equals(currentMethod.Name, "OnMapReady", StringComparison.Ordinal))
//                    {
//                        onMapReadyMethodInfo = currentMethod;

//                        break;
//                    }

//                    if (currentMethod.Name.EndsWith(".OnMapReady", StringComparison.Ordinal))
//                    {
//                        onMapReadyMethodInfo = currentMethod;

//                        break;
//                    }
//                }
//            }

//            if (onMapReadyMethodInfo != null)
//            {
//                onMapReadyMethodInfo.Invoke(this, new[] { googleMap });
//            }
//        }
//    }
//}
