
using OrderApplication.Core.Model.Util.Error;
using System.Net;

namespace OrderApplication.Model.Contant.Error
{
    public class CustomerErrorConstant
    {
        public static BaseServiceErrorConstant MODEL_CANNOT_BE_NULL => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Customer001", Message = "Model cannot be null." };
        public static BaseServiceErrorConstant MODEL_PROPERTY_CANNOT_BE_NULL(string propertyName) => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Customer002", Message = $"Model {propertyName} cannot be null." };
        public static BaseServiceErrorConstant MODEL_PROPERTY_FORMAT_NOT_VALID(string propertyName) => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Customerr002", Message = $"Model {propertyName} format not valid." };
    }
}
