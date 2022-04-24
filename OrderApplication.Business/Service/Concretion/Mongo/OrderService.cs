using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Validation.Order;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.Mongo;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Model.Util.Aspect;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Model.Contant.Error;
using OrderApplication.Model.Document;
using OrderApplication.Model.Util.Request;
using System.Reflection;

namespace OrderApplication.Business.Service.Concretion.Mongo
{
    public class OrderService : MongoService<Order>, IOrderService
    {
        private readonly IBusinessRuleEngine businessRuleEngine;
        private readonly ICustomerService customerService;
        private Order TempOrder = null;
        public OrderService(IMongoRepository<Order> _mongoRepository, IBusinessRuleEngine businessRuleEngine, ICustomerService customerService) : base(_mongoRepository)
        {
            this.businessRuleEngine = businessRuleEngine;
            this.customerService = customerService;
        }

        public override IEnumerable<Order> GetAllWithoutFilter()
        {
            var response = base.GetAllWithoutFilter().ToList();

            var customerIds = response.Select(b => b.CustomerId);
            var customerList = this.customerService.FilterBy(a => customerIds.Contains(a.Id)).ToList();
            response.ForEach(order =>
            {
                order.Customer = customerList.Find(a => a.Id == order.CustomerId);
            });

            return response;
        }

        public override Order FindByObjectId(string id)
        {
            var response = base.FindByObjectId(id);
            if (response != null && !string.IsNullOrWhiteSpace(response.CustomerId))
                response.Customer = this.customerService.FindByObjectId(response.CustomerId);
            return response;
        }

        [ValidationAspect(typeof(NewOrderValidator))]
        public override DataResponse InsertOne(Order document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckProperties(document));
            if (!response.IsSuccessful)
                return response;

            response = base.InsertOne(document);

            return response;
        }

        [ValidationAspect(typeof(UpdateOrderValidator))]
        public override DataResponse ReplaceOne(Order document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckProperties(document));
            if (!response.IsSuccessful)
                return response;

            response = base.ReplaceOne(document);

            return response;
        }

        public DataResponse ChangeStatus(ChangeStatusRequestModel changeStatusRequestModel)
        {
            DataResponse response = businessRuleEngine.Validate(CheckModelIfExist(changeStatusRequestModel));
            if (!response.IsSuccessful)
                return response;

            TempOrder.Status = changeStatusRequestModel.Status;
            TempOrder.UpdatedAt = DateTime.Now;

            response = base.ReplaceOne(TempOrder);

            return response;
        }

        private DataResponse CheckModelIfExist(ChangeStatusRequestModel changeStatusRequestModel)
        {
            if (changeStatusRequestModel == null)
                return ErrorDataResponse(OrderErrorConstant.MODEL_CANNOT_BE_NULL);

            if (string.IsNullOrWhiteSpace(changeStatusRequestModel.Id))
                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(nameof(changeStatusRequestModel.Id)));
            if (string.IsNullOrWhiteSpace(changeStatusRequestModel.Status))
                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(nameof(changeStatusRequestModel.Status)));

            TempOrder = this.FindByObjectId(changeStatusRequestModel.Id);
            if (TempOrder == null)
                return ErrorDataResponse(OrderErrorConstant.NOT_FOUND);

            return new DataResponse { IsSuccessful = true };
        }

        private DataResponse CheckProperties(Order document)
        {
            if (document == null)
                return ErrorDataResponse(OrderErrorConstant.MODEL_CANNOT_BE_NULL);

            foreach (PropertyInfo item in document.GetType().GetProperties())
            {
                var value = item.GetValue(document);

                if (value != null)
                {
                    try
                    {
                        if (item.PropertyType == typeof(string))
                            if (string.IsNullOrWhiteSpace(value.ToString()))
                                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));

                        if (item.PropertyType == typeof(Int32))
                            if (Convert.ToInt32(value) == 0)
                                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));

                        if (item.PropertyType == typeof(double))
                            if (Convert.ToDouble(value) == 0)
                                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));

                        if (item.PropertyType == typeof(DateTime))
                            if (Convert.ToDateTime(value) == null || Convert.ToDateTime(value) == DateTime.MinValue)
                                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));
                    }
                    catch (Exception ex)
                    {
                        return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_FORMAT_NOT_VALID(item.Name + $" - {ex.Message}"));
                    }
                }
                else
                {
                    if (item.Name == nameof(Order.UpdatedAt) ||
                        item.Name == nameof(Order.Customer) ||
                        item.Name == nameof(Order.Id))
                        continue;
                    return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));
                }
            }

            return new DataResponse { IsSuccessful = true };
        }
    }
}

