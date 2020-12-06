using System;
using System.ComponentModel.DataAnnotations;

namespace StoreApplication.WebApp.ViewModels
{
    // replaced by DetailedProductViewModel
    public class ProductViewModel
    {
        // [Required]
        [Display(Name = "Product ID")]
        public string UniqueID { get; set; }

        [Display(Name = "Product Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Product Category")]
        [Required]
        public string Category { get; set; }

        [Required]
        [Range(0, 99999)]
        public double Price { get; set; }

        // public int Quantity { get; set; }


    }
}
