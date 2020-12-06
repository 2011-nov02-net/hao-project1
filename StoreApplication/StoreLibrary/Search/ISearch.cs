namespace StoreLibrary.Search
{
    /// <summary>
    /// search interface to map out several search methods
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// search by name finds the first that matches
        /// serach by name and phone uniquely identify one
        /// </summary>
        bool SearchByName(CStore store, string firstname, string lastname, out string customerid);
        bool SearchByNameAndPhone(CStore store, string firstname, string lastname, string phonenumber, out string customerid);
    }
}
