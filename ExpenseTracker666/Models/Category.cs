using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker666.Models
{
    public class Category
    {

        public ObjectId _id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
