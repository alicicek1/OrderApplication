using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OrderApplication.Core.Model.Document
{
    public class Document : IDocument
    {
        [BsonId]
        public string? Id { get; set; }
    }
}
