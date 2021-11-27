INSERT INTO "Configuration"."Clients" VALUES (1, true, 'stack_underflow_client', 'oidc', false, 'StackUnderflowClient', NULL, NULL, NULL, false, true, false, true, false, false, true, NULL, true, NULL, true, false, 300, NULL, 120, 300, NULL, 2592000, 1296000, 1, false, 1, 0, true, true, false, 'client_', NULL, '2021-11-27 14:10:49.76167', NULL, NULL, NULL, NULL, 300, false);

INSERT INTO "Configuration"."ClientGrantTypes" VALUES (1, 'authorization_code', 1);

INSERT INTO "Configuration"."ClientPostLogoutRedirectUris" VALUES (1, 'http://localhost:3000/signout-callback-oidc', 1);

INSERT INTO "Configuration"."ClientRedirectUris" VALUES (1, 'http://localhost:3000/signin-oidc', 1);
INSERT INTO "Configuration"."ClientRedirectUris" VALUES (2, 'http://localhost:3000/signin-silent-oidc', 1);

INSERT INTO "Configuration"."ClientScopes" VALUES (1, 'email', 1);
INSERT INTO "Configuration"."ClientScopes" VALUES (2, 'stack_underflow_api', 1);
INSERT INTO "Configuration"."ClientScopes" VALUES (3, 'profile', 1);
INSERT INTO "Configuration"."ClientScopes" VALUES (4, 'openid', 1);

INSERT INTO "Configuration"."ClientSecrets" VALUES (1, NULL, 'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=', NULL, 'SharedSecret', '2021-11-27 15:25:11.650641', 1);

INSERT INTO "Configuration"."ApiResources" VALUES (1, true, 'stack_underflow_api', 'Stack Underflow Api', NULL, NULL, true, '2021-11-27 14:10:49.935503', NULL, NULL, false);

INSERT INTO "Configuration"."ApiResourceScopes" VALUES (1, 'stack_underflow_api', 1);

INSERT INTO "Configuration"."ApiResourceSecrets" VALUES (1, NULL, 'LJp04+p0hjGY1MUCg6psn/m2avQUboIwdxKYLNpRgxs=', NULL, 'SharedSecret', '2021-11-27 15:25:11.84266', 1);

INSERT INTO "Configuration"."ApiScopes" VALUES (1, true, 'stack_underflow_api', 'Stack Underflow Api', NULL, false, false, true);

INSERT INTO "Configuration"."IdentityResources" VALUES (1, true, 'openid', 'Your user identifier', NULL, true, false, true, '2021-11-27 14:10:50.003074', NULL, false);
INSERT INTO "Configuration"."IdentityResources" VALUES (2, true, 'profile', 'User profile', 'Your user profile information (first name, last name, etc.)', false, true, true, '2021-11-27 14:10:50.025379', NULL, false);
INSERT INTO "Configuration"."IdentityResources" VALUES (3, true, 'email', 'Email', NULL, false, false, true, '2021-11-27 14:10:50.031433', NULL, false);

INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (1, 'picture', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (2, 'locale', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (3, 'zoneinfo', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (4, 'birthdate', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (5, 'gender', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (6, 'website', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (7, 'profile', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (8, 'family_name', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (9, 'nickname', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (10, 'middle_name', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (11, 'given_name', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (12, 'updated_at', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (13, 'name', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (14, 'sub', 1);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (15, 'preferred_username', 2);
INSERT INTO "Configuration"."IdentityResourceClaims" VALUES (16, 'email', 3);
