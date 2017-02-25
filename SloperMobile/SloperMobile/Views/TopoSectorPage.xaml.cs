using Newtonsoft.Json;
using SloperMobile.Common.Constants;
using SloperMobile.Model;
using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SloperMobile.Views
{
    public partial class TopoSectorPage : CarouselPage
    {
        public MapListModel _CurrentSector { get; set; }
        public int _count = 0, _routeId = 0, _eleIndex = 0, _newIndex = 0, _topoIndex = -1;
        List<int> topoElement = new List<int>();
        List<int> newTopoElement = new List<int>();
        private TopoSectorViewModel topoSectorViewModel;        
        public TopoSectorPage(MapListModel CurrentSector, string routeId)
        {
            InitializeComponent();
            topoSectorViewModel = new TopoSectorViewModel();
            BindingContext = topoSectorViewModel;
            _CurrentSector = CurrentSector;
            _routeId = Convert.ToInt32(routeId);
        }

        protected async override void OnAppearing()
        {
            try
            {
                this.Children.Clear();
                topoSectorViewModel.IsRunningTasks = true;
                if (Device.OS == TargetPlatform.Android)
                {
                    LoadOnlyForDriodApp();
                }
                else if (Device.OS == TargetPlatform.iOS)
                {
                    LoadOnlyForIOSApp();
                }
                topoSectorViewModel.IsRunningTasks = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void LoadOnlyForDriodApp()
        {
            var topolistData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
            var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
            ContentPage newPage = new ContentPage();
            if (topoimgages.Count == 1)
            {
                _count = topoimgages.Count;
                this.Children.Add(newPage);
            }
            if (_routeId > 0)
            {
                //first add topo images with match routeid
                for (int i = 0; i < topoimgages.Count; i++)
                {
                    foreach (var item in topoimgages[i].drawing)
                    {
                        if (topoElement.Count == 0)
                        {
                            if (item.id == _routeId.ToString() && item.line.points.Count > 0)
                            {
                                _topoIndex = i;
                                topoElement.Add(i);
                                TopoMapRoutesPage topopageObj;
                                var topoimg = JsonConvert.SerializeObject(topoimgages[i]);
                                topopageObj = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", _routeId);                                
                                this.Children.Add(topopageObj);                                
                            }
                        }
                    }
                }
                //second add topo images without match routeid
                if (topoElement.Count > 0)
                {
                    newTopoElement.Add(topoElement[0]);
                    for (int j = (topoElement[0] + 1); j < topoimgages.Count; j++)
                    {
                        if (topoElement.Count > 0)
                        {
                            if (topoElement.Contains(j) == false)
                            {
                                newTopoElement.Add(j);
                                TopoMapRoutesPage topopageObj1;
                                var topoimg = JsonConvert.SerializeObject(topoimgages[j]);
                                topopageObj1 = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", 0);
                                this.Children.Add(topopageObj1);
                            }
                        }
                    }
                    for (int k = 0; k < topoimgages.Count; k++)
                    {
                        if (newTopoElement.Count > 0)
                        {
                            if (newTopoElement.Contains(k) == false)
                            {
                                TopoMapRoutesPage _topopageObj;
                                var topoimg = JsonConvert.SerializeObject(topoimgages[k]);
                                _topopageObj = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", 0);
                                this.Children.Add(_topopageObj);
                            }
                        }
                    }
                }
                //if routeid not present in topos then show blank page
                if (_topoIndex == -1)
                {
                    TopoMapRoutesPage _topopageObj;
                    var topoimg = string.Empty;
                    _topopageObj = new TopoMapRoutesPage(_CurrentSector, topoimg, _routeId);
                    this.Children.Add(_topopageObj);
                }
            }
            else
            {
                //load all carousel page with images when click on image
                foreach (TopoImageResponse topores in topoimgages)
                {
                    TopoMapRoutesPage topopageObj;
                    var topoimg = JsonConvert.SerializeObject(topores);
                    topopageObj = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", 0);
                    this.Children.Add(topopageObj);
                }
            }

            await Task.Delay(250);
            if (Children.Count > 0)
            {
                this.SelectedItem = this.Children.LastOrDefault();
            }

            if (topoimgages.Count == 1)
            {
                this.Children.Remove(newPage);
            }
            base.OnAppearing();
        }

        private void LoadOnlyForIOSApp()
        {
            var topolistData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
            var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
            if (_routeId > 0)
            {
                //first add topo images with match routeid
                for (int i = 0; i < topoimgages.Count; i++)
                {
                    foreach (var item in topoimgages[i].drawing)
                    {
                        if (topoElement.Count == 0)
                        {
                            if (item.id == _routeId.ToString() && item.line.points.Count > 0)
                            {
                                _topoIndex = i;
                                topoElement.Add(i);
                                TopoMapRoutesPage topopageObj;
                                var topoimg = JsonConvert.SerializeObject(topoimgages[i]);
                                topopageObj = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", 0);
                                this.Children.Add(topopageObj);
                            }
                        }
                    }
                }
                //second add topo images without match routeid
                if (topoElement.Count > 0)
                {
                    newTopoElement.Add(topoElement[0]);
                    for (int j = (topoElement[0] + 1); j < topoimgages.Count; j++)
                    {
                        if (topoElement.Count > 0)
                        {
                            if (topoElement.Contains(j) == false)
                            {
                                newTopoElement.Add(j);
                                TopoMapRoutesPage topopageObj1;
                                var topoimg = JsonConvert.SerializeObject(topoimgages[j]);
                                topopageObj1 = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", 0);
                                this.Children.Add(topopageObj1);
                            }
                        }
                    }
                    for (int k = 0; k < topoimgages.Count; k++)
                    {
                        if (newTopoElement.Count > 0)
                        {
                            if (newTopoElement.Contains(k) == false)
                            {
                                TopoMapRoutesPage _topopageObj;
                                var topoimg = JsonConvert.SerializeObject(topoimgages[k]);
                                _topopageObj = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", 0);
                                this.Children.Add(_topopageObj);
                            }
                        }
                    }
                }
                //if routeid not present in topos then show blank page
                if (_topoIndex == -1)
                {
                    TopoMapRoutesPage _topopageObj;
                    var topoimg = string.Empty;
                    _topopageObj = new TopoMapRoutesPage(_CurrentSector, topoimg, _routeId);
                    this.Children.Add(_topopageObj);
                }
            }
            else
            {
                //load all carousel page with images when click on image
                foreach (TopoImageResponse topores in topoimgages)
                {
                    TopoMapRoutesPage topopageObj;
                    var topoimg = JsonConvert.SerializeObject(topores);
                    topopageObj = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]", 0);
                    this.Children.Add(topopageObj);
                }
            }
            base.OnAppearing();
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Cache.SelectedTopoIndex= Children.IndexOf(CurrentPage);
            if (Device.OS == TargetPlatform.Android)
            {
                var index = Children.IndexOf(CurrentPage);
                if (index > 0)
                {
                    this.SelectedItem = Children[0];
                }
                if (index == 0)
                {
                    this.SelectedItem = Children[index];
                }
            }
        }
    }
}