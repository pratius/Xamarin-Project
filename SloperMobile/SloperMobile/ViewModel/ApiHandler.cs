using SloperMobile.Common.Helpers;

namespace SloperMobile.ViewModel
{
    internal class ApiHandler : HttpClientHelper
    {
        public ApiHandler(string endpoint, string accesstoken) : base(endpoint, accesstoken)
        {
        }
    }
}