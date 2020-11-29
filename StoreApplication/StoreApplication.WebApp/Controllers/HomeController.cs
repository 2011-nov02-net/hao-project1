using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.Models;
using StoreDatamodel;
using StoreApplication.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StoreLibrary;

namespace StoreApplication.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreRepository _storeRepo;

        public HomeController(ILogger<HomeController> logger, IStoreRepository storeRepo)
        {
            _logger = logger;
            _storeRepo = storeRepo;         
        }

        public IActionResult Index()
        {
            TempData.Remove("User");
            TempData.Remove("storeLoc");
            TempData.Remove("Total");
            TempData.Remove("Sum");



            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }   
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(int notUsed)
        {


            return View();
        }

        public IActionResult Tour()
        {
            var viewProduct = _storeRepo.GetAllProducts().Select(x => new ProductViewModel
            {
                 
                Name = x.Name,
                Category = x.Category,
                Price = x.Price,


            });
            return View(viewProduct);
            
        
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
