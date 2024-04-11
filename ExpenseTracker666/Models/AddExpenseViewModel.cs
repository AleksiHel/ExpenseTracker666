using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker666.Models
{
    public class AddExpenseViewModel
    {
        // ViewModel tietää kaikki uniikit kategoriat tietokannasta
        // Täten saa ne frontendiin näppärästi esille
        // TODO lisää DataManipulator classiin, että saa sieltä helposti uniikit, kun kuitenkin käytän useassa kohtaa tätä... DRY
        public List<Category> Categories { get; set; } = DatabaseManipulator.GetAll<Category>("Category").OrderBy(c => c.CategoryName).ToList();

        [Required(ErrorMessage = "Enter valid value")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Enter valid description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Choose valid date")]
        public DateTime ExpenseDate { get; set; }
        [Required(ErrorMessage = "Choose valid category")]

        public string CategoryName { get; set; }
    }
}
