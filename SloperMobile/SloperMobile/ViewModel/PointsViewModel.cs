using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class PointsViewModel : BaseViewModel
    {
        public PointsViewModel()
        {
            OnPagePrepration();
            //UserPoints = new List<Model.PointList>();
        }

        private async void OnPagePrepration()
        {
            try
            {
                PageHeaderText = "PROFILE";
                PageSubHeaderText = "Points";
            }
            catch (Exception ex)
            {
                //throw ex;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}
