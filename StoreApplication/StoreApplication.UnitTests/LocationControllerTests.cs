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
            var _mockRepo = new Mock<IStoreRepository>();
            _mockRepo.Setup(x => x.GetAllStores()).Returns(new List<CStore> {
                new CStore("Techland HQ 1", "9099999999","96291"), 
                new CStore("Techland London 1", "6066666666","85281"),
            });
            var controller = new LocationController(_mockRepo.Object, new NullLogger<LocationController> ());
            
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
        public void Create_AddOneStore()
        {           
            // tempo solution- a mix of old and new versions
            // arrange
            var _mockRepo = new Mock<IStoreRepository>();
            var controller = new LocationController(_mockRepo.Object, new NullLogger<LocationController>());
    
            CStore store = null;
            _mockRepo.Setup(x => x.AddOneStore(It.IsAny<CStore>())).Callback<CStore>(x => store = x);

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
            _mockRepo.Verify(r => r.AddOneStore(It.IsAny<CStore>()), Times.Once);
            Assert.Equal(viewLocation.Storeloc, store.Storeloc);
            Assert.Equal(viewLocation.Storephone,store.Storephone);
            Assert.Equal(viewLocation.Zipcode, store.Zipcode);
            Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);

            /*
            // old version that works
            // arrange
            var _mockRepo = new Mock<IStoreRepository>();
            var controller = new LocationController(_mockRepo.Object, new NullLogger<LocationController>());
            
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

            /*
            // new version that does not work
            mockStoreRepository.Setup(x => x.AddOneStore(It.IsAny<CStore>())).Verifiable();
            IActionResult actionResult = controller.Create(It.IsAny<StoreViewModel>());
            Assert.True(controller.ModelState.IsValid);
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            mockStoreRepository.Verify(r => r.AddOneStore(It.IsAny<CStore>()), Times.Once);
            */





        }


    }
}

