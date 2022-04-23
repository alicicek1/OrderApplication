using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OrderApplication.Core.Model.Document
{
    public interface IDocument
    {
        [Required]
        [BsonRequired]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }

    }
}
