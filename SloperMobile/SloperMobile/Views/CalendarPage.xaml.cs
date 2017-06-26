﻿using SloperMobile.ViewModel;
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
        int count = 0;
        DateTime month = new DateTime();
        //bool isMonthChanged = true;
        Label lbl;//,lbl1;
        private CalendarViewModel _calendarVM;
        public CalendarPage()
        {
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
            monthViewSettings.PreviousMonthTextColor = Color.Black;
            monthViewSettings.DayHeaderBackgroundColor = Color.Black;
            monthViewSettings.DayHeaderTextColor = Color.White;
            monthViewSettings.DateSelectionColor = Color.Silver;
            monthViewSettings.SelectedDayTextColor = Color.White;
            monthViewSettings.TodayTextColor = Color.White;

            calendar.MonthViewSettings = monthViewSettings;
        }
        void Cal_OnMonthCellLoaded(object sender, MonthCell args)
        {
            if (Device.RuntimePlatform == "Android")
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
                count++;
                string str = args.Date.ToString("dd");
                if (count == 1)
                {
                    month = args.Date.AddDays(21);
                }
                if (call)
                {
                    if (month.Month == args.Date.Month)
                    {
                        lbl = new Label() { Text = str, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                        //lbl1 = new Label() { Text = "_", FontSize = 40, HorizontalTextAlignment = TextAlignment.Center,
                        //    VerticalTextAlignment = TextAlignment.End, TextColor = Color.White };
                    }
                    Grid DynamicGrid = new Grid();
                    DynamicGrid.HorizontalOptions = LayoutOptions.Center;
                    DynamicGrid.VerticalOptions = LayoutOptions.Center;
                    RowDefinition gridRow1 = new RowDefinition();
                    gridRow1.Height = new GridLength(29);
                    RowDefinition gridRow2 = new RowDefinition();
                    gridRow2.Height = new GridLength(20);
                    RowDefinition gridRow3 = new RowDefinition();
                    gridRow3.Height = new GridLength(20);
                    DynamicGrid.RowDefinitions.Add(gridRow1);
                    DynamicGrid.RowDefinitions.Add(gridRow2);
                    DynamicGrid.RowDefinitions.Add(gridRow3);
                    Grid.SetRow(lbl, 1);
                    //Grid.SetRow(lbl1, 2);
                    //DynamicGrid.Children.Add(lbl1);
                    DynamicGrid.Children.Add(lbl);
                    args.View = DynamicGrid;
                }
                else
                {
                    if (month.Month == args.Date.Month)
                        lbl = new Label() { Text = str, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                    else
                        lbl = new Label() { Text = str, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center, TextColor = Color.Black };
                    args.View = lbl;
                }
                if (count == 42)
                {
                    count = 0;
                }

            }
        }
        void HandleMonthChangedEventHandler(object sender, MonthChangedEventArgs args)
        {
            CalendarHeader.Text = args.args.CurrentValue.ToString("Y").ToUpper();
        }
        private async void SfCalendar_OnOnCalendarTapped(object sender, CalendarTappedEventArgs args)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
            CalendarEventCollection events = args.Calendar.DataSource;
            foreach(var singleEvent in events)
            {
                if(args.datetime.ToString("yyyy/MM/dd").Equals(singleEvent.StartTime.ToString("yyyy/MM/dd")))
                {
                    await Navigation.PushAsync(new PointsPage(args.datetime.ToString("yyyyMMdd")));
                    break;
                }
            }
            Acr.UserDialogs.UserDialogs.Instance.Loading().Hide();
        }
    }
}