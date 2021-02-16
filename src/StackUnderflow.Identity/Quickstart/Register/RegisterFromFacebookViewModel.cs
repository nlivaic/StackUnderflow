using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterFromFacebookViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters.")]
        [RegularExpression("[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_.1234567890]{5,}", ErrorMessage = "Username must be at least 5 characters long.")]
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [HiddenInput]
        public string ReturnUrl { get; set; }
        [HiddenInput]
        public string Provider { get; set; }
        [HiddenInput]
        public string ProviderUserId { get; set; }
    }
}
