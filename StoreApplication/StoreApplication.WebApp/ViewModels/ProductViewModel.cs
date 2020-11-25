using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.WebApp.ViewModels
{
    public class ProductViewModel
    {
        [Display( Name = "Product ID")]
        [Required]
        public string UniqueID { get; set; }

        [Display( Name = "Product Name")]
        [Required]
        public string Name { get; set; }

        [Display( Name= "Product Category")]
        [Required]
        public string Category { get; set; }
        
        [Required]
        [Range(0,99999)]
        public double Price { get; set; }

        // public int Quantity { get; set; }


    }
}
