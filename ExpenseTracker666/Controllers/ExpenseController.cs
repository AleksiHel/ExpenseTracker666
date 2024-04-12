using ExpenseTracker666.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System.Text.Json;


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
                ExpenseId = expense._id,
                Amount = expense.Amount,
                Description = expense.Description,
                ExpenseDate = expense.ExpenseDate,
                CategoryName = categoryDictionary[expense.CategoryId.ToString()],
                UserName = User.Identity.Name
            }).ToList();

            // Sort by date, reverse :)
            expenseViewModels = expenseViewModels.OrderByDescending(x => x.ExpenseDate).ToList();


            return View(expenseViewModels) ;
        }


        [Authorize]
        public IActionResult Add()
        {
            var model = new AddExpenseViewModel();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            loggedInUserId = DatabaseManipulator.GetUsersID(User.Identity.Name);

                
            var Expense = new Expense
            {
                Amount = model.Amount,
                Description = model.Description,
                ExpenseDate = model.ExpenseDate,
                UserId = loggedInUserId,
                // Hae backendistä Idn arvo kategorian nimen perusteella
                CategoryId = DatabaseManipulator.GetCategoryId(model.CategoryName)

            };

            DatabaseManipulator.Save("Expenses", Expense);


            return RedirectToAction("");
        }

        [Authorize]
        public IActionResult Graphs()
        {

            var categories = DatabaseManipulator.GetAll<Category>("Category");
            var categoryDictionary = categories.ToDictionary(c => c._id.ToString(), c => c.CategoryName);

            loggedInUserId = DatabaseManipulator.GetUsersID(User.Identity.Name);
            var userData = DatabaseManipulator.GetAllById<Expense>(loggedInUserId);

            var graphViewModels = userData.Select(expense => new GraphViewModel
            {
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                CategoryName = categoryDictionary[expense.CategoryId.ToString()],
            }).ToList();


            return View(graphViewModels);




        }

        [Authorize]
        public IActionResult GetAllPurchases()
        {
           // TODO DRY
            var categories = DatabaseManipulator.GetAll<Category>("Category");
            var categoryDictionary = categories.ToDictionary(c => c._id.ToString(), c => c.CategoryName);



            loggedInUserId = DatabaseManipulator.GetUsersID(User.Identity.Name);
            var userData = DatabaseManipulator.GetAllById<Expense>(loggedInUserId);




            var expenseViewModels = userData.Select(expense => new ExpenseViewModel
            {
                ExpenseId = expense._id,
                Amount = expense.Amount,
                Description = expense.Description,
                ExpenseDate = expense.ExpenseDate,
                CategoryName = categoryDictionary[expense.CategoryId.ToString()],
                UserName = User.Identity.Name
            }).ToList();

            // Sort by date :)
            expenseViewModels = expenseViewModels.OrderByDescending(x => x.ExpenseDate).ToList();
            return PartialView("_AllPurchases", expenseViewModels);
        }


    }
}
