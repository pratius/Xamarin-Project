using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class PointsViewModel : BaseViewModel
    {
        public PointsViewModel(string date)
        {
            PointsDate = date;
        }


        private string pointsDate = " ";

        public string PointsDate
        {
            get { return pointsDate; }
            set { pointsDate = value; }
        }

        private int totalRoutes;

        public int TotalRoutes
        {
            get { return totalRoutes; }
            set { totalRoutes = value; OnPropertyChanged(); }
        }

        private int totalPoints;

        public int TotalPoints
        {
            get { return totalPoints; }
            set { totalPoints = value; OnPropertyChanged(); }
        }


        private ObservableCollection<ObservableGroupCollection<string, Model.Point>> pointsList;
        public ObservableCollection<ObservableGroupCollection<string, Model.Point>> PointsList
        {
            get { return pointsList; }
            set { pointsList = value; OnPropertyChanged(); }
        }

        private int pointsValues;

        public int PointsValues
        {
            get { return pointsValues; }
            set { pointsValues = value; OnPropertyChanged(); }
        }
        public async void OnPagePrepration()
        {
            try
            {
                await InvokeServiceGetPointsData();
                await InvokeServiceGetPointDetailsData();
                PageHeaderText = "PROFILE";
                PageSubHeaderText = "Points";
            }
            catch (Exception ex)
            {
                //throw ex;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task InvokeServiceGetPointsData()
        {
            HttpClientHelper apicall;
            if (string.IsNullOrWhiteSpace(PointsDate))
                apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetUserPoints, AppSetting.APP_ID), Settings.AccessTokenSettings);
            else
                apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetPoints, AppSetting.APP_ID, PointsDate), Settings.AccessTokenSettings);

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
                PointsList = new ObservableCollection<ObservableGroupCollection<string, Model.Point>>(reslist);
            }
        }

        private async Task InvokeServiceGetPointDetailsData()
        {
            HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetPointsDaily, AppSetting.APP_ID), Settings.AccessTokenSettings);


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
                PointsList = new ObservableCollection<ObservableGroupCollection<string, Model.Point>>(reslist);
            }
        }
    }
}
