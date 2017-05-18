﻿using Newtonsoft.Json;
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
    public class SendsViewModel : BaseViewModel
    {
        private INavigation _navigation;
        public SendsViewModel(string TabName, INavigation navigation)
        {
            _navigation = navigation;
            OnPagePrepration(TabName);
            //UserPoints = new List<Model.PointList>();
        }

        private ObservableCollection<Send> sendsList;
        private ObservableCollection<TickList> ticklistsList;
        private ObservableCollection<Model.Point> pointssList;
        //private List<PointList> _userPoint;

        public ObservableCollection<Send> SendsList
        {
            get { return sendsList; }
            set { sendsList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TickList> TickListsList
        {
            get { return ticklistsList; }
            set { ticklistsList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ObservableGroupCollection<string, Model.Point>> PointsGrouped { get; set; }
        public ObservableCollection<Model.Point> PointList
        {
            get { return pointssList; }
            set { pointssList = value; OnPropertyChanged(); }
        }
        //public List<PointList> UserPoints
        //{
        //    get { return _userPoint; }
        //    set { _userPoint = value; OnPropertyChanged(); }
        //}

        private int onsight;

        public int Onsight
        {
            get { return onsight; }
            set { onsight = value; OnPropertyChanged(); }
        }

        private int redpoint;

        public int Redpoint
        {
            get { return redpoint; }
            set { redpoint = value; OnPropertyChanged(); }
        }

        private int projects;

        public int Projects
        {
            get { return projects; }
            set { projects = value; OnPropertyChanged(); }
        }


        private string tabBackgroundColor;

        public string TabBackgroundColor
        {
            get { return tabBackgroundColor; }
            set { tabBackgroundColor = value; OnPropertyChanged(); }
        }

        private DateTime date_Created;
        public DateTime Date_Created
        {
            get { return date_Created; }
            set
            {
                date_Created = value;
                DateCreated = value.ToString("MM/dd/yy");
            }
        }
        private string dateCreated;

        public string DateCreated
        {
            get { return dateCreated; }
            set
            {
                dateCreated = value; OnPropertyChanged();

            }
        }



        private string routename;
        public string route_name
        {
            get { return routename; }
            set { routename = value; OnPropertyChanged(); }
        }
        private string gradename;
        public string grade_name
        {
            get { return gradename; }
            set { gradename = value; OnPropertyChanged(); }
        }
        private async void OnPagePrepration(string TabName)
        {
            try
            {
                if (TabName == "SENDS")
                {
                    PageHeaderText = "PROFILE";
                    PageSubHeaderText = "Sends";
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
                    await InvokeServiceGetAscentData();
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
                else if (TabName == "POINTS")
                {
                    PageHeaderText = "PROFILE";
                    PageSubHeaderText = "Points";
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
                   // await InvokeServiceGetPointsData();
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
                else
                {
                    PageHeaderText = "PROFILE";
                    PageSubHeaderText = "Tick List";
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
                    await InvokeServiceGetTickListData();
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }


            catch (Exception ex)
            {
                //throw ex;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }



        private void ExecuteOnTabSelection(object obj)
        {

        }

        private void SetChartValue()
        {
            int _onsight = 0, _redpoint = 0, _project = 0;
            if (SendsList.Count > 0)
            {
                foreach (var item in SendsList)
                {
                    if (item.Ascent_Type_Id == 1)
                        _onsight++;
                    else if (item.Ascent_Type_Id == 3)
                        _redpoint++;
                    else if (item.Ascent_Type_Id == 6)
                        _project++;
                }
                Onsight = (int)Math.Round((double)(100 * _onsight) / SendsList.Count);
                Redpoint = (int)Math.Round((double)(100 * _redpoint) / SendsList.Count);
                Projects = _project;
            }
        }

        #region Service Methods
        private async Task InvokeServiceGetAscentData()
        {
            try
            {
                HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_GetAscent_AppData, Settings.AccessTokenSettings);
                SendsDTO sendsobj = new SendsDTO();
                sendsobj.app_id = AppSetting.APP_ID;
                sendsobj.start_date = "20160101";
                sendsobj.end_date = "20300101";
                string sendsjson = JsonConvert.SerializeObject(sendsobj);
                var response = await apicall.Post<List<Send>>(sendsjson);
                if (response.Count > 0)
                {
                    SendsList = new ObservableCollection<Send>(response);
                    SetChartValue();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task InvokeServiceGetTickListData()
        {
            try
            {
                HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetTick_ListData, Settings.SelectedCragSettings), Settings.AccessTokenSettings);
                var response = await apicall.Get<TickList>();
                if (response.Count > 0)
                {
                    TickListsList = new ObservableCollection<TickList>(response);
                    if (TickListsList.Count > 0)
                    {
                        foreach (var item in TickListsList)
                        {
                            route_name = item.route_name;
                            grade_name = item.grade_name;
                            DateCreated = item.Date_Created.ToString("MM/dd/yy");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task InvokeServiceGetPointsData()
        {
            try
            {
                HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetUserPoints, AppSetting.APP_ID), Settings.AccessTokenSettings);
                var response = await apicall.Get<Model.Point>();
                if (response.Count > 0)
                {
                    //PointList = new ObservableCollection<Model.Point>(response);

                    //var groupedData = response.OrderByDescending(e => e.date_climbed).GroupBy(e => e.route_name[0].ToString()).Select(e => new ObservableGroupCollection<string, Model.Point>(e)).ToList();

                    //PointsGrouped = new ObservableCollection<ObservableGroupCollection<string, Model.Point>>(groupedData);

                    var sorted = from point in response  orderby point.date_climbed descending group point by point.date_climbed into pointGroup
                                 select new ObservableGroupCollection<string, Model.Point>(pointGroup.Key.ToString(), pointGroup);
                    PointsGrouped = new ObservableCollection<ObservableGroupCollection<string, Model.Point>>(sorted);

                    //var res = response.GroupBy(pts => pts.date_climbed).Select(grp => grp.ToList()).ToList();
                    //foreach(List<Model.Point> li in res)
                    //{
                    //    Model.PointList pl = new Model.PointList();
                    //    pl.UsersPoint = li;
                    //    UserPoints.Add(pl);
                    //}

                }
            }
            catch (Exception ex)
            {
                //throw ex;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        #endregion


    }
}
