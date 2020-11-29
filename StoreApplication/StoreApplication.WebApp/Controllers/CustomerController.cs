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
    public class CustomerController : Controller
    {
        private readonly IStoreRepository _storeRepo;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IStoreRepository storeRepo, ILogger<CustomerController> logger)
        {
            _storeRepo = storeRepo;
            _logger = logger;
        }

        // customers at one store location
        public ActionResult Index(string firstName,string lastName)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            var viewCustomer = _storeRepo.GetAllCustomersAtOneStore(storeLoc).Select(cCustomer => new CustomerViewModel
            {
                Customerid = cCustomer.Value.Customerid,
                Firstname = cCustomer.Value.FirstName,
                Lastname = cCustomer.Value.LastName,
                Phonenumber = cCustomer.Value.PhoneNumber,             
            });

             

            if (!String.IsNullOrEmpty(firstName) && !String.IsNullOrEmpty(lastName))
            {
                // get
                 var customers = _storeRepo.GetAllCustomersAtOneStoreByName(storeLoc, firstName, lastName);
                viewCustomer = customers.Select(x => new CustomerViewModel
                {
                    Customerid = x.Customerid,
                    Firstname = x.FirstName,
                    Lastname = x.LastName,
                    Phonenumber = x.PhoneNumber,

                });
                
            }

            return View(viewCustomer);
        }
        

        // search bar

        public ActionResult Details(string id)
        {
            CCustomer cCustomer = _storeRepo.GetOneCustomer(id);
            CCredential cCredential = _storeRepo.GetOneCredential(cCustomer.Email);

            // concurrent
            if (cCustomer == null)
            {
                ModelState.AddModelError("", "Another admin has just deleted this customer");
                return View();
            }
            if (cCredential == null)
            {
                ModelState.AddModelError("", "Another admin has just deleted this email");
                return View();
            }

            var viewCustomer = new CustomerViewModel
            {
                Customerid = cCustomer.Customerid,
                Firstname = cCustomer.FirstName,
                Lastname = cCustomer.LastName,
                Phonenumber = cCustomer.PhoneNumber,
                Email = cCustomer.Email,
                Password = cCredential.Password               
            };
            return View(viewCustomer);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel viewCustomer)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            try
            {
                if (!ModelState.IsValid)
                {
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
                    string customerID = Guid.NewGuid().ToString().Substring(0, 10);
                    cCustomer = new CCustomer(customerID, viewCustomer.Firstname, viewCustomer.Lastname, viewCustomer.Phonenumber, viewCustomer.Email);
                    CCredential cCredential = new CCredential(viewCustomer.Email, viewCustomer.Password);

                    // it is possible that the credential gets in and customer profile not
                    _storeRepo.AddOneCredential(cCredential);
                    _storeRepo.StoreAddOneCustomer(storeLoc,cCustomer);               

                }
                return RedirectToAction(nameof(Create));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error while trying to register");
                ModelState.AddModelError("", "failed to register");
                return View();
            }
        }
            // GET: CustomerController/Edit/5

        public ActionResult Edit(string id)
        {
            var cCustomer = _storeRepo.GetOneCustomer(id);
            CCredential cCredential = _storeRepo.GetOneCredential(cCustomer.Email);
            var viewCustomer = new CustomerViewModel
            {
                Customerid = cCustomer.Customerid,
                Firstname = cCustomer.FirstName,
                Lastname = cCustomer.LastName,
                Phonenumber = cCustomer.PhoneNumber,
                Email = cCustomer.Email,
                Password = cCredential.Password,
                //ConfirmPassword = cCredential.Password
                
            };
            return View(viewCustomer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, CustomerViewModel viewCustomer)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "invalid input format");
                    return View();
                }

                // concurrent
                CCustomer foundCustomer = _storeRepo.GetOneCustomer(id);
                if (foundCustomer == null)
                {
                    ModelState.AddModelError("", "Another Admin has just deleted this customer");
                    return View();
                }
                CCredential foundCredential = _storeRepo.GetOneCredential(foundCustomer.Email);
                if (foundCredential == null)
                {
                    ModelState.AddModelError("", "Another Admin has just deleted this email");
                    return View();
                }


                // if you have changed email
                if (foundCustomer.Email != viewCustomer.Email)
                {
                    // check if the changed email has already been used by someone else
                    CCustomer editedCustomer1 = _storeRepo.GetOneCustomerByEmail(viewCustomer.Email);
                    if (editedCustomer1 != null)
                    {
                        ModelState.AddModelError("", "This email is already in use");
                        return View();
                    }
                    
                }
                var editedCustomer = new CCustomer(id, viewCustomer.Firstname, viewCustomer.Lastname, viewCustomer.Phonenumber, viewCustomer.Email);
                var editedCredential = new CCredential(viewCustomer.Email, viewCustomer.Password);
                _storeRepo.DeleteOneCustomer(storeLoc,id);
                _storeRepo.DelelteOneCredential(foundCustomer.Email);
                // drop dependcy issue
                //_storeRepo.EditOneCredential(foundCredential.Email,editedCredential);
                _storeRepo.AddOneCredential(editedCredential);
                _storeRepo.StoreAddOneCustomer(storeLoc,editedCustomer);
                
                // add tempo data

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error while trying to edit a customer");
                ModelState.AddModelError("", "failed to edit a customer");
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(string id)
        {
            var cCustomer = _storeRepo.GetOneCustomer(id);
            CCredential cCredential = _storeRepo.GetOneCredential(cCustomer.Email);
            var viewCustomer = new CustomerViewModel
            {
                Customerid = cCustomer.Customerid,
                Firstname = cCustomer.FirstName,
                Lastname = cCustomer.LastName,
                Phonenumber = cCustomer.PhoneNumber,
                Email = cCustomer.Email,
                Password = cCredential.Password,
                ConfirmPassword = cCredential.Password

            };
            return View(viewCustomer);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, int unused)
        {
            string storeLoc = TempData.Peek("adminLoc").ToString();
            try
            {
                var foundCustomer = _storeRepo.GetOneCustomer(id);
                if (foundCustomer == null)
                {
                    ModelState.AddModelError("", "Another admin has already deleted this customer");
                    return View();
                }

                var foundCredential = _storeRepo.GetOneCredential(foundCustomer.Email);
                if (foundCredential == null)
                {
                    ModelState.AddModelError("", "Another admin has already deleted this email");
                    return View();
                }

                _storeRepo.DeleteOneCustomer(storeLoc,id);
                _storeRepo.DelelteOneCredential(foundCustomer.Email);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error while trying to delete a product");
                ModelState.AddModelError("", "Trying to delete a product that does not exist");
                var viewMode = _storeRepo.GetOneProduct(id);

                return View();
            }
        }
    }
}
