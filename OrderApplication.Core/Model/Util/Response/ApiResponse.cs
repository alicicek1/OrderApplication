using FluentValidation.Results;
using System.Net;

namespace OrderApplication.Core.Model.Util.Response
{
    public class ApiResponse
    {
        private List<ValidationFailure> _validationResult = new List<ValidationFailure>();

        public HttpStatusCode HttpStatusCode { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public string ErrorCode { get; set; }
        public List<string> ErrorMessageList { get; set; } = new List<string> { };
        public List<ValidationFailure> ValidationResult
        {
            get
            {
                if (_validationResult != null && _validationResult.Count > 0)
                    return _validationResult;
                else
                {
                    List<ValidationFailure> tempValidFailList = new List<ValidationFailure>();
                    if (ErrorMessageList.Any())
                    {
                        foreach (string errorItem in ErrorMessageList)
                        {
                            tempValidFailList.Add(new ValidationFailure("", "")
                            {
                                ErrorMessage = errorItem
                            });
                        }
                    }
                    _validationResult = tempValidFailList;
                    return _validationResult;
                }

            }
            set { this._validationResult = value; }
        }
    }
}
