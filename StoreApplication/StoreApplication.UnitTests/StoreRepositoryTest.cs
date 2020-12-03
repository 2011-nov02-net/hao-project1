﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class StoreRepositoryTest
    {
        // don't use actual Azure database, use SQLite for tests
      
        /*
        [Fact]
        public void DBAddOneStore()
        {
            // setup
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            var option = optionsBuilder.Options;
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


        /*
        [Fact]
        public void DBAddOneProduct() {
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            var option = optionsBuilder.Options;
            CProduct newProduct = new CProduct("Product101", "Duck", "Meat", 10.0);
            using (var context1 = new Project0databaseContext(option))
            {
                IStoreRepository repo = new StoreRepository(option);
                // action
                repo.AddOneProduct(newProduct);
            }
            // assert
            using var context2 = new Project0databaseContext(option);
            var dbProduct = context2.Products.Find("Product101");
            Assert.Equal(newProduct.Name, dbProduct.Name);
            Assert.Equal(newProduct.Category, dbProduct.Category);
            Assert.Equal(newProduct.Price, dbProduct.Price);           
        }
        */
        

        /*
        [Fact]
        public void DBGetAProductByNameAndCategory()
        {
            var optionsBuilder = new DbContextOptionsBuilder<Project0databaseContext>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            var option = optionsBuilder.Options;
            string name = "Duck";
            string category = "Meat";
            double price = 10.0;
            using (var context1 = new Project0databaseContext(option))
            {
                IStoreRepository repo = new StoreRepository(option);
                // action
                CProduct p = repo.GetOneProductByNameAndCategory(name, category);
            }
            // assert
            using var context2 = new Project0databaseContext(option);
            var dbProduct = context2.Products.Find("Product101");
            Assert.Equal(price, dbProduct.Price);         
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
        

        /*
        static string GetConnectionString()
        {
            string path = "../../../../../../project0-connection-string.json";
            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"required file {path} not found. should just be the connection string in quotes.");
                throw;
            }
            string connectionString = JsonSerializer.Deserialize<string>(json);
            return connectionString;
        }
        */
        
    }
}
