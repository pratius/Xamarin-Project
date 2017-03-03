using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class BucketLegends : BaseModel
    {
        public string BucketName1 { get; set; }
        public string BucketName2 { get; set; }
        public string BucketName3 { get; set; }
        public string BucketName4 { get; set; }
        public string BucketName5 { get; set; }
    }
    public class GradeId
    {
        public string grade_type_id { get; set; }
    }
}
