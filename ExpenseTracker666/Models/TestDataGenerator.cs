using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration.UserSecrets;
using MongoDB.Bson;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExpenseTracker666.Models
{
    public class TestDataGenerator
    {
        static private List<string> Usernames = ["admin", "Testaaja", "Matti", "Esko", "Teppo", "Aarne", "Topi", "Risto", "Simo", "Satu"];
        static private List<string> Categories = ["Cocaine", "Food", "Car", "Hobbies", "Children", "Alcohol", "Clothes"];
        static private string PlaceholderDescription = "Lorem ipsum dolor sit amet.";

        public Category Category { get; set; } = new Category();
        public Users Users { get; set; } = new Users();

        public Expense Expenses { get; set; } = new Expense();


        public static bool PopulateDb()
        {
            Random rnd = new Random();
            DateTime today  = DateTime.Today;

            // create users
            foreach (var x in Usernames)
            {
                var registerModel = new Users
                {
                    Username = x.ToLower(),
                    Password = x.ToLower()
                };
                DatabaseManipulator.Save("Users", registerModel);
            }

            // create categories
            foreach (var x in Categories)
            {
                var registerModel = new Category
                {
                    CategoryName = x.ToLower()
                };

                DatabaseManipulator.Save("Category", registerModel);
            }

            // create expenses

            for (var i = 0; i < 200; i++)
            {
                int amount = rnd.Next(1, 1000);
                int index = rnd.Next(Usernames.Count);
                string user = Usernames[index].ToLower();
                index = rnd.Next(Categories.Count);
                string category = Categories[index].ToLower();
                DateTime somedate = today.AddDays(rnd.Next(-60, 0));
                ObjectId userid = DatabaseManipulator.GetUsersID(user);
                ObjectId categoryid = DatabaseManipulator.GetCategoryId(category);

                var Expense = new Expense
                {
                    Amount = amount,
                    Description = PlaceholderDescription,
                    ExpenseDate = somedate,
                    UserId = userid,
                    CategoryId = categoryid
                };

                DatabaseManipulator.Save("Expenses", Expense);
            }






            return true;
        }


    

    }
}
