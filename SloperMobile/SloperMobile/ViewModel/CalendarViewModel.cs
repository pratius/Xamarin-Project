using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;

namespace SloperMobile.ViewModel
{
    public class CalendarViewModel : BaseViewModel
    {
        #region Constructor
        public CalendarViewModel()
        {
            CalendarModel = new CalendarModel();
            OnPagePrepration();
        }
        #endregion

        #region Properties
        private CalendarModel _calendarModel;
        public CalendarModel CalendarModel
        {
            get { return _calendarModel; }
            set { _calendarModel = value;OnPropertyChanged(); }
        }

        private List<DateTime> _selectedDates;
        public List<DateTime> SelectedDates
        {
            get { return _selectedDates; }
            set { _selectedDates = value; OnPropertyChanged(); }
        }
        #endregion

        private async void OnPagePrepration()
        {
            try
            {
                PageHeaderText = "PROFILE";
                PageSubHeaderText = "Calendar";
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
                await InvokeServiceGetAscentDates();
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        private async Task InvokeServiceGetAscentDates()
        {
            try
            {
                HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_GetAscentDates, Settings.AccessTokenSettings);
                CalendarModel.app_id = AppSetting.APP_ID;
                CalendarModel.start_date = "20160101";
                CalendarModel.end_date = "20300101";
                string calendarjson = JsonConvert.SerializeObject(CalendarModel);
                var response = await apicall.Post<CalendarResponse[]>(calendarjson);
                List<DateTime> localDates = new List<DateTime>();
                if(response!=null)
                {
                    foreach(var singleDate in response)
                    {
                        DateTime dt = Convert.ToDateTime(singleDate.date_climbed);
                        localDates.Add(dt);
                    }
                    SelectedDates = localDates;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }
    }
}
