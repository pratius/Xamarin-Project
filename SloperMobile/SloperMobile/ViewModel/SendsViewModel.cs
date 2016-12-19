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
            PageHeaderText = "Sends";
            await InvokeServiceGetAscentData();
        }


        private void ExecuteOnTabSelection(object obj)
        {

        }

        private void SetChartValue()
        {
            if (SendsList.Count > 0)
            {
                foreach (var item in SendsList)
                {
                    if (item.Ascent_Type_Id == 1)
                        Onsight = item.Ascent_Type_Id / SendsList.Count;
                    else if (item.Ascent_Type_Id == 3)
                        Redpoint = item.Ascent_Type_Id / SendsList.Count;
                    else if (item.Ascent_Type_Id == 6)
                        Projects = item.Ascent_Type_Id / SendsList.Count;
                }
            }
        }

        #region Service Methods
        private async Task InvokeServiceGetAscentData()
        {
            HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetAscent_AppData, 20160425, 20161030, "true", "json", "(withCredentials:false)", "application/json"), Cache.AccessToken);
            Dictionary<string, string> dictquery = new Dictionary<string, string>();
            var response = await apicall.Get<Send>();
            if (response.Count > 0)
            {
                SendsList = new ObservableCollection<Send>(response);
                SetChartValue();
            }

        }
        #endregion

    }
}
