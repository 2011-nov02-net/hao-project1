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
    public class OrderController : Controller
    {

        private readonly IStoreRepository _storeRepo;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IStoreRepository storeRepo, ILogger<OrderController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }

        // GET: OrderController
        public ActionResult Index(string firstName,string lastName)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            Dictionary<string, CCustomer> customers = _storeRepo.GetAllCustomersAtOneStore(storeLoc);
            var viewCustomer = customers.Select(x => new CustomerViewModel
            {
                Customerid = x.Value.Customerid,
                Firstname = x.Value.FirstName,
                Lastname = x.Value.LastName,
                Phonenumber = x.Value.PhoneNumber,
                Email = x.Value.Email,
            });

            if (!String.IsNullOrEmpty(firstName) && !String.IsNullOrEmpty(lastName))
            {
                // get
                var searchedCustomers = _storeRepo.GetAllCustomersAtOneStoreByName(storeLoc, firstName, lastName);
                viewCustomer = searchedCustomers.Select(x => new CustomerViewModel
                {
                    Customerid = x.Customerid,
                    Firstname = x.FirstName,
                    Lastname = x.LastName,
                    Phonenumber = x.PhoneNumber,

                });

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
            // order id passed in
            var products = _storeRepo.GetAllProductsOfOneOrder(id);
            var viewProduct = products.Select(x => new DetailedProductViewModel
            {
                UniqueID = x.UniqueID,
                Name = x.Name,
                Category = x.Category,
                Price = x.Price,
                Quantity = x.Quantity,
                TotalCostPerProduct  = x.Price*x.Quantity,

            });
            return View(viewProduct);

        }
    }
}
