using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.CustomControls;
using SloperMobile.Model;
using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Platform.Device;

namespace SloperMobile.Views
{
    public partial class TopoMapRoutesPage : ContentPage
    {
        private const string BoxYellowColor = "#b49800";
        private const int BoxTextFontSize = 11;
        private ObservableCollection<imgData> _imgData;
        public List<int> _diamondclickroute = new List<int>();
        public ObservableCollection<imgData> ImageDataList
        {
            get { return _imgData; }
            set { _imgData = value; OnPropertyChanged(); }
        }
        Dictionary<long, FingerPaintPolyline> inProgressPolylines = new Dictionary<long, FingerPaintPolyline>();
        public MapListModel _CurrentSector { get; set; }
        double currentScale = 1, startScale = 1, xOffset = 0, yOffset = 0;
        //float _currentScale = 1.0f;
        private List<Tuple<points, int>> _points = new List<Tuple<points, int>>();
        private List<Tuple<points, int>> _newPoints = new List<Tuple<points, int>>();
        public List<TopoImageResponse> topoimg = null;
        public List<Tuple<string, string>> _bucket = new List<Tuple<string, string>>();
        private ViewModel.TopoMapRoutesViewModel TopoMapRouteVM;
        public string staticAnnotationData = "[{\"AnnotationName\": \"Open Text (left)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Open Text (right)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (white)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Anchor (black)\",\"AnnotationType\": 0,\"ImageName\":\"\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Line\",\"AnnotationType\": 0,\"ImageName\":\"sample-line.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Route Badge\",\"AnnotationType\": 1,\"ImageName\":\"sample-route-badge.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\":\"Belay Point\",\"AnnotationType\": 2,\"ImageName\":\"sample-belay.png\",\"XCentreOffset\": -15,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Left\",\"AnnotationType\": 3,\"ImageName\":\"sample-lower-off-left.png\",\"XCentreOffset\": -22,\"YCentreOffset\":-15},{\"AnnotationName\": \"Lower-off Right\",\"AnnotationType\": 4,\"ImageName\":\"sample-lower-off-right.png\",\"XCentreOffset\": -8,\"YCentreOffset\":-15},{\"AnnotationName\": \"Grade Label (left)\",\"AnnotationType\": 5,\"ImageName\":\"sample-grade-label-left.png\",\"XCentreOffset\": -5,\"YCentreOffset\":0},{\"AnnotationName\": \"Grade Label (right)\",\"AnnotationType\": 6,\"ImageName\":\"sample-grade-label-right.png\",\"XCentreOffset\": 5,\"YCentreOffset\":0},{\"AnnotationName\": \"Line Break\",\"AnnotationType\": 7,\"ImageName\":\"sample-route-line-break.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"Cross Point\",\"AnnotationType\": 8,\"ImageName\":\"x-mark-32.png\",\"XCentreOffset\": -50,\"YCentreOffset\":-50},{\"AnnotationName\": \"Text Add\",\"AnnotationType\": 9,\"ImageName\":\"tl-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextAdd\",\"AnnotationType\": 10,\"ImageName\":\"tr-icon.png\",\"XCentreOffset\": -11,\"YCentreOffset\":-11},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 11,\"ImageName\":\"tlcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"TextRemove\",\"AnnotationType\": 12,\"ImageName\":\"trcross-icon.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveLeft\",\"AnnotationType\": 13,\"ImageName\":\"move_left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"MoveRight\",\"AnnotationType\": 14,\"ImageName\":\"move_right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"icon-arrow\",\"AnnotationType\": 15,\"ImageName\":\"icon-arrow.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"cross dark black\",\"AnnotationType\": 16,\"ImageName\":\"cross_black.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-left\",\"AnnotationType\": 17,\"ImageName\":\"sample-lower-off-black-left.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0},{\"AnnotationName\": \"sample-lower-off-black-right\",\"AnnotationType\": 18,\"ImageName\":\"sample-lower-off-black-right.png\",\"XCentreOffset\": 0,\"YCentreOffset\":0}]";
        string listData = string.Empty, _strimg64 = string.Empty;
        Double height, newHeight, globalHeight, globalWidth;
        int? _routeId = 0, _newRouteId = 0;
        float ratio;
        int diamondsCount;
        public bool isDiamondClick = false, isDiamondSingleClick = false;

        private readonly IDevice device;

        //Is used to eliminate usage of Canvas. Need to figure out why did canvas is not stoping to draw itself
        private int hasBeingDrawen, hasBeingRedrawing = 0;

        public TopoMapRoutesPage(MapListModel CurrentSector, string _lstData, int routeId)
        {
            try
            {
                InitializeComponent();
                device = XLabs.Ioc.Resolver.Resolve<IDevice>();
                _CurrentSector = CurrentSector;
                listData = _lstData;
                _routeId = routeId;
                _newRouteId = _routeId;
                topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(listData);
                NavigationPage.SetHasNavigationBar(this, false);
                Title = CurrentSector.SectorName;
                TopoMapRouteVM = new ViewModel.TopoMapRoutesViewModel(CurrentSector, Navigation, listData);
                TopoMapRouteVM.PointTapped += OnPointTapped;
                BindingContext = TopoMapRouteVM;
                // load the scenic shot if there are no topos available
                if (listData == string.Empty)
                {
                    TopoMapRouteVM.OnConditionNavigation = OnPageNavigation;
                    TopoMapRouteVM.LoadRouteData(routeId, listData);
                    TopoMapRouteVM.DisplayRoutePopupLg = true;                   
                    this.BackgroundImage = "scenic_shot_portrait";
                }
                // otherwise load the topos
                else
                {
                    TopoMapRouteVM.OnConditionNavigation = OnPageNavigation;
                    TopoMapRouteVM.IsRunningTasks = true;
                    if (routeId > 0)
                    {
                        TopoMapRouteVM.LoadRouteData(routeId, listData);
                        TopoMapRouteVM.DisplayRoutePopupSm = true;
                    }
                }

                //var deviceHeight = device.Display.Height - (1.7 * FooterUC.Height * device.Display.Scale) - (BackHeaderUC.Height * device.Display.Scale);
                //ratio = (float)deviceHeight / float.Parse(topoimg[0].image.height);
                //height = (int)(int.Parse(topoimg[0].image.height) * ratio);// - (1.5 * FooterUC.Height * device.Display.Scale) - (BackHeaderUC.Height * device.Display.Scale);
                //ratio = (float)height / float.Parse(topoimg[0].image.height);
                //globalHeight = height;
                //globalWidth = double.Parse(topoimg[0].image.width) * ratio;

                //AndroidAbsoluteLayout.HeightRequest = height / device.Display.Scale;
                //AndroidAbsoluteLayout.WidthRequest = globalWidth / device.Display.Scale;
                //iOSdAbsoluteLayout.HeightRequest = height / device.Display.Scale;
                //iOSdAbsoluteLayout.WidthRequest = globalWidth / device.Display.Scale;

            }
            catch(Exception exception)
            {
                TopoMapRouteVM.IsRunningTasks = false;
                throw;
            }

            TopoMapRouteVM.IsRunningTasks = false;
        }

        //TODO: Remove later - it's depricated now
        private void OnPointTapped(PointWithId point)
        {
            var nPooint = ConvertToPixel(new Point(point.X, point.Y));
            var routeId = _points?.FirstOrDefault(item =>
            {
                return ((Math.Round(Convert.ToDouble(item.Item1.X) + 7) >= Math.Round(nPooint.X)
                        && Math.Round(Convert.ToDouble(item.Item1.X) - 7) <= Math.Round(nPooint.X))
                        && (Math.Round(Convert.ToDouble(item.Item1.Y) + 5) >= Math.Round(nPooint.Y)
                        && Math.Round(Convert.ToDouble(item.Item1.Y) - 20) <= Math.Round(nPooint.Y)));
            })?.Item2;

            if (routeId == null)
            {
                return;
            }

            ShowRoute(routeId);
        }

        private void ShowRoute(int? routeId)
        {
            if (Device.OS == TargetPlatform.iOS)
                skCanvasiOS.InvalidateSurface();

            TopoMapRouteVM.LoadRouteData(routeId, listData);
            TopoMapRouteVM.DisplayRoutePopupSm = true;
            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            height = device.Display.Height;
            newHeight = GetHeight(height);
            _bucket.Clear();
            _routeId = routeId;
            _newRouteId = routeId;
            _diamondclickroute.Clear();
            isDiamondSingleClick = true;
            Redraw();
        }

        private async void OnPageNavigation(object obj)
        {
            await Navigation.PushAsync(new AscentProcessPage(Convert.ToString(obj), _CurrentSector));
        }

        private async Task OnScroll(object obj)
        {
            if (obj == "0")
            {
                await Task.Yield();
               // await scrollView.ScrollToAsync(0, 0, false);
            }
            else
            {
                double val = (Convert.ToDouble(obj) + 100);
                await Task.Yield();
               // await scrollView.ScrollToAsync(val, 0, false);
            }
        }
        

        private void OnPaintSample(object sender, SKPaintSurfaceEventArgs e)
        {
            _points.Clear();

            try
            {
                MainDrawing(e);                
            }
            catch
            {
                throw;
            }
        }

