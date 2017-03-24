using SloperMobile.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.ViewModel
{
    public class TopoSectorViewModel : BaseViewModel
    {
        public TopoSectorViewModel()
        {
            GoCommand = new DelegateCommand(ExecuteOnGo);
        }

        #region DelegateCommand
        public DelegateCommand GoCommand { get; set; }
        #endregion

        private async void ExecuteOnGo(object parma)
        {

        }
    }
}
