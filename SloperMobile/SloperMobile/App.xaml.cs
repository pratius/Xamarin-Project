using System;
using System.Linq;
using Plugin.Connectivity;
using SloperMobile.Common.Helpers;
using SloperMobile.DataBase;
using SloperMobile.MessagingTask;
using SloperMobile.Views;
using Xamarin.Forms;

namespace SloperMobile
{
    public partial class App : Application
    {
        static DataAccess dbUtils;
        private static string _selectedcrag;
        public App()
        {
            InitializeAppStep1();
            SetMainPage += (page) =>
            {
                MainPage = new NavigationPage(page);
            };
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
                if (CrossConnectivity.Current.IsConnected)
                {
                    var IsAppinitialized = DAUtil.CheckAppInitialization();
                    if (IsAppinitialized)
                    {
                        var message = new StartCheckForUpdatesTask();
                        MessagingCenter.Send(message, "StartCheckForUpdatesTaskMessage");
                        HandleReceivedMessages();
                    }
                }

            }
            catch
            {
                //need to implement network availability.
                Application.Current.MainPage.Navigation.PushAsync(new Views.NetworkErrorPage());
            }
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }
        protected override void OnResume()
        {
            // Handle when your app resumes

            if (CrossConnectivity.Current.IsConnected)
            {
                var IsAppinitialized = DAUtil.CheckAppInitialization();
                if (IsAppinitialized)
                {
                    var message = new StartCheckForUpdatesTask();
                    MessagingCenter.Send(message, "StartCheckForUpdatesTaskMessage");
                    HandleReceivedMessages();
                }
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
                    MainPage = new LoginPage();
                }

                else
                {
                    MainPage = new MenuNavigationPage();
                }
            }
            else{
                MainPage = new SplashPage();
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

        public static Action<Page> SetMainPage { get; set; }
    }
}
