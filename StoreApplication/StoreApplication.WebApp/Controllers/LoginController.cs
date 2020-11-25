using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.WebApp.ViewModels;
using StoreDatamodel;
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
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel viewLogin)
        {
            try
            {
                _storeRepo.GetOneCredential();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      

    }
}
