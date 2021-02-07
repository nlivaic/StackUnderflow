using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters.")]
        [RegularExpression("[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_.1234567890]{5,}", ErrorMessage = "Username must be at least 5 characters long.")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [EmailAddress(ErrorMessage = "Email not valid.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReturnUrl { get; set; }
    }
}
