using Newtonsoft.Json;
using SloperMobile.Model;
using SloperMobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace SloperMobile.Views
{
	public partial class TopoSectorPage : CarouselPage
    {
        public MapListModel CurrentSector { get; set; }
        private TopoSectorViewModel topoSectorViewModel;
		private int pageIndex;
		public int routeId = 0;
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
    }
}