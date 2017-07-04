using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.Model
{
    public class RankingModel : BaseModel
    {
        public string user_display_name { get; set; }
        public string points { get; set; }
        public bool logged_in_user { get; set; }
        public string user_id { get; set; }
    }

    public class ShowRanking
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public string Points { get; set; }
        public Color HighlightTextColor { get; set; }
    }
}
