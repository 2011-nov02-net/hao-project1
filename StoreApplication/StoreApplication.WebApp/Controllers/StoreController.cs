using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
using StoreLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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
 
        public ActionResult Index()
        {
            var viewStore = _storeRepo.GetAllStores().Select(x => new StoreViewModel
            {
                Storeloc = x.Storeloc,
                Storephone = x.Storephone,
            });
            return View(viewStore);
        }

        // a list of products sold at that location
        public ActionResult Select(string storeLoc)
        {            
            var viewProduct = _storeRepo.GetInventoryOfOneStore(storeLoc).Select(cProduct => new ProductViewModel
            {
                UniqueID = cProduct.UniqueID,
                Name = cProduct.Name,
                Category = cProduct.Category,
                Price = cProduct.Price,
            });
            TempData["storeLoc"] = storeLoc;
            TempData.Keep("storeLoc");
            return View(viewProduct);
        }

        // add a product to my cart
        [HttpGet]
        public ActionResult AddToCart(string id)
        {
            var cProduct = _storeRepo.GetOneProduct(id);
            var viewDP = new DetailedProductViewModel
            {
                UniqueID = cProduct.UniqueID,
                Name = cProduct.Name,
                Category = cProduct.Category,
                Price = cProduct.Price,
            };
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

            var viewProducts = Mapper.MapDetailedProducts(products);
             
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

       
        public ActionResult Proceed()
        {
            // place to change address and payment method
            // then finally checkout
            if (!TempData.ContainsKey("Cart"))
            {
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
                return View();
            }

            // orderid , store, customer, ordertime, totalcost
            string orderID = Guid.NewGuid().ToString().Substring(0, 10);
            string storeLoc = TempData.Peek("storeLoc").ToString();
            CStore store = _storeRepo.GetOneStore(storeLoc);
            string email = TempData.Peek("User").ToString();
            CCustomer cCustomer = _storeRepo.GetOneCustomerByEmail(email);
            // recalculate price in case customers change quantity in carts
            double totalCost = store.CalculateTotalPrice(products);

            COrder newOrder = new COrder(orderID, store, cCustomer ,totalCost);

            // check against inventory
            List<CProduct> inventory = _storeRepo.GetInventoryOfOneStore(storeLoc);
            store.CleanInventory();
            store.AddProducts(inventory);

            bool isSuccessful = false;
            try
            {
                // quantity limits
                newOrder.ProductList = products;
                isSuccessful = true;               
            }
            catch (ArgumentException e)
            {
                isSuccessful = false;
                _logger.LogError(e, "Quantity set exceeds the maximum allowed");
            }
            if (isSuccessful)
            {
                if (!store.CheckInventory(newOrder))
                {
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
            CCustomer foundCustomer = _storeRepo.GetOneCustomerOrderHistory(customer.FirstName, customer.LastName,
                customer.PhoneNumber , store);
            if (foundCustomer == null)
            {
                return View( new List<OrderViewModel>());
            }

            var viewOrder = foundCustomer.OrderHistory.Select(x => new OrderViewModel
            {
                Orderid = x.Orderid,
                StoreLoc = x.StoreLocation.Storeloc,
                OrderedTime = x.OrderedTime,
                TotalCost = x.TotalCost,
            });
            return View(viewOrder);
        }


        public ActionResult OrderDetail(string id)
        {
            List<CProduct> products= _storeRepo.GetAllProductsOfOneOrder(id);
            var viewproducts = products.Select(x => new DetailedProductViewModel
            { 
                UniqueID = x.UniqueID,
                Name = x.Name,
                Category = x.Category,
                Price = x.Price,
                Quantity = x.Quantity,
                TotalCostPerProduct = x.Price * x.Quantity,                    
            });
            return View(viewproducts);
        }
    
    }
}
