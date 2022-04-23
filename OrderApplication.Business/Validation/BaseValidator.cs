using FluentValidation;
using OrderApplication.Core.Model.Document;
using System.Reflection;

namespace OrderApplication.Business.Validation
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel>
        where TModel : Document
    {
        #region Variables

        public readonly IServiceProvider ServiceProvider;
        #endregion

        public BaseValidator(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;

            CascadeMode = CascadeMode.StopOnFirstFailure;
        }

        public void ValidateAll()
        {
            MethodInfo[] methods = this.GetType().GetMethods();

            if (methods != null && methods.Count() > 0)
            {
                foreach (MethodInfo method in methods)
                {
                    if (method.Name.ToLower().Contains("validate") &&
                       !method.Name.ToLower().Contains("validateall") &&
                        method.Name.ToLower() != "validate" &&
                       !method.IsVirtual)
                    {
                        if (!method.IsStatic)
                            method.Invoke(this, null);
                        else
                            method.Invoke(null, null);
                    }
                }
            }
        }

        public virtual void ValidatePk()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id not found.");
        }
    }
}
