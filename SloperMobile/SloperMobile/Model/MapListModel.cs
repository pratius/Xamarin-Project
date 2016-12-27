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
    }
}
