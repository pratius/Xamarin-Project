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

        public Action DisplayMessage;
        public Action PageNavigation;
    }
}
