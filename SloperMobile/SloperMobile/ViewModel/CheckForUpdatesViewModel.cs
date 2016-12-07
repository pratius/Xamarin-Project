using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Model;

namespace SloperMobile.ViewModel
{
    public class CheckForUpdatesViewModel : BaseViewModel
    {
        public CheckForUpdatesViewModel()
        {
            CheckForModelObj = new CheckForUpdateModel();
        }


        #region Properties
        private CheckForUpdateModel checkForModelObj;
        /// <summary>
        /// Get or set the Check for update class object
        /// </summary>
        public CheckForUpdateModel CheckForModelObj
        {
            get { return checkForModelObj; }
            set { checkForModelObj = value; OnPropertyChanged(); }
        }

        #endregion

        #region Functions

        public void OnPageAppearing()
        {
            //To do on page load 
        }

        #endregion

        #region Services 

        private void HttpGetCheckForUpdate()
        {
            //To do for Service Call 
        }
        #endregion
    }
}
