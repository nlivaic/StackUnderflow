// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "f368a2f6-3a56-441f-8b06-25272acc5ce7",
                        Username = "Frank",
                        Password = "password",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Frank Underwood"),
                            new Claim(JwtClaimTypes.GivenName, "Frank"),
                            new Claim(JwtClaimTypes.FamilyName, "Underwood"),
                            new Claim(JwtClaimTypes.Email, "liva.spamster@gmail.com"),
                            new Claim(JwtClaimTypes.Role, "user")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "9cca3868-4c09-4104-bab5-14b06f9e61bd",
                        Username = "Claire",
                        Password = "password",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Claire Underwood"),
                            new Claim(JwtClaimTypes.GivenName, "Claire"),
                            new Claim(JwtClaimTypes.FamilyName, "Underwood"),
                            new Claim(JwtClaimTypes.Email, "liva.spamster@gmail.com"),
                            new Claim(JwtClaimTypes.Role, "admin")
                        }
                    }
                };
            }
        }
    }
}