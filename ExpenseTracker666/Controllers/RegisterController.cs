using ExpenseTracker666.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker666.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Index(Users model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registerModel = new Users
            {
                Name = model.Name,
                Password = model.Password
            };

            DatabaseManipulator.Save(registerModel);


            return RedirectToAction("Index", "Home");
        }

    }
}
