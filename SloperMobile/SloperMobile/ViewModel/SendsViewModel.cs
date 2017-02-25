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

namespace SloperMobile.ViewModel
{
    public class SendsViewModel : BaseViewModel
    {
        public SendsViewModel()
        {
            OnPagePrepration();

        }

        private ObservableCollection<Send> sendsList;

        public ObservableCollection<Send> SendsList
        {
            get { return sendsList; }
            set { sendsList = value; OnPropertyChanged(); }
        }

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
            set { projects = value; }
        }


        private string tabBackgroundColor;

        public string TabBackgroundColor
        {
            get { return tabBackgroundColor; }
            set { tabBackgroundColor = value; OnPropertyChanged(); }
        }

        private async void OnPagePrepration()
        {
            PageHeaderText = "SENDS";
            await InvokeServiceGetAscentData();
        }


        private void ExecuteOnTabSelection(object obj)
        {

        }

        private void SetChartValue()
        {
            int _onsight = 0, _redpoint = 0;
            if (SendsList.Count > 0)
            {
                foreach (var item in SendsList)
                {
                    if (item.Ascent_Type_Id == 1)
                        _onsight++;
                    else if (item.Ascent_Type_Id == 3)
                        _redpoint++;
                    else if (item.Ascent_Type_Id == 6)
                        Projects++;
                }
                Onsight = (int)Math.Round((double)(100 * _onsight) / SendsList.Count);
                Redpoint = (int)Math.Round((double)(100 * _redpoint) / SendsList.Count);
            }
        }

        #region Service Methods
        private async Task InvokeServiceGetAscentData()
        {
            HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_GetAscent_AppData, Settings.AccessTokenSettings);
            SendsDTO sendsobj = new SendsDTO();
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
        #endregion


    }
}
