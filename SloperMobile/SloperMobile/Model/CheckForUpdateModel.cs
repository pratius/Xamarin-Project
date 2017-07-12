using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class CheckForUpdateModel : BaseModel
    {
        public string areas_modified { get; set; }
        public string crags_modified { get; set; }
        public string sectors_modified { get; set; }
        public string routes_modified { get; set; }
    }
}
