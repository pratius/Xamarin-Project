//using SloperMobile.UserControls.CustomMap;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SloperMobile.CustomControls
//{
    //public class iCustomMap : Xamarin.Forms.Maps.Map
    //{
    //    iMapPinInfoCtrl _pinInfoControl = new iMapPinInfoCtrl();    // {  IsVisible = false };

    //    public iCustomMap() : base()
    //    {
    //    }

    //    public iCustomMap(MapSpan mapSpan) : base(mapSpan)
    //    {
    //    }

    //    public iMapPinInfoCtrl PinInfoPanel
    //    {
    //        get { return _pinInfoControl; }
    //    }


    //    public void AddCustomPin(iCustomPin customPin)
    //    {
    //        if (customPin == null)
    //            return;

    //        base.Pins.Add(new Pin() { BindingContext = customPin, Label = customPin.MapPinLabel, Position = customPin.MapPinPosition, Type = customPin.MapPinType });
    //    }

    //    protected override void OnBindingContextChanged()
    //    {
    //        base.OnBindingContextChanged();
    //    }

    //    public iCustomPin GetPinAtPosition(Position pos)
    //    {
    //        if (Pins == null || pos == null)
    //            return null;

    //        Pin pin = Pins.FirstOrDefault(p => p.Position.Latitude == pos.Latitude && p.Position.Longitude == pos.Longitude);

    //        return pin == null ? null : pin.BindingContext as iCustomPin;
    //    }
    //}
//}
