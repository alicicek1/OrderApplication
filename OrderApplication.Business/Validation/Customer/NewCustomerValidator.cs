
using System;

namespace OrderApplication.Business.Validation.Customer
{
    public class NewCustomerValidator : CustomerValidator
    {
        public NewCustomerValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ValidateAll();
        }
    }
}
