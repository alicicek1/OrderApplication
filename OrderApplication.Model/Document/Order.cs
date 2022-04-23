using MongoDB.Bson.Serialization.Attributes;
using OrderApplication.Core.Model.Util.Attribute;

namespace OrderApplication.Model.Document
{
    [BsonCollection("Order")]
    public class Order : OrderApplication.Core.Model.Document.Document
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CustomerId { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public Address Address { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? UpdatedAt { get; set; }

        [BsonIgnore]
        public Customer? Customer { get; set; }
    }
}
