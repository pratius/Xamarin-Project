using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SloperMobile.Model
{
    public class MapListModel : BaseModel
    {
        public string SectorId { get; set; }
        public string SectorName { get; set; }
        public ImageSource SectorImage { get; set; }
        public string SectorLatLong { get; set; }
        public string SectorShortInfo { get; set; }
        public ImageSource Steepness1 { get; set; }
        public ImageSource Steepness2 { get; set; }
        public string BucketCount1 { get; set; } = "0";
        public string BucketCount2 { get; set; } = "0";
        public string BucketCount3 { get; set; } = "0";
        public string BucketCount4 { get; set; } = "0";
        public string BucketCount5 { get; set; } = "0";
    }
}
