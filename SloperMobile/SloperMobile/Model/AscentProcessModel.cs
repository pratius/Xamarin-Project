using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class AscentProcessModel : BaseModel
    {
        public string SendsTypeName { get; set; }
        public string SendsDate { get; set; }
        public string SendsGrade { get; set; }
        public string SendRating { get; set; }
        public string SendClimbingStyle { get; set; }
        public string SendHoldType { get; set; }
        public string SendRouteCharacteristics { get; set; }
    }
}
