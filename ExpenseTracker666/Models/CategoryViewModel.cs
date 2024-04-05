namespace ExpenseTracker666.Models
{
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public Category NewCategory { get; set; } = new Category();
    }
}
