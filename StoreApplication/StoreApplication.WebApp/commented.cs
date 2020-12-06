namespace StoreApplication
{
    class commented
    {
        // place order simplified, without considering customer order history
        // map products to an order, orders to a customer,
        // store now has complete information
        //foreach (var pair in store.CustomerDict)
        //{
        //CCustomer customer = pair.Value;
        //customer.OrderHistory = _storeRepo.GetAllOrdersOfOneCustomer(customer.Customerid, store, customer);
        //foreach (var order in customer.OrderHistory)
        //{
        //order.ProductList = _storeRepo.GetAllProductsOfOneOrder(order.Orderid);
        //order.TotalCost = store.CalculateTotalPrice(order.ProductList);
        //}
        //}
        //store.UpdateInventoryAndCustomerOrder(newOrder);


        // drop down list
        /*
            List<string> locations = new List<string>();
            foreach (var location in viewStore)
            {
                locations.Add(location.Storeloc);
            }

            ViewBag.Locations = new SelectList(locations);
        */


        /*
        // caching, use tempdata later
        string path = "../../SimplyWriteData.json";
        JsonFilePersist persist = new JsonFilePersist(path);
        List<CProduct> products = persist.ReadProductsData();
        if (products == null)
        {
            products = new List<CProduct>();
        }
        products.Add(cProduct);
        persist.WriteProductsData(products);
        return RedirectToAction("Select", "Store", new StoreViewModel { Storeloc = TempData.Peek("storeLoc").ToString() });
        */



        /*
        // entire store's order history in one method
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
        */


        /*
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
        */

        /*
        
        */

        /*
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
        */


        /*
        public IEnumerable<CCustomer> GetAllCustomers()
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbCustomers = context.Customers.ToList();
            var CCustomer = dbCustomers.Select(x => new CCustomer(x.Customerid,x.Firstname, x.Lastname,
                                                    x.Phonenumber, x.Email));
            return CCustomer;
        }

        public IEnumerable<CProduct> GetAllProducts()
        {
            using var context = new Project0databaseContext(_contextOptions);
            IEnumerable<Product> dbProducts = context.Products.ToList();
            IEnumerable<CProduct> conProducts = dbProducts.Select(x => new CProduct(x.Productid, x.Name, x.Category, x.Price, 1));
            return conProducts;
        }
        */

        /*public void EditOneStore(CStore store)
        {
            using var context = new Project0databaseContext(_contextOptions);
            var dbStore = context.Stores.FirstOrDefault(x => x.Storeloc == store.Storeloc);
            if (dbStore != null)
            {
                dbStore.Storeloc = store.Storeloc;
                dbStore.Storephone = store.Storephone;
                context.SaveChanges();
            }


         }*/

        /*
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
        */
    }
}
