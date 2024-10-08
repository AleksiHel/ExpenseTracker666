﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using static MongoDB.Driver.WriteConcern;

namespace ExpenseTracker666.Models
{
    public class DatabaseManipulator
    {
        private static IConfiguration? config;
        public static string? DATABASE_NAME;
        private static string? HOST;
        private static MongoServerAddress? address;
        private static MongoClientSettings? settings;
        private static MongoClient? client;
        public static IMongoDatabase? database;
        public static string? ConnectionString;



        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
            var sections = config.GetSection("ConnectionStrings");
            DATABASE_NAME = sections.GetValue<string>("DatabaseName");
            HOST = sections.GetValue<string>("MongoConnection");
            address = new MongoServerAddress(HOST);
            settings = new MongoClientSettings() { Server = address };
            client = new MongoClient(settings);
            database = client.GetDatabase(DATABASE_NAME);
            ConnectionString = $"mongodb://{HOST}:27017";

        }

        // TODO
        // Korjaa, buginen
        public static T Save<T>(T record)
        {
            try
            {
                // taulun nimi objektista
                var collection = database.GetCollection<T>(typeof(T).Name);


                PropertyInfo nameProp = typeof(T).GetProperty("Username");
                string nameValue = nameProp.GetValue(record)?.ToString();

                // Luo filtteri ja hae onko taulussa
                // pakko olla fiksumpi tapa? Ainakin searchia voisi käyttää apu metodina, mutta nyt ei jaksanut implementoida
                // ehkä sitäkin parempi ja siistimpi tapa varmasti on
                var filter = Builders<T>.Filter.Eq("Username", nameValue);
                var existingRecord = collection.Find(filter).FirstOrDefault();


                if (existingRecord != null)
                {
                    // ´Hae _id jo löytyvästä
                    PropertyInfo idProp = existingRecord.GetType().GetProperty("_id");


                    if (idProp != null)
                    {
                        var idValue = idProp.GetValue(existingRecord);

                        // Jos property _id aseta tässä
                        PropertyInfo recordIdProp = typeof(T).GetProperty("_id");

                        // kolmas if aika kys setit
                        if (recordIdProp != null)
                        {
                            recordIdProp.SetValue(record, idValue);
                        }

                        // luo filtteri replaceemiseen
                        // ei voi muuta sanoa kuin vittu mikä syntaxi
                        // no mut onneksi on nyt koodi esimerkki tallessa
                        var filter2 = Builders<T>.Filter.Eq("_id", idValue);
                        collection.ReplaceOne(filter2, record, new ReplaceOptions { IsUpsert = true });
                        Console.WriteLine($"Record updated with _id: {idValue}");
                    }
                }
                // jos ei oo taulussa case
                else
                {
                    collection.InsertOne(record);
                    Console.WriteLine($"{nameValue} not found in db, inserted new record.");
                }

                return record;

            }
            catch (Exception e) { Console.WriteLine(e); }

            return record;
        }

        // väliaikainen
        public static T SavetoCategory<T>(T record)
        {
            try
            {
                // taulun nimi objektista
                var collection = database.GetCollection<T>(typeof(T).Name);


                PropertyInfo nameProp = typeof(T).GetProperty("CategoryName");
                string nameValue = nameProp.GetValue(record)?.ToString();

                // Luo filtteri ja hae onko taulussa
                // pakko olla fiksumpi tapa? Ainakin searchia voisi käyttää apu metodina, mutta nyt ei jaksanut implementoida
                // ehkä sitäkin parempi ja siistimpi tapa varmasti on
                var filter = Builders<T>.Filter.Eq("CategoryName", nameValue);
                var existingRecord = collection.Find(filter).FirstOrDefault();


                if (existingRecord != null)
                {
                    // ´Hae _id jo löytyvästä
                    PropertyInfo idProp = existingRecord.GetType().GetProperty("_id");


                    if (idProp != null)
                    {
                        var idValue = idProp.GetValue(existingRecord);

                        // Jos property _id aseta tässä
                        PropertyInfo recordIdProp = typeof(T).GetProperty("_id");

                        // kolmas if aika kys setit
                        if (recordIdProp != null)
                        {
                            recordIdProp.SetValue(record, idValue);
                        }

                        // luo filtteri replaceemiseen
                        // ei voi muuta sanoa kuin vittu mikä syntaxi
                        // no mut onneksi on nyt koodi esimerkki tallessa
                        var filter2 = Builders<T>.Filter.Eq("_id", idValue);
                        collection.ReplaceOne(filter2, record, new ReplaceOptions { IsUpsert = true });
                        Console.WriteLine($"Record updated with _id: {idValue}");
                    }
                }
                // jos ei oo taulussa case
                else
                {
                    collection.InsertOne(record);
                    Console.WriteLine($"{nameValue} not found in db, inserted new record.");
                }

                return record;

            }
            catch (Exception e) { Console.WriteLine(e); }

            return record;
        }






