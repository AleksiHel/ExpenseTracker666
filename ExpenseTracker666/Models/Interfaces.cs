using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ExpenseTracker666.Models
{
    public interface IHasId
    {
        [BsonId]
    [BsonRepresentation(BsonType.String)]
    public ObjectId _id { get; set; }
}
}
