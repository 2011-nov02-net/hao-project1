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
    public class LoginController : Controller
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IStoreRepository _storeRepo;
        public LoginController(IStoreRepository storeRepo, ILogger<ProductController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }


        // GET: LoginController/Create
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel viewLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid login format");
                    return View();
                }

                CCredential cCredential = _storeRepo.GetOneCredential(viewLogin.EmailAddress);
                if (cCredential == null)
                {
                    ModelState.AddModelError("", "This email address has not been registered");
                    return View();
                }


                if (cCredential.Password == viewLogin.Password)
                { 
                    // temp data
                    ModelState.
                }
                else
                {
                    ModelState.AddModelError("", "Password does not match");
                    return View();
                }

                // relative path
                return RedirectToAction("Index","Home");
            }
            catch(Exception e)

            {
                _logger.LogError(e, "error while tring to login");
                ModelState.AddModelError("", "failed to login");
                return View();
            }
        }

      

    }
}
