using Microsoft.EntityFrameworkCore;
using StoreLibrary;

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
        // Get methods
        // store level
        public CStore GetOneStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            // store has no customer profile yet
            CStore domainStore = new CStore(dbStore.Storeloc, dbStore.Storephone,dbStore.Zipcode);          
            return domainStore;
        }
        public IEnumerable<CStore> GetAllStores()
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStores = context.Stores.ToList();
            if (dbStores == null) return null;

            var domainStores = dbStores.Select(x => new CStore
            {
                Storeloc = x.Storeloc,
                Storephone = x.Storephone,
                Zipcode = x.Zipcode,
            });


            /*
            List<CStore> stores = new List<CStore>();
            foreach (var store in dbStores)
            {
                CStore s = new CStore(store.Storeloc, store.Storephone, store.Zipcode);
                stores.Add(s);
            }
            */

            return domainStores;
        }
        public IEnumerable<CStore> GetAllStoresByZipcode(string zipCode)
        { 
            using var context = new Project0databaseContext(_contextOptions);
            IEnumerable<Store> dbStores ;
            try
            {
                dbStores = context.Stores.Where(x => x.Zipcode == zipCode).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
            var stores = dbStores.Select(x => new CStore
            {
                Storeloc = x.Storeloc,
                Storephone = x.Storephone,
                Zipcode = x.Zipcode,
            });
            return stores;
        }
        public IEnumerable<CProduct> GetInventoryOfOneStore(string storeLoc)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                            .ThenInclude(x => x.Product)
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;

            var domainInv = dbStore.Inventories.Select(x => new CProduct
            {
                UniqueID = x.Productid,
                Name = x.Product.Name,
                Category = x.Product.Category,
                Price = x.Product.Price,
                Quantity = x.Quantity,

            });
            /*
            List<CProduct> inventory = new List<CProduct>();
            foreach (var product in dbStore.Inventories)
            {
                CProduct p = new CProduct(product.Product.Productid, product.Product.Name,
                                            product.Product.Category, product.Product.Price, product.Quantity);
                inventory.Add(p);
            }
            */
            return domainInv;
        }
        public List<CProduct> GetInventoryOfOneStoreByCategory(string storeLoc,string category)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.Include(x => x.Inventories)
                                            .ThenInclude(x => x.Product)
                                                .FirstOrDefault(x => x.Storeloc == storeLoc);
            if (dbStore == null) return null;
            List<CProduct> inventory = new List<CProduct>();
            CProduct p = new CProduct();
            foreach (var product in dbStore.Inventories)
            {
                
                if (product.Product.Category == category)
                {
                    p = new CProduct(product.Product.Productid, product.Product.Name,
                                                product.Product.Category, product.Product.Price, product.Quantity);
                    inventory.Add(p);
                }
                    
            }
            return inventory;
        }


        // customer level
        public CCustomer GetOneCustomer(string id)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(x => x.Customerid == id);
            if (dbCustomer == null) return null;
            CCustomer cCustomer = new CCustomer(dbCustomer.Customerid, dbCustomer.Firstname, dbCustomer.Lastname,
                                               dbCustomer.Phonenumber, dbCustomer.Email);
            return cCustomer;
        }
        public CCustomer GetOneCustomerByEmail(string email)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.FirstOrDefault(x => x.Email == email);
            if (dbCustomer == null) return null;
            CCustomer c = new CCustomer(dbCustomer.Customerid, dbCustomer.Firstname, dbCustomer.Lastname, dbCustomer.Phonenumber, dbCustomer.Email);

            return c;
        }      
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

        // credential level
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

        // order level
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
        public List<COrder> GetAllOrdersOfOneCustomer(string customerid, CStore store, CCustomer customer)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomer = context.Customers.Include(x => x.Orderrs).FirstOrDefault(x => x.Customerid == customerid);
            if (dbCustomer == null) return null;
            List<COrder> orders = new List<COrder>();
            if (dbCustomer.Orderrs == null) return null;

            foreach (var order in dbCustomer.Orderrs)
            {
                // these orders have no product list
                // total cost not yet set
                COrder o = new COrder(order.Orderid, store, customer, order.Orderedtime,order.Totalcost);
                orders.Add(o);
            }

            return orders;

        }
        public List<COrder> GetOneCustomerOrderHistory(CCustomer customer, CStore store)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var customerExist = context.Storecustomers.FirstOrDefault(x => x.Storeloc == store.Storeloc && x.Customerid == customer.Customerid);
            if (customerExist == null) return null;

            List<COrder> OrderHistory = GetAllOrdersOfOneCustomer(customer.Customerid, store, customer);
            // has no order
            if (OrderHistory == null) return null;

            foreach (var order in OrderHistory)
            {
                order.ProductList = GetAllProductsOfOneOrder(order.Orderid);
                order.TotalCost = store.CalculateTotalPrice(order.ProductList);
            }
            return OrderHistory;
        }
      
        // product level
        public CProduct GetOneProduct(string productID)
        {
            using var context = new Project0databaseContext(_contextOptions);

            var dbProduct = context.Products.FirstOrDefault(x => x.Productid == productID);
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
        public CProduct GetOneProductWithQuantity(string storeLoc, string productID)
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

        // Add methods
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
       

        // Edit methods
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

        // Delete methods
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

        // Multi-purpsoe
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
    }
}


        

