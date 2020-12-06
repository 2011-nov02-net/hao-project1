namespace StoreLibrary
{
    public class CCredential
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public CCredential(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
