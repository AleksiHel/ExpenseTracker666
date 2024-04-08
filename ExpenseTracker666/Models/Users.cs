using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker666.Models
{
    public class Users
    {

        public ObjectId _id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
