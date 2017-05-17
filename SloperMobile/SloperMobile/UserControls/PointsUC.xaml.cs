using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SloperMobile.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PointsUC : ContentView
    {
        public PointsUC()
        {
            InitializeComponent();

            HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetUserPoints, AppSetting.APP_ID), Settings.AccessTokenSettings);
            var response = apicall.Get<Model.Point>();
            if (response.Result.Count > 0)
            {
                var sorted = from point in response.Result
                             orderby point.date_climbed descending
                             group point by point.date_climbed into pointGroup
                             select new ObservableGroupCollection<string, Model.Point>(pointGroup.Key.ToString(), pointGroup);
                BindingContext = new ObservableCollection<ObservableGroupCollection<string, Model.Point>>(sorted);                
            }
        }
    }
}