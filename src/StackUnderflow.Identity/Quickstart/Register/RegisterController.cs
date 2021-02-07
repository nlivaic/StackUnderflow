using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                new Claim(ClaimTypes.Name, model.FirstName + " " + model.LastName),
                new Claim(ClaimTypes.GivenName, model.FirstName),
                new Claim("family_name", model.LastName),
                new Claim("nickname", model.Username),
                new Claim(ClaimTypes.Email, model.Email),
            });
            await _signInManager.SignInAsync(user, false);

            if (_interaction.IsValidReturnUrl(model.ReturnUrl)
                || Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect("~/");
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("Invalid return URL.");
            }
        }
    }
}
