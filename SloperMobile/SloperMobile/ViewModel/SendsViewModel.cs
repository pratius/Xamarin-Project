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

        private string tabBackgroundColor;

        public string TabBackgroundColor
        {
            get { return tabBackgroundColor; }
            set { tabBackgroundColor = value; OnPropertyChanged(); }
        }

        private async void OnPagePrepration()
        {
            await InvokeServiceGetAscentData();
        }


        private void ExecuteOnTabSelection(object obj)
        {

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
            }

        }
        #endregion

    }
}
