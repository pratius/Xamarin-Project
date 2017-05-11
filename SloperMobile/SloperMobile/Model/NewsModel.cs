using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.Model
{
    public class NewsModel : BaseModel
    {
        public string id { get; set; }
        public string date_created { get; set; }
        public string sector_name { get; set; }
        public string new_route_count { get; set; }
        public string news_type { get; set; }
        public ImageSource news_image { get; set; } = null;
    }
}
