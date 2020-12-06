using System;

namespace StoreApplication.WebApp.ViewModels
{
    public class OrderViewModel
    {

        public string Orderid { get; set; }

        public string StoreLoc { get; set; }

        public string Customerid { get; set; }

        public DateTime OrderedTime { get; set; }

        public double TotalCost { get; set; }

    }
}
