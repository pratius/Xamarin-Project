﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.Common.Helpers
{
    public class GradeNameToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString().Contains("Total"))
            {
                return Color.FromHex("#FF8E2D");
            }
            else
            {
                return Color.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Color.White;
        }
    }
}
