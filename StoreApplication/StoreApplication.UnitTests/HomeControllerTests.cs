using Moq;
using StoreDatamodel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using StoreLibrary;
using StoreApplication.WebApp.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace StoreApplication.UnitTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_GetAllStores()
        {

            // use mocking
            var mockStoreRepository = new Mock<IStoreRepository>();
            mockStoreRepository.Setup(x => x.GetAllStores()).Returns(new List<CStore> {
                new CStore("Downtown Abbe 1", "9099999999"), 
                new CStore("Downtown View 2", "8889998888")});
            var controller = new HomeController(mockStoreRepository.Object, new NullLogger<HomeController>());
            IActionResult actionResult = controller.Index();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var stores = Assert.IsAssignableFrom<IEnumerable<CStore>>(viewResult.Model);
            var storeList = stores.ToList();
            Assert.Equal(2, storeList.Count);
            Assert.Equal("9099999999", storeList[0].Storephone);
            Assert.Equal("8889998888", storeList[1].Storephone);

        }
    }
}
