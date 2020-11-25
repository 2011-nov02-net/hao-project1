using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.WebApp.ViewModels
{
    public class StoreViewModel
    {
        [Display( Name = "Store Location") ]
        [Required]
        public string Storeloc { get; set; }

        [Display( Name = "Store Phone Number")]
        [Required]
        [StringLength(10)]
        [RegularExpression("[0-9]*")]
        public string Storephone { get; set; }

        // no attributes needed
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
