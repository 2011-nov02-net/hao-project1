using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.WebApp.Controllers
{
    public class StoreController : Controller
    {

        private readonly ILogger<StoreController> _logger;
        private readonly IStoreRepository _storeRepo;

        public StoreController(ILogger<StoreController> logger, IStoreRepository storeRepo)
        {
            _logger = logger;
            _storeRepo = storeRepo;
        }

        // GET: StoreController
        public ActionResult Index()
        {         
            var viewStore = _storeRepo.GetAllStores().Select(x => new StoreViewModel
            {
                Storeloc = x.Storeloc,
                Storephone = x.Storephone,
            });
            return View(viewStore);
        }


        public ActionResult Select(string storeLoc)
        {
            /*
            var viewProduct = _storeRepo.GetAllProducts().Select(cProduct => new ProductViewModel
            {
                UniqueID = cProduct.UniqueID,
                Name = cProduct.Name,
                Category = cProduct.Category,
                Price = cProduct.Price,
            });

            return View(viewProduct);
            */

            var viewProduct = _storeRepo.GetInventoryOfAStore(storeLoc).Select(cProduct => new ProductViewModel
            {
                UniqueID = cProduct.UniqueID,
                Name = cProduct.Name,
                Category = cProduct.Category,
                Price = cProduct.Price,
            });

            return View(viewProduct);
        }


        public ActionResult AddToCart(string id)
        {
            // this is returning a null reference
            CProduct foundProduct = _storeRepo.GetOneProduct(id);
            // concurrent

            if (foundProduct == null)
            {
                ModelState.AddModelError("", "Another admin has just deleted this product");
                return View();
            }

            var viewProduct = new ProductViewModel
            {
                UniqueID = foundProduct.UniqueID,
                Name = foundProduct.Name,
                Category = foundProduct.Category,
                Price = foundProduct.Price,
            };
            return View(viewProduct);
        }

            // GET: StoreController/Details/5
            public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StoreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
