
using OrderApplication.Core.Model.Util.Error;
using System.Net;

namespace OrderApplication.Model.Contant.Error
{
    public class OrderErrorConstant
    {
        public static BaseServiceErrorConstant MODEL_CANNOT_BE_NULL => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Order001", Message = "Model cannot be null." };
        public static BaseServiceErrorConstant MODEL_PROPERTY_CANNOT_BE_NULL(string propertyName) => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Order002", Message = $"Model {propertyName} cannot be null." };
        public static BaseServiceErrorConstant MODEL_PROPERTY_FORMAT_NOT_VALID(string propertyName) => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Order002", Message = $"Model {propertyName} format not valid." };
        public static BaseServiceErrorConstant NOT_FOUND => new BaseServiceErrorConstant { HttpStatusCode = HttpStatusCode.BadRequest, Code = "Order002", Message = $"There is no model with provided Id." };
    }
}
