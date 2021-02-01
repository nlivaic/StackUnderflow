using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StackUnderflow.Identity.Seeders
{
    public static class Seeder
    {
        public static IHost Seed(this IHost host)
        {
            IHostEnvironment hostEnvironment = (IHostEnvironment)host.Services.GetService(typeof(IHostEnvironment));
            if (hostEnvironment.IsDevelopment())
            {
                using (var scope = host.Services.CreateScope())
                {
                    var userManager = (UserManager<IdentityUser>)scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    if (!userManager.Users.Any())
                    {
                        var user1 = new IdentityUser
                        {
                            UserName = "Frank",
                            Email = "liva.spamster@gmail.com"
                        };
                        var user2 = new IdentityUser
                        {
                            UserName = "Claire",
                            Email = "liva.spamster@gmail.com"
                        };
                        userManager.CreateAsync(user1, "password").Wait();
                        userManager.AddClaimAsync(user1, new Claim(JwtClaimTypes.Name, "Frank Underwood")).Wait();
                        userManager.AddClaimAsync(user1, new Claim(JwtClaimTypes.GivenName, "Frank")).Wait();
                        userManager.AddClaimAsync(user1, new Claim(JwtClaimTypes.FamilyName, "Underwood")).Wait();
                        userManager.AddClaimAsync(user1, new Claim(JwtClaimTypes.NickName, "Luettgen")).Wait();
                        userManager.CreateAsync(user2, "password").Wait();
                        userManager.AddClaimAsync(user2, new Claim(JwtClaimTypes.Name, "Claire Underwood")).Wait();
                        userManager.AddClaimAsync(user2, new Claim(JwtClaimTypes.GivenName, "Claire")).Wait();
                        userManager.AddClaimAsync(user2, new Claim(JwtClaimTypes.FamilyName, "Underwood")).Wait();
                        userManager.AddClaimAsync(user1, new Claim(JwtClaimTypes.NickName, "Beahanaaa")).Wait();
                    }
                }
            }
            return host;
        }
    }
}