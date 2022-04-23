using MongoDB.Bson.Serialization.Attributes;
using OrderApplication.Core.Model.Util.Attribute;

namespace OrderApplication.Model.Document
{
    [BsonCollection("Customer")]
    public class Customer : OrderApplication.Core.Model.Document.Document
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public DateTime CreatedAt { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? UpdatedAt { get; set; }
    }
}
