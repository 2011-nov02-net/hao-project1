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
    public class LocationController : Controller
    {
        private readonly IStoreRepository _storeRepo;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IStoreRepository storeRepo, ILogger<LocationController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }

        // GET: LocationController
        public ActionResult Index()
        {
            var locations = _storeRepo.GetAllStores();
            var viewStores = locations.Select(x => new StoreViewModel
            {
                Storeloc = x.Storeloc,
                Storephone = x.Storephone,
                Zipcode = x.Zipcode,                     
            });
            return View(viewStores);
        }

        // GET: LocationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoreViewModel viewStore)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid input format");
                    return View(viewStore);
                }

                // unfinished
                var newLocation = new CStore(viewStore.Storeloc, viewStore.Storephone, viewStore.Zipcode);
                _storeRepo.AddOneStore(newLocation);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error while tring to add a store");
                ModelState.AddModelError("", "failed to create a store");
                return View();
            }
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LocationController/Edit/5
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

        // GET: LocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LocationController/Delete/5
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
