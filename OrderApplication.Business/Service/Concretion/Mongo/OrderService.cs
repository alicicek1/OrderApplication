using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Validation.Order;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.Mongo;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Model.Util.Aspect;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Model.Contant.Error;
using OrderApplication.Model.Document;
using OrderApplication.Model.Document.Common.Customer;
using OrderApplication.Model.Document.Common.Order;
using OrderApplication.Model.Util.Request;
using System.Net;
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
        public DataResponse Add(Order document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckInsertProperties(document));
            if (!response.IsSuccessful)
                return response;

            Order insertedOrder = base.InsertOne(document);

            if (insertedOrder != null)
                return new DataResponse { Document = insertedOrder };
            else
                return new DataResponse { ErrorMessageList = new List<string> { "An error occured while inserting." }, ErrorCode = "", HttpStatusCode = HttpStatusCode.BadRequest };
        }

        [ValidationAspect(typeof(UpdateOrderValidator))]
        public DataResponse Update(Order document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckUpdateProperties(document));
            if (!response.IsSuccessful)
                return response;

            Order updatedOrder = base.ReplaceOne(document);

            if (updatedOrder != null)
                return new DataResponse { Document = updatedOrder };
            else
                return new DataResponse { ErrorMessageList = new List<string> { "An error occured while updated." }, ErrorCode = "", HttpStatusCode = HttpStatusCode.BadRequest };
        }

        public DataResponse ChangeStatus(ChangeStatusRequestModel changeStatusRequestModel)
        {
            DataResponse response = businessRuleEngine.Validate(CheckChangeStatusRequestModel(changeStatusRequestModel));
            if (!response.IsSuccessful)
                return response;

            TempOrder.Status = changeStatusRequestModel.Status;
            TempOrder.UpdatedAt = DateTime.Now;

            response = this.Update(TempOrder);

            return response;
        }

        private DataResponse CheckChangeStatusRequestModel(ChangeStatusRequestModel changeStatusRequestModel)
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

        private DataResponse CheckInsertProperties(Order document)
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
                    if (document.GetType().Name == nameof(NewOrderModel) && item.Name == nameof(Customer.Id))
                        continue;
                    if (item.Name == nameof(Order.UpdatedAt) ||
                        item.Name == nameof(Order.Customer))
                        continue;
                    return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));
                }
            }

            return new DataResponse { IsSuccessful = true };
        }
        private DataResponse CheckUpdateProperties(Order document)
        {
            if (document == null)
                return ErrorDataResponse(OrderErrorConstant.MODEL_CANNOT_BE_NULL);

            if (string.IsNullOrWhiteSpace(document.Id))
                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(nameof(document.Id)));

            TempOrder = this.FindByObjectId(document.Id);
            if (TempOrder == null)
                return ErrorDataResponse(OrderErrorConstant.NOT_FOUND);

            return new DataResponse { IsSuccessful = true };
        }
    }
}

