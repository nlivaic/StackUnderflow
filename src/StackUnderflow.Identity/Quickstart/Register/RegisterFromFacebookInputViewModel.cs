namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterFromFacebookInputViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReturnUrl { get; set; }
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
    }
}
