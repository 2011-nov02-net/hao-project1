using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreDatamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApplication.UnitTests
{
    public static class DbOptionsFactory
    {
        /// <summary>
        /// preload a connection string to a dbcontext for testing store repository
        /// </summary>
        static DbOptionsFactory()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("default");
            DbContextOptions = new DbContextOptionsBuilder<Project0databaseContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public static DbContextOptions<Project0databaseContext> DbContextOptions { get; }

    }
}
