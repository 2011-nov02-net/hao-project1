using System;
using System.Collections.Generic;
using System.Text;
using StoreLibrary;

namespace StoreDatamodel
{
    public interface IStoreRepository
    {  
        // Get methods
        // store level
        CStore GetOneStore(string storeLoc);
        IEnumerable<CStore> GetAllStores();
        IEnumerable<CStore> GetAllStoresByZipcode(string zipCode);
        IEnumerable<CProduct> GetInventoryOfOneStore(string storeLoc);
        List<CProduct> GetInventoryOfOneStoreByCategory(string storeLoc, string category);

        // customer level
        CCustomer GetOneCustomer(string id);
        CCustomer GetOneCustomerByEmail(string email);       
        Dictionary<string, CCustomer> GetAllCustomersAtOneStore(string storeLoc);
        List<CCustomer> GetAllCustomersAtOneStoreByName(string storeLoc, string firstname, string lastName);

        // credential level
        CCredential GetOneCredential(string email);
        CAdmincredential GetOneAdminCredential(string email);

        // order level
        COrder GetAnOrderByID(string orderid);
        List<COrder> GetAllOrdersOfOneCustomer(string customerid, CStore store, CCustomer customer);
        List<COrder> GetOneCustomerOrderHistory(CCustomer customer, CStore store);

        // product level
        CProduct GetOneProduct(string productID);
        CProduct GetOneProductByNameAndCategory(string name, string category);
        CProduct GetOneProductWithQuantity(string storeLoc, string productID);       
        List<CProduct> GetAllProductsOfOneOrder(string orderid);

        // Add methods
        void StoreAddOneProduct(string storeLoc, CProduct product, int quantity);
        void StoreAddOneCustomer(string storeLoc, CCustomer customer);
        void AddOneStore(CStore store);
        void AddOneCustomer(CCustomer customer);
        void AddOneCredential(CCredential cerdential);

        // Edit methods
        public void EditOneProduct(string storeLoc, CProduct product, int quantity);

        // Delete methods
        void DeleteOneProduct(string storeLoc, string productID);
        void DeleteOneCustomer(string storeLoc, string customerID);
        void DelelteOneCredential(string email);

        // Multi-purpose      
        void CustomerPlaceOneOrder(COrder order, CStore store, double totalCost);


    }
}
