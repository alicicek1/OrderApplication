using OrderApplication.Core.Model.Util.Error;
using System.Net;

namespace OrderApplication.Core.Model.Constant.Error
{
    public class ErrorConstant
    {
        public static BaseServiceErrorConstant MODEL_DID_NOT_INSERTED => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Err001", Message = "Model did not inserted." };
        public static BaseServiceErrorConstant MODEL_DID_NOT_UPDATED => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Err002", Message = "Model did not updated." };
        public static BaseServiceErrorConstant MODEL_DID_NOT_DELETED => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Err003", Message = "Model did not deleted." };
    }
}
