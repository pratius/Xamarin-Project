using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class CalendarViewModel : BaseViewModel
    {
        #region Constructor
        public CalendarViewModel()
        {
            HeaderMonth = DateTime.Today.ToString("MMMM yyyy").ToUpper();
            CalendarModel = new CalendarModel();
            OnPagePrepration();
        }
        #endregion

        #region Properties
        private CalendarEventCollection _showDates;
        public CalendarEventCollection ShowDates
        {
            get { return _showDates; }
            set { _showDates = value; OnPropertyChanged("ShowDates"); }
        }

        private CalendarModel _calendarModel;
        public CalendarModel CalendarModel
        {
            get { return _calendarModel; }
            set { _calendarModel = value; OnPropertyChanged(); }
        }

        private List<DateTime> _selectedDates;
        public List<DateTime> SelectedDates
        {
            get { return _selectedDates; }
            set { _selectedDates = value; OnPropertyChanged(); }
        }

        private string _headerMonth;
        public string HeaderMonth
        {
            get { return _headerMonth; }
            set { _headerMonth = value; OnPropertyChanged(); }
        }
        #endregion

        private async void OnPagePrepration()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...", Acr.UserDialogs.MaskType.Black);

                PageHeaderText = "PROFILE";
                PageSubHeaderText = "Calendar";

                await InvokeServiceGetAscentDates();

                Acr.UserDialogs.UserDialogs.Instance.Loading().Hide();
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
                var Collection = new CalendarEventCollection();
                if (response != null)
                {
                    foreach (var singleDate in response)
                    {
                        Collection.Add(new CalendarInlineEvent()
                        {
                            StartTime = Convert.ToDateTime(singleDate.date_climbed),
                            EndTime = Convert.ToDateTime(singleDate.date_climbed),
                            Color = Color.DarkOrange
                        });
                    }
                }

                ShowDates = Collection;
            }
            catch (Exception ex)
            {
                throw ex;
                Acr.UserDialogs.UserDialogs.Instance.Loading().Hide();
            }
        }
    }
}
