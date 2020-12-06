using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace StoreLibrary
{
    /// <summary>
    /// order class, has no behaviors
    /// </summary>
    public class COrder
    {
        /// <summary>
        /// property orderid to uniquely identify an order
        /// </summary>
        public string Orderid { get; set; }

        /// <summary>
        /// property to reference a store location
        /// </summary>
        [JsonIgnore]
        public CStore StoreLocation { get; set; }

        /// <summary>
        /// property to reference a customer
        /// </summary>
        [JsonIgnore]
        public CCustomer Customer { get; set; }

        /// <summary>
        /// property to record date and time of an order 
        /// </summary>
        public DateTime OrderedTime { get; set; }

        private double totalCost;

        /// <summary>
        /// property to record total cost of an order, must set it positive
        /// total cost should be finalized 
        /// </summary>
        public double TotalCost
        {
            get { return totalCost; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("total cost must be non-negative");
                }
                totalCost = value;
            }
        }


        /// <summary>
        /// property to contain a list of products in an order, total quantity must not exceed 500
        /// </summary>
        public List<CProduct> ProductList { get; set; }


        /// <summary>
        /// parameterized constructor
        /// </summary>
        public COrder(CStore storeLocation, CCustomer customer, DateTime orderedTime, double totalCost, List<CProduct> productList)
        {
            StoreLocation = storeLocation;
            Customer = customer;
            OrderedTime = orderedTime;
            TotalCost = totalCost;
            ProductList = productList;
        }

        public COrder(string orderid, CStore storeLocation, CCustomer customer, DateTime orderedTime, double totalCost)
        {
            Orderid = orderid;
            StoreLocation = storeLocation;
            Customer = customer;
            OrderedTime = orderedTime;
            TotalCost = totalCost;
        }      

        public COrder(string orderid, CStore storeLocation, CCustomer customer, double totalCost)
        {
            Orderid = orderid;
            StoreLocation = storeLocation;
            Customer = customer;
            TotalCost = totalCost;
        }

    }
}
