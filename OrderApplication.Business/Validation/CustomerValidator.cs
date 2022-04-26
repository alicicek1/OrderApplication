using FluentValidation;
using System;

namespace OrderApplication.Business.Validation
{
    public class CustomerValidator : BaseValidator<OrderApplication.Model.Document.Customer>
    {
        public CustomerValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public void ValidateName()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name cannot be emty.");
        }
        public void ValidateEmail()
        {
            RuleFor(c => c.Email).NotEmpty().WithMessage("Email cannot be emty.");
        }
        public void ValidateAddress()
        {
            RuleFor(c => c.Address).NotEmpty().WithMessage("Address cannot be emty.");
        }
    }
}
