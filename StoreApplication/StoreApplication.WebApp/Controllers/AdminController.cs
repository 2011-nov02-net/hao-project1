using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var viewStore = _storeRepo.GetAllStores().Select(x => new StoreViewModel
            {
                Storeloc = x.Storeloc,
                Storephone = x.Storephone,
                Zipcode = x.Zipcode
            });

            if (!String.IsNullOrEmpty(zipCode))
            {

                var stores = _storeRepo.GetAllStoresByZipcode(zipCode);
                viewStore = stores.Select(x => new StoreViewModel
                {
                    Storeloc = x.Storeloc,
                    Storephone = x.Storephone,
                    Zipcode = x.Zipcode,
                });
            }
            return View(viewStore);
        }

        public ActionResult Select(string storeLoc)
        {
            TempData["adminLoc"] = storeLoc;
            TempData.Keep("adminLoc");
            return View();
        }

      
      
    }
}
