using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.Models;
using StoreDatamodel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreRepository _storeRepo;



        public HomeController(IStoreRepository storeRepo, ILogger<HomeController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var viewModel = _storeRepo.GetAllStores();
            

            return View(viewModel);
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
