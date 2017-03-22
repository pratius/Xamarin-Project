using SloperMobile.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.ViewModel
{
    public class NewsViewModel : BaseViewModel
    {
        private List<T_ROUTE> routelistdata;
        public List<T_ROUTE> RouteList
        {
            get { return routelistdata; }
            set { routelistdata = value; OnPropertyChanged(); }
        }
        public NewsViewModel()
        {
            PageHeaderText = "NEWS";
            RouteList = new List<T_ROUTE>();
            RouteList = App.DAUtil.GetRoutesForSelectedCrag().ToList();
        }
    }
}
