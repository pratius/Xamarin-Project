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
        public DateTime date { get; set; }
        public string title { get; set; }
        public string sub_title { get; set; }
        public string count { get; set; }
        public string message { get; set; }
        public string news_type { get; set; }
        public ImageSource news_image { get; set; } = null;
    }
}
