namespace ExpenseTracker666.Models
{
    public class ExpenseViewModel
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string UserName { get; set; } 
        public string CategoryName { get; set; }
    }


}
