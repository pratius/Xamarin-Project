using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.ViewModel
{
    public class CragMapPageViewModel : BaseViewModel
    {
        public CragMapPageViewModel()
        {
            PageHeaderText = "Crag Map";
            string crag_name = App.DAUtil.GetSelectedCragData().crag_name;
            if (!string.IsNullOrEmpty(crag_name))
            {
                PageSubHeaderText = crag_name;
            }
            else
            {
                PageSubHeaderText = "";
            }
        }
    }
}
