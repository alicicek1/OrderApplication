using OrderApplication.Core.Business.Abstraction.Mongo;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Model.Document;

namespace OrderApplication.Business.Service.Abstraction.Mongo
{
    public interface ICustomerService : IMongoService<Customer>
    {
        DataResponse Add(Customer document);
        DataResponse Update(Customer document);
    }
}
