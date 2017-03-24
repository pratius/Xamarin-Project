using SloperMobile.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            ClimbingYear = DateTime.Now.Year.ToString();
            ClimbDaysCount = Settings.ClimbingDaysSettings.ToString();
        }

        private string climbyear;
        public string ClimbingYear
        {
            get { return climbyear; }
            set { climbyear = value; OnPropertyChanged(); }
        }
        private string climbcount;
        public string ClimbDaysCount
        {
            get { return climbcount; }
            set { climbcount = value; OnPropertyChanged(); }
        }
    }
}
