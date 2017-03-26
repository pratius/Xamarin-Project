using SloperMobile.DataBase;
using SloperMobile.ViewModel;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SloperMobile.Common.Helpers;
using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Model;
using SloperMobile.MessagingTask;

namespace SloperMobile
{
    public partial class App : Application
    {
        static DataAccess dbUtils;
        private static string _selectedcrag;
        public App()
        {
            InitializeAppStep1();
        }

        public static string SelectedCrag
        {
            get
            {
                if (_selectedcrag == null)
                {
                    var menuDetails = App.DAUtil.GetCragList();
                    if (menuDetails.Count > 0)
                    {
                        var selectedItems = menuDetails?.FirstOrDefault();
                        _selectedcrag = selectedItems.crag_id;
                    }
                }
                return _selectedcrag;
            }
            set { _selectedcrag = value; }
        }
        public static DataAccess DAUtil
        {
            get
            {
                if (dbUtils == null)
                {
                    dbUtils = new DataAccess();
                }
                return dbUtils;
            }
        }


        protected override void OnStart()
        {
            // Handle when your app starts
            try
            {
                var IsAppinitialized = DAUtil.CheckAppInitialization();
                if (IsAppinitialized)
                {
                    var message = new StartCheckForUpdatesTask();
                    MessagingCenter.Send(message, "StartCheckForUpdatesTaskMessage");
                    HandleReceivedMessages();
                }
            }
            catch
            {
                //need to implement network availability.
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            try
            {
                var IsAppinitialized = DAUtil.CheckAppInitialization();
                if (IsAppinitialized)
                {
                    var message = new StartCheckForUpdatesTask();
                    MessagingCenter.Send(message, "StartCheckForUpdatesTaskMessage");
                    HandleReceivedMessages();
                }
            }
            catch
            {
                //need to implement network availability.
            }
        }
        void InitializeAppStep1()
        {
            InitializeComponent();
            var IsAppinitialized = DAUtil.CheckAppInitialization();
            if (IsAppinitialized)
            {
                if (string.IsNullOrEmpty(Settings.AccessTokenSettings))
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    MainPage = new NavigationPage(new MenuNavigationPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new SplashPage());
            }
        }

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<UpdateMessage>(this, "UpdateMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Info", message.Message, "ok");
                });
            });
        }
    }
}
