using OrderApplication.Core.Extension;
using OrderApplication.Core.Model.Document;

namespace OrderApplication.Core.Model.Util.Response
{
    public class SuccessDataResponse : DataResponse
    {
        public SuccessDataResponse()
        {
            this.IsSuccessful = true;
            this.HttpStatusCode = System.Net.HttpStatusCode.OK;
        }
    }
}
