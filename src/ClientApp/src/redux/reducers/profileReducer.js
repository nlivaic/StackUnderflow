import { LOGIN_SUCCESS, LOGOUT } from "../actions/actionTypes.js";

const profileReducer = (state = { isLoggedIn: false, roles: [] }, action) => {
  switch (action.type) {
    case LOGIN_SUCCESS:
      return action.profile;
    case LOGOUT:
      return { isLoggedIn: false, roles: [] };
    default:
      return state;
  }
};

export default profileReducer;

// Combined state selector.
export const getProfile = (state) => state;
