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

    public class CustomerControllerTests
    {
        string storeLoc = "fake1";
        string emptyFirst = null;
        string emptyLast = null;
        string firstName = "fakeFirst";
        string lastName = "fakeLast";
        // arrange
        [Fact]
        public void Index_GetAllCustomersAtOneStore()
        {
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["adminLoc"] = storeLoc;
            var controller = new CustomerController(_mockRepo.Object, new NullLogger<CustomerController>())
            {
                TempData = tempData
            };

            _mockRepo.Setup(x => x.GetAllCustomersAtOneStore(It.IsAny<string>())).Returns(
                new Dictionary<string, CCustomer>
                {
                    { "customer1",new CCustomer("customer1","Kyle","Crane","8883338888") },
                    { "customer2",new CCustomer("customer2","Rais","Thugs","6663331111") },
                });

            // act
            IActionResult actionResult = controller.Index(emptyFirst, emptyLast);

            // assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var customers = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(viewResult.Model);
            var customerList = customers.ToList();
            Assert.Equal("customer1", customerList[0].Customerid);
            Assert.Equal("Kyle", customerList[0].Firstname);
            Assert.Equal("Crane", customerList[0].Lastname);
            Assert.Equal("6663331111", customerList[1].Phonenumber);
            Assert.Null(customerList[1].Email);
        }

        [Fact]
        public void Index_SearchAllCustomersAtOneStore()
        {
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["adminLoc"] = storeLoc;
            var controller = new CustomerController(_mockRepo.Object, new NullLogger<CustomerController>())
            {
                TempData = tempData
            };

            _mockRepo.Setup(x => x.GetAllCustomersAtOneStore(It.IsAny<string>())).Returns(
                new Dictionary<string, CCustomer>
                {
                    { "customer1",new CCustomer("customer1","Kyle","Crane","8883338888") },
                    { "customer2",new CCustomer("customer2","Rais","Thugs","6663331111") },
                });
            _mockRepo.Setup(x => x.GetAllCustomersAtOneStoreByName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<CCustomer>
                { new CCustomer("customer1","Kyle","Crane","8883338888")});

            // act
            IActionResult actionResult = controller.Index(firstName, lastName);

            // assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var customers = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(viewResult.Model);
            var customerList = customers.ToList();
            Assert.Single(customerList);
            Assert.Equal("customer1", customerList[0].Customerid);
            Assert.Equal("Kyle", customerList[0].Firstname);
            Assert.Equal("Crane", customerList[0].Lastname);
            Assert.Equal("8883338888", customerList[0].Phonenumber);
            Assert.Null(customerList[0].Email);
        }
    }
}