        private void MainDrawing(SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear();
            if (string.IsNullOrEmpty(topoimg[0].image.data))
            {
                return;
            }

            //code to draw image
            string strimg64 = topoimg[0].image.data.Split(',')[1];
            if (!string.IsNullOrEmpty(strimg64))
            {
                byte[] imageBytes = Convert.FromBase64String(strimg64);
                using (var fileStream = new MemoryStream(imageBytes))
                {
                    ZoomableScrollView parent;
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        parent = skCanvasAndroid.Parent.Parent as ZoomableScrollView;
                    }
                    else
                    {
                        parent = skCanvasiOS.Parent.Parent as ZoomableScrollView;
                    }

                    if (hasBeingDrawen < 3)
                    {
                        var deviceHeight = device.Display.Height - ((1.9 * FooterUC.Height * device.Display.Scale) + (BackHeaderUC.Height * device.Display.Scale) );
                        ratio = (float)deviceHeight / float.Parse(topoimg[0].image.height);
                        height = (int)(int.Parse(topoimg[0].image.height) * ratio);// - (1.5 * FooterUC.Height * device.Display.Scale) - (BackHeaderUC.Height * device.Display.Scale);
                       // ratio = (float)height / float.Parse(topoimg[0].image.height);
                        globalHeight = height;
                        globalWidth = double.Parse(topoimg[0].image.width) * ratio;

                        AndroidAbsoluteLayout.HeightRequest = height / device.Display.Scale;
                        AndroidAbsoluteLayout.WidthRequest = globalWidth / device.Display.Scale;
                        iOSdAbsoluteLayout.HeightRequest = height / device.Display.Scale;
                        iOSdAbsoluteLayout.WidthRequest = globalWidth / device.Display.Scale;
                    }

                    // decode the bitmap from the stream
                    using (var stream = new SKManagedStream(fileStream))
                    using (var bitmap = SKBitmap.Decode(stream))
                    using (var paint = new SKPaint())
                    {
                        //skCanvasAndroid.TranslationY = 40 * parent.ScaleFactor;
                        canvas.DrawBitmap(bitmap, SKRect.Create((float)(globalWidth), (float)(height)), paint);
                        // canvas.DrawBitmap(bitmap, SKRect.Create((float)(AndroidAbsoluteLayout.WidthRequest * device.Display.Scale * parent.ScaleFactor), (float)(AndroidAbsoluteLayout.HeightRequest * device.Display.Scale * parent.ScaleFactor)), paint);
                       // AndroidAbsoluteLayout.HeightRequest = AndroidAbsoluteLayout.HeightRequest * parent.ScaleFactor;
                       // AndroidAbsoluteLayout.WidthRequest = AndroidAbsoluteLayout.WidthRequest * parent.ScaleFactor;
                       // ratio = (float)(AndroidAbsoluteLayout.HeightRequest * device.Display.Scale) / float.Parse(topoimg[0].image.height);

                        if (parent.IsScalingDown || parent.IsScalingUp)
                        {
                            try
                            {
                                for (int i = 0; i < diamondsCount; i++)
                                {
                                    AndroidAbsoluteLayout.Children.RemoveAt(i + 1);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }

            //code to draw line
            using (new SKAutoCanvasRestore(canvas, true))
            {
                //DrawLine(canvas, _routeId, ratio, (int)(AndroidAbsoluteLayout.HeightRequest * device.Display.Scale), (int)(AndroidAbsoluteLayout.WidthRequest * device.Display.Scale));
                DrawLine(canvas, _routeId, ratio, Convert.ToInt32(globalHeight), Convert.ToInt32(globalWidth));
            }

            hasBeingDrawen++;
        }


        private void Redraw()
        {
            // _newPoints.Clear();
            int height = 0;
            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            var topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(listData);
            if (topoimg != null)
            {
                var deviceHeight = device.Display.Height - (FooterUC.Height) - (BackHeaderUC.Height * device.Display.Scale);
                ratio = (float)deviceHeight / float.Parse(topoimg[0].image.height);
                height = (int)(int.Parse(topoimg[0].image.height) * ratio);// - (1.5 * FooterUC.Height * device.Display.Scale) - (BackHeaderUC.Height * device.Display.Scale);
                ratio = (float)height / float.Parse(topoimg[0].image.height);
                globalHeight = height;
                float width = float.Parse(topoimg[0].image.width) * ratio;

                if (Device.OS == TargetPlatform.Android)
                {
                    try
                    {
                        using (var surface = SKSurface.Create(Convert.ToInt32(width), height, SKColorType.Rgb565, SKAlphaType.Premul))
                        {
                            SKCanvas canvas = surface.Canvas;
                            //code to draw image
                            if (!string.IsNullOrEmpty(topoimg[0].image.data))
                            {
                                string strimg64 = topoimg[0].image.data.Split(',')[1];
                                if (!string.IsNullOrEmpty(strimg64))
                                {
                                    byte[] imageBytes = Convert.FromBase64String(strimg64);
                                    Stream fileStream = new MemoryStream(imageBytes);

                                    // clear the canvas / fill with white
                                    canvas.DrawColor(SKColors.White);

                                    // decode the bitmap from the stream
                                    using (var stream = new SKManagedStream(fileStream))
                                    using (var bitmap = SKBitmap.Decode(stream))
                                    using (var paint = new SKPaint())
                                    {
                                        canvas.DrawBitmap(bitmap, SKRect.Create(width, height), paint);
                                    }
                                }
                            }
                            //code to draw line
                            using (new SKAutoCanvasRestore(canvas, true))
                            {
                                ReDrawLine(canvas, _routeId, ratio, height, Convert.ToInt32(width));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (Device.OS == TargetPlatform.iOS)
                {
                    if (topoimg != null)
                    {
                        try
                        {
                            using (var surface = SKSurface.Create(Convert.ToInt32(width), height, SKColorType.Rgb565, SKAlphaType.Premul))
                            {
                                SKCanvas canvas = surface.Canvas;
                                //code to draw image
                                if (!string.IsNullOrEmpty(topoimg[0].image.data))
                                {
                                    string strimg64 = topoimg[0].image.data.Split(',')[1];
                                    if (!string.IsNullOrEmpty(strimg64))
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(strimg64);
                                        Stream fileStream = new MemoryStream(imageBytes);

                                        // clear the canvas / fill with white
                                        canvas.DrawColor(SKColors.White);

                                        // decode the bitmap from the stream
                                        using (var stream = new SKManagedStream(fileStream))
                                        using (var bitmap = SKBitmap.Decode(stream))
                                        using (var paint = new SKPaint())
                                        {
                                            canvas.DrawBitmap(bitmap, SKRect.Create(width, height), paint);
                                        }
                                    }
                                }

                                //code to draw line
                                using (new SKAutoCanvasRestore(canvas, true))
                                {
                                    ReDrawLine(canvas, _routeId, ratio, height, Convert.ToInt32(width));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        public void DrawLine(SKCanvas _skCanvas, int? route, float ratio, int _height, int _width)
        {
            var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
            float ptx1 = 0, ptx2 = 0, pty1 = 0, pty2 = 0;
            var topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(listData);

            using (var path = new SKPath())
            {
                foreach (var item in topoimg[0].drawing)
                {
                    diamondsCount += item.line.points.Where(line => line.type == "1").Count();
                }
              
                for (int j = 0; j < topoimg[0].drawing.Count; j++)
                {
                    if (_routeId == 0)
                    {
                        for (int i = 0; i < topoimg[0].drawing[j].line.points.Count; i++)
                        {
                            ptx1 = float.Parse(topoimg[0].drawing[j].line.points[i].x) * ratio;
                            pty1 = float.Parse(topoimg[0].drawing[j].line.points[i].y) * ratio;
                            if (i != (topoimg[0].drawing[j].line.points.Count - 1))
                            {
                                ptx2 = float.Parse(topoimg[0].drawing[j].line.points[i + 1].x) * ratio;
                                pty2 = float.Parse(topoimg[0].drawing[j].line.points[i + 1].y) * ratio;
                            }
                            SKColor _color = HexToColor(topoimg[0].drawing[j].line.style.color);
                            if (Device.OS == TargetPlatform.Android)
                            {
                                SKPaint thinLinePaint = new SKPaint
                                {
                                    //Style = SKPaintStyle.Stroke,                                    
                                    Color = _color,
                                    StrokeWidth = 5,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 30, 15, }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, thinLinePaint);
                            }
                            else
                            {
                                SKPaint thinLinePaint = new SKPaint
                                {
                                    Style = SKPaintStyle.Stroke,
                                    Color = _color,
                                    StrokeWidth = 2,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 20, 8 }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, thinLinePaint);
                            }
                            //add all points in list
                            points _pt = new points();
                            _pt.X = Convert.ToInt32((ptx1));// (Convert.ToInt32((ptx1 / ratio)) / 2);
                            _pt.Y = Convert.ToInt32((pty1));//(Convert.ToInt32((pty1 / ratio)) / 2);
                            _points.Add(new Tuple<points, int>(_pt, Convert.ToInt32(topoimg[0].drawing[j].id)));

                            //draw annotation
                            DrawAnnotation(topoimg[0].drawing[j].line, _skCanvas, ratio, topoimg[0].drawing[j].gradeBucket, (j + 1), long.Parse(topoimg[0].drawing[j].id));
                        }                                               
                    }
                    else if (_routeId.ToString() == topoimg[0].drawing[j].id)
                    {
                        for (int k = 0; k < topoimg[0].drawing[j].line.points.Count; k++)
                        {
                            ptx1 = float.Parse(topoimg[0].drawing[j].line.points[k].x) * ratio;
                            pty1 = float.Parse(topoimg[0].drawing[j].line.points[k].y) * ratio;
                            if (k != (topoimg[0].drawing[j].line.points.Count - 1))
                            {
                                ptx2 = float.Parse(topoimg[0].drawing[j].line.points[k + 1].x) * ratio;
                                pty2 = float.Parse(topoimg[0].drawing[j].line.points[k + 1].y) * ratio;
                            }
                            SKColor _color = HexToColor(topoimg[0].drawing[j].line.style.color);
                            if (Device.OS == TargetPlatform.Android)
                            {
                                SKPaint thinLinePaint = new SKPaint
                                {
                                    //Style = SKPaintStyle.Stroke,
                                    Color = _color,
                                    StrokeWidth = 5,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 30, 15, }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, thinLinePaint);
                            }
                            else
                            {
                                SKPaint thinLinePaint = new SKPaint
                                {
                                    Style = SKPaintStyle.Stroke,
                                    Color = _color,
                                    StrokeWidth = 2,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 20, 8 }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, thinLinePaint);
                            }
                            //add all points in list
                            points _pt = new points();
                            _pt.X = Convert.ToInt32((ptx1));// (Convert.ToInt32((ptx1 / ratio)) / 2);
                            _pt.Y = Convert.ToInt32((pty1));//(Convert.ToInt32((pty1 / ratio)) / 2);
                            _points.Add(new Tuple<points, int>(_pt, Convert.ToInt32(topoimg[0].drawing[j].id)));                           

                            //draw annotation
                            DrawAnnotation(topoimg[0].drawing[j].line, _skCanvas, ratio, topoimg[0].drawing[j].gradeBucket, (j + 1), long.Parse(topoimg[0].drawing[j].id));
                            if (Device.OS == TargetPlatform.Android)
                            {                                
                                if (globalWidth > globalHeight)
                                {
                                    double xVal = Convert.ToDouble((float.Parse(topoimg[0].drawing[j].line.points[0].x) * ratio));                                    
                                    if ((device.Display.Width/2) < xVal)
                                    {
                                        var newval = (xVal - (device.Display.Width / 2)) - 50;
                                        //var xCordinate = topoimg[0].drawing[j].line.points.Count > 0 ? Convert.ToDouble((float.Parse(topoimg[0].drawing[j].line.points[0].x) * ratio / 10)) : 0;
                                        var xCordinate = topoimg[0].drawing[j].line.points.Count > 0 ? newval : 0;
                                        OnScroll(xCordinate);
                                        // scrollView.ScrollToAsync(topoimg[0].drawing[j].line.points.Count > 0 ? Convert.ToDouble((float.Parse(topoimg[0].drawing[j].line.points[0].x) * ratio / 20)) : 0, 0, true);
                                    }
                                }
                            }
                            else
                            {
                                if (globalWidth > globalHeight)
                                {
                                    double xVal = Convert.ToDouble((float.Parse(topoimg[0].drawing[j].line.points[0].x) * ratio));
                                    if ((device.Display.Width / 2) < xVal)
                                    {
                                        var newval = (xVal - (device.Display.Width / 2));
                                        if (Convert.ToInt32(newval) > 50 && Convert.ToInt32(newval) < 100)
                                        {
                                            newval -= 10;
                                        }
                                        else if (Convert.ToInt32(newval) > 100 && Convert.ToInt32(newval) < 150)
                                        { newval -= 70; }
                                        else if (Convert.ToInt32(newval) > 150 && Convert.ToInt32(newval) < 200)
                                        { newval -= 130; }
                                        else if (Convert.ToInt32(newval) > 200 && Convert.ToInt32(newval) < 250)
                                        { newval -= 180; }
                                        else if (Convert.ToInt32(newval) > 250)
                                        { newval -= 50; }
                                        //scrollView.ScrollToAsync(topoimg[0].drawing[j].line.points.Count > 0 ? Convert.ToDouble((float.Parse(topoimg[0].drawing[j].line.points[0].x) * ratio / 6) + 20) : 0, 0, true);
                                      ////  scrollView.ScrollToAsync(topoimg[0].drawing[j].line.points.Count > 0 ? newval : 0, 0, true);
                                    }
                                }
                            }
                        }                        
                    }
                }

                path.Close();
            }
        }

        public void ReDrawLine(SKCanvas _skCanvas, int? route, float ratio, int _height, int _width)
        {
            float ptx1 = 0, ptx2 = 0, pty1 = 0, pty2 = 0;
            var topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(listData);
            using (var path = new SKPath())
            {
                for (int j = 0; j < topoimg[0].drawing.Count; j++)
                {
                    if (_routeId == 0)
                    {
                        for (int i = 0; i < topoimg[0].drawing[j].line.points.Count; i++)
                        {
                            ptx1 = float.Parse(topoimg[0].drawing[j].line.points[i].x) * ratio;
                            pty1 = float.Parse(topoimg[0].drawing[j].line.points[i].y) * ratio;
                            if (i != (topoimg[0].drawing[j].line.points.Count - 1))
                            {
                                ptx2 = float.Parse(topoimg[0].drawing[j].line.points[i + 1].x) * ratio;
                                pty2 = float.Parse(topoimg[0].drawing[j].line.points[i + 1].y) * ratio;
                            }
                            SKColor _color = HexToColor(topoimg[0].drawing[j].line.style.color);
                            if (Device.OS == TargetPlatform.Android)
                            {
                                SKPaint thinLinePaint = new SKPaint
                                {
                                    //Style = SKPaintStyle.Stroke,
                                    Color = _color,
                                    StrokeWidth = 5,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 30, 15, }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, thinLinePaint);
                            }
                            else
                            {
                                SKPaint thinLinePaint = new SKPaint
                                {
                                    Style = SKPaintStyle.Stroke,
                                    Color = _color,
                                    StrokeWidth = 2,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 20, 8 }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, thinLinePaint);
                            }
                            //add all points in list
                            points _pt = new points();
                            _pt.X = Convert.ToInt32((ptx1));// (Convert.ToInt32((ptx1 / ratio)) / 2);
                            _pt.Y = Convert.ToInt32((pty1));//(Convert.ToInt32((pty1 / ratio)) / 2);
                            _points.Add(new Tuple<points, int>(_pt, Convert.ToInt32(topoimg[0].drawing[j].id)));

                            //draw annotation
                            DrawAnnotation(topoimg[0].drawing[j].line, _skCanvas, ratio, topoimg[0].drawing[j].gradeBucket, (j + 1), long.Parse(topoimg[0].drawing[j].id));
                        }                                               
                    }
                    else if (_routeId.ToString() == topoimg[0].drawing[j].id)
                    {
                        _newRouteId = 0;
                        for (int k = 0; k < topoimg[0].drawing[j].line.points.Count; k++)
                        {
                            ptx1 = float.Parse(topoimg[0].drawing[j].line.points[k].x) * ratio;
                            pty1 = float.Parse(topoimg[0].drawing[j].line.points[k].y) * ratio;
                            if (k != (topoimg[0].drawing[j].line.points.Count - 1))
                            {
                                ptx2 = float.Parse(topoimg[0].drawing[j].line.points[k + 1].x) * ratio;
                                pty2 = float.Parse(topoimg[0].drawing[j].line.points[k + 1].y) * ratio;
                            }
                            SKColor _color = HexToColor(topoimg[0].drawing[j].line.style.color);
                            if (Device.OS == TargetPlatform.Android)
                            {
                                SKPaint rethinLinePaint = new SKPaint
                                {
                                    //Style = SKPaintStyle.Stroke,
                                    Color = SKColors.Transparent,
                                    StrokeWidth = 5,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 30, 15, }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, rethinLinePaint);
                            }
                            else
                            {
                                SKPaint rethinLinePaint = new SKPaint
                                {
                                    // Style = SKPaintStyle.Stroke,
                                    Color = SKColors.Transparent,
                                    StrokeWidth = 5,
                                    PathEffect = SKPathEffect.CreateDash(new float[] { 20, 8 }, 0)
                                };
                                //draw line
                                _skCanvas.DrawLine(ptx1, pty1, ptx2, pty2, rethinLinePaint);
                            }
                            //add all points in list
                            points _pt = new points();
                            _pt.X = Convert.ToInt32((ptx1));// (Convert.ToInt32((ptx1 / ratio)) / 2);
                            _pt.Y = Convert.ToInt32((pty1));//(Convert.ToInt32((pty1 / ratio)) / 2);
                            _newPoints.Add(new Tuple<points, int>(_pt, Convert.ToInt32(topoimg[0].drawing[j].id)));
                        }

                        //draw annotation
                        DrawAnnotation(topoimg[0].drawing[j].line, _skCanvas, ratio, topoimg[0].drawing[j].gradeBucket, (j + 1), long.Parse(topoimg[0].drawing[j].id));
                    }
                }

                path.Close();
            }
        }
        public void DrawAnnotation(topoline topoimgTop, SKCanvas _skCanvas, float ratio, string gradeBucket, int _routecnt, long id)
        {
            for (int i = 0; i < topoimgTop.points.Count; i++)
            {
                string strimg64 = string.Empty;

                if (topoimgTop.points[i].type == "1")
                {

                    //draw rect at start point                            
                    // draw these at specific locations        
                    if (!_diamondclickroute.Contains(Convert.ToInt32(id)))
                    {
                        _diamondclickroute.Add(Convert.ToInt32(id));
                        //draw rect at start point                            
                        // draw these at specific locations   
                        string _color = getGradeBucketHex(gradeBucket);

                        BoxView boxView = new BoxView
                        {
                            Color = Color.FromHex(_color),
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            Rotation = 40,
                            AutomationId = id.ToString(),
                        };
                        BoxView inner_boxView = new BoxView
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            BackgroundColor = Color.White,
                            Color = Color.White,
                            Rotation = 40
                        };

                        var labelWithId = new LabelWithId(id);
                        //labelWithId.BackgroundColor = Color.FromHex(_color); //BoxYellowColor
                        labelWithId.HorizontalTextAlignment = TextAlignment.Center;
                        labelWithId.VerticalTextAlignment = TextAlignment.Center;
                        labelWithId.Text = _routecnt.ToString();
                        labelWithId.TextColor = Color.White;
                        labelWithId.FontSize = BoxTextFontSize;

                        AbsoluteLayout parent;
                        double x = 0;
                        double y = 0;

                        if (Device.RuntimePlatform == Device.Android)
                        {
                            parent = skCanvasAndroid.Parent as AbsoluteLayout;
                            x = (((float.Parse(topoimgTop.points[i].x)) * ratio) - 30) / device.Display.Scale;
                            y = (((float.Parse(topoimgTop.points[i].y)) * ratio) - 40) / device.Display.Scale;
                        }
                        else
                        {
                            parent = skCanvasiOS.Parent as AbsoluteLayout;
                            x = (((float.Parse(topoimgTop.points[i].x)) * ratio) - 15) / device.Display.Scale;
                            y = (((float.Parse(topoimgTop.points[i].y)) * ratio) - 20) / device.Display.Scale;
                        }
                        if (isDiamondSingleClick)
                        {
                            for (int c = (parent.Children.Count() - 1); c > 0; c--)
                            {
                                if (c > 0)
                                    parent.Children.RemoveAt(c);
                            }
                        }

                        AbsoluteLayout.SetLayoutBounds(inner_boxView, new Rectangle(x - 1, y - 1.2, 20, 20));
                        AbsoluteLayout.SetLayoutFlags(inner_boxView, AbsoluteLayoutFlags.None);

                        AbsoluteLayout.SetLayoutBounds(boxView, new Rectangle(x, y, 18, 18));
                        AbsoluteLayout.SetLayoutFlags(boxView, AbsoluteLayoutFlags.None);

                        AbsoluteLayout.SetLayoutBounds(labelWithId, new Rectangle(x, y, 18, 18));
                        AbsoluteLayout.SetLayoutFlags(labelWithId, AbsoluteLayoutFlags.None);

                        var tapGesture = new TapGestureRecognizer();
                        tapGesture.Tapped += (item, eventArgs) =>
                        {
                            var boxViewWithId = item as LabelWithId;
                            if (boxViewWithId != null)
                            {
                                ShowRoute((int?)boxViewWithId.PointId);
                                //double Xcoordinate = (globalWidth / 2) - boxViewWithId.X;
                                if (Device.RuntimePlatform == Device.Android)
                                {
                                    androidZoomScroll.ScrollToAsync(boxViewWithId.X, 0, false);
                                }
                                else
                                {
                                    var xcoordinate = (device.Display.Width / 2) > boxViewWithId.X ? ((device.Display.Width / 2) - boxViewWithId.X) - 30 : boxViewWithId.X - (device.Display.Width / 2);
                                    iOSZoomScroll.ScrollToAsync(xcoordinate, 0, true);
                                }
                            }
                        };

                        labelWithId.GestureRecognizers.Add(tapGesture);
                        boxView.GestureRecognizers.Add(tapGesture);

                        parent.Children.Add(inner_boxView);
                        parent.Children.Add(boxView);
                        parent.Children.Add(labelWithId);
                    }
                }
                else if (topoimgTop.points[i].type == "3")
                {
                    if (topoimgTop.style.is_dark_checked.ToString().ToLower() == "true")
                    {
                        //17
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "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";
                        }
                        else
                        {
                            strimg64 = "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/GMzLdsAADowaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzA2NyA3OS4xNTc3NDcsIDIwMTUvMDMvMzAtMjM6NDA6NDIgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgICAgICAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgICAgICAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wOkNyZWF0ZURhdGU+MjAxNy0wMS0xOFQxNzoyMDozNyswNTozMDwveG1wOkNyZWF0ZURhdGU+CiAgICAgICAgIDx4bXA6TW9kaWZ5RGF0ZT4yMDE3LTAxLTE4VDE3OjQ2OjQ3KzA1OjMwPC94bXA6TW9kaWZ5RGF0ZT4KICAgICAgICAgPHhtcDpNZXRhZGF0YURhdGU+MjAxNy0wMS0xOFQxNzo0Njo0NyswNTozMDwveG1wOk1ldGFkYXRhRGF0ZT4KICAgICAgICAgPGRjOmZvcm1hdD5pbWFnZS9wbmc8L2RjOmZvcm1hdD4KICAgICAgICAgPHBob3Rvc2hvcDpDb2xvck1vZGU+MzwvcGhvdG9zaG9wOkNvbG9yTW9kZT4KICAgICAgICAgPHBob3Rvc2hvcDpJQ0NQcm9maWxlPnNSR0IgSUVDNjE5NjYtMi4xPC9waG90b3Nob3A6SUNDUHJvZmlsZT4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDpjZTY2MWNmNi1mMWYwLTY0NGEtYjA3Yi0wYWU2MmM0YmNkY2M8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEb2N1bWVudElEPmFkb2JlOmRvY2lkOnBob3Rvc2hvcDplY2JlY2VmMC1kZDc3LTExZTYtOWY1YS1hYjEwNTViOGVjOTc8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2MTFmYTRhZS1hZDI2LTgyNDgtYmQxOS03OWU3MmE3ZjI3NzM8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6NjExZmE0YWUtYWQyNi04MjQ4LWJkMTktNzllNzJhN2YyNzczPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE3LTAxLTE4VDE3OjIwOjM3KzA1OjMwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOmNlNjYxY2Y2LWYxZjAtNjQ0YS1iMDdiLTBhZTYyYzRiY2RjYzwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAxNy0wMS0xOFQxNzo0Njo0NyswNTozMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICAgICA8c3RFdnQ6Y2hhbmdlZD4vPC9zdEV2dDpjaGFuZ2VkPgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjE8L2V4aWY6Q29sb3JTcGFjZT4KICAgICAgICAgPGV4aWY6UGl4ZWxYRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWURpbWVuc2lvbj4KICAgICAgPC9yZGY6RGVzY3JpcHRpb24+CiAgIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7w+ybOAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAOcSURBVHja5JdNaFxVFMd/576ZeWlCp5pGaiUm45uYjW0outKCVnQj6EI3LroILtSFdSUKBoXRIuhCMBRMFy6sCt2r3VcRFyJWog3SxjTph6HWSDKxIXfeu/e4mDd1+pzJTE2kiAcOzH0f9z//c//n44mqcjPMcJPsvw0sIgZEOjwTXLfe7BmLSKCqrv47Ggb/CBCBGNCLoCdVF2ayz+Y2y7S+kRiR0ssP7ZcXnn7K3BmVBGPg0iJ8esIviUTHQN5U1ZVr4KrajYuqBk1uKpWKqd9DoHT0rdcjXTxTVr9cVl2N6r5S1t/OlfWDI2WF4c9gsD+NcK4bUJMCZ68H9U0GD73xaqR//FJWtWWrK1HilyPnlyOnK1Gi6+WavRLph1MjCne923j/WqiLYdgqmjuAlab1bcAu4EfAJd7vu3fv8KHxg0rfTuJ4VQtZifmaBoUd2Cce0/CZgzouUvpEdf7URqrOp6Da5L8CPzTWsfdjTz7uRgfvCMB6I1LXdrMbA7pG4dZblEcPmJ3A/W3TqWptAMQpQFsbHRl7fmBgPgkKvaDSRoDgHCI5/O27BGB3S+CqtVIMw8FuVO28WU0SBPVdZUGSAEjcjnEOWACGsn8+63Nz30/9PB+59bU1MEq2JIiA85DL410Nc2bWA5xtB9yI2dUW0eirWptfrdUEIPYPnnhviq+mTwMF4/J5vPfQcOcgMEAvdv684ePjOgPJl1ng/lTdtXS9lAUuhuHVYhjG2wsFFZED2wtfJ1A9fPhttzb3kxTIi8v3Ucv3mCTfY5J8L3HQS+3yOdk2+b7jm1PyjuqFSyIiuZShAr93ENN199JCIKpXvhApPVuLkyOvvZLrH9sDvdvqj9qacHZWmJzyybHjTKjOfdToEaKqAkgxDH0nFWfMA0HVWgN4kdIDIC8+Ny777x7RARGRCxd1efIo34GfUl34vLleN5pEUAxD14rZRqUa2P3SxMTlSqWi2giBDO0BMwRqgEU4P62q8d8aSlN3yhXDMOkSWJry3YmIwH051W/j1rn8cA5OetW/8i6XCR1NZ76hVa0NimHoqtaSso3rPfeeAHrS99cFTjtVTVrlbEOxvmqtSc+6E9u9ab0mIzgHuH8ygWjaCKQD25liGG5qgjCZPNWqtUsd2I42CXHrZq401PtasG4IavZfG/aq1k63uZ7vQgNdWcuZK918R5Z1MQzjrRpvb2TKlBusbFs2V2/pt87/7xPmzwEAcTwM3l47QREAAAAASUVORK5CYII=";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 22, (float.Parse(topoimgTop.points[i].y) * ratio) - 15, paint);
                            }
                        }
                    }
                    else
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "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";
                        }
                        else
                        {
                            strimg64 = "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";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 22, (float.Parse(topoimgTop.points[i].y) * ratio) - 15, paint);
                            }
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "2")
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        strimg64 = "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";
                    }
                    else
                    {
                        strimg64 = "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";
                    }
                    if (!string.IsNullOrEmpty(strimg64))
                    {
                        byte[] imageBytes = Convert.FromBase64String(strimg64);
                        Stream fileStream = new MemoryStream(imageBytes);

                        // decode the bitmap from the stream
                        using (var stream = new SKManagedStream(fileStream))
                        using (var bitmap = SKBitmap.Decode(stream))
                        using (var paint = new SKPaint())
                        {
                            if (Device.OS == TargetPlatform.Android)
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 30, (float.Parse(topoimgTop.points[i].y) * ratio) - 18, paint);
                            }
                            else
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 15, (float.Parse(topoimgTop.points[i].y) * ratio) - 18, paint);
                            }
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "17")
                {
                    if (topoimgTop.style.is_dark_checked.ToString().ToLower() == "true")
                    {
                        //17
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "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";
                        }
                        else
                        {
                            strimg64 = "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/GMzLdsAADowaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzA2NyA3OS4xNTc3NDcsIDIwMTUvMDMvMzAtMjM6NDA6NDIgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgICAgICAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgICAgICAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wOkNyZWF0ZURhdGU+MjAxNy0wMS0xOFQxNzoyMDozNyswNTozMDwveG1wOkNyZWF0ZURhdGU+CiAgICAgICAgIDx4bXA6TW9kaWZ5RGF0ZT4yMDE3LTAxLTE4VDE3OjQ2OjQ3KzA1OjMwPC94bXA6TW9kaWZ5RGF0ZT4KICAgICAgICAgPHhtcDpNZXRhZGF0YURhdGU+MjAxNy0wMS0xOFQxNzo0Njo0NyswNTozMDwveG1wOk1ldGFkYXRhRGF0ZT4KICAgICAgICAgPGRjOmZvcm1hdD5pbWFnZS9wbmc8L2RjOmZvcm1hdD4KICAgICAgICAgPHBob3Rvc2hvcDpDb2xvck1vZGU+MzwvcGhvdG9zaG9wOkNvbG9yTW9kZT4KICAgICAgICAgPHBob3Rvc2hvcDpJQ0NQcm9maWxlPnNSR0IgSUVDNjE5NjYtMi4xPC9waG90b3Nob3A6SUNDUHJvZmlsZT4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDpjZTY2MWNmNi1mMWYwLTY0NGEtYjA3Yi0wYWU2MmM0YmNkY2M8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEb2N1bWVudElEPmFkb2JlOmRvY2lkOnBob3Rvc2hvcDplY2JlY2VmMC1kZDc3LTExZTYtOWY1YS1hYjEwNTViOGVjOTc8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2MTFmYTRhZS1hZDI2LTgyNDgtYmQxOS03OWU3MmE3ZjI3NzM8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6NjExZmE0YWUtYWQyNi04MjQ4LWJkMTktNzllNzJhN2YyNzczPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE3LTAxLTE4VDE3OjIwOjM3KzA1OjMwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOmNlNjYxY2Y2LWYxZjAtNjQ0YS1iMDdiLTBhZTYyYzRiY2RjYzwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAxNy0wMS0xOFQxNzo0Njo0NyswNTozMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICAgICA8c3RFdnQ6Y2hhbmdlZD4vPC9zdEV2dDpjaGFuZ2VkPgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjE8L2V4aWY6Q29sb3JTcGFjZT4KICAgICAgICAgPGV4aWY6UGl4ZWxYRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWURpbWVuc2lvbj4KICAgICAgPC9yZGY6RGVzY3JpcHRpb24+CiAgIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7w+ybOAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAOcSURBVHja5JdNaFxVFMd/576ZeWlCp5pGaiUm45uYjW0outKCVnQj6EI3LroILtSFdSUKBoXRIuhCMBRMFy6sCt2r3VcRFyJWog3SxjTph6HWSDKxIXfeu/e4mDd1+pzJTE2kiAcOzH0f9z//c//n44mqcjPMcJPsvw0sIgZEOjwTXLfe7BmLSKCqrv47Ggb/CBCBGNCLoCdVF2ayz+Y2y7S+kRiR0ssP7ZcXnn7K3BmVBGPg0iJ8esIviUTHQN5U1ZVr4KrajYuqBk1uKpWKqd9DoHT0rdcjXTxTVr9cVl2N6r5S1t/OlfWDI2WF4c9gsD+NcK4bUJMCZ68H9U0GD73xaqR//FJWtWWrK1HilyPnlyOnK1Gi6+WavRLph1MjCne923j/WqiLYdgqmjuAlab1bcAu4EfAJd7vu3fv8KHxg0rfTuJ4VQtZifmaBoUd2Cce0/CZgzouUvpEdf7URqrOp6Da5L8CPzTWsfdjTz7uRgfvCMB6I1LXdrMbA7pG4dZblEcPmJ3A/W3TqWptAMQpQFsbHRl7fmBgPgkKvaDSRoDgHCI5/O27BGB3S+CqtVIMw8FuVO28WU0SBPVdZUGSAEjcjnEOWACGsn8+63Nz30/9PB+59bU1MEq2JIiA85DL410Nc2bWA5xtB9yI2dUW0eirWptfrdUEIPYPnnhviq+mTwMF4/J5vPfQcOcgMEAvdv684ePjOgPJl1ng/lTdtXS9lAUuhuHVYhjG2wsFFZED2wtfJ1A9fPhttzb3kxTIi8v3Ucv3mCTfY5J8L3HQS+3yOdk2+b7jm1PyjuqFSyIiuZShAr93ENN199JCIKpXvhApPVuLkyOvvZLrH9sDvdvqj9qacHZWmJzyybHjTKjOfdToEaKqAkgxDH0nFWfMA0HVWgN4kdIDIC8+Ny777x7RARGRCxd1efIo34GfUl34vLleN5pEUAxD14rZRqUa2P3SxMTlSqWi2giBDO0BMwRqgEU4P62q8d8aSlN3yhXDMOkSWJry3YmIwH051W/j1rn8cA5OetW/8i6XCR1NZ76hVa0NimHoqtaSso3rPfeeAHrS99cFTjtVTVrlbEOxvmqtSc+6E9u9ab0mIzgHuH8ygWjaCKQD25liGG5qgjCZPNWqtUsd2I42CXHrZq401PtasG4IavZfG/aq1k63uZ7vQgNdWcuZK918R5Z1MQzjrRpvb2TKlBusbFs2V2/pt87/7xPmzwEAcTwM3l47QREAAAAASUVORK5CYII=";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 22, (float.Parse(topoimgTop.points[i].y) * ratio) - 15, paint);
                            }
                        }
                    }
                    else
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "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";
                        }
                        else
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAADowaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzA2NyA3OS4xNTc3NDcsIDIwMTUvMDMvMzAtMjM6NDA6NDIgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgICAgICAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgICAgICAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wOkNyZWF0ZURhdGU+MjAxNy0wMS0xOFQxNzoyMDozNyswNTozMDwveG1wOkNyZWF0ZURhdGU+CiAgICAgICAgIDx4bXA6TW9kaWZ5RGF0ZT4yMDE3LTAxLTE4VDE3OjQ2OjQ3KzA1OjMwPC94bXA6TW9kaWZ5RGF0ZT4KICAgICAgICAgPHhtcDpNZXRhZGF0YURhdGU+MjAxNy0wMS0xOFQxNzo0Njo0NyswNTozMDwveG1wOk1ldGFkYXRhRGF0ZT4KICAgICAgICAgPGRjOmZvcm1hdD5pbWFnZS9wbmc8L2RjOmZvcm1hdD4KICAgICAgICAgPHBob3Rvc2hvcDpDb2xvck1vZGU+MzwvcGhvdG9zaG9wOkNvbG9yTW9kZT4KICAgICAgICAgPHBob3Rvc2hvcDpJQ0NQcm9maWxlPnNSR0IgSUVDNjE5NjYtMi4xPC9waG90b3Nob3A6SUNDUHJvZmlsZT4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDpjZTY2MWNmNi1mMWYwLTY0NGEtYjA3Yi0wYWU2MmM0YmNkY2M8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEb2N1bWVudElEPmFkb2JlOmRvY2lkOnBob3Rvc2hvcDplY2JlY2VmMC1kZDc3LTExZTYtOWY1YS1hYjEwNTViOGVjOTc8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2MTFmYTRhZS1hZDI2LTgyNDgtYmQxOS03OWU3MmE3ZjI3NzM8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6NjExZmE0YWUtYWQyNi04MjQ4LWJkMTktNzllNzJhN2YyNzczPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE3LTAxLTE4VDE3OjIwOjM3KzA1OjMwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOmNlNjYxY2Y2LWYxZjAtNjQ0YS1iMDdiLTBhZTYyYzRiY2RjYzwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAxNy0wMS0xOFQxNzo0Njo0NyswNTozMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICAgICA8c3RFdnQ6Y2hhbmdlZD4vPC9zdEV2dDpjaGFuZ2VkPgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjE8L2V4aWY6Q29sb3JTcGFjZT4KICAgICAgICAgPGV4aWY6UGl4ZWxYRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWURpbWVuc2lvbj4KICAgICAgPC9yZGY6RGVzY3JpcHRpb24+CiAgIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7w+ybOAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAOcSURBVHja5JdNaFxVFMd/576ZeWlCp5pGaiUm45uYjW0outKCVnQj6EI3LroILtSFdSUKBoXRIuhCMBRMFy6sCt2r3VcRFyJWog3SxjTph6HWSDKxIXfeu/e4mDd1+pzJTE2kiAcOzH0f9z//c//n44mqcjPMcJPsvw0sIgZEOjwTXLfe7BmLSKCqrv47Ggb/CBCBGNCLoCdVF2ayz+Y2y7S+kRiR0ssP7ZcXnn7K3BmVBGPg0iJ8esIviUTHQN5U1ZVr4KrajYuqBk1uKpWKqd9DoHT0rdcjXTxTVr9cVl2N6r5S1t/OlfWDI2WF4c9gsD+NcK4bUJMCZ68H9U0GD73xaqR//FJWtWWrK1HilyPnlyOnK1Gi6+WavRLph1MjCne923j/WqiLYdgqmjuAlab1bcAu4EfAJd7vu3fv8KHxg0rfTuJ4VQtZifmaBoUd2Cce0/CZgzouUvpEdf7URqrOp6Da5L8CPzTWsfdjTz7uRgfvCMB6I1LXdrMbA7pG4dZblEcPmJ3A/W3TqWptAMQpQFsbHRl7fmBgPgkKvaDSRoDgHCI5/O27BGB3S+CqtVIMw8FuVO28WU0SBPVdZUGSAEjcjnEOWACGsn8+63Nz30/9PB+59bU1MEq2JIiA85DL410Nc2bWA5xtB9yI2dUW0eirWptfrdUEIPYPnnhviq+mTwMF4/J5vPfQcOcgMEAvdv684ePjOgPJl1ng/lTdtXS9lAUuhuHVYhjG2wsFFZED2wtfJ1A9fPhttzb3kxTIi8v3Ucv3mCTfY5J8L3HQS+3yOdk2+b7jm1PyjuqFSyIiuZShAr93ENN199JCIKpXvhApPVuLkyOvvZLrH9sDvdvqj9qacHZWmJzyybHjTKjOfdToEaKqAkgxDH0nFWfMA0HVWgN4kdIDIC8+Ny777x7RARGRCxd1efIo34GfUl34vLleN5pEUAxD14rZRqUa2P3SxMTlSqWi2giBDO0BMwRqgEU4P62q8d8aSlN3yhXDMOkSWJry3YmIwH051W/j1rn8cA5OetW/8i6XCR1NZ76hVa0NimHoqtaSso3rPfeeAHrS99cFTjtVTVrlbEOxvmqtSc+6E9u9ab0mIzgHuH8ygWjaCKQD25liGG5qgjCZPNWqtUsd2I42CXHrZq401PtasG4IavZfG/aq1k63uZ7vQgNdWcuZK918R5Z1MQzjrRpvb2TKlBusbFs2V2/pt87/7xPmzwEAcTwM3l47QREAAAAASUVORK5CYII=";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 22, (float.Parse(topoimgTop.points[i].y) * ratio) - 15, paint);
                            }
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "4")
                {
                    if (topoimgTop.style.is_dark_checked.ToString().ToLower() == "true")
                    {
                        //18
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "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";
                        }
                        else
                        {
                            strimg64 = "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/GMzLdsAADowaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzA2NyA3OS4xNTc3NDcsIDIwMTUvMDMvMzAtMjM6NDA6NDIgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgICAgICAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgICAgICAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wOkNyZWF0ZURhdGU+MjAxNy0wMS0xOFQxNzoxNzowOCswNTozMDwveG1wOkNyZWF0ZURhdGU+CiAgICAgICAgIDx4bXA6TW9kaWZ5RGF0ZT4yMDE3LTAxLTE4VDE3OjU5OjUwKzA1OjMwPC94bXA6TW9kaWZ5RGF0ZT4KICAgICAgICAgPHhtcDpNZXRhZGF0YURhdGU+MjAxNy0wMS0xOFQxNzo1OTo1MCswNTozMDwveG1wOk1ldGFkYXRhRGF0ZT4KICAgICAgICAgPGRjOmZvcm1hdD5pbWFnZS9wbmc8L2RjOmZvcm1hdD4KICAgICAgICAgPHBob3Rvc2hvcDpDb2xvck1vZGU+MzwvcGhvdG9zaG9wOkNvbG9yTW9kZT4KICAgICAgICAgPHBob3Rvc2hvcDpJQ0NQcm9maWxlPnNSR0IgSUVDNjE5NjYtMi4xPC9waG90b3Nob3A6SUNDUHJvZmlsZT4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDpjNWNjYTAxYy02ODZhLTczNGQtYjFiMC1jMTM4OTliODY5OWU8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEb2N1bWVudElEPmFkb2JlOmRvY2lkOnBob3Rvc2hvcDpjODVkZjg0Ny1kZDc5LTExZTYtOWY1YS1hYjEwNTViOGVjOTc8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDphYzhiYmUwYy04MTg2LWE5NDgtYmFmOS1kODg2Y2ZjOGU1NzY8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6YWM4YmJlMGMtODE4Ni1hOTQ4LWJhZjktZDg4NmNmYzhlNTc2PC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE3LTAxLTE4VDE3OjE3OjA4KzA1OjMwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOmM1Y2NhMDFjLTY4NmEtNzM0ZC1iMWIwLWMxMzg5OWI4Njk5ZTwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAxNy0wMS0xOFQxNzo1OTo1MCswNTozMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICAgICA8c3RFdnQ6Y2hhbmdlZD4vPC9zdEV2dDpjaGFuZ2VkPgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjE8L2V4aWY6Q29sb3JTcGFjZT4KICAgICAgICAgPGV4aWY6UGl4ZWxYRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWURpbWVuc2lvbj4KICAgICAgPC9yZGY6RGVzY3JpcHRpb24+CiAgIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7ZnQCaAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAANLSURBVHja3JfBax1VFMZ/596Xd1trXpKKaEGa5CWKRLdSEIlGoYpg/wjBhQVxUS1KKTSiGxXBheLCoFB10YULRVHBVEGliCuxsRZJihIt7aL2JcFM5s18Lt5MfJnMJDUEUrxwmTtz77vf+c797jnnmSR2ojl2qF0fwGbmN1o8OWnOzLbFWJOEmVk2TjsGDD8AjINuBiKwn4DPpLmLmYEuX7vVVusGNRsdheTVo0/z2MS4d3v7IY7h51/E1Ml0zmzoJenCVGftGnDLOsA1G+Q7ym7ecdu+oXPTH48qujwiLTalhWbnebWp32dG9MxTw4Kh5yQhiVOn5CXVJFn+TZIvvJf23NUehj78+pPaofEHFdOWS+JVFjIDt4f2lT+sfuRYkr7zfnywt375S0HTYLaMTSuKrBGCSr53i2v/xLEjdujAPSkkcivL+GzOAJemuPYCPQO3pMnhJ7yD/icz/84CKuk0QlArimwTVduB++51hBtJ4mW8L2jbLDvHxLh9JHDw/rNjN+zp+yAHKWk5+GAVeC3D39vXm98o1CWUwl6eEHoInlRp/Ogm2sn38UBSwVhXFxY7A4kK9xiQshLFLC1DKs0XJq3M4EYISRnr3NVnvj2TsrKE79lFkhTsy8K58OLX2Yjp7+48Hy3/fVc3WCuKeje6tkXwDHhw+oWX9en3PziokdQDabf6zVCt1+Irl5x/8+0EaL2VnctNXcwWqlAbIcRFhTsz89LpNqTPPn64PfvNV64er+Bqu1FtF6rtRr4H/pyj/srrYuqkjkvzX2Tn0iyqueKs182vnk0nGg2OgXvtxPP28EPjjoGBTuQ6d168+1568fPT9qI0+0bGYhiY2wCwVCitKHJAui5Wm1kNmo+AJkC3AsvAj6CPpAtzXbHaNUJobwHYA4nlhUAG7iQllb9am0ysEUIf8Nc1ghtwdyuKzgJy/ypXkpSYmZucXJv6snTos3VpV2Ra+i8ZKQddTYsVzApXau3CRgj5cD/w2yasDRgDZgqxej1IsW3AYr5r8ypQWlE0s62lTxaZ6pu42Hd5aFtrrhWgv4S1AfvKioNtAW6EQCuKFivYXirLy7bVurrouorMRFUhYP+3gt7tFHC6U8DX71+YfwYAabfeRd42vXIAAAAASUVORK5CYII=";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 8, (float.Parse(topoimgTop.points[i].y) * ratio) - 14, paint);
                            }
                        }
                    }
                    else
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "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";
                        }
                        else
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAA6xJREFUeNrcl11oHFUUx393JtmN2rRNQLQqzWZ2I5oXoSABlWgstH2xIHkS8UGLDwpaaIsaFNmIgmj1RS1FGhX8QJonLUp9MCKtpYgoFD/6YZtgpcSqCXGbul93/j7sWTtZN4vaSooHLmdm7rn3f86Zc/5zx0liKSRgieTiAHbOha2MR0dd4Jy7IM46STjnnF3HNQd6bwMGQZcDJXBfA3ulyWlzMKjb/ltpS4I6l8uB3/7IZu4YGgyD7pVQqcB3R8TYm/Gkc5lnpKmxmu0CcGcD4G87FNYqO7r2mlWZwxN7cir9nJXORFIhqum5SCe/zWrbQ72CzGOSkMTu3QoltUly9WeSwob7pgNrpxAy7336QU76PVtWIapWZyJfnYni6kzk/WzkVY5KM1NZ3Xt3xsPVa21d0GLzluBWKKuHHt/qNg7cGINXUC4SWuE5IIhjgmqB9q4rYv/g/WEAKx8AkBSXSqUscB/wPPAocL1lUon0L1bVbuCWmwLSy/CVImHYUNvO2Xv0jr5smnW3ftM/MjqeA7amUqlDwBiwDXgW2A+sSy5vARx0r+isdxSLUJmAgHS6nXRIvHFD/3pgu3PuUmAHcBfwMtAN7AKu+nNRs6q2TecKZ7DsEbimPjogplyqMF+EUz/+eoABNhWLxWMdHR37zOhdYAVwDzAMvAR4W6xmqT742cGY8jxhewfe+4ZYZSGH4vsTJSYOXHd0eHjwK+C1BGin6T2m1zQE6JoA90w89Zw+/PyLANrwqTRxsgidQ22drjJ7Ogh37PLAbzsBxsfHU4m9CqbTpucSc5XGiBN9vLq/rzdzfN/enMq/RNLZyGs+8jobeRWi+NSRrEa2RIKeJxp6tlPSMknL7dmTqsmI3V9mc53JdqozjjFXTz8EL+ZH3Pq1gwFdXTXmOnxUvPFWPP3RJ+5p6cQrVoVONY8fBrYAs8Zaq2ycBE4DKeAS4AVgp2U5/gtXO+faINoAGgJdCRSBQ6D3panJBFcHQBW4AZiwamaRHv4JuBk4XiMr/DkKqxmGLWnOCCXBTHV22mzp9XGsql1XdE42NdJps82DfH4hFebzBOZpIw22m14uab+BlE3XHfjYAJEULODqRSJbMFpkog5+pyRvYKVE1LcnoqUl8D8cyQ3faQB+NTEXXGhgJKVMr5E0baA/SOprFm3i63TeUrE2+dI+GBhvH2tGlwmuPm+RFV8MvG6t9XaCHX3TM9d/IO2WhdaHvf/TuTpYKuB4qYAv3l+YPwYAT4aX+QonuXoAAAAASUVORK5CYII=";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 8, (float.Parse(topoimgTop.points[i].y) * ratio) - 14, paint);
                            }
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "18")
                {
                    if (topoimgTop.style.is_dark_checked.ToString().ToLower() == "true")
                    {
                        // 18
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "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";
                        }
                        else
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAADowaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzA2NyA3OS4xNTc3NDcsIDIwMTUvMDMvMzAtMjM6NDA6NDIgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgICAgICAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgICAgICAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wOkNyZWF0ZURhdGU+MjAxNy0wMS0xOFQxNzoxNzowOCswNTozMDwveG1wOkNyZWF0ZURhdGU+CiAgICAgICAgIDx4bXA6TW9kaWZ5RGF0ZT4yMDE3LTAxLTE4VDE3OjU5OjUwKzA1OjMwPC94bXA6TW9kaWZ5RGF0ZT4KICAgICAgICAgPHhtcDpNZXRhZGF0YURhdGU+MjAxNy0wMS0xOFQxNzo1OTo1MCswNTozMDwveG1wOk1ldGFkYXRhRGF0ZT4KICAgICAgICAgPGRjOmZvcm1hdD5pbWFnZS9wbmc8L2RjOmZvcm1hdD4KICAgICAgICAgPHBob3Rvc2hvcDpDb2xvck1vZGU+MzwvcGhvdG9zaG9wOkNvbG9yTW9kZT4KICAgICAgICAgPHBob3Rvc2hvcDpJQ0NQcm9maWxlPnNSR0IgSUVDNjE5NjYtMi4xPC9waG90b3Nob3A6SUNDUHJvZmlsZT4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDpjNWNjYTAxYy02ODZhLTczNGQtYjFiMC1jMTM4OTliODY5OWU8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEb2N1bWVudElEPmFkb2JlOmRvY2lkOnBob3Rvc2hvcDpjODVkZjg0Ny1kZDc5LTExZTYtOWY1YS1hYjEwNTViOGVjOTc8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDphYzhiYmUwYy04MTg2LWE5NDgtYmFmOS1kODg2Y2ZjOGU1NzY8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6YWM4YmJlMGMtODE4Ni1hOTQ4LWJhZjktZDg4NmNmYzhlNTc2PC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE3LTAxLTE4VDE3OjE3OjA4KzA1OjMwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOmM1Y2NhMDFjLTY4NmEtNzM0ZC1iMWIwLWMxMzg5OWI4Njk5ZTwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAxNy0wMS0xOFQxNzo1OTo1MCswNTozMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICAgICA8c3RFdnQ6Y2hhbmdlZD4vPC9zdEV2dDpjaGFuZ2VkPgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjE8L2V4aWY6Q29sb3JTcGFjZT4KICAgICAgICAgPGV4aWY6UGl4ZWxYRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWURpbWVuc2lvbj4KICAgICAgPC9yZGY6RGVzY3JpcHRpb24+CiAgIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7ZnQCaAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAANLSURBVHja3JfBax1VFMZ/596Xd1trXpKKaEGa5CWKRLdSEIlGoYpg/wjBhQVxUS1KKTSiGxXBheLCoFB10YULRVHBVEGliCuxsRZJihIt7aL2JcFM5s18Lt5MfJnMJDUEUrxwmTtz77vf+c797jnnmSR2ojl2qF0fwGbmN1o8OWnOzLbFWJOEmVk2TjsGDD8AjINuBiKwn4DPpLmLmYEuX7vVVusGNRsdheTVo0/z2MS4d3v7IY7h51/E1Ml0zmzoJenCVGftGnDLOsA1G+Q7ym7ecdu+oXPTH48qujwiLTalhWbnebWp32dG9MxTw4Kh5yQhiVOn5CXVJFn+TZIvvJf23NUehj78+pPaofEHFdOWS+JVFjIDt4f2lT+sfuRYkr7zfnywt375S0HTYLaMTSuKrBGCSr53i2v/xLEjdujAPSkkcivL+GzOAJemuPYCPQO3pMnhJ7yD/icz/84CKuk0QlArimwTVduB++51hBtJ4mW8L2jbLDvHxLh9JHDw/rNjN+zp+yAHKWk5+GAVeC3D39vXm98o1CWUwl6eEHoInlRp/Ogm2sn38UBSwVhXFxY7A4kK9xiQshLFLC1DKs0XJq3M4EYISRnr3NVnvj2TsrKE79lFkhTsy8K58OLX2Yjp7+48Hy3/fVc3WCuKeje6tkXwDHhw+oWX9en3PziokdQDabf6zVCt1+Irl5x/8+0EaL2VnctNXcwWqlAbIcRFhTsz89LpNqTPPn64PfvNV64er+Bqu1FtF6rtRr4H/pyj/srrYuqkjkvzX2Tn0iyqueKs182vnk0nGg2OgXvtxPP28EPjjoGBTuQ6d168+1568fPT9qI0+0bGYhiY2wCwVCitKHJAui5Wm1kNmo+AJkC3AsvAj6CPpAtzXbHaNUJobwHYA4nlhUAG7iQllb9am0ysEUIf8Nc1ghtwdyuKzgJy/ypXkpSYmZucXJv6snTos3VpV2Ra+i8ZKQddTYsVzApXau3CRgj5cD/w2yasDRgDZgqxej1IsW3AYr5r8ypQWlE0s62lTxaZ6pu42Hd5aFtrrhWgv4S1AfvKioNtAW6EQCuKFivYXirLy7bVurrouorMRFUhYP+3gt7tFHC6U8DX71+YfwYAabfeRd42vXIAAAAASUVORK5CYII=";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 8, (float.Parse(topoimgTop.points[i].y) * ratio) - 14, paint);
                            }
                        }
                    }
                    else
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAADwAAAA8CAYAAAA6/NlyAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAAJcEhZcwAACxIAAAsSAdLdfvwAAArsSURBVGhD7VoLcFTVGT7s5v2CECBZkr33nrPLKyJOhwIjBcKjPJyittq0jtUpYBUiEFAeESxQW5w6FIZ2pLaOxVor0olIazsQsC005akV0qE8gg8EwjMgQgIJyT5Ov//u3XhZ7i67m03VTs7MP/ex9557vvM/zvf/ZxnrbJ0z0DkDnTPwfzADXYDBBqFjXE1K1oWkPX3E9eE4XyKwdgN0zF0Q0GXLmI2kvRMX88djfCGokRDNqmmM9e3BGO/LWOEwxopKGFPGBYTOi4YEflNyLb4Xps8YR9YBjwfN2MKU3UWMqaMh0wBqKWQVzp+HrMH5ShwXQ6bg/Gs4FkAwQTe0drtIovFaDMidioH3B4j7BxUrS+65S3112kPqlrll2u55s7WaBeXaAZL5s/j+OdO1XT94SNs0bpT6UqFDXZyern4z8K7oahpoVJqWUsL3A5JokMH+zJo17hWlM+ZyM6ZNB+A/jRyufLygXPX95nkuq94UcudWId/5u5DvbsP5FiE3Vwr50i+4LJumNQ0brB7rXaCsw3uPQIqj1bQB0oajWToEdNCEjc4dGfDJUWS6Qwdrmx/7Pq9b8zPRvO0vQtbuc8mzH7rkp3UueeWUkFfOCHmpTsizHwh55F0hNwH4quWap2yaemL8GK1qQD9lPmPOr6K/7iGa1i9DQLZp1qxl0wQkDDxFY6OR7/G+NpuyzFGg1M58lAOoW56pdcuWC0LKRi7lNchVCJ3Tka5JrnB5vV7IusNCvrVeyPmzNTl2lLoDpv1UAPRnjQBVVkr79u0yCeckdisTNsDSb6T1hAEmDRvNeRsGWFYyXNs86zHevHGdS5475pKtDS6/v1n4/Ne5z3+N+/1XIY2G0DndI2nm/pbL3H8coKs2CFkxV7swZLD6zyKHMoux7oX4iP4th8ORUZBV0BMtK1o/7QjfhqaVUgB+a8ZUUVe9yS3Pf+ySANHqb+EefwP3+j7lPu8n3Oe5yP04kujXPpJLmIwmDc9qLQDvvXRcyA2/E3LKg6pvxJ3O1xjrjSXMgaWNMaxdXRERRTZjedECNrlAIgJaSRKAai6uPjNmpHpizQruPXvUJT2Nwu/3AHAz93gAVgcaFAJsnNN9D659DZrXf03z+FtxbOS+Q/DrF3/O5fdK3TsZK17E2ECzadszGRuTZbMty+zS5ad0DJW2+4xhrQ+0BEVw0Qva/fp9k5XXn56nyepNQrbUc9KuD+br9VzmvpYLXAKYhCZvEi/ueSCtn+gC89Z8/mbNe/ksl//6h5BzyvocZ2zABgD+llmjOkC7XWZD6BgqbffxXGgMaCdwrR+RiulTeNUrLwh5dD9MGRoiwNCsrj0CC/O1BEyToIMOAJa+qwHAvga3PHWkj1zxY+cFN8/bm5ma+2yvbnn35NjSvgOwSzJttmoCdSvBs3tocrBWDiXg8Dt77+zsvPz8fBhJXI2oofr03Me1XbTOnv4AgOGHEF8rgBIQK82G3gtqGqatW4Zs6iev1A2Qr/6q56XxI9lhV1Hy5p5du7+elZReo2vTZrPUbARNLzHg2cBoNAIdZ/R2jgTg5xaU8/d2gEhcPCmkvykewBpMW5O+BnIFl1e23Cabz98u33g5/+LkcazWXZhyoGd2tyM5SekXdHM1AbbSchB48Dc8v1v3efh+DsJ+aWmpvmTFoWMNQUFbtXA2r9n9doBMkHZ1DZM5U0Cy8N2bNWwG7PbK1tuxNt8h33zFce4bo9lB4Ug+1jMz53J2UnpLLBo2Azfea/NpYw2PFTQxK3UFAO8jqphwDf+24Pxdo9lhrSDpZF5m9lUA9uhag4YNTW8nzWXYbBUQ/WhE7O1m8zZp2gw4HlJCKZ+yFD68Z+tGIc+APibYh+tH38kOOnvZa/IyMw9lJ6XW3xCdQ6Jw0ERDo3gCo7Y6AIAfLZvGt/z+RSHfr0lQlG50y9O1iNLPOOu5krc3JbnrmrysrLKcpKTZALMU6+xzhk+OtvLDGACTlmOhoDwfPjzh/nu19UsXanJHFdZhrLvtXYevYB3eVy3k3Bnuk4z128hY//siBRi/358KAQELNDJvsyVE0DCBTQ7HyS2+OTiZsT7C7VJ/AsJf98uV3HfufZf06kwLdLFJ84JYBNbjCOK7okd2MC3QUKzjh98TklLKhx8QuwD4h2Bb+joabYtWw9H2F/KcTi8fgGlvApc+vaPKLeuPw7Svgxu3AjSWGi9ICEXtUAlyax1sCweXFt5LJ4Tc+JqQjzysylHDnevBoycwlg9GB9iMpRCPhllFJA4dBdgU0gsHAfDssSP41ice5y1/rnTJ+hO6pmldpmxIX6pAKm6UwBLmp2zK0yB8dbUu+fYfXXLxk/wiCgK7nIWFc5AyOIFVT/GQMWRnpaT0B9MixkVcenmQU+tRmrE76Dncqwhj0hW6yeM5Mxe34t5W2jflw5T8K8Xpqcpyl6Z+9MRM3rJji1ueA/MixqWnhASuyQBurNX6dQNSQyQRZ46iAvKGSy56ksvxYzWYsgJTLgo15Zz0pKQhmXb7WitAmAhMENSPYzy/0yREMvOQigflqMpYAj18qPbX6VPF2V+v5i27QEiO/QdrNEz16nkhm1AQIGk8F7j30QEhqSrywkouy2fwMxPHaX8r7k/JP4Ftq3gkE0Mi5aSlpTkBZjUFopwQPo0BVxnrcJVVUmH+3czQgv2YAVslGRY1LR00XM2JmVa2jCtRT/1okSb/8LLQI/i/dwp5cK+Qh94RsmaHkNWbhVy/lsuF5apnwhjlbF+uICIrM1E9gYt81shvwYE57uhWRQlEpGzJEmyYzCrcuh6uWmJRtSTzJp8uevArg5Rnv30vr0Q2VT1vFq+pmMtrUc348ClIRTmvnTeT758xVdt290RlXXFfZXl+D+d38d5ABMFuJrxdilHhcBcU9Azeo7W4jT2ZgJi1Fi5tjPReiIbD8u1wdWlEb6pg8skoUkDjGmrS2lpokCqTJDh3rghYQ9FEXEODN5Vu9L4rKyvp4+gv0G6VDydIwxETjDAF80FYPvqgJkUmSomGOgkA7w6Ifo5si6yBivC0vN3QbqhHm9K5dPjNCD3KUhYUwVSj/Q397DKY23iYp8PtdqdGk00FQce9t2TAtbQYAkxFPPhym7mTabfHl00MLJgvp2cmJw/skZHhiDZfjrDtEingh9es+ZdJkyallpSUtFUsSStWPhmtZk1ZFLaBAo3Adu/eHYtCbE0HHtwJNLY/b+ohuFtYWqpH36BlhK0jG5wXdDYxvmzFsdtT82rb+jTt95r9Ut8HjnV7lHyLKhaAnIuSrV69pIpGLJo2adbsu4gjLCU2vVo/fQNIPGJ1HdN34Me52OpQjL4oasfkyxa+mwY/KUbAItAJa+0GGhwJVR179+6dZ2iaweGGGlF7TzT+i6isVzPpPaPPFAILKhez7yZsdiJ1ZN47Mj93q/U5XF78Pxl0Ij4SBG4OYpF8OYG1rUQMP/Y+jKjdFmji0XCc1cvYB5uIN4wlJJimpiOQRWRgOjOjqB7Yc8ox3o+1VJuIobevD/yRJD0SAzP5bhvBIAqZm5vbNVpG1b4RJvjt0uLilPGDiLd/RkjMvmzFqFRV7VZURH/R+BI2I3iFzabCMKp4t1s+/xkKpYNQ2zCDgdUYOxQ1Ou/G/c9/tB04AioFGURkdQd+5ovTdYbdPpE0S8cvzqg6R9I5A3HPQJBYfCnX2nhQd8R/tOIZR+c7nTPQOQOdM9A5Ax09A/8FPQBCl6uA4EkAAAAASUVORK5CYII=";
                        }
                        else
                        {
                            strimg64 = "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/GMzLdsAADowaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjYtYzA2NyA3OS4xNTc3NDcsIDIwMTUvMDMvMzAtMjM6NDA6NDIgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgICAgICAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgICAgICAgICB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wOkNyZWF0ZURhdGU+MjAxNy0wMS0xOFQxNzoxNzowOCswNTozMDwveG1wOkNyZWF0ZURhdGU+CiAgICAgICAgIDx4bXA6TW9kaWZ5RGF0ZT4yMDE3LTAxLTE4VDE3OjU5OjUwKzA1OjMwPC94bXA6TW9kaWZ5RGF0ZT4KICAgICAgICAgPHhtcDpNZXRhZGF0YURhdGU+MjAxNy0wMS0xOFQxNzo1OTo1MCswNTozMDwveG1wOk1ldGFkYXRhRGF0ZT4KICAgICAgICAgPGRjOmZvcm1hdD5pbWFnZS9wbmc8L2RjOmZvcm1hdD4KICAgICAgICAgPHBob3Rvc2hvcDpDb2xvck1vZGU+MzwvcGhvdG9zaG9wOkNvbG9yTW9kZT4KICAgICAgICAgPHBob3Rvc2hvcDpJQ0NQcm9maWxlPnNSR0IgSUVDNjE5NjYtMi4xPC9waG90b3Nob3A6SUNDUHJvZmlsZT4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDpjNWNjYTAxYy02ODZhLTczNGQtYjFiMC1jMTM4OTliODY5OWU8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEb2N1bWVudElEPmFkb2JlOmRvY2lkOnBob3Rvc2hvcDpjODVkZjg0Ny1kZDc5LTExZTYtOWY1YS1hYjEwNTViOGVjOTc8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDphYzhiYmUwYy04MTg2LWE5NDgtYmFmOS1kODg2Y2ZjOGU1NzY8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6YWM4YmJlMGMtODE4Ni1hOTQ4LWJhZjktZDg4NmNmYzhlNTc2PC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE3LTAxLTE4VDE3OjE3OjA4KzA1OjMwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOmM1Y2NhMDFjLTY4NmEtNzM0ZC1iMWIwLWMxMzg5OWI4Njk5ZTwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAxNy0wMS0xOFQxNzo1OTo1MCswNTozMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICAgICA8c3RFdnQ6Y2hhbmdlZD4vPC9zdEV2dDpjaGFuZ2VkPgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjE8L2V4aWY6Q29sb3JTcGFjZT4KICAgICAgICAgPGV4aWY6UGl4ZWxYRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjMwPC9leGlmOlBpeGVsWURpbWVuc2lvbj4KICAgICAgPC9yZGY6RGVzY3JpcHRpb24+CiAgIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz7ZnQCaAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAANLSURBVHja3JfBax1VFMZ/596Xd1trXpKKaEGa5CWKRLdSEIlGoYpg/wjBhQVxUS1KKTSiGxXBheLCoFB10YULRVHBVEGliCuxsRZJihIt7aL2JcFM5s18Lt5MfJnMJDUEUrxwmTtz77vf+c797jnnmSR2ojl2qF0fwGbmN1o8OWnOzLbFWJOEmVk2TjsGDD8AjINuBiKwn4DPpLmLmYEuX7vVVusGNRsdheTVo0/z2MS4d3v7IY7h51/E1Ml0zmzoJenCVGftGnDLOsA1G+Q7ym7ecdu+oXPTH48qujwiLTalhWbnebWp32dG9MxTw4Kh5yQhiVOn5CXVJFn+TZIvvJf23NUehj78+pPaofEHFdOWS+JVFjIDt4f2lT+sfuRYkr7zfnywt375S0HTYLaMTSuKrBGCSr53i2v/xLEjdujAPSkkcivL+GzOAJemuPYCPQO3pMnhJ7yD/icz/84CKuk0QlArimwTVduB++51hBtJ4mW8L2jbLDvHxLh9JHDw/rNjN+zp+yAHKWk5+GAVeC3D39vXm98o1CWUwl6eEHoInlRp/Ogm2sn38UBSwVhXFxY7A4kK9xiQshLFLC1DKs0XJq3M4EYISRnr3NVnvj2TsrKE79lFkhTsy8K58OLX2Yjp7+48Hy3/fVc3WCuKeje6tkXwDHhw+oWX9en3PziokdQDabf6zVCt1+Irl5x/8+0EaL2VnctNXcwWqlAbIcRFhTsz89LpNqTPPn64PfvNV64er+Bqu1FtF6rtRr4H/pyj/srrYuqkjkvzX2Tn0iyqueKs182vnk0nGg2OgXvtxPP28EPjjoGBTuQ6d168+1568fPT9qI0+0bGYhiY2wCwVCitKHJAui5Wm1kNmo+AJkC3AsvAj6CPpAtzXbHaNUJobwHYA4nlhUAG7iQllb9am0ysEUIf8Nc1ghtwdyuKzgJy/ypXkpSYmZucXJv6snTos3VpV2Ra+i8ZKQddTYsVzApXau3CRgj5cD/w2yasDRgDZgqxej1IsW3AYr5r8ypQWlE0s62lTxaZ6pu42Hd5aFtrrhWgv4S1AfvKioNtAW6EQCuKFivYXirLy7bVurrouorMRFUhYP+3gt7tFHC6U8DX71+YfwYAabfeRd42vXIAAAAASUVORK5CYII=";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 8, (float.Parse(topoimgTop.points[i].y) * ratio) - 14, paint);
                            }
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "8")
                {
                    if (topoimgTop.style.is_dark_checked.ToString().ToLower() == "true")
                    {
                        // 16
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAADwAAAA8CAYAAAA6/NlyAAAABGdBTUEAALGPC/xhBQAAAo1JREFUaEPtmM2LT1EcxoeUxoIFmdhILJRiUFjYKWWkpGy8JTZYGO/szB/gD5jVJKWmGQuhvDdjRbY2FhQhL1EjsbDh89T3V7fTvT/3TlP33uk59en87r3fc895nu8595776+lxsQN2wA7YATtgB+yAHbADdsAO2AE7YAfsgB2wA7kOLOHsalBdpSh+1TTaVeljxmPnc8etcBA2Vbz7RuL3R/veim1rCd9Gr1fgBtyBEbgAW/4zms1cPx/xt6mvweUQXouQsp2eJvA9/M3wLUR3u4fEfs20+cPvV3CybMd1xe2O7L5ORD8N0Zqy2bIhMqvrWZM+cnwX9tUlpGy/SwncHqKzAvQ7L9NpZjttHhJ/AtaW7biuuLl03AcHYBTSTEuI1vRROAcPksx+4vhJZH0d9cK6hFTtdxkNdobobKZ/R6Yl7DP8TARL7CmQ2FaVeYxWog8VZDqd7q3NbJqVokynglub2bxpuICTQ6DpnArVsc5fhRWgZ8CsKJdQMVUgWOd1fVaU9agYBD2Nu2X4Ptf1sFJ8q0vRezZvapfZkTXWjKId1DtGPAk3YQxeJtO8aEfWWKEa2Bwoyuwjrp2FAdgF13PWdasyvQYBx+BeIkSbDGVWu6x+WAzLQTsyZfpNEq8d2XFo/NZS37KP4UsiQGKVWYnNFolWpiU6u65/cPwCtAVtdNnD6MbhQwj4Tj0BF0PsomT02pFJ9OEQrTUu4b/gGRxptFoGp88/TVtlRwN/DmdCbLexdzJ9K9q9pR6GHU0XrM9D/XuhdTwUGeqn1prtVvS3kETvjXZ6d+vjY2XTBXfGpyd1hypjnm67Kn041g7YATtgB+yAHbADdsAO2AE7YAfsgB2wA3ag3Q78A4nwyk/kbkW/AAAAAElFTkSuQmCC";
                        }
                        else
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAK1JREFUSEtjYBgFoyEwGgKjITCcQ4Ad6LkYPB6MAspx0iIAHgMN/Q/Eb7AY/goqd4MWFjtDDUe3HGYpSDyTFhaDzPREs/wFEj+PVpbCzEW2HORLEKa5pTDLPyD5FMSmC0COU5iPsSU4qjmGEWgSsqVFQL43jgRHNUtBBu1BsgRkKQwgW36KqjZCDSuFWlyIxXCY5TNoYTHITFBw4wL45GjlnlFzR0NgNARGYggAALsUMlbofzPnAAAAAElFTkSuQmCC";
                        }

                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 14, (float.Parse(topoimgTop.points[i].y) * ratio) - 14, paint);
                            }
                        }
                    }
                    else
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAADwAAAA8CAYAAAA6/NlyAAAABGdBTUEAALGPC/xhBQAAAqtJREFUaEPtmEtIVFEcxgUpI6Fs16ZCpAduQoiEolaSutOtUVC5EMJNukgi2hdBBBlUFK6lTVAKURRBtWqjQbQQFy160CI3McM8+v3hGLfT3MeRmTsq38DHmbnnf875vu9/XndaWvSRA3JADsgBOSAH5IAckANyQA7IATkgB+SAHJAD/ztQrVa3g11WhvhD/Daw08qQdk2PrVQqneC4lSFkiN8Heq0Made02GKxeKxUKk2Vy+W7kJ4B98A1cCKJlBM5SXkbPASPwHUy3dc0MVkGRuxFxH4GRcj+/ZjoFMGTtPkRbWPf09pl4dTQGAiehPgN8MET/M5lujdKgGc9wDL72ov/zrMXYKShhOvROST3gFt+tmplzMTGZPYxdcO02VsPTg3tA5KtkO0Hto4XPeGvXKbHKCfAvJfZLzybA2MmFmxpKNl6dW7HCqS7TXRMplfI7DfqVzzBJvY8OFgvLrn140QPxmT6Hx+IsczOg3ETG3p+5yYqbaC0TK+qdmJH+X0orc8NUc9xdYUp/LvW9Hab2U3K3RtCTBaSCL6M4F8JghPP6SxjrIuYQqHQg9hLiH2ekuE3TOspcGRdEF8ribhzdtNlOuEGteQ2qAeU0+B9lhvZWg3PrV3CDeopdaPgsO3IlJluZLkRDx0IAV1gBDyJuUGNm1CwlZg2cMpleiEaz3p/y9q/St3RUA65xkN6CLKzYLnGDeqCf86aaJfpaX9d265uu3uuAkIHg3QfIu6AT+58tbeeZ3aDcsLa/T5dpgdcpj+uCkfwVwRPhHLINR7S+8FZ8NIRt5eFcyDxbhzJ9H1n1E+bKTZjchUQOhgEOyBvLw1ngP3TcdrEgh1pfUXWtLWzd+RhcCCtnerlgByQA3JADsgBOSAH5IAckANyQA7IATkgB+TAZnfgD9XKTUf1XdqVAAAAAElFTkSuQmCC";
                        }
                        else
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAKVJREFUSEvtksENgCAMRVnEu3EBD07hAiYq27mRGxhPLIB+EptUIieoMaZcgNT8R581RpcaUANq4NcGvPddqkHUWpHmEbwe14oBqO2pWvZjEF5ReNgpkENx7rNBTwEIbjgc943uOE8iUNbhDR7AgFpRKIO7J+2icP5PX4NHg2QBrsXhgC5skGbSyuH4xhXXjdDhGqQxDid4eFxxsAaqATWgBj5j4ARpiup/xyDzKAAAAABJRU5ErkJggg==";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                if (Device.OS == TargetPlatform.Android)
                                {
                                    _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 26, (float.Parse(topoimgTop.points[i].y) * ratio) - 26, paint);
                                }
                                else
                                {
                                    _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 14, (float.Parse(topoimgTop.points[i].y) * ratio) - 14, paint);
                                }
                            }
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "16")
                {
                    if (topoimgTop.style.is_dark_checked.ToString().ToLower() == "true")
                    {
                        //16
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAADwAAAA8CAYAAAA6/NlyAAAABGdBTUEAALGPC/xhBQAAAo1JREFUaEPtmM2LT1EcxoeUxoIFmdhILJRiUFjYKWWkpGy8JTZYGO/szB/gD5jVJKWmGQuhvDdjRbY2FhQhL1EjsbDh89T3V7fTvT/3TlP33uk59en87r3fc895nu8595776+lxsQN2wA7YATtgB+yAHbADdsAO2AE7YAfsgB2wA7kOLOHsalBdpSh+1TTaVeljxmPnc8etcBA2Vbz7RuL3R/veim1rCd9Gr1fgBtyBEbgAW/4zms1cPx/xt6mvweUQXouQsp2eJvA9/M3wLUR3u4fEfs20+cPvV3CybMd1xe2O7L5ORD8N0Zqy2bIhMqvrWZM+cnwX9tUlpGy/SwncHqKzAvQ7L9NpZjttHhJ/AtaW7biuuLl03AcHYBTSTEuI1vRROAcPksx+4vhJZH0d9cK6hFTtdxkNdobobKZ/R6Yl7DP8TARL7CmQ2FaVeYxWog8VZDqd7q3NbJqVokynglub2bxpuICTQ6DpnArVsc5fhRWgZ8CsKJdQMVUgWOd1fVaU9agYBD2Nu2X4Ptf1sFJ8q0vRezZvapfZkTXWjKId1DtGPAk3YQxeJtO8aEfWWKEa2Bwoyuwjrp2FAdgF13PWdasyvQYBx+BeIkSbDGVWu6x+WAzLQTsyZfpNEq8d2XFo/NZS37KP4UsiQGKVWYnNFolWpiU6u65/cPwCtAVtdNnD6MbhQwj4Tj0BF0PsomT02pFJ9OEQrTUu4b/gGRxptFoGp88/TVtlRwN/DmdCbLexdzJ9K9q9pR6GHU0XrM9D/XuhdTwUGeqn1prtVvS3kETvjXZ6d+vjY2XTBXfGpyd1hypjnm67Kn041g7YATtgB+yAHbADdsAO2AE7YAfsgB2wA3ag3Q78A4nwyk/kbkW/AAAAAElFTkSuQmCC";
                        }
                        else
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAK1JREFUSEtjYBgFoyEwGgKjITCcQ4Ad6LkYPB6MAspx0iIAHgMN/Q/Eb7AY/goqd4MWFjtDDUe3HGYpSDyTFhaDzPREs/wFEj+PVpbCzEW2HORLEKa5pTDLPyD5FMSmC0COU5iPsSU4qjmGEWgSsqVFQL43jgRHNUtBBu1BsgRkKQwgW36KqjZCDSuFWlyIxXCY5TNoYTHITFBw4wL45GjlnlFzR0NgNARGYggAALsUMlbofzPnAAAAAElFTkSuQmCC";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 15, (float.Parse(topoimgTop.points[i].y) * ratio) - 15, paint);
                            }
                        }
                    }
                    else
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAADwAAAA8CAYAAAA6/NlyAAAABGdBTUEAALGPC/xhBQAAAo1JREFUaEPtmM2LT1EcxoeUxoIFmdhILJRiUFjYKWWkpGy8JTZYGO/szB/gD5jVJKWmGQuhvDdjRbY2FhQhL1EjsbDh89T3V7fTvT/3TlP33uk59en87r3fc895nu8595776+lxsQN2wA7YATtgB+yAHbADdsAO2AE7YAfsgB2wA7kOLOHsalBdpSh+1TTaVeljxmPnc8etcBA2Vbz7RuL3R/veim1rCd9Gr1fgBtyBEbgAW/4zms1cPx/xt6mvweUQXouQsp2eJvA9/M3wLUR3u4fEfs20+cPvV3CybMd1xe2O7L5ORD8N0Zqy2bIhMqvrWZM+cnwX9tUlpGy/SwncHqKzAvQ7L9NpZjttHhJ/AtaW7biuuLl03AcHYBTSTEuI1vRROAcPksx+4vhJZH0d9cK6hFTtdxkNdobobKZ/R6Yl7DP8TARL7CmQ2FaVeYxWog8VZDqd7q3NbJqVokynglub2bxpuICTQ6DpnArVsc5fhRWgZ8CsKJdQMVUgWOd1fVaU9agYBD2Nu2X4Ptf1sFJ8q0vRezZvapfZkTXWjKId1DtGPAk3YQxeJtO8aEfWWKEa2Bwoyuwjrp2FAdgF13PWdasyvQYBx+BeIkSbDGVWu6x+WAzLQTsyZfpNEq8d2XFo/NZS37KP4UsiQGKVWYnNFolWpiU6u65/cPwCtAVtdNnD6MbhQwj4Tj0BF0PsomT02pFJ9OEQrTUu4b/gGRxptFoGp88/TVtlRwN/DmdCbLexdzJ9K9q9pR6GHU0XrM9D/XuhdTwUGeqn1prtVvS3kETvjXZ6d+vjY2XTBXfGpyd1hypjnm67Kn041g7YATtgB+yAHbADdsAO2AE7YAfsgB2wA3ag3Q78A4nwyk/kbkW/AAAAAElFTkSuQmCC";
                        }
                        else
                        {
                            strimg64 = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAK1JREFUSEtjYBgFoyEwGgKjITCcQ4Ad6LkYPB6MAspx0iIAHgMN/Q/Eb7AY/goqd4MWFjtDDUe3HGYpSDyTFhaDzPREs/wFEj+PVpbCzEW2HORLEKa5pTDLPyD5FMSmC0COU5iPsSU4qjmGEWgSsqVFQL43jgRHNUtBBu1BsgRkKQwgW36KqjZCDSuFWlyIxXCY5TNoYTHITFBw4wL45GjlnlFzR0NgNARGYggAALsUMlbofzPnAAAAAElFTkSuQmCC";
                        }
                        if (!string.IsNullOrEmpty(strimg64))
                        {
                            byte[] imageBytes = Convert.FromBase64String(strimg64);
                            Stream fileStream = new MemoryStream(imageBytes);

                            // decode the bitmap from the stream
                            using (var stream = new SKManagedStream(fileStream))
                            using (var bitmap = SKBitmap.Decode(stream))
                            using (var paint = new SKPaint())
                            {
                                if (Device.OS == TargetPlatform.Android)
                                {
                                    _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 28, (float.Parse(topoimgTop.points[i].y) * ratio) - 28, paint);
                                }
                                else
                                {
                                    _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 15, (float.Parse(topoimgTop.points[i].y) * ratio) - 15, paint);
                                }
                            }
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "5")
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        // draw these at specific locations                       
                        var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) - 140, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 40, 100, 80);
                        using (var paint = new SKPaint())
                        {
                            _skCanvas.Save();
                            _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                        }
                    }
                    else
                    {
                        var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) - 80, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 20, 60, 40);
                        using (var paint = new SKPaint())
                        {
                            _skCanvas.Save();
                            _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                        }
                    }

                    using (var paint = new SKPaint())
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            paint.TextSize = 35.0f;
                        }
                        else
                        {
                            paint.TextSize = 20.0f;
                        }
                        paint.IsAntialias = true;
                        paint.Color = SKColors.White;
                        paint.IsStroke = true;
                        paint.StrokeWidth = 2;
                        paint.TextAlign = SKTextAlign.Center;
                        if (Device.OS == TargetPlatform.Android)
                        {
                            _skCanvas.DrawText((topoimgTop.points[i].label).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) - 80, (float.Parse(topoimgTop.points[i].y) * ratio) + 10, paint);
                        }
                        else
                        {
                            _skCanvas.DrawText((topoimgTop.points[i].label).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) - 50, (float.Parse(topoimgTop.points[i].y) * ratio) + 5, paint);
                        }
                    }
                }
                else if (topoimgTop.points[i].type == "6")
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        // draw these at specific locations                       
                        var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) + 20, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 40, 100, 80);

                        using (var paint = new SKPaint())
                        {
                            _skCanvas.Save();
                            _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                        }
                    }
                    else
                    {
                        // draw these at specific locations                       
                        var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) + 20, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 20, 60, 40);

                        using (var paint = new SKPaint())
                        {
                            _skCanvas.Save();
                            _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                        }
                    }
                    using (var paint = new SKPaint())
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            paint.TextSize = 35.0f;
                        }
                        else
                        {
                            paint.TextSize = 20.0f;
                        }
                        paint.IsAntialias = true;
                        paint.Color = SKColors.White;
                        paint.IsStroke = true;
                        paint.StrokeWidth = 2;
                        paint.TextAlign = SKTextAlign.Center;
                        if (Device.OS == TargetPlatform.Android)
                        {
                            _skCanvas.DrawText((topoimgTop.points[i].label).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) + 70, (float.Parse(topoimgTop.points[i].y) * ratio) + 10, paint);
                        }
                        else
                        {
                            _skCanvas.DrawText((topoimgTop.points[i].label).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) + 50, (float.Parse(topoimgTop.points[i].y) * ratio) + 5, paint);
                        }
                    }
                }

                //code to show left and right text
                for (int j = 0; j < topoimgTop.pointsText.Count; j++)
                {
                    //left side text                
                    if (topoimgTop.pointsText[j].point_id.Contains(i.ToString()))
                    {
                        if (topoimgTop.pointsText[j].text_id.IndexOf("L") > -1)
                        {
                            if (topoimgTop.pointsText[j].text_id.Contains(i.ToString()))
                            {
                                if (Device.OS == TargetPlatform.Android)
                                {
                                    // draw these at specific locations                       
                                    var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) - 100, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 20, 200, 80);

                                    using (var paint = new SKPaint())
                                    {
                                        _skCanvas.Save();
                                        _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                                    }
                                    using (var paint = new SKPaint())
                                    {
                                        paint.TextSize = 40.0f;
                                        paint.IsAntialias = true;
                                        paint.Color = SKColors.White;
                                        paint.IsStroke = true;
                                        paint.StrokeWidth = 3;
                                        paint.TextAlign = SKTextAlign.Center;

                                        _skCanvas.DrawText((topoimgTop.pointsText[j].text_value).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) - 70, (float.Parse(topoimgTop.points[i].y) * ratio) + 25, paint);
                                    }
                                }
                                else
                                {
                                    // draw these at specific locations                       
                                    var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) - 80, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 20, 60, 40);

                                    using (var paint = new SKPaint())
                                    {
                                        _skCanvas.Save();
                                        _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                                    }
                                    using (var paint = new SKPaint())
                                    {
                                        paint.TextSize = 20.0f;
                                        paint.IsAntialias = true;
                                        paint.Color = SKColors.White;
                                        paint.IsStroke = true;
                                        paint.StrokeWidth = 2;
                                        paint.TextAlign = SKTextAlign.Center;

                                        _skCanvas.DrawText((topoimgTop.pointsText[j].text_value).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) - 50, (float.Parse(topoimgTop.points[i].y) * ratio) + 5, paint);
                                    }
                                }
                            }
                        }
                    }

                    //right side text                
                    if (topoimgTop.pointsText[j].point_id.Contains(i.ToString()))
                    {
                        if (topoimgTop.pointsText[j].text_id.IndexOf("R") > -1)
                        {
                            if (topoimgTop.pointsText[j].text_id.Contains(i.ToString()))
                            {
                                if (Device.OS == TargetPlatform.Android)
                                {
                                    // draw these at specific locations                       
                                    var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) + 50, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 40, 200, 80);

                                    using (var paint = new SKPaint())
                                    {
                                        _skCanvas.Save();
                                        _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                                    }
                                    using (var paint = new SKPaint())
                                    {
                                        paint.TextSize = 40.0f;
                                        paint.IsAntialias = true;
                                        paint.Color = SKColors.White;
                                        paint.IsStroke = true;
                                        paint.StrokeWidth = 3;
                                        paint.TextAlign = SKTextAlign.Center;
                                        _skCanvas.DrawText((topoimgTop.pointsText[j].text_value).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) + 150, (float.Parse(topoimgTop.points[i].y) * ratio) + 15, paint);
                                    }
                                }
                                else
                                {
                                    // draw these at specific locations                       
                                    var Rect = SKRect.Create(((float.Parse(topoimgTop.points[i].x)) * ratio) + 20, ((float.Parse(topoimgTop.points[i].y)) * ratio) - 20, 60, 40);

                                    using (var paint = new SKPaint())
                                    {
                                        _skCanvas.Save();
                                        _skCanvas.DrawRect(Rect, new SKPaint() { Color = SKColors.Black });
                                    }
                                    using (var paint = new SKPaint())
                                    {
                                        paint.TextSize = 20.0f;
                                        paint.IsAntialias = true;
                                        paint.Color = SKColors.White;
                                        paint.IsStroke = true;
                                        paint.StrokeWidth = 2;
                                        paint.TextAlign = SKTextAlign.Center;
                                        _skCanvas.DrawText((topoimgTop.pointsText[j].text_value).ToString(), (float.Parse(topoimgTop.points[i].x) * ratio) + 50, (float.Parse(topoimgTop.points[i].y) * ratio) + 5, paint);
                                    }
                                }
                            }
                        }
                    }

                    //showing arrow logic                   
                    if (topoimgTop.pointsText[j].point_id.Contains(i.ToString()))
                    {
                        if (topoimgTop.pointsText[i].isdirection.ToLower() == "true")
                        {
                            //calculate angle                            
                            points _fpt = new points();
                            points _spt = new points();
                            _fpt.X = (i + 1) < topoimgTop.points.Count ? Convert.ToInt32(topoimgTop.points[i + 1].x) : Convert.ToInt32(topoimgTop.points[i - 1].x);
                            _fpt.Y = (i + 1) < topoimgTop.points.Count ? Convert.ToInt32(topoimgTop.points[i + 1].y) : Convert.ToInt32(topoimgTop.points[i - 1].y);
                            _spt.X = Convert.ToInt32(topoimgTop.points[i].x);
                            _spt.Y = Convert.ToInt32(topoimgTop.points[i].y);

                            double angleDeg;
                            if (_fpt.Y < _spt.Y)
                            {
                                angleDeg = Math.Round((Math.Atan2(_fpt.Y - _spt.Y, _fpt.X - _spt.X) * 180 / Math.PI) + 50);
                            }
                            else
                            {
                                angleDeg = Math.Round((Math.Atan2(_spt.Y - _fpt.Y, _spt.X - _fpt.X) * 180 / Math.PI) + 50);
                            }

                            HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_GetBitmap, Cache.AccessToken);
                            Dictionary<string, string> dictquery = new Dictionary<string, string>();
                            dictquery.Add("angle", angleDeg.ToString());
                            if (Device.OS == TargetPlatform.Android)
                            {
                                dictquery.Add("isandroid", "true");
                            }
                            else { dictquery.Add("isandroid", "false"); }
                            var response = apicall.Get<imgData>(dictquery);
                            _strimg64 = response.Result.base64;

                            if (!string.IsNullOrEmpty(_strimg64))
                            {
                                byte[] imageBytes = Convert.FromBase64String(_strimg64);
                                Stream fileStream = new MemoryStream(imageBytes);

                                // decode the bitmap from the stream
                                using (var stream = new SKManagedStream(fileStream))
                                using (var bitmap = SKBitmap.Decode(stream))
                                using (var paint = new SKPaint())
                                {
                                    if (Device.OS == TargetPlatform.Android)
                                    {
                                        _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 55, (float.Parse(topoimgTop.points[i].y) * ratio) - 60, paint);
                                    }
                                    else
                                    {
                                        _skCanvas.DrawBitmap(bitmap, (float.Parse(topoimgTop.points[i].x) * ratio) - 30, (float.Parse(topoimgTop.points[i].y) * ratio) - 30, paint);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static SKColor HexToColor(string color)
        {
            SKColor _color = new SKColor();
            if (color.StartsWith("#"))
                color = color.Remove(0, 1);
            byte r, g, b;
            if (color.Length == 3)
            {
                r = Convert.ToByte(color[0] + "" + color[0], 16);
                g = Convert.ToByte(color[1] + "" + color[1], 16);
                b = Convert.ToByte(color[2] + "" + color[2], 16);
            }
            else if (color.Length == 6)
            {
                r = Convert.ToByte(color[0] + "" + color[1], 16);
                g = Convert.ToByte(color[2] + "" + color[3], 16);
                b = Convert.ToByte(color[4] + "" + color[5], 16);
            }
            else
            {
                throw new ArgumentException("Hex color " + color + " is invalid.");
            }
            return _color = new SKColor(r, g, b);
        }
        public string getGradeBucketHex(string grade_bucket_id)
        {
            // SKColor color = new SKColor();
            string color = string.Empty;
            var hexCode = App.DAUtil.GetBucketHexColorByGradeBucketId(grade_bucket_id) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(grade_bucket_id);
            switch (grade_bucket_id)
            {
                case "1":
                    return color = "#036177";// new SKColor(3, 97, 119);
                case "2":
                    return color = "#1f8a70";//new SKColor(31, 138, 112);
                case "3":
                    return color = "#91a537"; //new SKColor(145, 165, 55);
                case "4":
                    return color = "#b49800";//new SKColor(180, 152, 0);
                case "5":
                    return color = "#fd7400";//new SKColor(253, 116, 0);
                default:
                    return color = "#cccccc";//new SKColor(204, 204, 204);
            }
        }
        public double GetHeight(Double _height)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                switch (Convert.ToInt32(_height))
                {
                    case 1334:
                        _height = _height + 200; //iphone 6 and iphone 7 and iphone 6s
                        break;
                    case 2208:
                        _height = _height - 580; //iPhone6Plus and iPhone7Plus and iphone 6s plus
                        break;
                    case 1136:
                        _height = _height + 350; //iPhone5 and iPhone5s and iPhone SE
                        break;
                    case 2048:
                        _height = _height - 840; //iPas Air 2 and iPad Air 
                        break;
                }
            }
            else
            {
                var device = XLabs.Ioc.Resolver.Resolve<IDevice>();
                //_height = (_height / device.Display.Scale) - 110;
                _height = (_height / device.Display.Scale) + 1350;
            }
            return _height;
        }
      
        SKPoint ConvertToPixel(Point pt)
        {
            SKPoint _pt;
            if (Device.OS == TargetPlatform.Android)
            {
                _pt= new  SKPoint((float)(skCanvasAndroid.CanvasSize.Width * pt.X / skCanvasAndroid.Width),
                               (float)(skCanvasAndroid.CanvasSize.Height * pt.Y / skCanvasAndroid.Height));
            }
            else
            {
                _pt = new SKPoint((float)(skCanvasiOS.CanvasSize.Width * pt.X / skCanvasiOS.Width),
                                              (float)(skCanvasiOS.CanvasSize.Height * pt.Y / skCanvasiOS.Height));
            }
            return _pt;
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            var parent = skCanvasAndroid.Parent.Parent as ZoomableScrollView;

            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = parent.Scale;
                parent.AnchorX = 0;
                parent.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = parent.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (parent.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = parent.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (parent.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * parent.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * parent.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                parent.TranslationX = targetX.Clamp(-parent.Width * (currentScale - 1), 0);
                parent.TranslationY = targetY.Clamp(-parent.Height * (currentScale - 1), 0);

                // Apply scale factor.
                parent.ScaleTo(currentScale);
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = parent.TranslationX;
                yOffset = parent.TranslationY;
            }
        }


        private void OnSwipeTopRoutePopupLg(object sender, EventArgs e)
        {
            if (listData != string.Empty)
            {
                TopoMapRouteVM.HideRoutePopupLgCommand.Execute(null);
            }
        }

        private void OnSwipeDownRoutePopupSm(object sender, EventArgs e)
        {
            TopoMapRouteVM.ShowRoutePopupLgCommand.Execute(null);
        }

        private void OnSwipeTopRoutePopupSm(object sender, EventArgs e)
        {
            TopoMapRouteVM.DisplayRoutePopupSm = false;
            _bucket.Clear();
            topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(listData);
            if (topoimg != null)
            {
                for (int i = 0; i < topoimg[0].drawing.Count; i++)
                {
                    _bucket.Add(new Tuple<string, string>(App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(topoimg[0].drawing[i].gradeBucket), topoimg[0].drawing[i].gradeBucket));
                }
            }
            // if we got to the topo via the map and not the list, redraw all the routes
            if (_newRouteId <= 0)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    androidZoomScroll.ScrollToAsync(0, 0, false);
                }
                else
                {
                    iOSZoomScroll.ScrollToAsync(0, 0, true);
                }
                // webView.CallJsFunction("initDrawing", staticAnnotationData, listData, newHeight,_bucket);
                _routeId = 0;
                hasBeingRedrawing = 0;
                isDiamondClick = false;
                isDiamondSingleClick = false;
                _diamondclickroute.Clear();
                if (Device.OS == TargetPlatform.iOS)
                    skCanvasiOS.InvalidateSurface();

                //skCanvasAndroid.InvalidateSurface();
                Redraw(); /*skCanvasAndroid.InvalidateSurface()*///
            }
        }
    }
}
public class points
{
    public int X { get; set; }
    public int Y { get; set; }
}
public class imgData
{
    public string base64 { get; set; }
}
