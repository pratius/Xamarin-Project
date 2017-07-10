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

        protected override void OnAppearing()
        {
			try
			{
				pageIndex = 0;
				this.Children.Clear();
				topoSectorViewModel.IsRunningTasks = true;
				InitialiTopoCarouselChildrens();
				topoSectorViewModel.IsRunningTasks = false;
			}
			catch
			{
				throw;
			}
        }

		private void InitialiTopoCarouselChildrens()
		{
			var topolistData = App.DAUtil.GetSectorLines(CurrentSector?.SectorId);
			var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
			if (routeId > 0)
			{
				//Finding all topos with selected route
				var allToposWithCurrentRoute = topoimgages.Select(item =>
				{
					var drawing = item.drawing.FirstOrDefault(route => Convert.ToInt32(route.id) == routeId);
					return drawing != null ? new TopoImageResponse
					{
						drawing = new topodrawing[] { drawing },
						image = item.image
					} : null;
				}).ToList();

				//Creating pages with specified topos
				foreach (var topoImage in allToposWithCurrentRoute)
				{
					if (topoImage == null)
					{
						continue;
					}

					SetupTopoPage(topoImage);
				}
			}
			else
			{
				//load all carousel page with images when click on image
				foreach (var topores in topoimgages)
				{
					SetupTopoPage(topores);
				}
			}
			
			if (Children.Count > 0)
			{
				this.SelectedItem = this.Children.LastOrDefault();
			}

			base.OnAppearing();
		}

		private void SetupTopoPage(TopoImageResponse topoImage)
		{
			var topoImageSerialized = JsonConvert.SerializeObject(topoImage);
			var serializedAsArray = $"[{topoImageSerialized}]";
			var topoPageObject = new TopoMapRoutesPage(CurrentSector, serializedAsArray, routeId, pageIndex, singleRoute);
			this.Children.Add(topoPageObject);
			pageIndex++;
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