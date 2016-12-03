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
    public class LAST_UPDATE
    {
        [PrimaryKey, AutoIncrement]
        public long ID
        { get; set; }
        [NotNull]
        public string UPDATED_DATE
        { get; set; }

    }
    public class T_AREA
    {
        [PrimaryKey, AutoIncrement]
        public long ID
        { get; set; }
        [NotNull]
        public long AREA_ID
        { get; set; }
        public int IS_ENABLED
        { get; set; }
        public string AREA_CITY
        { get; set; }
        public string AREA_LATITUDE
        { get; set; }
        public string AREA_LONGITUDE
        { get; set; }
        public string AREA_MAP_ZOOM
        { get; set; }
        public string AREA_NAME
        { get; set; }
        public string DETAILED_INFO
        { get; set; }
        public string AREA_CRAG_MAP_IMAGE_NAME
        { get; set; }
        public string AREA_STATIC_MAP
        { get; set; }
        public string AREA_CRAG_MAP_NAME
        { get; set; }
        public string GENERAL_INFO
        { get; set; }
        public string AREA_CRAG_MAP_IFRAME_SRC
        { get; set; }
        public string VERSION_NUMBER
        { get; set; }
        public string SORT_ORDER
        { get; set; }
    }
    public class T_ROUTE
    {
        [PrimaryKey, AutoIncrement]
        public long  ID
        { get; set; }
        [NotNull]

        public string  ROUTE_ID
        { get; set; }
        public string  AREA_ID
        { get; set; }
        public string  CRAG_ID
        { get; set; }
        public string  SECTOR_ID
        { get; set; }
        public string  IS_ENABLED
        { get; set; }
        public string  EQUIPPER_DATE
        { get; set; }
        public string  EQUIPPER_NAME
        { get; set; }
        public string  FIRST_ASCENT_DATE
        { get; set; }
        public string  FIRST_ASCENT_NAME
        { get; set; }
        public string  GRADE_BUCKET_ID
        { get; set; }
        public string  GRADE_NAME
        { get; set; }
        public string  GRADE_NAME_SORT_ORDER
        { get; set; }
        public string  GRADE_TYPE_ID
        { get; set; }
        public string  RATING_NAME
        { get; set; }
        public string  ROUTE_TYPE
        { get; set; }
        public string  ROUTE_TYPE_ID
        { get; set; }
        public string  TECH_GRADE
        { get; set; }
        public string  TECH_GRADE_SORT_ORDER
        { get; set; }
        public string  START_X
        { get; set; }
        public string  START_Y
        { get; set; }
        public string  ROUTE_LENGTH
        { get; set; }
        public string  ROUTE_NAME
        { get; set; }
        public string  ROUTE_INFO
        { get; set; }
        public string  DATE_MODIFIED
        { get; set; }
        public string  SORT_ORDER
        { get; set; }
        public string  GRADED_LIST_ORDER
        { get; set; }
        public string  VERSION_NUMBER
        { get; set; }
    }
    public class T_SECTOR
    {
        [PrimaryKey, AutoIncrement]
        public  long  ID
        { get; set; }
        [NotNull]
        public  string  SECTOR_ID
        { get; set; }
        public  string  CRAG_ID
        { get; set; }
        public  string  DATE_MODIFIED
        { get; set; }
        public  string  IS_ENABLED
        { get; set; }
        public  string  MAP_ID
        { get; set; }
        public  string  SECTOR_INFO
        { get; set; }
        public  string  SECTOR_INFO_SHORT
        { get; set; }
        public  string  SECTOR_MAP_RECT_H
        { get; set; }
        public  string  SECTOR_MAP_RECT_W
        { get; set; }
        public  string  SECTOR_MAP_RECT_X
        { get; set; }
        public  string  SECTOR_MAP_RECT_Y
        { get; set; }
        public  string  SECTOR_NAME
        { get; set; }
        public  string  SORT_ORDER
        { get; set; }
        public  string  TAP_RECT_IN_PARENT_MAP
        { get; set; }
        public  string  TOPO_NAME
        { get; set; }
        public  string  TOPO_TYPE_ID
        { get; set; }
        public  string  SCALE
        { get; set; }
        public  string  SECTOR_ORIENTATION
        { get; set; }
        public  string  VERSION_NUMBER
        { get; set; }
    }
    public class T_CRAG
    {
        [PrimaryKey,AutoIncrement]
        public long ID
        { get; set; }
        [NotNull]
        public string CRAG_ID
        { get; set; }
        public string CRAG_NAME
        { get; set; }
        public string WEATHER_PROVIDER_CODE
        { get; set; }
        public string WEATHER_PROVIDER_NAME
        { get; set; }
        public string AREA_NAME
        { get; set; }
        public string CRAG_TYPE
        { get; set; }
        public string CRAG_SECTOR_MAP_NAME
        { get; set; }
        public string CRAG_GRIDREF
        { get; set; }
        public string CRAG_NEAREST_TOWN
        { get; set; }
        public string CRAG_IS_FAVOURITE
        { get; set; }
        public string CRAG_MAP_ZOOM
        { get; set; }
        public string CRAG_MAP_ID
        { get; set; }
        public string CRAG_GUIDE_BOOK
        { get; set; }
        public string CRAG_PARKING_LONGITUDE
        { get; set; }
        public string CRAG_PARKING_LATITUDE
        { get; set; }
        public string CRAG_INFO_SHORT
        { get; set; }
        public string CRAG_LATITUDE
        { get; set; }
        public string CRAG_LONGITUDE
        { get; set; }
        public string AREA_ID
        { get; set; }
        public string CRAG_ACCESS_INFO
        { get; set; }
        public string CRAG_GENERAL_INFO
        { get; set; }
        public string CRAG_PARKING_INFO
        { get; set; }
        public string DATE_MODIFIED
        { get; set; }
        public string TAP_RECT_IN_AREA_MAP
        { get; set; }
        public string CLIMBING_ANGLES
        { get; set; }
        public string ORIENTATION
        { get; set; }
        public string SUN_FROM
        { get; set; }
        public string SUN_UNTIL
        { get; set; }
        public string WALK_IN_ANGLE
        { get; set; }
        public string WALK_IN_MINS
        { get; set; }
        public string TRAIL_INFO
        { get; set; }
        public string APPROACH_MAP_ID
        { get; set; }
        public string APPROACH_MAP_IMAGE_ID
        { get; set; }
        public string APPROACH_MAP_IMAGE_NAME
        { get; set; }
        public string VERSION_NUMBER
        { get; set; }
        public string IS_ENABLED
        { get; set; }
        public string CRAG_SORT_ORDER
        { get; set; }       
    }
    public class T_CRAG_SECTOR_MAP
    {
        [PrimaryKey, AutoIncrement]
        public long ID
        { get; set; }
        [NotNull]
        public string CRAG_ID
        { get; set; }
        public string NAME
        { get; set; }
        public string IMAGEDATA
        { get; set; }
        public string HEIGHT
        { get; set; }
        public string WIDTH
        { get; set; }
        public string SCALE
        { get; set; }
    }
    public class T_TOPO
    {
        [PrimaryKey, AutoIncrement]
        public long   TOPO_ID
        { get; set; }
        [NotNull]
        public string SECTOR_ID
        { get; set; }
        public string TOPO_JSON
        { get; set; }
        public string UPLOAD_DATE
        { get; set; }
    }

}
