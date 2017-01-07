using SloperMobile.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.ViewModel
{
    public class SplashViewModel : BaseViewModel
    {
        public SplashViewModel()
        {
            ContinueCommand = new DelegateCommand(ExecuteOnProcced);
            CancelCommand = new DelegateCommand(ExecuteOnCancel);
            IsProccedEnalbe = true;
        }

        #region Properties
        private bool isProccedEnalbe;

        public bool IsProccedEnalbe
        {
            get { return isProccedEnalbe; }
            set { isProccedEnalbe = value; OnPropertyChanged(); }
        }


        #endregion

        #region Commanding
        public DelegateCommand ContinueCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        #endregion

        #region Methods
        private async void ExecuteOnProcced(object obj)
        {
            IsProccedEnalbe = false;
            IsRunningTasks = true;
            await CheckForUpdatesViewModel.CurrentInstance().OnPageAppearing();
            OnConditionNavigation?.Invoke("Procced");
            IsProccedEnalbe = true;
            IsRunningTasks = false;
        }

        private void ExecuteOnCancel(object obj)
        {
            OnConditionNavigation?.Invoke("Cancel");
        }
        #endregion
    }
}
