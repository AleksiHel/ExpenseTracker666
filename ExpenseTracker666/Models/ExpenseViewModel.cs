using MongoDB.Bson;

namespace ExpenseTracker666.Models
{
    public class ExpenseViewModel
    {
        public ObjectId ExpenseId {  get; set; } 
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string UserName { get; set; } 
        public ObjectId UserId { get; set; }

        public string CategoryName { get; set; }
        public ObjectId CategoryId { get; set; }

    }


}
