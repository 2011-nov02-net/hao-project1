using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApplication.WebApp.ViewModels
{
    public class StoreViewModel
    {
        [Required]
        [Display(Name = "Store Location")]
        public string Storeloc { get; set; }

        [Required]
        [Display(Name = "Store Phone Number")]
        [StringLength(10)]
        [RegularExpression("[0-9]*")]
        public string Storephone { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [StringLength(5)]
        public string Zipcode { get; set; }

        public IEnumerable<StoreViewModel> Locations { get; set; }

        // no attributes needed
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
