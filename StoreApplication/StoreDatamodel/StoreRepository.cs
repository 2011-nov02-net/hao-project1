﻿using Microsoft.EntityFrameworkCore;
using StoreLibrary;
using StoreLibrary.IDGenerator;
using StoreLibrary.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoreDatamodel
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DbContextOptions<Project0databaseContext> _contextOptions;
        public StoreRepository(DbContextOptions<Project0databaseContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        // M V C design
        // re-implementation seperating business and data-access
        // create a default store with no customer profile and inventory    

        // used
        public CStore GetOneStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            // store has no customer profile yet
            CStore store = new CStore(dbStore.Storeloc, dbStore.Storephone);
            return store;
        }

        // used
        // create a dict of products that can be added to a given store
        public List<CProduct> GetInventoryOfOneStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                            .ThenInclude(x => x.Product)
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            List<CProduct> inventory = new List<CProduct>();
            foreach (var product in dbStore.Inventories)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name,
                                            product.Product.Category, product.Product.Price, product.Quantity);
                inventory.Add(p);
            }
            return inventory;
        }

        // used
        // create a dictionary of customer to be added to a given store
        public Dictionary<string, CCustomer> GetAllCustomersAtOneStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Storecustomers)
                                            .ThenInclude(x => x.Customer)
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            Dictionary<string, CCustomer> customers = new Dictionary<string, CCustomer>();
            foreach (var customer in dbStore.Storecustomers)
            {
                CCustomer c = new CCustomer(customer.Customer.Customerid, customer.Customer.Firstname,
                                                customer.Customer.Lastname, customer.Customer.Phonenumber,customer.Customer.Email);
                // these customers have no order history atm
                customers[c.Customerid] = c;
            }
            return customers;
        }

        public List<CCustomer> GetAllCustomersAtOneStoreByName(string storeLoc,string firstname,string lastName)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Storecustomers)
                                            .ThenInclude(x => x.Customer)
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            List<CCustomer> customers = new List<CCustomer>();
            foreach (var customer in dbStore.Storecustomers)
            {
                if (customer.Customer.Firstname == firstname && customer.Customer.Lastname == lastName)
                {
                    CCustomer c = new CCustomer(customer.Customer.Customerid, customer.Customer.Firstname,
                                                customer.Customer.Lastname, customer.Customer.Phonenumber, customer.Customer.Email);
                    // these customers have no order history atm
                    customers.Add(c);
                }
                
            }
            return customers;
        }

        // create a list of order for a customer
        public List<COrder> GetAllOrdersOfOneCustomer(string customerid, CStore store, CCustomer customer)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.Include(x => x.Orderrs).FirstOrDefault(x => x.Customerid == customerid);
            if (dbCustomer == null) return null;            
            if (dbCustomer.Orderrs == null) return null;

            List<COrder> orders = new List<COrder>();
            foreach (var order in dbCustomer.Orderrs)
            {
                // these orders have no product list
                // total cost not yet set
                COrder o = new COrder(order.Orderid, store, customer, order.Orderedtime,order.Totalcost);
                orders.Add(o);
            }

            return orders;

        }

        // create a list of products for an order
        public List<CProduct> GetAllProductsOfOneOrder(string orderid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbOrder = context.Orderrs.Include(x => x.Orderproducts)
                                            .ThenInclude(x => x.Product)
                                                .FirstOrDefault(x => x.Orderid == orderid);
            if (dbOrder == null) return null;
            List<CProduct> products = new List<CProduct>();
            foreach (var product in dbOrder.Orderproducts)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name, product.Product.Category,
                                            product.Product.Price, product.Quantity);
                products.Add(p);
            }
            return products;
        }


        // core functionalities
        public void StoreAddOneProduct(string storeLoc, CProduct product, int quantity)
        { 
            using var context = new Project0databaseContext(_contextOptions);
            var newProduct = new Product
            {
                Productid = product.UniqueID,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
            };
            context.Products.Add(newProduct);
            context.SaveChanges();

            var newBridge = new Inventory
            {
                Storeloc = storeLoc,
                Productid = product.UniqueID,
                Quantity = quantity,
            };
            context.Inventories.Add(newBridge);
            context.SaveChanges();
        }

        public void StoreAddOneCustomer(string storeLoc, CCustomer customer)
        {
            using var context = new Project0databaseContext(_contextOptions);
            // only have this part below in the data model, rest moves to console main
            var newCustomer = new Customer
            {
                Customerid = customer.Customerid,
                Firstname = customer.FirstName,
                Lastname = customer.LastName,
                Phonenumber = customer.PhoneNumber,
                Email = customer.Email,
            };
            context.Customers.Add(newCustomer);
            context.SaveChanges();

            // many to many, bridge table gets updated as well
            var newBridge = new Storecustomer
            {
                Storeloc = storeLoc,
                Customerid = customer.Customerid
            };
            context.Storecustomers.Add(newBridge);
            context.SaveChanges();

        }

        // same changes, only keep the part that updates tables, move others to class model or console main
        public void CustomerPlaceOneOrder(COrder order, CStore store, double totalCost)
        {
            using var context = new Project0databaseContext(_contextOptions);
            // update order
            var newOrder = new Orderr
            {
                Orderid = order.Orderid,
                Storeloc = order.StoreLocation.Storeloc,
                Customerid = order.Customer.Customerid,
                Orderedtime = DateTime.Now,
                Totalcost = totalCost
            };
            context.Orderrs.Add(newOrder);
            context.SaveChanges();

            // update Orderproduct  
            foreach (var product in order.ProductList)
            {
                var newOP = new Orderproduct
                {
                    Orderid = order.Orderid,
                    Productid = product.UniqueID,
                    Quantity = product.Quantity
                };
                context.Orderproducts.Add(newOP);
            }
            context.SaveChanges();

            var dbStore = context.Stores.Include(x => x.Inventories)
                                                .FirstOrDefault(x => x.Storeloc == order.StoreLocation.Storeloc);
            // if (dbStore == null) return null;
            // update inventory quantity          
            foreach (var product in order.ProductList)
            {
                foreach (var dbProd in dbStore.Inventories)
                {
                    if (product.UniqueID == dbProd.Productid)
                    {
                        dbProd.Quantity = store.Inventory[product.UniqueID].Quantity;
                    }
                }
            }
            context.SaveChanges();

        }
        // simply search for a customer       
        public CCustomer GetOneCustomerByNameAndPhone(string firstName, string lastName, string phonenumber)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers
                             .FirstOrDefault(x => x.Firstname == firstName && x.Lastname == lastName && x.Phonenumber == phonenumber);
            if (dbCustomer == null) return null;
            CCustomer foundCustomer;
            foundCustomer = new CCustomer(dbCustomer.Customerid,
                                                    dbCustomer.Firstname, dbCustomer.Lastname, dbCustomer.Phonenumber);
            return foundCustomer;
        }

        public COrder GetAnOrderByID(string orderid)
        {
            using var context = new Project0databaseContext(_contextOptions);
            Orderr dbOrder = context.Orderrs
                                    .Include(x => x.Orderproducts)
                                        .ThenInclude(x => x.Product)
                                        .FirstOrDefault(x => x.Orderid == orderid);
            Orderr dbCustomer = context.Orderrs.Include(x => x.Customer).FirstOrDefault(x => x.Orderid == orderid);
            if (dbOrder == null) return null;
            if (dbCustomer == null) return null;

            COrder order = new COrder(orderid, new CStore(dbOrder.Storeloc),
                                               new CCustomer(dbCustomer.Customer.Firstname, dbCustomer.Customer.Lastname,
                                                             dbCustomer.Customer.Phonenumber),
                                               dbOrder.Orderedtime, dbOrder.Totalcost);

            foreach (var product in dbOrder.Orderproducts)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name,
                                        product.Product.Category, product.Product.Price, product.Quantity);
                order.ProductList.Add(p);
            }
            return order;
        }
        // find all detail of a customer
        public CCustomer GetOneCustomerOrderHistory(string firstName, string lastName, string phoneNumber, CStore store)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers
                             .FirstOrDefault(x => x.Firstname == firstName && x.Lastname == lastName && x.Phonenumber == phoneNumber);
            if (dbCustomer == null) return null;
            CCustomer foundCustomer;
            foundCustomer = new CCustomer(dbCustomer.Customerid,
                                                    dbCustomer.Firstname, dbCustomer.Lastname, dbCustomer.Phonenumber);

            List<COrder> OrderHistory = GetAllOrdersOfOneCustomer(foundCustomer.Customerid, store, foundCustomer);
            if (OrderHistory == null) return null;
            foundCustomer.OrderHistory = OrderHistory;

            foreach (var order in foundCustomer.OrderHistory)
            {
                order.ProductList = GetAllProductsOfOneOrder(order.Orderid);
                order.TotalCost = store.CalculateTotalPrice(order.ProductList);
            }

            return foundCustomer;
        }

        public CStore GetOneStoreOrderHistory(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            // store has no customer profile yet
            CStore seekStore = new CStore(dbStore.Storeloc, dbStore.Storephone);
            seekStore.CustomerDict = GetAllCustomersAtOneStore(storeLoc);

            foreach (var customer in seekStore.CustomerDict)
            {
                CCustomer cust = customer.Value;
                cust.OrderHistory = GetAllOrdersOfOneCustomer(cust.Customerid, seekStore, cust);
                foreach (var order in cust.OrderHistory)
                {
                    order.ProductList = GetAllProductsOfOneOrder(order.Orderid);
                    order.TotalCost = seekStore.CalculateTotalPrice(order.ProductList);
                }
            }
            return seekStore;
        }




        // add methods
        public void AddOneStore(CStore store)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var newStore = new Store
            {
                Storeloc = store.Storeloc,
                Storephone = store.Storephone
            };
            context.Stores.Add(newStore);
            context.SaveChanges();

        }

        // web version
        public void AddOneCustomer(CCustomer customer)
        { 
            using var context = new Project0databaseContext(_contextOptions);
            var newCustomer = new Customer
            {
                Customerid = customer.Customerid,
                Firstname = customer.FirstName,
                Lastname = customer.LastName,
                Phonenumber = customer.PhoneNumber,
                Email = customer.Email

            };
            context.Customers.Add(newCustomer);
            context.SaveChanges();
        }

  

        public void AddOneProduct(CProduct product)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var newProduct = new Product
            {
                Productid = product.UniqueID,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price
            };
            context.Products.Add(newProduct);
            context.SaveChanges();

        }

        public void AddOneCredential(CCredential credential)
        {
            using var context = new Project0databaseContext(_contextOptions);
            Credential cCredential = new Credential
            {
                Email = credential.Email,
                Password = credential.Password
            };
            context.Credentials.Add(cCredential);
            context.SaveChanges();
        }

       



         
        public List<CStore> GetAllStores()
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStores = context.Stores.ToList();
            if (dbStores == null) return null;
            List<CStore> stores = new List<CStore>();
            foreach (var store in dbStores)
            {
                CStore s = new CStore(store.Storeloc, store.Storephone);
                stores.Add(s);
            }
            return stores;
        }

        public IEnumerable<CCustomer> GetAllCustomers()
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomers = context.Customers.ToList();
            var CCustomer = dbCustomers.Select(x => new CCustomer(x.Customerid,x.Firstname, x.Lastname,
                                                    x.Phonenumber, x.Email));
            return CCustomer;
