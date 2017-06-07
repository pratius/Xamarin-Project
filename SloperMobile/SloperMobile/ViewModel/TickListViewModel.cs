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
    public class TickListViewModel : BaseViewModel
    {
       
        public TickListViewModel()
        {
          
            OnPagePrepration();
            //UserPoints = new List<Model.PointList>();
        }
        private ObservableCollection<Send> sendsList;
        private ObservableCollection<TickList> ticklistsList;

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

        private async void OnPagePrepration()
        {
            try
            {

                PageHeaderText = "PROFILE";
                PageSubHeaderText = "Tick List";
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
                await InvokeServiceGetTickListData();
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();

            }


            catch (Exception ex)
            {
                //throw ex;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
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
    }
}
