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
        public string updated_date { get; set; }
    }
    public class GetConsensusSectorsDTO
    {
        public string app_id { get; set; }
        public string app_date_last_updated { get; set; }
    }

    public class GetConsensusRoutesDTO
    {
        public string app_id { get; set; }
        public string app_date_last_updated { get; set; }
    }
}
