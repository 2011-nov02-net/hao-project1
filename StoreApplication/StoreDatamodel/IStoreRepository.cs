using System;
using System.Collections.Generic;
using System.Text;
using StoreLibrary;

namespace StoreDatamodel
{
    public interface IStoreRepository
    {  
        // all setup methods pull data from db and return library model objects
        CStore GetAStore(string storeLoc);
        List<CProduct> GetInventoryOfAStore(string storeLoc);
        Dictionary<string, CCustomer> GetAllCustomersAtOneStore(string storeLoc);
        List<COrder> GetAllOrdersOfOneCustomer(string customerid, CStore store, CCustomer customer);
        List<CProduct> GetAllProductsOfOneOrder(string orderid);


        // required functionalities in order from 1-7
        void StoreAddOneCusomter(string storeLoc, CCustomer customer);
        void CustomerPlaceOneOrder(COrder order, CStore store, double totalCost);
        CCustomer GetOneCustomerByNameAndPhone(string firstName, string lastName, string phonenumber);
        COrder GetAnOrderByID(string orderid);
        CCustomer GetOneCustomerOrderHistory(string firstName, string lastName, string phoneNumber, CStore store);
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
        CProduct GetOneProduct(string productID);
        CCredential GetOneCredential(string email);
        CAdmincredential GetOneAdminCredential(string email);

        // delete methods

        void DeleteOneProduct(string productID);
        void DeleteOneCustomer(string customerID);
        void DelelteOneCredential(string email);
        // edit

        public void EditOneStore(CStore store);
        public void EditOneCustomer(CCustomer customer);
        public void EditOneProduct(CProduct product);


        public void EditOneCredential(string previousEmail, CCredential credential);



    }
}
