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
    static public class Mapper
    {
        // products
        static public IEnumerable<DetailedProductViewModel> MapDetailedProductsWithoutTotal(List<CProduct> products)
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
        static public IEnumerable<DetailedProductViewModel> MapDetailedProducts(List<CProduct> products )
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
            var viewProducts = new DetailedProductViewModel
            {
                UniqueID = product.UniqueID,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity,
            };
            return viewProducts;
        }
        static public DetailedProductViewModel MapSingleDetailedProduct (CProduct product )
        {
            var viewProducts = new DetailedProductViewModel
            {
                UniqueID = product.UniqueID,
                Name =  product.Name,
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity,
                TotalCostPerProduct = product.Price * product.Quantity,
            };
            return viewProducts;
        }

        // customers
        static public IEnumerable<CustomerViewModel> MapCustomersWithoutEmail(Dictionary<string,CCustomer> customers)
        { 
            var viewCustomer = customers.Select(x => new CustomerViewModel
             {
                 Customerid = x.Value.Customerid,
                 Firstname = x.Value.FirstName,
                 Lastname = x.Value.LastName,
                 Phonenumber = x.Value.PhoneNumber,
             });
            return viewCustomer;
        }
        static public IEnumerable<CustomerViewModel> MapCustomers(Dictionary<string, CCustomer> customers)
        {
            var viewCustomer = customers.Select(x => new CustomerViewModel
            {
                Customerid = x.Value.Customerid,
                Firstname = x.Value.FirstName,
                Lastname = x.Value.LastName,
                Phonenumber = x.Value.PhoneNumber,
                Email = x.Value.Email,
            });
            return viewCustomer;
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

    }
}
