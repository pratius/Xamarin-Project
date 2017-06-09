using Newtonsoft.Json;
using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.Common.Interfaces;
using SloperMobile.DataBase;
using SloperMobile.Model;
using SQLite.Net;
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
        SQLiteConnection dbConn;
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
            dbConn = DependencyService.Get<ISQLite>().GetConnection();
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
                    string strimg64 = string.Empty;
                    var sec_img = App.DAUtil.GetSectorTopoBySectorId(nm.id);
                    if (sec_img != null)
                    {
                        var topoimg = JsonConvert.DeserializeObject<List<TopoImageResponse>>(sec_img.topo_json);
                        if (!string.IsNullOrEmpty(topoimg[0].image.data))
                        {
                            if (topoimg[0].image.name == "No_Image.jpg")
                            {
                                //load Crag Scenic Action Landscape Shot (specific to Gym)
                                var item = dbConn.Table<TCRAG_IMAGE>().FirstOrDefault(tcragimg => tcragimg.crag_id == Settings.SelectedCragSettings);
                                if (item != null)
                                {
                                    strimg64 = item.crag_landscape_image.Split(',')[1];
                                }
                                else
                                {
                                    //other wise show sloper default                                
                                    if (AppSetting.APP_TYPE == "indoor")
                                    {
                                        nm.news_image = ImageSource.FromFile("default_sloper_indoor_landscape");
                                    }
                                    else { nm.news_image = ImageSource.FromFile("default_sloper_outdoor_landscape"); }
                                }
                            }
                            else
                            {
                                strimg64 = topoimg[0].image.data.Split(',')[1];
                            }
                            if (!string.IsNullOrEmpty(strimg64))
                            {
                                byte[] imageBytes = Convert.FromBase64String(strimg64);
                                nm.news_image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                            }
                            else
                            {
                                if (AppSetting.APP_TYPE == "indoor")
                                {
                                    nm.news_image = ImageSource.FromFile("default_sloper_indoor_landscape");
                                }
                                else { nm.news_image = ImageSource.FromFile("default_sloper_outdoor_landscape"); }
                            }
                        }
                        else
                        {
                            if (AppSetting.APP_TYPE == "indoor")
                            {
                                nm.news_image = ImageSource.FromFile("default_sloper_indoor_landscape");
                            }
                            else { nm.news_image = ImageSource.FromFile("default_sloper_outdoor_landscape"); }
                        }
                    }
                    else
                    {
                        if (AppSetting.APP_TYPE == "indoor")
                        {
                            nm.news_image = ImageSource.FromFile("default_sloper_indoor_landscape");
                        }
                        else { nm.news_image = ImageSource.FromFile("default_sloper_outdoor_landscape"); }
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
