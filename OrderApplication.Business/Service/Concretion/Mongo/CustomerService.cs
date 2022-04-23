using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Validation.Customer;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.Mongo;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Model.Util.Aspect;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Model.Contant.Error;
using OrderApplication.Model.Document;
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
        public override DataResponse InsertOne(Customer document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckProperties(document));
            if (!response.IsSuccessful)
                return response;

            response = base.InsertOne(document);

            return response;
        }

        [ValidationAspect(typeof(UpdateCustomerValidator))]
        public override DataResponse ReplaceOne(Customer document)
        {
            DataResponse response = businessRuleEngine.Validate(CheckProperties(document));
            if (!response.IsSuccessful)
                return response;

            response = base.ReplaceOne(document);

            return response;
        }

        private DataResponse CheckProperties(Customer document)
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
                    if (item.Name == nameof(Customer.UpdatedAt) ||
                        item.Name == nameof(Customer.Id))
                        continue;
                    return ErrorDataResponse(OrderErrorConstant.MODEL_PROPERTY_CANNOT_BE_NULL(item.Name));
                }
            }

            return new DataResponse { IsSuccessful = true };
        }
    }
}
