using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterController(
            IIdentityServerInteractionService interaction,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _interaction = interaction;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult New([FromQuery] string returnUrl)
        {
            var model = new RegisterUserViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> New([FromForm] RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new IdentityUser(model.Username)
            {
                Email = model.Email,
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError(e.Code, e.Description);
                };
                return View(model);
            }
            await _userManager.AddClaimsAsync(user, new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, model.FirstName + " " + model.LastName),
                new Claim(JwtClaimTypes.GivenName, model.FirstName),
                new Claim(JwtClaimTypes.FamilyName, model.LastName),
                new Claim(JwtClaimTypes.NickName, model.Username),
                new Claim(JwtClaimTypes.Email, model.Email),
            });
            await SendConfirmationEmail(user);
            return View("EmailSent", user.Id);
        }

        [HttpGet]
        public async Task<IActionResult> ResendEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await SendConfirmationEmail(user);
            return View("EmailSent");
        }

        private async Task SendConfirmationEmail(IdentityUser user)
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            emailConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));
            System.Diagnostics.Debug.WriteLine(Url.ActionLink("Activate", "Activation", new { userId = user.Id, token = emailConfirmationToken }));
        }
    }
}
