using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Model
{
    public class LoginResponse
    {
        public string displayName { get; set; }
        public string accessToken { get; set; }
        public string renewalToken { get; set; }
    }
}
