using SloperMobile.Common.NotifyProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.ViewModel
{
    public class BaseViewModel : NotifyPropertiesChanged
    {
        public BaseViewModel()
        {

        }

        private bool isRunningTasks;
        /// <summary>
        /// /Get or set the IsRunningTasks
        /// </summary>
        public bool IsRunningTasks
        {
            get { return isRunningTasks; }
            set { isRunningTasks = value; OnPropertyChanged(); }
        }

        private string pageHeaderText;

        public string PageHeaderText
        {
            get { return pageHeaderText.ToUpper(); }
            set { pageHeaderText = value; OnPropertyChanged(); }
        }

        private string pagesubHeaderText;

        public string PageSubHeaderText
        {
            get { return pagesubHeaderText; }
            set { pagesubHeaderText = value; OnPropertyChanged(); }
        }

        public Action DisplayMessage;
        public Action OnPageNavigation;
        public Action<object> OnConditionNavigation;
    }
}
