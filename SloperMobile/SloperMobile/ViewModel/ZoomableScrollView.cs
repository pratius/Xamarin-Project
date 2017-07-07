﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class ZoomableScrollView : ScrollView
    {
        public static readonly BindableProperty MinimumZoomScaleProperty = BindableProperty.Create("MinimumZoomScale", typeof(float), typeof(ZoomableScrollView), default(float));

        public float MinimumZoomScale
        {
            get { return (float)GetValue(MinimumZoomScaleProperty); }
            set { SetValue(MinimumZoomScaleProperty, value); }
        }

		public double InitialHeight
		{
			get;
			set;
		}

		public static readonly BindableProperty MaximumZoomScaleProperty = BindableProperty.Create("MaximumZoomScale", typeof(float), typeof(ZoomableScrollView), default(float));

        public float MaximumZoomScale
        {
            get { return (float)GetValue(MaximumZoomScaleProperty); }
            set { SetValue(MaximumZoomScaleProperty, value); }
        }

        public bool IsScalingUp { get; set; }
        public bool IsScalingDown { get; set; }

        public float ScaleFactor { get; set; } = 1;

        public static readonly BindableProperty TappedProperty = BindableProperty.CreateAttached("Tapped",
            typeof(Command<Point>), typeof(Gesture), null);

        public static Command<Point> GetCommand(BindableObject view)
        {
            return (Command<Point>)view.GetValue(TappedProperty);
        }

        public static void SetTapped(BindableObject view, Command<Point> value)
        {
            view.SetValue(TappedProperty, value);
        }
    }
}
