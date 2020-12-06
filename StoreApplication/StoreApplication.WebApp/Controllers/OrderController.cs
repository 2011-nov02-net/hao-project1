using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApplication.WebApp.Controllers
{
    public class OrderController : Controller
    {

        private readonly IStoreRepository _storeRepo;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IStoreRepository storeRepo, ILogger<OrderController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }

        public ActionResult Index(string firstName, string lastName)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            Dictionary<string, CCustomer> customers = _storeRepo.GetAllCustomersAtOneStore(storeLoc);
            var viewCustomer = ViewModelMapper.MapCustomers(customers);
            if (!String.IsNullOrEmpty(firstName) && !String.IsNullOrEmpty(lastName))
            {
                // get
                var searchedCustomers = _storeRepo.GetAllCustomersAtOneStoreByName(storeLoc, firstName, lastName);
                viewCustomer = ViewModelMapper.MapCustomers(searchedCustomers);
            }
            return View(viewCustomer);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(string id)
        {
            // customer id passed in
            string storeLoc = TempData.Peek("adminLoc").ToString();
            CStore store = _storeRepo.GetOneStore(storeLoc);
            CCustomer customer = _storeRepo.GetOneCustomer(id);

            var orders = _storeRepo.GetAllOrdersOfOneCustomer(id, store, customer);
            var viewOrder = orders.Select(x => new OrderViewModel
            {
                Orderid = x.Orderid,
                StoreLoc = x.StoreLocation.Storeloc,
                Customerid = x.Customer.Customerid,
                OrderedTime = x.OrderedTime,
                TotalCost = x.TotalCost,
            });

            return View(viewOrder);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
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

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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

        public ActionResult Products(string id)
        {
            var products = _storeRepo.GetAllProductsOfOneOrder(id);
            var viewProduct = ViewModelMapper.MapDetailedProducts(products);
            return View(viewProduct);
        }
    }
}
