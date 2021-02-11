using System.ComponentModel.DataAnnotations;

namespace StackUnderflow.Identity.Quickstart
{
    public class ResetPasswordRequestViewModel
    {
        [EmailAddress(ErrorMessage = "Email not valid.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}
