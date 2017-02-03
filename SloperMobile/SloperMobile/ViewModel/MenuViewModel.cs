using SloperMobile.Model;
using System;
using System.Collections.ObjectModel;
using SloperMobile.Views;
using SloperMobile.Common.Constants;
using System.Linq;
using SloperMobile.Common.Command;

namespace SloperMobile.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {

            FillMenuItems();
            SelectedMenu = new Model.MasterPageItem();
            TapBackCommand = new DelegateCommand(TapOnBackImage);
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

        #region DelegateCommand

        public DelegateCommand TapBackCommand { get; set; }
        #endregion
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
                
                foreach (var item in menuDetails)
                {
                    MenuList.Add(new MasterPageItem
                    {
                        Title = item.crag_name.ToUpper(),
                        ItemId = item.crag_id,
                        IconSource = "",
                        Contents = item.crag_general_info,
                        TargetType = typeof(MapPage),
                    });
                }
                MenuList.Add(new MasterPageItem
                {
                    Title = "DIRECTIONS",
                    IconSource = "",
                    TargetType = typeof(MapPage),
                });
                MenuList.Add(new MasterPageItem
                {
                    Title = "SETTINGS",
                    IconSource = "",
                    TargetType = typeof(SettingsPage),
                });
                MenuList.Add(new MasterPageItem
                {

                    Title = "ABOUT THIS APP",
                    IconSource = "",
                    TargetType = typeof(HomePage),
                });
                MenuList.Add(new MasterPageItem
                {
                    Title = "CHECK FOR UPDATES",
                    IconSource = "",
                    TargetType = typeof(CheckForUpdatesPage),
                });
                //MenuList.Add(new MasterPageItem
                //{
                //    Title = "Map",
                //    IconSource = "ic_gym_map_small",
                //    TargetType = typeof(MapPage),
                //});
                //MenuList.Add(new MasterPageItem
                //{
                //    Title = "News",
                //    IconSource = "",
                //    TargetType = typeof(MapPage),
                //});

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void TapOnBackImage(object obj)
        {
            try
            {
                Cache.MasterPage.IsPresented = false;
            }
            catch
            {
            }
        }
    }
}
