using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public ActionResult Index(string zipCode)
        {
            var stores = _storeRepo.GetAllStores();
            var viewStore = ViewModelMapper.MapStores(stores);

            if (!String.IsNullOrEmpty(zipCode))
            {
                var searchedStores = _storeRepo.GetAllStoresByZipcode(zipCode);
                viewStore = ViewModelMapper.MapStores(searchedStores);
            }
            return View(viewStore);
        }

        // a list of products sold at that location
        public ActionResult Select(string storeLoc, string category)
        {

            if (storeLoc == null) storeLoc = TempData.Peek("storeLoc").ToString();
            var products = _storeRepo.GetInventoryOfOneStore(storeLoc);
            var viewProduct = ViewModelMapper.MapNonDetailedProducts(products);

            if (!String.IsNullOrEmpty(category))
            {
                var foundProducts = _storeRepo.GetInventoryOfOneStoreByCategory(storeLoc, category);
                viewProduct = ViewModelMapper.MapNonDetailedProducts(foundProducts);
            }
            TempData["storeLoc"] = storeLoc;
            TempData.Keep("storeLoc");
            return View(viewProduct);
        }

        // add a product to my cart
        [HttpGet]
        public ActionResult AddToCart(string id)
        {
            var cProduct = _storeRepo.GetOneProduct(id);
            var viewDP = ViewModelMapper.MapSingleNonDetailedProduct(cProduct);
            return View(viewDP);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(string id, DetailedProductViewModel viewDP)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "invalid input format");
                    return View(viewDP);
                }

                // handle concurrency
                CProduct foundProduct = _storeRepo.GetOneProduct(id);
                if (foundProduct == null)
                {
                    ModelState.AddModelError("", "This product has just been deleted");
                    return View(viewDP);
                }
                CProduct cProduct = new CProduct(foundProduct.UniqueID, foundProduct.Name, foundProduct.Category, foundProduct.Price,
                                            viewDP.Quantity);

                // use tempdata to store products in a cart
                // do not know how to return a serialized string directly, use a local text file for now
                string path = "../../SimplyWriteData.json";
                JsonFilePersist persist = new JsonFilePersist(path);
                string json = "";
                if (TempData.ContainsKey("Cart"))
                {
                    json = TempData.Peek("Cart").ToString();
                }

                List<CProduct> products = persist.ReadProductsTempData(json);
                if (products == null)
                {
                    products = new List<CProduct>();
                }
                products.Add(cProduct);
                string cart = persist.WriteProductsTempData(products);
                TempData["Cart"] = cart;
                TempData.Keep("Cart");
                // route parameter is an object
                return RedirectToAction("Select", "Store", new StoreViewModel { Storeloc = TempData.Peek("storeLoc").ToString() });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error while tring to add a product");
                ModelState.AddModelError("", "failed to create a product");
                return View(viewDP);
            }
        }

        public ActionResult CheckCart()
        {
            JsonFilePersist persist = new JsonFilePersist();
            if (!TempData.ContainsKey("Cart"))
            {
                return View(new List<DetailedProductViewModel>());
            }
            List<CProduct> products = persist.ReadProductsTempData(TempData.Peek("Cart").ToString());
            // empty cart to start with
            if (products == null)
            {
                return View(new List<DetailedProductViewModel>());
            }

            var viewProducts = ViewModelMapper.MapDetailedProducts(products);

            //fixed            
            double total = 0;
            foreach (var item in viewProducts)
            {
                total += item.TotalCostPerProduct;
            }
            TempData["Total"] = total.ToString();
            TempData.Keep("Total");
            return View(viewProducts);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            JsonFilePersist persist = new JsonFilePersist();
            List<CProduct> products = persist.ReadProductsTempData(TempData.Peek("Cart").ToString());
            CProduct foundProduct;
            DetailedProductViewModel viewProduct;
            if (products == null)
            {
                return RedirectToAction("CheckCart");
            }
            foreach (var product in products)
            {
                if (product.UniqueID == id)
                {
                    foundProduct = product;
                    viewProduct = ViewModelMapper.MapSingleDetailedProductWithoutTotal(foundProduct);
                    return View(viewProduct);                 
                }
            }
            return View();     
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, DetailedProductViewModel viewDP)
        {
            string path = "../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            List<CProduct> products = persist.ReadProductsTempData(TempData.Peek("Cart").ToString());
            if (products == null)
            {
                return RedirectToAction("CheckCart");
            }
            foreach (var product in products)
            {
                if (product.UniqueID == id)
                {
                    product.Quantity = viewDP.Quantity;   
                    break;
                }                
            }
            string cart = persist.WriteProductsTempData(products);
            TempData["Cart"] = cart;
            TempData.Keep("Cart");
            return RedirectToAction("CheckCart");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            JsonFilePersist persist = new JsonFilePersist();
            List<CProduct> products = persist.ReadProductsTempData(TempData.Peek("Cart").ToString());
            CProduct foundProduct;
            DetailedProductViewModel viewProduct;
            if (products == null)
            {
                return RedirectToAction("CheckCart");
            }
            foreach (var product in products)
            {
                if (product.UniqueID == id)
                {
                    foundProduct = product;
                    viewProduct = ViewModelMapper.MapSingleDetailedProductWithoutTotal(foundProduct);
                    return View(viewProduct);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, DetailedProductViewModel viewDP)
        {
            string path = "../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            List<CProduct> products = persist.ReadProductsTempData(TempData.Peek("Cart").ToString());
            if (products == null)
            {
                return RedirectToAction("CheckCart");
            }
            foreach (var product in products)
            {
                if (product.UniqueID == id)
                {
                    products.Remove(product);
                    break;
                }
            }
            string cart = persist.WriteProductsTempData(products);
            TempData["Cart"] = cart;
            TempData.Keep("Cart");
            return RedirectToAction("CheckCart");
        }

        public ActionResult Proceed()
        {
            // place to change address and payment method
            // then finally checkout
            if (!TempData.ContainsKey("Cart"))
            {
                ModelState.AddModelError("", "Cannot checkout empty cart");
                return RedirectToAction("CheckCart");
            }
            return View();
        }

        public ActionResult Checkout()
        {
            JsonFilePersist persist = new JsonFilePersist();
            List<CProduct> products = persist.ReadProductsTempData(TempData.Peek("Cart").ToString());
            if (products == null)
            {
                return RedirectToAction("CheckCart");
            }

            // orderid , store, customer, ordertime, totalcost
            string orderID = Guid.NewGuid().ToString().Substring(0, 10);
            string storeLoc = TempData.Peek("storeLoc").ToString();
            CStore store = _storeRepo.GetOneStore(storeLoc);
            string email = TempData.Peek("User").ToString();
            CCustomer cCustomer = _storeRepo.GetOneCustomerByEmail(email);
            // recalculate price in case customers change quantity in carts
            double totalCost = store.CalculateTotalPrice(products);

            COrder newOrder = new COrder(orderID, store, cCustomer, totalCost);

            // check against inventory
            var inventory = _storeRepo.GetInventoryOfOneStore(storeLoc);
            store.CleanInventory();
            store.AddProducts(inventory);

            bool isSuccessful = false;
            if (store.CalculateThreshhold(products))
            {
                // quantity limits
                newOrder.ProductList = products;
                isSuccessful = true;
            }
            else
            {
                ModelState.AddModelError("", "Quantity set exceeds the maximum allowed");
                _logger.LogError("Quantity set exceeds the maximum allowed");
                return RedirectToAction("CheckCart");
            }
            if (isSuccessful)
            {

                if (!store.CheckInventory(newOrder))
                {
                    ModelState.AddModelError("", "Not enough left in the inventory");
                    _logger.LogError("Not enough left in the inventory");
                    return RedirectToAction("CheckCart");
                }
                else
                {
                    store.UpdateInventory(newOrder);
                    _storeRepo.CustomerPlaceOneOrder(newOrder, store, totalCost);
                }
            }

            // clean cart 
            StreamWriter sw = new StreamWriter("../../SimplyWriteData.json");
            sw.WriteLine(string.Empty);
            sw.Close();
            TempData.Remove("Cart");
            TempData.Remove("Total");
            return View();
        }


        public ActionResult CheckOrder()
        {
            string email = TempData.Peek("User").ToString();
            CCustomer customer = _storeRepo.GetOneCustomerByEmail(email);
            string storeLoc = TempData.Peek("storeLoc").ToString();
            CStore store = _storeRepo.GetOneStore(storeLoc);

            // collection information about this customer
            var OrderHistory = _storeRepo.GetOneCustomerOrderHistory(customer, store);
            if (OrderHistory == null)
            {
                return View(new List<OrderViewModel>());
            }
            var viewOrder = ViewModelMapper.MapOrders(OrderHistory);
            return View(viewOrder);
        }


        public ActionResult OrderDetail(string id)
        {
            List<CProduct> products = _storeRepo.GetAllProductsOfOneOrder(id);
            var viewproducts = ViewModelMapper.MapDetailedProducts(products);
            return View(viewproducts);
        }
    }
}
