using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class RankingViewModel : BaseViewModel
    {
        #region Constructor
        public RankingViewModel()
        {
            OnPagePrepration();
        }
        #endregion

        #region Properties
        private System.Collections.ObjectModel.ObservableCollection<ShowRanking> _rankings = new System.Collections.ObjectModel.ObservableCollection<ShowRanking>();
        public System.Collections.ObjectModel.ObservableCollection<ShowRanking> Rankings
        {
            get { return _rankings; }
            set { _rankings = value; OnPropertyChanged("Rankings"); }
        }

        private int _userRank;
        public int UserRank
        {
            get { return _userRank; }
            set { _userRank = value; OnPropertyChanged(); }
        }

        private double _rankPercentage;
        public double RankPercentage
        {
            get { return _rankPercentage; }
            set { _rankPercentage = value; OnPropertyChanged(); }
        }
        #endregion

        #region Method
        private async void OnPagePrepration()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...", Acr.UserDialogs.MaskType.Black);
                PageHeaderText = "PROFILE";
                PageSubHeaderText = "Ranking";

                await InvokeServiceGetAscentDates();

                Acr.UserDialogs.UserDialogs.Instance.Loading().Hide();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        private async Task InvokeServiceGetAscentDates()
        {
            try
            {
                HttpClientHelper apicall;
                var rank = new System.Collections.ObjectModel.ObservableCollection<ShowRanking>();
                apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetRankings, AppSetting.APP_ID), Settings.AccessTokenSettings);
                var response = apicall.Get<RankingModel>();
                if (response.Result.Count > 0)
                {
                    for (int i = 0; i < response.Result.Count; i++)
                    {
                        if (response.Result[i].logged_in_user)
                        {
                            UserRank = i + 1;
                            RankPercentage = (1 - (((double)UserRank / response.Result.Count) - ((double)1 / response.Result.Count))) * 100;
                            rank.Add(new ShowRanking()
                            {
                                Rank = i + 1,
                                Name = response.Result[i].user_display_name,
                                Points = response.Result[i].points,
                                HighlightTextColor = Color.FromHex("#FF8E2D")
                            });
                        }
                        else
                        {
                            rank.Add(new ShowRanking()
                            {
                                Rank = i + 1,
                                Name = response.Result[i].user_display_name,
                                Points = response.Result[i].points,
                                HighlightTextColor = Color.White
                            });
                        }
                    }
                }
                Rankings = rank;
            }
            catch (Exception ex)
            {
                throw ex;
                Acr.UserDialogs.UserDialogs.Instance.Loading().Hide();
            }
        }
        #endregion
    }
}
