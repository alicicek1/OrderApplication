using FluentValidation;
using OrderApplication.Core.Model.Util.Response;

namespace OrderApplication.Core.Validation
{
    public static class ValidationTool
    {
        public static DataResponse Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
                return new DataResponse { IsSuccessful = false, ErrorMessageList = result.Errors.Select(x => x.ErrorMessage).ToList() };
            else return new DataResponse { IsSuccessful = true };
        }
    }
}
