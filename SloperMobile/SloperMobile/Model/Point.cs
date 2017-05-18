using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class Point
    {
        public string route_name { get; set; }
        public string tech_grade { get; set; }
        public long points { get; set; }
        private DateTime dateclimbed;
        public DateTime date_climbed
        {
            get { return dateclimbed; }
            set { dateclimbed =value; }
        }
    }
    public class PointList
    {
        public List<Point> UsersPoint { get; set; }
    }
}
