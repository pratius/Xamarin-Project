using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SloperMobile.Common.Constants;
using SloperMobile.Common.Helpers;
using SloperMobile.DataBase;
using SloperMobile.Model;
using Xamarin.Forms;

namespace SloperMobile.ViewModel
{
    public class CheckForUpdatesViewModel : BaseViewModel
    {
        #region Properties
        private CheckForUpdateModel checkForModelObj;
        private List<T_AREA> areaObj;
        private List<CragTemplate> cragObj;
        private List<T_ROUTE> routeObj;
        private List<T_SECTOR> sectorObj;
        private string displayupdatemessage;
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

        public string DisplayUpdateMessage
        {
            get { return displayupdatemessage= "Checking for updates..."; }
            set { displayupdatemessage = value; OnPropertyChanged(); }
        }
        #endregion

        public CheckForUpdatesViewModel()
        {
            CheckForModelObj = new CheckForUpdateModel();
        }

        #region Functions

        public async void OnPageAppearing()
        {
            //DisplayUpdateMessage = "Checking for updates...";
            CheckForModelObj = await HttpGetCheckForUpdates();
            if (CheckForModelObj != null && Convert.ToInt32(CheckForModelObj.areas_modified) + Convert.ToInt32(CheckForModelObj.crags_modified) + Convert.ToInt32(CheckForModelObj.routes_modified) + Convert.ToInt32(CheckForModelObj.sectors_modified) > 1)
            {
                DisplayUpdateMessage = "Updates are available,downloading now.\nplease wait...";
                
                if (Convert.ToInt32(CheckForModelObj.areas_modified) > 0)
                {
                    DisplayUpdateMessage="Downloading Area now.\nplease wait...";
                    AreaObj = await HttpGetAreaUpdates();
                    foreach (T_AREA area in AreaObj)
                    {
                        App.DAUtil.SaveArea(area);
                    }
                }
                if (Convert.ToInt32(CheckForModelObj.crags_modified) > 0)
                {
                    DisplayUpdateMessage = "Downloading Crag now.\nplease wait...";
                    CragObj = await HttpGetCragUpdates();
                    foreach (CragTemplate crag in CragObj)
                    {
                        T_CRAG tcrag = new T_CRAG();
                        T_CRAG_SECTOR_MAP tcs_map = new T_CRAG_SECTOR_MAP();

                        tcrag.crag_id = crag.crag_id;
                        tcrag.crag_name = crag.crag_name;
                        tcrag.weather_provider_code = crag.weather_provider_code;
                        tcrag.weather_provider_name = crag.weather_provider_name;
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

                        App.DAUtil.SaveCrag(tcrag);
                        App.DAUtil.SaveCragSectorMap(tcs_map);

                    }
                }
                if (Convert.ToInt32(CheckForModelObj.routes_modified) > 0)
                {
                    DisplayUpdateMessage = "Downloading Route now.\nplease wait...";
                    RouteObj = await HttpGetRouteUpdates();
                    foreach (T_ROUTE route in RouteObj)
                    {
                        App.DAUtil.SaveRoute(route);

                        Dictionary<string, string> topodict = new Dictionary<string, string>();
                        topodict.Add("sectorID", route.sector_id);
                        HttpClientHelper apicall = new ApiHandler(ApiUrls.Url_GetUpdate_TopoData, string.Empty);
                        var topo_response = await apicall.GetJsonString<string>(topodict);
                        T_TOPO topo = new T_TOPO();
                        topo.sector_id = route.sector_id;
                        topo.topo_json = topo_response;
                        topo.upload_date = Helper.GetCurrentDate("yyyyMMdd");
                        App.DAUtil.SaveTopo(topo);
                    }

                }
                if (Convert.ToInt32(CheckForModelObj.sectors_modified) > 0)
                {
                    DisplayUpdateMessage = "Downloading Sector now.\nplease wait...";
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
                }
                LAST_UPDATE updated_date = new LAST_UPDATE();
                updated_date.UPDATED_DATE = Helper.GetCurrentDate("yyyyMMdd");
                App.DAUtil.SaveLastUpdate(updated_date);
                DisplayUpdateMessage = "Thanks for updating.";
            }
            else
            {
                LAST_UPDATE updated_date = new LAST_UPDATE();
                updated_date.UPDATED_DATE = Helper.GetCurrentDate("yyyyMMdd");
                App.DAUtil.SaveLastUpdate(updated_date);
                DisplayUpdateMessage = "Your app is uptodate...";
            }
        }

        #endregion

        #region Services 

        private async Task<CheckForUpdateModel> HttpGetCheckForUpdates()
        {
            HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_CheckUpdate_AppData, Cache.AccessToken);
            Dictionary<string, string> dictquery = new Dictionary<string, string>();
            dictquery.Add("appid", AppConstant.APP_ID);
            dictquery.Add("since", AppLastUpdateDate);
            var response = await apicall.Get<CheckForUpdateModel>(dictquery);
            return response;
        }
        private async Task<List<T_AREA>> HttpGetAreaUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppConstant.APP_ID, "json", "area", AppLastUpdateDate), Cache.AccessToken);
            var area_response = await apicall.Get<T_AREA>();
            return area_response;
        }
        private async Task<List<CragTemplate>> HttpGetCragUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppConstant.APP_ID, "json", "crag", AppLastUpdateDate), Cache.AccessToken);
            var crag_response = await apicall.Get<CragTemplate>();
            return crag_response;
        }
        private async Task<List<T_ROUTE>> HttpGetRouteUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppConstant.APP_ID, "json", "route", AppLastUpdateDate), Cache.AccessToken);
            var route_response = await apicall.Get<T_ROUTE>();
            return route_response;
        }
        private async Task<List<T_SECTOR>> HttpGetSectorUpdates()
        {
            HttpClientHelper apicall = new ApiHandler(string.Format(ApiUrls.Url_GetUpdate_AppData, AppConstant.APP_ID, "json", "sector", AppLastUpdateDate), Cache.AccessToken);
            var sector_response = await apicall.Get<T_SECTOR>();
            return sector_response;
        }


        #endregion
    }
}
