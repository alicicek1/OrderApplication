
using FluentValidation;

namespace OrderApplication.Business.Validation
{
    public class OrderValidator : BaseValidator<OrderApplication.Model.Document.Order>
    {
        public OrderValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public void ValidateCustomerId()
        {
            RuleFor(c => c.CustomerId).NotEmpty().WithMessage("Order must be releates to a customer.");
        }

        public void ValidateQuantity()
        {
            RuleFor(c => c.Quantity).NotEmpty().WithMessage("Quantity cannot be null.");
        }

        public void ValidatePrice()
        {
            RuleFor(c => c.Price).NotEmpty().WithMessage("Price cannot be null.");
        }

        public void ValidateStatus()
        {
            RuleFor(c => c.Status).NotEmpty().WithMessage("Status cannot be null.");
        }

        public void ValidateAddress()
        {
            RuleFor(c => c.Address).NotEmpty().WithMessage("Address cannot be null.");
        }

        public void ValidateProduct()
        {
            RuleFor(c => c.Product).NotEmpty().WithMessage("Product cannot be null.");
        }
    }
}
