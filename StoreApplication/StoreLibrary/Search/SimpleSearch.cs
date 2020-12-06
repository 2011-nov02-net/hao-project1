namespace StoreLibrary.Search
{
    /// <summary>
    /// simple approach of all search methods
    /// </summary>
    public class SimpleSearch : ISearch
    {
        /// <summary>
        /// simple approach to search by name, returns bool and customerid
        /// </summary>
        public bool SearchByName(CStore store, string firstname, string lastname, out string customerid)
        {
            foreach (var pair in store.CustomerDict)
            {
                CCustomer customer = pair.Value;
                if (firstname == customer.FirstName && lastname == customer.LastName)
                {
                    customerid = pair.Key;
                    return true;
                }
            }
            customerid = "";
            return false;
        }

        /// <summary>
        /// take an extra parameter of phone number to guarantee an accurate search
        /// </summary>
        public bool SearchByNameAndPhone(CStore store, string firstname, string lastname, string phonenumber, out string customerid)
        {
            foreach (var pair in store.CustomerDict)
            {
                CCustomer customer = pair.Value;
                if (firstname == customer.FirstName && lastname == customer.LastName && phonenumber == customer.PhoneNumber)
                {
                    customerid = pair.Key;
                    return true;
                }
            }
            customerid = "";
            return false;
        }
    }
}
