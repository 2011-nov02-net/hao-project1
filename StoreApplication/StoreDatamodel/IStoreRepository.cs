using System;
using System.Collections.Generic;
using System.Text;
using StoreLibrary;

namespace StoreDatamodel
{
    public interface IStoreRepository
    {  
        // all setup methods pull data from db and return library model objects
        CStore GetOneStore(string storeLoc);

        IEnumerable<CStore> GetAllStoresByZipcode(string zipCode);
        List<CProduct> GetInventoryOfOneStore(string storeLoc);

        List<CProduct> GetInventoryOfOneStoreByCategory(string storeLoc, string category);
        Dictionary<string, CCustomer> GetAllCustomersAtOneStore(string storeLoc);
        List<CCustomer> GetAllCustomersAtOneStoreByName(string storeLoc, string firstname, string lastName);
        List<COrder> GetAllOrdersOfOneCustomer(string customerid, CStore store, CCustomer customer);
        List<CProduct> GetAllProductsOfOneOrder(string orderid);


        // required functionalities 
        void StoreAddOneProduct(string storeLoc, CProduct product, int quantity);
        void StoreAddOneCustomer(string storeLoc, CCustomer customer);
        void CustomerPlaceOneOrder(COrder order, CStore store, double totalCost);
        CCustomer GetOneCustomerByNameAndPhone(string firstName, string lastName, string phonenumber);
       
        COrder GetAnOrderByID(string orderid);
        List<COrder> GetOneCustomerOrderHistory(CCustomer customer, CStore store);
        CStore GetOneStoreOrderHistory(string storeLoc);

        
        // all add methods take library model objects, convert them to dbcontext objects and map them to db
        void AddOneStore(CStore store);
        void AddOneCustomer(CCustomer customer);
        
        void AddOneProduct(CProduct product);
        void AddOneCredential(CCredential cerdential);



        // helper methods
        List<CStore> GetAllStores();
        IEnumerable<CCustomer> GetAllCustomers();
        IEnumerable<CProduct> GetAllProducts();
        CCustomer GetOneCustomerByEmail(string email);
        CCustomer GetOneCustomer(string id);

        CProduct GetOneProductByNameCategoryPrice(string name, string category, double price);
        CProduct GetOneProductByNameAndCategory(string name, string category);
        CProduct GetOneProductWithQuantity(string storeLoc,string productID);
        CProduct GetOneProduct(string productID);

        CCredential GetOneCredential(string email);
        CAdmincredential GetOneAdminCredential(string email);

        // delete methods

        void DeleteOneProduct(string storeLoc, string productID);
        void DeleteOneCustomer(string storeLoc,string customerID);
        void DelelteOneCredential(string email);
        // edit

        public void EditOneStore(CStore store);
        public void EditOneCustomer(CCustomer customer);
        public void EditOneProduct(string storeLoc,CProduct product,int quantity);


        public void EditOneCredential(string previousEmail, CCredential credential);



    }
}
