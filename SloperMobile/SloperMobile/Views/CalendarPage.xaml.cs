using SloperMobile.ViewModel;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SloperMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        int i = 0;
        bool isMonthChanged = true;
        DateTime month = new DateTime();
        Label lblDateText, lblAppointment;
        private CalendarViewModel _calendarVM;
        public CalendarPage()
        {
            //Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _calendarVM = new CalendarViewModel();
            BindingContext = _calendarVM;

            CalendarHeader.Text = DateTime.Now.ToString("Y").ToUpper();
            calendar.MonthChanged += HandleMonthChangedEventHandler;
            calendar.OnMonthCellLoaded += Cal_OnMonthCellLoaded;

            MonthViewSettings monthViewSettings = new MonthViewSettings();
            monthViewSettings.BorderColor = Color.Black;
            monthViewSettings.CurrentMonthBackgroundColor = Color.Black;
            monthViewSettings.CurrentMonthTextColor = Color.White;
            monthViewSettings.PreviousMonthBackgroundColor = Color.Black;
            monthViewSettings.PreviousMonthTextColor = Color.FromHex("#333333");
            monthViewSettings.DayHeaderBackgroundColor = Color.Black;
            monthViewSettings.DayHeaderTextColor = Color.White;
            monthViewSettings.DateSelectionColor = Color.Black;
            monthViewSettings.TodayTextColor = Color.Black;
            monthViewSettings.SelectedDayTextColor = Color.White;

            calendar.MonthViewSettings = monthViewSettings;

            //Acr.UserDialogs.UserDialogs.Instance.Loading().Hide();
        }
        void Cal_OnMonthCellLoaded(object sender, MonthCell args)
        {
            if (Device.RuntimePlatform == Device.iOS)
                month = args.Date.AddDays(-21);
            else if (Device.RuntimePlatform == Device.Android)
            {
                bool call = false;
                for (int i = 0; i < calendar.DataSource.Count; i++)
                {
                    if ((args.Date.Date.CompareTo(calendar.DataSource[i].StartTime.Date) == 0 ||
                        args.Date.Date.CompareTo(calendar.DataSource[i].StartTime.Date) == 1) &&
                        (args.Date.Date.CompareTo(calendar.DataSource[i].EndTime.Date) == 0 ||
                        args.Date.Date.CompareTo(calendar.DataSource[i].EndTime.Date) == -1))
                    {
                        call = true;
                    }
                }
                i++;
                string str = args.Date.ToString("dd");
                if (i == 1)
                    month = args.Date.AddDays(21);
                if (call)
                {
                    if (month.Month == args.Date.Month)
                    {
                        lblDateText = new Label()
                        {
                            Text = str,
                            FontSize = 15,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White,
                            Margin = new Thickness(0, 20, 0, 0)
                        };
                        lblAppointment = new Label()
                        {
                            Text = "_",
                            FontSize = 40,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.End,
                            TextColor = Color.FromHex("#FF8E2D"),
                        };
                    }
                    else
                    {
                        lblDateText = new Label()
                        {
                            Text = str,
                            FontSize = 15,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.FromHex("#333333"),
                            Margin = new Thickness(0, 20, 0, 0)
                        };
                        lblAppointment = new Label()
                        {
                            Text = "_",
                            FontSize = 40,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.End,
                            TextColor = Color.FromHex("#803c00")
                        };
                    }
                    if (DateTime.Today.ToString("MM/dd/yyyy") == args.Date.ToString("MM/dd/yyyy"))
                        lblDateText.TextColor = Color.FromHex("#FF8E2D");

                    Grid DynamicGrid = new Grid();
                    DynamicGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    DynamicGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) });
                    DynamicGrid.Children.Add(lblDateText, 0, 0);
                    DynamicGrid.Children.Add(lblAppointment, 0, 1);
                    args.View = DynamicGrid;
                }
                else
                {
                    if (month.Month == args.Date.Month)
                        lblDateText = new Label()
                        {
                            Text = str,
                            FontSize = 15,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White
                        };
                    else
                        lblDateText = new Label()
                        {
                            Text = str,
                            FontSize = 15,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.FromHex("#333333")
                        };

                    if (DateTime.Today.ToString("MM/dd/yyyy") == args.Date.ToString("MM/dd/yyyy"))
                        lblDateText.TextColor = Color.FromHex("#FF8E2D");

                    args.View = lblDateText;
                }
                if (i == 42)
                    i = 0;
            }
        }
        void HandleMonthChangedEventHandler(object sender, MonthChangedEventArgs args)
        {
            CalendarHeader.Text = args.args.CurrentValue.ToString("Y").ToUpper();
        }
        private async void SfCalendar_OnOnCalendarTapped(object sender, CalendarTappedEventArgs args)
        {
            //Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
            CalendarEventCollection events = args.Calendar.DataSource;
            if (events != null)
            {
                foreach (var singleEvent in events)
                {
                    if (args.datetime.ToString("yyyy/MM/dd").Equals(singleEvent.StartTime.ToString("yyyy/MM/dd"))
                        && args.datetime.Month == DateTime.Today.Month)
                    {
                        await Navigation.PushAsync(new PointsPage(args.datetime.ToString("yyyyMMdd")));
                        break;
                    }
                }
            }
            //Acr.UserDialogs.UserDialogs.Instance.Loading().Hide();
        }
    }
}