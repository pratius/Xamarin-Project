using SloperMobile.Model;
using System;
using System.Collections.ObjectModel;
using SloperMobile.Views;

namespace SloperMobile.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {

            FillMenuItems();

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
                    if (item.crag_name == "CCC Chinook")
                    {
                        MenuList.Add(new MasterPageItem
                        {
                            Title = "CCC Chinook",
                            IconSource = "Menu_side_bar_service_mode",
                            Contents = item.crag_general_info,
                            TargetType = typeof(MapPage),
                        });
                    }
                    if (item.crag_name == "Grassi Lakes")
                    {
                        MenuList.Add(new MasterPageItem
                        {
                            Title = "Grassi Lakes",
                            IconSource = "Menu_side_bar_service_mode",
                            Contents = item.crag_general_info,
                            TargetType = typeof(MapPage),
                        });
                    }
                    if (item.crag_name == "The Hanger")
                    {
                        MenuList.Add(new MasterPageItem
                        {
                            Title = "The Hanger",
                            IconSource = "Menu_side_bar_account_settings",
                            Contents = item.crag_general_info,
                            TargetType = typeof(MapPage),
                        });
                    }
                    if (item.crag_name == "The Stronghold")
                    {
                        MenuList.Add(new MasterPageItem
                        {
                            Title = "The Stronhold",
                            IconSource = "Menu_side_bar_about",
                            Contents = item.crag_general_info,
                            TargetType = typeof(MapPage),
                        });
                    }

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
