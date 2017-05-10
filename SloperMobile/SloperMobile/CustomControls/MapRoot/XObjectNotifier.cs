using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace SloperMobile.CustomControls.MapRoot
{
    [DataContract(Namespace = "")]
    public abstract class XObjectNotifier : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            if (property == null || PropertyChanged == null)
                return;

            string name = GetPropertyName(property);

            if (string.IsNullOrEmpty(name))
                return;

            NotifyPropertyChanged(name);
        }

        protected void NotifyPropertyChanged(string property_name)
        {
            if (PropertyChanged == null)    //|| string.IsNullOrEmpty( property_name))	// note: raising NotifyPropertyChanged(null) = notify all ptoperties changed
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property_name));
        }

        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> property)
        {
            if (property == null)
                return null;

            var expression = property.Body as MemberExpression;

            if (expression == null || expression.Member == null)
                return null;

            return expression.Member.Name;

        }

    }
}
