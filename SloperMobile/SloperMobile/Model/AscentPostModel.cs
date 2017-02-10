using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class AscentPostModel : BaseModel
    {
        public string ascent_id { get; set; }
        public string route_id { get; set; }
        public string ascent_type_id { get; set; }
        public DateTime ascent_date { get; set; }
        public string route_type_id { get; set; }
        public string grade_id { get; set; }
        public string tech_grade_id { get; set; }
        public string rating { get; set; }
        public string photo { get; set; }
        public string comment { get; set; }
        public string video { get; set; }
        public string climbing_angle { get; set; }
        public string climbing_angle_value { get; set; }
        public string hold_type { get; set; }
        public string hold_type_value { get; set; }
        public string route_style { get; set; }
        public string route_style_value { get; set; }
        public string ImageData { get; set; }
        public string ImageName { get; set; }
    }

    public class AscentReponse
    {
        public string id { get; set; }
        public string climbingDays { get; set; }
    }
}
