using ExpenseTracker666.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Operations;

namespace ExpenseTracker666.Controllers
{
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add(string username, string password)
        {

            return View();
        }
    }
}
