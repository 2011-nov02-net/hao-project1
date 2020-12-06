using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Linq;

namespace StoreApplication.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IStoreRepository _storeRepo;

        public AdminController(ILogger<AdminController> logger, IStoreRepository storeRepo)
        {
            _logger = logger;
            _storeRepo = storeRepo;
        }

        // GET: AdminController
        public ActionResult Index(string zipCode)
        {
            var stores = _storeRepo.GetAllStores();
            var viewStores = ViewModelMapper.MapStores(stores);             
            if (!String.IsNullOrEmpty(zipCode))
            {
                var searchedStores = _storeRepo.GetAllStoresByZipcode(zipCode);
                viewStores = ViewModelMapper.MapStores(searchedStores);
            }
            return View(viewStores);
        }

        public ActionResult Select(string storeLoc)
        {
            TempData["adminLoc"] = storeLoc;
            TempData.Keep("adminLoc");
            return View();
        }



    }
}
