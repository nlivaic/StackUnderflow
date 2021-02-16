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
            await SendConfirmationEmail(user, model.ReturnUrl);
            return View("EmailSent", new ResendEmailConfirmationViewModel
            {
                UserId = user.Id,
                ReturnUrl = model.ReturnUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> ResendEmail(ResendEmailConfirmationViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            await SendConfirmationEmail(user, model.ReturnUrl);
            return View("EmailSent", model);
        }

        [HttpGet]
        public IActionResult RegisterFromFacebook(RegisterFromFacebookInputViewModel model)
        {
            var registerInput = new RegisterFromFacebookViewModel
            {
                Username = model.Username,
                FirstName = model.FirstName,
                Email = model.Email,
                LastName = model.LastName,
                ReturnUrl = model.ReturnUrl,
                Provider = model.Provider,
                ProviderUserId = model.ProviderUserId
            };
            return View(registerInput);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterFromFacebook(RegisterFromFacebookViewModel model)
        {
            var user = new IdentityUser(model.Username);
            var userLoginInfo = new UserLoginInfo(model.Provider, model.ProviderUserId, model.Provider);
            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                throw new Exception(
                    $"Could not provision user with external provider id {model.ProviderUserId} based on external provider {model.Provider}.");
            }
            var addLoginResult = await _userManager.AddLoginAsync(user, userLoginInfo);
            if (!addLoginResult.Succeeded)
            {
                throw new Exception(
                    $"Could not provision user with external provider id {model.ProviderUserId} based on external provider {model.Provider}.");
            }
            await _userManager.AddClaimsAsync(user, new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, model.FirstName + " " + model.LastName),
                new Claim(JwtClaimTypes.GivenName, model.FirstName),
                new Claim(JwtClaimTypes.FamilyName, model.LastName),
                new Claim(JwtClaimTypes.NickName, model.Username),
                new Claim(JwtClaimTypes.Email, model.Email),
            });
            return RedirectToAction("Callback", "External");
        }
        private async Task SendConfirmationEmail(IdentityUser user, string returnUrl)
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            emailConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));
            System.Diagnostics.Debug.WriteLine(Url.ActionLink("Activate", "Activation", new { userId = user.Id, token = emailConfirmationToken, returnUrl }));
        }
    }
}
