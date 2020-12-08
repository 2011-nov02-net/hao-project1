using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace StoreApplication.WebApp.ViewModels
{
    // bind never
    public class BindedProductViewModel
    {
        [BindNever]
        // [Required]
        [Display(Name = "Product ID")]
        public string UniqueID { get; set; }

        [BindNever]
        [Display(Name = "Product Name")]
        //[Required]
        public string Name { get; set; }

        [BindNever]
        [Display(Name = "Product Category")]
        //[Required]
        public string Category { get; set; }

        [BindNever]
        //[Required]
        [Range(0, 99999)]
        public double Price { get; set; }

        [Required]
        [Range(0, 99999)]
        public int Quantity { get; set; }


    }
}
