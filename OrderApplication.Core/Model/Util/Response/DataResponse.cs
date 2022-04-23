using MongoDB.Bson.Serialization.Attributes;
using OrderApplication.Core.Model.Document;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OrderApplication.Core.Model.Util.Response
{
    public class DataResponse : ApiResponse
    {
        private IDocument? _data;

        public long? PkId { get; set; }
        public IDocument? Document
        {
            get { return _data; }
            set
            {
                _data = value;
                if (_data != null)
                {
                    this.IsSuccessful = true;
                    var findPkIdAttr = _data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .FirstOrDefault(x => x.GetCustomAttribute<BsonIdAttribute>() != null || x.GetCustomAttribute<RequiredAttribute>() != null)?.GetValue(_data);
                    if (findPkIdAttr != null)
                        this.PkId = int.Parse(findPkIdAttr.ToString());
                }
                else if (!IsSuccessful && (this.ErrorMessageList == null || this.ErrorMessageList.Count == 0))
                {
                    this.ErrorMessageList = new List<string> { "Not found." };
                }
            }
        }
    }
}
