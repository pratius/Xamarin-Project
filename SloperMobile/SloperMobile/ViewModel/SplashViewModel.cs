using SloperMobile.Common.Command;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.DataBase;
using SloperMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
namespace SloperMobile.ViewModel
{
    public class SplashViewModel : BaseViewModel
    {
        public SplashViewModel(INavigation navigation)
        {
            _navigation = navigation;
            AppTitle = (AppSetting.APP_TITLE).ToUpper();
            AppCompany = AppSetting.APP_COMPANY;
            ContinueCommand = new DelegateCommand(ExecuteOnProcced);
            CancelCommand = new DelegateCommand(ExecuteOnCancel);
            IsProccedEnalbe = true;
            IsDisplayThanksNote = true;
        }

        #region Properties
        private INavigation _navigation;
        private bool isProccedEnalbe;
        private bool is_displaytanksnote;
        private string commandtext = "Let's Go!";
        private string progresstext = "Initializing App...";
        private string progressvalue = "0";
        private string apptitle;
        private string appcompany;

        private CheckForUpdateModel checkForModelObj;
        private List<T_AREA> areaObj;
        private List<CragTemplate> cragObj;
        private List<T_ROUTE> routeObj;
        private List<T_SECTOR> sectorObj;
        private List<T_GRADE> gradeObj;
        private List<T_BUCKET> gradebktObj;
        private List<TTECH_GRADE> ttechgradeObj;
        /// <summary>
        /// Get or set the Check for update class object
        /// </summary>
        public CheckForUpdateModel CheckForModelObj
        {
            get { return checkForModelObj; }
            set { checkForModelObj = value; OnPropertyChanged(); }
        }
        public List<T_AREA> AreaObj
        {
            get { return areaObj; }
            set { areaObj = value; OnPropertyChanged(); }
        }
        public List<CragTemplate> CragObj
        {
            get { return cragObj; }
            set { cragObj = value; OnPropertyChanged(); }
        }
        public List<T_ROUTE> RouteObj
        {
            get { return routeObj; }
            set { routeObj = value; OnPropertyChanged(); }
        }
        public List<T_SECTOR> SectorObj
        {
            get { return sectorObj; }
            set { sectorObj = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Returns app's last updated date.
        /// </summary>
        public string AppLastUpdateDate
        {
            get
            {
                string lastupdate = App.DAUtil.GetLastUpdate();
                if (string.IsNullOrEmpty(lastupdate))
                {
                    lastupdate = "20160101";
                }
                return lastupdate;
            }
        }
        public bool IsProccedEnalbe
        {
            get { return isProccedEnalbe; }
            set { isProccedEnalbe = value; OnPropertyChanged(); }
        }

        public bool IsDisplayThanksNote
        {
            get { return is_displaytanksnote; }
            set { is_displaytanksnote = value; OnPropertyChanged(); }
        }

        public string CommandText
        {
            get { return commandtext; }
            set { commandtext = value; OnPropertyChanged(); }
        }

        public string ProgressText
        {
            get { return progresstext; }
            set { progresstext = value; OnPropertyChanged(); }
        }

        public string ProgressValue
        {
            get { return progressvalue; }
            set { progressvalue = value; OnPropertyChanged(); }
        }

        public string AppTitle
        {
            get { return apptitle; }
            set { apptitle = value; OnPropertyChanged(); }
        }

        public string AppCompany
        {
            get { return appcompany; }
            set { appcompany = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commanding
        public DelegateCommand ContinueCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        #endregion

        #region Methods
        private async void ExecuteOnProcced(object obj)
        {

            if (CommandText == "CONTINUE")
            {
                OnConditionNavigation?.Invoke("Procced");
            }

            if (CommandText == "Let's Go!")
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    IsDisplayThanksNote = false;
                    IsRunningTasks = true;
                    CommandText = "CANCEL";

                    await DownloadUpdates();
                    CommandText = "CONTINUE";
                    IsRunningTasks = true;
                }
                else
                    await _navigation.PushAsync(new Views.NetworkErrorPage());
            }

            if (CommandText == "CANCEL")
            {
                IsRunningTasks = false;
                IsDisplayThanksNote = true;
                CommandText = "Let's Go!";
                OnConditionNavigation?.Invoke("CANCEL");
            }
        }

        private void ExecuteOnCancel(object obj)
        {
            OnConditionNavigation?.Invoke("CANCEL");
        }
        #endregion

        #region Functions

        public async Task DownloadUpdates()
        {
            IsRunningTasks = true;
            CheckForModelObj = await HttpGetCheckForUpdates();
            if (CheckForModelObj != null && Convert.ToInt32(CheckForModelObj.areas_modified) + Convert.ToInt32(CheckForModelObj.crags_modified) + Convert.ToInt32(CheckForModelObj.routes_modified) + Convert.ToInt32(CheckForModelObj.sectors_modified) > 1)
            {
                ProgressText = "Initializing App, please wait...";

                if (Convert.ToInt32(CheckForModelObj.areas_modified) > 0)
                {
                    ProgressText = "Loading Areas, please wait...";
                    AreaObj = await HttpGetAreaUpdates();
                    foreach (T_AREA area in AreaObj)
                    {
                        App.DAUtil.SaveArea(area);
                    }
                    ProgressValue = "0.1";
                }
                if (Convert.ToInt32(CheckForModelObj.crags_modified) > 0)
                {
                    ProgressText = "Loading Crags, please wait...";
                    CragObj = await HttpGetCragUpdates();
                    foreach (CragTemplate crag in CragObj)
                    {
                        T_CRAG tcrag = new T_CRAG();
                        T_CRAG_SECTOR_MAP tcs_map = new T_CRAG_SECTOR_MAP();

                        tcrag.crag_id = crag.crag_id;
                        tcrag.crag_name = crag.crag_name;
                        tcrag.weather_provider_code = crag.weather_provider_code;
                        tcrag.weather_provider_name = crag.weather_provider_name;
                        tcrag.season = crag.season;
                        tcrag.area_name = crag.area_name;
                        tcrag.crag_type = crag.crag_type;
                        tcrag.crag_sector_map_name = crag.crag_sector_map_name;
                        tcrag.crag_gridref = crag.crag_gridref;
                        tcrag.crag_nearest_town = crag.crag_nearest_town;
                        tcrag.crag_is_favourite = crag.crag_is_favourite;
                        tcrag.crag_map_zoom = crag.crag_map_zoom;
                        tcrag.crag_map_id = crag.crag_map_id;
                        tcrag.crag_guide_book = crag.crag_guide_book;
                        tcrag.crag_parking_longitude = crag.crag_parking_longitude;
                        tcrag.crag_parking_latitude = crag.crag_parking_latitude;
                        tcrag.crag_info_short = crag.crag_info_short;
                        tcrag.crag_latitude = crag.crag_latitude;
                        tcrag.crag_longitude = crag.crag_longitude;
                        tcrag.area_id = crag.area_id;
                        tcrag.crag_access_info = crag.crag_access_info;
                        tcrag.crag_general_info = crag.crag_general_info;
                        tcrag.crag_parking_info = crag.crag_parking_info;
                        tcrag.date_modified = crag.date_modified;
                        tcrag.tap_rect_in_area_map = crag.tap_rect_in_area_map;
                        tcrag.climbing_angles = crag.climbing_angles;
                        tcrag.orientation = crag.orientation;
                        tcrag.sun_from = crag.sun_from;
                        tcrag.sun_until = crag.sun_until;
                        tcrag.walk_in_angle = crag.walk_in_angle;
                        tcrag.walk_in_mins = crag.walk_in_mins;
                        tcrag.trail_info = crag.trail_info;
                        tcrag.approach_map_id = crag.approach_map_id;
                        tcrag.approach_map_image_id = crag.approach_map_image_id;
                        tcrag.approach_map_image_name = crag.approach_map_image_name;
                        tcrag.version_number = crag.version_number;
                        tcrag.is_enabled = crag.is_enabled;
                        tcrag.crag_sort_order = crag.crag_sort_order;

                        tcs_map.crag_id = crag.crag_id;
                        tcs_map.name = crag.crag_sector_map.name;
                        tcs_map.imagedata = crag.crag_sector_map.imagedata;
                        tcs_map.height = crag.crag_sector_map.height;
                        tcs_map.width = crag.crag_sector_map.width;
                        tcs_map.scale = crag.crag_sector_map.scale;
                        //Added by Ravi on 02-May-2017
                        if (!string.IsNullOrEmpty(crag.crag_image))
                        {
                            TCRAG_IMAGE tci = new TCRAG_IMAGE();
                            tci.crag_id = crag.crag_id;
                            tci.crag_image = crag.crag_image;
                            App.DAUtil.SaveTCragImage(tci);
                        }
                        //Added by Sandeep on 23-May-2017
                        if (!string.IsNullOrEmpty(crag.crag_portrait_image))
                        {
                            TCRAG_PORTRAIT_IMAGE tcpi = new TCRAG_PORTRAIT_IMAGE();
                            tcpi.crag_id = crag.crag_id;
                            tcpi.crag_portrait_image = crag.crag_portrait_image;
                            App.DAUtil.SaveTCragPortraitImage(tcpi);
                        }
                        //Added by Sandeep on 23-May-2017
                        if (!string.IsNullOrEmpty(crag.crag_landscape_image))
                        {
                            TCRAG_LANDSCAPE_IMAGE tcli = new TCRAG_LANDSCAPE_IMAGE();
                            tcli.crag_id = crag.crag_id;
                            tcli.crag_landscape_image = crag.crag_image;
                            App.DAUtil.SaveTCragLandscapeImage(tcli);
                        }
                        App.DAUtil.SaveCrag(tcrag);
                        App.DAUtil.SaveCragSectorMap(tcs_map);

                    }
                    ProgressValue = "0.3";
                }

                if (Convert.ToInt32(CheckForModelObj.sectors_modified) > 0)
                {
                    ProgressText = "Loading Sectors, please wait...";
                    SectorObj = await HttpGetSectorUpdates();

                    foreach (T_SECTOR sector in SectorObj)
                    {
                        App.DAUtil.SaveSector(sector);
                        Dictionary<string, string> topodict = new Dictionary<string, string>();
                        topodict.Add("sectorID", sector.sector_id);
                        HttpClientHelper apicall = new ApiHandler(ApiUrls.Url_GetUpdate_TopoData, string.Empty);
                        var topo_response = await apicall.GetJsonString<string>(topodict);
                        T_TOPO topo = new T_TOPO();
                        topo.sector_id = sector.sector_id;
                        topo.topo_json = topo_response;
                        topo.upload_date = Helper.GetCurrentDate("yyyyMMdd");
                        App.DAUtil.SaveTopo(topo);

                    }
                    ProgressValue = "0.5";
                }

                if (Convert.ToInt32(CheckForModelObj.routes_modified) > 0)
                {
                    ProgressText = "Loading Routes, please wait...";
                    RouteObj = await HttpGetRouteUpdates();
                    foreach (T_ROUTE route in RouteObj)
                    {
                        App.DAUtil.SaveRoute(route);
                    }
                    ProgressValue = "0.7";
                }
                //==========================Updating GRADE here =======================

                ProgressText = "Loading Grades, please wait...";
                App.DAUtil.DropAndCreateTable(typeof(T_GRADE));
                gradeObj = await HttpGetGradeUpdates();
                foreach (T_GRADE grade in gradeObj)
                {
                    App.DAUtil.SaveGrade(grade);
                }
                ProgressValue = "0.8";
                
                ProgressText = "Loading Grades Buckets, please wait...";
                App.DAUtil.DropAndCreateTable(typeof(T_BUCKET));
                gradebktObj = await HttpGetGradeBuckets();
                foreach (T_BUCKET gradebkt in gradebktObj)
                {
                    App.DAUtil.SaveGradeBucket(gradebkt);
                }

                //================= Added by Ravi on 28-Apr-2017=============
                ProgressValue = "0.9";
                ProgressText = "Loading Grades Thoughts, please wait...";
                ttechgradeObj = await HttpGetTTechGrade();
                foreach (TTECH_GRADE ttgrade in ttechgradeObj)
                {
                    App.DAUtil.SaveTTechGrade(ttgrade);
                }
                //===========================================================

                ProgressValue = "1";
                //=====================================================================
                APP_SETTING updated_date = new APP_SETTING();
                updated_date.UPDATED_DATE = Helper.GetCurrentDate("yyyyMMdd");
                updated_date.IS_INITIALIZED = true;
                App.DAUtil.SaveLastUpdate(updated_date);
                ProgressText = "Finished.";
                IsRunningTasks = false;
            }
            else
            {
                APP_SETTING updated_date = new APP_SETTING();
                updated_date.UPDATED_DATE = Helper.GetCurrentDate("yyyyMMdd");
                App.DAUtil.SaveLastUpdate(updated_date);
                ProgressText = "Your app is up to date.";
                IsRunningTasks = false;
            }
        }

        #endregion

        #region Services 

        private async Task<CheckForUpdateModel> HttpGetCheckForUpdates()
        {
            HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_CheckUpdate_AppData, Cache.AccessToken);
            Dictionary<string, string> dictquery = new Dictionary<string, string>();
            dictquery.Add("appid", AppSetting.APP_ID);
            dictquery.Add("since", AppLastUpdateDate);
            var response = await apicall.Get<CheckForUpdateModel>(dictquery);
            return response;
        }
        private async Task<List<T_AREA>> HttpGetAreaUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppSetting.APP_ID, AppLastUpdateDate, "area", true), Cache.AccessToken);
            var area_response = await apicall.Get<T_AREA>();
            return area_response;
        }
        private async Task<List<CragTemplate>> HttpGetCragUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppSetting.APP_ID, AppLastUpdateDate, "crag", true), Cache.AccessToken);
            var crag_response = await apicall.Get<CragTemplate>();
            return crag_response;
        }
        private async Task<List<T_ROUTE>> HttpGetRouteUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppSetting.APP_ID, AppLastUpdateDate, "route", true), Cache.AccessToken);
            var route_response = await apicall.Get<T_ROUTE>();
            return route_response;
        }
        private async Task<List<T_SECTOR>> HttpGetSectorUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppSetting.APP_ID, AppLastUpdateDate, "sector", true), Cache.AccessToken);
            var sector_response = await apicall.Get<T_SECTOR>();
            return sector_response;
        }

        private async Task<List<T_GRADE>> HttpGetGradeUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppSetting.APP_ID, AppLastUpdateDate, "grade", true), Cache.AccessToken);
            var grade_response = await apicall.Get<T_GRADE>();
            return grade_response;
        }

        private async Task<List<T_BUCKET>> HttpGetGradeBuckets()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetGradeBuckets, AppSetting.APP_ID), Cache.AccessToken);
            var gradebkt_response = await apicall.Get<T_BUCKET>();
            return gradebkt_response;
        }
        //================= Added by Ravi on 28-Apr-2017=============
        private async Task<List<TTECH_GRADE>> HttpGetTTechGrade()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetTTechGrades, AppSetting.APP_ID), Cache.AccessToken);
            var ttgrade_response = await apicall.Get<TTECH_GRADE>();
            return ttgrade_response;
        }
        #endregion
    }
}
