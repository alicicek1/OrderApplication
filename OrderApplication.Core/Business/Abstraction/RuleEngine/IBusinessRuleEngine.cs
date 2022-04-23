using OrderApplication.Core.Model.Util.Response;

namespace OrderApplication.Core.Business.Abstraction.RuleEngine
{
    public interface IBusinessRuleEngine
    {
        DataResponse Validate(params DataResponse[] rules);
    }
}
