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

        // GET: ProductController
        public ActionResult Index()
        {
            var viewProduct = _storeRepo.GetAllProducts().Select(cProduct => new ProductViewModel
            {
                UniqueID = cProduct.UniqueID,
                Name = cProduct.Name,
                Category = cProduct.Category,
                Price = cProduct.Price,
            });

            return View(viewProduct);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(string id)
        {
            // this is returning a null reference
            CProduct cProduct = _storeRepo.GetOneProduct(id);
            
            var viewProduct = new ProductViewModel
            {
                UniqueID = cProduct.UniqueID,
                Name = cProduct.Name,
                Category = cProduct.Category,
                Price = cProduct.Price,
            };           
            return View(viewProduct);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
                   
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewProduct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                // view model does not have quantity
                var cProduct = new CProduct(viewProduct.UniqueID, viewProduct.Name, viewProduct.Category, viewProduct.Price);
                _storeRepo.AddOneProduct(cProduct);

                // add tempdata here

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "error while tring to add a product");
                ModelState.AddModelError("", "Product ID already in use or other issues");
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ProductViewModel viewProduct)
        {
            try
            {
                if(!ModelState.IsValid)
                { 
                    return View();
                }
                
                CProduct cProduct = _storeRepo.GetOneProduct(viewProduct.UniqueID);

                if (cProduct != null)
                {                  
                    cProduct = new CProduct(viewProduct.UniqueID, viewProduct.Name, viewProduct.Category, viewProduct.Price);
                    _storeRepo.EditOneProduct(cProduct);
                    // add tempo data
                    
                }
                else
                {
                    ModelState.AddModelError("", "Trying to edit a product that does not exist");
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            catch( Exception e)
            {
                _logger.LogError( e , "error while trying to edit a product");
                ModelState.AddModelError("","Failed to edit a product" );
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(string id)
        {
            var cProduct = _storeRepo.GetOneProduct(id);
            var viewProduct = new ProductViewModel
            {
                UniqueID = cProduct.UniqueID,
                Name = cProduct.Name,
                Category = cProduct.Category,
                Price = cProduct.Price,
            };

            return View(viewProduct);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // id is the primary key
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
               
                _storeRepo.DeleteOneProduct(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError("", "error while trying to delete a product");
                ViewData["ErrorMsg"] = "Trying to delete a product that does not exist";
                var viewMode = _storeRepo.GetOneProduct(id);

                return View();
            }
        }
    }
}
