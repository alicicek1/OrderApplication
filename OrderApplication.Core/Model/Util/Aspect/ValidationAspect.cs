using Castle.DynamicProxy;
using FluentValidation;
using OrderApplication.Core.Extension;
using OrderApplication.Core.Interception;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Core.Tool;
using OrderApplication.Core.Validation;

namespace OrderApplication.Core.Model.Util.Aspect
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        private DataResponse result { get; set; } = new DataResponse();
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("fdsfs");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {

            var validator = (IValidator)Activator.CreateInstance(_validatorType, ServiceTool.ServiceProvider);
            var modelType = _validatorType.BaseType.BaseType.GetGenericArguments()[0];

            var model = invocation.Arguments.Where(t => t.GetType().BaseType == modelType).FirstOrDefault();

            if (model.IsNull())
                model = invocation.Arguments.Where(t => t.GetType() == modelType).FirstOrDefault();

            result = ValidationTool.Validate(validator, model);
            if (!result.IsSuccessful)
                invocation.ReturnValue = result;
            else invocation.Proceed();

        }
    }
}
