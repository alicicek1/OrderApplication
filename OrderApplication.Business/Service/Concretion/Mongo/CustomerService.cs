using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Validation.Customer;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.Mongo;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Model.Util.Aspect;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Model.Contant.Error;
using OrderApplication.Model.Document;
using OrderApplication.Model.Document.Common.Customer;
using OrderApplication.Model.Document.Common.Order;
using System.Net;
using System.Reflection;

namespace OrderApplication.Business.Service.Concretion.Mongo
{
    public class CustomerService : MongoService<Customer>, ICustomerService
    {
        private readonly IBusinessRuleEngine businessRuleEngine;

        public CustomerService(IMongoRepository<Customer> _mongoRepository, IBusinessRuleEngine businessRuleEngine) : base(_mongoRepository)
        {
            this.businessRuleEngine = businessRuleEngine;
        }

        [ValidationAspect(typeof(NewCustomerValidator))]
        public DataResponse Add(Customer document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckInsertProperties(document));
            if (!response.IsSuccessful)
                return response;

            Customer insertedCustomer = base.InsertOne(document);

            if (insertedCustomer != null)
                return new DataResponse { Document = insertedCustomer };
            else
                return new DataResponse { ErrorMessageList = new List<string> { "An error occured while inserting." }, ErrorCode = "", HttpStatusCode = HttpStatusCode.BadRequest };
        }

        [ValidationAspect(typeof(UpdateCustomerValidator))]
        public DataResponse Update(Customer document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckUpdateProperties(document));
            if (!response.IsSuccessful)
                return response;

            Customer updatedCustomer = base.ReplaceOne(document);

            if (updatedCustomer != null)
                return new DataResponse { Document = updatedCustomer };
            else
                return new DataResponse { ErrorMessageList = new List<string> { "An error occured while updated." }, ErrorCode = "", HttpStatusCode = HttpStatusCode.BadRequest };
        }

        private DataResponse CheckInsertProperties(Customer document)
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
                    if (document.GetType().Name == nameof(NewCustomerModel) && item.Name == nameof(Customer.Id))
                        continue;
                    if (item.Name == nameof(Customer.UpdatedAt))
                        continue;
                    return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));
                }
            }

            return new DataResponse { IsSuccessful = true };
        }

        private DataResponse CheckUpdateProperties(Customer document)
        {
            if (document == null)
                return ErrorDataResponse(OrderErrorConstant.MODEL_CANNOT_BE_NULL);

            if (string.IsNullOrWhiteSpace(document.Id))
                return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(nameof(document.Id)));

            if (this.FindByObjectId(document.Id) == null)
                return ErrorDataResponse(OrderErrorConstant.NOT_FOUND);

            return new DataResponse { IsSuccessful = true };
        }
    }
}
