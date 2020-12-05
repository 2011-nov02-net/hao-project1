using StoreApplication.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoreLibrary
{
    /// <summary>
    /// static mapper class to convert objects between View and Model
    /// </summary>
    static public class ViewModelMapper
    {
        // products
        static public IEnumerable<DetailedProductViewModel> MapDetailedProductsWithoutTotal(IEnumerable<CProduct> products)
        {
            var viewProducts = products.Select(x => new DetailedProductViewModel
            {
                UniqueID = x.UniqueID,
                Name = x.Name,
                Category = x.Category,
                Price = x.Price,
                Quantity = x.Quantity,         
            });
            return viewProducts;
        }
        static public IEnumerable<DetailedProductViewModel> MapDetailedProducts(IEnumerable<CProduct> products )
        {
            var viewProducts = products.Select(x => new DetailedProductViewModel
            {
                UniqueID = x.UniqueID,
                Name = x.Name,
                Category = x.Category,
                Price = x.Price,
                Quantity = x.Quantity,
                TotalCostPerProduct = x.Price * x.Quantity,
            });
            return viewProducts;
        }
        static public DetailedProductViewModel MapSingleDetailedProductWithoutTotal(CProduct product)
        {
            var viewProduct = new DetailedProductViewModel
            {
                UniqueID = product.UniqueID,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity,
            };
            return viewProduct;
        }
        static public DetailedProductViewModel MapSingleDetailedProduct (CProduct product )
        {
            var viewProduct = new DetailedProductViewModel
            {
                UniqueID = product.UniqueID,
                Name =  product.Name,
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity,
                TotalCostPerProduct = product.Price * product.Quantity,
            };
            return viewProduct;
        }

        // customers
        static public IEnumerable<CustomerViewModel> MapCustomersWithoutEmail(Dictionary<string,CCustomer> customers)
        { 
            var viewCustomers = customers.Select(x => new CustomerViewModel
             {
                 Customerid = x.Value.Customerid,
                 Firstname = x.Value.FirstName,
                 Lastname = x.Value.LastName,
                 Phonenumber = x.Value.PhoneNumber,
             });
            return viewCustomers;
        }
        static public IEnumerable<CustomerViewModel> MapCustomers(Dictionary<string, CCustomer> customers)
        {
            var viewCustomers = customers.Select(x => new CustomerViewModel
            {
                Customerid = x.Value.Customerid,
                Firstname = x.Value.FirstName,
                Lastname = x.Value.LastName,
                Phonenumber = x.Value.PhoneNumber,
                Email = x.Value.Email,
            });
            return viewCustomers;
        }
        static public CustomerViewModel MapSingleCustomerWithCredential(CCustomer customer, CCredential credential )
        {
            var viewCustomer = new CustomerViewModel
            {
                Customerid = customer.Customerid,
                Firstname = customer.FirstName,
                Lastname = customer.LastName,
                Phonenumber = customer.PhoneNumber,
                Email = customer.Email,
                Password = credential.Password,
            };
            return viewCustomer;
        }

        // orders
        static public IEnumerable<OrderViewModel> MapOrders(List<COrder> orders)
        {
            var viewOrders = orders.Select(x => new OrderViewModel
            {
                Orderid = x.Orderid,
                StoreLoc = x.StoreLocation.Storeloc,
                OrderedTime = x.OrderedTime,
                TotalCost = x.TotalCost,
            });
            return viewOrders;
               
        }
    }
}
