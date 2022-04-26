using Castle.DynamicProxy;

namespace OrderApplication.Core.Model.Util.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : System.Attribute, IInterceptor
    {
        public virtual void Intercept(IInvocation invocation)
        {

        }
    }
}
