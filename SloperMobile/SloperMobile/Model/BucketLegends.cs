using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.Model
{
    public class BucketLegends : BaseModel
    {
        public string BucketName1 { get; set; }
        public string BucketName2 { get; set; }
        public string BucketName3 { get; set; }
        public string BucketName4 { get; set; }
        public string BucketName5 { get; set; }

        public class Comparer : IEqualityComparer<BucketLegends>
        {
            public bool Equals(BucketLegends x, BucketLegends y)
            {
                return x.BucketName1 == y.BucketName1
                    && x.BucketName2 == y.BucketName2
                    && x.BucketName3 == y.BucketName3
                    && x.BucketName4 == y.BucketName4
                    && x.BucketName5 == y.BucketName5;
            }

            public int GetHashCode(BucketLegends obj)
            {
                unchecked  // overflow is fine
                {
                    int hash = 17;
                    hash = hash * 23 + (obj.BucketName1 ?? "").GetHashCode();
                    hash = hash * 23 + (obj.BucketName2 ?? "").GetHashCode();
                    hash = hash * 23 + (obj.BucketName3 ?? "").GetHashCode();
                    hash = hash * 23 + (obj.BucketName4 ?? "").GetHashCode();
                    hash = hash * 23 + (obj.BucketName5 ?? "").GetHashCode();
                    return hash;
                }
            }
        }
    }


    public class Buckets
    {
        public string BucketName { get; set; }
        public string HexColor { get; set; }
    }
    public class GradeId
    {
        public string grade_type_id { get; set; }
    }
}
