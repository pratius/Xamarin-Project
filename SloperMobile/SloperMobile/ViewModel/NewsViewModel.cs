using Newtonsoft.Json;
using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.DataBase;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class NewsViewModel : BaseViewModel
    {

        private ObservableCollection<NewsModel> _newsList;
        public ObservableCollection<NewsModel> NewsList
        {
            get { return _newsList ?? (_newsList = new ObservableCollection<NewsModel>()); }
            set { _newsList = value; OnPropertyChanged(); }
        }
        #region DelegateCommand

        public DelegateCommand LoadMoreNews { get; set; }
        #endregion
        public NewsViewModel()
        {
            PageHeaderText = "NEWS";
            PageSubHeaderText = "What's New?";
            LoadMoreNews = new DelegateCommand(LoadNews);
        }

        private void LoadNews(object obj)
        {
            try
            {
                var app_news = App.DAUtil.GetAppNews(NewsList.Count(), 10);
                foreach (NewsModel nm in app_news)
                {
                    var sec_img = App.DAUtil.GetSectorTopoBySectorId(nm.id);
                    if (sec_img != null)
                    {
                        var topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(sec_img.topo_json);
                        if (!string.IsNullOrEmpty(topoimg[0].image.data))
                        {
                            string strimg64 = topoimg[0].image.data.Split(',')[1];
                            if (!string.IsNullOrEmpty(strimg64))
                            {
                                byte[] imageBytes = Convert.FromBase64String(strimg64);
                                nm.news_image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                            }
                            else
                            {
                                if (AppSetting.APP_TYPE == "indoor")
                                {
                                    nm.news_image = ImageSource.FromFile("default_sloper_portrait_image_indoor.png");
                                }
                                else { nm.news_image = ImageSource.FromFile("default_sloper_portrait_image_outdoor.png"); }
                            }
                        }
                        else
                        {
                            if (AppSetting.APP_TYPE == "indoor")
                            {
                                nm.news_image = ImageSource.FromFile("default_sloper_portrait_image_indoor.png");
                            }
                            else { nm.news_image = ImageSource.FromFile("default_sloper_portrait_image_outdoor.png"); }
                        }
                    }
                    else
                    {
                        if (AppSetting.APP_TYPE == "indoor")
                        {
                            nm.news_image = ImageSource.FromFile("default_sloper_portrait_image_indoor.png");
                        }
                        else { nm.news_image = ImageSource.FromFile("default_sloper_portrait_image_outdoor.png"); }
                    }
                    NewsList.Add(nm);
                }



            }
            catch (Exception ex)
            {
                string strerr = ex.Message;
            }
        }
    }
}
