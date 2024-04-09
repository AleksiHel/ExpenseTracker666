using ExpenseTracker666.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker666.Controllers
{
    public class CategoryController : Controller
    {




        [Authorize(Roles = "admin")]

        public IActionResult Index()
        {
            var model = new CategoryViewModel
            {
                Categories = DatabaseManipulator.GetAll<Category>("Category").OrderBy(c => c.CategoryName).ToList()
                //NewCategory = new Category() // Initialize if needed
            };


            
            return View(model);
        }



        [HttpPost]

        public async Task<IActionResult> Index(CategoryViewModel model)


        {
            // Korjaa
            //var model2 = new CategoryViewModel
            //{
            //    Categories = DatabaseManipulator.GetAll<Category>("Category").OrderBy(c => c.Name).ToList()
            //    //NewCategory = new Category() // Initialize if needed
            //};


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var Category = new Category
            {
                CategoryName = model.NewCategory.CategoryName,
            };

            DatabaseManipulator.SavetoCategory(Category);


            return RedirectToAction("Index");
        }

        public IActionResult _TotalSpendingPage()
        {
            return PartialView();
        }
    }


}
