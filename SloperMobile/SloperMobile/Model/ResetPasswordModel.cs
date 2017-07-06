using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class ResetPasswordModel : BaseModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
