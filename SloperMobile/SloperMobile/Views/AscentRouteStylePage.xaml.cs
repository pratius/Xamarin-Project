﻿using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class AscentRouteStylePage : ContentPage
    {
        public AscentRouteStylePage()
        {
            InitializeComponent();
            BindingContext = AscentProcessViewModel.CurrentInstance(Navigation);

        }
    }
}