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
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
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
					//MainPage = new NavigationPage(new MenuNavigationPage());
					//removed by Steve replaced with code below - 2017-02-15

					MainPage = new MenuNavigationPage();

                    //HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_Login_Extend, Settings.AccessTokenSettings);
                    //ExtendURL extnobj = new ExtendURL();
                    //extnobj.rtoken = Settings.RenewalTokenSettings;
                    //var extnjson = JsonConvert.SerializeObject(extnobj);
                    //var response = await apicall.Post<LoginResponse>(extnjson);
                    //if (response != null)
                    //{
                    //    Settings.AccessTokenSettings = response.accessToken;
                    //    Settings.RenewalTokenSettings = response.renewalToken;
                    //    Settings.DisplayNameSettings = response.displayName;
                    //    MainPage = new NavigationPage(new HomePage());
                    //}
                    //else
                    //    MainPage = new NavigationPage(new LoginPage());
                }

            }
            else
            {
                MainPage = new NavigationPage(new SplashPage());
            }
        }
    }
}
