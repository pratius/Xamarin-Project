using SloperMobile.Model;
using System;
using System.Linq;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public static class Gesture
    {
        public static readonly BindableProperty TappedProperty = BindableProperty.CreateAttached("Tapped",
            typeof(Command<PointWithId>), typeof(Gesture), null, propertyChanged: CommandChanged);

        public static Command<PointWithId> GetCommand(BindableObject view)
        {
            return (Command<PointWithId>)view.GetValue(TappedProperty);
        }

        public static void SetTapped(BindableObject view, Command<PointWithId> value)
        {
            view.SetValue(TappedProperty, value);
        }

        private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view != null)
            {
                var effect = GetOrCreateEffect(view);
            }
        }

        private static GestureEffect GetOrCreateEffect(View view)
        {
            try
            {

                var effect = (GestureEffect)view.Effects.FirstOrDefault(e => e is GestureEffect);
                if (effect != null)
                {
                    return effect;
                }

                effect = new GestureEffect();
                view.Effects.Add(effect);
                return effect;
            }
            catch (Exception e)
            {
                //System.Console.WriteLine(e);

            }

            return null;
        }
    }

    public class GestureEffect : RoutingEffect
    {
        public GestureEffect() : base("SloperSoftware.TapGestureWithPointEffect")
        {
        }
    }
}
