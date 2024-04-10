using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker666.Models
{
    public class Expense
    {

        public ObjectId _id { get; set; }
        [Required]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime ExpenseDate { get; set; }
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId UserId { get; set; }
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId CategoryId { get; set; }
        
    }
}
