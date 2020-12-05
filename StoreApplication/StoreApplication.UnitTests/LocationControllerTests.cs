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
    public class LocationControllerTests
    {      
        [Fact]
        public void Index_GetAllStores()
        {
            // arrange
            var mockStoreRepository = new Mock<IStoreRepository>();
            mockStoreRepository.Setup(x => x.GetAllStores()).Returns(new List<CStore> {
                new CStore("Techland HQ 1", "9099999999","96291"), 
                new CStore("Techland London 1", "6066666666","85281"),
            });
            var controller = new LocationController(mockStoreRepository.Object, new NullLogger<LocationController> ());
            
            // act
            IActionResult actionResult = controller.Index();

            // assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var stores = Assert.IsAssignableFrom<IEnumerable<StoreViewModel>>(viewResult.Model);
            var storeList = stores.ToList();
            Assert.Equal(2, storeList.Count);
            Assert.Equal("Techland HQ 1", storeList[0].Storeloc);
            Assert.Equal("9099999999", storeList[0].Storephone);
            Assert.Equal("85281", storeList[1].Zipcode); ;
        }

        [Fact]
        public void Index_AddOneStore()
        {
            // arrange
            var mockStoreRepository = new Mock<IStoreRepository>();
            var controller = new LocationController(mockStoreRepository.Object, new NullLogger<LocationController>());
            /*
            var newLocation = new CStore("Techland Paris 3", "7071231234", "85041");
            var viewLocation = new StoreViewModel
            {
                Storeloc = newLocation.Storeloc,
                Storephone = newLocation.Storephone,
                Zipcode = newLocation.Zipcode,
            };

            // act
            IActionResult actionResult = controller.Create(viewLocation);

            // assert
            Assert.True(controller.ModelState.IsValid);
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            */

            // using it
            mockStoreRepository.Setup(x => x.AddOneStore(It.IsAny<CStore>())).Verifiable();
            IActionResult actionResult = controller.Create(It.IsAny<StoreViewModel>());
            Assert.True(controller.ModelState.IsValid);
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            mockStoreRepository.Verify(r => r.AddOneStore(It.IsAny<CStore>()), Times.Once);







        }


    }
}

