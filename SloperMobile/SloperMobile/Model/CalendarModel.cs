using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class CalendarModel : BaseModel
    {
        public string app_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
    public class CalendarResponse
    {
        public string date_climbed { get; set; }
    }
}
