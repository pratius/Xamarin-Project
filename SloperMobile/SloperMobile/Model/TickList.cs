using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class TickList : BaseModel
    {
        public string route_name { get; set; }
        public string grade_name { get; set; }
        public int Grade_Id { get; set; }
        public long RouteID { get; set; }
        public long sector_id { get; set; }

        private DateTime date_Created;

        public DateTime Date_Created
        {
            get { return date_Created; }
            set
            {
                date_Created = value;
                DateCreated = value.ToString("MM/dd/yy");
            }
        }

        private string dateCreated;

        public string DateCreated
        {
            get { return dateCreated; }
            set
            {
                dateCreated = value; OnPropertyChanged();

            }
        }
    }
}
