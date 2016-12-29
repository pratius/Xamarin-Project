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
            set { menuList = value; }
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
                    IconSource = "ic_about_us"

                });

                MenuList.Add(new MasterPageItem
                {
                    Title = "Map",
                    IconSource = "ic_gym_map_small",
                    TargetType = typeof(MapPage),
                });

                MenuList.Add(new MasterPageItem
                {
                    Title = "CCC Chinook",
                    IconSource = "Menu_side_bar_service_mode",

                });

                MenuList.Add(new MasterPageItem
                {
                    Title = "The Hanger",
                    IconSource = "Menu_side_bar_account_settings",

                });

                MenuList.Add(new MasterPageItem
                {
                    Title = "The Stronhold",
                    IconSource = "Menu_side_bar_about",

                });


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
