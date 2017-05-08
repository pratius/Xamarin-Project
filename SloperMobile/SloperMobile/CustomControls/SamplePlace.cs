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
    public class SamplePlace : XObjectNotifier
    {

        string _name,
                        _imageFile;
        string _address;
        double _latitude,
                        _longitude;
        string _description;
        bool _isPink = false;


        public SamplePlace() : base()
        {

        }

        public SamplePlace(string name, string imageFile) : base()
        {
            _name = name;
            _imageFile = imageFile;
        }

        public SamplePlace(string name, string imageFile, double latitude, double longitude) : base()
        {
            _name = name;
            _imageFile = imageFile;
            _latitude = latitude;
            _longitude = longitude;
        }

        public string MapPinText
        {
            get { return "• " + _address + "\n• " + _description; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;

                _name = value;
                NotifyPropertyChanged(() => Name);
                NotifyPropertyChanged(() => MapPinText);
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (value == _address)
                    return;

                _address = value;
                NotifyPropertyChanged(() => Address);
                NotifyPropertyChanged(() => MapPinText);
            }
        }


        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                    return;

                _description = value;
                NotifyPropertyChanged(() => Description);
                NotifyPropertyChanged(() => MapPinText);
            }
        }

        public string ImageFile
        {
            get { return string.IsNullOrEmpty(_imageFile) ? "" : Device.RuntimePlatform == Device.WinPhone ? "Assets/" + _imageFile : _imageFile; }
            set
            {
                if (value == _imageFile)
                    return;

                _imageFile = value;
                NotifyPropertyChanged(() => ImageFile);
            }
        }

        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (value == _latitude)
                    return;

                _latitude = value;
                NotifyPropertyChanged(() => Latitude);
                NotifyPropertyChanged(() => PlacePosition);
            }
        }

        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (value == _longitude)
                    return;

                _longitude = value;
                NotifyPropertyChanged(() => Longitude);
                NotifyPropertyChanged(() => PlacePosition);
            }
        }

        Position PlacePosition
        {
            get { return new Position(_latitude, _longitude); }
        }

        Pin CreatePlacePin()
        {
            return new Pin() { Address = _address, Label = _name, Position = PlacePosition, Type = PinType.Place };
        }

        public iCustomPin CreateCustomPin(string iconAssetFile)
        {
            return new iCustomPin(this, this.CreatePlacePin(), iconAssetFile);
        }

        public bool IsPink
        {
            get { return _isPink; }
            set
            {
                if (value == _isPink)
                    return;

                _isPink = value;
                NotifyPropertyChanged(() => IsPink);
            }
        }
    }
}
