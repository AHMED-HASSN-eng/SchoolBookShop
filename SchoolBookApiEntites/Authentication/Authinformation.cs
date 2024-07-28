namespace SchoolBookShop.Authentication
{
    public class Authinformation
    {
        public string Message { get; set; }
        public string UserName { get; set; }    
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime ExpireDate { get; set; }
        public List<string> Roles { get; set;}
    }
}
