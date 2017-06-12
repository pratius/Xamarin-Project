﻿using SloperMobile.Common.Command;
using SloperMobile.Common.Helpers;
using SloperMobile.DataBase;
using SloperMobile.Common.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SloperMobile.Model;
using System.Collections.ObjectModel;

namespace SloperMobile.ViewModel
{
    public class CragDetailsViewModel : BaseViewModel
    {
        private T_CRAG currentCrag;
        public ObservableCollection<Color> BarColors { get; set; }
        public CragDetailsViewModel()
        {
            currentCrag = App.DAUtil.GetSelectedCragData();
            PageHeaderText = currentCrag.crag_name;
            PageSubHeaderText = currentCrag.area_name;
            BarColors = new ObservableCollection<Color>();
            GraphData = new List<CragDetailModel>();
            LoadLegendsBucket();
            LoadGraphData();
            CragDesc = currentCrag.crag_general_info;
            var cragimg = App.DAUtil.GetScenicImageForCrag(Settings.SelectedCragSettings);
            if (cragimg != null)
            {
                byte[] imageBytes = Convert.FromBase64String(cragimg.crag_image.Split(',')[1]);
                CragImage = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            else
            {
                if (AppSetting.APP_TYPE == "indoor")
                {
                    CragImage = ImageSource.FromFile("default_sloper_indoor_square");
                }
                else
                {
                    CragImage = ImageSource.FromFile("default_sloper_outdoor_square");
                }

            }
        }

        #region Properties
        private ImageSource _cragimg;
        public ImageSource CragImage
        {
            get { return _cragimg; }
            set { _cragimg = value; OnPropertyChanged(); }
        }
        private string _cragdesc;
        public string CragDesc
        {
            get { return _cragdesc; }
            set { _cragdesc = value; OnPropertyChanged(); }
        }
        private DataTemplate legendsdata;
        public DataTemplate LegendsDataTemplate
        {
            get { return legendsdata; }
            set { legendsdata = value; OnPropertyChanged(); }
        }

        private List<CragDetailModel> _graphdata;
        public List<CragDetailModel> GraphData
        {
            get { return _graphdata; }
            set { _graphdata = value; OnPropertyChanged(); }
        }
        #endregion


        #region Methods
        private void LoadLegendsBucket()
        {
            try
            {
                var leg_buckets = App.DAUtil.GetBucketsByCragID(Settings.SelectedCragSettings);
                if (leg_buckets != null)
                {
                    int gc = App.DAUtil.GetTotalBucketForApp();
                    int gr = leg_buckets.Count / gc;
                    Grid grdLegend = new Grid();
                    for (var i = 0; i < gr; i++)
                    {
                        grdLegend.RowDefinitions?.Add(new RowDefinition { Height = GridLength.Auto });
                    }

                    for (var i = 0; i < gc; i++)
                    {
                        grdLegend.ColumnDefinitions?.Add(new ColumnDefinition { Width = GridLength.Star });
                    }

                    var batches = leg_buckets.Select((x, i) => new { x, i }).GroupBy(p => (p.i / gc), p => p.x);

                    int r = 0;
                    foreach (var row in batches)
                    {
                        int c = 0;
                        foreach (var item in row)
                        {
                            grdLegend.Children.Add(new Label { Text = item.BucketName, HorizontalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.FromHex(item.HexColor) }, c, r);
                            c++;
                        }
                        r++;
                    }
                    LegendsDataTemplate = new DataTemplate(() =>
                    {
                        return grdLegend;
                    });

                }
            }
            catch (Exception ex)
            {
            }
        }


        private void LoadGraphData()
        {
            int totalbuckets = App.DAUtil.GetTotalBucketForApp();
            for (int i = 1; i <= totalbuckets; i++)
            {
                CragDetailModel objcount = new CragDetailModel();
                objcount.BucketCount = string.IsNullOrEmpty(App.DAUtil.GetBucketCountByCragIdAndGradeBucketId(currentCrag.crag_id, i.ToString())) ? "0" : App.DAUtil.GetBucketCountByCragIdAndGradeBucketId(currentCrag.crag_id, i.ToString());
                BarColors.Add(Color.FromHex(App.DAUtil.GetBucketHexColorByGradeBucketId(i.ToString()) == null ? "#cccccc" : App.DAUtil.GetBucketHexColorByGradeBucketId(i.ToString())));
                GraphData.Add(objcount);
            }
        }
        #endregion
    }
}
