using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;

namespace SloperMobile.Droid.MapRenderers
{
    public static class DroidXFUtilities
    {
        public static Android.Views.View GetAndroidNativeView(Xamarin.Forms.View xfView)
        {
            if (xfView == null)
                return null;

            var renderer = Platform.CreateRenderer(xfView); // RendererFactory.GetRenderer(xfView);

            renderer.SetElement(xfView);
            renderer.UpdateLayout();

            var viewGroup = renderer.ViewGroup;
            return viewGroup;
        }


        /// <summary>
        /// transform a XF View into a Droid ViewGroup
        /// </summary>
        /// <param name="view">the XF view</param>
        /// <param name="size">the target droid view rectangle</param>
        /// <returns>the droid view group</returns>
        public static ViewGroup ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size)
        {
            var vRenderer = Platform.CreateRenderer(view);  // RendererFactory.GetRenderer(view);
            var viewGroup = vRenderer.ViewGroup;

            vRenderer.Tracker.UpdateLayout();

            var layoutParams = new ViewGroup.LayoutParams((int)size.Width, (int)size.Height);

            viewGroup.LayoutParameters = layoutParams;
            viewGroup.DrawingCacheEnabled = true;
            view.Layout(size);
            viewGroup.Layout(0, 0, (int)size.Width, (int)size.Height);  // (int)view.WidthRequest, (int)view.HeightRequest);
            return viewGroup;
        }


        /// <summary>
        /// transform a droid native view into bitmap. draws the sub elements of a viewgroup
        /// Note: this method does not recurese into sub-children. it would be good to do this (one day!)
        /// </summary>
        /// <param name="viewGroup">the native view group</param>
        /// <param name="context">the context</param>
        /// <param name="width">desired bitmap width</param>
        /// <param name="height">desired bitmap height</param>
        /// <param name="density">device display metrics density</param>
        /// <param name="forMaps">true to act specifically for maps</param>
        /// <returns>the bitmap of the native view group</returns>
        public static Bitmap ViewGroupToBitmap(ViewGroup viewGroup, Context context, int width, int height, double density, bool forMaps)
        {
            int viewCount = viewGroup == null ? 0 : viewGroup.ChildCount;

            if (context == null || viewGroup == null || width <= 0 || height <= 0)
                return null;

            Android.Widget.LinearLayout layout = new Android.Widget.LinearLayout(context);
            Bitmap bmpLayout = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);  //.Rgb565);
            Android.Graphics.Color white = Android.Graphics.Color.Argb(0xff, 0xff, 0xff, 0xff);

            layout.DrawingCacheEnabled = true;
            layout.SetBackgroundColor(white);


            Canvas canvas = new Canvas(bmpLayout);
            Paint paint = new Paint();
            int left = 10,
                    top = 10;
            bool prevText = false;

            if (forMaps)
            {
                bmpLayout.EraseColor(white);
                canvas.DrawBitmap(bmpLayout, 0, 0, paint);
            }

            // this call just sets the canvas background
            //if (!forMaps)
            //	viewGroup.Draw(canvas);

            // add the sub views contained in this view group
            for (int ndx = 0; ndx < viewCount; ndx++)
            {
                Android.Views.View view = viewGroup.GetChildAt(0);

                int wid = Math.Max(0, view.MeasuredWidth),
                        hi = Math.Max(0, view.MeasuredHeight),
                        itemLeft = 0;
                bool isImage = (view is ImageRenderer),
                        isText = (view is LabelRenderer);
                LabelRenderer labelRenderer = view as LabelRenderer;

                if (isText && prevText)
                    hi += 16 * (int)density;   // interlines (empiric -:))

                if (!prevText)
                    prevText = isText;
                wid = Math.Min(wid, width - left);

                viewGroup.RemoveView(view);

                // there is nothing to draw
                if (wid <= 0 || hi <= 0)
                    continue;

                if (isImage)
                {
                    ImageRenderer imgR = view as ImageRenderer;
                    var src = imgR.Element.Source;
                    FileImageSource fileSrc = src as FileImageSource;
                    Bitmap bmp = null;

                    imgR.Layout(0, 0, wid, hi);

                    if (fileSrc != null)
                    {
                        // get the resource id of the image file (should be in 'drawable' folder)
                        int id = context.Resources.GetIdentifier(System.IO.Path.GetFileNameWithoutExtension(fileSrc.File), "drawable", context.PackageName);

                        bmp = BitmapFactory.DecodeResource(context.Resources, id);
                        view = new ImageView(context);

                        if (bmp != null)
                            (view as ImageView).SetImageBitmap(bmp);

                        itemLeft = imgR.PaddingLeft + imgR.Left;

                        view.Layout(left + itemLeft, top, itemLeft + wid, hi);
                    }

                    layout.AddView(view, wid, hi);
                }
                else
                    layout.AddView(view, wid, hi);

                if (!isImage && view.Width > 0 && view.Height > 0)
                    view.Layout(left + itemLeft, top, left + wid, top + hi);

                top += hi;
            }

            layout.Draw(canvas);

            return bmpLayout;
        }

    }
}