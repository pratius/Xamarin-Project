using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Common.Command;
using SloperMobile.Common.Helpers;

namespace SloperMobile.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {
        private string _displayname;

        public string DisplayName
        {
            get { return _displayname; }
            set { _displayname = value; OnPropertyChanged(); }
        }
        public SettingViewModel()
        {
            PageHeaderText = "SETTINGS";
            PageSubHeaderText = "User";
            DisplayName = Settings.DisplayNameSettings;
            LogOutCommand = new DelegateCommand(ExecuteOnLogOut);
        }



        #region DelegateCommand

        public DelegateCommand LogOutCommand { get; set; }

        #endregion

        private void ExecuteOnLogOut(object parma)
        {
            try
            {
                Settings.AccessTokenSettings = "";
                Settings.RenewalTokenSettings = "";
                Settings.DisplayNameSettings = "";
                OnPageNavigation?.Invoke();
            }
            catch
            {

            }
        }
    }
}
