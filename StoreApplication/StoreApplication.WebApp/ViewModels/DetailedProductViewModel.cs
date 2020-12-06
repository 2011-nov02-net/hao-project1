using System;
using System.ComponentModel.DataAnnotations;

namespace StoreApplication.WebApp.ViewModels
{
    public class DetailedProductViewModel
    {

        [Display(Name = "Product ID")]
        //[Required]
        public string UniqueID { get; set; }

        [Display(Name = "Product Name")]
        //[Required]
        public string Name { get; set; }

        [Display(Name = "Product Category")]
        //[Required]
        public string Category { get; set; }

        //[Required]
        [Range(0, 99999)]
        public double Price { get; set; }

        [Required]
        [Range(0, 99999)]
        public int Quantity { get; set; }

        //[Required]
        [Range(0, 99999)]
        [Display(Name = "Total")]
        public double TotalCostPerProduct { get; set; }
    }
}
