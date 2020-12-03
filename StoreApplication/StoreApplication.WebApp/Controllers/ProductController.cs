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
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IStoreRepository _storeRepo;
        public ProductController(IStoreRepository storeRepo, ILogger<ProductController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }

        // updated with quantity
        public ActionResult Index()
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            var products = _storeRepo.GetInventoryOfOneStore(storeLoc);
            var viewProduct = ViewModelMapper.MapDetailedProductsWithoutTotal(products);         
            return View(viewProduct);
        }

        // placeholder for more information in the future
        public ActionResult Details(string id)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            CProduct foundProduct = _storeRepo.GetOneProductWithQuantity(storeLoc,id);

            // concurrent            
            if (foundProduct == null)
            {
                ModelState.AddModelError("", "Another admin has just deleted this product");
                return View();
            }           
            var viewProduct = ViewModelMapper.MapSingleDetailedProduct(foundProduct);           
            return View(viewProduct);
        }

         
        public ActionResult Create()
        {                
            return View();
        }

        // create a new product with quantity, update produc table, and inventory table
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailedProductViewModel viewDP)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid input format");
                    return View();
                }
                // avoid duplicate
                var foundProduct = _storeRepo.GetOneProductByNameAndCategory(viewDP.Name, viewDP.Category);
                if (foundProduct != null)
                {
                    ModelState.AddModelError("","This product already exist in this category");
                    return View();
                }
                // a new randomly generated id for a new product                      
                string productID = Guid.NewGuid().ToString().Substring(0, 10);
                var cProduct = new CProduct(productID, viewDP.Name, viewDP.Category, viewDP.Price,viewDP.Quantity);
                _storeRepo.StoreAddOneProduct(storeLoc,cProduct,viewDP.Quantity);
    
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "error while tring to add a product");
                ModelState.AddModelError("", "failed to create a product");
                return View();
            }
        }

        // not yet updated, should be able to update quantity as well
        public ActionResult Edit(string id)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            var cProduct = _storeRepo.GetOneProductWithQuantity(storeLoc,id);
            var viewProduct = ViewModelMapper.MapSingleDetailedProductWithoutTotal(cProduct);
            return View(viewProduct);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( string id, DetailedProductViewModel viewDP)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            try
            {
                if(!ModelState.IsValid)
                { 
                    return View();
                }

                // concurrent
                var foundProduct = _storeRepo.GetOneProduct(id);
                if (foundProduct == null)
                {
                    ModelState.AddModelError("", "Another admin has just deleted this product");
                    return View();
                }

                // check if you have changed the name or category
                if (foundProduct.Name != viewDP.Name || foundProduct.Category != viewDP.Category)
                {
                    // see if the edited version already exist
                    var editedProduct = _storeRepo.GetOneProductByNameAndCategory(viewDP.Name, viewDP.Category);
                    if (editedProduct != null)
                    {
                        ModelState.AddModelError("", "A record with the same data already exist in this category");
                        return View();
                    }
                }          
                foundProduct = new CProduct(foundProduct.UniqueID, viewDP.Name, viewDP.Category, viewDP.Price);
                _storeRepo.EditOneProduct(storeLoc,foundProduct, viewDP.Quantity);                              
                return RedirectToAction(nameof(Index));
            }
            catch( Exception e)
            {
                _logger.LogError( e , "error while trying to edit a product");
                ModelState.AddModelError("","failed to edit a product" );
                return View();
            }
        }

        // not yet updated
        public ActionResult Delete(string id)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            var cProduct = _storeRepo.GetOneProductWithQuantity(storeLoc,id);
            var viewProduct = ViewModelMapper.MapSingleDetailedProductWithoutTotal(cProduct);
            return View(viewProduct);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            try
            {
                // concurrent
                var foundProduct = _storeRepo.GetOneProduct(id);
                if (foundProduct == null)
                {
                    ModelState.AddModelError("", "Another admin has just deleted this product");
                    return View();
                }

                _storeRepo.DeleteOneProduct(storeLoc,id);
                return RedirectToAction(nameof(Index));
            }
            catch( Exception e)
            {
                _logger.LogError(e, "error while trying to delete a product");
                ModelState.AddModelError("","Trying to delete a product that does not exist");
                var viewMode = _storeRepo.GetOneProduct(id);

                return View();
            }
        }
    }
}
