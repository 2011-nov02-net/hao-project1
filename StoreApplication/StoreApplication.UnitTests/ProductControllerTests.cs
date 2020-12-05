using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApplication.WebApp.Controllers;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreApplication.UnitTests
{
    /*
    public class ProductControllerTests
    {
        
        string storeLoc = "Central Ave 1";
        string category = "drink";
        [Fact]
        public void GetAllProductsAtOneStore()
        {
            // arrange
            var mockStoreRepo = new Mock<IStoreRepository>();
            //TempData["adminLoc"] = "Central Ava 1";
            mockStoreRepo.Setup(x => x.GetInventoryOfOneStore(storeLoc)).Returns(new List<CProduct>
            {
                new CProduct ("P101","Dying Light","Game",24.99,600),
                new CProduct ("P102","Dying Light 2","Game",59.99,700),               
            });
            var controller = new ProductController(mockStoreRepo.Object, new NullLogger<ProductController>());

            // act
            IActionResult actionResult = controller.Index(category);

            // aasert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var products = Assert.IsAssignableFrom<IEnumerable<DetailedProductViewModel>>(viewResult.Model);
            var productsList = products.ToList();
            Assert.Equal(2, productsList.Count);
            Assert.Equal("P101", productsList[0].UniqueID);
            Assert.Equal("Dying Light", productsList[0].Name);
            Assert.Equal(59.99, productsList[1].Price);
            Assert.Equal(700, productsList[1].Quantity);
        }

        [Fact]
        public void SearchAllProductsAtOneStore()
        { }
    }
    */
}
