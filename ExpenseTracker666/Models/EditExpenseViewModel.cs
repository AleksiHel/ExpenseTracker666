using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker666.Models
{
    public class EditExpenseViewModel
    {
        public List<Category> Categories { get; set; } = DatabaseManipulator.GetAll<Category>("Category").OrderBy(c => c.CategoryName).ToList();

        [Required(ErrorMessage = "Enter valid value")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Enter valid description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Choose valid date")]
        public DateTime ExpenseDate { get; set; }
        [Required(ErrorMessage = "Choose valid category")]

        public string CategoryName { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]

        public ObjectId ExpenseId { get; set; }

    }
}
