using OrderApplication.Core.Business.Abstraction.Mongo;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Model.Document;
using OrderApplication.Model.Util.Request;

namespace OrderApplication.Business.Service.Abstraction.Mongo
{
    public interface IOrderService : IMongoService<Order>
    {
        DataResponse Add(Order document);
        DataResponse Update(Order document);
        DataResponse ChangeStatus(ChangeStatusRequestModel changeStatusRequestModel);
    }
}
