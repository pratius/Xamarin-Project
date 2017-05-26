using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.DataBase
{
    class DataTables
    {
    }

    public class APP_SETTING
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long ID
        { get; set; }
        [NotNull]
        public string UPDATED_DATE
        { get; set; }
        public bool IS_INITIALIZED
        { get; set; }
    }

    public class T_AREA
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        [NotNull]
        public long area_id
        { get; set; }
        public bool is_enabled
        { get; set; }
        public string area_city
        { get; set; }
        public string area_latitude
        { get; set; }
        public string area_longitude
        { get; set; }
        public string area_map_zoom
        { get; set; }
        public string area_name
        { get; set; }
        public string detailed_info
        { get; set; }
        public string area_crag_map_image_name
        { get; set; }
        public string area_static_map
        { get; set; }
        public string area_crag_map_name
        { get; set; }
        public string general_info
        { get; set; }
        public string area_crag_map_iframe_src
        { get; set; }
        public string version_number
        { get; set; }
        public string sort_order
        { get; set; }
    }

    public class T_ROUTE
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        [NotNull]

        public string route_id
        { get; set; }
        public string area_id
        { get; set; }
        public string crag_id
        { get; set; }
        public string sector_id
        { get; set; }
        public bool is_enabled
        { get; set; }
        public string equipper_date
        { get; set; }
        public string equipper_name
        { get; set; }
        public string first_ascent_date
        { get; set; }
        public string first_ascent_name
        { get; set; }
        public string grade_bucket_id
        { get; set; }
        public string grade_name
        { get; set; }
        public string grade_name_sort_order
        { get; set; }
        public string grade_type_id
        { get; set; }
        public string rating_name
        { get; set; }
        public string route_type
        { get; set; }
        public string route_type_id
        { get; set; }
        public string tech_grade
        { get; set; }
        public string tech_grade_sort_order
        { get; set; }
        public string start_x
        { get; set; }
        public string start_y
        { get; set; }
        public string route_length
        { get; set; }
        public string route_name
        { get; set; }
        public string route_info
        { get; set; }
        public string date_modified
        { get; set; }
        public int sort_order
        { get; set; }
        public string graded_list_order
        { get; set; }
        public string version_number
        { get; set; }
        public string angles
        { get; set; }

        public string angles_top_1
        { get; set; }
        public string hold_type_top_1
        { get; set; }
        public string route_style_top_1
        { get; set; }
        public string angles_top_2
        { get; set; }
        public string hold_type_top_2
        { get; set; }
        public string route_style_top_2
        { get; set; }
        public string rating
        { get; set; }
        public DateTime date_created
        { get; set; }
        public DateTime route_set_date
        { get; set; }
    }

    public class T_SECTOR
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        [NotNull]
        public string sector_id
        { get; set; }
        public string crag_id
        { get; set; }
        public string date_modified
        { get; set; }
        public bool is_enabled
        { get; set; }
        public string map_id
        { get; set; }
        public string sector_info
        { get; set; }
        public string sector_info_short
        { get; set; }
        public string sector_map_rect_h
        { get; set; }
        public string sector_map_rect_w
        { get; set; }
        public string sector_map_rect_x
        { get; set; }
        public string sector_map_rect_y
        { get; set; }
        public string sector_name
        { get; set; }
        public string sort_order
        { get; set; }
        public string tap_rect_in_parent_map
        { get; set; }
        public string topo_name
        { get; set; }
        public string topo_type_id
        { get; set; }
        public string scale
        { get; set; }
        public string sector_orientation
        { get; set; }
        public string version_number
        { get; set; }
        public string latitude
        { get; set; }
        public string longitude
        { get; set; }
        public string angles
        { get; set; }
        public string angles_top_2
        { get; set; }
    }

    public class T_CRAG
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        [NotNull]
        public string crag_id
        { get; set; }
        public string crag_name
        { get; set; }
        public string weather_provider_code
        { get; set; }
        public string weather_provider_name
        { get; set; }
        public string season
        { get; set; }
        public string area_name
        { get; set; }
        public string crag_type
        { get; set; }
        public string crag_sector_map_name
        { get; set; }
        public string crag_gridref
        { get; set; }
        public string crag_nearest_town
        { get; set; }
        public string crag_is_favourite
        { get; set; }
        public string crag_map_zoom
        { get; set; }
        public string crag_map_id
        { get; set; }
        public string crag_guide_book
        { get; set; }
        public string crag_parking_longitude
        { get; set; }
        public string crag_parking_latitude
        { get; set; }
        public string crag_info_short
        { get; set; }
        public string crag_latitude
        { get; set; }
        public string crag_longitude
        { get; set; }
        public string area_id
        { get; set; }
        public string crag_access_info
        { get; set; }
        public string crag_general_info
        { get; set; }
        public string crag_parking_info
        { get; set; }
        public string date_modified
        { get; set; }
        public string tap_rect_in_area_map
        { get; set; }
        public string climbing_angles
        { get; set; }
        public string orientation
        { get; set; }
        public string sun_from
        { get; set; }
        public string sun_until
        { get; set; }
        public string walk_in_angle
        { get; set; }
        public string walk_in_mins
        { get; set; }
        public string trail_info
        { get; set; }
        public string approach_map_id
        { get; set; }
        public string approach_map_image_id
        { get; set; }
        public string approach_map_image_name
        { get; set; }
        public string version_number
        { get; set; }
        public bool is_enabled
        { get; set; }
        public string crag_sort_order
        { get; set; }
    }

    public class T_CRAG_SECTOR_MAP
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        [NotNull]
        public string crag_id
        { get; set; }
        public string name
        { get; set; }
        public string imagedata
        { get; set; }
        public string height
        { get; set; }
        public string width
        { get; set; }
        public string scale
        { get; set; }
    }

    public class T_TOPO
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long topo_id
        { get; set; }
        [NotNull]
        public string sector_id
        { get; set; }
        public string topo_json
        { get; set; }
        public string upload_date
        { get; set; }
    }

    public class TASCENT_TYPE
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long ascent_type_id
        { get; set; }
        public string ascent_type_description
        { get; set; }

    }

    public class TTECH_GRADE
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        public string tech_grade_id
        { get; set; }
        public string grade_type_id
        { get; set; }
        public string tech_grade
        { get; set; }
        public int sort_order
        { get; set; }
        public string grade_bucket_id
        { get; set; }
        public string sloper_points
        { get; set; }
    }

    public class T_GRADE
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long ID
        { get; set; }
        [NotNull]
        public string area_id
        { get; set; }
        public string crag_id
        { get; set; }
        public string sector_id
        { get; set; }
        public string grade_bucket_id
        { get; set; }
        public string grade_bucket_id_count
        { get; set; }
    }

    public class T_BUCKET
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public long ID
        { get; set; }
        [NotNull]
        public string grade_type_id
        { get; set; }
        public string grade_bucket_id
        { get; set; }
        public string bucket_name
        { get; set; }
        public string hex_code
        { get; set; }
        public string grade_bucket_group
        { get; set; }
    }


    public class TCRAG_IMAGE
    {
       [PrimaryKey, AutoIncrement]
       public long id
        { get; set; }
        public string crag_id
        { get; set; }
        public string crag_image
        { get; set; }
        public string crag_portrait_image
        { get; set; }
        public string crag_landscape_image
        { get; set; }
    }
    public class TCRAG_PORTRAIT_IMAGE
    {
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        public string crag_id
        { get; set; }
        public string crag_portrait_image
        { get; set; }
    }
    public class TCRAG_LANDSCAPE_IMAGE
    {
        [PrimaryKey, AutoIncrement]
        public long id
        { get; set; }
        public string crag_id
        { get; set; }
        public string crag_landscape_image
        { get; set; }
    }
}
