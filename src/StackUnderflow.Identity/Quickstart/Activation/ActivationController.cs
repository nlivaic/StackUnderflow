using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace StackUnderflow.Identity.Quickstart.Activation
{
    public class ActivationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;

        public ActivationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
        }

        [HttpGet]
        public async Task<IActionResult> Activate(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                // @nl: log
                return View("NoActivation", new ActivateAccountViewModel());
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return View("NoActivation", new ActivateAccountViewModel { EmailAlreadyConfirmed = true });
            }
            var decodedRaw = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedRaw);
            var confirmationResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!confirmationResult.Succeeded)
            {
                // @nl: log
                return View("NoActivation", new ActivateAccountViewModel());
            }
            return View();
        }
    }
}
