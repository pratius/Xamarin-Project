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
        public CalendarPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new CalendarViewModel();

            MonthLabelSettings labelSettings = new MonthLabelSettings();
            labelSettings.BackgroundColor = Color.Black;

            MonthViewSettings monthViewSettings = new MonthViewSettings();
            monthViewSettings.BorderColor = Color.Black;
            monthViewSettings.CurrentMonthBackgroundColor = Color.Black;
            monthViewSettings.CurrentMonthTextColor = Color.White;
            monthViewSettings.PreviousMonthBackgroundColor = Color.Black;
            monthViewSettings.PreviousMonthTextColor = Color.White;
            monthViewSettings.DayHeaderBackgroundColor = Color.Black;
            monthViewSettings.DayHeaderTextColor = Color.White;
            monthViewSettings.HeaderBackgroundColor = Color.Black;
            monthViewSettings.HeaderTextColor = Color.White;
            monthViewSettings.SelectedDayTextColor = Color.White;
            monthViewSettings.TodayTextColor = Color.FromHex("#FF8E2D");

            monthViewSettings.MonthLabelSettings = labelSettings;
            calendar.MonthViewSettings = monthViewSettings;
        }
    }
}