using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class AscentProcessViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private string highlightingcolor="Black";
        public AscentProcessViewModel(INavigation navigation)
        {
            PageHeaderText = Cache.SelctedCurrentSector?.SectorName;
            SendTypeCommand = new DelegateCommand(ExecuteOnSendType);
            SendTypeHoldCommand = new DelegateCommand(ExecuteOnSendHold);
            SendDataCommand = new DelegateCommand(ExecuteOnData);
            SendClimbingCommand = new DelegateCommand(ExecuteOnClimbingAngle);
            SendRouteStyleCommand = new DelegateCommand(ExecuteOnRouteStyle);
            SendRatingCommand = new DelegateCommand(ExecuteOnRating);
            SendSummaryCommand = new DelegateCommand(ExecuteOnSummary);
            _navigation = navigation;
            currentInstance = this;
        }

        private static AscentProcessViewModel currentInstance;
        public static AscentProcessViewModel CurrentInstance(INavigation navigation)
        {
            if (currentInstance == null)
                return new AscentProcessViewModel(navigation);
            else
                return currentInstance;
        }

        public string HighlightingColor
        {
            get { return highlightingcolor; }
            set { highlightingcolor = value;OnPropertyChanged(); }
        }
        public DelegateCommand SendTypeCommand { get; set; }
        public DelegateCommand SendTypeHoldCommand { get; set; }
        public DelegateCommand SendDataCommand { get; set; }
        public DelegateCommand SendClimbingCommand { get; set; }
        public DelegateCommand SendRouteStyleCommand { get; set; }
        public DelegateCommand SendRatingCommand { get; set; }
        public DelegateCommand SendSummaryCommand { get; set; }

        private  void ExecuteOnSendType(object obj)
        {
            HighlightingColor = "Yellow";
            //var param = Convert.ToString(obj);
            //await _navigation.PushAsync(new AscentDatePage());
        }

        private async void ExecuteOnData(object obj)
        {
            await _navigation.PushAsync(new AscentRatingPage());

        }

        private async void ExecuteOnSendHold(object obj)
        {
            await _navigation.PushAsync(new AscentClimbingAnglePage());

        }

     

        private async void ExecuteOnClimbingAngle(object obj)
        {
            await _navigation.PushAsync(new AscentSummaryPage());

        }

        private async void ExecuteOnRouteStyle(object obj)
        {
            await _navigation.PushAsync(new AscentHoldTypePage());

        }

        private async void ExecuteOnRating(object obj)
        {
            await _navigation.PushAsync(new AscentRouteStylePage());

        }

        private async void ExecuteOnSummary(object obj)
        {
           await _navigation.PushAsync(new HomePage());

        }

    }
}