;        }

        public IEnumerable<CProduct> GetAllProducts()
        {
            using var context = new Project0databaseContext(_contextOptions);
            IEnumerable<Product> dbProducts = context.Products.ToList();
            IEnumerable<CProduct> conProducts = dbProducts.Select(x => new CProduct(x.Productid, x.Name, x.Category, x.Price, 1));
            return conProducts;
        }

        public CCustomer GetOneCustomerByEmail(string email)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(x => x.Email == email);
            if (dbCustomer == null) return null;
            CCustomer c = new CCustomer(dbCustomer.Customerid, dbCustomer.Firstname, dbCustomer.Lastname, dbCustomer.Phonenumber, dbCustomer.Email);
            
            return c;
        }

        public CCustomer GetOneCustomer(string id)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(x => x.Customerid == id);
            if (dbCustomer == null) return null;
            CCustomer cCustomer = new CCustomer(dbCustomer.Customerid, dbCustomer.Firstname, dbCustomer.Lastname,
                                               dbCustomer.Phonenumber, dbCustomer.Email);
            return cCustomer;
        }

        //
        public CProduct GetOneProductByNameCategoryPrice(string name, string category, double price)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbProduct = context.Products.FirstOrDefault(x => x.Name == name && x.Category == category && x.Price == price);
            if (dbProduct == null) return null;
            CProduct p = new CProduct(dbProduct.Productid, dbProduct.Name, dbProduct.Category, dbProduct.Price);

            return p;
        }

        public CProduct GetOneProductByNameAndCategory(string name, string category)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbProduct = context.Products.FirstOrDefault(x => x.Name == name && x.Category == category);
            if (dbProduct == null) return null;
            CProduct p = new CProduct(dbProduct.Productid, dbProduct.Name, dbProduct.Category, dbProduct.Price);

            return p;
        }

        public CProduct GetOneProductWithQuantity(string storeLoc,string productID)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                            .ThenInclude(x => x.Product)
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            // List<CProduct> inventory = new List<CProduct>();
            CProduct p = new CProduct();
            foreach (var product in dbStore.Inventories)
            {
                if (product.Productid == productID)
                {
                    p = new CProduct(product.Product.Productid, product.Product.Name,
                                            product.Product.Category, product.Product.Price, product.Quantity);
                }
            }
            return p;

        }

        public CProduct GetOneProduct(string productID)
        { 
            using var context = new Project0databaseContext(_contextOptions);
  
            var dbProduct = context.Products.FirstOrDefault(x => x.Productid == productID);
            if (dbProduct == null) return null;
            CProduct p = new CProduct(dbProduct.Productid,dbProduct.Name, dbProduct.Category, dbProduct.Price);
            return p;

        }
       
        public CCredential GetOneCredential(string email)
        { 
            using var context = new Project0databaseContext(_contextOptions);
            var dbCredential = context.Credentials.FirstOrDefault(x => x.Email == email);
            if (dbCredential == null) return null;
            CCredential c = new CCredential(dbCredential.Email, dbCredential.Password);
            return c;
        }
        public CAdmincredential GetOneAdminCredential(string email)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbAdmincredential = context.Admincredentials.FirstOrDefault(x => x.Email == email);
            if (dbAdmincredential == null) return null;
            CAdmincredential a = new CAdmincredential(dbAdmincredential.Email, dbAdmincredential.Password);
            return a;
        }



        // delete methods
        // all set to on delete cascade
        public void DeleteOneProduct(string storeLoc, string productID)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbBridge = context.Inventories.FirstOrDefault(x => x.Storeloc == storeLoc && x.Productid == productID);
            if (dbBridge != null)
            {
                context.Inventories.Remove(dbBridge);
                context.SaveChanges();
            }

            var dbProduct = context.Products.FirstOrDefault(x => x.Productid == productID);
            if (dbProduct != null)
            {
                context.Products.Remove(dbProduct);
                context.SaveChanges();
            }
            // null references handled in the view layer
        }


        public void DeleteOneCustomer(string storeLoc, string customerID)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbBridge = context.Storecustomers.FirstOrDefault(x => x.Storeloc == storeLoc && x.Customerid == customerID);
            if (dbBridge != null)
            {
                context.Storecustomers.Remove(dbBridge);
                context.SaveChanges();
            }

            var dbCustomer = context.Customers.FirstOrDefault(x => x.Customerid == customerID);
            if (dbCustomer != null)
            {
                context.Customers.Remove(dbCustomer);
                context.SaveChanges();
            }
            // null references handled in the view layer
        }

        public void DelelteOneCredential(string email)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCredential = context.Credentials.FirstOrDefault(x => x.Email == email);
            if (dbCredential != null)
            {
                context.Credentials.Remove(dbCredential);
                context.SaveChanges();
            }
        }




        // edit methods
        public void EditOneStore(CStore store)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(x => x.Storeloc == store.Storeloc);
            if (dbStore != null)
            {
                dbStore.Storeloc = store.Storeloc;
                dbStore.Storephone = store.Storephone;
                context.SaveChanges();
            }


        }
        public void EditOneCustomer(CCustomer customer)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(x => x.Customerid == customer.Customerid);
            if (dbCustomer != null)
            {
                dbCustomer.Customerid = customer.Customerid;
                dbCustomer.Firstname = customer.FirstName;
                dbCustomer.Lastname = customer.LastName;
                dbCustomer.Phonenumber = customer.PhoneNumber;
                dbCustomer.Email = customer.Email;
                context.SaveChanges();
            }

        }
        public void EditOneProduct(string storeLoc, CProduct product, int quantity)
        { 
            using var context = new Project0databaseContext(_contextOptions);
            var dbBridge = context.Inventories.FirstOrDefault(x => x.Storeloc == storeLoc && x.Productid == product.UniqueID);
            if (dbBridge != null)
            {
                dbBridge.Quantity = quantity;
                context.SaveChanges();
            }


            var dbProduct = context.Products.FirstOrDefault(x => x.Productid == product.UniqueID);
            if (dbProduct != null)
            {
                dbProduct.Productid = product.UniqueID;
                dbProduct.Name = product.Name;
                dbProduct.Category = product.Category;
                dbProduct.Price = product.Price;
                context.SaveChanges();
            }
        }

        public void EditOneCredential(string previousEmail, CCredential credential)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCredential = context.Credentials.FirstOrDefault(x => x.Email == previousEmail);
            if (dbCredential != null)
            {
                dbCredential.Email = credential.Email;
                dbCredential.Password = credential. Password;
            
                context.SaveChanges();
            }
        }
       

        

    }
}


        

