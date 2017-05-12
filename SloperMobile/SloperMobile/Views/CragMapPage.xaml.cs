using Plugin.Geolocator;
using SloperMobile.Common.Helpers;
using SloperMobile.CustomControls;
using SloperMobile.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CragMapPage : ContentPage
    {
        iCustomMap theMap;
        public CragMapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            theMap = new iCustomMap();
            PutCragDataOnMap();
            // show slider only for iOS (useless in WP, not needed in Droid)
            if (Device.RuntimePlatform != Device.iOS)
            {
                theMap.SetValue(Grid.RowSpanProperty, 2);
            }
            Content = theMap;
            base.OnAppearing();
        }
        void PutCragDataOnMap()
        {
            T_CRAG selected_crag = new T_CRAG();
            var crags = App.DAUtil.GetCragList();
            foreach (T_CRAG tcrag in crags)
            {
                if (!string.IsNullOrEmpty(tcrag.crag_latitude) && !string.IsNullOrEmpty(tcrag.crag_longitude))
                {
                    SamplePlace p = new SamplePlace(tcrag.crag_name, "scenic_shot_portrait.png", Convert.ToDouble(tcrag.crag_latitude), Convert.ToDouble(tcrag.crag_longitude))
                    {
                        Address = tcrag.crag_general_info,
                        CragId = tcrag.crag_id
                    };

                    if (Settings.SelectedCragSettings == tcrag.crag_id)
                    {
                        theMap.AddCustomPin(p.CreateCustomPin("icon_pin_crag_current.png"));
                        selected_crag = tcrag;
                    }
                    else
                    {
                        theMap.AddCustomPin(p.CreateCustomPin("icon_pin_crag.png"));
                    }

                }
            }
            if (selected_crag != null)
            {
                theMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Convert.ToDouble(selected_crag.crag_latitude), Convert.ToDouble(selected_crag.crag_longitude)), Distance.FromKilometers(2.5)));
            }
            theMap.PropertyChanged += TheMap_PropertyChanged;
        }

        private void TheMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (zoomSlider.Minimum == 2.5f) //.BindingContext != null)
            //    return;

            if (e.PropertyName == "VisibleRegion" && theMap.VisibleRegion != null)
            {
                //zoomSlider.Maximum = 500.0f;
                //zoomSlider.Minimum = 1.5f;
                //zoomSlider.Value = 2.5f;
                //zoomSlider.ValueChanged += (s, ev) =>
                //{
                //    var value = ev.NewValue;
                //    theMap.MoveToRegion(MapSpan.FromCenterAndRadius(theMap.VisibleRegion.Center, Distance.FromKilometers(value)));
                //};
                //zoomSlider.BindingContext	= theMap.VisibleRegion.Radius.Kilometers;
                //zoomSlider.SetBinding<iCustomMap>(Slider.ValueProperty, o => o.VisibleRegion.Radius.Kilometers, BindingMode.TwoWay);
                theMap.PropertyChanged -= TheMap_PropertyChanged;
            }
        }
    }
}
