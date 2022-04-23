
namespace OrderApplication.Business.Validation.Order
{
    public class UpdateOrderValidator : OrderValidator
    {
        public UpdateOrderValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ValidatePk();
            ValidateAll();
        }
    }
}
