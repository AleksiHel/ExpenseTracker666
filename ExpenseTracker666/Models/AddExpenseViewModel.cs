namespace ExpenseTracker666.Models
{
    public class AddExpenseViewModel
    {
        // ViewModel tietää kaikki uniikit kategoriat tietokannasta
        // Täten saa ne frontendiin näppärästi esille
        // TODO lisää DataManipulator classiin, että saa sieltä helposti uniikit, kun kuitenkin käytän useassa kohtaa tätä...
        public List<Category> Categories { get; set; } = DatabaseManipulator.GetAll<Category>("Category").OrderBy(c => c.CategoryName).ToList();

        public Expense NewExpense { get; set; } = new Expense();

    }
}
