using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{

    
    public class Send:BaseModel
    {
        public int Ascent_Id { get; set; }
        public int User_Id { get; set; }
        public int Route_Id { get; set; }
        public int Ascent_Type_Id { get; set; }
        public DateTime Date_Climbed { get; set; }
        public int Route_Type_Id { get; set; }
        public int Grade_Id { get; set; }
        public int Tech_Grade_Id { get; set; }
        public int Rating { get; set; }
        public int Image_id { get; set; }
        public string Comment { get; set; }
        public string Video { get; set; }
        public int Climbing_Angle { get; set; }
        public int Hold_Type { get; set; }
        public int Route_Style { get; set; }
        public string Guid { get; set; }
        public DateTime Date_Modified { get; set; }
    }

}
