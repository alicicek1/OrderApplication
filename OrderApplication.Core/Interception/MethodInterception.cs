using Castle.DynamicProxy;
using OrderApplication.Core.Model.Util.Attribute;
using OrderApplication.Core.Model.Util.Response;

namespace OrderApplication.Core.Interception
{
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual DataResponse OnAfter(IInvocation invocation)
        {
            return new DataResponse { };
        }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }

        public override void Intercept(IInvocation invocation)
        {
            OnBefore(invocation);
        }
    }
}
