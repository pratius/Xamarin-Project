using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class SendsPage : ContentPage
    {
        #region Properties

        public new event PropertyChangedEventHandler PropertyChanged;

        #region PointerValue

        private double pointerValue = 60;

        public double PointerValue
        {
            get { return pointerValue; }
            set
            {
                pointerValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("PointerValue"));
                }
            }
        }

        #endregion PointerValue

        #endregion Properties
        public SendsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new SendsViewModel();
            circularGauge.BindingContext = this;
           // pointer_slider.BindingContext = this;

            #region Conditions



            //Label Offset
            if (Device.OS == TargetPlatform.Android)
            {
                scale.LabelOffset = 0.2;
            }
            else
            {
                scale.LabelOffset = 0.1;
            }

            #region Pointer Slider

            //PointerSlider label FontSize
            if (Device.OS == TargetPlatform.iOS)
            {
                ///pointer_text.FontSize = 15;
            }
            else if (Device.OS == TargetPlatform.Android)
            {
             //   pointer_text.FontSize = 15;
            }
            else
            {
               // pointer_text.FontSize = 20;
            }

            //PointerSlider BackgroundColor
            if (Device.OS == TargetPlatform.WinPhone)
            {
              //  pointer_slider.BackgroundColor = Color.Gray;
            }

            #endregion Pointer Slider

            #endregion Conditions            
        }

        #region Events

        //main_layout SizeChanged
        void main_layout_SizeChanged(object sender, EventArgs e)
        {
            circularGauge.WidthRequest = 330;
            circularGauge.HeightRequest = 330;
        }

        #endregion Events
    }
}
