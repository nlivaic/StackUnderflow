import { LOGIN_SUCCESS, LOGOUT } from "./actionTypes";
import * as usersApi from "../../api/usersApi.js";
import * as apiStatusActions from "../actions/apiStatusActions.js";
import { getUser } from "../../utils/authService.js";

function loginSuccess(profile) {
  return {
    type: LOGIN_SUCCESS,
    profile: {
      isLoggedIn: true,
      ...profile,
    },
  };
}

export const getCurrentUser = () => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      debugger;
      let data = await usersApi.getUser();
      if (!data) {
        const user = (await getUser()).profile;
        data = await usersApi.createUser({
          email: user.email,
          username: user.nickname,
        });
      }
      dispatch(loginSuccess(data));
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

export const logoutSuccess = () => {
  return {
    type: LOGOUT,
  };
};
