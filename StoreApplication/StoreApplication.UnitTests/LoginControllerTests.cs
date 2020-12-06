using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    public class LoginControllerTests
    {
       
        [Fact]
        public void Index_AdminLoginSuccess()
        {
            // arrange
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());         
            var controller = new LoginController(_mockRepo.Object, new NullLogger<LoginController>())
            {
                TempData = tempData
            };            
            _mockRepo.Setup(x => x.GetOneAdminCredential(It.IsAny<string>())).Returns(
                new CAdmincredential("admin@gmail.com", "admin12345"));

            var viewLogin = new LoginViewModel
            {
                Email = "admin@gmail.com",
                Password = "admin12345"
            };

            // act
            IActionResult actionResult = controller.Index(viewLogin);

            // assert
            Assert.True(controller.ModelState.IsValid);
            Assert.Equal("admin@gmail.com", controller.TempData.Peek("User"));
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            Assert.Equal("Admin", viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);

        }

        [Fact]
        public void Index_RegularUserLoginSuccess() 
        {
            // arrange
            var _mockRepo = new Mock<IStoreRepository>();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var controller = new LoginController(_mockRepo.Object, new NullLogger<LoginController>())
            {
                TempData = tempData
            };
            _mockRepo.Setup(x => x.GetOneAdminCredential(It.IsAny<string>())).Returns(
                (CAdmincredential)null);
            _mockRepo.Setup(x => x.GetOneCredential(It.IsAny<string>())).Returns(
                new CCredential("user@gmail.com", "user12345"));

            var viewLogin = new LoginViewModel
            {
                Email = "user@gmail.com",
                Password = "user12345",
            };

            // act
            IActionResult actionResult = controller.Index(viewLogin);

            // assert
            Assert.True(controller.ModelState.IsValid);
            Assert.Equal("user@gmail.com", controller.TempData.Peek("User"));
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            Assert.Equal("Store", viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
