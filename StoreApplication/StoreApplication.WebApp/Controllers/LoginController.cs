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

        private readonly ILogger<LoginController> _logger;
        private readonly IStoreRepository _storeRepo;
        public LoginController(IStoreRepository storeRepo, ILogger<LoginController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }


        public ActionResult Index()
        {
            return View();
        }

    
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

                // admin login
                CAdmincredential cAdmin = _storeRepo.GetOneAdminCredential(viewLogin.Email);
                if (cAdmin != null)
                {
                    if (cAdmin.Password == viewLogin.Password)
                    {
                        // admin successful login
                        TempData["User"] = viewLogin.Email;
                        TempData.Keep("User");
                        // each user can store some information
                        TempData[viewLogin.Email] = 1;
                        return RedirectToAction("Index", "Admin");
                    }
                }

                // memeber login
                CCredential cCredential = _storeRepo.GetOneCredential(viewLogin.Email);
                if (cCredential == null)
                {
                    ModelState.AddModelError("", "This email address has not been registered");
                    return View();
                }                
                
                if (cCredential.Password == viewLogin.Password)
                {
                    // user successful login
                    TempData["User"] = viewLogin.Email;
                    TempData.Keep("User");
                    TempData[viewLogin.Email] = 1;

                }
                else
                {
                    ModelState.AddModelError("", "Password does not match");
                    return View();
                }
                // relative path
                return RedirectToAction("Index","Store");
            }
            catch(Exception e)
            {
                _logger.LogError(e, "error while tring to login");
                ModelState.AddModelError("", "failed to login");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerViewModel viewCustomer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid input format");
                    return View();
                }
                if (viewCustomer.Password != viewCustomer.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match");
                    return View();
                }

                CCustomer cCustomer = _storeRepo.GetOneCustomerByEmail(viewCustomer.Email);
                if (cCustomer != null)
                {
                    ModelState.AddModelError("", "This email is already in use, try a different one");
                    return View();
                }
                else
                {
                    // customer don't type in his ID number, is assigned automatically
                    string customerID = Guid.NewGuid().ToString().Substring(0,10);
                  
                    cCustomer = new CCustomer(customerID, viewCustomer.Firstname, viewCustomer.Lastname, viewCustomer.Phonenumber, viewCustomer.Email);    
                    CCredential cCredential = new CCredential(viewCustomer.Email, viewCustomer.Password);
                    // it is possible that the credential gets in and customer profile not
                    _storeRepo.AddOneCredential(cCredential);
                    _storeRepo.AddOneCustomer(cCustomer);
                    
                    TempData["User"] = cCustomer.Email;
                    TempData.Keep("User");
                    // changed to shopping cart later
                    TempData[cCustomer.Email] = 1;
                   
                }
                return RedirectToAction("Index","Store");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error while trying to register");
                ModelState.AddModelError("", "failed to register");
                return View();
            }
        }
      

    }
}
