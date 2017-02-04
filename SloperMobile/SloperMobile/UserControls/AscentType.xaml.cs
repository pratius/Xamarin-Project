﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.UserControls
{
    public partial class AscentType : StackLayout
    {
        public AscentType()
        {
            InitializeComponent();
        }
                
        public void SetFrameColor(object sender, EventArgs e)
        {
            Onsight.BackgroundColor = Color.Black;
            Flash.BackgroundColor = Color.Black;
            Redpoint.BackgroundColor = Color.Black;
            Repeat.BackgroundColor = Color.Black;
            var ascframe = (Frame)sender;
            ascframe.BackgroundColor = Color.FromHex("#FF9933");
        }
    }
}