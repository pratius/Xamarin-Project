using SloperMobile.Model;
using System;
using System.Collections.ObjectModel;
using SloperMobile.Views;
using SloperMobile.Common.Constants;
using System.Linq;

namespace SloperMobile.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {

            FillMenuItems();
            SelectedMenu = new Model.MasterPageItem();
        }


        private ObservableCollection<MasterPageItem> menuList;
        /// <summary>
        /// Get and set the user menu list
        /// </summary>
        public ObservableCollection<MasterPageItem> MenuList
        {
            get { return menuList; }
            set { menuList = value; OnPropertyChanged(); }
        }

        private MasterPageItem selecteMenu;

        public MasterPageItem SelectedMenu
        {
            get { return selecteMenu; }
            set
            {
                selecteMenu = value;
                if (value?.Title != null)
                {
                    var menuDetails = App.DAUtil.GetCragList();
                    if (menuDetails.Count > 0)
                    {
                        var selectedItems = menuDetails?.FirstOrDefault(s => s.crag_name == selecteMenu.Title);
                        Cache.Selected_CRAG = selectedItems;
                    }
                }
                OnPropertyChanged();
            }
        }


        private void FillMenuItems()
        {
            try
            {
                var menuDetails = App.DAUtil.GetCragList();
                MenuList = new ObservableCollection<MasterPageItem>();

                MenuList.Add(new MasterPageItem
                {

                    Title = "About This App",
                    IconSource = "ic_about_us",
                    TargetType = typeof(HomePage),
                });
                MenuList.Add(new MasterPageItem
                {
                    Title = "Map",
                    IconSource = "ic_gym_map_small",
                    TargetType = typeof(MapPage),
                });
                MenuList.Add(new MasterPageItem
                {
                    Title = "News",
                    IconSource = "",
                    TargetType = typeof(MapPage),
                });
                MenuList.Add(new MasterPageItem
                {
                    Title = "Directions",
                    IconSource = "",
                    TargetType = typeof(MapPage),
                });
                MenuList.Add(new MasterPageItem
                {
                    Title = "Settings",
                    IconSource = "",
                    TargetType = typeof(MapPage),
                });
                foreach (var item in menuDetails)
                {
                    MenuList.Add(new MasterPageItem
                    {
                        Title = item.crag_name,
                        ItemId = item.crag_id,
                        IconSource = "Menu_side_bar_service_mode",
                        Contents = item.crag_general_info,
                        TargetType = typeof(MapPage),
                    });

                }

                MenuList.Add(new MasterPageItem
                {
                    Title = "Check for Update",
                    IconSource = "Menu_side_bar_about",
                    TargetType = typeof(CheckForUpdatesPage),
                });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