        public static bool CheckPassword(string username, string password)
        {

            try
            {
                var mongoCollection = database.GetCollection<BsonDocument>("Users");
                var filter = Builders<BsonDocument>.Filter.Eq("Username", username) & Builders<BsonDocument>.Filter.Eq("Password", password);

                var existingRecord = mongoCollection.Find(filter).FirstOrDefault();

                return existingRecord != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }



        public static List<T> GetAll<T>(string table)
        {
            var mongotable = database.GetCollection<T>(table);
            return mongotable.Find(new BsonDocument()).ToList();
        }

        public static ObjectId GetUsersID(string username)
        {
            var mongotable = database.GetCollection<Users>("Users");
            var filter = Builders<Users>.Filter.Eq("Username", username);
            var user =  mongotable.Find(filter).FirstOrDefault();

            return user._id;
        }

        public static List<T> GetAllById<T>(ObjectId ID)
        {
            var mongotable = database.GetCollection<T>("Expenses");

            var filter = Builders<T>.Filter.Eq("UserId", ID);
            var testi = mongotable.Find(filter).ToList();
            return testi;

        }

       public static List<T> GetExpenseById<T>(ObjectId ID)
        {
            var mongotable = database.GetCollection<T>("Expenses");

            var filter = Builders<T>.Filter.Eq("_id", ID);
            var testi = mongotable.Find(filter).ToList();
            return testi;

        }

        public static ObjectId GetCategoryId(string categoryName)
        {
            var mongotable = database.GetCollection<Category>("Category");
            var filter = Builders<Category>.Filter.Eq("CategoryName", categoryName);
            var category = mongotable.Find(filter).FirstOrDefault();

            return category._id;
        }



        public static T Save<T>(string table, T record)
        {
            var mongotable = database.GetCollection<T>(table);
            try { mongotable.InsertOne(record); } 
            catch { Console.WriteLine("Error while saving");  }
            return record;
        }


        public static T EditExpense<T>(T record, ObjectId IdValue)
        {
            var mongotable = database.GetCollection<T>("Expenses");
            var filter = Builders<T>.Filter.Eq("_id", IdValue);




            try { mongotable.ReplaceOne(filter, record, new ReplaceOptions { IsUpsert = true }); }
            catch { Console.WriteLine("Error while saving"); }
            return record;
        }

        public static bool DeleteExpense(ObjectId IdValue)
        {
            var mongotable = database.GetCollection<Expense>("Expenses");
            var filter = Builders<Expense>.Filter.Eq("_id", IdValue);

            try
            {
                mongotable.DeleteOne(filter);
            } catch
                { Console.WriteLine("Error while deleting");
                return false;
            }

            return true;

        }



        //public static ObjectId(string username)
        //{
        //    var mongotable = database.GetCollection<T>(username);

        //    var filter = Builders<T>.Filter.Eq("Name", username);

        //    string testi;

        //    mongotable = mongotable.Find(filter).ToList();


        //}


        //public static T GetByObjectId<T>(string table, ObjectId id)
        //{
        //    var mongotable = database.GetCollection<T>(table);
        //    var filter = Builders<T>.Filter.Eq("_id", id);
        //    return mongotable.Find(filter).FirstOrDefault();
        //}

        ////    public static bool LogInChecker<T>(string username, string password)
        ////{
        ////    var mongotable = database.GetCollection<T>("Users");

        ////    try
        ////    {
        ////        var testi = mongotable.Find(username).FirstOrDefault();
        ////        return true;
        ////    } catch

        ////    {
        ////        return false;
        ////    }


        ////}
    }

        
    }


