import { LOGIN, LOGOUT } from "./actionTypes";

export const loginSuccess = () => {
  return {
    type: LOGIN,
  };
};

export const logoutSuccess = () => {
  return {
    type: LOGOUT,
  };
};
