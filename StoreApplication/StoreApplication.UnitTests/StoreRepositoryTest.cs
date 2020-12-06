
using Microsoft.EntityFrameworkCore;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class StoreRepositoryTest
    {

        // repository only handles data logic, if conditions are all in controllers
        // test get methods first to avoid duplicate records/null references
        [Fact]
        public void Database_GetAProductByNameAndCategory()
        {
            // arrange
            /*
            IServiceCollection services;
            services.AddDbContext<AspDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("optimumDB")));
            */

            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            var option = optionsBuilder.Options;
            string name = "diet coke";
            string category = "drink";
            CProduct searchedProduct;
            using (var context1 = new Project0databaseContext(option))
            {
                IStoreRepository repo = new StoreRepository(option);
                // action
                searchedProduct = repo.GetOneProductByNameAndCategory(name, category);
            }
            // assert
            using var context2 = new Project0databaseContext(option);
            var dbProduct = context2.Products.Find("p101");
            Assert.Equal(dbProduct.Price, searchedProduct.Price);
        }

        [Fact]
        public void Database_GetOneCusomerByEmail()
        {
            // arrange
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            var option = optionsBuilder.Options;
            string email = "JSmith@gmail.com";
            CCustomer searchedCustomer;
            using (var context1 = new Project0databaseContext(option))
            {
                IStoreRepository repo = new StoreRepository(option);
                // action
                searchedCustomer = repo.GetOneCustomerByEmail(email);
            }
            // assert
            using var context2 = new Project0databaseContext(option);
            var dbCustomer = context2.Customers.Find("cus1");
            Assert.Equal(dbCustomer.Firstname, searchedCustomer.FirstName);
            Assert.Equal(dbCustomer.Lastname, searchedCustomer.LastName);
            Assert.Equal(dbCustomer.Phonenumber, searchedCustomer.PhoneNumber);
        }

        [Fact]
        public void Database_AddOneStore()
        {
            // setup
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            var option = optionsBuilder.Options;
            CStore newStore = new CStore("Techland China 5", "6026626662", "84561");
            using (var context1 = new Project0databaseContext(option))
            {
                IStoreRepository repo = new StoreRepository(option);
                // action
                repo.AddOneStore(newStore);
            }
            // asert
            using var context2 = new Project0databaseContext(option);
            var dbStore = context2.Stores.FirstOrDefault(x => x.Storeloc == "Techland China 5");
            Assert.Equal(newStore.Storephone, dbStore.Storephone);
            Assert.Equal(newStore.Zipcode, dbStore.Zipcode);
            Assert.Empty(dbStore.Storecustomers);
        }

        [Fact]
        public void Database_DeleteOneStore()
        {
            // setup
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            var option = optionsBuilder.Options;
            string storeLoc = "Techland China 5";
            using (var context1 = new Project0databaseContext(option))
            {
                IStoreRepository repo = new StoreRepository(option);
                // action
                repo.DeleteOneStore(storeLoc);
            }
            // asert
            using var context2 = new Project0databaseContext(option);
            var dbStore = context2.Stores.FirstOrDefault(x => x.Storeloc == "Techland China 5");
            Assert.Null(dbStore);
        }

        // SQLite
        /*
        [Fact]
        public void DBAddOneStore()
        {
            // arrange
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var option = new DbContextOptionsBuilder<Project0databaseContext>().UseSqlite(connection).Options;
            CStore newStore = new CStore("Mountain View 1", "6026626662");

            using (var context1 = new Project0databaseContext(option))
            {
                IStoreRepository repo = new StoreRepository(option);
                // action
                repo.AddOneStore(newStore);
            }
            // asert
            using var context2 = new Project0databaseContext(option);
            var dbStore = context2.Stores.First(x => x.Storeloc == "Mountain View 1");
            Assert.Equal(newStore.Storephone, dbStore.Storephone);
            Assert.Empty(dbStore.Storecustomers);
        }
        */

        static string GetConnectionString()
        {
            string path = "../../../pcs.json";
            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"required file {path} not found. should just be the connection string in quotes.");
                throw;
            }
            string connectionString = JsonSerializer.Deserialize<string>(json);
            return connectionString;
        }


    }
}
