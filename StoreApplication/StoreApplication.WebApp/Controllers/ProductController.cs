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
                    ModelState.AddModelError("", "Invalid input format");
                    return View();
                }

                var foundProduct = _storeRepo.GetOneProductByNameAndCategory(viewProduct.Name, viewProduct.Category);
                if (foundProduct != null)
                {
                    ModelState.AddModelError("","This product already exist in this category");
                    return View();
                }

                
                // view model does not have quantity               
                string productID = Guid.NewGuid().ToString().Substring(0, 10);
                var cProduct = new CProduct(productID, viewProduct.Name, viewProduct.Category, viewProduct.Price);
                _storeRepo.AddOneProduct(cProduct);
                
                // add tempdata here

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "error while tring to add a product");
                ModelState.AddModelError("", "failed to create a product");
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(string id)
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

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( string id, ProductViewModel viewProduct)
        {
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

                // see if the edited version already exist in db
                var editedProduct = _storeRepo.GetOneProductByNameCategoryPrice(viewProduct.Name, viewProduct.Category,viewProduct.Price);
                if (editedProduct != null)
                {
                    ModelState.AddModelError("", "A record with the same data already exist in this category");
                    return View();
                }
                               
                foundProduct = new CProduct(foundProduct.UniqueID, viewProduct.Name, viewProduct.Category, viewProduct.Price);
                _storeRepo.EditOneProduct(foundProduct);
                    // add tempo data                                  
                return RedirectToAction(nameof(Index));
            }
            catch( Exception e)
            {
                _logger.LogError( e , "error while trying to edit a product");
                ModelState.AddModelError("","failed to edit a product" );
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
                // concurrent
                var foundProduct = _storeRepo.GetOneProduct(id);
                if (foundProduct == null)
                {
                    ModelState.AddModelError("", "Another admin has just deleted this product");
                    return View();
                }

                _storeRepo.DeleteOneProduct(id);
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
