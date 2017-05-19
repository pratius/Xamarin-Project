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
                             orderby Convert.ToDateTime(point.date_climbed) descending
                             group point by (point.date_climbed) into pointGroup
                             select new ObservableGroupCollection<string, Model.Point>(pointGroup.Key.ToString(), pointGroup);

                var reslist = sorted.ToList();

                for (int i = 0; i < reslist.Count; i++)
                {
                    int pts = 0;
                    var obj = reslist[i];
                    Model.Point newpoint = new Model.Point();
                    for (int j = 0; j < obj.Count; j++)
                    {
                        pts += Convert.ToInt32(obj[j].points);
                    }
                    newpoint.date_climbed = obj[0].date_climbed;
                    newpoint.route_name = "";
                    newpoint.tech_grade = "Total";
                    newpoint.points = pts;
                    obj.Add(newpoint);
                    reslist[i] = obj;
                }
                BindingContext = new ObservableCollection<ObservableGroupCollection<string, Model.Point>>(reslist);
            }
        }

        private void lstViewPoint_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.Item == null)
            {
                return;
            }
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null;
            // de-select the row after ripple effect
        }
    }
}