using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class CragTemplate : BaseModel
    {
        public string crag_name { get; set; }
        public string crag_image { get; set; }
        public string crag_portrait_image { get; set; }
        public string crag_landscape_image { get; set; }
        public string season { get; set; }
        public string weather_provider_code { get; set; }
        public string weather_provider_name { get; set; }
        public string area_name { get; set; }
        public string crag_type { get; set; }
        public string crag_sector_map_name { get; set; }
        public CragSectorMap crag_sector_map { get; set; }
        public string crag_gridref { get; set; }
        public string crag_nearest_town { get; set; }
        public string crag_is_favourite { get; set; }
        public string crag_map_zoom { get; set; }
        public string crag_map_id { get; set; }
        public string crag_guide_book { get; set; }
        public string crag_parking_longitude { get; set; }
        public string crag_parking_latitude { get; set; }
        public string crag_info_short { get; set; }
        public string crag_id { get; set; }
        public string crag_latitude { get; set; }
        public string crag_longitude { get; set; }
        public string area_id { get; set; }
        public string crag_access_info { get; set; }
        public string crag_general_info { get; set; }
        public string crag_parking_info { get; set; }
        public string date_modified { get; set; }
        public string tap_rect_in_area_map { get; set; }
        public string climbing_angles { get; set; }
        public string orientation { get; set; }
        public string sun_from { get; set; }
        public string sun_until { get; set; }
        public string walk_in_angle { get; set; }
        public string walk_in_mins { get; set; }
        public string trail_info { get; set; }
        public string approach_map_id { get; set; }
        public string approach_map_image_id { get; set; }
        public string approach_map_image_name { get; set; }
        public string version_number { get; set; }
        public bool is_enabled { get; set; }
        public string crag_sort_order { get; set; }
    }
    public class CragSectorMap
    {
        public string name { get; set; }
        public string imagedata { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string scale { get; set; }
    }
}
