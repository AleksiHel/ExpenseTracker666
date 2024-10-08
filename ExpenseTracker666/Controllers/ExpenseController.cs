﻿using ExpenseTracker666.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using MongoDB.Driver.Linq;
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


            return View(expenseViewModels);
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
                ExpenseDate = model.ExpenseDate.AddDays(1),
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
        [Authorize]
        [HttpGet]
        public ActionResult Edit(ObjectId id)
        {

            var categories = DatabaseManipulator.GetAll<Category>("Category");
            var categoryDictionary = categories.ToDictionary(c => c._id.ToString(), c => c.CategoryName);

            Console.WriteLine(id);
            var expense = DatabaseManipulator.GetExpenseById<Expense>(id);
            Console.WriteLine(expense);


            var model = new EditExpenseViewModel
            {
                Amount = expense[0].Amount,
                Description = expense[0].Description,
                ExpenseDate = expense[0].ExpenseDate,
                CategoryName = categoryDictionary[expense[0].CategoryId.ToString()],
                ExpenseId = id
            };


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            loggedInUserId = DatabaseManipulator.GetUsersID(User.Identity.Name);

            var x = model.ExpenseId;

            var Expense = new Expense
            {
                _id = model.ExpenseId,
                Amount = model.Amount,
                Description = model.Description,
                // jostain syystä antaa 1 päivän aikasemman zzz, korjaa
                ExpenseDate = model.ExpenseDate.AddDays(1),
                UserId = loggedInUserId,
                // Hae backendistä Idn arvo kategorian nimen perusteella
                CategoryId = DatabaseManipulator.GetCategoryId(model.CategoryName)

            };

            DatabaseManipulator.EditExpense(Expense, model.ExpenseId);


            return RedirectToAction("");
        }

        [Authorize]
        public ActionResult Delete(ObjectId id)
        {

            var categories = DatabaseManipulator.GetAll<Category>("Category");
            var categoryDictionary = categories.ToDictionary(c => c._id.ToString(), c => c.CategoryName);

            Console.WriteLine(id);
            var expense = DatabaseManipulator.GetExpenseById<Expense>(id);
            Console.WriteLine(expense);


            var model = new EditExpenseViewModel
            {
                Amount = expense[0].Amount,
                Description = expense[0].Description,
                ExpenseDate = expense[0].ExpenseDate,
                CategoryName = categoryDictionary[expense[0].CategoryId.ToString()],
                ExpenseId = id
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditExpenseViewModel model)
        {


            DatabaseManipulator.DeleteExpense(model.ExpenseId);


            return RedirectToAction("");
        }

    }
}
