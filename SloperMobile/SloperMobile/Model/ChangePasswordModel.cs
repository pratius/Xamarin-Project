using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class ChangePasswordModel : BaseModel
    {
        public string currentpassword{ get; set; }
        public string newpassword{ get; set; }
        public string confirmpassword{ get; set; }
    }
    public class ChangePasswordResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
