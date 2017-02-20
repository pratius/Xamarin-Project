using Newtonsoft.Json;
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
        private TopoSectorViewModel topoSectorViewModel;
        public int _count = 0;       
        public TopoSectorPage(MapListModel CurrentSector)
        {
            InitializeComponent();
            topoSectorViewModel = new TopoSectorViewModel();
            BindingContext = topoSectorViewModel;
            _CurrentSector = CurrentSector;
            //var topolistData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
        }

        protected async override void OnAppearing()
        {
            try
            {                
                this.Children.Clear();
                topoSectorViewModel.IsRunningTasks = true;
               
                var topolistData = App.DAUtil.GetSectorLines(_CurrentSector?.SectorId);
                var topoimgages = JsonConvert.DeserializeObject<List<TopoImageResponse>>(topolistData);
                ContentPage newPage = new ContentPage();
                if (topoimgages.Count == 1)
                {
                    _count = topoimgages.Count;                   
                    this.Children.Add(newPage);
                }
                foreach (TopoImageResponse topores in topoimgages)
                {                    
                    TopoMapRoutesPage topopageObj;
                    var topoimg = JsonConvert.SerializeObject(topores);
                    topopageObj = new TopoMapRoutesPage(_CurrentSector, "[" + topoimg + "]");
                    this.Children.Add(topopageObj);
                }

                await Task.Delay(2000);
                this.SelectedItem = this.Children.LastOrDefault();
                topoSectorViewModel.IsRunningTasks = false;
                if (topoimgages.Count == 1)
                {                   
                    this.Children.Remove(newPage);
                }
                base.OnAppearing();
            }
            catch (Exception ex)
            {

               throw ex;
            }            
        }       

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();            
            var index = Children.IndexOf(CurrentPage);
            if (index > 0)
            {
                this.SelectedItem = Children[index];
            }
            if (index == 0)
            {               
                this.SelectedItem = Children[Children.Count() - 1];
            }
        }
    }
}
