using SloperMobile.DataBase;
using SloperMobile.ViewModel;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SloperMobile
{
    public partial class App : Application
    {
        static DataAccess dbUtils;
        public static string _selectedcrag = "344";//"The Hanger"
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
        }

        public static string SelectedCrag
        {
            get { return _selectedcrag; }
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
    }
}
