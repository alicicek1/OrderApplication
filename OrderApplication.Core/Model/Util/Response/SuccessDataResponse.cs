using OrderApplication.Core.Model.Document;

namespace OrderApplication.Core.Model.Util.Response
{
    public class SuccessDataResponse : DataResponse
    {
        public SuccessDataResponse(IDocument document)
        {
            this.IsSuccessful = true;
            this.Document = document;
        }
    }
}
