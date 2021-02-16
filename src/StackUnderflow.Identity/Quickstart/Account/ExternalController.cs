using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackUnderflow.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerHost.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<ExternalController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly IEventService _events;
        private readonly IClaimsMappingFactory _claimsMappingFactory;
        private Random _random;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            IClaimsMappingFactory claimsMappingFactory,
            ILogger<ExternalController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signinManager,
            Random random)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _logger = logger;
            _userManager = userManager;
            _signinManager = signinManager;
            _events = events;
            _claimsMappingFactory = claimsMappingFactory;
            _random = random;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string scheme, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
            };

            return Challenge(props, scheme);

        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                _logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);
            if (user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = await AutoProvisionUserAsync(provider, providerUserId, claims);
            }

            // this allows us to collect any additional claims or properties
            // for the specific protocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            await _signinManager.SignInAsync(user, true);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, user.UserName, true, context?.Client.ClientId));

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", returnUrl);
                }
            }

            return Redirect(returnUrl);
        }

        private async Task<(IdentityUser user, string provider, string providerUserId, IEnumerable<Claim> claims)> FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            // find external user
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private async Task<IdentityUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            // We are using this as a username only temporarily.
            // User will confirm or change their preferred username later in the sign up process.
            var username = claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value.Replace(' ', '_');
            if ((await _userManager.FindByNameAsync(username) != null))
            {
                username += "_" + _random.Next(1000).ToString();
            }
            var user = new IdentityUser(username);
            var userLoginInfo = new UserLoginInfo(provider, providerUserId, string.Empty);
            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                throw new Exception(
                    $"Could not provision user with external provider id {providerUserId} based on external provider {provider}, with claims: " +
                    $"{string.Join(';', claims.Select(c => c.Type + ": " + c.Value))}.");
            }
            var addLoginResult = await _userManager.AddLoginAsync(user, userLoginInfo);
            if (!addLoginResult.Succeeded)
            {
                throw new Exception(
                    $"Could not provision user with external provider id {providerUserId} based on external provider {provider}, with claims: " +
                    $"{string.Join(';', claims.Select(c => c.Type + ": " + c.Value))}.");
            }
            var claimsMapper = _claimsMappingFactory.CreateMapper(provider);
            var transformedClaims = claims
                .Select(claim => new Claim(claimsMapper[claim.Type], claim.Value))
                .ToList();
            transformedClaims.Add(new Claim(JwtClaimTypes.NickName, username));
            await _userManager.AddClaimsAsync(user, transformedClaims);
            return user;
        }

        // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
        // this will be different for WS-Fed, SAML2p or other protocols
        private void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties.GetTokenValue("id_token");
            if (idToken != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }
        }
    }
}