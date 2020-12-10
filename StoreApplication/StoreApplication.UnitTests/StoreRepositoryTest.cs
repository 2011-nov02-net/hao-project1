using Microsoft.Extensions;
using Microsoft.EntityFrameworkCore;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

using Microsoft.Extensions.Configuration;

 

namespace StoreApplication.UnitTests
{ 
    public class StoreRepositoryTest
    {           
        // repository only handles data logic, if conditions are all in controllers       
        [Fact]
        public void Database_GetAProductByNameAndCategory()
        {
            // arrange               
            string name = "Dying Light";
            string category = "Game";
            CProduct searchedProduct;
            using (var context1 = new Project0databaseContext(DbOptionsFactory.DbContextOptions))
            {
                IStoreRepository repo = new StoreRepository(DbOptionsFactory.DbContextOptions);
                // action
                searchedProduct = repo.GetOneProductByNameAndCategory(name, category);
            }
            // assert
            using var context2 = new Project0databaseContext(DbOptionsFactory.DbContextOptions);
            var dbProduct = context2.Products.Find("p101");
            Assert.Equal(dbProduct.Price, searchedProduct.Price);
        }

        [Fact]
        public void Database_GetOneCusomerByEmail()
        {
            // arrange            
            string email = "JSmith@gmail.com";
            CCustomer searchedCustomer;
            using (var context1 = new Project0databaseContext(DbOptionsFactory.DbContextOptions))
            {
                IStoreRepository repo = new StoreRepository(DbOptionsFactory.DbContextOptions);
                // action
                searchedCustomer = repo.GetOneCustomerByEmail(email);
            }
            // assert
            using var context2 = new Project0databaseContext(DbOptionsFactory.DbContextOptions);
            var dbCustomer = context2.Customers.Find("cus1");
            Assert.Equal(dbCustomer.Firstname, searchedCustomer.FirstName);
            Assert.Equal(dbCustomer.Lastname, searchedCustomer.LastName);
            Assert.Equal(dbCustomer.Phonenumber, searchedCustomer.PhoneNumber);
        }

        [Fact]
        public void Database_AddOneStore()
        {
            // setup
            
            CStore newStore = new CStore("Techland Phoenix 5", "6025555555", "85281");
            using (var context1 = new Project0databaseContext(DbOptionsFactory.DbContextOptions))
            {
                IStoreRepository repo = new StoreRepository(DbOptionsFactory.DbContextOptions);
                // action
                repo.AddOneStore(newStore);
            }
            // asert
            using var context2 = new Project0databaseContext(DbOptionsFactory.DbContextOptions);
            var dbStore = context2.Stores.FirstOrDefault(x => x.Storeloc == "Techland Phoenix 5");
            Assert.Equal(newStore.Storephone, dbStore.Storephone);
            Assert.Equal(newStore.Zipcode, dbStore.Zipcode);
            Assert.Empty(dbStore.Storecustomers);
        }

        [Fact]
        public void Database_DeleteOneStore()
        {
            // setup
            
            string storeLoc = "Techland Phoenix 5";
            using (var context1 = new Project0databaseContext(DbOptionsFactory.DbContextOptions))
            {
                IStoreRepository repo = new StoreRepository(DbOptionsFactory.DbContextOptions);
                // action
                repo.DeleteOneStore(storeLoc);
            }
            // asert
            using var context2 = new Project0databaseContext(DbOptionsFactory.DbContextOptions);
            var dbStore = context2.Stores.FirstOrDefault(x => x.Storeloc == "Techland Phoenix 5");
            Assert.Null(dbStore);
        }
    }
}
