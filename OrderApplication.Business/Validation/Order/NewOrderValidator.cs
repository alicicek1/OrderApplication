

namespace OrderApplication.Business.Validation.Order
{
    public class NewOrderValidator : OrderValidator
    {
        public NewOrderValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ValidateAll();
        }
    }
}
