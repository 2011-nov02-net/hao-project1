using StoreDatamodel;
using StoreLibrary;
using System.Collections.Generic;
using System.Linq;

namespace StoreApplication.WebApp
{
    public static class DomainModelMapper
    {
        public static IEnumerable<CStore> MapStore(IEnumerable<Store> stores)
        {
            var domainStores = stores.Select(x => new CStore
            {
                Storeloc = x.Storeloc,
                Storephone = x.Storephone,
                Zipcode = x.Zipcode,
            });
            return domainStores;
        }

        public static CCustomer MapSingleCustomer(Customer customer)
        {
            var domainCustomer = new CCustomer
            {
                Customerid = customer.Customerid,
                FirstName = customer.Firstname,
                LastName = customer.Lastname,
                PhoneNumber = customer.Phonenumber,
                Email = customer.Email,
            };
            return domainCustomer;

        }

    }
}
