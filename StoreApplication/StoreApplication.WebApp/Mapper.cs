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
    }
}
