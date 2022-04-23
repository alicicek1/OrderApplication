using OrderApplication.Core.Model.Util.Request;

namespace OrderApplication.Model.Util.Request
{
    public class ChangeStatusRequestModel : IRequest
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}
