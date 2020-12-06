using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApplication.WebApp.Controllers;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using StoreLibrary;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StoreApplication.UnitTests
{

    public class ProductControllerTests
    {
        // can't seem to mock arguments
        string storeLoc = "dummy1";
        string noCategory = "";
        string category = "dummy2";

        [Fact]
        public void Index_GetAllProductsAtOneStore()
        {
            // arrange
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["adminLoc"] = storeLoc;
            var controller = new ProductController(_mockRepo.Object, new NullLogger<ProductController>())
            {
                TempData = tempData
            };

            _mockRepo.Setup(x => x.GetInventoryOfOneStore(It.IsAny<string>())).Returns(new List<CProduct>
            {
                new CProduct ("P101","Dying Light","Game",24.99,600),
                new CProduct ("P102","Dying Light 2","Game",59.99,700),
            });

            // act
            IActionResult actionResult = controller.Index(noCategory);

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
        public void Index_SearchAllProductsAtOneStore()
        {
            // arrange
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["adminLoc"] = storeLoc;
            var controller = new ProductController(_mockRepo.Object, new NullLogger<ProductController>())
            {
                TempData = tempData
            };

            _mockRepo.Setup(x => x.GetInventoryOfOneStore(It.IsAny<string>())).Returns(new List<CProduct>
            {
                new CProduct ("P101","Dying Light","Game",24.99,600),
                new CProduct ("P102","Dying Light 2","Game",59.99,700),
            });
            _mockRepo.Setup(x => x.GetInventoryOfOneStoreByCategory(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new List<CProduct>
            {
                new CProduct("P101", "Dying Light", "Game", 24.99, 600),
            });

            // act
            IActionResult actionResult = controller.Index(category);

            // aasert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var products = Assert.IsAssignableFrom<IEnumerable<DetailedProductViewModel>>(viewResult.Model);
            var productsList = products.ToList();
            Assert.Single(productsList);
            Assert.Equal("P101", productsList[0].UniqueID);
            Assert.Equal("Dying Light", productsList[0].Name);
            Assert.Equal(24.99, productsList[0].Price);
            Assert.Equal(600, productsList[0].Quantity);
        }

        [Fact]
        public void Create_ValidState()
        {
            // arrange
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["adminLoc"] = storeLoc;
            var controller = new ProductController(_mockRepo.Object, new NullLogger<ProductController>())
            {
                TempData = tempData
            };

            // bypass duplicate checking
            _mockRepo.Setup(x => x.GetOneProductByNameAndCategory(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((CProduct)null);

            string loc = null;
            CProduct product = null;
            int quantity = 0;
            _mockRepo.Setup(x => x.StoreAddOneProduct(It.IsAny<string>(), It.IsAny<CProduct>(), It.IsAny<int>()))
                .Callback<string, CProduct, int>((x, y, z) =>
                {
                    loc = x;
                    product = y;
                    quantity = z;
                });

            var viewDP = new DetailedProductViewModel
            {
                // ID is automatically assigned
                Name = "Dying Light 3",
                Category = "Game",
                Price = 79.99,
                Quantity = 100
            };

            // act
            IActionResult actionResult = controller.Create(viewDP);

            // assert
            Assert.True(controller.ModelState.IsValid);
            _mockRepo.Verify(r => r.StoreAddOneProduct(It.IsAny<string>(), It.IsAny<CProduct>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(viewDP.Name, product.Name);
            Assert.Equal(viewDP.Category, product.Category);
            Assert.Equal(viewDP.Price, product.Price);
            Assert.Equal(viewDP.Quantity, quantity);
            Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
        }

        [Fact]
        public void Create_InvalidModelState()
        {
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["adminLoc"] = storeLoc;
            var controller = new ProductController(_mockRepo.Object, new NullLogger<ProductController>())
            {
                TempData = tempData
            };
            controller.ModelState.AddModelError("", "Invalid input format");

            var viewDP = new DetailedProductViewModel
            {
                Name = "Dying Light 3",
                Category = "Game",
                Price = 79.99,
                Quantity = 100,
            };

            // act
            IActionResult actionResult = controller.Create(viewDP);

            // assert
            Assert.False(controller.ModelState.IsValid);
            Assert.Equal(2, controller.ModelState.ErrorCount);
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
        }
    }
}
