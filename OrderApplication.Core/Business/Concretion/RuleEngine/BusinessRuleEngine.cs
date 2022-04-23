using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Model.Util.Response;

namespace OrderApplication.Core.Business.Concretion.RuleEngine
{
    public class BusinessRuleEngine : IBusinessRuleEngine
    {
        public DataResponse Validate(params DataResponse[] rules)
        {
            foreach (var result in rules)
            {
                if (!result.IsSuccessful)
                    return result;
            }
            return new DataResponse { IsSuccessful = true };
        }
    }
}
