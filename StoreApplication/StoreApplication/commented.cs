using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        products.Select(x => new DetailedProductViewModel
    {
        UniqueID = x.UniqueID,
        Name = x.Name,
        Category = x.Category,
        Price = x.Price,
        Quantity = x.Quantity,
        TotalCostPerProduct = x.Price * x.Quantity,
    });
        */
    }
}
