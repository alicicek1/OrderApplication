
using System.Net;

namespace OrderApplication.Core.Model.Util.Error
{
    public class BaseServiceErrorConstant
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
