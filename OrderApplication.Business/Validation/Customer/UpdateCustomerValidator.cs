
using System;

namespace OrderApplication.Business.Validation.Customer
{
    public class UpdateCustomerValidator : CustomerValidator
    {
        public UpdateCustomerValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ValidatePk();
            ValidateAll();
        }
    }
}
