import {
  InMemoryWebStorage,
  UserManager,
  WebStorageStateStore,
} from "oidc-client";

const config = {
  authority: "https://localhost:6001",
  client_id: "stack_underflow_client",
  redirect_uri: "http://localhost:3000/signin-oidc",
  response_type: "code",
  scope: "openid profile stack_underflow_api email",
  post_logout_redirect_uri: "http://localhost:3000/signout-callback-oidc",
  userStore: new WebStorageStateStore({ store: new InMemoryWebStorage() }),
  automaticSilentRenew: true,
  silent_redirect_uri: "http://localhost:3000/signin-silent-oidc",
};

const userManager = new UserManager(config);

export const signinRedirect = async () => await userManager.signinRedirect();

export const signinRedirectCallback = async () =>
  await userManager.signinRedirectCallback();

export const signinSilent = async () => await userManager.signinSilent();

export const signinSilentCallback = async () =>
  await userManager.signinSilentCallback();

export const signoutRedirect = () => {
  return userManager.signoutRedirect();
};

export const signoutRedirectCallback = () => {
  return userManager.signoutRedirectCallback();
};

export const getUser = async () => await userManager.getUser();

export const getAccessToken = async () => {
  var user = await userManager.getUser();
  if (user) return user.access_token;
  else return "";
};
