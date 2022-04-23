using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderApplication.Core.Model.Document
{
    public class Document : IDocument
    {
        public ObjectId ObjectId { get; set; }
    }
}
