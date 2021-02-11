using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace StackUnderflow.Identity.Quickstart
{
    public class ResetPasswordConfirmationViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Email { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
