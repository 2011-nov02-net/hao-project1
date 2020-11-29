using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // a list of store locations
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
            // no longer takes parameter id
            // string id = TempData.Peek("storeLoc").ToString();
            
            var viewProduct = _storeRepo.GetInventoryOfAStore(storeLoc).Select(cProduct => new ProductViewModel
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

                // concurrent
                CProduct foundProduct = _storeRepo.GetOneProduct(id);
                if (foundProduct == null)
                {
                    ModelState.AddModelError("", "This product has just been deleted");
                    return View(viewDP);
                }

                // total cost per product to be implamented
                // change quantity change total

                CProduct cProduct = new CProduct(foundProduct.UniqueID, foundProduct.Name, foundProduct.Category, foundProduct.Price,
                                            viewDP.Quantity);
                string path = "../../SimplyWriteData.json";
                JsonFilePersist persist = new JsonFilePersist(path);
                List<CProduct> products = persist.ReadProductsData();
                if (products == null)
                {
                    products = new List<CProduct>();
                }
                products.Add(cProduct);
                persist.WriteProductsData(products);    
                return RedirectToAction("Select","Store", new StoreViewModel { Storeloc = TempData.Peek("storeLoc").ToString() });
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
            string path = "../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            List<CProduct> products = persist.ReadProductsData();
            if (products == null)
            {
                return View( new List<DetailedProductViewModel>() );
            }

            var viewProducts = products.Select(x => new DetailedProductViewModel
            {
                UniqueID = x.UniqueID,
                Name = x.Name,
                Category = x.Category,
                Price = x.Price,
                Quantity = x.Quantity,
                TotalCostPerProduct = x.Price * x.Quantity,
            });

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
            return View();
        }

        public ActionResult Checkout()
       {
            string path = "../../SimplyWriteData.json";
            JsonFilePersist persist = new JsonFilePersist(path);
            List<CProduct> products = persist.ReadProductsData();
            if (products == null)
            {
                return View();
            }

            // orderid , store, customer, ordertime, totalcost
            string orderID = Guid.NewGuid().ToString().Substring(0, 10);
            string storeLoc = TempData.Peek("storeLoc").ToString();
            CStore store = _storeRepo.GetAStore(storeLoc);
            string email = TempData.Peek("User").ToString();
            CCustomer cCustomer = _storeRepo.GetOneCustomerByEmail(email);               
            double totalCost = store.CalculateTotalPrice(products);

            // in case quantity change
            //TempData["Sum"] = totalCost;
            //TempData.Keep("Sum");

            COrder newOrder = new COrder(orderID, store, cCustomer ,totalCost);

            List<CProduct> inventory = _storeRepo.GetInventoryOfAStore(storeLoc);
            store.CleanInventory();
            store.AddProducts(inventory);

            Dictionary<string, CCustomer> customers = _storeRepo.GetAllCustomersAtOneStore(storeLoc);
            store.CustomerDict = customers;
          
            bool isSuccessful = false;
            try
            {
                // quantity limits
                newOrder.ProductList = products;
                isSuccessful = true;
                // Console.WriteLine("Order created successfully!");
            }
            catch (ArgumentException e)
            {
                isSuccessful = false;
                // Console.WriteLine("This order exceeds the max allowed quantities, failed to create the order!");
            }

            if (isSuccessful)
            {
                if (!store.CheckInventory(newOrder))
                {
                    // Console.WriteLine("Do not have enough products left to fulfill this order!");
                }
                else
                {
                    // map products to an order, orders to a customer,
                    // store now has complete information
                    foreach (var pair in store.CustomerDict)
                    {
                        CCustomer customer = pair.Value;
                        customer.OrderHistory = _storeRepo.GetAllOrdersOfOneCustomer(customer.Customerid, store, customer);
                        foreach (var order in customer.OrderHistory)
                        {
                            order.ProductList = _storeRepo.GetAllProductsOfOneOrder(order.Orderid);
                            order.TotalCost = store.CalculateTotalPrice(order.ProductList);
                        }
                    }
                    store.UpdateInventoryAndCustomerOrder(newOrder);
                    _storeRepo.CustomerPlaceOneOrder(newOrder, store, totalCost);
                }
            }

            // clean order
            StreamWriter sw = new StreamWriter("../../SimplyWriteData.json");
            sw.WriteLine(string.Empty);
            sw.Close();
            TempData.Remove("Total");
            return View();        
       }


        public ActionResult CheckOrder()
        {
            string email = TempData.Peek("User").ToString();
            CCustomer customer = _storeRepo.GetOneCustomerByEmail(email);
            string storeLoc = TempData.Peek("storeLoc").ToString();
            CStore store = _storeRepo.GetAStore(storeLoc);

            // collection information about this customer
            CCustomer foundCustomer = _storeRepo.GetOneCustomerOrderHistory(customer.FirstName, customer.LastName,
                customer.PhoneNumber , store);          
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
