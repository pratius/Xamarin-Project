using SloperMobile.Common.Constants;
using SloperMobile.Common.Enumerators;
using SloperMobile.Common.Helpers;
using SloperMobile.CustomControls.MapRoot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SloperMobile.CustomControls
{
    public class iCustomPin : XObjectNotifier
    {
        public event EventHandler Clicked;

        Pin _mapPin;
        string _pinIconAsset;
        object _dataObject;

        public iCustomPin() : base()
        {

        }

        public iCustomPin(object dataObject, Pin mapPin, string iconAsset) : base()
        {
            _mapPin = mapPin;
            _dataObject = dataObject;
            _pinIconAsset = iconAsset;
        }

        public async void RaiseClickEvent()
        {
            try
            {
                SamplePlace place = this.DataObject as SamplePlace;
                if (place != null)
                {
                    Settings.SelectedCragSettings = place.CragId;
                    //await Application.Current.MainPage.Navigation.PushAsync(new Views.HomePage("MapPage"));
                    //await Application.Current.MainPage.Navigation.PushAsync(new Views.MapPage());
                    await Application.Current.MainPage.Navigation.PushAsync(new Views.CragDetailsPage());
                }
                Clicked?.Invoke(this, new EventArgs());
            }
            catch
            {
            }
            //if (Clicked != null)
            //    Clicked.Invoke(this, new EventArgs());
        }


        public PinType MapPinType
        {
            get { return _mapPin == null ? PinType.Generic : _mapPin.Type; }
        }

        public Position MapPinPosition
        {
            get { return _mapPin == null ? new Position() : _mapPin.Position; }
        }

        public string MapPinLabel
        {
            get { return _mapPin == null ? "" : _mapPin.Label; }
        }

        public Pin MapPin
        {
            get { return _mapPin; }
            set
            {
                if (value == _mapPin)
                    return;

                _mapPin = value;
                NotifyPropertyChanged(() => MapPin);
                NotifyPropertyChanged(() => MapPinLabel);
                NotifyPropertyChanged(() => MapPinPosition);
                NotifyPropertyChanged(() => MapPinType);
            }
        }


        /// <summary>
        /// the icon asset file name
        /// </summary>
        public string IconResource
        {
            get { return _pinIconAsset; }
            set
            {
                if (value == _pinIconAsset)
                    return;

                _pinIconAsset = value;
                NotifyPropertyChanged(() => IconResource);
            }
        }

        /// <summary>
        /// get/set put here whatever object you would like to handle
        /// will be passed to the renderer
        /// </summary>
        public object DataObject
        {
            get { return _dataObject; }
            set
            {
                if (value == _dataObject)
                    return;

                _dataObject = value;
                NotifyPropertyChanged(() => DataObject);
            }
        }
    }
}
