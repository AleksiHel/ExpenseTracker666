using ExpenseTracker666.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;

namespace ExpenseTracker666.Controllers
{
    public class ExpenseController : Controller
    {

        ObjectId loggedInUserId;


        [Authorize]
        public IActionResult Index()        
        {
            
            var categories = DatabaseManipulator.GetAll<Category>("Category");
            var categoryDictionary = categories.ToDictionary(c => c._id.ToString(), c => c.CategoryName);



            loggedInUserId = DatabaseManipulator.GetUsersID(User.Identity.Name);
            var userData = DatabaseManipulator.GetAllById<Expense>(loggedInUserId);




            var expenseViewModels = userData.Select(expense => new ExpenseViewModel
            {
                Amount = expense.Amount,
                Description = expense.Description,
                ExpenseDate = expense.ExpenseDate,
                CategoryName = categoryDictionary[expense.CategoryId.ToString()],
                UserName = User.Identity.Name
            }).ToList();

            return View(expenseViewModels);
        }


        [Authorize]
        public IActionResult Add()
        {

            return View();
        }

        [Authorize]
        public IActionResult _AllPurchases(int number)
        {
            return PartialView();
        }

    }
}
