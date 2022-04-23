using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderApplication.Core.Model.Document
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId ObjectId { get; set; }
    }
}
