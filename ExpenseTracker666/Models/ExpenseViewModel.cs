using MongoDB.Bson;

namespace ExpenseTracker666.Models
{
    public class ExpenseViewModel
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string UserName { get; set; } 
        public ObjectId UserId { get; set; }

        public string CategoryName { get; set; }
        public ObjectId CategoryId { get; set; }

        // Viewmodeli tuntee uniikit categoriat, että ne on valmiina addissa
        // tartte luoda omaa viewmodelia sitten adille
        // Ei tartte
        //public List<Category> UniqueCategories { get; set; } = DatabaseManipulator.GetAll<Category>("Category").OrderBy(c => c.CategoryName).ToList();
    }


}
