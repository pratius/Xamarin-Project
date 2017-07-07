using Acr.UserDialogs;
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
        public MapListModel CurrentSector { get; set; }
        public int _count = 0, routeId = 0, _eleIndex = 0, _newIndex = 0, _topoIndex = -1, cnt = 0;
        List<int> topoElement = new List<int>();
        List<int> newTopoElement = new List<int>();
        private TopoSectorViewModel topoSectorViewModel;
		private int pageIndex;
		private bool singleRoute;

        public TopoSectorPage(MapListModel CurrentSector, string routeId, bool singleRoute)
        {
            InitializeComponent();
            topoSectorViewModel = new TopoSectorViewModel();
            BindingContext = topoSectorViewModel;
            this.CurrentSector = CurrentSector;
            this.routeId = Convert.ToInt32(routeId);
			this.singleRoute = singleRoute;
		}

        protected async override void OnAppearing()
        {
            try
            {
				pageIndex = 0;
				this.Children.Clear();
                topoSectorViewModel.IsRunningTasks = true;
                if (Device.RuntimePlatform == Device.Android)
                {                    
                    LoadOnlyForDriodApp();                   
                }
                else if (Device.RuntimePlatform == Device.iOS)
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
			var topolistData = App.DAUtil.GetSectorLines(CurrentSector?.SectorId);
			var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
			ContentPage newPage = new ContentPage();
			if (topoimgages.Count == 1 || topoimgages.Count == 2)
			{
				_count = topoimgages.Count;
				this.Children.Add(newPage);
			}
			if (routeId > 0)
			{
				//first add topo images with match routeid
				for (int i = 0; i < topoimgages.Count; i++)
				{
					foreach (var item in topoimgages[i].drawing)
					{
						if (topoElement.Count == 0)
						{
							if (item.id == routeId.ToString() && item.line.points.Count > 0)
							{
								_topoIndex = i;
								topoElement.Add(i);
								TopoMapRoutesPage topopageObj;
								var topoimg = JsonConvert.SerializeObject(topoimgages[i]);
								topopageObj = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", routeId, pageIndex, singleRoute);
								this.Children.Add(topopageObj);
								pageIndex++;
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
								topopageObj1 = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", 0, pageIndex, singleRoute);
								this.Children.Add(topopageObj1);
								pageIndex++;
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
								_topopageObj = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", 0, pageIndex, singleRoute);
								this.Children.Add(_topopageObj);
								pageIndex++;
							}
						}
					}
				}
				//if routeid not present in topos then show blank page
				if (_topoIndex == -1)
				{
					TopoMapRoutesPage _topopageObj;
					var topoimg = string.Empty;
					_topopageObj = new TopoMapRoutesPage(CurrentSector, topoimg, routeId, pageIndex, singleRoute);
					this.Children.Add(_topopageObj);
					pageIndex++;
				}
			}
			else
			{
				//load all carousel page with images when click on image
				foreach (TopoImageResponse topores in topoimgages)
				{
					TopoMapRoutesPage topopageObj;
					var topoimg = JsonConvert.SerializeObject(topores);
					topopageObj = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", 0, pageIndex, singleRoute);
					this.Children.Add(topopageObj);
					pageIndex++;
				}
			}

			await Task.Delay(250);
			if (Children.Count > 0)
			{
				this.SelectedItem = this.Children.LastOrDefault();
			}

			if (topoimgages.Count == 1 || topoimgages.Count == 2)
			{
				this.Children.Remove(newPage);
			}
			base.OnAppearing();
		}

		private void LoadOnlyForIOSApp()
		{
			var topolistData = App.DAUtil.GetSectorLines(CurrentSector?.SectorId);
			var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
			if (routeId > 0)
			{
				//first add topo images with match routeid
				for (int i = 0; i < topoimgages.Count; i++)
				{
					foreach (var item in topoimgages[i].drawing)
					{
						if (topoElement.Count == 0)
						{
							if (item.id == routeId.ToString() && item.line.points.Count > 0)
							{
								_topoIndex = i;
								topoElement.Add(i);
								TopoMapRoutesPage topopageObj;
								var topoimg = JsonConvert.SerializeObject(topoimgages[i]);
								topopageObj = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", routeId, pageIndex, singleRoute);
								this.Children.Add(topopageObj);
								pageIndex++;
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
								topopageObj1 = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", 0, pageIndex, singleRoute);
								this.Children.Add(topopageObj1);
								pageIndex++;
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
								_topopageObj = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", 0, pageIndex, singleRoute);
								this.Children.Add(_topopageObj);
								pageIndex++;
							}
						}
					}
				}
				//if routeid not present in topos then show blank page
				if (_topoIndex == -1)
				{
					TopoMapRoutesPage _topopageObj;
					var topoimg = string.Empty;
					_topopageObj = new TopoMapRoutesPage(CurrentSector, topoimg, routeId, pageIndex, singleRoute);
					this.Children.Add(_topopageObj);
					pageIndex++;
				}
			}
			else
			{
				//load all carousel page with images when click on image
				foreach (TopoImageResponse topores in topoimgages)
				{
					TopoMapRoutesPage topopageObj;
					var topoimg = JsonConvert.SerializeObject(topores);
					topopageObj = new TopoMapRoutesPage(CurrentSector, "[" + topoimg + "]", 0, pageIndex, singleRoute);
					this.Children.Add(topopageObj);
					pageIndex++;
				}
			}
			base.OnAppearing();
		}

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Cache.SelectedTopoIndex = Children.IndexOf(CurrentPage);
            if (Device.RuntimePlatform ==Device.Android)
            {
                var index = Children.IndexOf(CurrentPage);
                if (index == -1)
                {
                    cnt++;
                }
                if (index > 0)
                {
                    this.SelectedItem = Children[0];
                }
                if (index == 0)
                {
                    this.SelectedItem = Children[index];
                }
                else if (index == -1)
                {
                    if (cnt == 1)
                    {
                        Navigation.PushAsync(new MapDetailPage(CurrentSector));
                        cnt = 0;
                    }
                    else
                    {
                        Navigation.PopAsync().ConfigureAwait(false);
                    }
                }
            }
            else
            {
                var index = Children.IndexOf(CurrentPage);
                if (index == -1)
                {
                    cnt++;
                }
                if (index > 0)
                {
                    this.SelectedItem = Children[0];
                }
                if (index == 0)
                {
                    this.SelectedItem = Children[index];
                }
                else if (index == -1)
                {
                    if (cnt == 1)
                    {
                        Navigation.PushAsync(new MapDetailPage(CurrentSector));
                        cnt = 0;
                    }
                    else
                    {
                        Navigation.PopAsync().ConfigureAwait(false);
                    }
                }
            }
        }
    }
}