using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.Mongo;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Model.Document;

namespace OrderApplication.Business.Service.Concretion.Mongo
{
    public class OrderService : MongoService<Order>, IOrderService
    {
        private readonly IBusinessRuleEngine businessRuleEngine;
        public OrderService(IMongoRepository<Order> _mongoRepository, IBusinessRuleEngine businessRuleEngine) : base(_mongoRepository)
        {
            this.businessRuleEngine = businessRuleEngine;
        }
    }
}

