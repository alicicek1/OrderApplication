using System.Net;

namespace OrderApplication.Core.Model.Util.Response
{
    public class ErrorDataResonse : DataResponse
    {
        public ErrorDataResonse(List<string> errorMessages,string errorCode,HttpStatusCode httpStatusCode)
        {
            this.IsSuccessful = false;
            this.ErrorMessageList = errorMessages;
            this.ErrorCode = errorCode;
            this.HttpStatusCode = httpStatusCode;
        }
    }
}
